using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiPrimeraAplicacionBlazor.Shared
{
    public class UsuarioCLS
    {
        public int iidUsuario { get; set; }

        [Required(ErrorMessage = "Campo Usuario es obligatorio")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Campo Password es obligatorio")]
        [MinLength(8, ErrorMessage = "Campo Contraseña mínimo 8 caracteres")]
        public string password { get; set; }
    }
}
