using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiriWeb.Models
{
    public class MTemas
    {
        public MTemas(string nameTema,string idtema)
        {
            this.IdTema =Convert.ToInt32(idtema);
            this.Tema = nameTema;
        }

        public int IdTema { get; set; }

        public string Tema { get; set; }
    }
}