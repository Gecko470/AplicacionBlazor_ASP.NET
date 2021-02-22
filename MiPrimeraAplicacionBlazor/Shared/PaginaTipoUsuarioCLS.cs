using System;
using System.Collections.Generic;
using System.Text;

namespace MiPrimeraAplicacionBlazor.Shared
{
    public class PaginaTipoUsuarioCLS
    {
        public int iidPaginaTipoUsuario { get; set; }

        public string nombrePagina { get; set; }

        public string nombreTipoUsuario { get; set; }

        public List<int> idsButtons { get; set; } = new List<int>();
    }
}
