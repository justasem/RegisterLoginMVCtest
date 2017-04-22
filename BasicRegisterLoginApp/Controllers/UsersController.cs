using BasicRegisterLoginApp.DAL;
using BasicRegisterLoginApp.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BasicRegisterLoginApp.Controllers
{
    public class UsersController : Controller
    {
        private Context context = new Context();

        public ViewResult Index(SortFilterPageParameters parameters)
        {
            ViewBag.CurrentSort = parameters.sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(parameters.sortOrder) ? "name_desc" : "";
            ViewBag.EmailSortParm = parameters.sortOrder == "Email" ? "email_desc" : "Email";

            if (parameters.searchNameString != null)
            {
                parameters.page = 1;
            }
            else
            {
                parameters.searchNameString = parameters.currentFilter;
            }

            ViewBag.CurrentFilter = parameters.searchNameString;

            if (parameters.searchEmailString != null)
            {
                parameters.page = 1;
            }
            else
            {
                parameters.searchEmailString = parameters.currentFilter;
            }

            ViewBag.CurrentFilter = parameters.searchEmailString;

            var users = from u in context.Users
                        select u;

            if (!string.IsNullOrEmpty(parameters.searchNameString))
            {
                users = users.Where(u => u.Name.Contains(parameters.searchNameString));
            }
            if (!string.IsNullOrEmpty(parameters.searchEmailString))
            {
                users = users.Where(u => u.Email.Contains(parameters.searchEmailString));
            }

            switch (parameters.sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(u => u.Name);
                    break;
                case "Email":
                    users = users.OrderBy(u => u.Email);
                    break;
                case "email_desc":
                    users = users.OrderByDescending(u => u.Email);
                    break;
                default:
                    users = users.OrderBy(u => u.Name);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (parameters.page ?? 1);
            return View(users.ToPagedList(pageNumber, pageSize));

        }


        public ActionResult Register()
        {
            return View();

        }

        public void AddUser(User user)
        {

            int nextAvailableUserId = context.Users
                .Max(e => e.Id) + 1;

            user.Id = nextAvailableUserId;

            context.Users.Add(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                context.Users.Add(user);
                context.SaveChanges();

                ModelState.Clear();
                TempData["Message"] = "You have successfully registered with the site!";
                return RedirectToAction("Index");

            }
            return View(user);

        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var userLogin = context.Users
                .Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
            if (userLogin != null)
            {
                Session["UserId"] = userLogin.Id.ToString();
                Session["Email"] = userLogin.Email.ToString();
                return RedirectToAction("LoggedIn");
            }
            else
            {
                ModelState.AddModelError("", "Email or Password incorrect.");
            }
            return View();

        }

        public ActionResult LoggedIn()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

    }
}