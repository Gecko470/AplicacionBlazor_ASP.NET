using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiPrimeraAplicacionBlazor.Shared
{
    public class AutorCLS
    {
        public int iidAutor { get; set; }

        [Required(ErrorMessage = "El campo nombre es onbligatorio")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [MinLength(2, ErrorMessage = "Mínimo 2 caracteres")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "El campo apellido paterno es onbligatorio")]
        [MaxLength(150, ErrorMessage = "Máximo 150 caracteres")]
        [MinLength(3, ErrorMessage = "Mínimo 3 caracteres")]
        public string aPaterno { get; set; }


        [Required(ErrorMessage = "El campo apellido materno es onbligatorio")]
        [MaxLength(150, ErrorMessage = "Máximo 150 caracteres")]
        [MinLength(3, ErrorMessage = "Mínimo 3 caracteres")]
        public string aMaterno { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string iidSexo { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string iidPais { get; set; }

        public string nombreAutor { get; set; }

        public string nombreSexo { get; set; }

        public string nombrePais { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MaxLength(500, ErrorMessage = "Máximo 500 caracteres")]
        [MinLength(10, ErrorMessage = "Mínimo 10 caracteres")]
        public string descripcion { get; set; }

    }
}
