using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ekipa.Models;
using Ekipa.Models.ViewModel.Company;
using Ekipa.Models.DB;


namespace Ekipa.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        // GET: Company
        public ActionResult Index()
        {
            ViewBag.CityList = CitiesQuery();
            return View();
        }


        [HttpGet]
        [ActionName("CompanyDetails")]
        public ActionResult CompanyDetails()
        {
            var user = User as MPrincipal;
            var login = user.UserDetails.Login;
            ViewBag.UserName = user.UserDetails.Login;
            ViewBag.UserRole = 4;

            CompanyDetailsVM compDetVM = null;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var company = db.Companies.SingleOrDefault(x => x.Login == login);
                CompanyInfoVM companyInfoVM = Controllers.PublicCompanyController.CompanyInfo(company.Id);
                bool tag = false;
                if (companyInfoVM.CompanyTagList != null)
                {
                    tag = true;
                }

                var city = db.Cities.SingleOrDefault(x => x.ID == company.CityId);
                compDetVM = new CompanyDetailsVM();
                compDetVM.Pricing = company.Pricing;
                compDetVM.Services = company.Services;
                compDetVM.Speciality = company.Speciality;
                compDetVM.SelectedCityID = company.City.ID;
                compDetVM.CityName = city.Name;
                compDetVM.CompanyTagList = companyInfoVM.CompanyTagList;
                compDetVM.TagExist = tag;
            }
            return View(compDetVM);
        }

        [HttpGet]
        [ActionName("EditCompanyDetails")]
        public ActionResult EditCompanyDetails()
        {
            ViewBag.CityList = CitiesQuery();
            var user = User as MPrincipal;
            var login = user.UserDetails.Login;

            ViewBag.UserName = user.UserDetails.Login;
            ViewBag.UserRole = 4;

            CompanyDetailsVM compDetVM = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var comp = db.Companies.FirstOrDefault(u => u.Login.Equals(login));
                ViewBag.UserName = comp.Login;
                ViewBag.Role = comp.RoleId;
                compDetVM = new CompanyDetailsVM();
                compDetVM.Pricing = comp.Pricing;
                compDetVM.Services = comp.Services;
                compDetVM.Speciality = comp.Speciality;
                compDetVM.CityName = comp.City.Name;
                var citesQuery = CitiesQuery();

                List<SelectListItem> sortedCities = new List<SelectListItem>();
                SelectListItem firstCity = new SelectListItem()
                {
                    Text = comp.City.Name,
                    Value = comp.City.ID.ToString()
                };
                sortedCities.Add(firstCity);
                foreach (var item in citesQuery)
                {
                    if (item.Text != compDetVM.CityName)
                    {
                        sortedCities.Add(item);
                    }
                }
                compDetVM.Cities = sortedCities;
            }

            return View("EditCompanyDetails", compDetVM);
        }


        private static List<SelectListItem> CitiesQuery()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                items = (from li in db.Cities
                         select new SelectListItem
                         {
                             Text = li.Name,
                             Value = li.ID.ToString()
                         }).ToList();
            }
            return items;
        }

        [HttpPost]
        [ActionName("EditCompanyDetails")]
        public ActionResult EditCompanyDetails(CompanyDetailsVM model)
        {
            ViewBag.CityList = CitiesQuery();
            var user = User as MPrincipal;
            var login = user.UserDetails.Login;
            ViewBag.UserName = user.UserDetails.Login;
            ViewBag.UserRole = 4;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var comp = db.Companies.FirstOrDefault(u => u.Login.Equals(login));
                ViewBag.UserName = comp.Login;
                if (comp != null)
                {
                    comp.Pricing = model.Pricing ?? "";
                    comp.Services = model.Services ?? "";
                    comp.Speciality = model.Speciality ?? "";
                    if (model.SelectedCityID != null)
                    {
                        comp.CityId = model.SelectedCityID;
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("EditCompanyDetails");
        }
      
    }
}
