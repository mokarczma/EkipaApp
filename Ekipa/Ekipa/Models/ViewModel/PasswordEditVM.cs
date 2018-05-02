using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ekipa.Models.ViewModel
{
    public class PasswordEditVM
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Display(Name = "Stare hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(50)]
        [Display(Name = "Nowe hasło")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^[A-Z-a-z0-9_\@]{8,16}$", ErrorMessage = "Nowe hasło niepoprawne, co najmniej 8 znaków")]
        public string NewPassword { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdż hasło")]
        public string ConfirmPassword { get; set; }
    }
}