using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using System.Data.SqlClient;
using System.Data;

namespace Data
{
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {
            List<Usuario> listaUsuarios = new List<Usuario>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select IdUsuario, Nombres, Apellidos, Correo, Direccion, Reestablecer, Activo from USUARIO";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listaUsuarios.Add(new Usuario
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Direccion = dr["Direccion"].ToString(),
                                Reestablecer = Convert.ToBoolean(dr["Reestablecer"]),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                listaUsuarios = new List<Usuario>();

            }
            return listaUsuarios;
        }
        public List<Usuario> ListarIS()
        {
            List<Usuario> listaUsuarios = new List<Usuario>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select IdUsuario, Nombres, Apellidos, Correo, Clave, Direccion, Reestablecer, Activo from USUARIO";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listaUsuarios.Add(new Usuario
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Direccion = dr["Direccion"].ToString(),
                                Reestablecer = Convert.ToBoolean(dr["Reestablecer"]),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                listaUsuarios = new List<Usuario>();

            }
            return listaUsuarios;
        }

        public int RegistrarUsuario(Usuario obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Direccion", obj.Direccion);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
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

        public bool EditarUsuario(Usuario obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("@IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("@Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("@Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("@Direccion", obj.Direccion);
                    cmd.Parameters.AddWithValue("@Activo", obj.Activo);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }





        public bool EliminarUsuario(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("delete top (1) from usuario where IdUsuario = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", id);
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


        public bool CambiarClave(int idusuario, string nuevaclave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update USUARIO set Clave = @nuevaclave, reestablecer = 0 where IdUsuario = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", idusuario);
                    cmd.Parameters.AddWithValue("@nuevaclave", nuevaclave);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }

        public bool ReestablecerClave(int idusuario, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update USUARIO set Clave = @clave, reestablecer = 1 where IdUsuario = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", idusuario);
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
    }
}

