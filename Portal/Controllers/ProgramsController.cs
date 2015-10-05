using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ProgramsController : Controller
    {
    	private Models.Program[] getPrograms()
        {
            // TODO временная заглушка
            Models.Program[] programs = new Models.Program[7];
            for (int i = 0; i < 7; ++i)
            {
                programs[i] = new Models.Program(i);
            }

            return programs;
        }

        public ActionResult Index()
        {
            return View(getPrograms());
        }

        //
        // GET: /Courses/
        public ActionResult Program(int? id)
        {
            if (id != null)
            {
            	return View(new Models.Program((int) id));
            }
            else 
            {
            	return View("Index");	
            }
        }
	}
}