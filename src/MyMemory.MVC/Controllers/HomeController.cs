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
        private readonly StudyManager _mngStudy;

        public HomeController()
        {
            _mng = new MemoryManager();
            _mngStudy = new StudyManager();
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult NextStep(string answer)
        {
            var data = _mngStudy.NextStep(null, null);

            return Json(data);
        }

        private StudyData LoadStudyData()
        {
            return Session[DATA_SESSION] as StudyData;
        }

        private void SaveStudyData(StudyData data)
        {
            Session[DATA_SESSION] = data;
        }
    }
}