using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using PEDFAM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace PEDFAM
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            //CreateRoles();
           
        }
        private void CreateRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            if (!roleManager.RoleExists("Administrador"))
            {
                var role = new IdentityRole("Administrador");
                roleManager.Create(role);
              
            }

            if (!roleManager.RoleExists("Usuario"))
            {
                var role = new IdentityRole("Usuario");
                roleManager.Create(role);
            }
        }
    }
}
