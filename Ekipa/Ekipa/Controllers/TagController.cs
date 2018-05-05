using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ekipa.Models.DB;
using Ekipa.Models.ViewModel;
using Ekipa.Models.ViewModel.Company;
using Ekipa.Models;

namespace Ekipa.Controllers
{
    public class TagController : Controller
    {
        // GET: Tag
        [HttpGet]
        [Authorize]
        public ActionResult CompanyTagsList()
        {
            var user = User as MPrincipal;
            var login = user.UserDetails.Login;
            ViewBag.UserName = user.UserDetails.Login;
            ViewBag.UserRole = 4;


            CompanyTagsVM model = null;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var company = db.Companies.FirstOrDefault(u => u.Login.Equals(login));
                model = new CompanyTagsVM();

                var dbCompanyTags = db.Tags.Where(t => t.CompanyTag.Any(c => c.CompanyId == company.Id)).ToList();
                ViewBag.NoTags = false;

                if (dbCompanyTags == null)
                {
                    ViewBag.NoTags = true;
                }
                List<CompanyTagVM> ctVMList = new List<CompanyTagVM>();
                for (int i = 0; i < dbCompanyTags.Count(); i++)
                {
                    CompanyTagVM ctVM = new CompanyTagVM()
                    {
                        Id = dbCompanyTags[i].Id,
                        Name = dbCompanyTags[i].Name,
                        DeleteFromCompany = dbCompanyTags[i].IsDelete
                    };
                    ctVMList.Add(ctVM);
                }
                model.CompanyTags = ctVMList;

                List<SelectListItem> allTags = new List<SelectListItem>();
                allTags = (from t in db.Tags orderby t.Name
                           select new SelectListItem 
                           {
                               Text = t.Name,
                               Value = t.Id.ToString()
                           }).ToList();

                List<SelectListItem> otherTagsList = new List<SelectListItem>();

                foreach (var item in allTags)
                {
                    otherTagsList.Add(item);
                    if (model.CompanyTags != null)
                    {
                        foreach (var i in model.CompanyTags)
                        {
                            if (item.Value == i.Id.ToString())
                            {
                                otherTagsList.Remove(item);
                            }
                        }
                    }
                }

                model.OtherTags = otherTagsList;
            }
            return View("CompanyTagsList", model);
        }

        [HttpGet]
        [ActionName("CreateTag")]
        public ActionResult CreateTag()
        {
            var user = User as MPrincipal;
            var login = user.UserDetails.Login;
            ViewBag.UserName = user.UserDetails.Login;
            ViewBag.UserRole = 4;
            return View();
        }

        //POST: Tag/Create
        [HttpPost]
        [ActionName("CreateTag")]
        public ActionResult CreateTag(TagVM model)

        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var nameTag = db.Tags.FirstOrDefault(x => x.Name == model.Name && x.IsDelete == false);
                    if (nameTag == null)
                    {
                        Models.DB.Tag tag = new Models.DB.Tag();
                        tag.Name = model.Name;
                        tag.IsDelete = false;
                        db.Tags.Add(tag);
                        db.SaveChanges();
                        return RedirectToAction("CompanyTagsList");
                    }
                    else
                    {
                        ModelState.AddModelError("Tag", "Etykieta o takiej nazwie już istnieje");
                    }
                }
                return RedirectToAction("CompanyTagsList");
            }
            return View(model);
        }
        //GET: Tag/Delete/5
        public ActionResult TagDelete(int id)
        {
            return View();
        }

        //POST: Tag/Delete/5
        [HttpPost]
        [ActionName("TagDelete")]
        public ActionResult TagDelete(TagVM model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Models.DB.Tag tag = db.Tags.FirstOrDefault(t => t.Name == model.Name);
                tag.IsDelete = true;
                db.SaveChanges();
                return RedirectToAction("TagsList");
            }
        }
        [HttpGet]
        [ActionName("AddCompanyTag")]
        public ActionResult AddCompanyTag()
        {
            return View();
        }


        [HttpPost]
        [ActionName("AddCompanyTag")]

        public ActionResult AddCompanyTag(CompanyTagsVM model)
        {
            var user = User as MPrincipal;
            var login = user.UserDetails.Login;
            ViewBag.UserName = login;
            ViewBag.UserRole = 4;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var company = db.Companies.SingleOrDefault(x => x.Login == login);
                CompanyTag cTag = new CompanyTag()
                {
                    CompanyId = company.Id,
                    TagId = Convert.ToInt32(model.SelectedTagId)
                };
                db.CompanyTags.Add(cTag);
                db.SaveChanges();

            }
            return RedirectToAction("CompanyTagsList");
        }

        [HttpPost]
        [ActionName("DeleteCompanyTag")]

        public ActionResult DeleteCompanyTag(CompanyTagVM model)
        {
            var userCompany = User as MPrincipal;
            var login = userCompany.UserDetails.Login;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var company = db.Companies.FirstOrDefault(x => x.Login == login);

                var tag = db.CompanyTags.FirstOrDefault(t => t.TagId == model.Id && t.CompanyId == company.Id);
                db.CompanyTags.Remove(tag);
                db.SaveChanges();
            }
            return RedirectToAction("CompanyTagsList");
        }
    }
}
