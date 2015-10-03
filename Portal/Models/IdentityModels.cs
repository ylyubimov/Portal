using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Portal.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Picture> Picture { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<Presentation> Presentation { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Program> Program { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Faculty> Faculty{ get; set; }
        public DbSet<Base_Company> Base_Company { get; set; }
        public DbSet<Base_Part> Base_Part { get; set; }

    }
}

    