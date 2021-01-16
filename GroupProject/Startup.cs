using GroupProject.Data;
using GroupProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GroupProject.Startup))]
namespace GroupProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers(); // OM: new method to create roles and admin user
        }

        // In this method we will create default User roles and Admin user for login    
        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("Admin"))
            {
                // first we create Admin rool 
                var role = new IdentityRole() { Name = "Admin" };
                //role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                   

                var user = new ApplicationUser() { UserName = "admin", Email = "admin@admin.com", FirstName = "Uperoxos", LastName = "Glukoulhs" };
                //user.UserName = "admin";
                //user.Email = "admin@admin.com";
                //user.FirstName = "Uperoxos";
                //user.LastName = "Glukoulhs";
                //user.Address = "Spiti mou";

                string userPWD = "admin123"; // Needs to be more than 6 characters. Check account controller for additions need on scafolding for this to work.

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Admin"); //var result1 = 
                }
            }

            // creating Employee role     
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new IdentityRole() { Name = "Employee" };
                //role.Name = "Employee";
                roleManager.Create(role);
            }

            // creating Customer role
            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole() { Name = "User" };
                //role.Name = "User";
                roleManager.Create(role);
            }
        }
    }
}
