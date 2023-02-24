using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiriWeb.Models
{
    public class MCompartir
    {
        public MCompartir(string idtema, string username)
        {
            this._idtema = Convert.ToInt32(idtema);
            this._username = username;
        }

        public int _idtema  { get; set; }
        public string _username { get; set; }
    }
}