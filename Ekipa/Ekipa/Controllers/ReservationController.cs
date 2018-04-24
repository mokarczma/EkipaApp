using Ekipa.Models;
using Ekipa.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ekipa.Controllers
{
    public class ReservationController : Controller
    {
        // GET: Reservation
        [HttpGet]
        public ActionResult MyReservation()
        {
            var user = User as MPrincipal;
            if (user != null)
            {
                List<Reservation> resList = new List<Reservation>();
                var login = user.UserDetails.Login;
                ViewBag.UserName = user.UserDetails.Login;
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var customer = db.Customers.FirstOrDefault(u => u.Login.Equals(login));
                    ViewBag.UserRole = customer.ID;
                    //resList = customer.ReservationList.ToList();
                }
                return View(resList);
            }
            return View();
        }
    }
}