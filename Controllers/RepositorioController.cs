using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MiriWeb.Models;
using Newtonsoft.Json;

namespace MiriWeb.Controllers
{
    public class RepositorioController : Controller
    {

        private string BaseURL = ConfigurationManager.AppSettings["BaseURL:url"];
      
        // GET: Repositorio
        
        public async Task<ActionResult> Tema(FormCollection objetoForm)
        {
              List<MTemas> temas = new List<MTemas>();
            respuestaAPIMiri respAPIMIRI = new respuestaAPIMiri();
            if (Session["idUser"] != null)
            {
                try
                {
                    if (objetoForm["btncrear"] != null)
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(BaseURL);
                            client.DefaultRequestHeaders.Clear();
                            var mTemaUsuario = new MTemaUsuario(objetoForm["nameTema"], Session["idUser"].ToString());
                            var json = JsonConvert.SerializeObject(mTemaUsuario);
                            var content = new StringContent(json, Encoding.UTF8, "application/json");
                            var response = await client.PostAsync("temaController/createTemaUsuario", content);
                            if (response.IsSuccessStatusCode)
                            {
                                var miriResp = response.Content.ReadAsStringAsync().Result;
                                respAPIMIRI = JsonConvert.DeserializeObject<respuestaAPIMiri>(miriResp);
                                switch (respAPIMIRI.codigo)
                                {
                                    case 111:
                                        ViewBag.AlertSuccess = respAPIMIRI.Descripcion; break;
                                    case 222:
                                        ViewBag.AlertWarning = respAPIMIRI.Descripcion; break;
                                    case 333:
                                        ViewBag.AlertWarning = respAPIMIRI.Descripcion; break;
                                    case -300:
                                        ViewBag.AlertWarning = respAPIMIRI.Descripcion; break;
                                    case -200:
                                        ViewBag.AlertDanger = respAPIMIRI.Descripcion; break;
                                }
                            }
                            else
                            {
                                ViewBag.AlertDanger = response.StatusCode + "\nDetalles:" + response.RequestMessage;

                            }
                        }
                    }else if (objetoForm["btnUpdateTema"] != null)
                    {
                        var idtema = objetoForm["hiddenIDTema"].ToString();
                        var tema = objetoForm["nameTema"].ToString();
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(BaseURL);
                            client.DefaultRequestHeaders.Clear();
                            var updateTema = new MTemas(objetoForm["nameTema"], objetoForm["hiddenIDTema"]);
                            var json = JsonConvert.SerializeObject(updateTema);
                            var content = new StringContent(json, Encoding.UTF8, "application/json");
                            var response = await client.PutAsync("temaController/updateTema", content);
                            if (response.IsSuccessStatusCode)
                            {
                                var miriResp = response.Content.ReadAsStringAsync().Result;
                                respAPIMIRI = JsonConvert.DeserializeObject<respuestaAPIMiri>(miriResp);
                                switch (respAPIMIRI.codigo)
                                {
                                    case 444:
                                        ViewBag.AlertSuccess = respAPIMIRI.Descripcion; break;
                                    case 333:
                                        ViewBag.AlertWarning = respAPIMIRI.Descripcion; break;
                                    case -200:
                                        ViewBag.AlertDanger = respAPIMIRI.Descripcion; break;
                                }
                            }
                            else
                            {
                                ViewBag.AlertDanger = response.StatusCode + "\nDetalles:" + response.RequestMessage;

                            }
                        }


                    }
                        temas = await listaTemas(Session["idUser"].ToString());
                }
                catch (Exception ex)
                {
                    ViewBag.AlertDanger = ex;
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            return View(temas);
        }

        /// <summary>
        /// Metodo para listar todos los temas que pertenecen al usuario actual
        /// </summary>
        /// <param name="iduser"></param>
        /// <returns>Devuelve una lista de temas de tipo MTemas</returns>
        [HttpGet]
        public async Task<List<MTemas>> listaTemas(string iduser)
        {
            List<MTemas> temas = new List<MTemas>();
          
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseURL);
                    client.DefaultRequestHeaders.Clear();
                    var response = await client.GetAsync("temaController/readTemas/" + iduser);
                    if (response.IsSuccessStatusCode)
                    {
                        var miriResp = response.Content.ReadAsStringAsync().Result;
                        temas = JsonConvert.DeserializeObject<List<MTemas>>(miriResp);
                    }
                }
            return temas;
        }



    }
}