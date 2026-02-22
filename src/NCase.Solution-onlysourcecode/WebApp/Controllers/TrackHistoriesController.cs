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
    [RoutePrefix("TrackHistories")]
	public class TrackHistoriesController : Controller
	{
		private readonly ITrackHistoryService  trackHistoryService;
		private readonly IUnitOfWorkAsync unitOfWork;
        private readonly NLog.ILogger logger;
		public TrackHistoriesController (
          ITrackHistoryService  trackHistoryService, 
          IUnitOfWorkAsync unitOfWork,
          NLog.ILogger logger
          )
		{
			this.trackHistoryService  = trackHistoryService;
			this.unitOfWork = unitOfWork;
            this.logger = logger;
		}
        		                [Route("Index", Name = "案件节点跟踪记录", Order = 1)]
		public ActionResult Index() => this.View();


    public async Task<JsonResult> GetHistoryData(string caseid) {
      var data = ( await this.trackHistoryService.Queryable()
              .Where(x => x.CaseId == caseid)
              .OrderBy(x => x.Id)
              .ToListAsync() )
              .Select(n => new
              {
                Id = n.Id,
                CaseId = n.CaseId,
                Status = n.Status,
                Node = n.Node,
                NodeStatus = n.NodeStatus,
                Owner = n.Owner??"",
                ToUser = n.ToUser ?? "",
                Recorder=n.Recorder??"",
                BeginDate = n.BeginDate.ToString("MM-dd HH:mm"),
                CompletedDate = (n.CompletedDate?.ToString("MM-dd HH:mm"))??"",
                Expires = n.Expires,
                DoDate = n.DoDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                Elapsed = n.Elapsed,
                State = n.State ?? "",
                Comment = n.Comment ?? "",
                Created = n.Created.ToString("yyyy-MM-dd HH:mm:ss")
              }).ToList();
      return Json(data, JsonRequestBehavior.AllowGet);

      }


				        
		[HttpGet]
        		 public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
		{
			var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			var pagerows  = (await this.trackHistoryService
						               .Query(new TrackHistoryQuery().Withfilter(filters))
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out var totalCount))
                                       .Select(  n => new { 

    Id = n.Id,
    CaseId = n.CaseId,
    Status = n.Status,
    Node = n.Node,
    NodeStatus = n.NodeStatus,
    Owner = n.Owner,
    ToUser = n.ToUser,
                                         Recorder = n.Recorder ?? "",
                                         BeginDate = n.BeginDate.ToString("yyyy-MM-dd HH:mm:ss"),
    CompletedDate = n.CompletedDate?.ToString("yyyy-MM-dd HH:mm:ss"),
    Expires = n.Expires,
    DoDate = n.DoDate?.ToString("yyyy-MM-dd HH:mm:ss"),
    Elapsed = n.Elapsed,
    State = n.State,
    Comment = n.Comment,
    Created = n.Created.ToString("yyyy-MM-dd HH:mm:ss")
}).ToList();
			var pagelist = new { total = totalCount, rows = pagerows };
			return Json(pagelist, JsonRequestBehavior.AllowGet);
		}
        		[HttpPost]
		public async Task<JsonResult> SaveData(TrackHistory[] trackhistories)
		{
            if (trackhistories == null)
            {
                throw new ArgumentNullException(nameof(trackhistories));
            }
            if (ModelState.IsValid)
			{
            try{
               foreach (var item in trackhistories)
               {
                 this.trackHistoryService.ApplyChanges(item);
               }
			   var result = await this.unitOfWork.SaveChangesAsync();
			   return Json(new {success=true,result}, JsonRequestBehavior.AllowGet);
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
			
			var trackHistory = this.trackHistoryService.Find(id);
			if (trackHistory == null)
			{
				return HttpNotFound();
			}
			return View(trackHistory);
		}
                [HttpGet]
        public async Task<JsonResult> GetItem(int id) {
            var  trackHistory = await this.trackHistoryService.FindAsync(id);
            return Json(trackHistory,JsonRequestBehavior.AllowGet);
        }
		        		public ActionResult Create()
				{
			var trackHistory = new TrackHistory();
						return View(trackHistory);
		}
						[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(TrackHistory trackHistory)
		{
			if (trackHistory == null)
            {
                throw new ArgumentNullException(nameof(trackHistory));
            } 
            if (ModelState.IsValid)
			{
                try{ 
				this.trackHistoryService.Insert(trackHistory);
				var result = await this.unitOfWork.SaveChangesAsync();
                return Json(new { success = true,result }, JsonRequestBehavior.AllowGet);
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
			else {
			   var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			   return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
			   			}
					}

                [HttpGet]
        public async Task<JsonResult> NewItem() {
            var trackHistory = await Task.Run(() => {
                return new TrackHistory();
                });
            return Json(trackHistory, JsonRequestBehavior.AllowGet);
        }

         
				public ActionResult Edit(int id)
		{
			var trackHistory = this.trackHistoryService.Find(id);
			if (trackHistory == null)
			{
				return HttpNotFound();
			}
			return View(trackHistory);
		}
						[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(TrackHistory trackHistory)
		{
            if (trackHistory == null)
            {
                throw new ArgumentNullException(nameof(trackHistory));
            }
			if (ModelState.IsValid)
			{
				trackHistory.TrackingState = TrackingState.Modified;
				                try{
				this.trackHistoryService.Update(trackHistory);
				                
				var result = await this.unitOfWork.SaveChangesAsync();
                return Json(new { success = true,result = result }, JsonRequestBehavior.AllowGet);
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
			else {
			var modelStateErrors =string.Join(",", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
			return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
						}
								}
        		        [HttpGet]
		public async Task<ActionResult> Delete(int id)
		{
          try{
               await this.trackHistoryService.Queryable().Where(x => x.Id == id).DeleteAsync();
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
        public async Task<JsonResult> DeleteChecked(int[] id) {
           if (id == null)
           {
                throw new ArgumentNullException(nameof(id));
           }
           try{
               this.trackHistoryService.Delete(id);
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
		public async Task<ActionResult> ExportExcel( string filterRules = "",string sort = "Id", string order = "asc")
		{
			var fileName = "trackhistories_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
			var stream = await this.trackHistoryService.ExportExcelAsync(filterRules,sort, order );
			return File(stream, "application/vnd.ms-excel", fileName);
		}
		private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
        private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;
		 
	}
}
