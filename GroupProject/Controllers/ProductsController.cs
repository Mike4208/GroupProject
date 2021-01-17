using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GroupProject.Data;
using GroupProject.Models;
using PagedList;
using PagedList.Mvc;

namespace GroupProject.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, string selectedCategory, string selectedManufacturer, int? page)
        {
            ViewBag.PageName = "Products";

            var products = db.Products.Include(p => p.Category).Include(p => p.Manufacturer); //OM: get all products
            ViewBag.IsAdminOrEmployee = User.IsInRole("Admin") || User.IsInRole("Employee"); //OM: Used to check what to hide in View depending on role

            products = products.OrderBy(x => x.Name); // OM: initial order, pagedlist must have been ordered at least once

            // OM: Searchbar
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            ViewBag.CurrentFilter = searchString; // OM: to keep searchstring in different pages
            
            // OM: filter by category
            var categories = (from c in db.Categories select c.Name).Distinct();
            ViewBag.SelectedCategory = new SelectList(categories);
            ViewBag.CurrentCategory = selectedCategory;

            // OM: filter by manufacturer
            var manufacturers = (from c in db.Manufacturers select c.Name).Distinct();
            ViewBag.SelectedManufacturer = new SelectList(manufacturers);
            ViewBag.CurrentManufacturer = selectedManufacturer;

            // OM: actually do the filtering according to the above
            if (!string.IsNullOrEmpty(selectedManufacturer) && !string.IsNullOrEmpty(selectedCategory) && !string.IsNullOrEmpty(searchString))
                products = db.Products.Where(x => x.Category.Name == selectedCategory).Where(y => y.Manufacturer.Name == selectedManufacturer).Where(s => s.Name.Contains(searchString));
            else if ((!string.IsNullOrEmpty(selectedManufacturer) && !string.IsNullOrEmpty(searchString)))
                products = db.Products.Where(y => y.Manufacturer.Name == selectedManufacturer).Where(s => s.Name.Contains(searchString));
            else if ((!string.IsNullOrEmpty(selectedCategory) && !string.IsNullOrEmpty(searchString)))
                products = db.Products.Where(x => x.Category.Name == selectedCategory).Where(s => s.Name.Contains(searchString));
            else if (!string.IsNullOrEmpty(selectedManufacturer) && !string.IsNullOrEmpty(selectedCategory))
                products = db.Products.Where(x => x.Category.Name == selectedCategory).Where(y => y.Manufacturer.Name == selectedManufacturer);
            else if (!string.IsNullOrEmpty(selectedCategory))
                products = db.Products.Where(x => x.Category.Name == selectedCategory);
            else if (!string.IsNullOrEmpty(selectedManufacturer))
                products = db.Products.Where(x => x.Manufacturer.Name == selectedManufacturer);
            else if (!string.IsNullOrEmpty(searchString))
                products = db.Products.Where(x => x.Name.Contains(searchString));

            // OM: sort by price
            ViewBag.sortParam = string.IsNullOrEmpty(sortOrder) ? "price_asc" : ""; // OM: default sort is price ascending
            ViewBag.CurrentSort = sortOrder; // OM: to keep sortorder in different pages
            if (string.IsNullOrEmpty(sortOrder) ? false : sortOrder.Equals("price_asc")) // OM: !sortOrder == "price_asc"
            {
                products = products.OrderByDescending(x => x.Price);
                ViewBag.Descending = true; // OM: to check in View and print it
            }
            else
            {
                products = products.OrderBy(x => x.Price);
                ViewBag.Descending = false; // OM: to check in View and print it
            }

            ViewBag.ItemCount = products.Count();

            // OM: pages
            int pageSize = 12;
            int pageNumber = (page ?? 1);

            return View(products.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.PageName = "Products";

            ViewBag.AddedToCart = "";
            if (!(TempData["AddedToCart"] == null))
                ViewBag.AddedToCart = TempData["AddedToCart"].ToString();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Create()
        {
            ViewBag.PageName = "Products";

            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ID", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID, Name, Description, ProductImage, Price, CategoryID, ManufacturerID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ID", "Name", product.ManufacturerID);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Edit(int? id)
        {
            ViewBag.PageName = "Products";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ID", "Name", product.ManufacturerID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID, Name, Description, ProductImage, Price, CategoryID, ManufacturerID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            ViewBag.ManufacturerID = new SelectList(db.Manufacturers, "ID", "Name", product.ManufacturerID);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin, Employee")]
        public ActionResult Delete(int? id)
        {
            ViewBag.PageName = "Products";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
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
