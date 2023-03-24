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
                                var mUsuario = new MUsuario("0",objetoForm["nombre"], objetoForm["aPaterno"], objetoForm["aMaterno"], objetoForm["usuario"], objetoForm["pwd"], Convert.ToInt32(objetoForm["selectPerfil"]),false,false);
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
                    if (TempData["mjsOtherController"] != null)
                    {
                        ViewBag.AlertSuccess = TempData["mjsOtherController"].ToString();
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
            bool checkboxpwd, darkmode;
            if (Session["idUser"] != null)
            {
                try
                {
                    if (objetoForm["btnGuardar"] != null)
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(BaseURL);
                            client.DefaultRequestHeaders.Clear();
                           
                            if(objetoForm["darkmode"] != null)
                            {
                                darkmode = true;
                            }
                            else
                            {
                                darkmode = false;
                            }
                            if(objetoForm["checkboxPwd"] != null)
                            {
                                checkboxpwd = true; ;
                            }
                            else
                            {
                                checkboxpwd = false;
                            }
                            var updateUsuario = new MUsuario(objetoForm["hiddenIDuser"].ToString(), objetoForm["nombre"], objetoForm["aPaterno"], objetoForm["aMaterno"], objetoForm["usuario"], objetoForm["pwd"], Convert.ToInt32(objetoForm["selectPerfil"]), darkmode, checkboxpwd);
                            var json = JsonConvert.SerializeObject(updateUsuario);
                            var content = new StringContent(json, Encoding.UTF8, "application/json");
                            var response = await client.PutAsync("loginController/updateDatosUser", content);
                            if (response.IsSuccessStatusCode)
                            {
                                var miriResp = response.Content.ReadAsStringAsync().Result;
                                respAPIMIRI = JsonConvert.DeserializeObject<respuestaAPIMiri>(miriResp);
                                switch (respAPIMIRI.codigo)
                                {
                                    case 444:
                                        ViewBag.AlertSuccess = respAPIMIRI.Descripcion;
                                        TempData["mjsOtherController"] = respAPIMIRI.Descripcion;
                                        return RedirectToAction("Usuarios", "Usuarios");
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
                    if(Iduser != null)
                    {
                        ViewBag.IDUserTemp = Iduser;
                        data.musuarios = await listaUsuario(Iduser);
                    }
                    else
                    {
                        
                        data.musuarios = await listaUsuario(objetoForm["hiddenIDuser"].ToString());
                    }
                
                    data.mperfiles = await listaPerfiles();
                   
                    foreach(var item in data.musuarios)
                    {
                        ViewBag.Usuario = item.Usuario1;
                        ViewBag.Nombre = item.Nombre;
                        ViewBag.APaterno = item.APaterno;
                        ViewBag.AMaterno = item.AMaterno;
                        ViewBag.Habilitado = item.Habilitado;
                        ViewBag.OptionSelect = item.IdPerfil;
                        if (ViewBag.Habilitado)
                        {
                            ViewBag.LblHabilitado = "Habilitado";
                        }
                        else
                        {
                            ViewBag.LblHabilitado = "Deshabilitado";
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
                return RedirectToAction("Login", "Home");
            }
            return View(data);
        }

        /// <summary>
        /// Metodo encargado de Listar los perfiles
        /// </summary>
        /// <returns>Devuelve una lista de tipo MPerfiles</returns>
        [HttpGet]
        public async Task<List<MPerfiles>> listaPerfiles()
        {
            List<MPerfiles> perfil = new List<MPerfiles>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                var response = await client.GetAsync("loginController/readPerfiles");
                if (response.IsSuccessStatusCode)
                {
                    var miriResp = response.Content.ReadAsStringAsync().Result;
                    perfil = JsonConvert.DeserializeObject<List<MPerfiles>>(miriResp);
                }
            }
            return perfil;
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