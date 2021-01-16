using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class Order
    {
        public int ID { get; set; }
        public string ApplicationUserID { get; set; }
        public decimal TotalPrice { get; set; }
        //public string Username { get; set; }
        //public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        [DisplayName("Order Date")]
        public DateTime OrderDate { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}