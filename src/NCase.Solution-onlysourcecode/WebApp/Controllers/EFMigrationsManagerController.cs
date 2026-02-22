namespace WebApp.Controllers
{
  using System.Net;
  using System.Web;
  using System.Web.Mvc;
  using Microsoft.AspNet.Identity.Owin;
  using NB.Apps.EFMigrationsManager.CustomAttributes;
  using NB.Apps.EFMigrationsManager.Models;
  using NB.Apps.EFMigrationsManager.Services;

  [VerifyIsEFMigrationUpToDate(true)]
  [RoutePrefix("EFMigrationsManager")]
  public class EFMigrationsManagerController : Controller
  {
    private ApplicationSignInManager _signInManager;

    public ApplicationSignInManager SignInManager
        {
            get => this._signInManager ?? this.HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => this._signInManager = value;
        }
        public ActionResult Publish(bool isRollback = false)
    {
      var _service = new EFMigrationService();
      if (!_service.IsAuthorizedUser())
      {
        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
      }
      var vm = _service.LoadMigrationDetails(isRollback);
      return this.View(vm);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public ActionResult Publish(EFMigrationDetails entity)
    {
      var _service = new EFMigrationService();
      if (!_service.IsAuthorizedUser())
      {
        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
      }
      if (entity == null || string.IsNullOrWhiteSpace(entity.TargetMigration))
      {
        throw new System.ArgumentException("Invalid Parameters...");
      }

      _service.Update(entity.TargetMigration);

      this.TempData["StatusMessage"] = entity.IsRollback ? "Database restored successfully." : "Database updated successfully.";
            return this.Redirect("/");
    }

    public ActionResult DbMaintenance() =>
                                    
                        
                        
                        this.View();
  }
}