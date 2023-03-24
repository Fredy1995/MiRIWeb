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
        private RepositorioController repositorio = new RepositorioController();
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
        public async Task<ActionResult> Clasificacion(FormCollection objetoForm, string idT, string tema)
        {
            if (Session["idUser"] != null)
            {
                try
                {
                    ////FALTA AGREGAR EVENTOS

                    if (idT != null)
                    {
                        data.mclasificaciones = await repositorio.listaClasificaciones(idT, Session["idUser"].ToString());
                    }
                    else
                    {
                        data.mclasificaciones = await repositorio.listaClasificaciones(ViewBag.IdDirectorioT, Session["idUser"].ToString());
                    }

                    if (idT != null && tema != null)
                    {
                        ViewBag.NameDirectorioSelec = tema;
                        ViewBag.IdDirectorioT = idT;

                    }
                } catch (Exception ex)
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

        public async Task<ActionResult> Grupo(FormCollection objetoForm, string idC, string clasif)
        {
            
            if (Session["idUser"] != null)
            {
                try
                {
                    ////FALTA AGREGAR EVENTOS
                    
                    if (idC != null && clasif != null)
                    {
                        data.mtemas = await repositorio.devuelvaObjTema(Convert.ToInt32(idC));
                        foreach (var item in data.mtemas)
                        {
                            ViewBag.NameDirectorioSelec = item.Tema;
                            ViewBag.IdDirectorioSelec = item.IdTema;
                        }


                        ViewBag.NameDirectorioSelecActual = clasif;
                        ViewBag.IdDirectorioT = idC;
                    }
                    if (idC != null)
                    {
                        data.mgrupos = await repositorio.listaGrupos(Convert.ToInt32(ViewBag.IdDirectorioSelec), idC, Session["idUser"].ToString());
                    }
                    else
                    {

                        data.mgrupos = await repositorio.listaGrupos(Convert.ToInt32(ViewBag.IdDirectorioSelec), ViewBag.IdDirectorioT, Session["idUser"].ToString());
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
        public async Task<ActionResult> Archivos(FormCollection objetoForm, string idG, string grupo)
        {
            if (Session["idUser"] != null)
            {
                try
                {
                    if (idG != null && grupo != null)
                    {
                        data.mclasificaciones = await repositorio.devuelvaObjClasificacion(Convert.ToInt32(idG));
                        foreach (var itemc in data.mclasificaciones)
                        {
                            data.mtemas = await repositorio.devuelvaObjTema(itemc.idClasif);
                            foreach (var item in data.mtemas)
                            {
                                ViewBag.NameDirectorioSelecAnterior = item.Tema;
                                ViewBag.IdDirectorioSelecAnterior = item.IdTema;
                            }
                            ViewBag.NameDirectorioSelec = itemc.Clasificacion;
                            ViewBag.IdDirectorioSelec = itemc.idClasif;
                        }

                        ViewBag.NameDirectorioSelecActual = grupo;
                        ViewBag.IdDirectorioT = idG;
                    }
                    if (idG != null)
                    {
                        //AQUI VAN LOS DATA PARA LISTAR O MOSTRAR LOS ARCHIVOS QUE HAY EN EL GRUPO SELECCIONADO
                        //data.mgrupos = await listaGrupos(Convert.ToInt32(ViewBag.IdDirectorioSelec), idC);
                    }
                    else
                    {
                        //AQUI VAN LOS DATA PARA LISTAR O MOSTRAR LOS ARCHIVOS QUE HAY EN EL GRUPO SELECCIONADO
                        //data.mgrupos = await listaGrupos(Convert.ToInt32(ViewBag.IdDirectorioSelec), ViewBag.IdDirectorioT);
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
            return View();
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