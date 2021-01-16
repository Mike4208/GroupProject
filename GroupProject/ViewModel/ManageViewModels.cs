using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace GroupProject.Models
{
    public class IndexViewModel
    {
        //public bool HasPassword { get; set; }
        //public IList<UserLoginInfo> Logins { get; set; }
        //public string PhoneNumber { get; set; }
        //public bool TwoFactor { get; set; }
        //public bool BrowserRemembered { get; set; }

        // OM for current user manager

        //public string UserId { get; set; }

        [Required]
        [DisplayName("User Name")]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 6)]
        [EmailAddress]

        public string Email { get; set; }

        //[DisplayName("User Roles")]
        //public string UserRoles { get; set; }

        [DisplayName("Date Created")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Created { get; set; }
        [DisplayName("Last Login")]
        public DateTime? LastLogin { get; set; }
        //[Required]
        [DisplayName("First Name")]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }
        //[Required]
        [DisplayName("Last Name")]
        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }
        [StringLength(50, MinimumLength = 5)]
        public string Address { get; set; }
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "City must have more than 3 letters")]
        public string City { get; set; }
        //[StringLength(5, MinimumLength = 5, ErrorMessage = "Invalid Postal Code")]
        public string PostalCode { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}