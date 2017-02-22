using MyMemory.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMemory.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly MemoryManager _mng;

        public HomeController()
        {
            _mng = new MemoryManager();
        }

        public ActionResult Index()
        {
            var list = _mng.GetItems();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}