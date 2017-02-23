using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyMemory.MVC.Models;
using MyMemory.Domain;
using MyMemory.BLL;
using System.Web.Security;

namespace MyMemory.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly MemoryManager _mng;

        public AccountController()
        {
            _mng = new MemoryManager();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                MemoryUser user = _mng.FindUser(model.Name);
                if (user == null)
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
                else if (user.Password != model.Password)
                {
                    ModelState.AddModelError("", "Не верный пароль");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(model.Name, true);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mng.FindUser(model.Name);
                if (user != null)
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
                else
                {
                    user = new MemoryUser()
                    {
                        Name = model.Name,
                        Password = model.Password
                    };
                    _mng.SaveUser(user);

                    user = _mng.FindUser(model.Name);
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Name, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View(model);
        }

        [Authorize]
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}