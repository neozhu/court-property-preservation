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
namespace WebApp.Controllers
{
                            [Authorize]
  [RoutePrefix("Attachments")]
  public class AttachmentsController : Controller
  {
    private readonly IAttachmentService attachmentService;
    private readonly IUnitOfWorkAsync unitOfWork;
    private readonly NLog.ILogger logger;
    public AttachmentsController(
          IAttachmentService attachmentService,
          IUnitOfWorkAsync unitOfWork,
          NLog.ILogger logger
          )
    {
      this.attachmentService = attachmentService;
      this.unitOfWork = unitOfWork;
      this.logger = logger;
    }
            [Route("Index", Name = "附件信息", Order = 1)]
    public ActionResult Index() => this.View();

            public async Task<JsonResult> GetAttachments(string caseid) {
      var items =await this.attachmentService.GetAttachments(caseid);
      return Json(items, JsonRequestBehavior.AllowGet);
    }

    [HttpGet]
        public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.attachmentService
                           .Query(new AttachmentQuery().Withfilter(filters))
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {

                                         Id = n.Id,
                                         CaseId = n.CaseId,
                                         Description = n.Description,
                                         DocId = n.DocId,
                                         Type = n.Type,
                                         Path = n.Path,
                                         Ext = n.Ext,
                                         ExpireDate = n.ExpireDate?.ToString("yyyy-MM-dd HH:mm:ss")
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
        [HttpPost]
    public async Task<JsonResult> SaveData(Attachment[] attachments)
    {
      if (attachments == null)
      {
        throw new ArgumentNullException(nameof(attachments));
      }
      if (ModelState.IsValid)
      {
        try
        {
          foreach (var item in attachments)
          {
            this.attachmentService.ApplyChanges(item);
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
        public ActionResult Details(int id)
    {

      var attachment = this.attachmentService.Find(id);
      if (attachment == null)
      {
        return HttpNotFound();
      }
      return View(attachment);
    }
        [HttpGet]
    public async Task<JsonResult> GetItem(int id)
    {
      var attachment = await this.attachmentService.FindAsync(id);
      return Json(attachment, JsonRequestBehavior.AllowGet);
    }
        public ActionResult Create()
    {
      var attachment = new Attachment();
            return View(attachment);
    }
            [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(Attachment attachment)
    {
      if (attachment == null)
      {
        throw new ArgumentNullException(nameof(attachment));
      }
      if (ModelState.IsValid)
      {
        try
        {
          this.attachmentService.Insert(attachment);
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
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
              }
          }

        [HttpGet]
    public async Task<JsonResult> NewItem()
    {
      var attachment = await Task.Run(() =>
      {
        return new Attachment();
      });
      return Json(attachment, JsonRequestBehavior.AllowGet);
    }


        public ActionResult Edit(int id)
    {
      var attachment = this.attachmentService.Find(id);
      if (attachment == null)
      {
        return HttpNotFound();
      }
      return View(attachment);
    }
            [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(Attachment attachment)
    {
      if (attachment == null)
      {
        throw new ArgumentNullException(nameof(attachment));
      }
      if (ModelState.IsValid)
      {
        attachment.TrackingState = TrackingState.Modified;
        try
        {
          this.attachmentService.Update(attachment);

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

                      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
              }
          }
            [HttpGet]
    public async Task<ActionResult> Delete(int id)
    {
      try
      {
        await this.attachmentService.Queryable().Where(x => x.Id == id).DeleteAsync();
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




        [HttpPost]
    public async Task<JsonResult> DeleteChecked(int[] id)
    {
      if (id == null)
      {
        throw new ArgumentNullException(nameof(id));
      }
      try
      {
        this.attachmentService.Delete(id);
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
        [HttpPost]
    public async Task<ActionResult> ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var fileName = "attachments_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
      var stream = await this.attachmentService.ExportExcelAsync(filterRules, sort, order);
      return File(stream, "application/vnd.ms-excel", fileName);
    }
    private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
    private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;

  }
}
