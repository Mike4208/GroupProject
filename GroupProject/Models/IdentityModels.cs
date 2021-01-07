using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GroupProject.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //[Column(TypeName = "datetime2")]
        //public DateTime Created { get; set; }
        //[Column(TypeName = "datetime2")]
        //public DateTime LastLog { get; set; }
        //[DisplayName("First Name")]
        //public string FirstName { get; set; }
        //[DisplayName("Last Name")]
        //public string LastName { get; set; }
        //public string Address { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public virtual DbSet<Products> Products { get; set; }
        //public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        //public DbSet<Cart> Carts { get; set; }
        //public DbSet<Order> Orders { get; set; }
    }
}