using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;
using Ekipa.Models.DB;

namespace Ekipa.Models.ViewModel.Company
{
    public class CompanyDetailsVM
    {
        [StringLength(200)]
        [Display(Name = "Specjaność firmy")]
        [Required(ErrorMessage = "Specjalność jest wymagana", AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~\s`]{2,100}$", ErrorMessage = "Zbyt mało znaków")]
        public string Speciality { get; set; }

        [StringLength(1000)]
        [Display(Name = "Zakres usług")]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~\s`]{2,1000}$", ErrorMessage = "Zbyt mało znaków")]
        public string Services { get; set; }

        [StringLength(1000)]
        [Display(Name = "Wyceny")]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~\s`]{2,1000}$", ErrorMessage = "Zbyt mało znaków")]
        public string Pricing { get; set; }

        public int CityId { get; set; }

        [Display(Name = "Miasto")]

        public string CityName { get; set; }

        [Display(Name = "Lokalizacja")]
        public List<SelectListItem> Cities { get; set; }
        public int SelectedCityID { get; set; }

        public List<Tag> CompanyTagList { get; set; }
        public bool TagExist { get; set; }
    }
}