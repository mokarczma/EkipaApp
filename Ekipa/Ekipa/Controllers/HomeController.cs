using Ekipa.Models;
using Ekipa.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Sql;
using Ekipa.Models.DB;
using Ekipa.Models.ViewModel.Company;

namespace Ekipa.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()

        {
            var user = User as MPrincipal;
            ViewBag.Home = 1;
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
                    else
                    {
                        ViewBag.UserRole = comp.RoleId;
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult OpinionAcceptList()
        {
            var user = User as MPrincipal;
            var login = user.UserDetails.Login;
            ViewBag.UserName = user.UserDetails.Login;
            ViewBag.UserRole = 5;
            List<OpinionVM> opinionList = new List<OpinionVM>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
               var opinions = db.Opinions.Where(o => o.AdminAccept == false && o.IsDelete == false).ToList();
                foreach (var item in opinions)
                {
                    OpinionVM opinionVM = new OpinionVM()
                    {
                        Description = item.Description,
                        Id = item.Id,
                    };
                    opinionList.Add(opinionVM);
                }
            }
            return View(opinionList);
        }
        [HttpGet]
        public ActionResult OpinionAdminAccept(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var opinion = db.Opinions.FirstOrDefault(o => o.Id == id);
                opinion.AdminAccept = true;
                db.SaveChanges();
            }
                return RedirectToAction("OpinionAcceptList");
        }
        [HttpGet]
        public ActionResult OpinionDelete(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var opinion = db.Opinions.FirstOrDefault(o => o.Id == id);
                opinion.IsDelete = true;
                db.SaveChanges();
            }
            return RedirectToAction("OpinionAcceptList");
        }
    }
}