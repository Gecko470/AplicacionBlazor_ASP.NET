using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiPrimeraAplicacionBlazor.Shared
{
    public class RegistroUsuarioCLS
    {
        public int iidPersona { get; set; }

        [Required(ErrorMessage = "Campo Nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Campo Apellido Paterno es obligatorio")]
        [MaxLength(150, ErrorMessage = "Máximo 150 caracteres")]
        public string aPaterno { get; set; }

        [Required(ErrorMessage = "Campo Apellido Materno es obligatorio")]
        [MaxLength(150, ErrorMessage = "Máximo 150 caracteres")]
        public string aMaterno { get; set; }

        [Required(ErrorMessage = "Campo Teléfono es obligatorio")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string telefono { get; set; }

        [Required(ErrorMessage = "Campo Email es obligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string email { get; set; }

        public DateTime fechaNto { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Campo Nombre Usuario es obligatorio")]
        public string nombreUsuario { get; set; }

        [MinLength(8, ErrorMessage = "Campo Contraseña mínimo 8 caracteres")]
        public string contrasenia { get; set; }
    }
}
