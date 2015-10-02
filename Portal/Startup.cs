using Microsoft.Owin;
using Owin;
using Portal.Models;
namespace Portal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ApplicationDbContext context = new ApplicationDbContext();
            context.Pictures.Add(new Picture { Name = "Ghoha", URL = "url.ru" });
            context.SaveChanges();
        }
    }
}
