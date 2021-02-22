using System;
using System.Collections.Generic;

namespace MiPrimeraAplicacionBlazor.Server.Models
{
    public partial class TipoUsuario
    {
        public TipoUsuario()
        {
            PaginaTipoUsuario = new HashSet<PaginaTipoUsuario>();
            Usuario = new HashSet<Usuario>();
        }

        public int Iidtipousuario { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual ICollection<PaginaTipoUsuario> PaginaTipoUsuario { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
