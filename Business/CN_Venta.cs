using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Entity;

namespace Business
{
    public class CN_Venta
    {
        private CD_Venta datosVenta = new CD_Venta();
        public bool RegistrarVenta(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            return datosVenta.RegistrarVenta(obj, DetalleVenta, out Mensaje);
        }
    }
}
