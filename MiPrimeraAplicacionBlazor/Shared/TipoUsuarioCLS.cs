using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiPrimeraAplicacionBlazor.Shared
{
    public class TipoUsuarioCLS
    {
        public int iidTipoUsuario { get; set; }

        [Required(ErrorMessage = "Campo Nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Campo Descripción es obligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string descripcion { get; set; }

        public List<int> arrayId { get; set; } = new List<int>();

    }
}
