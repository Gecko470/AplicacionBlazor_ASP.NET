using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiPrimeraAplicacionBlazor.Shared
{
    public class PaginaCLS
    {
        public int iidPagina { get; set; }

        [Required(ErrorMessage = "Campo Mensaje obligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string mensaje { get; set; }

        [Required(ErrorMessage = "Campo Accion obligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string accion { get; set; }

        //Para el agregar, visible true o false
        public bool bvisible { get; set; }

        //Para el listado, visible "si" o "no"
        public string nombreVisible { get; set; }
    }
}
