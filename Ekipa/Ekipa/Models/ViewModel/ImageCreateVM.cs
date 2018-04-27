using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Ekipa.Models.ViewModel
{
    public class ImageCreateVM
    {
        public int Id { get; set; }
        [StringLength(150)]
        [Display(Name = "Opis")]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~\s`]{2,50}$", ErrorMessage = "Zbyt mało znaków")]
        public string Description { get; set; }

        [Display(Name = "Wgraj zdjęcie")]
        public HttpPostedFileBase File { get; set; }


    }
}