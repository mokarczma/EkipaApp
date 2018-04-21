using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ekipa.Models.ViewModel
{
    public class MessageVm
    {
        public int Id { get; set; }
        [StringLength(1000)]
        [Display(Description = "Treść")]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~`]{2,50}$", ErrorMessage = "Zbyt mało znaków")]
        public string Description { get; set; }
        public string EmailToSend { get; set; }
    }
}