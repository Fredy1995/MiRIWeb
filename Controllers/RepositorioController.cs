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
        public ActionResult Sinpermisos()
        {
            return View();
        }
        public async Task<ActionResult> Tema(FormCollection objetoForm)
        {
      
            if (Session["idUser"] != null)
            {
                if (Session["perfil"].ToString().Equals("Administrador de contenido"))
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
                        }
                        else if (objetoForm["btnUpdateDiretorio"] != null)
                        {

                            if (objetoForm["nameDirectorio"].ToString() != "")
                            {
                                using (var client = new HttpClient())
                                {
                                    client.BaseAddress = new Uri(BaseURL);
                                    client.DefaultRequestHeaders.Clear();
                                    var updateTema = new MTemas(objetoForm["nameDirectorio"].ToString(), objetoForm["hiddenIDDirectorio"].ToString(), Session["idUser"].ToString());
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
                            if (elementos[0].ToString() != "")
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
                    return RedirectToAction("Sinpermisos", "Repositorio");
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
                        ViewBag.NameDirectorioSelec = objetoForm["hiddenNameDirectorioSelec"].ToString(); //Necesario para mostrar la ruta en el directorio posicionado
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
                            ViewBag.IdDirectorio = objetoForm["hiddenIDDirectorio"].ToString(); //Directorio seleccionado con clic 
                            ViewBag.IdDirectorioT = objetoForm["hiddenIDDirectorioT"].ToString();
                            ViewBag.NameDirectorioSelec = objetoForm["hiddenNameDirectorioSelec"].ToString(); //Necesario para mostrar la ruta en el directorio posicionado
                            using (var client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(BaseURL);
                                client.DefaultRequestHeaders.Clear();
                                var updateClasif = new MClasificaciones(ViewBag.IdDirectorio, objetoForm["nameDirectorio"].ToString(), Session["idUser"].ToString());
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
                        ViewBag.IdDirectorio = ViewBag.IdDirectorioT;
                    }
                    else if (objetoForm["btnAceptar"] != null)
                    {
                        List<string> elementos = objetoForm["listUsers"].Split(',').ToList();
                        var idClasif = objetoForm["hiddenIDDirectorioC"].ToString();
                        ViewBag.IdDirectorioT = objetoForm["hiddenIDDirectorioT"].ToString();
                        ViewBag.NameDirectorioSelec = objetoForm["hiddenNameDirectorioSelec"].ToString(); //Necesario para mostrar la ruta en el directorio posicionado
                        if (elementos[0].ToString() != "")
                        {
                            using (var client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(BaseURL);
                                client.DefaultRequestHeaders.Clear();
                                foreach (var item in elementos)
                                {
                                    var mCompartir = new MCompartir(idClasif, item.ToString());
                                    var json = JsonConvert.SerializeObject(mCompartir);
                                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                                    var response = await client.PostAsync("clasificacionController/compartirClasificacion", content);
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
                        }
                        else
                        {
                            ViewBag.AlertWarning = "NO SE SELECCIONÓ NINGÚN ELEMENTO";
                        }
                    }

                    if (idT != null)
                    {
                        data.mclasificaciones = await listaClasificaciones(idT, Session["idUser"].ToString());
                    }
                    else
                    {
                        data.mclasificaciones = await listaClasificaciones(ViewBag.IdDirectorioT, Session["idUser"].ToString());
                    }

                    if (idT != null && tema != null)
                    {
                        ViewBag.NameDirectorioSelec = tema;
                        ViewBag.IdDirectorioT = idT;

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
      /// <summary>
      /// Metodo encargado de devolver el objeto Tema, solo recibe el id de la clasificación
      /// </summary>
      /// <param name="idClasif"></param>
      /// <returns>Metodo encargado de devolver un objeto de tipo MTemas</returns>
        [HttpGet]
        public async Task<List<MTemas>> devuelvaObjTema(int idClasif)
        {
            List<MTemas> tema = new List<MTemas>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                var response = await client.GetAsync("clasificacionController/devuelveObjTema/"+idClasif);
                if (response.IsSuccessStatusCode)
                {
                    var miriRep = response.Content.ReadAsStringAsync().Result;
                    tema = JsonConvert.DeserializeObject<List<MTemas>>(miriRep);
                }
            }
            return tema;
        }
        public async Task<ActionResult> Grupo(FormCollection objetoForm,string idC, string clasif)
        {
            if (Session["idUser"] != null)
            {
                try
                {

                    if (objetoForm["btncrear"] != null)
                    {
                        ViewBag.NameDirectorioSelecActual = objetoForm["hiddenNameDirectorioSelecActual"].ToString();
                        ViewBag.IdDirectorioSelec = objetoForm["hiddenIdDirectorioSelec"].ToString();
                        ViewBag.IdDirectorioT = objetoForm["hiddenIDDirectorioT"].ToString();
                        ViewBag.NameDirectorioSelec = objetoForm["hiddenNameDirectorioSelec"].ToString(); //Necesario para mostrar la ruta en el directorio posicionado
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(BaseURL);
                            client.DefaultRequestHeaders.Clear();
                            var mGrupoCT = new MGrupoCT(ViewBag.IdDirectorioT, objetoForm["nameDirectorio"], Session["idUser"].ToString(),2);
                            var json = JsonConvert.SerializeObject(mGrupoCT);
                            var content = new StringContent(json, Encoding.UTF8, "application/json");
                            var response = await client.PostAsync("grupoController/createGrupoClasificacionTema", content);
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
                            ViewBag.NameDirectorioSelecActual = objetoForm["hiddenNameDirectorioSelecActual"].ToString(); //Directorio clasificación
                            ViewBag.IdDirectorioSelec = objetoForm["hiddenIdDirectorioSelec"].ToString();
                            ViewBag.IdDirectorio = objetoForm["hiddenIDDirectorio"].ToString(); //Directorio seleccionado con clic 
                            ViewBag.IdDirectorioT = objetoForm["hiddenIDDirectorioT"].ToString();
                            ViewBag.NameDirectorioSelec = objetoForm["hiddenNameDirectorioSelec"].ToString(); //Necesario para mostrar la ruta en el directorio posicionado
                            using (var client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(BaseURL);
                                client.DefaultRequestHeaders.Clear();
                                var updateGrupo = new MGrupos(objetoForm["nameDirectorio"].ToString(), ViewBag.IdDirectorio, Session["idUser"].ToString());
                                var json = JsonConvert.SerializeObject(updateGrupo);
                                var content = new StringContent(json, Encoding.UTF8, "application/json");
                                var response = await client.PutAsync("grupoController/updateGrupo", content);
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
                        ViewBag.IdDirectorio = ViewBag.IdDirectorioT;
                    }
                    else if (objetoForm["btnAceptar"] != null)
                    {
                       
                        List<string> elementos = objetoForm["listUsers"].Split(',').ToList();
                        ViewBag.NameDirectorioSelecActual = objetoForm["hiddenNameDirectorioSelecActual"].ToString(); //Directorio clasificación
                        ViewBag.IdDirectorioSelec = objetoForm["hiddenIdDirectorioSelec"].ToString();
                        var idGrupo = objetoForm["hiddenIDDirectorioC"].ToString();
                        ViewBag.IdDirectorioT = objetoForm["hiddenIDDirectorioT"].ToString();
                        ViewBag.NameDirectorioSelec = objetoForm["hiddenNameDirectorioSelec"].ToString(); //Necesario para mostrar la ruta en el directorio posicionado
                        if (elementos[0].ToString() != "")
                        {
                            using (var client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(BaseURL);
                                client.DefaultRequestHeaders.Clear();
                                foreach (var item in elementos)
                                {
                                    var mCompartir = new MCompartirGrupo(idGrupo, item.ToString(), objetoForm["permiso-select"].ToString());
                                    var json = JsonConvert.SerializeObject(mCompartir);
                                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                                    var response = await client.PostAsync("grupoController/compartirGrupo", content);
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
                        }
                        else
                        {
                            ViewBag.AlertWarning = "NO SE SELECCIONÓ NINGÚN ELEMENTO";
                        }
                    }

                    if (idC != null && clasif != null)
                    {
                        data.mtemas = await devuelvaObjTema(Convert.ToInt32(idC));
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
                        data.mgrupos = await listaGrupos(Convert.ToInt32(ViewBag.IdDirectorioSelec), idC, Session["idUser"].ToString());
                    }
                    else
                    {
                        
                        data.mgrupos = await listaGrupos(Convert.ToInt32(ViewBag.IdDirectorioSelec), ViewBag.IdDirectorioT, Session["idUser"].ToString());
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
        /// Metodo para listar todos los grupos que pertenecen al tema, clasificación y el usuario
        /// </summary>
        /// <param name="idTema"></param>
        /// <param name="idClasif"></param>
        /// <param name="iduser"></param>
        /// <returns>Devuelve una lista de grupos de tipo MGrupos</returns>
        [HttpGet]
        public async Task<List<MGrupos>> listaGrupos(int idtema,string idclasif,string idUser)
        {
            List<MGrupos> grupos = new List<MGrupos>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                var response = await client.GetAsync("grupoController/readGrupo/" + idtema + "/" + idclasif + "/" + idUser);
                if (response.IsSuccessStatusCode)
                {
                    var miriResp = response.Content.ReadAsStringAsync().Result;
                    grupos = JsonConvert.DeserializeObject<List<MGrupos>>(miriResp);
                }
            }
            return grupos;
        }
        /// <summary>
        /// Metodo encargado de devolver el objeto Clasificion, solo recibe el id del grupo
        /// </summary>
        /// <param name="idClasif"></param>
        /// <returns>Metodo encargado de devolver un objeto de tipo MClasificaciones</returns>
        [HttpGet]
        public async Task<List<MClasificaciones>> devuelvaObjClasificacion(int idClasif)
        {
            List<MClasificaciones> clasificacion = new List<MClasificaciones>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                var response = await client.GetAsync("grupoController/devuelveObjClasif/" + idClasif);
                if (response.IsSuccessStatusCode)
                {
                    var miriRep = response.Content.ReadAsStringAsync().Result;
                    clasificacion = JsonConvert.DeserializeObject<List<MClasificaciones>>(miriRep);
                }
            }
            return clasificacion;
        }
        public async Task<ActionResult> Archivos(FormCollection objetoForm, string idG, string grupo)
        {
            if (Session["idUser"] != null)
            {
                try
                {
                    if (idG != null && grupo != null)
                    {
                        data.mclasificaciones = await devuelvaObjClasificacion(Convert.ToInt32(idG));
                        foreach(var itemc in data.mclasificaciones)
                        {
                            data.mtemas = await devuelvaObjTema(itemc.idClasif);
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
       
    }
}