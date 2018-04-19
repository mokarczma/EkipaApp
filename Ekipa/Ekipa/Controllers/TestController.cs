using Ekipa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ekipa.Controllers
{
    public class TestController : Controller
    {
        public List<SelectListItem> CitiesQuery()
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
    }
}