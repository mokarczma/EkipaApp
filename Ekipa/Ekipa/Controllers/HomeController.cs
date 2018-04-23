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
        public ActionResult Index()

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
            return View();
        }     
    }
}