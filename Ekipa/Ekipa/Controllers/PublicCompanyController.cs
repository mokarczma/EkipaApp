﻿using Ekipa.Models;
using Ekipa.Models.DB;
using Ekipa.Models.ViewModel;
using Ekipa.Models.ViewModel.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ekipa.Controllers
{
    public class PublicCompanyController : Controller
    {
        public static CompanyInfoVM CompanyInfo(int companyId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var company = db.Companies.SingleOrDefault(x => x.Id == companyId);

                List<Tag> tagList = new List<Tag>();
                tagList = db.Tags.Where(t => t.CompanyTag.Any(c => c.CompanyId == company.Id)).ToList();

                List<Term> termList = new List<Term>();
                termList = db.Terms.Where(t => t.CompanyId == company.Id).ToList();
                List<CompanyAddTermVM> compTermsList = new List<CompanyAddTermVM>();

                foreach (var item in termList)
                {
                    CompanyAddTermVM TermVM = new CompanyAddTermVM
                    {
                        ID = item.Id,

                        DateStart = item.DateStart,
                        YearFrom = item.DateStart.Year,
                        MonthFrom = item.DateStart.Month.ToString(),
                        DayFrom = item.DateStart.Day,

                        DateStop = item.DateStop,
                        YearTo = item.DateStop.Year,
                        MonthTo = item.DateStop.Month.ToString(),
                        DayTo = item.DateStop.Day,
                    };
                    compTermsList.Add(TermVM);
                }
                List<Image> imageList = new List<Image>();
                imageList = db.Images.Where(t => t.CompanyId == company.Id).ToList();
                CompanyInfoVM companyInfoVM = new CompanyInfoVM()
                {
                    IdCompany = company.Id,
                    CityName = company.City.Name,
                    CompanyName = company.CompanyName,
                    Speciality = company.Speciality,
                    Services = company.Services,
                    Pricing = company.Pricing,
                    PhoneNumer = company.PhoneNumer,
                    CompanyTagList = tagList,
                    CompanyTermVMList = compTermsList,
                    CompanyImageList = imageList
                };
                return companyInfoVM;
            };
        }


        [HttpGet]
        public ActionResult InfoAboutCompany(int id)
        {
            var user = User as MPrincipal;
            if (user != null)
            {
                var login = user.UserDetails.Login;
                ViewBag.UserName = user.UserDetails.Login;


                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var comp = db.Companies.FirstOrDefault(u => u.Login.Equals(login));
                    var cust = db.Companies.FirstOrDefault(u => u.Login.Equals(login));
                    if (cust != null)
                    {
                        ViewBag.UserRole = 3;
                    }
                    else
                    {
                        ViewBag.UserRole = 4;
                    }
                }
            }
            CompanyInfoVM companyInfoVM = CompanyInfo(id);
            return View(companyInfoVM);
        }
        [HttpGet]
        public ActionResult SearchView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchView(MainViewVM model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var dbCompany = db.Companies.Where(t => t.CityId == model.PlaceSearch && t.CompanyName.Contains(model.NameSearch)).ToList<Company>();

                if (model.PlaceSearch == 1)
                {
                    if (model.NameSearch == null)
                    {
                        dbCompany = db.Companies.ToList();
                    }
                    else
                    {
                        dbCompany = db.Companies.Where(t => t.CompanyName.Contains(model.NameSearch)).ToList();
                    }
                }
                else
                {
                    if (model.NameSearch == null)
                    {
                        dbCompany = db.Companies.Where(t => t.CityId == model.PlaceSearch).ToList();
                    }
                }

                List<BasicCompanyInfoVM> basicCompanyInfoList = new List<BasicCompanyInfoVM>();
                foreach (var item in dbCompany)
                {
                    CompanyInfoVM company = CompanyInfo(item.Id);
                    Image imageMain = new Image();
                     imageMain= db.Images.FirstOrDefault(i => i.CompanyId == company.IdCompany && i.MainPicture == true);
                    if (imageMain == null)
                    {
                        imageMain = new Image() {                          
                            Link = "~/Content/images/brakZdj.jpg" };
                    }
                    BasicCompanyInfoVM basicInfo = new BasicCompanyInfoVM()
                    {
                        IdCompany = company.IdCompany,
                        CityName = company.CityName,
                        CompanyName = company.CityName,
                        CompanyMainImage = imageMain,
                        CompanyTagList = company.CompanyTagList,
                        AverageRating = 4.5,
                        Services = company.Services,
                        Speciality = company.Speciality
                    };
                    basicCompanyInfoList.Add(basicInfo);
                }
                BasicCompanyInfoListVM searched = new BasicCompanyInfoListVM();
                searched.basicCompanyInfoVMlist = basicCompanyInfoList;

                return View(searched);
            }
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
                ViewBag.Role = cust.RoleId;
                res.CustomerId = cust.ID;
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
