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

                List<CompanyTerm> termList = new List<CompanyTerm>();
                termList = db.CompanyTerm.Where(t => t.CompanyId == company.Id).ToList();
                CompanyTermsVM companyTerms = new CompanyTermsVM();
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
                companyTerms.CompanyTermsList = compTermsList;

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
        //[HttpPost]
        //public ActionResult InfoAboutCompany(BasicCompanyInfoVM model)
        //{
        //    var user = User as MPrincipal;
        //    if (user != null)
        //    {
        //        var login = user.UserDetails.Login;
        //        ViewBag.UserName = user.UserDetails.Login;


        //        using (ApplicationDbContext db = new ApplicationDbContext())
        //        {
        //            var comp = db.Companies.FirstOrDefault(u => u.Login.Equals(login));
        //            var cust = db.Companies.FirstOrDefault(u => u.Login.Equals(login));
        //            if (comp != null)
        //            {
        //                ViewBag.UserRole = 6;
        //            }
        //            else
        //            {
        //                ViewBag.UserRole = 5;
        //            }
        //        }
        //    }
        //    CompanyInfoVM companyInfoVM = CompanyInfo(model.IdCompany);
        //    return View(companyInfoVM);
        //}
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

                List<BasicCompanyInfoVM> basicCompanyInfoList = new List<BasicCompanyInfoVM>();
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
                    basicCompanyInfoList.Add(basicInfo);
                }
                BasicCompanyInfoListVM searched = new BasicCompanyInfoListVM();
                searched.basicCompanyInfoVMlist = basicCompanyInfoList;

                return View(searched);
            }
        }
    }
}
