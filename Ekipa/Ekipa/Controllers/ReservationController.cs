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
                    var company = db.Companies.FirstOrDefault(u => u.Login.Equals(login));

                    List<Term> termList = new List<Term>();

                    if (customer != null)
                    {
                        ViewBag.UserRole = customer.RoleId;
                        termList = db.Terms.Where(t => t.CustomerId == customer.ID).ToList<Term>();
                    }
                    if (company != null)
                    {
                        ViewBag.UserRole = company.RoleId;
                        termList = db.Terms.Where(t => t.CompanyId == company.Id && t.CustomerId != null).ToList<Term>();

                    }

                    foreach (var item in termList)
                    {
                        var reservationDB = db.Reservations.FirstOrDefault(r => r.TermId == item.Id && r.IsDelete == false);
                        var opinion = db.Opinions.FirstOrDefault(o => o.ReservationId == reservationDB.Id);
                        bool opinionAdded = false;
                        if (opinion != null)
                        {
                            opinionAdded = true;
                        }
                   
                            var companyDB = db.Companies.FirstOrDefault(c => c.Id == item.CompanyId);
                            var customerDB = db.Customers.FirstOrDefault(c => c.ID == item.CustomerId);
                            ReservationVM reservationVM = new ReservationVM()
                            {
                                DescriptionCompany = reservationDB.DescriptionCompany,
                                DescriptionCustomer = reservationDB.DescriptionCustomer,
                                CompanyAccept = reservationDB.CompanyAccept,
                                DateStart = item.DateStart.ToShortDateString(),
                                DateStop = item.DateStop.ToShortDateString(),
                                CustomerName = customerDB.Name + " " + customerDB.Surname,
                                CompanyName = companyDB.CompanyName,
                                CompanyId = item.CompanyId,
                                CustomerId = customerDB.ID,
                                TermId = item.Id,
                                OpinionAdded = opinionAdded
                            };
                            resList.Add(reservationVM);
                    }
                }
                return View(resList);
            }
            return View();
        }


        [HttpGet]
        public ActionResult AddOpinion(int idTerm)
        {
            var user = User as MPrincipal;
            var login = user.UserDetails.Login;
            ViewBag.UserName = user.UserDetails.Login;
            ViewBag.UserRole = 3;

            OpinionVM opinion = new OpinionVM();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var term = db.Terms.FirstOrDefault(u => u.Id.Equals(idTerm));
                var comp = db.Companies.FirstOrDefault(c => c.Id == term.CompanyId);
                var res = db.Reservations.FirstOrDefault(r => r.TermId == idTerm);
                opinion.CompanyId = comp.Id;
                opinion.CompanyName = comp.CompanyName;
                opinion.ReservationId = res.Id;
            }
            return View(opinion);
        }

        [HttpPost]
        public ActionResult AddOpinion(OpinionVM model)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Opinion opinion = new Opinion
                    {
                        Description = model.Description,
                        CompanyId = model.CompanyId,
                        GradeValue = model.GradeValue,
                        ReservationId = model.ReservationId
                    };
                    db.Opinions.Add(opinion);
                    db.SaveChanges();
                }
                return RedirectToAction("MyReservation");
            }

            return View(model);
        }
        [HttpGet]
        public ActionResult CompanyReservation(int id)
        {
            var user = User as MPrincipal;
            var login = user.UserDetails.Login;
            ViewBag.UserName = user.UserDetails.Login;
            ViewBag.UserRole = 4;
            ReservationVM res = new ReservationVM();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var dbReservation = db.Reservations.FirstOrDefault(u => u.TermId == id);
                res.TermId = dbReservation.TermId;
                res.DescriptionCustomer = dbReservation.DescriptionCustomer;
                res.DescriptionCompany = dbReservation.DescriptionCompany;
                res.CompanyAccept = dbReservation.CompanyAccept;
                res.CustomerName = dbReservation.Term.Customer.Name + " " + dbReservation.Term.Customer.Surname;
                res.CustomerId = dbReservation.Term.Customer.ID;
                res.CustomerNumber = dbReservation.Term.Customer.PhoneNumber;
                res.DateStart = dbReservation.Term.DateStart.ToShortDateString();
                res.DateStop = dbReservation.Term.DateStop.ToShortDateString();
            }
            return View(res);
        }
        [HttpPost]
        public ActionResult CompanyReservation(ReservationVM model)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var dbReservation = db.Reservations.FirstOrDefault(u => u.TermId == model.Id && u.IsDelete == false);
                    dbReservation.DescriptionCompany = model.DescriptionCompany ?? "";
                    dbReservation.CompanyAccept = model.CompanyAccept;
                    dbReservation.IsDelete = model.IsDelete;
                    db.SaveChanges();
                }
                return RedirectToAction("CompanyTermList", "Term");

            }

            return View(model);
        }
        [HttpGet]
        public ActionResult CancelReservation(ReservationVM reservation)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var reservationDB = db.Reservations.FirstOrDefault(u => u.TermId == reservation.TermId);
                reservationDB.IsDelete = true;
                int termId = reservationDB.TermId;
                var termDB = db.Terms.FirstOrDefault(t => t.Id == termId);
                termDB.CustomerId = null;
                db.SaveChanges();
            }
            return RedirectToAction("CompanyTermList", "Company");
        }
        [HttpGet]
        public ActionResult CustomerReservation(int id)
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
            res.TermId = id;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var cust = db.Customers.FirstOrDefault(u => u.Login.Equals(login));
                if (cust == null)
                {
                    TempData["alertMessage"] = "Termin możesz zarezerwować tylko jako kient";
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.UserRole = cust.RoleId;
                res.CustomerId = cust.ID;
                var term = db.Terms.FirstOrDefault(u => u.Id == id);
                res.CompanyName = term.Company.CompanyName;
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
                    var termDB = db.Terms.FirstOrDefault(t => t.Id.Equals(model.TermId));
                    termDB.CustomerId = model.CustomerId;
                    Reservation newReservation = new Reservation()
                    {
                        DescriptionCustomer = model.DescriptionCustomer,
                        Term = termDB,
                        TermId = termDB.Id,
                    };
                    db.Reservations.Add(newReservation);
                    db.SaveChanges();
                }
                TempData["alertMessage"] = "Zarezerwowano";
                return RedirectToAction("MyReservation", "Reservation");
            }
            return View(model);
        }

        }
    }


