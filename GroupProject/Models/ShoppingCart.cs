using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class ShoppingCart
    {
        public int ID { get; set; }
        public string ApplicationUserID { get; set;}
        public ApplicationUser ApplicationUser { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [NotMapped]
        public decimal Price { get; set; }
        [Range(1, 100, ErrorMessage = "Invalid Quantity selected")]
        public int Quantity { get; set; }
    }
}