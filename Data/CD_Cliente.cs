using Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class CD_Cliente
    {
        public int RegistrarCliente(Cliente obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarCliente", oconexion);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Direccion", obj.Direccion);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;
            }
            return idautogenerado;
        }
        public int RegAuditoriaISCliente(int IdCliente)
        {
            int idautogenerado = 0;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    oconexion.Open();

                    // Obtener el contador actual de sesiones iniciadas
                    SqlCommand ocmdContador = new SqlCommand("SELECT SesionesIniciadas FROM AUDITORIASCLIENTE WHERE IdUsuario = @IdCliente", oconexion);
                    ocmdContador.Parameters.AddWithValue("@IdCliente", IdCliente);

                    int sesionesIniciadas = Convert.ToInt32(ocmdContador.ExecuteScalar());

                    // Llamar al procedimiento almacenado
                    SqlCommand ocmd = new SqlCommand("sp_RegistroAuditoriaCliente", oconexion);
                    ocmd.CommandType = CommandType.StoredProcedure;

                    ocmd.Parameters.AddWithValue("@IdCliente", IdCliente);

                    ocmd.ExecuteNonQuery();

                    idautogenerado = sesionesIniciadas + 1; // Incrementar el contador localmente
                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
            }

            return idautogenerado;
        }




        public List<Cliente> Listar()
        {
            List<Cliente> listaClientes = new List<Cliente>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select IdCliente, Nombres, Apellidos, Correo, Direccion, Reestablecer from CLIENTE";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listaClientes.Add(new Cliente
                            {
                                IdCliente = Convert.ToInt32(dr["IdCliente"]),
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Direccion = dr["Direccion"].ToString(),
                                Reestablecer = Convert.ToBoolean(dr["Reestablecer"]),
                            });
                        }
                    }
                }
            }
            catch
            {
                listaClientes = new List<Cliente>();

            }
            return listaClientes;
        }

      

        public List<Cliente> ListarIS()
        {
            List<Cliente> listaClientes = new List<Cliente>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select IdCliente, Nombres, Apellidos, Correo, Clave, Direccion, Reestablecer from CLIENTE";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listaClientes.Add(new Cliente
                            {
                                IdCliente = Convert.ToInt32(dr["IdCliente"]),
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Direccion = dr["Direccion"].ToString(),
                                Reestablecer = Convert.ToBoolean(dr["Reestablecer"]),
                            });
                        }
                    }
                }
            }
            catch
            {
                listaClientes = new List<Cliente>();

            }
            return listaClientes;
        }

        public bool CambiarClave(int idcliente, string nuevaclave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_verHistorialClave", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    cmd.Parameters.AddWithValue("@idCliente", idcliente);
                    cmd.Parameters.AddWithValue("@nuevaClave", nuevaclave);

                    // Parámetros de salida
                    SqlParameter resultadoParam = new SqlParameter("@resultado", SqlDbType.Bit);
                    resultadoParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(resultadoParam);

                    SqlParameter mensajeParam = new SqlParameter("@mensaje", SqlDbType.NVarChar, 100);
                    mensajeParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(mensajeParam);

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = (bool)resultadoParam.Value;
                    mensaje = mensajeParam.Value.ToString();

                    // Si la nueva clave es válida, realizar la actualización en la base de datos
                    if (resultado)
                    {
                        cmd = new SqlCommand("UPDATE CLIENTE SET Clave = @nuevaclave, Reestablecer = 0 WHERE IdCliente = @id", oconexion);
                        cmd.Parameters.AddWithValue("@id", idcliente);
                        cmd.Parameters.AddWithValue("@nuevaclave", nuevaclave);
                        cmd.CommandType = CommandType.Text;

                        cmd.ExecuteNonQuery();

                        // Insertar la nueva clave en el historial
                        cmd = new SqlCommand("INSERT INTO HistorialClaves(IdCliente, Clave) VALUES (@IdCliente, @Clave)", oconexion);
                        cmd.Parameters.AddWithValue("@IdCliente", idcliente);
                        cmd.Parameters.AddWithValue("@Clave", nuevaclave);
                        cmd.CommandType = CommandType.Text;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return resultado;
        }



        public bool ReestablecerClave(int idcliente, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update CLIENTE set Clave = @clave, Reestablecer = 1 where IdCliente = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", idcliente);
                    cmd.Parameters.AddWithValue("@clave", clave);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public bool InsertarCodigo(int idCliente, string codigoVerificacion, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE CLIENTE SET CodigoVerificacion = @codigoVerificacion WHERE IdCliente = @idCliente", oConexion);
                    cmd.Parameters.AddWithValue("@codigoVerificacion", codigoVerificacion);
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return resultado;
        }

        public string CodigoVerificacion(int idcliente)
        {
            string codigoVerificacion = null;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SELECT CodigoVerificacion FROM CLIENTE WHERE IdCliente = @idcliente", oConexion);
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        codigoVerificacion = reader["CodigoVerificacion"].ToString();
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
            }

            return codigoVerificacion;
        }

        public List<Provincia> ObtenerProvincias()
        {
            List<Provincia> listaProvincias = new List<Provincia>();

            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT ProvinciaId, Nombre FROM Provincias";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandType = CommandType.Text;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Provincia provincia = new Provincia
                            {
                                ProvinciaId = Convert.ToInt32(reader["ProvinciaId"]),
                                Nombre = reader["Nombre"].ToString()
                            };

                            listaProvincias.Add(provincia);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                listaProvincias = new List<Provincia>();
            }

            return listaProvincias;
        }

        public List<Canton> ObtenerCantones(int provinciaId)
        {
            List<Canton> listaCantones = new List<Canton>();

            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT CantonId, Nombre FROM Cantones WHERE ProvinciaId = @ProvinciaId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@ProvinciaId", provinciaId);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Canton canton = new Canton
                            {
                                CantonId = Convert.ToInt32(reader["CantonId"]),
                                Nombre = reader["Nombre"].ToString()
                            };

                            listaCantones.Add(canton);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                listaCantones = new List<Canton>();
            }

            return listaCantones;
        }

        public List<Distrito> ObtenerDistritos(int cantonId)
        {
            List<Distrito> listaDistritos = new List<Distrito>();

            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT DistritoId, Nombre FROM Distritos WHERE CantonId = @CantonId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@CantonId", cantonId);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Distrito distrito = new Distrito
                            {
                                DistritoId = Convert.ToInt32(reader["DistritoId"]),
                                Nombre = reader["Nombre"].ToString()
                            };

                            listaDistritos.Add(distrito);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                listaDistritos = new List<Distrito>();
            }

            return listaDistritos;
        }






        public bool EliminarCodigo(int idCliente)
        {
            bool resultado = false;
     

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE CLIENTE SET CodigoVerificacion = NULL WHERE IdCliente = @idCliente", oConexion);
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
              
            }

            return resultado;
        }

        public List<Carrito> ListarCarrito()
        {
            List<Carrito> listaCarrito = new List<Carrito>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select IdCarrito, NombreProd, Precio from CARRITO";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listaCarrito.Add(new Carrito
                            {
                                IdCarrito = Convert.ToInt32(dr["IdCarrito"]),
                                NombreProducto = dr["NombreProd"].ToString(),
                                Precio = dr.GetDecimal(dr.GetOrdinal("Precio"))
                            });
                        }
                    }
                }
            }
            catch
            {
                listaCarrito = new List<Carrito>();
            }

            return listaCarrito;
        }


        public bool AgregarProductoAlCarrito(int IdProducto)
        {
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    oconexion.Open();

                    SqlCommand cmd = new SqlCommand("AgregarProductoAlCarrito", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Definir los parámetros necesarios para el procedimiento almacenado
                    cmd.Parameters.AddWithValue("@IdProducto", IdProducto);

                    // Ejecutar el procedimiento almacenado
                    cmd.ExecuteNonQuery();

                    // Si no hubo errores y la ejecución fue exitosa, retornar true
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones: Si ocurre algún error, retornar false
                Console.WriteLine("Error al agregar el producto al carrito: " + ex.Message);
                return false;
            }
        }

        public bool EliminarProdCarrito(int id)
        {
            bool resultado = false;
          

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("delete top (1) from CARRITO where IdCarrito = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
    
            }
            return resultado;
        }

    }



}

