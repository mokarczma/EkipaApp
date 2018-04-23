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

        public bool Loged { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int UserRole { get; set; }
        public List<SelectListItem> CitiesDB { get; set; }


        public MainViewVM()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            LogedUserVM logedUser = new LogedUserVM();


            //using (Controllers.AccountController accCon = new Controllers.AccountController())
            //{
            //    logedUser = accCon.LogedUser();
            //}
            //if (logedUser != null)
            //{
            //    Loged = logedUser.Loged;
            //    UserId = logedUser.UserId;
            //    UserName = logedUser.UserName;
            //    UserRole = logedUser.UserRole;
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
            }

        }
    }
   
}