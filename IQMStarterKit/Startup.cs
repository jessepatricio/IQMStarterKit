using IQMStarterKit.Models;
using Microsoft.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;
   

[assembly: OwinStartupAttribute(typeof(IQMStarterKit.Startup))]
namespace IQMStarterKit
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //CreateRolesUsers();

        }

        private void CreateRolesUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //// In Startup iam creating first Admin Role and creating a default Admin User    
            //if (!roleManager.RoleExists("Admin"))
            //{

            //    // first we create Admin rool   
            //    var role = new IdentityRole();
            //    role.Name = "Admin";
            //    roleManager.Create(role);

            //    //Here we create a Admin super user who will maintain the website                  

            //    //var user = new ApplicationUser();
            //    //user.UserName = "Sabrina";
            //    //user.Email = "tiz00004pk@gmail.com";

            //    //string userPWD = "A@Z200711";

            //    //var chkUser = UserManager.Create(user, userPWD);

            //    ////Add default User to Role Admin   
            //    //if (chkUser.Succeeded)
            //    //{
            //    //    var result1 = UserManager.AddToRole(user.Id, "Admin");

            //    //}
            //}

            // creating Creating Manager role    
            //if (!roleManager.RoleExists("Tutor"))
            //{
            //    var role = new IdentityRole();
            //    role.Name = "Tutor";
            //    roleManager.Create(role);

            //}

            //// creating Creating Employee role    
            //if (!roleManager.RoleExists("Student"))
            //{
            //    var role = new IdentityRole();
            //    role.Name = "Student";
            //    roleManager.Create(role);

            //}

        }
    }
}
