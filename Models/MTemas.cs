using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiriWeb.Models
{
    public class MTemas
    {
        public MTemas(string nameTema,string idtema, string iduser)
        {
            this.IdTema =Convert.ToInt32(idtema);
            this.Tema = nameTema;
            this.IdUser = Convert.ToInt32(iduser);
        }

        public int IdTema { get; set; }

        public string Tema { get; set; }

        public int IdUser { get; set; }
    }
}