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
        public int ApplicationUserID { get; set; }
        public virtual ICollection<OrderProducts> OrderProducts { get; set; }
        public decimal Price { get; set; }
        [DisplayName("Order Date")]
        public DateTime OrderDate { get; set; }
    }
}