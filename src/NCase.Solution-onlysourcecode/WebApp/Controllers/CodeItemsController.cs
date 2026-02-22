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
using WebApp.Models;
using WebApp.Services;
using WebApp.Repositories;
 
using System.IO;
using TrackableEntities;

namespace WebApp.Controllers
{
  [RoutePrefix("CodeItems")]
  public class CodeItemsController : Controller
    {

                        
                private readonly ICodeItemService _codeItemService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public CodeItemsController(ICodeItemService codeItemService, IUnitOfWorkAsync unitOfWork)
        {
            _codeItemService = codeItemService;
            _unitOfWork = unitOfWork;
        }

        [Route("Index", Name = "键值对维护", Order = 1)]
    public ActionResult Index()
        {




            return View();

        }

                        [HttpGet]
        public async Task<ActionResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
            var totalCount = 0;
                        var codeitems = await _codeItemService
       .Query(new CodeItemQuery().Withfilter(filters))
       .OrderBy(n => n.OrderBy(sort, order))
       .SelectPageAsync(page, rows, out totalCount);

            var datarows = codeitems.Select(n => new {
                Multiple = n.Multiple,
                CodeType =n.CodeType, Id = n.Id, Code = n.Code, Text = n.Text, Description = n.Description, IsDisabled = n.IsDisabled }).ToList();
            var pagelist = new { total = totalCount, rows = datarows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateJavascript() {
            var jsfilename = Path.Combine(Server.MapPath("~/Scripts/"), "jquery.extend.formatter.js");
           await this._codeItemService.UpdateJavascriptAsync(jsfilename);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public async Task<ActionResult> SaveData(CodeItemChangeViewModel codeitems)
        {
            if (codeitems.updated != null)
            {
                foreach (var item in codeitems.updated)
                {
                    _codeItemService.Update(item);
                }
            }
            if (codeitems.deleted != null)
            {
                foreach (var item in codeitems.deleted)
                {
                    _codeItemService.Delete(item);
                }
            }
            if (codeitems.inserted != null)
            {
                foreach (var item in codeitems.inserted)
                {
                    _codeItemService.Insert(item);
                }
            }
            await _unitOfWork.SaveChangesAsync();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }





                public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var codeItem = await _codeItemService.FindAsync(id);

            if (codeItem == null)
            {
                return HttpNotFound();
            }
            return View(codeItem);
        }


                public ActionResult Create()
        {
            var codeItem = new CodeItem();
              
          
            return View(codeItem);
        }

                        [HttpPost]
                public async Task<ActionResult> Create([Bind(Include = "BaseCode,Id,Code,Text,Description,IsDisabled,BaseCodeId,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy")] CodeItem codeItem)
        {
            if (ModelState.IsValid)
            {
                _codeItemService.Insert(codeItem);
                await _unitOfWork.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                                return RedirectToAction("Index");
            }
            else
            {
                var modelStateErrors = String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
                }
            }
    
           

            return View(codeItem);
        }

                public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var codeItem = await _codeItemService.FindAsync(id);
            if (codeItem == null)
            {
                return HttpNotFound();
            }

         
            return View(codeItem);
        }

                        [HttpPost]
                public async Task<ActionResult> Edit([Bind(Include = "BaseCode,Id,Code,Text,Description,IsDisabled,BaseCodeId,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy")] CodeItem codeItem)
        {
            if (ModelState.IsValid)
            {
                codeItem.TrackingState = TrackingState.Modified;
                _codeItemService.Update(codeItem);

                await _unitOfWork.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Index");
            }
            else
            {
                var modelStateErrors = String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
                }
            }
           
            return View(codeItem);
        }

                public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var codeItem = await _codeItemService.FindAsync(id);
            if (codeItem == null)
            {
                return HttpNotFound();
            }
            return View(codeItem);
        }

                [HttpPost, ActionName("Delete")]
                public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var codeItem = await _codeItemService.FindAsync(id);
            _codeItemService.Delete(codeItem);
            await _unitOfWork.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }






                [HttpPost]
        public async Task<ActionResult> ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
        {
            var fileName = "codeitems_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var stream = await _codeItemService.ExportExcelAsync(filterRules, sort, order);
            return File(stream, "application/vnd.ms-excel", fileName);

        }


     

         
    }
}
