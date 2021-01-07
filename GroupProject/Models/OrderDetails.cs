using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class OrderDetails
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string ApplicationUserID { get; set; }
        public DateTime OrderDate { get; set; }
    }
}