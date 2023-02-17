using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiriWeb.Models
{
    public class MTemaUsuario
    {
        public MTemaUsuario(string nameTema, string iduser)
        {
            this._tema = nameTema;
            this._iduser = Convert.ToInt32(iduser);
        }

        public string _tema { get; set; }
        public int _iduser { get; set; }
    }
}