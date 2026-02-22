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
using Microsoft.AspNet.Identity.Owin;

namespace WebApp.Controllers
{
                            [Authorize]
  [RoutePrefix("NodeConfigs")]
  public class NodeConfigsController : Controller
  {
    private readonly INodeConfigService nodeConfigService;
    private readonly IUnitOfWorkAsync unitOfWork;
    private readonly NLog.ILogger logger;

    public ApplicationRoleManager RoleManager => this.HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
    public ApplicationUserManager UserManager => this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
    public NodeConfigsController(
          INodeConfigService nodeConfigService,
          IUnitOfWorkAsync unitOfWork,
          NLog.ILogger logger
          )
    {
      this.nodeConfigService = nodeConfigService;
      this.unitOfWork = unitOfWork;
      this.logger = logger;
    }

        public async Task<JsonResult> GetRoles() {

      var roles = await this.RoleManager.Roles.Select(x=>new {Name = x.Name}).ToListAsync();
      return Json(roles, JsonRequestBehavior.AllowGet);

    }
    public async Task<JsonResult> GetUsers(string rolename,string q="") {
      var userid = (await this.RoleManager.FindByNameAsync(rolename)).Users.Select(x=>x.UserId).ToArray();
      var users = await this.UserManager.Users.Where(x => userid.Contains(x.Id) && x.FullName.Contains(q)).Select(x => new { x.UserName, x.FullName }).ToListAsync();
      return Json(users, JsonRequestBehavior.AllowGet);
    }

            [Route("Index", Name = "节点配置", Order = 1)]
    public ActionResult Index() => this.View();

        
    [HttpGet]
        public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
    {
      var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
      var pagerows = ( await this.nodeConfigService
                           .Query(new NodeConfigQuery().Withfilter(filters))
                         .OrderBy(n => n.OrderBy(sort, order))
                         .SelectPageAsync(page, rows, out var totalCount) )
                                       .Select(n => new
                                       {

                                         Id = n.Id,
                                         Node = n.Node,
                                         Status=n.Status,
                                         Description = n.Description,
                                         Expires = n.Expires,
                                         Roles = n.Roles,
                                         Users = n.Users,
                                         NextNode = n.NextNode,
                                         NextStatus=n.NextStatus
                                       }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }
        [HttpPost]
    public async Task<JsonResult> SaveData(NodeConfig[] nodeconfigs)
    {
      if (nodeconfigs == null)
      {
        throw new ArgumentNullException(nameof(nodeconfigs));
      }
      if (ModelState.IsValid)
      {
        try
        {
          foreach (var item in nodeconfigs)
          {
            this.nodeConfigService.ApplyChanges(item);
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

      var nodeConfig = this.nodeConfigService.Find(id);
      if (nodeConfig == null)
      {
        return HttpNotFound();
      }
      return View(nodeConfig);
    }
        [HttpGet]
    public async Task<JsonResult> GetItem(int id)
    {
      var nodeConfig = await this.nodeConfigService.FindAsync(id);
      return Json(nodeConfig, JsonRequestBehavior.AllowGet);
    }
        public ActionResult Create()
    {
      var nodeConfig = new NodeConfig();
            return View(nodeConfig);
    }
            [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(NodeConfig nodeConfig)
    {
      if (nodeConfig == null)
      {
        throw new ArgumentNullException(nameof(nodeConfig));
      }
      if (ModelState.IsValid)
      {
        try
        {
          this.nodeConfigService.Insert(nodeConfig);
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
      var nodeConfig = await Task.Run(() =>
      {
        return new NodeConfig();
      });
      return Json(nodeConfig, JsonRequestBehavior.AllowGet);
    }


        public ActionResult Edit(int id)
    {
      var nodeConfig = this.nodeConfigService.Find(id);
      if (nodeConfig == null)
      {
        return HttpNotFound();
      }
      return View(nodeConfig);
    }
            [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(NodeConfig nodeConfig)
    {
      if (nodeConfig == null)
      {
        throw new ArgumentNullException(nameof(nodeConfig));
      }
      if (ModelState.IsValid)
      {
        nodeConfig.TrackingState = TrackingState.Modified;
        try
        {
          this.nodeConfigService.Update(nodeConfig);

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
        await this.nodeConfigService.Queryable().Where(x => x.Id == id).DeleteAsync();
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
        this.nodeConfigService.Delete(id);
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
      var fileName = "nodeconfigs_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
      var stream = await this.nodeConfigService.ExportExcelAsync(filterRules, sort, order);
      return File(stream, "application/vnd.ms-excel", fileName);
    }
    private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
    private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;

  }
}
