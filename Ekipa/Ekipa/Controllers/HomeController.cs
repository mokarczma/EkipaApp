using Ekipa.Models;
using Ekipa.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            var cos = new MainViewVM();
            var m = cos.Miasta;
            ViewBag.CityList = m;
            var user = User as MPrincipal;
            if (user != null)
            {
                var login = user.UserDetails.Login;
                ViewBag.UserName = user.UserDetails.Login;
            }
            return View();
        }
        [HttpGet]
        public ActionResult MainView()
        {
            MainViewVM model = new MainViewVM();
            string szukaj = "";
            string msc = "";
         
            return View(szukaj,msc);
        }

        [HttpPost]
        public ActionResult MainView(MainViewVM model)

        {
            return View();
        }

    }
}