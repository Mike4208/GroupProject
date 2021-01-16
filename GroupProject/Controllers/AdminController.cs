using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using GroupProject.Models;
using GroupProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using GroupProject.Data;

namespace GroupProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationUserManager _userManager;
        readonly ApplicationDbContext context;

        public AdminController()
        {
            context = new ApplicationDbContext();
        }

        public AdminController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
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

        public ActionResult Index()
        {
            ViewBag.PageName = "Admin";
            return View();
        }

        public ActionResult UserList()
        {
            ViewBag.PageName = "Admin";
            var users = (from user in context.Users
                                  select new
                                  {
                                      user.FirstName,
                                      user.LastName,
                                      user.Roles,
                                      user.CurrentLog,
                                      user.Created,
                                      user.Id,
                                      user.UserName,
                                      user.Email,
                                      user.Address,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in context.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()
                                  }).ToList().Select(p => new UserView()
                                  {
                                      FirstName = p.FirstName,
                                      LastName = p.LastName,
                                      LastLogin = p.CurrentLog,
                                      Created = p.Created,
                                      UserId = p.Id,
                                      Username = p.UserName,
                                      Email = p.Email,
                                      Address = p.Address,
                                      UserRoles = string.Join(",", p.RoleNames)
                                  });
            users = users.OrderBy(x => x.UserRoles);
            return View(users);
        }

        //-----------------------------------------User Actions----------------------------------
        public ActionResult DeleteUser(string id)
        {
            ViewBag.PageName = "Admin";
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = context.Users.Find(id);
            if (user == null)
                return HttpNotFound();
            return View(user);
        }

        [HttpPost]
        [ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserConfirmed(string id)
        {
            var userid = context.Users.Where(x => x.Id == id).Single();
            context.Users.Remove(userid);
            context.SaveChanges();
            return RedirectToAction("UserList");
        }
        
        public ActionResult EditUser(string id)
        {
            ViewBag.PageName = "Admin";
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = context.Users.Find(id);
            if (user == null)
                return HttpNotFound();
            var model = new IndexViewModel()
            {
                Email = user.Email,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("EditUser")]
        public ActionResult EditUserConfirmed(string id, IndexViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            if (context.Users.Any(x => x.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(model);
            }
            if (context.Users.Any(x => x.UserName == model.Username))
            {
                ModelState.AddModelError("Username", "Username already exists");
                return View(model);
            }

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var userToUpdate = context.Users.Find(id);

            if (TryUpdateModel(userToUpdate, "", new string[] { "Email", "Username", "FirstName", "LastName", "Address" }))
            {
                try
                {
                    context.SaveChanges();

                    return RedirectToAction("UserList");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(userToUpdate);
        }

        public ActionResult Create()
        {
            ViewBag.PageName = "Admin";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Created = DateTime.Now, FirstName = model.FirstName, LastName = model.LastName, Address = model.Address };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await this.UserManager.AddToRoleAsync(user.Id, "Employee");
                    return RedirectToAction("UserList", "Admin");
                }
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");

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

        //-----------------------------------------Roles Actions----------------------------------
        public ActionResult IndexRole()
        {
            ViewBag.PageName = "Admin";
            if (User.Identity.IsAuthenticated)
            {
                if (!IsAdminUser())
                    return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Index", "Home");
            var Roles = context.Roles.ToList();
            return View(Roles);
        }

        public Boolean IsAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                    return true;
                else
                    return false;
            }
            return false;
        }

        public ActionResult CreateRole()
        {
            ViewBag.PageName = "Admin";
            var Role = new IdentityRole();
            return View(Role);
        }

        /// <summary>
        /// Create a New Role
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRole(IdentityRole Role)
        {
            context.Roles.Add(Role);
            context.SaveChanges();
            return RedirectToAction("IndexRole");
        }

        public ActionResult DeleteRole(string id)
        {
            ViewBag.PageName = "Admin";
            var role = context.Roles.Find(id);
            return View(role);
        }

        [HttpPost]
        [ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleConfirmed(string id)
        {
            var roled = context.Roles.Where(x => x.Id == id).Single();
            context.Roles.Remove(roled);
            context.SaveChanges();
            return RedirectToAction("IndexRole");
        }
    }
}
