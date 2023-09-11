using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Distrito
    {
        public int DistritoId { get; set; }
        public string Nombre { get; set; }
        public int CantonId { get; set; }
        public Canton Canton { get; set; }
    }
}
