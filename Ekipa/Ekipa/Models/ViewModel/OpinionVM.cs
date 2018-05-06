using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ekipa.Models.ViewModel
{
    public class OpinionVM
    {
        public int Id { get; set; }

        [StringLength(1500)]
        [Display(Name = "Treść opini")]
        [Required(ErrorMessage = "Treść opini jest wymagana", AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~\s`]{2,1500}$", ErrorMessage = "Zbyt mało znaków")]
        public string Description { get; set; }

        public int GradeValue { get; set; }
        public bool AdminAccept { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int ReservationId { get; set; }
        public string CustomerName { get; set; }
    }
}