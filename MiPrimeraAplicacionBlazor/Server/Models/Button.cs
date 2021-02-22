using System;
using System.Collections.Generic;

namespace MiPrimeraAplicacionBlazor.Server.Models
{
    public partial class Button
    {
        public Button()
        {
            PaginaTipoUsuButton = new HashSet<PaginaTipoUsuButton>();
        }

        public int Iidbutton { get; set; }
        public string Nombrebutton { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual ICollection<PaginaTipoUsuButton> PaginaTipoUsuButton { get; set; }
    }
}
