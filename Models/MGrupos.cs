using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiriWeb.Models
{
    public class MGrupos
    {
        public MGrupos(string nameGrupo, string idgrupo)
        {
            this.IdGrupo = Convert.ToInt32(idgrupo);
            this.Grupo = nameGrupo;
        }

        public int IdGrupo { get; set; }
        public string Grupo { get; set; }
    }
}