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
        public virtual ApplicationUser ApplicationUser { get; set; }
        public decimal TotalPrice { get; set; }
        [DisplayName("Order Date")]
        public DateTime OrderDate { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}