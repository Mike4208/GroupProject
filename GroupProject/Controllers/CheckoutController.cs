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

        readonly ApplicationDbContext context = new ApplicationDbContext(); // OM: make readonly

        //
        // GET: /Checkout
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            ViewBag.PageName = "Cart";
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewBag.NoOrders = false;
            if (cart.GetCartItems().Count == 0)
            {
                ViewBag.NoOrders = true;
                return View();
            }

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
        // POST: /Checkout
        [HttpPost]
        public ActionResult Index(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.NoOrders = false; // OM: because GET /Checkout uses this Viewbag bool for a check, so it cant be null
                return View(model);
            }

            var order = new Order();
            TryUpdateModel(order);
            order.UserName = User.Identity.GetUserName();
            try
            {
                order.OrderDate = DateTime.Now;
                var cart = ShoppingCart.GetCart(this.HttpContext);
                order.TotalPrice = cart.GetTotal();
                //Save Order
                //context.Orders.Add(order);
                //context.SaveChanges();
                //Process the order
                //order.ID = cart.CreateOrder(order);

                // OM: Pass order info to PayPal payment and add the order only after payment has gone through
                TempData["Order"] = order;
                return RedirectToAction("PaymentWithPaypal", "PayPal");

                //return RedirectToAction("Complete", new { id = order.ID });
            }
            catch
            {
                ViewBag.NoOrders = false; // OM: because GET: /Checkout uses this Viewbag bool for a check, so it cant be null
                return View(model);
            }
        }

        [Authorize(Roles = "User")]
        public ActionResult Complete(int? id)
        {
            if (id == null)
                return View("Error");

            ViewBag.PageName = "Cart";

            // Validate customer owns this order
            bool isValid = context.Orders.Any(
                o => o.ID == id &&
                o.UserName == User.Identity.Name);

            if (isValid)
                return View(id);
            else
                return View("Error");
        }
    }
}