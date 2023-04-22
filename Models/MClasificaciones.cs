using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiriWeb.Models
{
    public class MClasificaciones
    {
        public MClasificaciones(string idClasif, string nameClasif,string iduser)
        {
            this.idClasif = Convert.ToInt32(idClasif);
            this.Clasificacion = nameClasif;
            this.IdUser = Convert.ToInt32(iduser);
        }
        public int idClasif { get; set; }
        public string Clasificacion { get; set; }
        public int IdUser { get; set; }
    }
}