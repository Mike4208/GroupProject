using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GroupProject.ViewModel
{
    public class UserView
    {
        public string UserId { get; set; }
        [DisplayName("User Name")]
        public string Username { get; set; }
        public string Email { get; set; }
        [DisplayName("User Roles")]
        public string UserRoles { get; set; }

        [DisplayName("User Created")]
        public DateTime? Created { get; set; }
        [DisplayName("Last Login")]
        public DateTime? LastLogin { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
    }
}