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

        public JsonResult FirstQuestion()
        {
            var data = new StudyData
            {
                CurrentQuestion = new StudyQuestion { Text = "Кто написал Обломова?" }
            };
            return Json(data);
        }

        public JsonResult NextQuestion(string answer)
        {
            var data = new StudyData
            {
                CurrentQuestion = new StudyQuestion { Text = "Имя Обломова?" },
                PrevAnswer = new StudyAnswer {  CorrectAnswer = "Гончаров", IsCorrectAnswer = true }
            };
            return Json(data);
        }

    }
}