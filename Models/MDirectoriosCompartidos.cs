using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiriWeb.Models
{
    public class MDirectoriosCompartidos
    {
        public MDirectoriosCompartidos(int IdDirectorio, string nombreDirectorio)
        {
            this.IdDirectorio = IdDirectorio;
            this.NameDirectorio = nombreDirectorio;
        }

        public int IdDirectorio { get; set; }
        public string NameDirectorio { get; set; }
    }
}