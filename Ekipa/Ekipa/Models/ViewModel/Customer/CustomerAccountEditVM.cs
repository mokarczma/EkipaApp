using System.ComponentModel.DataAnnotations;


namespace Ekipa.Models.ViewModel.Customer
{
    public class CustomerAccountEditVM
    {
        public int ID { get; set; }

        [StringLength(50)]
        [Display(Name = "Imie")]
        [Required(ErrorMessage = "Imie jest wymagane", AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~\s`]{2,50}$", ErrorMessage = "Niepoprawne imię")]
        public string Name { get; set; }

        [StringLength(100)]
        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "Nazwisko jest wymagane", AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~\s`]{2,100}$", ErrorMessage = "Niepoprawne nazwisko")]
        public string Surname { get; set; }

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
        
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdż hasło")]
        public string ConfirmPassword { get; set; }
    }
}