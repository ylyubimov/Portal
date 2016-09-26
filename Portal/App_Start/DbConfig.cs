using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Portal.Models;

namespace Portal
{
    public class DbConfig
    {
        public static void ConfigDataBase()
        {
            var db = new ApplicationDbContext();
            db.Database.Initialize( false );
        }
    }
}