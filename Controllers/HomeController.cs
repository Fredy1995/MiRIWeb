using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiriWeb.Controllers
{
   
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["idUser"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Login(string user, string pass)
        {
            if (user.Equals("admin") && pass.Equals("1234"))
            {
                //add session
                //Session["FullName"] = data.FirstOrDefault().nombre + " " + data.FirstOrDefault().aPaterno;
                //Session["Email"] = data.FirstOrDefault().correo;
                Session["idUser"] = 453;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "Login failed";
                return RedirectToAction("Login");
            }
        }
        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }


    }
}