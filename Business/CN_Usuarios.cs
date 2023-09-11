using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Data;
using Entity;

namespace Business
{
    public class CN_Usuarios
    {
        private CD_Usuarios datosUsuarios = new CD_Usuarios();

        public List<Usuario> Listar() {
     
            return datosUsuarios.Listar();
            
        }
        public List<Usuario> ListarIS()
        {

            return datosUsuarios.ListarIS();

        }

        public int RegistrarUsuario(Usuario obj, out string Mensaje) { 

            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres)) {
                Mensaje = "El nombre de usuario no puede ser vacío";
            }
            else if(string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos)) {
                Mensaje = "El apellido del usuario no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Apellidos)){
                Mensaje = "El correo del usuario no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.Direccion) || string.IsNullOrWhiteSpace(obj.Apellidos)){
                Mensaje = "La dirección del usuario no puede ser vacía";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                string clave =  CN_Recursos.GenerarClave();

                //Estructura envio de correo
                string asunto = "Creacion de cuenta";
                string mensaje_correo = "<h3>Su cuenta fue creada correctamente</h3></br><p>Su contraseña para acceder es: !clave! </p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", clave);

                bool respuesta = CN_Recursos.EnviarCorreo(obj.Correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    obj.Clave = CN_Recursos.ConvertirSha256(clave);
                    return datosUsuarios.RegistrarUsuario(obj, out Mensaje);
                }
                else {
                    Mensaje = "No se puede enviar el correo";
                    return 0;
                }
            }
            else {
                return 0;
            }
        }
        public bool EditarUsuario(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre de usuario no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del usuario no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El correo del usuario no puede ser vacío";
            }
            else if (string.IsNullOrEmpty(obj.Direccion) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "La dirección del usuario no puede ser vacía";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return datosUsuarios.EditarUsuario(obj, out Mensaje);
            }
            else {
                return false;
            }
        }

        public bool Eliminar(int id, out string Mensaje) { 
            return datosUsuarios.EliminarUsuario(id, out Mensaje);
        }

        //Autenticacion
        public bool CambiarClave(int idusuario, string nuevaclave, out string mensaje)
        {
            return datosUsuarios.CambiarClave(idusuario, nuevaclave, out mensaje);
        }
        public Boolean ReestablecerClave(int idusuario, string correo, out string Mensaje)
        {

            Mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();
            bool resultado = datosUsuarios.ReestablecerClave(idusuario, CN_Recursos.ConvertirSha256(nuevaclave),  out Mensaje);

            if (resultado)
            {
                string asunto = "Contraseña reestablecida";
                string mensaje_correo = "<h3>Su cuenta fue reestablecida correctamente</h3></br><p>Su contraseña para acceder es: !clave! </p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", nuevaclave);

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
            else {
                Mensaje = "No se pudo reestablecer la contraseña";
                return false;
            }
        }

    }
}
