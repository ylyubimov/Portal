using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Portal.Models
{
    public class Initializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext db)
        {
            // Добавление Факультетов
            Faculty fivt = db.Faculty.Add(new Faculty { Name = "Fivt" });
            Faculty fupm = db.Faculty.Add(new Faculty { Name = "Fupm" });
            db.SaveChanges();
            // Добавление базовых кафедр
            Base_Company basefivt = db.Base_Company.Add(new Base_Company { Name = "Abbyy", Faculty = fivt });
            Base_Company basefupm = db.Base_Company.Add(new Base_Company { Name = "Data processing", Faculty = fupm });
            db.SaveChanges();
            // Добавление подразделений базовых кафедр
            Base_Part base_part_abbyy1 = db.Base_Part.Add(new Base_Part { Name = "RIOT", Base_Сompany = basefivt });
            Base_Part base_part_abbyy2 = db.Base_Part.Add(new Base_Part { Name = "RIOT", Base_Сompany = basefivt });
            db.SaveChanges();
            // Добавление групп
            Group group1 = db.Group.Add(new Group { Name = "292", Faculty = fivt });
            Group group2 = db.Group.Add(new Group { Name = "271" , Faculty = fupm});


            // Добавление студентов
            List<Student> students = new List<Student>
            {
                new Student { Email = "qr@yandex.ru", Password = "qwerty", First_Name = "Gosha", Second_Name ="Kuzenko", Middle_Name = "Sergeevich", Group = group1, Faculty = fivt, Base_Company = basefivt, Base_Part = base_part_abbyy1  },
                new Student { Email = "qrd@yandex.ru", Password = "qwerty", First_Name = "Gosha1", Second_Name ="Kuzenko1", Middle_Name = "Sergeevich1", Group = group1, Faculty = fivt, Base_Company = basefivt, Base_Part = base_part_abbyy2  },
                new Student { Email = "qra@yandex.ru", Password = "qwerty", First_Name = "Gosha2", Second_Name ="Kuzenko2", Middle_Name = "Sergeevich2", Group = group2, Faculty = fupm, Base_Company = basefupm },
            };
            foreach (Student s in students)
                db.Student.Add(s);
            
            db.SaveChanges();

            // Добавление учителей
            List<Teacher> teachers = new List<Teacher>
            {
                new Teacher { Email = "qrad@yandex.ru", Password = "qwerty", First_Name = "Ura", Second_Name = "Lushkov", Middle_Name = "Urevich", Base_Company = basefivt },
                new Teacher { Email = "qrasd@yandex.ru", Password = "qwerty", First_Name = "Ura1", Second_Name = "Lushkov1", Middle_Name = "Urevich1", Base_Company = basefupm },
                new Teacher { Email = "qradz@yandex.ru", Password = "qwerty", First_Name = "Ura2", Second_Name = "Lushkov2", Middle_Name = "Urevich2", Base_Company = basefivt },
            };
            foreach (Teacher t in teachers)
            {
                db.Teacher.Add(t);
            }
            db.SaveChanges();
            ////
            
            // Получение сущностей с ID
            List<Person> authors = new List<Person>(); 
            foreach ( Teacher p in db.Teacher)
                authors.Add(p);

            List<Teacher> NewTeachers = new List<Teacher>();
            foreach (Teacher p in db.Teacher)
                NewTeachers.Add(p);

            List<Student> NewStudents = new List<Student>();
            foreach ( Student p in db.Student)
                NewStudents.Add(p);
            ////
            
            // Добавление статей

            List<Article> articles = new List<Article>
            {
                new Article { Name = "Глобус", Text = "Глобус - это уменьшенная копия Земли", Likes_Count = 0, Dislikes_Count = 0, Authors = authors, Date_of_Creation = DateTime.Parse("2015-09-10")  },
                new Article { Name = "Книга", Text = "Книга - это печатный материал", Likes_Count = 0, Dislikes_Count = 0, Authors = authors, Date_of_Creation = DateTime.Parse("2015-09-10") },
                new Article { Name = "Ножницы", Text = "Ножницы - это инcтрумент для ручного разрезания материи", Likes_Count = 0, Dislikes_Count = 0, Authors = authors, Date_of_Creation = DateTime.Parse("2015-09-10") },
            };
            foreach (Article a in articles)
                db.Article.Add(a);

            db.SaveChanges();
            ////

            // Получение Articles c ID 
            List<Article> NewArticles = new List<Article>();
            foreach (Article a in db.Article)
                NewArticles.Add(a);
            ////
            
            // Добавление курсов
            List<Course> courses = new List<Course>
            {
                new Course { Name = "PPS", Teachers = NewTeachers, Students = NewStudents },
                new Course { Name = "Funkan", Teachers = NewTeachers, Students = NewStudents }
            };
            List<Course> NewCourses = new List<Course>();
            foreach (Course c in courses)
                NewCourses.Add(db.Course.Add(c) );
            
            db.SaveChanges();
            ////

            // Добавление программы
            db.Program.Add(new Program { Name = "PMF", Courses = NewCourses });
            ////    

            // Добавление блогов
            db.Blog.Add(new Blog { Name = "PPS_Blog", Articles = NewArticles, Authors = authors });
            
            
            db.Blog.Add(new Blog { Name = "Funkan_Blog", Articles = NewArticles, Authors = authors });
            db.SaveChanges();

            ////
            base.Seed(db);

        }
    }
}