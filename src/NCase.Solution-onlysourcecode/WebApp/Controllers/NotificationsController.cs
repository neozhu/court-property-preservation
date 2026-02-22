using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using TrackableEntities;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Services;
using WebApp.SmartHubs;
using Z.EntityFramework.Plus;
namespace WebApp.Controllers
{
                            [System.Web.Mvc.Authorize]
  [RoutePrefix("Notifications")]
  public class NotificationsController : Controller
  {
    private readonly INotificationService notificationService;
    private readonly IUnitOfWorkAsync unitOfWork;
    private readonly IHubContext hub;
    public NotificationsController(INotificationService notificationService, IUnitOfWorkAsync unitOfWork)
    {
      
      this.notificationService = notificationService;
      this.unitOfWork = unitOfWork;
      this.hub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
    }
        [HttpGet]
    public async Task<JsonResult> GetNotifyData(string userName = "",int notifygroup=-1)
    {
      userName = string.IsNullOrEmpty(userName) ? this.User.Identity.Name : userName;
      if (notifygroup < 0)
      {
        var data = await this.notificationService.Queryable()
          .Where(x => x.Read == false && ( x.To == "ALL" || x.To == userName ))
          .OrderByDescending(x => x.Id).ToListAsync();
        return Json(new { data = data }, JsonRequestBehavior.AllowGet);
      }
      else
      {
        var data = await this.notificationService.Queryable()
          .Where(x => x.Read == false && ( x.To == "ALL" || x.To == userName ) && x.Group==notifygroup)
          .OrderByDescending(x => x.Id).ToListAsync();
        return Json(new { data = data }, JsonRequestBehavior.AllowGet);
      }
    }
    [HttpGet]
    public async Task<JsonResult> SetRead(int id) {
      await this.notificationService.Queryable().Where(x => x.Id == id).UpdateAsync(x => new Notification() { Read = true });
      this.hub.Clients.All.broadcastChanged();
      return Json(new { success = true }, JsonRequestBehavior.AllowGet);
    }
    [HttpGet]
    public async Task<JsonResult> SetAllRead(string userName)
    {
      await this.notificationService.Queryable().Where(x => x.To == userName || x.To=="ALL").UpdateAsync(x => new Notification() { Read = true });
      this.hub.Clients.All.broadcastChanged();
      return Json(new { success = true }, JsonRequestBehavior.AllowGet);
    }
            [Route("Index", Name = "消息推送", Order = 1)]
    public ActionResult Index() => this.View();

            [HttpGet]
    public async Task<JsonResult> GetDataAsync(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.notificationService
                                 .Query(new NotificationQuery().Withfilter(filters))
                                 .OrderBy(n => n.OrderBy(sort, order))
                                 .SelectPageAsync(page, rows, out var totalCount) )
                                 .Select(n => new
                                 {

                                   Id = n.Id,
                                   Title = n.Title,
                                   Content = n.Content,
                                   Link = n.Link,
                                   Read = n.Read,
                                   From = n.From,
                                   To = n.To,
                                   Group = n.Group,
                                   Created = n.Created.ToString("yyyy-MM-dd HH:mm:ss")
                                 }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return this.Json(pagelist, JsonRequestBehavior.AllowGet);
    }
        [HttpPost]
    public async Task<JsonResult> SaveDataAsync(Notification[] notifications)
    {
      if (notifications == null)
      {
        throw new ArgumentNullException(nameof(notifications));
      }
      if (this.ModelState.IsValid)
      {
        try
        {
                    foreach (var item in notifications)
          {
            this.notificationService.ApplyChanges(item);
            
          }
          var result = await this.unitOfWork.SaveChangesAsync();
          this.hub.Clients.All.broadcastChanged();
          return this.Json(new { success = true, result }, JsonRequestBehavior.AllowGet);
        }
        catch (System.Data.Entity.Validation.DbEntityValidationException e)
        {
          var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
          return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
        }
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return this.Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
      }

    }
        public ActionResult Details(int id)
    {

      var notification = this.notificationService.Find(id);
      if (notification == null)
      {
        return this.HttpNotFound();
      }
      return this.View(notification);
    }
        [HttpGet]
    public async Task<JsonResult> GetItemAsync(int id)
    {
      var notification = await this.notificationService.FindAsync(id);
      return this.Json(notification, JsonRequestBehavior.AllowGet);
    }
        public ActionResult Create()
    {
      var notification = new Notification();
            return this.View(notification);
    }
            [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> CreateAsync(Notification notification)
    {
      if (notification == null)
      {
        throw new ArgumentNullException(nameof(notification));
      }
      if (this.ModelState.IsValid)
      {
        try
        {
          this.notificationService.Insert(notification);
          var result = await this.unitOfWork.SaveChangesAsync();
          return this.Json(new { success = true, result }, JsonRequestBehavior.AllowGet);
        }
        catch (System.Data.Entity.Validation.DbEntityValidationException e)
        {
          var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
          return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
        }
              }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return this.Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
              }
          }

        [HttpGet]
    public async Task<JsonResult> NewItemAsync()
    {
      var notification = await Task.Run(() =>
      {
        return new Notification();
      });
      return this.Json(notification, JsonRequestBehavior.AllowGet);
    }


        public ActionResult Edit(int id)
    {
      var notification = this.notificationService.Find(id);
      if (notification == null)
      {
        return this.HttpNotFound();
      }
      return this.View(notification);
    }
            [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> EditAsync(Notification notification)
    {
      if (notification == null)
      {
        throw new ArgumentNullException(nameof(notification));
      }
      if (this.ModelState.IsValid)
      {
        notification.TrackingState = TrackingState.Modified;
        try
        {
          this.notificationService.Update(notification);

          var result = await this.unitOfWork.SaveChangesAsync();
          return this.Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
        }
        catch (System.Data.Entity.Validation.DbEntityValidationException e)
        {
          var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
          return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception e)
        {
          return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
        }

                      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return this.Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
              }
          }
            [HttpGet]
    public async Task<ActionResult> DeleteAsync(int id)
    {
      try
      {
        await this.notificationService.Queryable().Where(x => x.Id == id).DeleteAsync();
        return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (System.Data.Entity.Validation.DbEntityValidationException e)
      {
        var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
        return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }




        [HttpPost]
    public async Task<JsonResult> DeleteCheckedAsync(int[] id)
    {
      if (id == null)
      {
        throw new ArgumentNullException(nameof(id));
      }
      try
      {
        this.notificationService.Delete(id);
        await this.unitOfWork.SaveChangesAsync();
        return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      catch (System.Data.Entity.Validation.DbEntityValidationException e)
      {
        var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
        return this.Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
      }
      catch (Exception e)
      {
        return this.Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
      }
    }
        [HttpPost]
    public async Task< ActionResult> ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
    {
      var fileName = "notifications_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
      var stream = await this.notificationService.ExportExcelAsync(filterRules, sort, order);
      return this.File(stream, "application/vnd.ms-excel", fileName);
    }
    private void DisplaySuccessMessage(string msgText) => this.TempData["SuccessMessage"] = msgText;
    private void DisplayErrorMessage(string msgText) => this.TempData["ErrorMessage"] = msgText;

  }
}
