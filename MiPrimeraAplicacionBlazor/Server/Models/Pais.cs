using System;
using System.Collections.Generic;

namespace MiPrimeraAplicacionBlazor.Server.Models
{
    public partial class Pais
    {
        public Pais()
        {
            Autor = new HashSet<Autor>();
        }

        public int Iidpais { get; set; }
        public string Nombre { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual ICollection<Autor> Autor { get; set; }
    }
}
