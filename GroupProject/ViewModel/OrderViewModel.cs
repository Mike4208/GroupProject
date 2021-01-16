using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.ViewModel
{
    public class OrderViewModel
    {
        [Required]
        [DisplayName("First Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must have more than 3 letters")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must have more than 3 letters")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Address must have more than 5 letters")]
        public string Address { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "City must have more than 3 letters")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Postal Code")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Invalid Postal Code")]
        public string PostalCode { get; set; }
    }
}