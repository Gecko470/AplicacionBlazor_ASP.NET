using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MiPrimeraAplicacionBlazor.Server.Clases
{
    public static class Generic
    {
        public static int enviarCorreo(string nombreCorreo, string asunto, string contenido)
        {
            int respuesta = 0;

            try
            {
                string correo = "juangomezsousa@outlook.com";
                string clave = "paloM6597";
                string servidor = "smtp.office365.com";
                int puerto = 25;

                MailMessage mail = new MailMessage();
                mail.Subject = asunto;
                mail.IsBodyHtml = true;
                mail.Body = contenido;
                mail.From = new MailAddress(correo);
                mail.To.Add(new MailAddress(nombreCorreo));

                SmtpClient smtp = new SmtpClient();
                smtp.Host = servidor;
                smtp.EnableSsl = true;
                smtp.Port = puerto;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(correo, clave);

                smtp.Send(mail);
                respuesta = 1;
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
                respuesta = 0;
            }

            return respuesta;
        }
    }
}
