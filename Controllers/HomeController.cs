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
         
            respuestaAPIMiri respAPIMIRI = new respuestaAPIMiri();
            UsuariosPerfiles up = new UsuariosPerfiles();
            //string version = ConfigurationManager.AppSettings["webpages:Version"];
            
                try
                {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseURL);
                    client.DefaultRequestHeaders.Clear();

                    var response = await client.GetAsync("loginController/login/" + user + "/" + pass);
                    if (response.IsSuccessStatusCode)
                    {
                        var miriResp = response.Content.ReadAsStringAsync().Result;
                        up = JsonConvert.DeserializeObject<UsuariosPerfiles>(miriResp);
                        
                        if(up.respuestaAPI.codigo == 110)
                        {
                            response = await client.GetAsync("loginController/readIDUsuarioDB/"+up.idUser);
                            if (response.IsSuccessStatusCode)
                            {
                                miriResp = response.Content.ReadAsStringAsync().Result;
                                Session["idUser"] = JsonConvert.DeserializeObject<int>(miriResp);
                                Session["nameUser"] = up.nombreUsuario;
                                Session["nameComplete"] = up.nombreCompleto;
                                Session["perfil"] = up.perfil;
                                var mUsuario = new Usuario(0, up.idUser);
                                var json = JsonConvert.SerializeObject(mUsuario);
                                var content = new StringContent(json, Encoding.UTF8, "application/json");
                                response = await client.PostAsync("loginController/createIDUser", content);
                                if (response.IsSuccessStatusCode)
                                {
                                    miriResp = response.Content.ReadAsStringAsync().Result;
                                    respAPIMIRI = JsonConvert.DeserializeObject<respuestaAPIMiri>(miriResp);
                                    if (respAPIMIRI.codigo == 111 || respAPIMIRI.codigo == 222)
                                    {
                                        return RedirectToAction("Index");
                                    }
                                    else
                                    {
                                        ViewBag.AlertDanger = respAPIMIRI.Descripcion;

                                    }

                                }
                                else
                                {
                                    ViewBag.AlertDanger = response.StatusCode + "\nDetalles:" + response.RequestMessage;

                                }
                            }
                            else
                            {
                                ViewBag.AlertDanger = response.StatusCode + "\nDetalles:" + response.RequestMessage;

                            }

                        }
                        else
                        {
                            ViewBag.AlertWarning =  up.respuestaAPI.Descripcion;
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