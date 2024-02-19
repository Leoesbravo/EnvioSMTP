using Microsoft.AspNetCore.Mvc;
using PL.Models;
using System.Net.Mail;
using System.Web;

namespace PL.Controllers
{
    public class LoginController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public LoginController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        //Constructor
        //Patrones de diseño
            //
        //inyeccion de dependencias --
        //agregar la dependencia de una interfaz al constructor de nuestra clase
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult RecuperarPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RecuperarPassword(string email)
        {
            //llamar al getbyemail
            //en caso de que sea valido
            EnviarMail(email);
            return View();
                //devolver a la vista
        }
        //public ActionResult EnviarEmail(string destinatario)
        //{

        //}
        public ActionResult EnviarMail(string email)
        {
            //llamar al metodo
            string emailOrigen = "leoebravo@gmail.com";

            MailMessage mailMessage = new MailMessage(emailOrigen, email, "Recuperar Contraseña", "<p>Correo para recuperar contraseña</p>");
            mailMessage.IsBodyHtml = true;

            //string contenidoHTML = System.IO.File.ReadAllText(@"C:\users\digis\Documents\IISExpress\Leonardo Escogido Bravo\Proyecto2023Ecommerce\PL\Views\Usuario\Email.html");

            //string contenidoHTML = System.IO.File.ReadAllText(Path.Combine("Views", "Usuario", "Email.html"));

            //mandemos a llamar a nuestro template
            string contenidoHTML = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "Templates", "Email.html"));
            //sin template
            //string contenidoHTML = "<table> etc";




            mailMessage.Body = contenidoHTML;
            //string url = "http://localhost:5057/Usuario/NewPassword/" + HttpUtility.UrlEncode(email);
            string url = "http://192.168.0.104/Usuario/NewPassword/" + HttpUtility.UrlEncode(email);
            mailMessage.Body = mailMessage.Body.Replace("{Url}", url);
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, "kuwzpcgwbrmueviw");

            smtpClient.Send(mailMessage);
            smtpClient.Dispose();

            ViewBag.Modal = "show";
            ViewBag.Mensaje = "Se ha enviado un correo de confirmación a tu correo electronico";
            return View();
        }


    }
}
