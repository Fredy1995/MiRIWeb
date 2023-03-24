using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiriWeb.Models
{
    public class MCompartir
    {
        public MCompartir(string iddirectorio, string username)
        {
            this._idDirectorio = Convert.ToInt32(iddirectorio);
            this._username = username;
        }

        public int _idDirectorio  { get; set; }
        public string _username { get; set; }
    }
}