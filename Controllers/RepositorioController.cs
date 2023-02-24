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

        // GET: Repositorio
       
        public async Task<ActionResult> Tema(FormCollection objetoForm)
        {
          
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
                    else if (objetoForm["btnAceptar"] != null)
                    {
                        List<string> elementos = objetoForm["listUsers"].Split(',').ToList();
                        var idtema = objetoForm["hiddenIDTemaC"].ToString();
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(BaseURL);
                            client.DefaultRequestHeaders.Clear();
                            foreach(var item in elementos)
                            {
                                var mCompartir = new MCompartir(idtema,item.ToString());
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
                   
                    data.mtemas = await listaTemas(Session["idUser"].ToString());
                    //if(objetoForm["hiddenIDTemaC"] != null)
                    //{
                    //    data.musuariosintema = await listaSelect(Convert.ToInt32(dato));
                    //}
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
        /// Metodo encargado de obtener una lista de usuarios que no son responsables del TEMA seleccionado
        /// </summary>
        /// <returns>Devuelve una lista de tipo MUsuariosSinTema</returns>
        [HttpGet]
        public async Task<List<MUsuariosSinTema>> listaSelect(int idtema)
        {
            List<MUsuariosSinTema> must = new List<MUsuariosSinTema>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURL);
                client.DefaultRequestHeaders.Clear();
                var response = await client.GetAsync("temaController/readUsuariosSinTema/"+ idtema);
                if (response.IsSuccessStatusCode)
                {
                    var miriResp = response.Content.ReadAsStringAsync().Result;
                    must = JsonConvert.DeserializeObject<List<MUsuariosSinTema>>(miriResp);
                }
            }
            return must;
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