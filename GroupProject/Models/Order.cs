using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class Order
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        [DisplayName("Total Price")]
        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [DisplayName("Order Date")]
        public DateTime OrderDate { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}