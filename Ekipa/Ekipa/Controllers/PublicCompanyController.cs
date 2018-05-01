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

                bool tagExist = false;
                if (tagList != null)
                {
                    tagExist = true;
                }

                List<CompanyTermVM> compTermList = Controllers.TermController.CompanyTermToList(company.Id).Where(c => c.Actual == true && c.CustomerID == null).ToList();

                CompanyTermVM nearestTerm = compTermList.FirstOrDefault(t => t.DateStart > DateTime.Now);

                string nearestFreeTerm = "Brak wolnych terminów";
                if (nearestTerm != null)
                {
                    nearestFreeTerm = nearestTerm.DateStart.ToShortDateString();
                }

                List<Image> imageList = new List<Image>();
                imageList = db.Images.Where(t => t.CompanyId == company.Id && t.IsDelete == false).ToList();

                Image imageMain = new Image();
                imageMain = db.Images.FirstOrDefault(i => i.CompanyId == company.Id && i.MainPicture == true);
                if (imageMain == null)
                {
                    imageMain = new Image()
                    {
                        Link = "~/Content/images/brakZdj.jpg"
                    };
                }
                var opinionList = db.Opinions.Where(o => o.CompanyId == company.Id && o.IsDelete == false && o.AdminAccept == true).ToList();
                List<OpinionVM> opinionListVM = new List<OpinionVM>();

                List<int> gradeValues = new List<int>();

                for (int i = 0; i < opinionList.Count(); i++)
                {
                    var customName = opinionList[i].Reservation.Term.Customer.Name + " " + opinionList[i].Reservation.Term.Customer.Surname;
                    OpinionVM opinionVM = new OpinionVM
                    {
                        Description = opinionList[i].Description,
                        CustomerName = customName,
                        GradeValue = opinionList[i].GradeValue
                    };
                    opinionListVM.Add(opinionVM);
                    gradeValues.Add(opinionList[i].GradeValue);
                }

                double average = 0;
                if (gradeValues.Count > 0)
                {
                   average = gradeValues.Average();
                }
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
                    CompanyTermVMList = compTermList,
                    CompanyImageList = imageList,
                    TagExist = tagExist,
                    NearestFreeDate = nearestFreeTerm,
                    CompanyMainImage = imageMain,
                    CompanyOpinionList = opinionListVM,
                    AverageRating = average
                    
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
            BasicCompanyInfoListVM searched = new BasicCompanyInfoListVM();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var dbCompany = SearchCompany(model.NameSearch, Convert.ToInt32(model.PlaceSearch));
                List<BasicCompanyInfoVM> basicCompanyInfoList = new List<BasicCompanyInfoVM>();

                foreach (var item in dbCompany)
                {
                    CompanyInfoVM company = CompanyInfo(item.Id);

                    BasicCompanyInfoVM basicInfo = new BasicCompanyInfoVM()
                    {
                        IdCompany = company.IdCompany,
                        CityName = company.CityName,
                        CompanyName = company.CompanyName,
                        CompanyMainImage = company.CompanyMainImage,
                        CompanyTagList = company.CompanyTagList,
                        AverageRating = company.AverageRating,
                        Services = company.Services,
                        Speciality = company.Speciality,
                        NearestFreeDate = company.NearestFreeDate
                    };
                    basicCompanyInfoList.Add(basicInfo);
                }
                searched.basicCompanyInfoVMlist = basicCompanyInfoList;
            }
            return View(searched);
        }

        private static IEnumerable<Company> SearchCompany(string name, int cityId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (cityId == 1)
                {
                    if (name == null)
                    {
                        var dbCompany = db.Companies.ToList();
                        return dbCompany;
                    }
                    return (from tag in db.Tags
                            join compTag in db.CompanyTags on tag.Id equals compTag.TagId
                            join comp in db.Companies on compTag.CompanyId equals comp.Id
                            where (tag.Name.Contains(name) || comp.CompanyName.Contains(name))
                            select new
                            {
                                Id = comp.Id,
                                CityId = comp.CityId,
                                CompanyName = comp.CompanyName,
                                Services = comp.Services,
                                Speciality = comp.Speciality
                            }).ToList().Select(x => new Company
                            {
                                Id = x.Id,
                                CityId = x.CityId,
                                CompanyName = x.CompanyName,
                                Services = x.Services,
                                Speciality = x.Speciality
                            });
                }
                if (name == null)
                {
                    var dbCompany = db.Companies.Where(t => t.CityId == cityId).ToList();
                    return dbCompany;
                }

                return (from tag in db.Tags
                        join compTag in db.CompanyTags on tag.Id equals compTag.TagId
                        join comp in db.Companies on compTag.CompanyId equals comp.Id
                        where (tag.Name.Contains(name) || comp.CompanyName.Contains(name)) && comp.CityId == cityId
                        select new
                        {
                            Id = comp.Id,
                            CityId = comp.CityId,
                            CompanyName = comp.CompanyName,
                            Services = comp.Services,
                            Speciality = comp.Speciality
                        }).ToList().Select(x => new Company
                        {
                            Id = x.Id,
                            CityId = x.CityId,
                            CompanyName = x.CompanyName,
                            Services = x.Services,
                            Speciality = x.Speciality
                        });
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
