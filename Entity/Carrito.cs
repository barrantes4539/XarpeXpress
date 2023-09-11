using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Carrito
    {
        public int IdCarrito { get; set; }
        public Cliente oCliente { get; set; }
        public Producto oProducto { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
