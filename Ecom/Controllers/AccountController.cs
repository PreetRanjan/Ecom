using Ecom.Data;
using Ecom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Ecom.Controllers
{
    public class AccountController : Controller
    {
        private EcomContext _db;
        public AccountController()
        {
            _db = new EcomContext();
        }
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }
        // POST: Account creation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                //Check if user exists
                var isUserExists = _db.AppUsers.Any(x => x.Email == model.Email);
                if (isUserExists)
                {
                    ModelState.AddModelError("Duplicate Email", "Email is already registered with us.");
                    return View(model);
                }
                //Register user
                var newUser = new AppUser()
                {
                    FullName = model.Fullname,
                    Email = model.Email,
                    MobileNo = model.MobileNo,
                    UserPassword=model.Password
                };
                _db.AppUsers.Add(newUser);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(model);
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
                var isValidUser = _db.AppUsers.Any(x => x.Email == model.Username && x.UserPassword == model.Password);
                if (isValidUser)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("Invalid Login", "Username/Password is incorrect");
            return View(model);
        }
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}