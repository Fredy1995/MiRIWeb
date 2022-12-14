using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiriWeb.Controllers
{
    public class RepositorioController : Controller
    {
        // GET: Repositorio
        public ActionResult Tema()
        {
            if (Session["idUser"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
    }
}