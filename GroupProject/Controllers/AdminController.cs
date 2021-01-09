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

namespace GroupProject.Controllers
{
    public class AdminController : Controller
    {

        ApplicationDbContext context;

        public AdminController()
        {
            context = new ApplicationDbContext();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;
                ViewBag.displayMenu = "No";
                if (isAdminUser())
                    ViewBag.displayMenu = "Yes";
                return View();
            }
            else
                ViewBag.Name = "Unknown!";
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
        [ActionName("DeleteUser")]
        public ActionResult DeleteUser(string id, string user)
        {
            var userid = context.Users.Where(x => x.Id == id).Single();
            context.Users.Remove(userid);
            context.SaveChanges();
            return RedirectToAction("UserList");
        }
        
        // todo! Make admin able to change user role
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = context.Users.Find(id);

            if (user == null)
                return HttpNotFound();
            ViewBag.Name = new SelectList(context.Roles.Where(u => !u.Name.Contains("Admin"))
                                        .ToList(), "Name", "Name");
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

            if (TryUpdateModel(userToUpdate, "", new string[] { "Email", "Username", "FirstName", "LastName" }))
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
                ViewBag.Name = new SelectList(context.Roles.Where(u => !u.Name.Contains("Admin")).ToList(), "Name", "Name");
            }
            return View(userToUpdate);
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
