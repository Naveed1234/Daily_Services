using Daily_Services.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Daily_Services.ApplicationSignInManager;

namespace Daily_Services.Controllers
{
    //[AllowAnonymous]
    [Authorize]

    public class AccountController : Controller
    {
        public ApplicationUser context = new ApplicationUser();
        public ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationUser UpdateProfile(string id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var user = context.Users.Where(x => x.Id == id).FirstOrDefault();
            context.Entry(user).State = EntityState.Detached;
            return user;
        }

        [HttpGet]
        public ActionResult UpdateCustomer()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            List<Gender> genders = db.Genders.ToList();
            ViewBag.Gender_Id = new SelectList(genders, "Gender_Id", "Gender_Type");
            ViewBag.Qualification_Id = new SelectList(db.Qualifications, "Qualification_Id", "Qualification_Type");
            ViewBag.Category_Id = new SelectList(db.Categories, "Category_Id", "Category_Type");
            var userId = User.Identity.GetUserId();
            ApplicationUser user = context.Users.Find(userId);

            return View(user);
        }
        [HttpPost]
        public ActionResult UpdateCustomer(ApplicationUser update, HttpPostedFileBase file)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    if (file != null)
                    {
                        string ImageName = System.IO.Path.GetFileName(file.FileName);
                        string physicalPath = Server.MapPath("~/AppImages/" + ImageName);

                        // save image in folder
                        file.SaveAs(physicalPath);

                        //save new record in database
                        update.Image_Path = ImageName;
                        //db.UserInfoes.Add(newRecord);
                    }

                    var cutomer_Id = User.Identity.GetUserId();
                    var prof = UpdateProfile(cutomer_Id);
                    prof.Email = update.Email;
                    prof.First_Name = update.First_Name;
                    prof.Last_Name = update.Last_Name;
                    prof.Address = update.Address;
                    prof.Gender_Id = update.Gender_Id;
                    prof.Description = update.Description;
                    prof.Qualification_Id = update.Qualification_Id;
                    prof.Phone_Number = update.Phone_Number;
                    prof.Category_Id = update.Category_Id;
                    prof.SubCategory_Id = update.SubCategory_Id;
                    prof.Image_Path = update.Image_Path;
                    //profile.UpdateUserProfile(update);

                    context.Users.Add(prof);
                    context.Entry(prof).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    ViewBag.Gender_Id = new SelectList(db.Genders, "Gender_Id", "Gender_Type");
                    ViewBag.Qualification_Id = new SelectList(db.Qualifications, "Qualification_Id", "Qualification_Type");
                    ViewBag.Category_Id = new SelectList(db.Categories, "Category_Id", "Category_Type");
                    return RedirectToAction("UserProfile");
                }
            }
            catch (Exception e)
            {
                var msg = "" + e;

            }
            //ViewBag.Gender_Id = new SelectList(db.Genders, "Gender_Id", "Gender_Type");
            //ViewBag.Qualification_Id = new SelectList(db.Qualifications, "Qualification_Id", "Qualification_Type");
            //ViewBag.Category_Id = new SelectList(db.Categories, "Category_Id", "Category_Type");
            return View();
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    
                    return RedirectToAction("UserProfile");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //User Profile

        public ApplicationUser GetProfile(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var user = db.Users.Where(x => x.Id == id).Include(x => x.Category).FirstOrDefault();

            return user;
        }

        public ActionResult UserProfile()
        {

            var userId = User.Identity.GetUserId();
            var user = GetProfile(userId);

            return View(user);
        }

        //Update User Profile
        [HttpGet]
        public ActionResult UpdateSP(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = new ApplicationUser();
            user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);

            //SP list


        }


        public ActionResult ServiceProvider()
        {
            var list = db.Users.Where(x => x.ServiceOffer == true && x.IsActive == true && x.IsDeleted == false).ToList();

            return View(list);
        }


        //Post:Update User Profile
        [HttpPost]
        public ActionResult UpdateSP(RegisterViewModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
            //  return View();

        }




        //Delete User Pofile
        [HttpGet]
        public ActionResult DeleteSP(int? id)
        {
            ApplicationUser user = new ApplicationUser();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //  ApplicationUser user = new ApplicationUser();
            user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);


        }


        [HttpPost]
        public ActionResult DeleteSP(RegisterViewModel model)
        {

            return View();

        }












        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {


            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var role in RoleManager.Roles)
            {
                if (role.Name == "ServiceProvider")
                {
                    list.Add(new SelectListItem() { Value = role.Name, Text = role.Name });
                }
            }

            ViewBag.Roles = list;


            ViewBag.Category_Id = new SelectList(db.Categories, "Category_Id", "Category_Type");
            ViewBag.Gender_Id = new SelectList(db.Genders, "Gender_Id", "Gender_Type");
            ViewBag.Qualification_Id = new SelectList(db.Qualifications, "Qualification_Id", "Qualification_Type");

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                if (file != null)
                {
                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    string physicalPath = Server.MapPath("~/images/" + ImageName);

                    // save image in folder
                    file.SaveAs(physicalPath);

                    //save new record in database
                    model.Image_Path = ImageName;
                    //db.UserInfoes.Add(newRecord);
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Image_Path = model.Image_Path,
                    Gender_Id = model.Gender_Id,
                    Qualification_Id = model.Qualification_Id,
                    Category_Id = model.Category_Id,
                    SubCategory_Id = model.SubCategory_Id,
                    First_Name = model.First_Name,
                    Last_Name = model.Last_Name,
                    Phone_Number = model.phone_Number,
                    CNIC_No = model.CNIC_No,
                    Address = model.Address,
                    Description = model.Description,
                    DateOfBirth = model.DateOfBirth,
                    Postal_Code = model.Postal_Code,
                    IsActive = true,
                    IsDeleted = false,
                    ServiceOffer = true


                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }



        //Customer

        // GET: /Account/Register Customer
        [AllowAnonymous]
        public ActionResult CustomerRegister()
        {

             List<SelectListItem> list = new List<SelectListItem>();
            foreach (var role in RoleManager.Roles)
            {
                if (role.Name == "User")
                {
                    list.Add(new SelectListItem() { Value = role.Name, Text = role.Name });
                }
            }
            ViewBag.Roles = list;
            // ViewBag.Gender_Id = new SelectList(db.Genders, "Gender_Id", "Gender_Type");

            return View();
        }

        // POST: /Account/Customer
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CustomerRegister(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser
                {

                    Email = model.Email,
                    UserName = model.Email,
                    First_Name = model.First_Name,
                    Last_Name = model.Last_Name,
                    Phone_Number = model.phone_Number,

                    Address = model.Address,
                    IsActive = true,
                    IsDeleted = false,
                    ServiceOffer = false


                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }




        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);       
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/External
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login","Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}
