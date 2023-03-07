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
        private modelShared data = new modelShared();
        private respuestaAPIMiri respAPIMIRI = new respuestaAPIMiri();
        // GET: Repositorio

        public async Task<ActionResult> Tema(FormCollection objetoForm)
        {
          
           
         
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
                            var mTemaUsuario = new MTemaUsuario(objetoForm["nameDirectorio"], Session["idUser"].ToString());
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
                    }else if (objetoForm["btnUpdateDiretorio"] != null)
                    {
                     
                        if(objetoForm["nameDirectorio"].ToString() != "")
                        {
                            using (var client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(BaseURL);
                                client.DefaultRequestHeaders.Clear();
                                var updateTema = new MTemas(objetoForm["nameDirectorio"].ToString(), objetoForm["hiddenIDDirectorio"].ToString());
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
                            ViewBag.AlertWarning = "DEBE SELECCIONAR UN ELEMENTO";
                        }
                       
                    }
                    else if (objetoForm["btnAceptar"] != null)
                    {
                        List<string> elementos = objetoForm["listUsers"].Split(',').ToList();
                        var idtema = objetoForm["hiddenIDDirectorioC"].ToString();
                        if(elementos[0].ToString() != "")
                        {
                            using (var client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(BaseURL);
                                client.DefaultRequestHeaders.Clear();
                                foreach (var item in elementos)
                                {
                                    var mCompartir = new MCompartir(idtema, item.ToString());
                                    var json = JsonConvert.SerializeObject(mCompartir);
                                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                                    var response = await client.PostAsync("temaController/compartirTema", content);
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
                        }
                        else
                        {
                            ViewBag.AlertWarning = "NO SE SELECCIONÓ NINGÚN ELEMENTO";
                        }
                     
                    }
                   
                    data.mtemas = await listaTemas(Session["idUser"].ToString());
                   
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

        public async Task<ActionResult> Clasificacion(FormCollection objetoForm,string idT,string tema)
        {
           
            if (Session["idUser"] != null)
            {
                try
                {
                    if (objetoForm["btncrear"] != null)
                    {
                        ViewBag.IdDirectorioT = objetoForm["hiddenIDDirectorioT"].ToString();
                        ViewBag.NameDirectorioSelec = objetoForm["hiddenNameDirectorioSelec"].ToString();
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(BaseURL);
                            client.DefaultRequestHeaders.Clear();
                            var mClasificacionTema = new MClasificacionTema(ViewBag.IdDirectorioT, objetoForm["nameDirectorio"], Session["idUser"].ToString());
                            var json = JsonConvert.SerializeObject(mClasificacionTema);
                            var content = new StringContent(json, Encoding.UTF8, "application/json");
                            var response = await client.PostAsync("clasificacionController/createClasificacionTema", content);
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
                    }
                    else if (objetoForm["btnUpdateDiretorio"] != null)
                    {
                      
                        if (objetoForm["nameDirectorio"].ToString() != "")
                        {
                            ViewBag.IdDirectorio = objetoForm["hiddenIDDirectorio"].ToString(); //Directorio seleccionado con clic derecho
                            ViewBag.IdDirectorioT = objetoForm["hiddenIDDirectorioT"].ToString();
                            ViewBag.NameDirectorioSelec = objetoForm["hiddenNameDirectorioSelec"].ToString();
                            using (var client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(BaseURL);
                                client.DefaultRequestHeaders.Clear();
                                var updateClasif = new MClasificaciones(ViewBag.IdDirectorio, objetoForm["nameDirectorio"].ToString());
                                var json = JsonConvert.SerializeObject(updateClasif);
                                var content = new StringContent(json, Encoding.UTF8, "application/json");
                                var response = await client.PutAsync("clasificacionController/updateClasificacion", content);
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
                        else
                        {
                            ViewBag.AlertWarning = "DEBE SELECCIONAR UN ELEMENTO";
                        }
                        ViewBag.IdDirectorio = ViewBag.IdDirectorioT;
                    }


                    if (idT != null)
                    {
                        data.mclasificaciones = await listaClasificaciones(idT, Session["idUser"].ToString());
                    }
                    else
                    {
                        data.mclasificaciones = await listaClasificaciones(ViewBag.IdDirectorioT, Session["idUser"].ToString());
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
            if(idT != null && tema != null)
            {
                ViewBag.NameDirectorioSelec = tema;
                ViewBag.IdDirectorioT = idT;
              
            }
         
            return View(data);
        }
        /// <summary>
        ///  Metodo para listar todos las clasificaciones que pertenecen al tema y usuario actual
        /// </summary>
        /// <param name="idTema"></param>
        /// <param name="iduser"></param>
        /// <returns>Devuelve una lista de clasificaciones de tipo MClasificaciones</returns>
        [HttpGet]
        public async Task<List<MClasificaciones>> listaClasificaciones(string idTema,string iduser)
        {
            List<MClasificaciones> clasificaciones = new List<MClasificaciones>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                var response = await client.GetAsync("clasificacionController/readClasificaciones/"+idTema+"/" + iduser);
                if (response.IsSuccessStatusCode)
                {
                    var miriResp = response.Content.ReadAsStringAsync().Result;
                    clasificaciones = JsonConvert.DeserializeObject<List<MClasificaciones>>(miriResp);
                }
            }
            return clasificaciones;
        }

    }
}