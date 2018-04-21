using Ekipa.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;

namespace Ekipa.Controllers
{
    public class MessageController : Controller
    {
        // GET: Message
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SendMessage()
        {
            MessageVm message = new MessageVm();
            return View(message);
        }



        [HttpPost]
        public ActionResult SendMessage(MessageVm mess)
        {
            using (SmtpClient client = new SmtpClient())
            {
                //podajemy dane dostępowe
                var credential = new NetworkCredential
                {
                    UserName = "znajdzEkipe.pl@gmail.com",
                    Password = "MessageController"
                };
                client.Credentials = credential;

                //host oraz port poczty,
                //dostawca udostępnia nam te dane
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;

                //tworzymy wiadomość
                var message = new MailMessage();

                //dodajemy odbiorców
                message.To.Add(new MailAddress("znajdzEkipe.pl@gmail.com"));
                //podajemy adres nadawcy
                message.From = new MailAddress("kamoniska@gmail.com");
                //Tytuł wiadomości
                message.Subject = "Tytuł nowej wiadomości";
                message.Body = "Wiadomość testowa";
                //Możemy uzyć znaczników html wewnątrz ciała wiadomości (parametr Body), w tym celu ustawiamy parametr na true
                message.IsBodyHtml = true;

                //Opcjonalnie możemy również dodać załącznik
                //Attachment a = new Attachment("zdjecie.jpg", System.Net.Mime.MediaTypeNames.Image.Jpeg);
                //message.Attachments.Add(a);

                client.Send(message);

                return View();
            }
        }
    }
}