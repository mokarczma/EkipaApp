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
            return View();
        }
        

    }
}