using System;
using System.Web.Mvc;
using System.Web.Security;
using KitchIn.Core.Interfaces;
using KitchIn.Resources;
using KitchIn.Web.Core.Models.Account;
using SmartArch.Web.Attributes;
using SmartArch.Web.Membership;

namespace KitchIn.Web.Controllers
{
    using System.Web;

    /// <summary>
    /// The account controller
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// The membership provider
        /// </summary>
        private readonly IMembershipProvider provider;

        /// <summary>
        /// Represents service for authentication users
        /// </summary>
        private readonly IAuthenticationService authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <param name="authenticationService">
        /// The authentication Service.
        /// </param>
        public AccountController(IMembershipProvider provider, IAuthenticationService authenticationService)
        {
            this.provider = provider;
            this.authenticationService = authenticationService;
        }

        /// <summary>
        /// Logon user of system
        /// </summary>
        /// <returns>
        /// View home page
        /// </returns>
        [HttpGet]
        public ActionResult LogOff()
        {
            var user = this.provider.GetUser(User.Identity.Name);
            if (user != null)
            {
                this.provider.LogoutUser(user);
            }
            this.authenticationService.SignOut();
            return this.RedirectToAction("LogOn", "Account");
        }

        /// <summary>
        /// Call user authentication view
        /// </summary>
        /// <returns>The home page</returns>
        [HttpGet]
        public ActionResult LogOn()
        {
            var isAuth = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (isAuth)
            {
                return this.RedirectToAction("Index", "Users", new { area = "Admin" });
            }
            return this.View();
        }

        /// <summary>
        /// The user logon
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return url.</param>
        /// <returns>View in according to the role</returns>
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                model.Email = model.Login;
                try
                {
                    if (this.provider.ValidateUser(model.Email, model.Password))
                    {
                        this.authenticationService.SignIn(model);
                        this.provider.LoginUser(model.Email, model.Password);
                        return this.RedirectToAction("Index", "Users", new { area = "Admin" });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, WEB.ModelState_InvaliLoginPassword);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, WEB.ModelState_LoginError);
                }
            }
            return this.View();
        }

        ///// <summary>
        ///// Call view register user in system
        ///// </summary>
        ///// <returns>The register view</returns>
        //[HttpGet]
        //public ActionResult Register()
        //{
        //    return this.View();
        //}

        // <summary>
        // Register user in system
        // </summary>
        // <param name="model">The model.</param>
        // <returns>Logon view</returns>
        //[HttpPost]
        //[Transaction]
        //public ActionResult Register(RegisterModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        this.provider.CreateUser(model);
        //        return this.RedirectToAction("LogOn", "Account");
        //    }

        //    return this.View(model);
        //}

    //    /// <summary>
    //    /// Call view change the user password
    //    /// </summary>
    //    /// <returns>View ChangePassword</returns>
    //    [HttpGet]
    //    [Authorize]
    //    public ActionResult ChangePassword()
    //    {
    //        return this.View();
    //    }

    //    /// <summary>
    //    /// Change the user password
    //    /// </summary>
    //    /// <param name="model">The model.</param>
    //    /// <returns>
    //    /// If success - redirect to the ChangePasswordSuccess, else display an error message
    //    /// </returns>
    //    [Transaction]
    //    [HttpPost]
    //    [Authorize]
    //    public ActionResult ChangePassword(ChangePasswordModel model)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                var user = this.provider.GetUser(User.Identity.Name);
    //                if (this.provider.ValidateUser(user.Email, model.OldPassword))
    //                {
    //                    this.provider.ChangeUserPassword(user.Id, model.NewPassword);
    //                    return this.View("ChangePasswordSuccess");
    //                }
    //                else
    //                {
    //                    ModelState.AddModelError(string.Empty, WEB.ModelState_OldPasswordError);
    //                }
    //            }
    //            catch (Exception)
    //            {
    //                ModelState.AddModelError(string.Empty, WEB.ModelState_ChangePasswordError);
    //            }
    //        }

    //        return this.View(model);
    //    }
    }
}