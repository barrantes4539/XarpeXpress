using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Business;
using Entity;

namespace Xarpexpress_Administrador.Controllers
{
   
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        //-------------------------------------  CONTROLLER USUARIOS  ------------------------------------//
        #region USUARIOS
        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuarios()
        {

            List<Usuario> oLista = new List<Usuario>();

            oLista = new CN_Usuarios().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        //Modificar a configuracion de productos

        [HttpPost]
        public JsonResult GuardarUsuario(Usuario objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdUsuario == 0)
            {
                resultado = new CN_Usuarios().RegistrarUsuario(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Usuarios().EditarUsuario(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //----------------------------------   CONTROLLER PRODUCTOS  -------------------------------------//
        #region PRODUCTO

        //[HttpGet]
        //public JsonResult ListarProductos()
        //{

        //    List<Producto> oLista = new List<Producto>();

        //    oLista = new CN_Producto().Listar();

        //    return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen) {
        //    string mensaje = string.Empty;
        //    bool operacion_exitosa = true;
        //    bool guardar_imagen_exito = true;

        //    Producto oProducto = new Producto();
        //    oProducto = JsonConvert.DeserializeObject<Producto>(objeto);
        //    decimal precio;

        //    if (decimal.TryParse(oProducto.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-PE"), out precio)) { 
        //        oProducto.Precio = precio;
        //    }
        //    else
        //    {
        //        return Json(new CN_Producto().Registrar(oProducto, out mensaje));

        //        if (oProducto.IdProducto == 0) 
        //        {
        //            int idProductoGenerado = new CN_Producto().Registrar(oProducto, out mensaje);

        //            if (idProductoGenerado != 0)
        //            {
        //                oProducto.IdProducto = idProductoGenerado;
        //            }
        //            else
        //            {
        //                operacion_exitosa = false;
        //            }

        //        }
        //        else
        //        {
        //            operacion_exitosa = new CN_Producto().Editar(oProducto, out mensaje);
        //        }

        //        if (operacion_exitosa) {
        //            if (archivoImagen != null) {
        //                string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotos"];
        //                string extension = Path.GetExtension(archivoImagen.FileName);
        //                string nombre_imagen = string.Concat(oProducto.IdProducto.ToString(), extension);

        //                try
        //                {
        //                    archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));
        //                }
        //                catch (Exception ex)
        //                {
        //                    string msg = ex.Message;
        //                    guardar_imagen_exito = false;

        //                }

        //                if (guardar_imagen_exito)
        //                {
        //                    oProducto.RutaImagen = ruta_guardar;
        //                    oProducto.NombreImagen = nombre_imagen;

        //                    bool rspta = new CN_Producto().GuardarDatosImagen(oProducto, out mensaje);
        //                }
        //                else
        //                {
        //                    mensaje = "Se guardó el producto pero hubo problemas con la imagen";
        //                }
        //            }
        //        }
        //    }
        //}

        //Funcion que devuelve una cadena de conexion y asi permitir pintar una imagen
        //[HttpPost]
        //public JsonResult ImagenProducto(int id) {
        //    bool conversion;

        //    Producto oproducto = new CN_Producto().Listar().Where(p => p.IdProducto == id).FirstOrDefault();

        //    string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oproducto.RutaImagen.NombreImagen);

        //    return Json(new
        //    {
        //        conversion = conversion,
        //        textoBase64 = textoBase64,
        //        extension = Path.GetExtension(oproducto.NombreImagen)
        //    },
        //    JsonRequestBehavior.AllowGet
        //    );
        //}

        //Funcion para eliminar productos
        //[HttpPost]
        //public JsonResult EliminarProducto(int id) {
        //    bool respuesta = false;
        //    string mensaje = string.Empty;

        //    respuesta = new CN_Producto().Eliminar(id, out mensaje);

        //    return Json(new { resultado = respuesta, mensaje = mensaje}, JsonRequestBehavior.AllowGet);
        //}

        #endregion

    }
}