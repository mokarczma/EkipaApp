using Ekipa.Models;
using Ekipa.Models.DB;
using Ekipa.Models.ViewModel.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ekipa.Controllers
{
    public class TermController : Controller
    {
        [HttpGet]
        public ActionResult AddCompanyTerm()
        {
            var user = User as MPrincipal;
            var login = user.UserDetails.Login;
            ViewBag.UserName = user.UserDetails.Login;
            ViewBag.UserRole = 4;
            return View();
        }

        [HttpGet]
        public ActionResult DeleteTerm(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var term = db.Terms.FirstOrDefault(t => t.Id == id);
                term.IsDelete = true;
                db.SaveChanges();
            }
            return RedirectToAction("CompanyTermList", "Term");
        }

        public static IEnumerable<DateTime> DateRangeToArray(DateTime start,
                                                     DateTime end)
        {
            DateTime curDate = start;
            while (curDate <= end)
            {
                yield return curDate;
                curDate = curDate.AddDays(1);
            }
        }

        [HttpPost]
        public ActionResult AddCompanyTerm(CompanyTermVM model)
        {
            var user = User as MPrincipal;
            var login = user.UserDetails.Login;
            ViewBag.UserName = user.UserDetails.Login;
            ViewBag.UserRole = 4;

            if (ModelState.IsValid)
            {
                if (model.DateStop < model.DateStart)
                {
                    ModelState.AddModelError("DateStop", "Data końca nie może być wcześniejsza od daty początku");
                    return View(model);
                }

                using (ApplicationDbContext db = new ApplicationDbContext())
                {

                    var company = db.Companies.SingleOrDefault(x => x.Login == login);

                    if (company == null)
                    {
                        return View(model);
                    }
                    //sprawdzanie czy ten dzień nie ma już terminu

                    IEnumerable<DateTime> dniTworzone = DateRangeToArray(model.DateStart, model.DateStop).ToList();
                    var termList = db.Terms.Where(t => t.CompanyId == company.Id).ToList();

                    foreach (var item in termList)
                    {
                        IEnumerable<DateTime> dateTimesZajete = DateRangeToArray(item.DateStart, item.DateStop).ToList();
                        foreach (var zajte in dateTimesZajete)
                        {
                            foreach (var tworzone in dniTworzone)
                            {
                                if (tworzone == zajte)
                                {
                                    ModelState.AddModelError("DateStop", "W terminie, który chcesz utworzyć, występują dni, które już zaplanowałeś, sprawdź inne terminy");
                                    return View(model);
                                }
                            }
                        }
                    }

                    var companyTerms = new Term()
                    {
                        CompanyId = company.Id,
                        DateStart = model.DateStart,
                        DateStop = model.DateStop,
                    };

                    company.CompanyTerms.Add(companyTerms);

                    db.SaveChanges();
                }
            }
            return View("");
        }
        [HttpGet]
        [ActionName("CompanyTermList")]
        public ActionResult CompanyTermList()
        {
            var user = User as MPrincipal;
            var login = user.UserDetails.Login;
            ViewBag.UserName = user.UserDetails.Login;
            ViewBag.UserRole = 4;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var company = db.Companies.SingleOrDefault(x => x.Login == login);

                List<CompanyTermVM> compTerm = CompanyTermToList(company.Id);
                if (compTerm == null)
                {
                    ViewBag.NoTerm = true;
                    return View("");
                }
                CompanyTermsVM companyTermsVM = new CompanyTermsVM() { CompanyTermsList = compTerm };
                return View(companyTermsVM);
            }
        }
            public  static List<CompanyTermVM> CompanyTermToList(int companyID)
            {
                List<CompanyTermVM> compTermsList = new List<CompanyTermVM>();

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var company = db.Companies.SingleOrDefault(x => x.Id == companyID);
                    var termList = db.Terms.Where(t => t.CompanyId == company.Id && t.IsDelete == false).OrderBy(x => x.DateStart).ToList();

                    foreach (var item in termList)
                    {
                        bool actual = false;
                        if (item.DateStart > DateTime.Now || (item.DateStart < DateTime.Now && item.DateStop > DateTime.Now))
                        {
                            actual = true;
                        }

                        var rez = db.Reservations.FirstOrDefault(x => x.TermId == item.Id);
                        int reservationId = 0;
                        bool acceptedRez = false;

                        if (rez != null)
                        {
                            reservationId = rez.Id;
                            acceptedRez = rez.CompanyAccept;
                        }

                        CompanyTermVM TermVM = new CompanyTermVM
                        {
                            ID = item.Id,
                            DateStart = item.DateStart,
                            DateStop = item.DateStop,
                            CustomerID = item.CustomerId,
                            Actual = actual,
                            ReservationId = reservationId,
                            Accepted = acceptedRez
                        };
                        compTermsList.Add(TermVM);
                    }
                }
                return compTermsList;
            }
        }
    }

