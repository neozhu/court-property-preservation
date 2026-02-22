using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Infrastructure;
using Z.EntityFramework.Plus;
using TrackableEntities;
using WebApp.Models;
using WebApp.Services;
using WebApp.Repositories;
using WebApp.Models.ViewModel;
using System.IO;
using ImApiDotNet;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using static NPOI.HSSF.Util.HSSFColor;

namespace WebApp.Controllers
{
  /// <summary>
  /// File: LegalCasesController.cs
  /// Purpose:案件管理/立案登记
  /// Created Date: 3/4/2020 9:32:04 AM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// TODO: Registers the type mappings with the Unity container(Mvc.UnityConfig.cs)
  /// <![CDATA[
  ///    container.RegisterType<IRepositoryAsync<LegalCase>, Repository<LegalCase>>();
  ///    container.RegisterType<ILegalCaseService, LegalCaseService>();
  /// ]]>
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
  [Authorize]
  [RoutePrefix("LegalCases")]
  public class LegalCasesController : Controller
  {
    private readonly ILegalCaseService legalCaseService;
    private readonly IUnitOfWorkAsync unitOfWork;
    private readonly NLog.ILogger logger;
    public LegalCasesController(
          ILegalCaseService legalCaseService,
          IUnitOfWorkAsync unitOfWork,
          NLog.ILogger logger
          )
    {
      this.legalCaseService = legalCaseService;
      this.unitOfWork = unitOfWork;
      this.logger = logger;
    }
    [Route("Search", Name = "案件查询", Order = 1)]
    public ActionResult Search() => this.View();
    //GET: LegalCases/Index
    //[OutputCache(Duration = 60, VaryByParam = "none")]
    [Route("Index", Name = "归档处理", Order = 1)]
    public ActionResult Index() => this.View();

    //测试短信
    public JsonResult TestSMS()
    {
      var apiclient = new APIClient();
      var IP = System.Configuration.ConfigurationManager.AppSettings.Get("IP");
      var Username = System.Configuration.ConfigurationManager.AppSettings.Get("Username");
      var Password = System.Configuration.ConfigurationManager.AppSettings.Get("Password");
      var Code = System.Configuration.ConfigurationManager.AppSettings.Get("Code");
      var Db = System.Configuration.ConfigurationManager.AppSettings.Get("Db");
      var Phonenumber = System.Configuration.ConfigurationManager.AppSettings.Get("Phonenumber");
      var Tmp1 = System.Configuration.ConfigurationManager.AppSettings.Get("Tmp1");
      var SmId= System.Configuration.ConfigurationManager.AppSettings.Get("SmId");
      try
      {
        var con = apiclient.init(IP, Username, Password, Code, Db);
        var msg = string.Format(Tmp1, DateTime.Now);
        var sm = apiclient.sendSM(Phonenumber,msg,Convert.ToInt32(SmId));
        return Json(new { success = true}, JsonRequestBehavior.AllowGet);
      }
      catch(Exception e)
      {
        this.logger.Error(e);
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }

    //验证案号是否存在
    public async Task<JsonResult> ValidateCaseId(string caseid) {
      var exist =await this.legalCaseService.ValidateCaseId(caseid);
      return Json(exist, JsonRequestBehavior.AllowGet);
    }
    //首页获取案件信息
    public async Task<JsonResult> GetCalendarData() {
      var nodes = new string[] {"立案","分案","办案","归档" };
      var data = (await this.legalCaseService.Queryable()
          .Where(x => nodes.Contains(x.Node) && x.Status!="完成归档")
          .Select(n => new
          {
           n.Id,
           n.CaseId,
            n.ToDoDate,
            n.Node,
            n.Status
          }).ToListAsync())
          .Select(x=>new {
            id=x.Id,
            title=x.CaseId,
            start=x.ToDoDate.ToUniversalTime(),
            description=$"案件节点:{x.Node},状态:{x.Status}",
            className=new string[] { "event", this.styletext(x.Node) }
          });
      return Json(data, JsonRequestBehavior.AllowGet);
    }

    private string styletext(string node) {
      if (node == "立案")
      {
        return "bg-color-blue";
      }
      else if (node == "办案")
      {
        return "bg-color-greenLight";
      }
      else if (node == "分案")
      {
        return "bg-color-red";
      }
      else
      {
        return "bg-color-darken";
      }

     }


    [Route("Step1", Name = "立案登记", Order = 1)]
    public ActionResult Step1() => this.View();
    public async Task<JsonResult> GetStep1Data(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.legalCaseService
                           .Query(new LegalCaseQuery().Step1Withfilter(filters))
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {

                                         Id = n.Id,
                                         CaseId = n.CaseId,
                                         Project = n.Project,
                                         Category = n.Category,
                                         Status = n.Status,
                                         Node = n.Node,
                                         Expires = n.Expires,
                                         Cause = n.Cause,
                                         Feature = n.Feature,
                                         BasedOn = n.BasedOn,
                                         Subject = n.Subject,
                                         FromDepartment = n.FromDepartment,
                                         ToDepartment = n.ToDepartment,
                                         ToUser = n.ToUser,
                                         Recorder = n.Recorder,
                                         Examiner = n.Examiner,
                                         OriginCaseId = n.OriginCaseId,
                                         ReceiveDate = n.ReceiveDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         RegisterDate = n.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         PreUnderlyingAsset = n.PreUnderlyingAsset,
                                         AllocateDate = n.AllocateDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         PreCloseDate = n.PreCloseDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         ClosedDate = n.ClosedDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         CloseType = n.CloseType,
                                         UnderlyingAsset = n.UnderlyingAsset,
                                         Court = n.Court,
                                         Org = n.Org,
                                         Accuser = n.Accuser,
                                         AccuserAddress = n.AccuserAddress,
                                         Defendant = n.Defendant,
                                         DefendantAddress = n.DefendantAddress,
                                         Receivable = n.Receivable,
                                         Received = n.Received,
                                         Opinion1 = n.Opinion1,
                                         Opinion2 = n.Opinion2,
                                         Opinion3 = n.Opinion3,
                                         Comment = n.Comment,
                                         Proposer = n.Proposer,
                                         n.SerialNumber,
                                         n.ToManager,
                                         n.Manager,
                                         FilingDate = n.FilingDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         ToDoDate = n.ToDoDate.ToString("yyyy-MM-dd HH:mm:ss")
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    //移交案件
    public async Task<JsonResult> ToNext(int[] id) {
      try
      {
        await this.legalCaseService.ToNext(id,Auth.GetFullName());
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }


    }

    //分案阶段
    [Route("Step2", Name = "分案处理", Order = 2)]
    public ActionResult Step2() => this.View();

    public async Task<JsonResult> GetStep2Data(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.legalCaseService
                           .Query(new LegalCaseQuery().Step2Withfilter(filters))
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {

                                         Id = n.Id,
                                         CaseId = n.CaseId,
                                         Project = n.Project,
                                         Category = n.Category,
                                         Status = n.Status,
                                         Node = n.Node,
                                         Expires = n.Expires,
                                         Cause = n.Cause,
                                         Feature = n.Feature,
                                         BasedOn = n.BasedOn,
                                         Subject = n.Subject,
                                         FromDepartment = n.FromDepartment,
                                         ToDepartment = n.ToDepartment,
                                         ToUser = n.ToUser,
                                         Recorder = n.Recorder,
                                         Examiner = n.Examiner,
                                         OriginCaseId = n.OriginCaseId,
                                         ReceiveDate = n.ReceiveDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         RegisterDate = n.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         PreUnderlyingAsset = n.PreUnderlyingAsset,
                                         AllocateDate = n.AllocateDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         PreCloseDate = n.PreCloseDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         ClosedDate = n.ClosedDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         CloseType = n.CloseType,
                                         UnderlyingAsset = n.UnderlyingAsset,
                                         Court = n.Court,
                                         Org = n.Org,
                                         Accuser = n.Accuser,
                                         AccuserAddress = n.AccuserAddress,
                                         Defendant = n.Defendant,
                                         DefendantAddress = n.DefendantAddress,
                                         Receivable = n.Receivable,
                                         Received = n.Received,
                                         Opinion1 = n.Opinion1,
                                         Opinion2 = n.Opinion2,
                                         Opinion3 = n.Opinion3,
                                         Comment = n.Comment,
                                         Proposer = n.Proposer,
                                         n.SerialNumber,
                                         n.ToManager,
                                         n.Manager,
                                         FilingDate = n.FilingDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         ToDoDate = n.ToDoDate.ToString("yyyy-MM-dd HH:mm:ss")
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }

    //完成状态
    public async Task<JsonResult> DoComplete(int id) {
      try
      {
        await this.legalCaseService.DoComplete(id,Auth.GetFullName(this.User.Identity.Name));
        await this.unitOfWork.SaveChangesAsync();

        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    public async Task<JsonResult> ExportWord(int id) {
      try
      { 
      var templatefile = this.Server.MapPath("/ExcelTemplate/立案审批表模板.docx");
      var destpath = this.Server.MapPath("~/UploadFiles");
      var filename =await this.legalCaseService.ExportWord(id, templatefile, destpath);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success=true, filename=filename }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    
    public async Task<JsonResult> BackStep1(int id) {
      try
      {
        await this.legalCaseService.BackStep1(id, Auth.GetFullName(this.User.Identity.Name));
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    public async Task<JsonResult> AssigningTask(LegalCase legalcase) {
      try
      {
        await this.legalCaseService.AssigningTask(legalcase,Auth.GetFullName());
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    public async Task<JsonResult> ToDoAssigning(AssigningViewModel viewmodel)
    {
      try
      {
        await this.legalCaseService.Assigning(viewmodel,Auth.GetFullName());
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //变更承办人
    public async Task<JsonResult> ChangeAssigning(AssigningViewModel viewmodel)
    {
      try
      {
        await this.legalCaseService.ChangeAssigning(viewmodel, Auth.GetFullName());
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    public async Task<JsonResult> GetCaseId(string year, string serial, string category) {
      var result = await this.legalCaseService.GetCaseId(year, serial, category);
      return Json(new { caseid = result.Item1,seqno=result.Item2, expires=result.Item3 }, JsonRequestBehavior.AllowGet);

      }
    public async Task<JsonResult> GetCaseExpires(string year, string serial, string category)
    {
      var result = await this.legalCaseService.GetCaseExpires(year, serial, category);
      return Json(new {  expires = result.Item1,seqno=result.Item2 }, JsonRequestBehavior.AllowGet);

    }
    //办案阶段
    [Route("Step3", Name = "办案处理", Order = 3)]
    public ActionResult Step3() => this.View();
    public async Task<JsonResult> GetStep3MeData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.legalCaseService
                           .Query(new LegalCaseQuery().Step3WithMefilter(Auth.GetFullName(this.User.Identity.Name),this.User.IsInRole("部门领导"),filters))
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {

                                         Id = n.Id,
                                         CaseId = n.CaseId,
                                         Project = n.Project,
                                         Category = n.Category,
                                         Status = n.Status,
                                         Node = n.Node,
                                         Expires = n.Expires,
                                         Cause = n.Cause,
                                         Feature = n.Feature,
                                         BasedOn = n.BasedOn,
                                         Subject = n.Subject,
                                         FromDepartment = n.FromDepartment,
                                         ToDepartment = n.ToDepartment,
                                         ToUser = n.ToUser,
                                         Recorder = n.Recorder,
                                         Examiner = n.Examiner,
                                         OriginCaseId = n.OriginCaseId,
                                         ReceiveDate = n.ReceiveDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         RegisterDate = n.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         PreUnderlyingAsset = n.PreUnderlyingAsset,
                                         AllocateDate = n.AllocateDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         PreCloseDate = n.PreCloseDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         ClosedDate = n.ClosedDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         CloseType = n.CloseType,
                                         UnderlyingAsset = n.UnderlyingAsset,
                                         Court = n.Court,
                                         Org = n.Org,
                                         Accuser = n.Accuser,
                                         AccuserAddress = n.AccuserAddress,
                                         Defendant = n.Defendant,
                                         DefendantAddress = n.DefendantAddress,
                                         Receivable = n.Receivable,
                                         Received = n.Received,
                                         Opinion1 = n.Opinion1,
                                         Opinion2 = n.Opinion2,
                                         Opinion3 = n.Opinion3,
                                         Comment = n.Comment,
                                         Proposer = n.Proposer,
                                         n.SerialNumber,
                                         n.ToManager,
                                         n.Manager,
                                         FilingDate = n.FilingDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         ToDoDate = n.ToDoDate.ToString("yyyy-MM-dd HH:mm:ss")
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    public async Task<JsonResult> GetStep3Data(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.legalCaseService
                           .Query(new LegalCaseQuery().Step3Withfilter(filters))
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {

                                         Id = n.Id,
                                         CaseId = n.CaseId,
                                         Project = n.Project,
                                         Category = n.Category,
                                         Status = n.Status,
                                         Node = n.Node,
                                         Expires = n.Expires,
                                         Cause = n.Cause,
                                         Feature = n.Feature,
                                         BasedOn = n.BasedOn,
                                         Subject = n.Subject,
                                         FromDepartment = n.FromDepartment,
                                         ToDepartment = n.ToDepartment,
                                         ToUser = n.ToUser,
                                         Recorder = n.Recorder,
                                         Examiner = n.Examiner,
                                         OriginCaseId = n.OriginCaseId,
                                         ReceiveDate = n.ReceiveDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         RegisterDate = n.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         PreUnderlyingAsset = n.PreUnderlyingAsset,
                                         AllocateDate = n.AllocateDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         PreCloseDate = n.PreCloseDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         ClosedDate = n.ClosedDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         CloseType = n.CloseType,
                                         UnderlyingAsset = n.UnderlyingAsset,
                                         Court = n.Court,
                                         Org = n.Org,
                                         Accuser = n.Accuser,
                                         AccuserAddress = n.AccuserAddress,
                                         Defendant = n.Defendant,
                                         DefendantAddress = n.DefendantAddress,
                                         Receivable = n.Receivable,
                                         Received = n.Received,
                                         Opinion1 = n.Opinion1,
                                         Opinion2 = n.Opinion2,
                                         Opinion3 = n.Opinion3,
                                         Comment = n.Comment,
                                         Proposer = n.Proposer,
                                         n.SerialNumber,
                                         n.ToManager,
                                         n.Manager,
                                         FilingDate = n.FilingDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         ToDoDate = n.ToDoDate.ToString("yyyy-MM-dd HH:mm:ss")
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    //上传附件
    [HttpPost]
    public async Task<ActionResult> UploadFile()
    {
      if (Request.Files.Count > 0)
      {
        for (var i = 0; i < this.Request.Files.Count; i++)
        {
          var caseid =Convert.ToInt32( Request.Form["CaseId"]);
          var file = Request.Files[i];
          if (file != null && file.ContentLength > 0)
          {
            var filename = file.FileName;
            var contenttype = file.ContentType;
            var size = file.ContentLength;
            var ext = Path.GetExtension(filename);

            var folder = this.Server.MapPath($"~/UploadFiles/{caseid}");
            if (!Directory.Exists(folder))
            {
              Directory.CreateDirectory(folder);
            }
            var virtualPath = Path.Combine(folder, filename);
            // 文件系统不能使用虚拟路径
            //string path = this.Server.MapPath(virtualPath);
            file.InputStream.Position = 0;
            file.SaveAs(virtualPath);
            await this.legalCaseService.UploadFile(caseid, filename, ext, $"/UploadFiles/{caseid}/{filename}");
            await this.unitOfWork.SaveChangesAsync();
            return Content($"{filename}|{caseid}");
          }
        }
      }
      return Content(null);
    }
    //删除附件
    [HttpDelete]
    public async Task<JsonResult> Revert()
    {
      var req = Request.InputStream;
      var array = new StreamReader(req).ReadToEnd().Split(new char[] { '|' });
      if (!string.IsNullOrEmpty(array[0]))
      {
        var folder = this.Server.MapPath($"~/UploadFiles/{array[1]}");
        var path = Path.Combine(folder, array[0]);
        if (System.IO.File.Exists(path))
        {
          System.IO.File.Delete(path);
        }
        await this.legalCaseService.DeleteFile(Convert.ToInt32(array[1]), array[0]);
        await this.unitOfWork.SaveChangesAsync();
      }
      return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
    }
    //执行完成批量
    public async Task<JsonResult> ExecutionTaskALL(int[] id,string manager="")
    {

      try
      {
        var items =await this.legalCaseService.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
        foreach (var item in items)
        {
          if (string.IsNullOrEmpty(manager))
          {
            item.ToManager = false;
            item.Manager = null;
            await this.legalCaseService.ExecutionTask(item, Auth.GetFullName());
          }
          else
          {
            item.ToManager = true;
            item.Manager = manager;
            await this.legalCaseService.ExecutionTask(item, Auth.GetFullName());
          }
        }
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //Get :LegalCases/GetData
    //For Index View datagrid datasource url
    //执行完成
    public async Task<JsonResult> ExecutionTask(LegalCase legalcase) {

      try
      {
        await this.legalCaseService.ExecutionTask(legalcase,Auth.GetFullName());
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //批量审批完成
    public async Task<JsonResult> ManagerTaskALL(int[] id)
    {

      try
      {
        var items = await this.legalCaseService.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
        foreach (var item in items)
        {
          await this.legalCaseService.ManagerTask(item, Auth.GetFullName());
        }
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //审批完成
    public async Task<JsonResult> ManagerTask(LegalCase legalcase)
    {

      try
      {
        await this.legalCaseService.ManagerTask(legalcase,Auth.GetFullName());
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //批量结案
    public async Task<JsonResult> CloseTaskALL(int[] id)
    {

      try
      {
        var items = await this.legalCaseService.Queryable().Where(x => id.Contains(x.Id)).ToListAsync();
        foreach (var item in items)
        {
          await this.legalCaseService.CloseTask(item, Auth.GetFullName());
        }
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //结案完成
    public async Task<JsonResult> CloseTask(LegalCase legalcase)
    {

      try
      {
        await this.legalCaseService.CloseTask(legalcase, Auth.GetFullName());
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //退回选中的记录
    [HttpPost]
    public async Task<JsonResult> GoBackAll(int[] id) {
      try
      {
        foreach (var key in id)
        {
          await this.legalCaseService.GoBack(key, Auth.GetFullName(this.User.Identity.Name));
        }
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //退回
    public async Task<JsonResult> GoBack(int id) {

      try
      {
        await this.legalCaseService.GoBack(id,Auth.GetFullName(this.User.Identity.Name));
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }

    //调取阶段
    [Route("Step6", Name = "案件调取", Order = 3)]
    public ActionResult Step6() => this.View();

    public async Task<JsonResult> GetStep6Data(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.legalCaseService
                           .Query(new LegalCaseQuery().Step6Withfilter(filters,Auth.GetFullName()))
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {

                                         Id = n.Id,
                                         CaseId = n.CaseId,
                                         Project = n.Project,
                                         Category = n.Category,
                                         Status = n.Status,
                                         Node = n.Node,
                                         Expires = n.Expires,
                                         Cause = n.Cause,
                                         Feature = n.Feature,
                                         BasedOn = n.BasedOn,
                                         Subject = n.Subject,
                                         FromDepartment = n.FromDepartment,
                                         ToDepartment = n.ToDepartment,
                                         ToUser = n.ToUser,
                                         Recorder = n.Recorder,
                                         Examiner = n.Examiner,
                                         OriginCaseId = n.OriginCaseId,
                                         ReceiveDate = n.ReceiveDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         RegisterDate = n.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         PreUnderlyingAsset = n.PreUnderlyingAsset,
                                         AllocateDate = n.AllocateDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         PreCloseDate = n.PreCloseDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         ClosedDate = n.ClosedDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         CloseType = n.CloseType,
                                         UnderlyingAsset = n.UnderlyingAsset,
                                         Court = n.Court,
                                         Org = n.Org,
                                         Accuser = n.Accuser,
                                         AccuserAddress = n.AccuserAddress,
                                         Defendant = n.Defendant,
                                         DefendantAddress = n.DefendantAddress,
                                         Receivable = n.Receivable,
                                         Received = n.Received,
                                         Opinion1 = n.Opinion1,
                                         Opinion2 = n.Opinion2,
                                         Opinion3 = n.Opinion3,
                                         Comment = n.Comment,
                                         Proposer = n.Proposer,
                                         n.SerialNumber,
                                         n.ToManager,
                                         n.Manager,
                                         FilingDate = n.FilingDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         ToDoDate = n.ToDoDate.ToString("yyyy-MM-dd HH:mm:ss")
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
 

    //完成调取
    public async Task<JsonResult> ExtractionAll(int[] ids)
    {
      try
      {
        foreach (var id in ids)
        {
          await this.legalCaseService.ExtractionTask(id, Auth.GetFullName(this.User.Identity.Name));
        }
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    public async Task<JsonResult> ExtractionTask(LegalCase legalcase)
    {
      try
      {
        await this.legalCaseService.ExtractionTask(legalcase.Id, Auth.GetFullName(this.User.Identity.Name));
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //归档阶段
    [Route("Step7", Name = "案件归档", Order = 3)]
    public ActionResult Step7() => this.View();

    public async Task<JsonResult> GetStep7Data(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.legalCaseService
                           .Query(new LegalCaseQuery().Step7Withfilter(filters, this.User))
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {

                                         Id = n.Id,
                                         CaseId = n.CaseId,
                                         Project = n.Project,
                                         Category = n.Category,
                                         Status = n.Status,
                                         Node = n.Node,
                                         Expires = n.Expires,
                                         Cause = n.Cause,
                                         Feature = n.Feature,
                                         BasedOn = n.BasedOn,
                                         Subject = n.Subject,
                                         FromDepartment = n.FromDepartment,
                                         ToDepartment = n.ToDepartment,
                                         ToUser = n.ToUser,
                                         Recorder = n.Recorder,
                                         Examiner = n.Examiner,
                                         OriginCaseId = n.OriginCaseId,
                                         ReceiveDate = n.ReceiveDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         RegisterDate = n.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         PreUnderlyingAsset = n.PreUnderlyingAsset,
                                         AllocateDate = n.AllocateDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         PreCloseDate = n.PreCloseDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         ClosedDate = n.ClosedDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         CloseType = n.CloseType,
                                         UnderlyingAsset = n.UnderlyingAsset,
                                         Court = n.Court,
                                         Org = n.Org,
                                         Accuser = n.Accuser,
                                         AccuserAddress = n.AccuserAddress,
                                         Defendant = n.Defendant,
                                         DefendantAddress = n.DefendantAddress,
                                         Receivable = n.Receivable,
                                         Received = n.Received,
                                         Opinion1 = n.Opinion1,
                                         Opinion2 = n.Opinion2,
                                         Opinion3 = n.Opinion3,
                                         Comment = n.Comment,
                                         Proposer = n.Proposer,
                                         n.SerialNumber,
                                         n.ToManager,
                                         n.Manager,
                                         FilingDate = n.FilingDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         ToDoDate = n.ToDoDate.ToString("yyyy-MM-dd HH:mm:ss")
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    //完成归档
    public async Task<JsonResult> ArchiveAll(int[] ids)
    {
      try
      {
        foreach (var id in ids)
        {
          await this.legalCaseService.ArchiveTask(id, Auth.GetFullName(this.User.Identity.Name));
        }
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    public async Task<JsonResult> ArchiveTask(int id)
    {
      try
      {
        await this.legalCaseService.ArchiveTask(id, Auth.GetFullName(this.User.Identity.Name));
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    [HttpGet]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
    public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.legalCaseService
                           .Query(new LegalCaseQuery().Withfilter(filters))
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {

                                         Id = n.Id,
                                         CaseId = n.CaseId,
                                         Project = n.Project,
                                         Category = n.Category,
                                         Status = n.Status,
                                         Node = n.Node,
                                         Expires = n.Expires,
                                         Cause = n.Cause,
                                         Feature = n.Feature,
                                         BasedOn = n.BasedOn,
                                         Subject = n.Subject,
                                         FromDepartment = n.FromDepartment,
                                         ToDepartment = n.ToDepartment,
                                         ToUser = n.ToUser,
                                         Recorder = n.Recorder,
                                         Examiner = n.Examiner,
                                         OriginCaseId = n.OriginCaseId,
                                         ReceiveDate = n.ReceiveDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         RegisterDate = n.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         PreUnderlyingAsset = n.PreUnderlyingAsset,
                                         AllocateDate = n.AllocateDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         PreCloseDate = n.PreCloseDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         ClosedDate = n.ClosedDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                                         CloseType = n.CloseType,
                                         UnderlyingAsset = n.UnderlyingAsset,
                                         Court = n.Court,
                                         Org = n.Org,
                                         Accuser = n.Accuser,
                                         AccuserAddress = n.AccuserAddress,
                                         Defendant = n.Defendant,
                                         DefendantAddress = n.DefendantAddress,
                                         Receivable = n.Receivable,
                                         Received = n.Received,
                                         Opinion1 = n.Opinion1,
                                         Opinion2 = n.Opinion2,
                                         Opinion3 = n.Opinion3,
                                         Comment = n.Comment,
                                         Proposer = n.Proposer,
                                         FilingDate = n.FilingDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                         n.SerialNumber,
                                         n.ToManager,
                                         n.Manager,
                                         ToDoDate = n.ToDoDate.ToString("yyyy-MM-dd HH:mm:ss")
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
    //easyui datagrid post acceptChanges 
    [HttpPost]
    public async Task<JsonResult> SaveData(LegalCase[] legalcases)
    {
      if (legalcases == null)
      {
        throw new ArgumentNullException(nameof(legalcases));
      }
      if (ModelState.IsValid)
      {
        try
        {
          foreach (var item in legalcases)
          {
            this.legalCaseService.ApplyChanges(item);
          }
          var result = await this.unitOfWork.SaveChangesAsync();
          return Json(new { success = true, result }, JsonRequestBehavior.AllowGet);
        }
        catch (System.Data.Entity.Validation.DbEntityValidationException e)
        {
          var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
          return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
        }
      }
      else
      {
        var modelStateErrors = string.Join(",", ModelState.Keys.SelectMany(key => ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
      }

    }
    //GET: LegalCases/Details/:id
    public ActionResult Details(int id)
    {

      var legalCase = this.legalCaseService.Find(id);
      if (legalCase == null)
      {
        return HttpNotFound();
      }
      return View(legalCase);
    }
    //GET: LegalCases/GetItem/:id
    [HttpGet]
    public async Task<JsonResult> GetItem(int id)
    {
      var legalCase = await this.legalCaseService.FindAsync(id);
      return Json(legalCase, JsonRequestBehavior.AllowGet);
    }
    //GET: LegalCases/Create
    public ActionResult Create()
    {
      var legalCase = new LegalCase();
      //set default value
      return View(legalCase);
    }
    //POST: LegalCases/Create
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(LegalCase legalCase)
    {
      if (legalCase == null)
      {
        throw new ArgumentNullException(nameof(legalCase));
      }
      if (ModelState.IsValid)
      {
        try
        {
          this.legalCaseService.Insert(legalCase);
          var result = await this.unitOfWork.SaveChangesAsync();
          return Json(new { success = true, result }, JsonRequestBehavior.AllowGet);
        }
        catch (System.Data.Entity.Validation.DbEntityValidationException e)
        {
          var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
          return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
        }
        //DisplaySuccessMessage("Has update a legalCase record");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(legalCase);
    }
    [HttpPost]
    public async Task<ActionResult> Register(LegalCase legalCase)
    {
      if (legalCase == null)
      {
        throw new ArgumentNullException(nameof(legalCase));
      }
      if (ModelState.IsValid)
      {
        try
        {
          await this.legalCaseService.Register(legalCase);
          var result = await this.unitOfWork.SaveChangesAsync();
          return Json(new { success = true, result }, JsonRequestBehavior.AllowGet);
        }
        catch (System.Data.Entity.Validation.DbEntityValidationException e)
        {
          var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
          return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
        }
        //DisplaySuccessMessage("Has update a legalCase record");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(legalCase);
    }

    //新增对象初始化
    [HttpGet]
    public async Task<JsonResult> NewItem()
    {
      var legalCase = await this.legalCaseService.InitItemAsync(Auth.CurrentApplicationUser.FullName);
      return Json(legalCase, JsonRequestBehavior.AllowGet);
    }


    //GET: LegalCases/Edit/:id
    public ActionResult Edit(int id)
    {
      var legalCase = this.legalCaseService.Find(id);
      if (legalCase == null)
      {
        return HttpNotFound();
      }
      return View(legalCase);
    }
    //POST: LegalCases/Edit/:id
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(LegalCase legalCase)
    {
      if (legalCase == null)
      {
        throw new ArgumentNullException(nameof(legalCase));
      }
      if (ModelState.IsValid)
      {
        legalCase.TrackingState = TrackingState.Modified;
        try
        {
          this.legalCaseService.Update(legalCase);

          var result = await this.unitOfWork.SaveChangesAsync();
          return Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
        }
        catch (System.Data.Entity.Validation.DbEntityValidationException e)
        {
          var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
          return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
        }

        //DisplaySuccessMessage("Has update a LegalCase record");
        //return RedirectToAction("Index");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(legalCase);
    }
    //删除当前记录
    //GET: LegalCases/Delete/:id
    [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
      try
      {
        await this.legalCaseService.Queryable().Where(x => x.Id == id).DeleteAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (System.Data.Entity.Validation.DbEntityValidationException e)
      {
        var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
        return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }




    //删除选中的记录
    [HttpPost]
    public async Task<JsonResult> DeleteChecked(int[] id)
    {
      if (id == null)
      {
        throw new ArgumentNullException(nameof(id));
      }
      try
      {
        this.legalCaseService.Delete(id);
        await this.unitOfWork.SaveChangesAsync();
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (System.Data.Entity.Validation.DbEntityValidationException e)
      {
        var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
        return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
    //导出Excel
    [HttpPost]
    public async Task<ActionResult> ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var fileName = "legalcases_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
      var stream = await this.legalCaseService.ExportExcelAsync(filterRules, sort, order);
      return File(stream, "application/vnd.ms-excel", fileName);
    }
    private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
    private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;

  }
}
