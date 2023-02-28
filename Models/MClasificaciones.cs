using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiriWeb.Models
{
    public class MClasificaciones
    {
        public MClasificaciones(string idClasif, string nameClasif)
        {
            this.idClasif = Convert.ToInt32(idClasif);
            this.Clasificacion = nameClasif;
        }
        public int idClasif { get; set; }
        public string Clasificacion { get; set; }
    }
}