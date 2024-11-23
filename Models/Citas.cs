using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PEDFAM.Models
{
    public class Citas
    {
        public string fechaProgramada { get; set; }
        public string horaProgramada { get; set; }
        public string id { get; set; }
        public string pacienteId { get; set; }
        public string servicioOfrecido { get; set; }
        public string titulo { get; set; }
    }
}