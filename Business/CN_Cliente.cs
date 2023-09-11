using Data;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace Business
{
    public class CN_Cliente
    {
        private CD_Cliente datosClientes = new CD_Cliente();

        public List<Cliente> Listar()
        {

            return datosClientes.Listar();

        }
        public List<Carrito> ListarCarrito()
        {

            return datosClientes.ListarCarrito();

        }
        public bool EliminarProdCarrito(int idCarrito)
        {
            return datosClientes.EliminarProdCarrito(idCarrito);
        }

        public List<Provincia> ObtenerProvincias()
        {

            return datosClientes.ObtenerProvincias();

        }

        public List<Canton> ObtenerCantones(int idProvincia)
        {

            return datosClientes.ObtenerCantones(idProvincia);

        }

        public List<Distrito> ObtenerDistritos(int idCanton)
        {

            return datosClientes.ObtenerDistritos(idCanton);

        }
        public List<Cliente> ListarIS()
        {

            return datosClientes.ListarIS();
        }

        public static string EncriptarClave(string password)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri($"http://127.0.0.1:5000/Convsha256/{password}");
                request.Method = HttpMethod.Get;

                request.Headers.Add("Accept", "*/*");
                request.Headers.Add("User-Agent", "Thunder Client (https://www.thunderclient.com)");

                var response = client.SendAsync(request).Result;

                // Verificar si la solicitud fue exitosa
                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            catch (HttpRequestException ex)
            {
                // Capturar errores de solicitud HTTP
                Console.WriteLine("Error en la solicitud HTTP: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Capturar otros errores
                Console.WriteLine("Error inesperado: " + ex.Message);
            }

            // Si ocurrió un error, retornar un valor predeterminado o lanzar una excepción adecuada según tu necesidad
            return null;
        }


        public int RegistrarCliente(Cliente obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del cliente no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del cliente no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El correo del cliente no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.Direccion) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "La dirección del cliente no puede ser vacía";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                obj.Clave = EncriptarClave(obj.Clave);

                return datosClientes.RegistrarCliente(obj, out Mensaje);

            }
            else
            {
                return 0;
            }
        }
        public bool AgregaralCarrito(int IdProducto)
        {

            return datosClientes.AgregarProductoAlCarrito(IdProducto);
        }

        public int RegAuditoriaISCliente(int id)
        {
            return datosClientes.RegAuditoriaISCliente(id);
        }


        //Autenticacion
        public bool CambiarClave(int idusuario, string nuevaclave, out string mensaje)
        {
            return datosClientes.CambiarClave(idusuario, nuevaclave, out mensaje);
        }
        public Boolean ReestablecerClave(int idusuario, string correo, out string Mensaje)
        {

            Mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();
            bool resultado = datosClientes.ReestablecerClave(idusuario, CN_Recursos.ConvertirSha256(nuevaclave), out Mensaje);

            if (resultado)
            {

                string asunto = "Contraseña reestablcecida";
                string mensaje_correo = @"<!DOCTYPE html>
                                            <html>
                                            <head>
                                              <title>Plantilla de Correo Electrónico</title>
                                              <style>
                                                /* Estilos generales */
                                                body {
                                                  font-family: Arial, sans-serif;
                                                  background-color: #f5f5f5;
                                                  margin: 0;
                                                  padding: 0;
                                                }
    
                                                .container {
                                                  max-width: 600px;
                                                  margin: 0 auto;
                                                  background-color: #fff;
                                                  padding: 20px;
                                                  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                                                }
    
                                                /* Encabezado */
                                                .header {
                                                  text-align: center;
                                                  margin-bottom: 20px;
                                                }
    
                                                .logo {
                                                  max-width: 200px;
                                                  height: auto;
                                                }
    
                                                .company-name {
                                                  font-size: 24px;
                                                  font-weight: bold;
                                                  margin-top: 10px;
                                                }
    
                                                /* Contenido */
                                                .content {
                                                  margin-bottom: 20px;
                                                }
    
                                                .content p {
                                                  margin-bottom: 10px;
                                                }
    
                                                /* Pie de página */
                                                .footer {
                                                  text-align: center;
                                                  font-size: 14px;
                                                  color: #999;
                                                }
                                              </style>
                                            </head>
                                            <body>
                                              <div class=""container"">
                                                <div class=""header"">
                                                  <img class=""logo"" src=""https://xarpexpress.com/wp-content/uploads/2023/05/logo-xarpe-blanco-02-1-2048x1484.png"" alt=""Logo"">
                                                  <h1 class=""company-name"">XarpeXpress</h1>
                                                </div>
    
                                                <div class=""content"">
                                                  <p>¡Gracias por elegir nuestros servicios! Estamos encantados de atender sus necesidades.</p>
                                                  <p>Su código para acceder es: " + nuevaclave + @"</p>
                                                </div>
    
                                                <div class=""footer"">
                                                  <p>&copy; 2023 XarpeXpress. Todos los derechos reservados.</p>
                                                </div>
                                              </div>
                                            </body>
                                            </html>";

                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    return true;

                }
                else
                {
                    Mensaje = "No se puedo enviar el correo";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se pudo reestablecer la contraseña";
                return false;
            }
        }

        public bool EnviarVerificacion(int idCliente, string correo, out string mensaje)
        {
            Cliente cliente = new Cliente();
            mensaje = string.Empty;
            cliente.CodigoVerificacion = CN_Recursos.GenerarClave();

            bool codigoInsertado = datosClientes.InsertarCodigo(idCliente, cliente.CodigoVerificacion, out mensaje);

            //Verificacion codigo insertado en la base de datos
            if (codigoInsertado)
            {
                string asunto = "Verificación de cuenta";
                string mensajeCorreo = @"<!DOCTYPE html>
                                            <html>
                                            <head>
                                              <title>Plantilla de Correo Electrónico</title>
                                              <style>
                                                /* Estilos generales */
                                                body {
                                                  font-family: Arial, sans-serif;
                                                  background-color: #f5f5f5;
                                                  margin: 0;
                                                  padding: 0;
                                                }
    
                                                .container {
                                                  max-width: 600px;
                                                  margin: 0 auto;
                                                  background-color: #fff;
                                                  padding: 20px;
                                                  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                                                }
    
                                                /* Encabezado */
                                                .header {
                                                  text-align: center;
                                                  margin-bottom: 20px;
                                                }
    
                                                .logo {
                                                  max-width: 200px;
                                                  height: auto;
                                                }
    
                                                .company-name {
                                                  font-size: 24px;
                                                  font-weight: bold;
                                                  margin-top: 10px;
                                                }
    
                                                /* Contenido */
                                                .content {
                                                  margin-bottom: 20px;
                                                }
    
                                                .content p {
                                                  margin-bottom: 10px;
                                                }
    
                                                /* Pie de página */
                                                .footer {
                                                  text-align: center;
                                                  font-size: 14px;
                                                  color: #999;
                                                }
                                              </style>
                                            </head>
                                            <body>
                                              <div class=""container"">
                                                <div class=""header"">
                                                  <img class=""logo"" src=""https://xarpexpress.com/wp-content/uploads/2023/05/logo-xarpe-blanco-02-1-2048x1484.png"" alt=""Logo"">
                                                  <h1 class=""company-name"">XarpeXpress</h1>
                                                </div>
    
                                                <div class=""content"">
                                                  <p>¡Gracias por elegir nuestros servicios! Estamos encantados de atender sus necesidades.</p>
                                                  <p>Su código para acceder es: " + cliente.CodigoVerificacion + @"</p>
                                                </div>
    
                                                <div class=""footer"">
                                                  <p>&copy; 2023 XarpeXpress. Todos los derechos reservados.</p>
                                                </div>
                                              </div>
                                            </body>
                                            </html>";
                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensajeCorreo);

                //Verificacion envio de correo
                if (respuesta)
                {
                    return true;
                }
                else
                {
                    mensaje = "No se pudo enviar el correo";
                    return false;
                }
            }
            else
            {
                mensaje = "No se pudo enviar el código de verificación";
                return false;
            }

        }

        public string CodigoVerificacion(int idcliente)
        {
            return datosClientes.CodigoVerificacion(idcliente);
        }

        public bool EliminarCodigo(int id)
        {
            return datosClientes.EliminarCodigo(id);
        }

    }
}
