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
  /// <summary>
  /// File: NodeConfigsController.cs
  /// Purpose:节点时间管理/节点配置
  /// Created Date: 3/3/2020 11:26:32 AM
  /// Author: neo.zhu
  /// Tools: SmartCode MVC5 Scaffolder for Visual Studio 2017
  /// TODO: Registers the type mappings with the Unity container(Mvc.UnityConfig.cs)
  /// <![CDATA[
  ///    container.RegisterType<IRepositoryAsync<NodeConfig>, Repository<NodeConfig>>();
  ///    container.RegisterType<INodeConfigService, NodeConfigService>();
  /// ]]>
  /// Copyright (c) 2012-2018 All Rights Reserved
  /// </summary>
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

    //获取角色
    public async Task<JsonResult> GetRoles() {

      var roles = await this.RoleManager.Roles.Select(x=>new {Name = x.Name}).ToListAsync();
      return Json(roles, JsonRequestBehavior.AllowGet);

    }
    public async Task<JsonResult> GetUsers(string rolename,string q="") {
      var userid = (await this.RoleManager.FindByNameAsync(rolename)).Users.Select(x=>x.UserId).ToArray();
      var users = await this.UserManager.Users.Where(x => userid.Contains(x.Id) && x.FullName.Contains(q)).Select(x => new { x.UserName, x.FullName }).ToListAsync();
      return Json(users, JsonRequestBehavior.AllowGet);
    }

    //GET: NodeConfigs/Index
    //[OutputCache(Duration = 60, VaryByParam = "none")]
    [Route("Index", Name = "节点配置", Order = 1)]
    public ActionResult Index() => this.View();

    //Get :NodeConfigs/GetData
    //For Index View datagrid datasource url

    [HttpGet]
    //[OutputCache(Duration = 10, VaryByParam = "*")]
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
    //easyui datagrid post acceptChanges 
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
    //GET: NodeConfigs/Details/:id
    public ActionResult Details(int id)
    {

      var nodeConfig = this.nodeConfigService.Find(id);
      if (nodeConfig == null)
      {
        return HttpNotFound();
      }
      return View(nodeConfig);
    }
    //GET: NodeConfigs/GetItem/:id
    [HttpGet]
    public async Task<JsonResult> GetItem(int id)
    {
      var nodeConfig = await this.nodeConfigService.FindAsync(id);
      return Json(nodeConfig, JsonRequestBehavior.AllowGet);
    }
    //GET: NodeConfigs/Create
    public ActionResult Create()
    {
      var nodeConfig = new NodeConfig();
      //set default value
      return View(nodeConfig);
    }
    //POST: NodeConfigs/Create
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
        //DisplaySuccessMessage("Has update a nodeConfig record");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(nodeConfig);
    }

    //新增对象初始化
    [HttpGet]
    public async Task<JsonResult> NewItem()
    {
      var nodeConfig = await Task.Run(() =>
      {
        return new NodeConfig();
      });
      return Json(nodeConfig, JsonRequestBehavior.AllowGet);
    }


    //GET: NodeConfigs/Edit/:id
    public ActionResult Edit(int id)
    {
      var nodeConfig = this.nodeConfigService.Find(id);
      if (nodeConfig == null)
      {
        return HttpNotFound();
      }
      return View(nodeConfig);
    }
    //POST: NodeConfigs/Edit/:id
    //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        //DisplaySuccessMessage("Has update a NodeConfig record");
        //return RedirectToAction("Index");
      }
      else
      {
        var modelStateErrors = string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
        return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
        //DisplayErrorMessage(modelStateErrors);
      }
      //return View(nodeConfig);
    }
    //删除当前记录
    //GET: NodeConfigs/Delete/:id
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
    //导出Excel
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
