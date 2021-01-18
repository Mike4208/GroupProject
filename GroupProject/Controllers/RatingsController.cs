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

        public ActionResult Details(int? id)
        {
            var productswithreviews = (from r in db.Ratings
                                       join p in db.Products on r.ProductId equals p.ID
                                       join u in db.Users on r.Id equals u.Id

                                       select new
                                       {
                                           ratingid = r.RatingId,
                                           productid = p.ID,
                                           id = u.Id,
                                           ratigttext = r.RatingText,
                                           username = u.UserName,
                                           name = p.Name,
                                           approved = r.IsApproved,
                                           date = r.ReviewCreated,
                                           stars = r.Stars,


                                       }).ToList().Select(p => new RatingViewModel()

                                       {
                                           RatingID = p.ratingid,
                                           ProductID = p.productid,
                                           ID = p.id,
                                           RatingText = p.ratigttext,
                                           Username = p.username,
                                           Name = p.name,
                                           IsApproved = p.approved,
                                           ReviewCreated = p.date,
                                           Stars = (int)p.stars

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


        // GET: Ratings
        public async Task<ActionResult> Index()
        {
            return View(await db.Ratings.ToListAsync());
        }


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


        // POST: Ratings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RatingId,RatingText,IsApproved, ApplicationUser, ProductId, Stars")] Rating rating, int? id)
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

                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                rating.ApplicationUser = currentUser;
                var currentProduct = db.Products.Where(p => p.ID == id).Select(x => x.ID).Single();
                rating.ProductId = currentProduct;
                var ratingExists = from r in db.Ratings
                                   select new
                                   {
                                       r.Id,
                                       r.ProductId
                                   };
                foreach (var item in ratingExists)
                {
                    if (item.ProductId == currentProduct && item.Id == currentUser.Id)
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

        public ActionResult RatingError()
        {
            return View();
        }

        public ActionResult RatingSuccess()
        {
            return View();
        }

        public ActionResult RatingFail()
        {
            return View();
        }

        // GET: Ratings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

            Rating rating = await db.Ratings.FindAsync(id);
            if (!User.IsInRole("Admin"))
            {
                if (currentUser.Id != rating.Id)
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

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost/*, Route("/{username:string}")*/]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RatingId,RatingText,IsApproved,ProductId,Id")] Rating rating, int? id, string randomNo)
        {

            if (ModelState.IsValid)
            {
                var currentStar = db.Ratings.Where(x => x.RatingId == id).Select(i => i.Stars).SingleOrDefault();
                var currentPid = rating.ProductId;
                var currentUser = User.Identity.GetUserId();
                if (!User.IsInRole("Admin"))
                {
                    rating.IsApproved = false;
                    rating.IsEdited = true;
                    rating.Id = currentUser;
                }
                rating.Id = randomNo;
                rating.ProductId = currentPid;
                rating.Stars = currentStar;
                rating.ReviewCreated = DateTime.Now;
                db.Entry(rating).State = EntityState.Modified;
                await db.SaveChangesAsync();
                if (!User.IsInRole("Admin"))
                {
                    return RedirectToAction("RatingSuccess");
                }
                return RedirectToAction("RatingsList", "Admin");
            }
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