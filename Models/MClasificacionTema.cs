using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiriWeb.Models
{
    public class MClasificacionTema
    {
        public MClasificacionTema(string idt,string namec, string iduser)
        {
            this._idTema =Convert.ToInt32(idt);
            this._nombreClasificacion = namec;
            this._idUser =Convert.ToInt32(iduser);
        }

        public int _idTema { get; set; }
        public string _nombreClasificacion { get; set; }
        public int _idUser { get; set; }
    }
}