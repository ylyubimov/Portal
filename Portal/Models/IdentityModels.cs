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
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Portal.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationUserManager : UserManager<Person>
    {
        public ApplicationUserManager(IUserStore<Person> store)
                : base(store)
        {
            UserValidator = new UserValidator<Person>(this) { 
                AllowOnlyAlphanumericUserNames = false,
            }; 
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new Initializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Entity<Course>()
            .HasMany(c => c.Students)
            .WithMany(p => p.Subscribed_Courses)
            .Map(m =>
            {
                // Ссылка на промежуточную таблицу
                m.ToTable("StudentsCourses");

                // Настройка внешних ключей промежуточной таблицы
                m.MapLeftKey("StudentId");
                m.MapRightKey("CourseId");
            });
            modelBuilder.Entity<Course>()
            .HasMany(c => c.Teachers)
            .WithMany(p => p.Taught_Courses)
            .Map(m =>
            {
                   // Ссылка на промежуточную таблицу
                   m.ToTable("TeachersCourses");

                   // Настройка внешних ключей промежуточной таблицы
                   m.MapLeftKey("TeacherId");
                m.MapRightKey("CourseId");
            });
            modelBuilder.Entity<Program>()
            .HasMany(c => c.Students)
            .WithMany(p => p.Subscribed_Programs)
            .Map(m =>
            {
                // Ссылка на промежуточную таблицу
                m.ToTable("StudentsPrograms");

                // Настройка внешних ключей промежуточной таблицы
                m.MapLeftKey("StudentId");
                m.MapRightKey("ProgramId");
            });
            modelBuilder.Entity<Program>()
            .HasMany(c => c.Teachers)
            .WithMany(p => p.Taught_Programs)
            .Map(m =>
            {
                // Ссылка на промежуточную таблицу
                m.ToTable("TeachersPrograms");
                // Настройка внешних ключей промежуточной таблицы
                m.MapLeftKey("TeacherId");
                m.MapRightKey("ProgramId");
            });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Picture> Picture { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Program> Program { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Faculty> Faculty{ get; set; }
        public DbSet<Base_Company> Base_Company { get; set; }
        public DbSet<Base_Part> Base_Part { get; set; }
    }
}

    