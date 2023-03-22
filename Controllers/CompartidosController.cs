using MiriWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MiriWeb.Controllers
{
    public class CompartidosController : Controller
    {
        private string BaseURL = ConfigurationManager.AppSettings["BaseURL:url"];
        private modelShared data = new modelShared();
        private respuestaAPIMiri respAPIMIRI = new respuestaAPIMiri();

        // GET: Compartidos
        public async Task<ActionResult> Compartidos()
        {
            if (Session["idUser"] != null)
            {
                try
                {
                    data.mdirectorioscompartidos = await listaDirectoriosCompartidos(Session["idUser"].ToString());
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
            return View(data);
        }
        /// <summary>
        /// Metodo encargado de obtener una lista de los directorios compartidos al usuario pasado por parametro
        /// </summary>
        /// <param name="iduser"></param>
        /// <returns>Devuelve una lista de tipo MDirectoriosCompartidos con todos los directorios compartidos </returns>
        [HttpGet]
        public async Task<List<MDirectoriosCompartidos>> listaDirectoriosCompartidos(string iduser)
        {
            List<MDirectoriosCompartidos> compartidos = new List<MDirectoriosCompartidos>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                var response = await client.GetAsync("compartidosController/compartidosConmigo/" + iduser);
                if (response.IsSuccessStatusCode)
                {
                    var miriResp = response.Content.ReadAsStringAsync().Result;
                    compartidos = JsonConvert.DeserializeObject<List<MDirectoriosCompartidos>>(miriResp);
                }
            }
            return compartidos;
        }
    }
}