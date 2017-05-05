using MyMemory.BLL;
using MyMemory.Domain;
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
                var groups = _mng.GetTreeGroups();
                var list = CreateTreeGroupsForSelection(groups);

                var vm = new ItemEditVM()
                {
                    Id = item.Id,
                    Question = item.Question,
                    Answer = item.Answer,
                    GroupName = item.Group.Name,
                    GroupId = item.Group.Id,
                    ListGroups = new SelectList(list, "Id", "Name"),
                };
                return PartialView(vm);
            }
            return View("Index");
        }

        public List<object> CreateTreeGroupsForSelection(List<MemoryGroup> groups)
        {
            var list = new List<object>();

            foreach (var group in groups)
            {
                GetTreeGroupsSimple(list, group, 0);
            }

            return list;
        }

        private void GetTreeGroupsSimple(List<object> listGroups, MemoryGroup group, int step = 0)
        {
            var name = (new String('–', step) + " " + group.Name).Trim();
            var obj = new { Name = name, Id = group.Id };

            listGroups.Add(obj);

            foreach (var gr in group.Childs)
            {
                GetTreeGroupsSimple(listGroups, gr, step + 1);
            }
        }

        [HttpPost]
        public ActionResult EditItem(ItemEditVM item)
        {
            var itemDb = _mng.GetItem(item.Id);
            if (itemDb != null)
            {
                var group = _mng.GetGroup(item.GroupId);

                itemDb.Question = item.Question;
                itemDb.Answer = item.Answer;
                itemDb.Group = group;

                _mng.SaveItem(itemDb);
            };

            return null;
        }

        public ActionResult Info()
        {
            ViewBag.NumberOfTasks = _mng.GetTasks().Length;
            return View();
        }
    }
}