using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiriWeb.Models
{
    public class MLogin
    {
        public int idUser { get; set; }
        public string nombreUsuario { get; set; }
        public string nombre { get; set; }
        public string aPaterno { get; set; }
        public string aMaterno { get; set; }
        public string perfil { get; set; }
        public respuestaAPIMiri respuestaAPI { get; set; }
    }
}