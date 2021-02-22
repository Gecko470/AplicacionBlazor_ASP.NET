using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiPrimeraAplicacionBlazor.Shared
{
    public class TipoLibroCLS
    {
        public int IIDTIPOLIBRO { get; set; }

        [Required(ErrorMessage = "Debe introducir un nombre válido del Tipo Libro")]
        public string NOMBRETIPOLIBRO { get; set; }

        [Required(ErrorMessage = "Debe introducir una descripción válida del Tipo Libro")]
        public string DESCRIPCIONTIPOLIBRO { get; set; }
    }
}
