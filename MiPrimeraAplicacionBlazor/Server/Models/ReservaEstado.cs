using System;
using System.Collections.Generic;

namespace MiPrimeraAplicacionBlazor.Server.Models
{
    public partial class ReservaEstado
    {
        public ReservaEstado()
        {
            Reserva = new HashSet<Reserva>();
            ReservaHistorial = new HashSet<ReservaHistorial>();
        }

        public int Iidestadoreserva { get; set; }
        public string Vnombre { get; set; }
        public string Vdescripcion { get; set; }
        public int? Bhabilitado { get; set; }

        public virtual ICollection<Reserva> Reserva { get; set; }
        public virtual ICollection<ReservaHistorial> ReservaHistorial { get; set; }
    }
}
