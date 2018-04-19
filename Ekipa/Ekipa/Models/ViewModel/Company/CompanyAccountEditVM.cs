﻿using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Ekipa.Models.ViewModel.Company
{
    public class CompanyAccountEditVM
    {
        public int ID { get; set; }

        [StringLength(200)]
        [Display(Name = "Nazwa firmy")]
        [Required(ErrorMessage = "Nazwa firmy jest wymagana", AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~`]{2,200}$", ErrorMessage = "Niepoprawna nazwa firmy")]
        public string CompanyName { get; set; }

        [StringLength(200)]
        [Display(Name = "Login")]
        [Required(ErrorMessage = "Login jest wymagany", AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~`]{2,200}$", ErrorMessage = "Niepoprawny login")]
        public string Login { get; set; }

        [StringLength(100)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email jest wymagany", AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Za-z0-9_\-\.]{1,}@[a-z0-9_\.]{1,}\.[a-z]{2,5}$", ErrorMessage = "Niepoprawny email")]
        public string Email { get; set; }

        [StringLength(50)]
        [Display(Name = "Numer telefonu")]
        [Required(ErrorMessage = "Numer telefonu jest wymagany", AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~`]{6,50}$", ErrorMessage = "Niepoprawny numer telefonu")]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        [Display(Name = "Stare hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(50)]
        [Display(Name = "Nowe hasło")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^[A-Z-a-z0-9_\@]{8,16}$", ErrorMessage = "Nowe hasło niepoprawne, co najmniej 8 znaków")]
        public string NewPassword { get; set; }

        //[Compare("password")]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdż hasło")]
        public string ConfirmPassword { get; set; }

        public CompanyAccountEditVM() : base()
        {

        }

    }

}