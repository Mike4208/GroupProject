using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupProject.ViewModel
{
    public class UserView
    {
        public string UserId { get; set; }
        [Required]
        [DisplayName("User Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must have more than 3 letters")]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(50, MinimumLength = 7, ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        [DisplayName("User Roles")]
        public string UserRoles { get; set; }
        [DisplayName("User Created")]
        public DateTime? Created { get; set; }
        [DisplayName("Last Login")]
        public DateTime? LastLogin { get; set; }
        [DisplayName("First Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must have more than 3 letters")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must have more than 3 letters")]
        public string LastName { get; set; }
    }
}