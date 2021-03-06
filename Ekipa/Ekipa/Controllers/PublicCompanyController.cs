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
                    AverageRating = average,
                    OpinionsCount = opinionListVM.Count()
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
                    var cust = db.Customers.FirstOrDefault(u => u.Login.Equals(login));
                    if (cust != null)
                    {
                        ViewBag.UserRole = cust.RoleId;
                    }
                    else if(comp != null)
                    {
                        ViewBag.UserRole = comp.RoleId;
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
            
            var user = User as MPrincipal;

            if (user != null)
            {

                var login = user.UserDetails.Login;
                ViewBag.UserName = user.UserDetails.Login;

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var compUser = db.Companies.FirstOrDefault(u => u.Login.Equals(login));
                    var custUser = db.Customers.FirstOrDefault(u => u.Login.Equals(login));
                    if (compUser != null)
                    {
                        ViewBag.UserRole = compUser.RoleId;
                    }
                    else if (custUser != null)
                    {
                        ViewBag.UserRole = custUser.RoleId;

                    }
                }
            }


            var dbCompany = SearchCompany(model.NameSearch, Convert.ToInt32(model.PlaceSearch)).Select(c => c.Id).Distinct();

                List<BasicCompanyInfoVM> basicCompanyInfoList = new List<BasicCompanyInfoVM>();

                foreach (var item in dbCompany)
                {
                    CompanyInfoVM company = CompanyInfo(item);

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
                        NearestFreeDate = company.NearestFreeDate,
                        OpinionsCount = company.OpinionsCount
                    };
                    basicCompanyInfoList.Add(basicInfo);
                }
                searched.basicCompanyInfoVMlist = basicCompanyInfoList;
                 searched.ListCount = basicCompanyInfoList.Count();
         
            return View(searched);
        }

        private static IEnumerable<Company> SearchCompany(string name, int cityId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (cityId == 1 || cityId == 0)
                {
                    if (name == null)
                    {
                        var dbCompany = db.Companies.ToList();
                        return dbCompany;
                    }
                    var tags = db.Tags.Where(t => t.Name.Contains(name));

                    return (from tag in db.Tags
                                  join compTag in db.CompanyTags on tag.Id equals compTag.TagId
                                  join comp in db.Companies on compTag.CompanyId equals comp.Id
                                  where (tag.Name.Contains(name))
                                  select new
                                  {
                                      Id = comp.Id
                                  }).ToList().Select(x => new Company
                                  {
                                      Id = x.Id
                                  }).Concat((from com in db.Companies
                                             where com.CompanyName.Contains(name)
                                             select new { IdCompany = com.Id }).ToList().Select(k => new Company
                                             {
                                                 Id = k.IdCompany
                                             })).ToList();

                }
                if (name == null)
                {
                    var dbCompany = db.Companies.Where(t => t.CityId == cityId).ToList();
                    return dbCompany;
                }

               return (from tag in db.Tags
                                 join compTag in db.CompanyTags on tag.Id equals compTag.TagId
                                 join comp in db.Companies on compTag.CompanyId equals comp.Id
                                 where (tag.Name.Contains(name) && comp.CityId == cityId)
                                 select new
                                 {
                                     Id = comp.Id
                                 }).ToList().Select(x => new Company
                                 {
                                     Id = x.Id
                                 }).Concat((from com in db.Companies
                                            where com.CompanyName.Contains(name) && com.CityId == cityId
                                            select new { IdCompany = com.Id }).ToList().Select(k => new Company
                                            {
                                                Id = k.IdCompany
                                            })).ToList();
            }
        }
        
      
        
    }
}
