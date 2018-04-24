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
                        ViewBag.UserRole = 3;
                    }
                    else
                    {
                        ViewBag.UserRole = 4;
                    }
                }
            }
            return View();
        }
    }
}