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
            //if (Session["idUser"] != null)
            //{
                return View();
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}
        }

        public ActionResult Login()
        {
            return View();
        }
      
    }
}