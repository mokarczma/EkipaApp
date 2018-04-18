using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ekipa.Models.ViewModel.Company;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;



namespace Ekipa.Models.ViewModel.Company
{
    public class CompanyTagsVM
    {
        public int Id;
        public List<CompanyTagVM> CompanyTags { get; set; }
        public List<SelectListItem> ChosenTags { get; set; }

        [Display(Name = "Wybierz nową etykietę")]
        public List<SelectListItem> OtherTags { get; set; }
    }
}