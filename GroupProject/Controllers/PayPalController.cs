using GroupProject.Data;
using GroupProject.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GroupProject.Controllers
{
    public class PayPalController : Controller
    {

        readonly ApplicationDbContext db = new ApplicationDbContext(); // OM: make readonly
        //  Work with PayPal Payment
        private PayPal.Api.Payment payment;

        ////
        //// GET: PayPal
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // Create a payment using an APIContext
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            // similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            string currentUsername = User.Identity.Name;
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.UserName == currentUsername);
            var carts = db.Carts.Where(x => x.CartID == currentUsername).ToList();

            List<decimal> price = new List<decimal>();
            List<decimal> totalPrices = new List<decimal>();
            int count = 0;

            foreach (var item in carts)
            {
                price.Add(item.Product.Price);
                itemList.items.Add(new Item()
                {
                    name = item.Product.Name,
                    price = price[count].ToString(),
                    quantity = item.Quantity.ToString(),
                    currency = "EUR",
                    sku = item.ProductID.ToString()
                });
                totalPrices.Add(price[count] * item.Quantity);
                count++;
            }

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            var total = totalPrices.Sum();
            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                //tax = "1",
                //shipping = "1",
                subtotal = total.ToString()
            };
            
            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "EUR",
                total = total.ToString(), // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "sales",
                invoice_number = "l;6l",
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        public ActionResult PaymentWithPaypal()
        {
            // getting the apiContext as earlier
            APIContext apiContext = Configuration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    // this section will be executed first because PayerID doesn't exist
                    // it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    // So we have provided URL of this controller only
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Paypal/PaymentWithPayPal?";

                    // guid we are generating for storing the paymentID received in session
                    // after calling the create function and it is used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    // CreatePayment function gives us the payment approval url
                    // on which payer is redirected for paypal account payment

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    // get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters
                    // from the previous call to the function Create

                    // Executing a payment
                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                PayPalLogger.Log("Error" + ex.Message);
                return View("FailureView");
            }
            return RedirectToAction("Complete", "Checkout");
        }
    }
}