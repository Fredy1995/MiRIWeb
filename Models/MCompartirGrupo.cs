using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiriWeb.Models
{
    public class MCompartirGrupo
    {
        public MCompartirGrupo(string idDirectorio,string username, string permiso)
        {
            this._idDirectorio = Convert.ToInt32(idDirectorio);
            this._username = username;
            this._permiso = Convert.ToInt32(permiso);
        }

        public int _idDirectorio { get; set; }
        public string _username { get; set; }
        public int _permiso { get; set; }
    }
}