using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class Rating
    {
        public int RatingID { get; set; }
        public string RatingText { get; set; }
        public bool IsApproved { get; set; }
        public bool IsEdited { get; set; }
        public DateTime ReviewCreated { get; set; }
        public string Id { get; set; }
        public int ProductID { get; set; }
        public virtual Product Products { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}