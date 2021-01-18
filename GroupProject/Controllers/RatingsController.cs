using GroupProject.Data;
using GroupProject.Models;
using GroupProject.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GroupProject.Controllers
{
    public class RatingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //
        // GET: Ratings/Details
        public ActionResult Details(int? id)
        {
            //var productswithreviews = (from r in db.Ratings
            //                           join p in db.Products on r.ProductId equals p.ID
            //                           join u in db.Users on r.Id equals u.Id
            //                           select new
            //                           {
            //                               ratingid = r.RatingId,
            //                               productid = p.ID,
            //                               id = u.Id,
            //                               ratigttext = r.RatingText,
            //                               username = u.UserName,
            //                               name = p.Name,
            //                               approved = r.IsApproved,
            //                               date = r.ReviewCreated,
            //                               stars = r.Stars,
            //                           }).ToList().Select(p => new RatingViewModel()
            //                           {
            //                               RatingID = p.ratingid,
            //                               ProductID = p.productid,
            //                               ID = p.id,
            //                               RatingText = p.ratigttext,
            //                               Username = p.username,
            //                               Name = p.name,
            //                               IsApproved = p.approved,
            //                               ReviewCreated = p.date,
            //                               Stars = (int)p.stars
            //                           }).Where(x => x.ProductID == id).ToList();

            List<RatingViewModel> productswithreviews = (from r in db.Ratings
                                                         join p in db.Products on r.ProductId equals p.ID
                                                        select new RatingViewModel
                                                        {
                                                            RatingID = r.RatingId,
                                                            ProductID = r.ProductId,
                                                            RatingText = r.RatingText,
                                                            Username = r.UserName,
                                                            Name = p.Name,
                                                            IsApproved = r.IsApproved,
                                                            ReviewCreated = r.ReviewCreated,
                                                            Stars = (int)r.Stars
                                                        }).Where(x => x.ProductID == id).ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (productswithreviews.Count() == 0)
            {
                return RedirectToAction("RatingError");
            }

            return View(productswithreviews);
        }

        //
        // GET: Ratings
        public async Task<ActionResult> Index()
        {
            var model = await db.Ratings.ToListAsync();
            return View(model);
        }

        //
        // GET: Ratings/Create
        public ActionResult Create()
        {
            ViewBag.Stars = new List<SelectListItem>()
                    {
                        new SelectListItem() {Text="1", Value = "1" },
                        new SelectListItem() {Text="2", Value = "2" },
                        new SelectListItem() {Text="3", Value = "3" },
                        new SelectListItem() {Text="4", Value = "4" },
                        new SelectListItem() {Text="5", Value = "5" }
                    };
            return View();
        }

        //
        // POST: Ratings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]                                      
        public ActionResult Create([Bind(Include = "RatingId, RatingText, IsApproved, UserName, ProductId, Stars")] Rating rating, int? id)
        {
            ViewBag.Stars = new List<SelectListItem>()
                    {
                        new SelectListItem() {Text="1", Value = "1" },
                        new SelectListItem() {Text="2", Value = "2" },
                        new SelectListItem() {Text="3", Value = "3" },
                        new SelectListItem() {Text="4", Value = "4" },
                        new SelectListItem() {Text="5", Value = "5" }
                    };

            if (ModelState.IsValid)
            {
                rating.UserName = User.Identity.GetUserName();
                var currentProduct = db.Products.Where(p => p.ID == id).Select(x => x.ID).Single();
                rating.ProductId = currentProduct;
                var ratingExists = from r in db.Ratings
                                   select new
                                   {
                                       r.UserName,
                                       r.ProductId
                                   };
                foreach (var item in ratingExists)
                {
                    if (item.ProductId == currentProduct && item.UserName == User.Identity.GetUserName())
                    {
                        return RedirectToAction("RatingFail", "Ratings");
                    }
                }
                rating.ReviewCreated = DateTime.Now;
                db.Ratings.Add(rating);
                db.SaveChanges();
            }
            return RedirectToAction("RatingSuccess", "Ratings");
        }

        //
        //
        public ActionResult RatingError()
        {
            return View();
        }

        //
        //
        public ActionResult RatingSuccess()
        {
            return View();
        }

        //
        // OM: when user has already made a review for a product
        public ActionResult RatingFail()
        {
            return View();
        }

        //
        // GET: Ratings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string currentUsername = User.Identity.GetUserName(); 
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.UserName == currentUsername);
            Rating rating = await db.Ratings.FindAsync(id);

            // OM: only allow admin and user who made the review to edit the review
            if (!User.IsInRole("Admin"))
            {
                if (currentUser.UserName != rating.UserName)
                {
                    return RedirectToAction("RatingError");
                }
            }

            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        //
        // POST: Ratings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RatingId, RatingText, UserName, IsApproved, ProductId, ReviewCreated, Stars")] Rating rating, int? id)
        {
            if (ModelState.IsValid)
            {
                if (!User.IsInRole("Admin"))
                {
                    rating.IsApproved = false;
                    rating.IsEdited = true;
                }
                rating.ReviewCreated = DateTime.Now;
                db.Entry(rating).State = EntityState.Modified;
                await db.SaveChangesAsync();

                if (!User.IsInRole("Admin"))
                {
                    return RedirectToAction("RatingSuccess");
                }
                return RedirectToAction("RatingsList", "Admin");
            }
            // OM: If all else fails
            return View(rating);
        }

        // GET: Ratings/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = await db.Ratings.FindAsync(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Rating rating = await db.Ratings.FindAsync(id);
            db.Ratings.Remove(rating);
            await db.SaveChangesAsync();
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Products");
            }
            return RedirectToAction("RatingsList", "Admin");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}