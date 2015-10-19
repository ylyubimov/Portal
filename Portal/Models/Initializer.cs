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


            Picture p1 = new Picture { Name = "ava1", URL = "http://mobini.pl/upload/files/127/095/114/902/672/728_peter-parker.jpg" };
            Picture p2 = new Picture { Name = "ava2", URL = "http://rebarron.com/wp-content/uploads/2013/06/Angry-yelling-man.jpg" };
            Picture p3 = new Picture { Name = "ava3", URL = "http://1.fwcdn.pl/ph/87/13/138713/98628.1.jpg" };
            Picture p4 = new Picture { Name = "ava1", URL = "http://s019.radikal.ru/i626/1501/ad/9c9a041ff700.jpg" };
            Picture p5 = new Picture { Name = "ava2", URL = "http://fs00.infourok.ru/images/doc/148/171148/hello_html_33f0bc3e.jpg" };
            Picture p6 = new Picture { Name = "ava3", URL = "http://www.yaom.ru/wp-content/uploads/will-smith.jpeg" };

            p1 = db.Picture.Add(p1);
            p2 = db.Picture.Add(p2);
            p3 = db.Picture.Add(p3);
            p4 = db.Picture.Add(p4);
            p5 = db.Picture.Add(p5);
            p6 = db.Picture.Add(p6);
            db.SaveChanges();

            // Добавление студентов
            List<Student> students = new List<Student>
            {
                new Student { Email = "qr@yandex.ru", Password = "qwerty", First_Name = "Gosha", Second_Name ="Kuzenko", Middle_Name = "Sergeevich", Group = group1, Faculty = fivt, Base_Company = basefivt, Base_Part = base_part_abbyy1, Year_of_Graduating = 2015, Picture = p1  },
                new Student { Email = "qrd@yandex.ru", Password = "qwerty", First_Name = "Grisha", Second_Name ="Yakovlev", Middle_Name = "Sergeevich1", Group = group1, Faculty = fivt, Base_Company = basefivt, Base_Part = base_part_abbyy2 , Picture = p2 },
                new Student { Email = "qra@yandex.ru", Password = "qwerty", First_Name = "Dmitriy", Second_Name ="Kozhoma", Middle_Name = "Vasilevich", Group = group2, Faculty = fupm, Base_Company = basefupm, Picture = p3 },
            };
            foreach (Student s in students)
                db.Student.Add(s);
            
            db.SaveChanges();
            
            foreach( Person p in db.Person)
            {
                if (p is Person)
                {
                    Student s = new Student();
                }
                //s = (Student)p;
               // Console.Write(s.Group.Name);
            }

            // Добавление учителей
           
            List <Teacher> teachers = new List<Teacher>
            {
                new Teacher { Email = "qrad@yandex.ru", Password = "qwerty", First_Name = "Uriy", Second_Name = "Lushkov", Middle_Name = "Urevich", Base_Company = basefivt, Picture = p4   },
                new Teacher { Email = "qrasd@yandex.ru", Password = "qwerty", First_Name = "Sergey", Second_Name = "Lavrov", Middle_Name = "Michailovich", Base_Company = basefupm, Picture = p4 },
                new Teacher { Email = "qradz@yandex.ru", Password = "qwerty", First_Name = "Konstantin", Second_Name = "Kobalt", Middle_Name = "Borisovich", Base_Company = basefivt, Picture = p6 },
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
            {
                authors.Add(p);
            }
                
            

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
                new Article { Name = "Глобус", Text = "Глобус - это уменьшенная копия Земли. Крутится, вертится шар голубой." +
                    "Крутится, вертится над головой… С момента, когда ученые доказали, что земля имеет форму шара, а не стоит на" +
                    "китах или слонах, прошло много лет. Человек за это время успел слетать в космос, а типографии – напечатать несколько" +
                    "миллионов экземпляров карт мира для изучения географии. Но гораздо большим спросом у школьников все же пользуется глобус." + 
                    "Он и выглядит эффектнее, и материки на нем смотрятся более натурально. Самым оригинальным все же считается изделие от Andy " +
                    "Yoder из разноцветных спичек.", Likes_Count = 0, Dislikes_Count = 0, Authors = authors, Date_of_Creation = DateTime.Parse("2015-09-10")  },
                new Article { Name = "Книга", Text = "один из видов печатной продукции: непериодическое издание, состоящее из сброшюрованных или отдельных бумажных листов (страниц) или тетрадей, на которых нанесена типографским или рукописным способом текстовая и графическая (иллюстрации) информация, имеющая объём более сорока восьми страниц и, как правило, твёрдый переплёт." +
                    "Также книгой может называться литературное или научное произведение, предназначенное для печати в виде отдельного сброшюрованного издания[2]." +
                    "Современные детские книги-картинки могут иметь нетрадиционную форму и быть представлены в виде отдельных листов или карточек с иллюстрациями и заданиями. Листы или карточки должны быть собраны вместе с помощью внешнего элемента (коробки, кольца, папки, суперобложки, зажима). При этом листы и карточки могут быть как скреплены между собой так и идти отдельно[3].",
                    Likes_Count = 0, Dislikes_Count = 0, Authors = authors, Date_of_Creation = DateTime.Parse("2015-09-10") },
                new Article { Name = "Ножницы", Text = "Ножницы - это инcтрумент для ручного разрезания материи. Самые древние ножницы, найденные археологами на современной территории Древнего Рима, имеют возраст 3-4 тысяч лет и были предназначены для стрижки овец. Эти ножницы больше походили на небольшой пинцет с двумя тупыми лезвиями на обоих концах. Конструкция просуществовала более двух тысяч лет без принципиальных изменений. Рычаг в ножницах стал использоваться около тысячи лет назад.",
                    Likes_Count = 0, Dislikes_Count = 0, Authors = authors, Date_of_Creation = DateTime.Parse("2015-09-10") },
                new Article { Name = "Клюква", Text = "Клюква – название растения из семейства брусничных и его плодов.Клюква представляет собой вечнозелёное растение, кустарник с тонкими и невысокими побегами. Длина побегов в среднем около 30 см, ягоды дикой клюквы красные, шаровидные, 8-12 мм в диаметре. Некоторые специально выведенные сорта имеют ягоды до 2 см в диаметре. Цветёт клюква в июне, сбор ягод начинается в сентябре и продолжается всю осень. Плантационные ягоды созревают на 1-2 недели раньше диких. Ягоды клюквы легко могут сохраняться до весны.Клюкву употребляют в пищу как в свежем, так и в замороженном, моченном или сушеном виде, из нее готовят соки, морсы, желе, варенья, кисели, коктейли и клюквенный квас, или добавляют в салаты, пироги и другие блюда. Клюква, растёт во многих странах, если позволяют условия, любит болотистую лесную почву, осоко-сфагновые болота, тундровые и моховые болота. Только в Карелии растёт около 22 сортов клюквы, среди которых встречаются крупноплодные сорта с диаметром ягод до 2 см. Сегодня клюкву можно встретить на всей территории России, включая Дальний Восток. Богаты клюквой Украина, большая часть Европы, Север США, Канада и Аляска. Американцы считают родиной клюквы Северную Америку. Индейцы-делавэры считали, что ягоды росли на земле, где пролилась кровь воинов, погибших в битве с великанами.",
                    Likes_Count = 0, Dislikes_Count = 0, Authors = authors, Date_of_Creation = DateTime.Parse("2015-07-10") },
                new Article { Name = "Витамин A",  Text = "Антиинфекционный витамин, антиксерофтальмический витамин, ретинол, дегидроретинол Витамин А включает значительное число жирорастворимых соединений, важнейшими среди которых являются ретинол, ретиналь, ретиноевая кислота и эфиры ретинола. Витамин А выполняет множество функций в организме: способствует росту и регенерации тканей, обеспечивает эластичность кожи и волос.Оказывает антиоксидантное действие, повышает иммунитет, усиливает сопротивляемость организма к инфекциям. Витамин А нормализует деятельность половых желез, необходим для образования спермы и развития яйцеклетки. Одна из важных функций витамина А -предотвращение куриной слепоты -гемералопатия(нарушение сумеречного зрения).",
                     Likes_Count = 0, Dislikes_Count = 0, Authors = authors, Date_of_Creation = DateTime.Parse("2015-05-10") }
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

            // Добавление блогов
            Blog b1 = db.Blog.Add(new Blog { Name = "PPS_Blog", Articles = NewArticles, Authors = authors });
            Blog b2 = db.Blog.Add(new Blog { Name = "Funkan_Blog", Articles = NewArticles, Authors = authors });
            db.SaveChanges();


            // Добавление курсов
            List<Course> courses = new List<Course>
            {
                new Course { Name = "PPS", Teachers = NewTeachers, Students = NewStudents, Blogs = new List<Blog> { b1 }, Description = "В данном курсе даются основы проектирования систем по принципу ООП", Number_of_Classes = 12, Number_of_Hours = 48 },
                new Course { Name = "Funkan", Teachers = NewTeachers, Students = NewStudents, Blogs = new List<Blog> { b2 }, Description = "Данных курс расширяет понятия используемые в матанализе, тем самым усиливая математический аппарат", Number_of_Hours = 64, Number_of_Classes = 16  },
            };
            List<Course> NewCourses = new List<Course>();
            foreach (Course c in courses)
                NewCourses.Add(db.Course.Add(c) );
            db.SaveChanges();
            ////

            // Добавление программы
            db.Program.Add(new Program { Name = "PMF", Courses = NewCourses });
            base.Seed(db);

        }
    }
}