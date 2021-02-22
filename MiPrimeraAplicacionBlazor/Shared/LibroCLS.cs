using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiPrimeraAplicacionBlazor.Shared
{
    public class LibroCLS
    {
        public int iidLibro { get; set; }

        [Required(ErrorMessage = "Campo Título obligatorio")]
        public string titulo { get; set; }

        [Required(ErrorMessage = "Campo Resumen obligatorio")]
        public string resumen { get; set; }

        [Required(ErrorMessage = "Campo Número de Páginas obligatorio")]
        public int numPag { get; set; }

        [Required(ErrorMessage = "Campo Stock obligatorio")]
        public int stock { get; set; }

        [Required(ErrorMessage = "Campo Autor obligatorio")]
        public string iidAutor { get; set; }//Lo ponemos con string para que sea más manejable en el combobox

        public string fotoCaratula { get; set; }

        public string libroPDF { get; set; }

        public string nombreAutor { get; set; }

    }
}
