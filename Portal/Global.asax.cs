﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Portal.Models;
using System.Data.Entity;
namespace Portal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //// Инициализация для создания базы
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
            using (var db = new ApplicationDbContext())
            {
                if (!db.Database.Exists())
                {
                    db.Database.Initialize(false);
                }
            }
            ////
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
