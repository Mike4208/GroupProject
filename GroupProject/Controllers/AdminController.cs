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

namespace GroupProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationUserManager _userManager;

        ApplicationDbContext context;

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

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UserList()
        {
            var users = (from user in context.Users
                                  select new
                                  {
                                      FirstName = user.FirstName,
                                      LastName = user.LastName,
                                      user.Roles,
                                      user.LastLog,
                                      Creation = user.Created,
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      user.Email,
                                      Address = user.Address,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in context.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()
                                  }).ToList().Select(p => new UserView()
                                  {
                                      FirstName = p.FirstName,
                                      LastName = p.LastName,
                                      LastLogin = p.LastLog,
                                      Created = p.Creation,
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      Email = p.Email,
                                      Address = p.Address,
                                      UserRoles = string.Join(",", p.RoleNames)
                                  });
            return View(users);
        }

        //-----------------------------------------User Actions----------------------------------
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteUser(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = context.Users.Find(id);
            if (user == null)
                return HttpNotFound();
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult DeleteUser(string id, string user)
        {
            var userid = context.Users.Where(x => x.Id == id).Single();
            context.Users.Remove(userid);
            context.SaveChanges();
            return RedirectToAction("UserList");
        }
        
        // TODO! Make admin able to change user role
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = context.Users.Find(id);

            if (user == null)
                return HttpNotFound();
            //ViewBag.Name = new SelectList(context.Roles.Where(u => !u.Name.Contains("Admin")).ToList(), "Name", "Name");
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(string id, ApplicationUser user)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var userToUpdate = context.Users.Find(id);
            //var userToUpdate = context.Users.SingleOrDefault(u => u.Id == user.Id);

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
                //ViewBag.Name = new SelectList(context.Roles.Where(u => !u.Name.Contains("Admin")).ToList(), "Name", "Name");
            }
            return View(userToUpdate);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult IndexRole()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!isAdminUser())
                    return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Index", "Home");
            var Roles = context.Roles.ToList();
            return View(Roles);
        }

        public Boolean isAdminUser()
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

        public Boolean isEmployeeUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Employee")
                    return true;
                else
                    return false;
            }
            return false;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateRole()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!isAdminUser())
                    return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Index", "Home");
            var Role = new IdentityRole();
            return View(Role);
        }

        /// <summary>
        /// Create a New Role
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateRole(IdentityRole Role)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!isAdminUser())
                    return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Index", "Home");
            context.Roles.Add(Role);
            context.SaveChanges();
            return RedirectToAction("IndexRole");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteRole(string id)
        {
            var role = context.Roles.Find(id);
            return View(role);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ActionName("DeleteRole")]
        public ActionResult DeleteRole(string id, string role)
        {
            var roled = context.Roles.Where(x => x.Id == id).Single();
            context.Roles.Remove(roled);
            context.SaveChanges();
            return RedirectToAction("IndexRole");
        }
    }
}
