using MyMemory.BLL;
using MyMemory.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyMemory.MVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private const string DATA_SESSION = "StudyData";

        private readonly MemoryManager _mng;

        public HomeController()
        {
            _mng = new MemoryManager();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Info()
        {
            ViewBag.NumberOfTasks = _mng.GetTasks().Length;
            return View();
        }
    }
}