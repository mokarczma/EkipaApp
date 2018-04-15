using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Ekipa.Models.ViewModel
{
    public class ImageVM
    {
        public int Id { get; set; }
        [StringLength(150)]
        [Display(Description = "Opis")]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~`]{2,150}$]")]
        public string Description { get; set; }
       
    }
}