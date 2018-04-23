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
    public class CompanyForCustomerController : Controller
    {
        public static CompanyInfoVM CompanyInfo(int companyId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var company = db.Companies.SingleOrDefault(x => x.Id == companyId);

                List<Tag> tagList = new List<Tag>();
                tagList = db.Tags.Where(t => t.CompanyTag.Any(c => c.CompanyId == company.Id)).ToList();

                List<CompanyTerm> termList = new List<CompanyTerm>();
                termList = db.CompanyTerm.Where(t => t.CompanyId == company.Id).ToList();
                CompanyTermsVM companyTerms = new CompanyTermsVM();

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
                    companyTerms.CompanyTermsList.Add(TermVM);
                }


                List<Image> imageList = new List<Image>();
                imageList = db.Images.Where(t => t.CompanyId == company.Id).ToList();
                CompanyInfoVM companyInfoVM = new CompanyInfoVM()
                {
                    Id = company.Id,
                    CityName = company.City.Name,
                    CompanyName = company.CompanyName,
                    Speciality = company.Speciality,
                    Services = company.Services,
                    Pricing = company.Pricing,
                    PhoneNumer = company.PhoneNumer,
                    CompanyTagList = tagList,
                    CompanyTerms = companyTerms,
                    CompanyImageList = imageList
                };
                return companyInfoVM;
            };
        }
        [HttpGet]
        public ActionResult InfoAboutCompany(int companyId)
        {
            var user = User as MPrincipal;
            var login = user.UserDetails.Login;
            ViewBag.UserName = user.UserDetails.Login;
            CompanyInfoVM companyInfoVM = Controllers.CompanyForCustomerController.CompanyInfo(companyId);
            return View(companyInfoVM);

        }
        [HttpGet]
        public ActionResult MainView()
        {
            MainViewVM model = new MainViewVM();
            string szukaj = "";
            string msc = "";

            return View(szukaj, msc);
        }

        [HttpPost]
        public ActionResult MainView(MainViewVM model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var dbCompany = db.Companies.Where(t => t.CityId == model.PlaceSearch && t.CompanyName.Contains(model.NameSearch)).ToList<Company>();
                foreach (var item in dbCompany)
                {
                    CompanyInfoVM company = CompanyInfo(item.Id);
                    BasicCompanyInfoVM basicInfo = new BasicCompanyInfoVM()
                    {
                        IdCompany = company.Id,
                        CityName = company.CityName,
                        CompanyName = company.CityName,
                        CompanyMainImage = company.CompanyImageList.FirstOrDefault(c => c.MainPicture == true),
                        CompanyTagList = company.CompanyTagList,
                        AverageRating = 4.5,
                        Services = company.Services,
                        Speciality = company.Speciality
                    };

                }


                return RedirectToAction("InfoAboutCompany", "CompanyForCustomerController");
            }
        }
    }
}
