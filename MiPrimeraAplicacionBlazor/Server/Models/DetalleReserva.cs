using System;
using System.Collections.Generic;

namespace MiPrimeraAplicacionBlazor.Server.Models
{
    public partial class DetalleReserva
    {
        public int Iiddetallereserva { get; set; }
        public int? Iidreserva { get; set; }
        public int? Iidlibro { get; set; }
        public int? Cantidad { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual Libro IidlibroNavigation { get; set; }
        public virtual Reserva IidreservaNavigation { get; set; }
    }
}
