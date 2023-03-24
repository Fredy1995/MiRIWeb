using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiriWeb.Models
{
    public class MGrupoCT
    {
        public MGrupoCT(string idClasif, string nameGroup, string idUser, int permiso)
        {
            this._idClasificacion = Convert.ToInt32(idClasif);
            this._nombreGrupo = nameGroup;
            this._idUser = Convert.ToInt32(idUser);
            this._permiso = permiso;
        }

        public int _idClasificacion { get; set; }
        public string _nombreGrupo { get; set; }
        public int _idUser { get; set; }
        public int _permiso { get; set; }
    }
}