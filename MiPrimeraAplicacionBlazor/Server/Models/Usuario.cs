using System;
using System.Collections.Generic;

namespace MiPrimeraAplicacionBlazor.Server.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            ReservaHistorial = new HashSet<ReservaHistorial>();
        }

        public int Iidusuario { get; set; }
        public string Nombreusuario { get; set; }
        public string Contra { get; set; }
        public int? Iidpersona { get; set; }
        public int? Bhabilitado { get; set; }
        public int? Iidtipousuario { get; set; }
        public string Token { get; set; }

        public virtual Persona IidpersonaNavigation { get; set; }
        public virtual TipoUsuario IidtipousuarioNavigation { get; set; }
        public virtual ICollection<ReservaHistorial> ReservaHistorial { get; set; }
    }
}
