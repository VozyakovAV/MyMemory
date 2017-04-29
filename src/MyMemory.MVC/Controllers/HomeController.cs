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
            var groups = _mng.GetGroups();
            var model = new HomeVM();
            
            foreach (var group in groups)
            {
                model.Groups.Add(new GroupVM()
                {
                    Id = group.Id,
                    Name = group.Name,
                });
            }

            return View(model);
        }

        public ActionResult EditGroup(int id)
        {
            var list = new List<ItemVM>();

            var group = _mng.GetGroup(id);

            foreach (var item in group.Items)
            {
                list.Add(new ItemVM()
                {
                    Id = item.Id,
                    Question = item.Question,
                    Answer = item.Answer
                });
            }

            var model = new GroupEditVM()
            {
                Id = id,
                Name = group.Name,
                Items = list.ToArray(),
            };
            
            return View(model);
        }

        public ActionResult EditItem(int id)
        {
            var item = _mng.GetItem(id);
            if (item != null)
            {
                var vm = new ItemVM()
                {
                    Id = item.Id,
                    Question = item.Question,
                    Answer = item.Answer
                };
                return PartialView(vm);
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult EditItem(ItemVM item)
        {
            //var project = mng.Projects.EditProject(projectPattern);
            //return Json(project != null);
            return null;
        }

        public ActionResult Info()
        {
            ViewBag.NumberOfTasks = _mng.GetTasks().Length;
            return View();
        }
    }
}