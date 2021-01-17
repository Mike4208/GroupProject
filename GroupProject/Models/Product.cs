using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class Product
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [StringLength(1024)]
        public string ProductImage { get; set; }
        [Required]
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public bool Offer { get; set; }
        public decimal? Discount { get; set; }
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public int ManufacturerID { get; set; }
        public virtual Manufacturer Manufacturer {get;set;}
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}