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
        public ActionResult TagsList()
        {
            var userCompany = User as MPrincipal;
            var login = userCompany.UserDetails.Login;
            CompanyTagVM model = null;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var company = db.Companies.FirstOrDefault(u => u.Login.Equals(login));
                model = new CompanyTagVM();
                //List<Tag> compTag = db.Tags.Where(t => t.CompanyTag.Any(c => c.CompanyId == company.Id)).ToList();
                //List<SelectListItem> sli = new List<SelectListItem>();

                //foreach (var item in compTag)
                //{
                //    SelectListItem s = new SelectListItem()
                //    {
                //        Text = item.Name,
                //        Value = item.Id.ToString()
                //    };
                //sli.Add(s);
                //}
                model.CompanyTags = db.Tags.Where(t => t.CompanyTag.Any(c => c.CompanyId == company.Id)).ToList();

                List<SelectListItem> allTags = new List<SelectListItem>();
                allTags = (from t in db.Tags
                           select new SelectListItem
                           {
                               Text = t.Name,
                               Value = t.Id.ToString()
                           }).ToList();

                List<SelectListItem> testtt = new List<SelectListItem>();
                foreach (var item in allTags)
                {
                    testtt.Add(item);
                    foreach (var i in model.CompanyTags)
                    {
                        if (item.Value == i.Id.ToString())
                        {
                            testtt.Remove(item);
                        }
                    }
                }

                model.OtherTags = testtt;

            }
            return View("TagsList", model);
        }

        [HttpGet]
        [ActionName("CreateTag")]
        public ActionResult CreateTag()
        {
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
                        Tag tag = new Tag();
                        tag.Name = model.Name;
                        tag.IsDelete = false;
                        db.Tags.Add(tag);
                        db.SaveChanges();
                        return RedirectToAction("TagsList");
                    }
                    else
                    {
                        ModelState.AddModelError("Tag", "Etykieta o takiej nazwie już istnieje");
                    }
                }
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
                Tag tag = db.Tags.FirstOrDefault(t => t.Name == model.Name);
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

        public ActionResult AddCompanyTag(Tag tag)
        {
            var userCompany = User as MPrincipal;
            var login = userCompany.UserDetails.Login;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var company = db.Companies.SingleOrDefault(x => x.Login == login);
                CompanyTag cTag = new CompanyTag()
                {
                    CompanyId = company.Id,
                    TagId = tag.Id

                };
                db.CompanyTags.Add(cTag);
            }
            return RedirectToAction("TagsList");
        }
        [HttpGet]
        [ActionName("DeleteCompanyTag")]
        public ActionResult DeleteCompanyTag()
        {
            return View();
        }


        [HttpDelete]
        [ActionName("DeleteCompanyTag")]

        public ActionResult DeleteCompanyTag(Tag tag)
        {
            var userCompany = User as MPrincipal;
            var login = userCompany.UserDetails.Login;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var company = db.Companies.SingleOrDefault(x => x.Login == login);
                CompanyTag cTag = new CompanyTag()
                {
                    CompanyId = company.Id,
                    TagId = tag.Id

                };
                db.CompanyTags.Remove(cTag);
            }
            return RedirectToAction("TagsList");
        }
    }
}
