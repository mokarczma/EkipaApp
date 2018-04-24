using Ekipa.Models;
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
                termList = db.CompanyTerm.Where(t => t.CompanyId == company.Id).ToList();
                List<CompanyAddTermVM> compTermsList = new List<CompanyAddTermVM>();

                foreach (var item in termList)
                {
                    CompanyAddTermVM TermVM = new CompanyAddTermVM
                    {
                        ID = item.Id,

                        DateFrom = item.DateFrom,
                        YearFrom = item.DateFrom.Year,
                        MonthFrom = item.DateFrom.Month.ToString(),
                        DayFrom = item.DateFrom.Day,

                        DateTo = item.DateTo,
                        YearTo = item.DateTo.Year,
                        MonthTo = item.DateTo.Month.ToString(),
                        DayTo = item.DateTo.Day,
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
                    if (comp != null)
                    {
                        ViewBag.UserRole = 6;
                    }
                    else
                    {
                        ViewBag.UserRole = 5;
                    }
                }
            }
            CompanyInfoVM companyInfoVM = CompanyInfo(id);
            return View(companyInfoVM);
        }
        [HttpGet]
        public ActionResult SearchView()
        {          return View();
        }

        [HttpPost]
        public ActionResult SearchView(MainViewVM model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var dbCompany = db.Companies.Where(t => t.CityId == model.PlaceSearch && t.CompanyName.Contains(model.NameSearch)).ToList<Company>();

                if (model.PlaceSearch ==2)
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
                    BasicCompanyInfoVM basicInfo = new BasicCompanyInfoVM()
                    {
                        IdCompany = company.IdCompany,
                        CityName = company.CityName,
                        CompanyName = company.CityName,
                        CompanyMainImage = company.CompanyImageList.FirstOrDefault(c => c.MainPicture == true),
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
                if (cust.RoleId == 6)
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
                    var termDB = db.CompanyTerm.FirstOrDefault(t => t.Id.Equals(model.TermId));
                    termDB.CustomerId = model.CustomerId;
                    Reservation newReservation = new Reservation()
                    {
                        CompanyId = termDB.CompanyId,
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
