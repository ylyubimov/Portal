using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Portal.Models
{
    public class PortalDataBaseInitializer : DropCreateDatabaseAlways<ApplicationDbContext> 
    {
        protected override void Seed(ApplicationDbContext context)
        {
            context.Pictures.Add(new Picture { ID = 1, Name = "Ghoha", URL = "url.ru" } );
            context.SaveChanges();

        }
    }
}