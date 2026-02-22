using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Repository.Pattern.UnitOfWork;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
  public class FileUploadController : Controller
  {

    private readonly ICodeItemService _codeService;
    private readonly IEmployeeService _empService;
    private readonly ILegalCaseService legalCaseService;
    private readonly IUnitOfWorkAsync _unitOfWork;
    private readonly NLog.ILogger logger;
    private ApplicationUserManager userManager
    {
      get => this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

    }
    private ApplicationRoleManager roleManager
    {
      get => this.HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
    }
    public FileUploadController(
       NLog.ILogger logger,
       ILegalCaseService legalCaseService,
            IEmployeeService _empService,
            ICodeItemService _codeService,
            IUnitOfWorkAsync unitOfWork)
    {
                        this._empService = _empService;
      this._unitOfWork = unitOfWork;
      this._codeService = _codeService;
      this.logger = logger;
      this.legalCaseService = legalCaseService;
    }
        [HttpPost]
    public async Task<JsonResult> Upload()
    {
      
      var watch = new Stopwatch();
      var uploadfilename = string.Empty;
      var newfileName = string.Empty;

      try
      {

        watch.Start();
                if (this.Request.Files.Count == 0)
        {
          return this.Json(new { success = false, message = "没有上传文件" }, JsonRequestBehavior.AllowGet);
        }
        var filedata = this.Request.Files[0];
        var model = this.Request.Form["model"];
        uploadfilename = System.IO.Path.GetFileName(filedata.FileName);
        var ext = System.IO.Path.GetExtension(filedata.FileName);
        var folder = this.Server.MapPath("~/UploadFiles");
        newfileName = $"{filedata.FileName.Replace(ext, "")}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{ext}";        if (!Directory.Exists(folder))
        {
          Directory.CreateDirectory(folder);
        }
        var virtualPath = Path.Combine(folder, newfileName);
        filedata.InputStream.Seek(0, SeekOrigin.Begin);
        var datatable = await NPOIHelper.GetDataTableFromExcelAsync(filedata.InputStream,ext);
                if (model == "ApplicationUser")
        {
          await this.ImportUser(datatable);
        }
        if (model == "Employee")
        {
          this._unitOfWork.SetAutoDetectChangesEnabled(false);
          await this._empService.ImportDataTableAsync(datatable);
          await this._unitOfWork.SaveChangesAsync();
          this._unitOfWork.SetAutoDetectChangesEnabled(true);
                  }
        if (model == "CodeItem")
        {
          this._unitOfWork.SetAutoDetectChangesEnabled(false);
          await this._codeService.ImportDataTableAsync(datatable);
          await this._unitOfWork.SaveChangesAsync();

                    this._unitOfWork.SetAutoDetectChangesEnabled(true);
        }

        if (model == "LegalCase")
        {
          await this.legalCaseService.ImportDataTableAsync(datatable,Auth.GetFullName());
          await _unitOfWork.SaveChangesAsync();
        }



        watch.Stop();
                var elapsedTime = watch.ElapsedMilliseconds.ToString();
        filedata.InputStream.Position = 0;
        filedata.SaveAs(virtualPath);
        return this.Json(new { success = true, filename = newfileName, elapsedTime = elapsedTime }, JsonRequestBehavior.AllowGet);
      }
      catch (System.Data.SqlClient.SqlException e)
      {
        this.logger.Error(e, $"文件名:{uploadfilename},{e.GetBaseException().Message}");
        return this.Json(new { success = false, filename = uploadfilename, message = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
      catch (System.Data.Entity.Infrastructure.DbUpdateException e)
      {
        this.logger.Error(e, $"文件名:{uploadfilename},{e.GetBaseException().Message}");
        return this.Json(new { success = false, filename = uploadfilename, message = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
      catch (System.Data.Entity.Validation.DbEntityValidationException e)
      {
        var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage).Distinct());
        this.logger.Error(e, $"文件名:{uploadfilename},{errormessage}");
        return this.Json(new { success = false, filename = uploadfilename, message = errormessage }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        this.logger.Error(e, $"文件名:{uploadfilename},{e.GetBaseException().Message}");
        return this.Json(new { success = false, filename = uploadfilename, message = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }


    [FileDownload]
    public async  Task<FileContentResult> Download(string file = "")
    {
      if (string.IsNullOrEmpty(file))
      {
        throw new ArgumentNullException($"input file name is empty!");
      }
      byte[] fileContent = null;
      var fileName = "";
      var mimeType = "";
      this.HttpContext.Response.AppendCookie(new HttpCookie("fileDownload", "true") { Path = "/" });

      var downloadFile = new FileInfo(this.Server.MapPath(file));
      if (downloadFile.Exists)
      {
        fileName = downloadFile.Name;
        mimeType = this.GetMimeType(downloadFile.Extension);
        fileContent = new byte[Convert.ToInt32(downloadFile.Length)];
        using (var fs = downloadFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
        {
          await fs.ReadAsync(fileContent, 0, Convert.ToInt32(downloadFile.Length));
        }
        return this.File(fileContent, mimeType, fileName);
      }
      else
      {
        throw new FileNotFoundException($"not found file {file}!");
      }
    }
    [HttpDelete]
    public async Task<JsonResult> Revert() {
      var req = Request.InputStream;
      var filename =await new StreamReader(req).ReadToEndAsync();
      if (!string.IsNullOrEmpty(filename))
      {
        var folder = this.Server.MapPath("~/UploadFiles");
        var path = Path.Combine(folder, filename);
        if (System.IO.File.Exists(path))
        {
          System.IO.File.Delete(path);
        }
      }
      return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
    }
    [HttpPost]
    public JsonResult Remove(string filename = "")
    {
      if (!string.IsNullOrEmpty(filename))
      {
        var folder = this.Server.MapPath("~/UploadFiles");
        var path = Path.Combine(folder, filename);
        if (System.IO.File.Exists(path))
        {
          System.IO.File.Delete(path);
        }
      }
      return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
    }

    private string GetMimeType(string fileExtensionStr)
    {
      var ContentTypeStr = "application/octet-stream";
      var fileExtension = fileExtensionStr.ToLower();
      switch (fileExtension)
      {
        case ".mp3":
          ContentTypeStr = "audio/mpeg3";
          break;
        case ".mpeg":
          ContentTypeStr = "video/mpeg";
          break;
        case ".jpg":
          ContentTypeStr = "image/jpeg";
          break;
        case ".bmp":
          ContentTypeStr = "image/bmp";
          break;
        case ".gif":
          ContentTypeStr = "image/gif";
          break;
        case ".doc":
          ContentTypeStr = "application/msword";
          break;
        case ".css":
          ContentTypeStr = "text/css";
          break;
        case ".html":
          ContentTypeStr = "text/html";
          break;
        case ".htm":
          ContentTypeStr = "text/html";
          break;
        case ".swf":
          ContentTypeStr = "application/x-shockwave-flash";
          break;
        case ".exe":
          ContentTypeStr = "application/octet-stream";
          break;
        case ".inf":
          ContentTypeStr = "application/x-texinfo";
          break;
        case ".xls":
        case ".xlsx":
          ContentTypeStr = "application/vnd.ms-excel";
          break;
        default:
          ContentTypeStr = "application/octet-stream";
          break;
      }
      return ContentTypeStr;
    }
    private async Task ImportUser(DataTable datatable)
    {
      foreach (DataRow dr in datatable.Rows)
      {
        var userName = dr["账号"].ToString();
        var email = dr["电子邮件"].ToString();
        var password = dr["密码"].ToString();
        var fullName = dr["姓名"].ToString();
        var role = dr["角色"].ToString();
        var user = new ApplicationUser
        {
          UserName = userName,
          Email = email,
          FullName = fullName,
          Gender = 1,
          TenantId = Auth.GetTenantId(),
          CompanyCode = "0",
          CompanyName = "",
          PhoneNumber = null,
          AccountType = 1,
          AvatarsX50 = "male",
          AvatarsX120 = "male_big"
        };
        var result = await this.userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
          await this.userManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("http:          await this.userManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("FullName", string.IsNullOrEmpty(user.FullName) ? "" : user.FullName));
          await this.userManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("CompanyName", string.IsNullOrEmpty(user.CompanyName) ? "" : user.CompanyName));
          await this.userManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email));
          await this.userManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("AvatarsX50", user.AvatarsX50));
          await this.userManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("AvatarsX120", user.AvatarsX120));
          await this.userManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("PhoneNumber", string.IsNullOrEmpty(user.PhoneNumber) ? "" : user.PhoneNumber));
          if (!string.IsNullOrEmpty(role))
          {
            var rolearray = role.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var r in rolearray)
            {
              var any = await this.roleManager.FindByNameAsync(r);
              if (any != null)
              {
                await this.userManager.AddToRoleAsync(user.Id, r);
              }
              else
              {
                await this.roleManager.CreateAsync(new ApplicationRole() { Name = r });
                await this.userManager.AddToRoleAsync(user.Id, role);
              }
            }
          }

        }
      }
    }
  }
}
