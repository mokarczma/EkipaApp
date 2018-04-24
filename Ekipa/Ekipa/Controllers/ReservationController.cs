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
        // GET: Reservation
        [HttpGet]
        public ActionResult MyReservation()
        {
            var user = User as MPrincipal;
            if (user != null)
            {
                List<ReservationVM> resList = new List<ReservationVM>();
                var login = user.UserDetails.Login;
                ViewBag.UserName = user.UserDetails.Login;
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var customer = db.Customers.FirstOrDefault(u => u.Login.Equals(login));
                    ViewBag.UserRole = customer.RoleId;
                    var termList = db.Terms.Where(t => t.CustomerId == customer.ID).ToList<Term>();
                    foreach (var item in termList)
                    {
                        var reservationDB = db.Rezervations.FirstOrDefault(r => r.TermId == item.Id);
                        if (reservationDB.IsDelete == false)
                        {
                            var companyDB = db.Companies.FirstOrDefault(c => c.Id == item.CompanyId);
                            ReservationVM reservationVM = new ReservationVM()
                            {
                                DescriptionCompany = reservationDB.DescriptionCompany,
                                DescriptionCustomer = reservationDB.DescriptionCustomer,
                                CompanyAccept = reservationDB.CompanyAccept,
                                DateFrom = item.DateFrom,
                                DateTo = item.DateTo,
                                CustomerName = customer.Name,
                                CompanyName = companyDB.CompanyName,
                            };
                            resList.Add(reservationVM);
                        }
                    }
                }
                return View(resList);
            }
            return View();
        }
    }
}