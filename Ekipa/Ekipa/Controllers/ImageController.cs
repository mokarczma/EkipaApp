using Ekipa.Models;
using Ekipa.Models.DB;
using Ekipa.Models.ViewModel;
using Ekipa.Models.ViewModel.Company;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;

namespace Ekipa.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        [HttpGet]
        public ActionResult CompanyImagesList()
        {
            var user = User as MPrincipal;
            var login = user.UserDetails.Login;
            ViewBag.UserName = user.UserDetails.Login;
            ViewBag.UserRole = 4;

            CompanyImagesVM model = null;
            List<ImageVM> imageList = new List<ImageVM>();


            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var company = db.Companies.FirstOrDefault(u => u.Login.Equals(login));
                var dbImages = db.Images.Where(t => t.CompanyId == company.Id).ToList();

                model = new CompanyImagesVM();
                string mainPicture = "";
                foreach (var item in dbImages)
                {
                    if (item.IsDelete == false)
                    {
                        ImageVM imageVM = new ImageVM()
                        {

                            Id = item.Id,
                            Description = item.Description,
                            Link = item.Link
                        };
                        imageList.Add(imageVM);

                        if (item.MainPicture == true)
                        {
                            model.MainPicturePath = item.Link;
                            model.MainPictureID = item.Id.ToString();

                        }
                    }
                }
                model.ImageList = imageList;
                model.MainPicturePath = mainPicture;
            }


            return View("CompanyImagesList", model);

        }
        [HttpPost]
        public ActionResult CompanyImagesList(CompanyImagesVM imagesModel)
        {

            var user = User as MPrincipal;
            var login = user.UserDetails.Login;
            ViewBag.UserName = user.UserDetails.Login;
            ViewBag.UserRole = 4;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var company = db.Companies.FirstOrDefault(u => u.Login.Equals(login));
                var dbImages = db.Images.Where(t => t.CompanyId == company.Id).ToList();
                foreach (var item in imagesModel.ImageList)
                {
                    foreach (var dbItem in dbImages)
                    {
                        if (dbItem.Id == item.Id)
                        {
                            dbItem.Link = item.Link;
                            dbItem.Description = item.Description;
                            dbItem.CompanyId = company.Id;
                            if (imagesModel.MainPictureID == dbItem.Id.ToString())
                            {
                                dbItem.MainPicture = true;
                            }
                            else
                            {
                                dbItem.MainPicture = false;
                            }
                        }
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("IndexCompany", "Account");
            }
        }

        [HttpGet]
        public ActionResult ImageAdd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ImageAdd(ImageCreateVM model)
        {
            var path = "";
            var user = User as MPrincipal;
            var login = user.UserDetails.Login;
            ViewBag.UserName = user.UserDetails.Login;
            ViewBag.UserRole = 4;

            if (model.File != null)
            {
                if (model.File.ContentLength > 0)
                {
                    if (Path.GetExtension(model.File.FileName).ToLower() == ".jpg" || Path.GetExtension(model.File.FileName).ToLower() == ".png"
                        || Path.GetExtension(model.File.FileName).ToLower() == ".gif" || Path.GetExtension(model.File.FileName).ToLower() == ".jpeg")

                    {
                        path = Path.Combine(Server.MapPath(@"\AppImages"), login.ToString() + "_" + model.File.FileName);
                        //path = @"D:\EkipaApp\Ekipa\Ekipa\AppImages\" + login.ToString() + "_" + model.File.FileName;
                        model.File.SaveAs(path);
                        ViewBag.UploadSuccess = true;
                        using (ApplicationDbContext db = new ApplicationDbContext())
                        {
                            var company = db.Companies.SingleOrDefault(x => x.Login == login);
                            var image = new Image()
                            {
                                CompanyId = company.Id,
                                Description = model.Description,
                                Link = @"~/AppImages/" + login.ToString() + "_" + model.File.FileName,
                                IsDelete = false,
                                MainPicture = false

                            };
                            db.Images.Add(image);
                            db.SaveChanges();
                        }


                    }
                }

            }
            return RedirectToAction("CompanyImagesList");
        }
        [HttpGet]
        public ActionResult DeleteImage(int id)
        {
            var userCompany = User as MPrincipal;
            var login = userCompany.UserDetails.Login;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var company = db.Companies.FirstOrDefault(x => x.Login == login);

                var image = db.Images.FirstOrDefault(t => t.Id == id && t.CompanyId == company.Id);
                db.Images.Remove(image);
                db.SaveChanges();
            }
            return RedirectToAction("CompanyImagesList");
        }
    }
}
