#region Using

using System;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using WebApp.Models;
using WebApp.Services;

#endregion

namespace WebApp.Controllers
{
  [Authorize]
  public class AccountController : Controller
  {
                private ApplicationUserManager _userManager;
    public ApplicationUserManager UserManager
        {
            get => this._userManager ?? this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => this._userManager = value;
        }
    private ApplicationSignInManager _signInManager;

    public ApplicationSignInManager SignInManager
        {
            get => this._signInManager ?? this.HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => this._signInManager = value;
        }
    private readonly ICompanyService _companyService;
    private readonly NLog.ILogger logger;
    public AccountController(
      NLog.ILogger logger,
      ICompanyService companyService) {
      this._companyService = companyService;
      this.logger = logger;
      }

        [AllowAnonymous]
    public ActionResult ForgotPassword()
    {
            this.EnsureLoggedOut();

      return this.View();
    }

        [AllowAnonymous]

    public ActionResult Login(string returnUrl)
    {
            this.EnsureLoggedOut();

      
      return this.View(new AccountLoginModel { ReturnUrl = returnUrl });
    }

        [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Login(AccountLoginModel viewModel)
    {
            if (!this.ModelState.IsValid)
      {
        return this.View(viewModel);
      }

            var user = await this.UserManager.FindByNameAsync(viewModel.UserName);
 
            if (user != null)
      {
        switch (await this.SignInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, viewModel.RememberMe, shouldLockout: true))
        {
          case SignInStatus.Success:
                        await this.SignInAsync(user, viewModel.RememberMe);
                        this.logger.Info($"{viewModel.UserName}:登录成功");
            return this.RedirectToLocal(viewModel.ReturnUrl);
          case SignInStatus.LockedOut:
            this.ModelState.AddModelError("", "账号已禁用");
            return this.View(viewModel);
          case SignInStatus.RequiresVerification:
          case SignInStatus.Failure:
          default:
            this.ModelState.AddModelError("", "用户名或密码不准确");
            return this.View(viewModel);
        }
      }

            this.ModelState.AddModelError("", "用户不存在.");

            return this.View(viewModel);
    }

        [AllowAnonymous]
    public ActionResult Error()
    {
            this.EnsureLoggedOut();

      return this.View();
    }

        [AllowAnonymous]
    public ActionResult Error404() => this.View();

        [AllowAnonymous]

    public ActionResult Register()
    {
            this.EnsureLoggedOut();
      var data = this._companyService.Queryable().Select(x => new ListItem() { Value = x.Id.ToString(), Text = x.Name });

      this.ViewBag.companylist = data;
     var model = new AccountRegistrationModel
      {
        CompanyName = data.FirstOrDefault()?.Text
      };

      return this.View(model);
    }

        [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Register(AccountRegistrationModel viewModel)
    {
      var data = this._companyService.Queryable().Select(x => new ListItem() { Value = x.Id.ToString(), Text = x.Name });
      this.ViewBag.companylist = data;

            if (!this.ModelState.IsValid)
      {
        return this.View(viewModel);
      }

            try
      {
                var user = new ApplicationUser
        {
          UserName = viewModel.Username,
          FullName = viewModel.FullName,
          CompanyCode = viewModel.CompanyCode,
          CompanyName = viewModel.CompanyName,
          TenantId = viewModel.TenantId,
          Gender = viewModel.Gender,
          Email = viewModel.Email,
          PhoneNumber=viewModel.PhoneNumber,
          AccountType = 0,
          AvatarsX50 = viewModel.Gender == 2 ? "femal1" : "male1",
          AvatarsX120 = viewModel.Gender == 2 ? "femal1_big" : "male1_big",

        };
        var result = await this.UserManager.CreateAsync(user, viewModel.Password);

                if (!result.Succeeded)
        {
                    this.AddErrors(result);

          return this.View(viewModel);
        }
        this.logger.Info($"注册成功【{user.UserName}】");
        await this.UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("http:        await this.UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("CompanyName", user.CompanyName));
        await this.UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email));
        await this.UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("FullName", user.FullName));
        await this.UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("AvatarsX50", user.AvatarsX50));
        await this.UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("AvatarsX120", user.AvatarsX120));
        await this.UserManager.AddClaimAsync(user.Id, new System.Security.Claims.Claim("PhoneNumber", user.PhoneNumber));
                        await this.SignInAsync(user, true);

        return this.RedirectToLocal();
      }
      catch (DbEntityValidationException ex)
      {
                this.AddErrors(ex);

        return this.View(viewModel);
      }
    }
                                public async Task<JsonResult> ChangePassword(string username, string currpwd, string pwd)
    {
      var user = await this.UserManager.FindByNameAsync(username);
      var result = await this.UserManager.ChangePasswordAsync(user.Id, currpwd, pwd);
      if (result.Succeeded)
      {
        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
      }
      else
      {
        return Json(new { success = false, err = string.Join(",", result.Errors) }, JsonRequestBehavior.AllowGet);
      }
    }
        [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Logout()
    {
            FormsAuthentication.SignOut();
      this.SignInManager.SignOut();
            this.HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

                  return this.RedirectToLocal();
    }
    public  ActionResult Profile() => this.View();
    private ActionResult RedirectToLocal(string returnUrl = "")
    {
                  if (!returnUrl.IsNullOrWhiteSpace() && this.Url.IsLocalUrl(returnUrl))
      {
        return this.Redirect(returnUrl);
      }

            return this.RedirectToAction("index", "home");
    }

    private void AddErrors(DbEntityValidationException exc)
    {
      foreach (var error in exc.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors.Select(validationError => validationError.ErrorMessage)))
      {
        this.ModelState.AddModelError("", error);
      }
    }

    private void AddErrors(IdentityResult result)
    {
            foreach (var error in result.Errors)
      {
        this.ModelState.AddModelError("", error);
      }
    }

    private void EnsureLoggedOut()
    {
            if (this.Request.IsAuthenticated)
        this.Logout();
    }
    private async Task SignInAsync(ApplicationUser user, bool isPersistent) =>
                        
        
                FormsAuthentication.SetAuthCookie(( await this.SignInManager.CreateUserIdentityAsync(user) ).Name, isPersistent);
    [HttpGet]
    public ActionResult SetCulture(string lang)
    {
      switch (lang.Trim())
      {
        case "en":
          CultureInfo.CurrentCulture = new CultureInfo("en-US");
          CultureInfo.CurrentUICulture = new CultureInfo("en-US");
          break;
        case "cn":
          CultureInfo.CurrentCulture = new CultureInfo("zh-CN");
          CultureInfo.CurrentUICulture = new CultureInfo("zh-CN");
          break;
        case "tw":
          CultureInfo.CurrentCulture = new CultureInfo("zh-TW");
          CultureInfo.CurrentUICulture = new CultureInfo("zh-TW");
          break;
      }

      var cookie = new HttpCookie("culture", lang)
      {
        Expires = DateTime.Now.AddYears(1)
      };
      this.Response.Cookies.Add(cookie);
      return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
    }

        [AllowAnonymous]
    public ActionResult Lock() => this.View();
  }
}