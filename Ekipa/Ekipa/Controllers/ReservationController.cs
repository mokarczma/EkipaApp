using Ekipa.Models;
using Ekipa.Models.DB;
using Ekipa.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ekipa.Controllers
{
    public class ReservationController : Controller
    {
        // GET: Rezervation
        [HttpGet]
        public ActionResult CustomerReservation(int termId)
        {
            var user = User as MPrincipal;
            if (user == null)
            {
                TempData["alertMessage"] = "Zaloguj się, aby zarezerwować termin";
                return RedirectToAction("LoginCustomer", "Account");
            }
            var login = user.UserDetails.Login;
            ViewBag.UserName = user.UserDetails.Login;
            ReservationVM res = new ReservationVM();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var cust = db.Customers.FirstOrDefault(u => u.Login.Equals(login));
                ViewBag.Role = cust.RoleId;
                if (ViewBag.Role = 6)
                {
                    TempData["alertMessage"] = "Termin możesz zarezerwować tylko jako kient";
                    return RedirectToAction("Index", "Home");
                }
                var term = db.CompanyTerm.FirstOrDefault(t => t.Id.Equals(termId));
                term.CustomerId = cust.ID;
                res.Term = term;
            }

            return View(res);
        }
        [HttpPost]
        public ActionResult CustomerReservation(ReservationVM model)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var termDB = db.CompanyTerm.FirstOrDefault(t => t.Id.Equals(model.Term.Id));
                    termDB.CustomerId = model.Term.CustomerId;
                    Reservation newReservation = new Reservation()
                    {
                        CompanyId = model.Term.CompanyId,
                        DescriptionCustomer = model.DescriptionCustomer,
                    };
                    db.Rezervations.Add(newReservation);
                    db.SaveChanges();
                }
                TempData["alertMessage"] = "Zarezerwowano";
                return RedirectToAction("MyReservation", "Home");

            }
            return View(model);


        }
    }
}