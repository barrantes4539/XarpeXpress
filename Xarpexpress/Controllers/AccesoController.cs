using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;
using Business;
using System.Security.Claims;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.Services.Description;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Xarpexpress.Controllers
{
    public class AccesoController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }
        public ActionResult DobleFactorAut()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }
       

        [HttpPost]
        public ActionResult Registrar(Cliente objeto)
        {
            int resultado;
            string mensaje;

            ViewData["Nombres"] = string.IsNullOrEmpty(objeto.Nombres) ? "" : objeto.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(objeto.Apellidos) ? "" : objeto.Apellidos;
            ViewData["Correo"] = string.IsNullOrEmpty(objeto.Correo) ? "" : objeto.Correo;
            ViewData["Direccion"] = string.IsNullOrEmpty(objeto.Direccion) ? "" : objeto.Direccion;

            if (objeto.Clave != objeto.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            resultado = new CN_Cliente().RegistrarCliente(objeto, out mensaje);

            if (resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }
     

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Cliente oCliente = null;

            oCliente = new CN_Cliente().ListarIS().Where(item => item.Correo == correo && item.Clave == CN_Cliente.EncriptarClave(clave)).FirstOrDefault();

            if (oCliente == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos";
                return View();

            }
            if (oCliente == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos";
                return View();

            }
            else
            {
                if (oCliente.Reestablecer)
                {
                    TempData["IdCliente"] = oCliente.IdCliente;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(oCliente.Correo, false);
                    Session["Cliente"] = oCliente;
                    string mensaje = string.Empty;
                    bool respuesta = new CN_Cliente().EnviarVerificacion(oCliente.IdCliente, correo, out mensaje);
                    if (respuesta)
                    {
                        ViewBag.Error = null;
                        return RedirectToAction("DobleFactorAut");
                    }
                    else
                    {
                        return View();
                    }
                }

            }
        }

        [HttpPost]
        public ActionResult DobleFactorAut(string codigoVerificacion)
        {
            Cliente oCliente = (Cliente)Session["Cliente"];
            CN_Cliente negocios = new CN_Cliente();
            TempData["IdCliente"] = oCliente.IdCliente;

            string mensaje = string.Empty;
            string codigoCliente = negocios.CodigoVerificacion(oCliente.IdCliente);

            if (codigoCliente != null && codigoCliente.Equals(codigoVerificacion))
            {
                FormsAuthentication.SetAuthCookie(oCliente.Correo, false);

                negocios.EliminarCodigo(oCliente.IdCliente);
                negocios.RegAuditoriaISCliente(oCliente.IdCliente);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Código de verificación incorrecto";
                return View();
            }
        }

        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Cliente oCliente = new Cliente();

            oCliente = new CN_Cliente().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if (oCliente == null)
            {
                ViewBag.Error = "No se encontró un usuario relacionado a ese correo";
                return View();
            }

            string mensaje = string.Empty;

            bool respuesta = new CN_Cliente().ReestablecerClave(oCliente.IdCliente, correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string idcliente, string claveactual, string nuevaclave, string confirmarclave)
        {
            Cliente oCliente = new Cliente();

            oCliente = new CN_Cliente().ListarIS().Where(u => u.IdCliente == int.Parse(idcliente)).FirstOrDefault();

            if (oCliente.Clave != CN_Cliente.EncriptarClave(claveactual))
            {
                TempData["IdCliente"] = idcliente;
                ViewData["vclave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();

            }
            else if (nuevaclave != confirmarclave)
            {
                TempData["IdCliente"] = idcliente;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            ViewData["vclave"] = "";
            nuevaclave = CN_Cliente.EncriptarClave(nuevaclave);

            string mensaje = string.Empty;

            bool respuesta = new CN_Cliente().CambiarClave(int.Parse(idcliente), nuevaclave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdCliente"] = idcliente;
                ViewBag.Error = mensaje;
            }

            return View();
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}