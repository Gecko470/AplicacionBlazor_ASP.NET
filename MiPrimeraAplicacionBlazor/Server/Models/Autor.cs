using System;
using System.Collections.Generic;

namespace MiPrimeraAplicacionBlazor.Server.Models
{
    public partial class Autor
    {
        public Autor()
        {
            Libro = new HashSet<Libro>();
        }

        public int Iidautor { get; set; }
        public string Nombre { get; set; }
        public string Appaterno { get; set; }
        public string Apmaterno { get; set; }
        public int? Iidsexo { get; set; }
        public int? Iidpais { get; set; }
        public string Descripcion { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual Pais IidpaisNavigation { get; set; }
        public virtual Sexo IidsexoNavigation { get; set; }
        public virtual ICollection<Libro> Libro { get; set; }
    }
}
