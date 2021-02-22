using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiPrimeraAplicacionBlazor.Shared
{
    public class ReservaCLS
    {
        public int iidReserva { get; set; }

        [Required(ErrorMessage = "Campo Usuario obligatorio")]
        public string iidUsuario { get; set; }

        [Required(ErrorMessage = "Campo Libro obligatorio")]
        public int? iidLibro { get; set; }//Ponemos int? porque si no no hace las validaciones, no salen los mensajes de error. TRUCO.

        [Required(ErrorMessage = "Campo Cantidad obligatorio")]
        public int? cantidad { get; set; }

        public DateTime fechaInicio { get; set; } = DateTime.Now;

        public DateTime fechaFin { get; set; } = DateTime.Now;

        public string nombreLibro { get; set; }

        public string nombreUsuario { get; set; }

        public string fechaInicioCadena { get; set; }//Hago fechaInicio y fechaFin en formato string para el listado

        public string fechaFinCadena { get; set; }

        public int iidUsuarioRegistra { get; set; }
    }
}
