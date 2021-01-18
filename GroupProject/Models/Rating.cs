using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        public string UserName { get; set; }
        public string RatingText { get; set; }
        public bool IsApproved { get; set; }
        public bool IsEdited { get; set; }
        public double Stars { get; set; }
        public DateTime ReviewCreated { get; set; }
        public int ProductId { get; set; }
        //public string Id { get; set; }
        public virtual Product Products { get; set; }
        //public virtual ApplicationUser ApplicationUser { get; set; }

    }
}