using Business;
using Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data;
using System.Text;
using System.Web.Script.Serialization;

namespace Xarpexpress.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult IniciarSesion()
        {
            return View();
        }
        public ActionResult RegistroUsuario()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]

        public ActionResult Productos()
        {
            return View();
        }

        [Authorize]
        public ActionResult Valoraciones()
        {
            return View();
        }

        [Authorize]
        public ActionResult TipoCambio()
        {
            return View();
        }

        [Authorize]
        public ActionResult Perfil()
        {
            return View();
        }

        [Authorize]
        public ActionResult Carrito()
        {
            return View();
        }

        [Authorize]
        public ActionResult PasarelaPago()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerProvincias()
        {
            List<Provincia> listaProvincias = new CN_Cliente().ObtenerProvincias();
            return Json(listaProvincias, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerCantones(int idProvincia)
        {
            List<Canton> listaCantones = new CN_Cliente().ObtenerCantones(idProvincia);
            return Json(listaCantones, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerDistritos(int idCanton)
        {
            List<Distrito> listaDistritos = new CN_Cliente().ObtenerDistritos(idCanton);
            return Json(listaDistritos, JsonRequestBehavior.AllowGet);
        }

        //Carrito de compras
        [HttpGet]
        public JsonResult ListarCarrito()
        {

            List<Carrito> oLista = new List<Carrito>();

            oLista = new CN_Cliente().ListarCarrito();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AgregaralCarrito(int IdProducto)
        {
            object resultado;


            resultado = new CN_Cliente().AgregaralCarrito(IdProducto);


            return Json(new { result = resultado }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<JsonResult> ListarCarritoPython()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {

                    string apiEndpoint = "http://127.0.0.1:5000/listaCarrito";
                    HttpResponseMessage response = await client.GetAsync(apiEndpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        List<Carrito> carritoList = new List<Carrito>();
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        // Deserialize the JSON response into a C# object

                        dynamic resultObject = JsonConvert.DeserializeObject(jsonResponse);
                        if (resultObject != null && resultObject.carrito != null)
                        {
                            foreach (var item in resultObject.carrito)
                            {
                                Carrito carrito = new Carrito
                                {
                                    IdCarrito = item.IdCarrito,
                                    NombreProducto = item.NombreProd,
                                    Precio = item.Precio
                                    // Add more fields if necessary
                                };
                                carritoList.Add(carrito);
                            }

                            return Json(new { data = carritoList }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            string errorMessage = "Objeto de datos vacio: " + response.StatusCode;
                            return Json(new { error = errorMessage }, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        // Handle error response from the Python API
                        string errorMessage = "Error response from the Python API: " + response.StatusCode;
                        return Json(new { error = errorMessage }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the API call
                return Json(new { error = "An error occurred while consuming the Python API: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<JsonResult> VerificarTarjetaPython(string IdTarjeta, string CVV, string fechaVencimiento)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiEndpoint = "http://127.0.0.1:5000/verificarTarjeta";

                    // Create a dictionary to hold the request data
                    var requestData = new Dictionary<string, string>
            {
                { "IdTarjeta", IdTarjeta },
                { "CVV", CVV },
                { "FechaVencimiento", fechaVencimiento }
            };

                    // Serialize the request data to JSON and create HttpContent
                    string jsonRequest = JsonConvert.SerializeObject(requestData);
                    HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                    // Send a POST request to the Python API
                    HttpResponseMessage response = await client.PostAsync(apiEndpoint, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        dynamic resultObject = JsonConvert.DeserializeObject(jsonResponse);

                        // Check if the response contains a 'mensaje' field
                        if (resultObject != null && resultObject.mensaje != null)
                        {
                            string mensaje = resultObject.mensaje;
                            return Json(new { mensaje }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            // Handle invalid response from the Python API
                            string errorMessage = "Invalid response from the Python API.";
                            return Json(new { error = errorMessage }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        // Handle error response from the Python API
                        string errorMessage = "Error response from the Python API: " + response.StatusCode;
                        return Json(new { error = errorMessage }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the API call
                return Json(new { error = "An error occurred while consuming the Python API: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult TipoCambio(string indicador, string fechaInicio, string fechaFinal, string nombre, string correo, string token)
        {
            Cliente datosClientes = new Cliente();
            cr.fi.bccr.gee.wsindicadoreseconomicos clientes = new cr.fi.bccr.gee.wsindicadoreseconomicos();
            DataSet tipoCambioDataSet = clientes.ObtenerIndicadoresEconomicos(indicador, fechaInicio, fechaFinal, nombre, "N", correo, token);

            // Convertir el DataSet a una lista de diccionarios
            List<Dictionary<string, object>> tipoCambioList = new List<Dictionary<string, object>>();
            foreach (DataRow row in tipoCambioDataSet.Tables[0].Rows)
            {
                Dictionary<string, object> rowData = new Dictionary<string, object>();
                foreach (DataColumn col in tipoCambioDataSet.Tables[0].Columns)
                {
                    rowData[col.ColumnName] = row[col];
                }
                tipoCambioList.Add(rowData);
            }

            // Retornar el resultado como JSON
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonData = serializer.Serialize(tipoCambioList);
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult TipoCambioGet(string indicador, string fechaInicio, string fechaFinal, string nombre, string correo, string token)
        {
            Cliente datosClientes = new Cliente();
            cr.fi.bccr.gee.wsindicadoreseconomicos clientes = new cr.fi.bccr.gee.wsindicadoreseconomicos();
            DataSet tipoCambioDataSet = clientes.ObtenerIndicadoresEconomicos(indicador, fechaInicio, fechaFinal, nombre, "N", correo, token);

            // Convertir el DataSet a una lista de diccionarios
            List<Dictionary<string, object>> tipoCambioList = new List<Dictionary<string, object>>();
            foreach (DataRow row in tipoCambioDataSet.Tables[0].Rows)
            {
                Dictionary<string, object> rowData = new Dictionary<string, object>();
                foreach (DataColumn col in tipoCambioDataSet.Tables[0].Columns)
                {
                    rowData[col.ColumnName] = row[col];
                }
                tipoCambioList.Add(rowData);
            }

            // Retornar el resultado como JSON
            return Json(tipoCambioList, JsonRequestBehavior.AllowGet);
        }





    }
}