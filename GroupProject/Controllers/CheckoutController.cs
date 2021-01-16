using GroupProject.Data;
using GroupProject.Models;
using GroupProject.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GroupProject.Controllers
{
    public class CheckoutController : Controller
    {

        ApplicationDbContext context = new ApplicationDbContext();

        // GET: Checkout
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            var user = context.Users.Single(x => x.UserName == User.Identity.Name);
            OrderViewModel model = new OrderViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                City = user.City,
                PostalCode = user.PostalCode
            };
            return View(model);
        }

        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult Index(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var order = new Order();
            TryUpdateModel(order);
            try
            {
                order.ApplicationUserID = context.Users.Single(x => x.UserName == User.Identity.Name).Id;
                order.OrderDate = DateTime.Now;
                //Save Order
                context.Orders.Add(order);
                context.SaveChanges();
                //Process the order
                var cart = ShoppingCart.GetCart(this.HttpContext);
                cart.CreateOrder(order); //order.ID = 
                return RedirectToAction("Complete", new { id = order.ID });
            }
            catch
            {
                return View(model);
            }
        }

        [Authorize(Roles = "User")]
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = context.Orders.Any(o => o.ID == id &&
                o.ApplicationUser.UserName == User.Identity.Name);
            if (isValid)
                return View(id);
            else
                return View("Error");
        }
    }
}