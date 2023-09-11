using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Canton
    {
        public int CantonId { get; set; }
        public string Nombre { get; set; }
        public int ProvinciaId { get; set; }
        public Provincia Provincia { get; set; }
    }
}
