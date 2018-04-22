using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Ekipa.Models.DB;

namespace Ekipa.Models.ViewModel
{
    public class MainViewVM
    {
       [Display(Name = "Szukaj")]
       public string NameSearch { get; set; } 

       [Display(Name = "Miejscowość")]
       public int PlaceSearch { get; set; }

        public string UserName;
        public int UserRole;

        public List<SelectListItem> CitiesDB { get; set; }
        public MainViewVM()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            //using (Controllers.AccountController accCon = new Controllers.AccountController())
            //{
            //    if (accCon.User.Identity.Name != null)
            //    {
            //        UserName = accCon.User.Identity.Name;
            //    }
            //}

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                items = (from li in db.Cities
                         select new SelectListItem
                         {
                             Text = li.Name,
                             Value = li.ID.ToString()
                         }).ToList();
                CitiesDB = items;
                //var user = db.Companies.FirstOrDefault(u => u.Login.Equals(UserName));
                //UserRole = user.RoleId;}
            }

        }
    }
   
}