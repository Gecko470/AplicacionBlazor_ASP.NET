using System;
using System.Collections.Generic;

namespace MiPrimeraAplicacionBlazor.Server.Models
{
    public partial class Reserva
    {
        public Reserva()
        {
            DetalleReserva = new HashSet<DetalleReserva>();
            ReservaHistorial = new HashSet<ReservaHistorial>();
        }

        public int Iidreserva { get; set; }
        public int? Iidusuario { get; set; }
        public int? Numlibros { get; set; }
        public int? Iidestadoreserva { get; set; }
        public DateTime? Dfechareserva { get; set; }
        public DateTime? Dfechafinreserva { get; set; }
        public int? Bhabilitado { get; set; }
        public int? Iidlibro { get; set; }
        public DateTime? Dfechainicioreserva { get; set; }

        public virtual ReservaEstado IidestadoreservaNavigation { get; set; }
        public virtual ICollection<DetalleReserva> DetalleReserva { get; set; }
        public virtual ICollection<ReservaHistorial> ReservaHistorial { get; set; }
    }
}
