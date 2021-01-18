using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GroupProject.Models
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        public string RatingText { get; set; }
        public bool IsApproved { get; set; }
        public bool IsEdited { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ReviewCreated { get; set; }
        public string Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product Products { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public double Stars { get; set; }

    }
}