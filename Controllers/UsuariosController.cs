using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MiriWeb.Models;
using Newtonsoft.Json;

namespace MiriWeb.Controllers
{
    public class UsuariosController : Controller
    {
        private string BaseURL = ConfigurationManager.AppSettings["BaseURL:url"];
        private modelShared data = new modelShared();
        private respuestaAPIMiri respAPIMIRI = new respuestaAPIMiri();
        // GET: Usuarios
        public async Task<ActionResult> Usuarios()
        {
            if (Session["idUser"] != null)
            {
                try
                {
                    data.musuarios = await listaUsuarios();
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
        /// Metodo para listar todos los usuarios registrados
        /// </summary>
        /// <returns>Devuelve una lista de usuarios de tipo MUsuarios</returns>
        [HttpGet]
        public async Task<List<MUsuario>> listaUsuarios()
        {
            List<MUsuario> usuarios = new List<MUsuario>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                var response = await client.GetAsync("loginController/readUsuarios" );
                if (response.IsSuccessStatusCode)
                {
                    var miriResp = response.Content.ReadAsStringAsync().Result;
                    usuarios = JsonConvert.DeserializeObject<List<MUsuario>>(miriResp);
                }
            }
            return usuarios;
        }
    }
}