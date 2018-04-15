using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ekipa.Models.DB;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;



namespace Ekipa.Models.ViewModel.Company
{
    public class CompanyTagVM
    {
        public int Id;
        public List<Tag> CompanyTags { get; set; }
        public List<SelectListItem> ChosenTags { get; set; }

        [Display(Name = "Wybierz nową etykietę")]
        public List<SelectListItem> OtherTags { get; set; }
    }
}