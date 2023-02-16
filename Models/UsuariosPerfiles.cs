using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiriWeb.Models
{
    public class UsuariosPerfiles
    {
        public int idUser { get; set; }
        public string nombreUsuario { get; set; }
        public string nombreCompleto { get; set; }
        public string perfil { get; set; }
        public respuestaAPIMiri respuestaAPI { get; set; }
    }
}