using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Ekipa.Models.ViewModel
{
    public class SearchVM
    {
        [Display(Name = "Szukaj")]
        public string Name { get; set; }

        [Display(Name = "Miejscowość")]
        public string Place { get; set; }
    }
}