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
            _mngStudy = new StudyManager()
            {
                IsRandom = true
            };
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

        public JsonResult StartStudy(int groupId)
        {
            var userName = this.User.Identity.Name;
            var data = _mngStudy.Start(userName, groupId);
            SaveStudyData(data);
            return Json(data);
        }

        public JsonResult NextStep(string answer)
        {
            var data = LoadStudyData();
            var data2 = _mngStudy.NextStep(data, answer);
            SaveStudyData(data2);
            return Json(data2);
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