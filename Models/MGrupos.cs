using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiriWeb.Models
{
    public class MGrupos
    {
        public MGrupos(string nameGrupo, string idgrupo,string iduser)
        {
            this.IdGrupo = Convert.ToInt32(idgrupo);
            this.Grupo = nameGrupo;
            this.IdUser = Convert.ToInt32(iduser);
        }

        public int IdGrupo { get; set; }
        public string Grupo { get; set; }
        public int IdUser { get; set; }
    }
}