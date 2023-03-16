using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiriWeb.Models
{
    public class MUsuario
    {
        public MUsuario(string IdUser,string nombre, string aPaterno, string aMaterno, string usuario,string pwd,int idperfil,bool habilitado,bool check)
        {
            this.IdUsuario =Convert.ToInt32(IdUser);
            this.Nombre = nombre;
            this.APaterno = aPaterno;
            this.AMaterno = aMaterno;
            this.Usuario1 = usuario;
            this.Contraseña = pwd;
            this.IdPerfil = idperfil;
            this.Habilitado = habilitado;
            this.check = check;
        }

        public int IdUsuario { get; set; }

        public string Usuario1 { get; set; }

        public string Contraseña { get; set; }

        public string Nombre { get; set; }

        public string APaterno { get; set; }

        public string AMaterno { get; set; }

        public int IdPerfil { get; set; }

        public string Perfil { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public bool? Habilitado { get; set; }

        public bool? check { get; set; }
    }
}