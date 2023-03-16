using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiriWeb.Models
{
    public class modelShared
    {
        public List<MTemas> mtemas { get; set; }
        public List<MClasificaciones> mclasificaciones { get; set; }
        public List<MGrupos> mgrupos { get; set; }
        public List<MUsuario> musuarios { get; set; }
        public List<MPerfiles> mperfiles { get; set; }
    }
}