﻿using Ekipa.Models;
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
                ViewBag.UserRole = 3;
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var customer = db.Customers.FirstOrDefault(u => u.Login.Equals(login));
                    ViewBag.UserRole = customer.RoleId;
                    var termList = db.Terms.Where(t => t.CustomerId == customer.ID).ToList<Term>();

                    foreach (var item in termList)
                    {
                        var reservationDB = db.Reservations.FirstOrDefault(r => r.TermId == item.Id && r.IsDelete == false);
                        var opinion = db.Opinions.FirstOrDefault(o => o.ReservationId == reservationDB.Id);
                        bool opinionAdded = false;
                        if (opinion != null)
                        {
                            opinionAdded = true;
                        }
                        if (reservationDB.IsDelete == false)
                        {
                            var companyDB = db.Companies.FirstOrDefault(c => c.Id == item.CompanyId);
                            ReservationVM reservationVM = new ReservationVM()
                            {
                                DescriptionCompany = reservationDB.DescriptionCompany,
                                DescriptionCustomer = reservationDB.DescriptionCustomer,
                                CompanyAccept = reservationDB.CompanyAccept,
                                DateStart = item.DateStart,
                                DateStop = item.DateStop,
                                CustomerName = customer.Name,
                                CompanyName = companyDB.CompanyName,
                                CompanyId = item.CompanyId,
                                CustomerId = customer.ID,
                                TermId = item.Id,
                                OpinionAdded = opinionAdded
                            };
                            resList.Add(reservationVM);
                        }
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
            ViewBag.Role = 4;
            ReservationVM res = new ReservationVM();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var dbReservation = db.Reservations.FirstOrDefault(u => u.TermId == id);
                res.TermId = dbReservation.TermId;
                res.DescriptionCustomer = dbReservation.DescriptionCustomer;
                res.DescriptionCompany = dbReservation.DescriptionCompany;
                res.CompanyAccept = dbReservation.CompanyAccept;
                res.CustomerName = dbReservation.Term.Customer.Name + " " + dbReservation.Term.Customer.Surname;
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
                return RedirectToAction("CompanyTermList", "Company");

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
    }
}


