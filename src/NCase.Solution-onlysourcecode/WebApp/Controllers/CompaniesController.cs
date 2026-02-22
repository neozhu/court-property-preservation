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
    [RoutePrefix("Companies")]
	public class CompaniesController : Controller
	{
		private readonly ICompanyService  companyService;
		private readonly IUnitOfWorkAsync unitOfWork;
        private readonly NLog.ILogger logger;
		public CompaniesController (
          ICompanyService  companyService, 
          IUnitOfWorkAsync unitOfWork,
          NLog.ILogger logger
          )
		{
			this.companyService  = companyService;
			this.unitOfWork = unitOfWork;
            this.logger = logger;
		}
        		                [Route("Index", Name = "公司信息", Order = 1)]
		public ActionResult Index() => this.View();

				        
		[HttpGet]
        		 public async Task<JsonResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
		{
			var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
			var pagerows  = (await this.companyService
						               .Query(new CompanyQuery().Withfilter(filters))
							           .OrderBy(n=>n.OrderBy(sort,order))
							           .SelectPageAsync(page, rows, out var totalCount))
                                       .Select(  n => new { 

    Departments = n.Departments,
    Employees = n.Employees,
    Id = n.Id,
    Name = n.Name,
    Code = n.Code,
    Address = n.Address,
    Contect = n.Contect,
    PhoneNumber = n.PhoneNumber,
    RegisterDate = n.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss")
}).ToList();
			var pagelist = new { total = totalCount, rows = pagerows };
			return Json(pagelist, JsonRequestBehavior.AllowGet);
		}
    [HttpGet]
    public async Task<JsonResult> GetComboData(string q = "", int page = 1, int rows = 20, string sort = "Id", string order = "asc")
    {

      var pagerows = ( await this.companyService
                                 .Query(new CompanyQuery().WithfilterQ(q))
                                 .OrderBy(n => n.OrderBy(sort, order))
                                 .SelectPageAsync(page, rows, out var totalCount) )
                                 .Select(n => new {
                                   Id = n.Id,
                                   Name = n.Name,
                                   Code = n.Code,
                                   Type = "0",
                                   Address = n.Address,
                                   Contect = n.Contect,
                                   PhoneNumber = n.PhoneNumber,
                                   RegisterDate = n.RegisterDate.ToString("yyyy-MM-dd HH:mm:ss")
                                 }).ToList();
      var pagelist = new { total = totalCount, rows = pagerows };
      return Json(pagelist, JsonRequestBehavior.AllowGet);
    }

        [HttpPost]
		public async Task<JsonResult> SaveData(Company[] companies)
		{
            if (companies == null)
            {
                throw new ArgumentNullException(nameof(companies));
            }
            if (ModelState.IsValid)
			{
            try{
               foreach (var item in companies)
               {
                 this.companyService.ApplyChanges(item);
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
			
			var company = this.companyService.Find(id);
			if (company == null)
			{
				return HttpNotFound();
			}
			return View(company);
		}
                [HttpGet]
        public async Task<JsonResult> GetItem(int id) {
            var  company = await this.companyService.FindAsync(id);
            return Json(company,JsonRequestBehavior.AllowGet);
        }
		        		public ActionResult Create()
				{
			var company = new Company();
						return View(company);
		}
						[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(Company company)
		{
			if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            } 
            if (ModelState.IsValid)
			{
                try{ 
				this.companyService.Insert(company);
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
            var company = await Task.Run(() => {
                return new Company();
                });
            return Json(company, JsonRequestBehavior.AllowGet);
        }

         
				public ActionResult Edit(int id)
		{
			var company = this.companyService.Find(id);
			if (company == null)
			{
				return HttpNotFound();
			}
			return View(company);
		}
						[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(Company company)
		{
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }
			if (ModelState.IsValid)
			{
				company.TrackingState = TrackingState.Modified;
				                try{
				this.companyService.Update(company);
				                
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
               await this.companyService.Queryable().Where(x => x.Id == id).DeleteAsync();
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
               this.companyService.Delete(id);
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
			var fileName = "companies_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
			var stream= await  this.companyService.ExportExcelAsync(filterRules,sort, order );
			return File(stream, "application/vnd.ms-excel", fileName);
		}
		private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
        private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;
		 
	}
}
