using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiriWeb.Models
{
    public class Usuario
    {
        public Usuario(int iduser,int numUsuario)
        {
            this.IdUsuario = iduser;
            this.NumUsuario = numUsuario;
        }

        public int IdUsuario { get; set; }

        public int NumUsuario { get; set; }
    }
}