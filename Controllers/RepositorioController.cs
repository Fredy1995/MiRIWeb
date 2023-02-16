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
    public class RepositorioController : Controller
    {

        private string BaseURL = ConfigurationManager.AppSettings["BaseURL:url"];
        private MTemas temas = new MTemas();
        // GET: Repositorio
        [HttpGet]
        //public async Task<ActionResult> Tema()
        //{
        //    //if (Session["idUser"] != null)
        //    //{
        //    //    try
        //    //    {
        //    //        using (var client = new HttpClient())
        //    //        {
        //    //            client.BaseAddress = new Uri(BaseURL);
        //    //            client.DefaultRequestHeaders.Clear();
        //    //            var response = await client.GetAsync("temaController/readTemas/" + Session["idUser"].ToString());
        //    //            if (response.IsSuccessStatusCode)
        //    //            {
        //    //                var miriResp = response.Content.ReadAsStringAsync().Result;
        //    //                temas = JsonConvert.DeserializeObject<?>(miriResp);

        //    //            }
        //    //        }
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        ViewBag.AlertDanger = ex;
        //    //    }

        //    //    return View();
        //    //}
        //    //else
        //    //{
        //    //    return RedirectToAction("Login", "Home");
        //    //}
        //}

        

    }
}