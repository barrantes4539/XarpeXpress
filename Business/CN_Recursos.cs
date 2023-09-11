﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Entity;
using System.Net.Http;

namespace Business
{
    public class CN_Recursos
    {
        public static string GenerarClave() { 
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);
            return clave;
        }
        public static string ConvertirSha256(string texto) { 
            StringBuilder Sb = new StringBuilder();

            //REFERENCIA DE "System.Security.Cryptography"
            using (SHA256 hash = SHA256Managed.Create()) { 
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                {
                    Sb.Append(b.ToString("x2"));
                }

                return Sb.ToString();
            }
        }

        public static bool EnviarCorreo(string correo, string asunto, string mensaje) {


            bool resultado = false;

            try { 
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("progra5Kevin@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("progra5Kevin@gmail.com", "mrpielcktknnidwc"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };

                smtp.Send(mail);
                resultado = true;
            }catch(Exception ex){
                resultado = false;
            }
            return resultado;
        }


        //Funcion que permite manejar rutas de imagenes
        public static string ConvertirBase64(string ruta, out bool conversion) {
            string textoBase64 = string.Empty;
            conversion = true;

            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {

                conversion = false;
            }
            return textoBase64;

        }
    }
}
