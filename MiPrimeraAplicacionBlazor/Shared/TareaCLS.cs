using System;
using System.Collections.Generic;
using System.Text;

namespace MiPrimeraAplicacionBlazor.Shared
{
    public class TareaCLS
    {
        public string diaSemana { get; set; }

        public string Tarea { get; set; }

        public bool Realizado { get; set; }

        public DateTime fechaTarea { get; set; }
    }
}