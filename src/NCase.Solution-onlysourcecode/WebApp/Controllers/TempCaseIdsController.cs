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
  [RoutePrefix("TempCaseIds")]
  public class TempCaseIdsController : Controller
  {
    private readonly ITempCaseIdService tempCaseIdService;
    private readonly IUnitOfWorkAsync unitOfWork;
    private readonly NLog.ILogger logger;
    public TempCaseIdsController(
          ITempCaseIdService tempCaseIdService,
          IUnitOfWorkAsync unitOfWork,
          NLog.ILogger logger
          )
    {
      this.tempCaseIdService = tempCaseIdService;
      this.unitOfWork = unitOfWork;
      this.logger = logger;
    }
            [Route("Index", Name = "已删案件号", Order = 1)]
    public ActionResult Index() => this.View();

        
    [HttpGet]
        public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.tempCaseIdService
                           .Query(new TempCaseIdQuery().Withfilter(filters))
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {

                                         Id = n.Id,
                                         CaseId = n.CaseId,
                                         Category = n.Category,
                                         SerialNumber = n.SerialNumber,
                                         Flag = n.Flag,
                                         Year = n.Year,
                                         Expires = n.Expires
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
        [HttpPost]
    public async Task<JsonResult> SaveData(TempCaseId[] tempcaseids)
    {
      if (tempcaseids == null)
      {
        throw new ArgumentNullException(nameof(tempcaseids));
      }
      if (ModelState.IsValid)
      {
        try
        {
          foreach (var item in tempcaseids)
          {
            this.tempCaseIdService.ApplyChanges(item);
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

      var tempCaseId = this.tempCaseIdService.Find(id);
      if (tempCaseId == null)
      {
        return HttpNotFound();
      }
      return View(tempCaseId);
    }
        [HttpGet]
    public async Task<JsonResult> GetItem(int id)
    {
      var tempCaseId = await this.tempCaseIdService.FindAsync(id);
      return Json(tempCaseId, JsonRequestBehavior.AllowGet);
    }
        public ActionResult Create()
    {
      var tempCaseId = new TempCaseId();
            return View(tempCaseId);
    }
            [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(TempCaseId tempCaseId)
    {
      if (tempCaseId == null)
      {
        throw new ArgumentNullException(nameof(tempCaseId));
      }
      if (ModelState.IsValid)
      {
        try
        {
          this.tempCaseIdService.Insert(tempCaseId);
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
      var tempCaseId = await Task.Run(() =>
      {
        return new TempCaseId();
      });
      return Json(tempCaseId, JsonRequestBehavior.AllowGet);
    }


        public ActionResult Edit(int id)
    {
      var tempCaseId = this.tempCaseIdService.Find(id);
      if (tempCaseId == null)
      {
        return HttpNotFound();
      }
      return View(tempCaseId);
    }
            [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(TempCaseId tempCaseId)
    {
      if (tempCaseId == null)
      {
        throw new ArgumentNullException(nameof(tempCaseId));
      }
      if (ModelState.IsValid)
      {
        tempCaseId.TrackingState = TrackingState.Modified;
        try
        {
          this.tempCaseIdService.Update(tempCaseId);

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
        await this.tempCaseIdService.Queryable().Where(x => x.Id == id).DeleteAsync();
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
        await this.tempCaseIdService.Delete(id);
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
      var fileName = "tempcaseids_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
      var stream = await this.tempCaseIdService.ExportExcelAsync(filterRules, sort, order);
      return File(stream, "application/vnd.ms-excel", fileName);
    }
    private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
    private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;

  }
}
