using System;
using System.Collections.Generic;

namespace MiPrimeraAplicacionBlazor.Server.Models
{
    public partial class PaginaTipoUsuario
    {
        public PaginaTipoUsuario()
        {
            PaginaTipoUsuButton = new HashSet<PaginaTipoUsuButton>();
        }

        public int Iidpaginatipousuario { get; set; }
        public int? Iidpagina { get; set; }
        public int? Iidtipousuario { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual Pagina IidpaginaNavigation { get; set; }
        public virtual TipoUsuario IidtipousuarioNavigation { get; set; }
        public virtual ICollection<PaginaTipoUsuButton> PaginaTipoUsuButton { get; set; }
    }
}
