using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using GroupProject.Models;
using System.Net;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using GroupProject.Data;

namespace GroupProject.Controllers
{

    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ApplicationDbContext context;


        public ManageController()
        {
            context = new ApplicationDbContext();
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        //
        // GET: /Manage/Index
        public ActionResult Index(ManageMessageId? message)
        {
            ViewBag.PageName = "Manage";
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.ChangeAccountDetailsSuccess ? "Profile updated successfully."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);
            var model = new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Created = user.Created,
                LastLogin = user.LastLog
            };
            var s = UserManager.GetRoles(userId);
            if (s[0].ToString().Equals("User"))
                ViewBag.IsUser = true;
            else
                ViewBag.IsUser = false;
            return View(model);
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            ViewBag.PageName = "Manage";
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/Edit
        [Authorize]
        public ActionResult Edit()
        {
            ViewBag.PageName = "Manage";
            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);
            var model = new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address
            };
            return View(model);
        }

        //
        // POST: /Manage/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Username, Email, FirstName, LastName, Address, City, PostalCode")] IndexViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (UserManager.FindByEmail(model.Email) != null)
            {
                ModelState.AddModelError("Email", "Email already exists");
                if (UserManager.FindByName(model.Username) != null)
                    ModelState.AddModelError("Username", "Username already exists");
                return View(model);
            }

            var userId = User.Identity.GetUserId();
            var user = UserManager.FindById(userId);
            user.UserName = model.Username;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Address = model.Address;
            user.City = model.City;
            user.PostalCode = model.PostalCode;

            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, true, true);
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangeAccountDetailsSuccess });
            }

            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/Orders
        [Authorize(Roles = "User")]
        public ActionResult Orders()
        {
            ViewBag.PageName = "Manage";
            var id = User.Identity.GetUserId();
            var user = UserManager.FindById(id);
            user.Orders = context.Orders.Where(x => x.ApplicationUserID == id).ToList();
            var model = user.Orders;
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
            base.Dispose(disposing);
        }

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

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            ChangeAccountDetailsSuccess,
            Error
        }
    }
}