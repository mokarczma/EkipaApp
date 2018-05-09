using Ekipa.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using Ekipa.Models;

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
        public ActionResult Message(MessageVm model)
        {
                var user = User as MPrincipal;
                var login = user.UserDetails.Login;
                ViewBag.UserName = user.UserDetails.Login;
                ViewBag.UserRole = model.UserRoleId;
                return View(model);
        }


        [HttpPost]
        public ActionResult SendMessage(MessageVm model)
        {
            if (ModelState.IsValid)
            {
                using (SmtpClient client = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "znajdzEkipe.pl@gmail.com",
                        Password = "MessageController"
                    };
                    client.Credentials = credential;

                    client.Host = "smtp.gmail.com";
                    client.Port = 587;
                    client.EnableSsl = true;

                    var message = new MailMessage();

                    message.To.Add(new MailAddress(model.AddresseeEmail));
                    message.From = new MailAddress(model.SenderEmail);

                    message.Subject = "ZnajdzEkipe.pl - wiadomość od uzytownika:" + model.SenderName;
                    message.Body = model.Description + "<br/> <br/> Adres emailowy użytkownika " + model.SenderName + ": " + model.SenderEmail;

                    message.IsBodyHtml = true;
                    client.Send(message);
                    TempData["alertMessage"] = "Wiadomość została wysłana";
                }
            }
            else
            {
                TempData["alertMessage"] = "Wiadomość zawiera nieodpowiednie znaki";
            }
            if (model.CompanyId > 0)
            {
                return RedirectToAction("InfoAboutCompany", "PublicCompany", new { id = model.CompanyId });
            }
            return RedirectToAction("CompanyTermList", "Term");

        }

        [HttpGet]
        public ActionResult MessageFromCustomer(int idCompany)
        {
            MessageVm message = new MessageVm();
            var user = User as MPrincipal;
            if (user == null)
            {
                TempData["alertMessage"] = "Zaloguj się, aby wysłać wiadomość";
                return RedirectToAction("LoginCustomer", "Account");
            }
            {
                var login = user.UserDetails.Login;
                ViewBag.UserName = user.UserDetails.Login;
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var comp = db.Companies.FirstOrDefault(u => u.Id.Equals(idCompany));
                    var cust = db.Customers.FirstOrDefault(u => u.Login.Equals(login));

                    if (cust == null)
                    {
                        TempData["alertMessage"] = "Wiadomość do firmy możesz wysłać tylko jako kient";
                        return RedirectToAction("Index", "Home");
                    }

                    message.SenderName = cust.Name + " " + cust.Surname;
                    message.SenderEmail = cust.Email;
                    message.AddresseeName = comp.CompanyName;
                    message.AddresseeEmail = comp.Email;
                    message.CompanyId = idCompany;
                    message.UserRoleId = cust.RoleId;
                }
            }
            return RedirectToAction("Message",message);
        }
        [HttpGet]
        public ActionResult MessageFromCompany(int idCustomer)
        {
            MessageVm message = new MessageVm();
            var user = User as MPrincipal;
            {
                var login = user.UserDetails.Login;
                ViewBag.UserName = user.UserDetails.Login;
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var comp = db.Companies.FirstOrDefault(u => u.Login == login);
                    var cust = db.Customers.FirstOrDefault(u => u.ID == idCustomer);
                    message.AddresseeName = cust.Name + " " + cust.Surname;
                    message.AddresseeEmail = cust.Email;
                    message.SenderName = comp.CompanyName;
                    message.SenderEmail = comp.Email;
                    message.UserRoleId = comp.RoleId;
                   
                }
            }
            return RedirectToAction("Message", message);
        }
    }
}