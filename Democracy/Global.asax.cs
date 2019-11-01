using Democracy.Context;
using Democracy.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Democracy
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Automatic Migrations Enable
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<Context.DemocracyContext, Migrations.Configuration>());

            // Check SuperUser
            this.CheckSuperUser();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Check SuperUser, create it if not exists
        /// </summary>
        private void CheckSuperUser()
        {
            var userContext = new ApplicationDbContext();
            DemocracyContext db = new DemocracyContext();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            string roleNameAdmin = "Admin";
            string roleNameUser = "User";

            // Check if Admin and User roles exist
            this.CheckRole(roleNameAdmin, userContext);
            this.CheckRole(roleNameUser, userContext);

            // Check if SuperUser already exists in User Model
            var user = db.Users
                .Where(u => u.UserName.ToLower().Equals("sevann.radhak@gmail.com")).FirstOrDefault();

            if(user == null )
            {
                // Create and save the record
                user = new User
                {
                    Address = "Buenos Aires",
                    FirstName = "Sevann",
                    LastName = "Radhak",
                    Phone = "(54 9)11 73627795",
                    UserName = "sevann.radhak@gmail.com"
                };

                db.Users.Add(user);
                db.SaveChanges();
            }

            // Check if SupperUser already exists in ASPUser 
            var userASP = userManager.FindByName(user.UserName);

            if(userASP == null)
            {
                // Create the ASP User
                userASP = new ApplicationUser
                {
                    UserName = user.UserName,
                    Email = user.UserName,
                    PhoneNumber = user.Phone
                };

                userManager.Create(userASP, "Sevann123.");
            }

            // Asign role Admin
            userManager.AddToRole(userASP.Id, roleNameAdmin);
            userManager.AddToRole(userASP.Id, roleNameUser);
        }

        /// <summary>
        /// Check RoleName, create it if not exists
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="userContext"></param>
        private void CheckRole(string roleName, ApplicationDbContext userContext)
        {
            // User management
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));

            // Check if Role Exists, if not create it
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }
        }
    }
}
