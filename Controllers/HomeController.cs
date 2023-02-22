using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MiriWeb.Models;
using Newtonsoft.Json;

namespace MiriWeb.Controllers
{
   
    public class HomeController : Controller
    {
        private Seguridad _security = new Seguridad();
        private string BaseURL = ConfigurationManager.AppSettings["BaseURL:url"];
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            Mtotales totales = new Mtotales();
            if (Session["idUser"] != null)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(BaseURL);
                        client.DefaultRequestHeaders.Clear();
                        var response = await client.GetAsync("totalesController/readTotales/" + Session["idUser"].ToString());
                        if (response.IsSuccessStatusCode)
                        {
                            var miriResp = response.Content.ReadAsStringAsync().Result;
                            totales = JsonConvert.DeserializeObject<Mtotales>(miriResp);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.AlertDanger = ex;
                }
               
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View(totales);
        }

        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public  async  Task<ActionResult> Login(string user, string pass)
        {
         
            MLogin login = new MLogin();
            
                try
                {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseURL);
                    client.DefaultRequestHeaders.Clear();
                    var response = await client.GetAsync("loginController/login/" + user + "/" + _security.cifrar(pass));
                    if (response.IsSuccessStatusCode)
                    {
                        var miriResp = response.Content.ReadAsStringAsync().Result;
                        login = JsonConvert.DeserializeObject<MLogin>(miriResp);
                        if(login.respuestaAPI.codigo == 110)
                        {
                            Session["idUser"] = login.idUser;
                            Session["nameUser"] = login.nombre;
                            Session["nameComplete"] = login.nombre + " " + login.aPaterno +" "+ login.aMaterno;
                            Session["perfil"] = login.perfil;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            switch (login.respuestaAPI.codigo)
                            {
                                case -300:
                                    ViewBag.AlertWarning = login.respuestaAPI.Descripcion; break;
                                case 220:
                                    ViewBag.AlertWarning = login.respuestaAPI.Descripcion; break;
                                case 333:
                                    ViewBag.AlertWarning = login.respuestaAPI.Descripcion; break;
                                case -200:
                                    ViewBag.AlertDanger = login.respuestaAPI.Descripcion; break;
                            }
                        }
                       
                    }
                    else
                    {
                        ViewBag.AlertDanger =response.StatusCode + " \nDetalles:" + response.RequestMessage;
                    }
                }
               
                }
                catch (Exception ex)
                {

                    ViewBag.AlertDanger = ex;


                }
            return View();
        }
        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }


    }
}