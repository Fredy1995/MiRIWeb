using MiriWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
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
        private respuestaAPIMiriServer rms = new respuestaAPIMiriServer();
       

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
                            rms = await repositorio.GetFolder(ViewBag.NameDirectorioSelec + "/" + objetoForm["nameDirectorio"]); //**********************Consumo de API MIRIServer ***************
                            if (response.IsSuccessStatusCode)
                            {
                                var miriResp = response.Content.ReadAsStringAsync().Result;
                                respAPIMIRI = JsonConvert.DeserializeObject<respuestaAPIMiri>(miriResp);
                                switch (respAPIMIRI.codigo)
                                {
                                    case 111:
                                        if (rms.estatus)
                                        {
                                            ViewBag.AlertSuccess = respAPIMIRI.Descripcion; break;
                                        }
                                        else
                                        {
                                            ViewBag.AlertDanger = response.StatusCode + "\nDetalles:" + rms.respuesta; break;
                                        }
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
                            var mGrupoCT = new MGrupoCT(ViewBag.IdDirectorioT, objetoForm["nameDirectorio"], Session["idUser"].ToString(), 2);
                            var json = JsonConvert.SerializeObject(mGrupoCT);
                            var content = new StringContent(json, Encoding.UTF8, "application/json");
                            var response = await client.PostAsync("grupoController/createGrupoClasificacionTema", content);
                            rms = await repositorio.GetFolder(ViewBag.NameDirectorioSelec + "/" + ViewBag.NameDirectorioSelecActual + "/" + objetoForm["nameDirectorio"]); //**********************Consumo de API MIRIServer ***************
                            if (response.IsSuccessStatusCode)
                            {
                                var miriResp = response.Content.ReadAsStringAsync().Result;
                                respAPIMIRI = JsonConvert.DeserializeObject<respuestaAPIMiri>(miriResp);
                                switch (respAPIMIRI.codigo)
                                {
                                    case 111:
                                        if (rms.estatus)
                                        {
                                            ViewBag.AlertSuccess = respAPIMIRI.Descripcion; break;
                                        }
                                        else
                                        {
                                            ViewBag.AlertDanger = response.StatusCode + "\nDetalles:" + rms.respuesta; break;
                                        }
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
                        data.mtemas = await repositorio.devuelvaObjTema(Convert.ToInt32(idC));
                        foreach (var item in data.mtemas)
                        {
                            ViewBag.NameDirectorioSelec = item.Tema;
                            ViewBag.IdDirectorioSelec = item.IdTema;
                            ////FUNCION NECESARIA PARA VERIFICAR SI EL USUARIO TIENE ACCESO AL DIRECTORIO INDICADO
                            ViewBag.ActivarLinks = await accesoAdirectorio(Convert.ToInt32(item.IdTema), Convert.ToInt32(Session["idUser"].ToString()), "t");
                        }
                        ViewBag.NameDirectorioSelecActual = clasif;
                        ViewBag.IdDirectorioT = idC;
                    }
                    if (idC != null)
                    {
                        ViewBag.ActivarLinks = await accesoAdirectorio(Convert.ToInt32(ViewBag.IdDirectorioSelec), Convert.ToInt32(Session["idUser"].ToString()), "t");
                        data.mgrupos = await repositorio.listaGrupos(Convert.ToInt32(ViewBag.IdDirectorioSelec), idC, Session["idUser"].ToString());
                    }
                    else
                    {
                        data.mgrupos = await repositorio.listaGrupos(Convert.ToInt32(ViewBag.IdDirectorioSelec), ViewBag.IdDirectorioT, Session["idUser"].ToString());
                        ViewBag.ActivarLinks = await accesoAdirectorio(Convert.ToInt32(ViewBag.IdDirectorioSelec), Convert.ToInt32(Session["idUser"].ToString()), "t");
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
                    //AQUI VAN EL CODIGÓ ENCARGADO DE SUBIR EL ARCHIVO AL SERVIDOR DE ARCHIVOS
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
                                ////FUNCION NECESARIA PARA VERCAR SI EL USUARIO TIENE ACCESO AL DIRECTORIO INDICADO
                                ViewBag.ActivarLinks = await accesoAdirectorio(Convert.ToInt32(item.IdTema), Convert.ToInt32(Session["idUser"].ToString()), "t");
                            }
                            ViewBag.NameDirectorioSelec = itemc.Clasificacion;
                            ViewBag.IdDirectorioSelec = itemc.idClasif;
                            ViewBag.ActivarLinks2 = await accesoAdirectorio(Convert.ToInt32(itemc.idClasif), Convert.ToInt32(Session["idUser"].ToString()), "c");
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

        /// <summary>
        /// Metodo encargado de verificar si el usuario tiene acceso al directorio indicado
        /// </summary>
        /// <param name="directorio"></param>
        /// <param name="iduser"></param>
        /// <param name="tipoD"></param>
        /// <returns>Devuelve verdadero si el usuario tiene acceso al directorio indicado</returns>
        [HttpGet]
        public async Task<bool> accesoAdirectorio(int directorio, int iduser , string tipoD)
        {
            bool respuesta = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                var response = await client.GetAsync("compartidosController/accesoAdirectorio/" + directorio + "/" + iduser + "/" + tipoD);
                if (response.IsSuccessStatusCode)
                {
                    var miriResp = response.Content.ReadAsStringAsync().Result;
                    respuesta = JsonConvert.DeserializeObject<bool>(miriResp);
                }
            }
            return respuesta;
        }
    }
}