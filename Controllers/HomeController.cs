using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibSystem.Models;
using System.Data.Entity;

namespace LibSystem.Controllers
{
    public class HomeController : Controller
    {
        LibraryDBEntities1 db = new LibraryDBEntities1();

        public ActionResult Index()
        {
            return View();
        }

        

      
    }
}