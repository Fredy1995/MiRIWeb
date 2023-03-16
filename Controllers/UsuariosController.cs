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
    public class UsuariosController : Controller
    {
        private string BaseURL = ConfigurationManager.AppSettings["BaseURL:url"];
        private modelShared data = new modelShared();
        private respuestaAPIMiri respAPIMIRI = new respuestaAPIMiri();
        // GET: Usuarios
        public async Task<ActionResult> Usuarios(FormCollection objetoForm)
        {
            if (Session["idUser"] != null)
            {
                try
                {
                    if (objetoForm["btnAgregar"] != null)
                    {
                  
                      if (objetoForm["selectPerfil"] != ""  )
                        {
                            using (var client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(BaseURL);
                                client.DefaultRequestHeaders.Clear();
                                var mUsuario = new MUsuario(objetoForm["nombre"], objetoForm["aPaterno"], objetoForm["aMaterno"], objetoForm["usuario"], objetoForm["pwd"], Convert.ToInt32(objetoForm["selectPerfil"]));
                                var json = JsonConvert.SerializeObject(mUsuario);
                                var content = new StringContent(json, Encoding.UTF8, "application/json");
                                var response = await client.PostAsync("loginController/createUser", content);
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
                        else
                        {
                            ViewBag.AlertWarning = "Olvidó seleccionar el perfil...";
                        }
                        
                    }
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

        public async  Task<ActionResult> EditUser(FormCollection objetoForm,string Iduser)
        {
           
            if (Session["idUser"] != null)
            {
                try
                {
                    if (objetoForm["btnGuardar"] != null)
                    {
                       
                    }
                    data.musuarios = await listaUsuario(Iduser);
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
        /// Metodo encargado de obtener una lista de propiedades de un usuario
        /// </summary>
        /// <param name="iduser"></param>
        /// <returns>Devuelve una lista de tipo MUsuario</returns>
        [HttpGet]
        public async Task<List<MUsuario>> listaUsuario(string Iduser)
        {
            List<MUsuario> usuario = new List<MUsuario>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseURL);
                    client.DefaultRequestHeaders.Clear();
                    var response = await client.GetAsync("loginController/readUsuario/" + Iduser);
                    if (response.IsSuccessStatusCode)
                    {
                        var miriResp = response.Content.ReadAsStringAsync().Result;
                        usuario = JsonConvert.DeserializeObject<List<MUsuario>>(miriResp);
                    }
                }
            return usuario;
        }
    }
}