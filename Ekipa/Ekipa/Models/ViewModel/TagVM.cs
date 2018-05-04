using System.ComponentModel.DataAnnotations;


namespace Ekipa.Models.ViewModel
{
    public class TagVM
    {
        public int ID { get; set; }
        [StringLength(50)]
        [Display(Name = "Etykieta")]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~\s`]{2,50}$", ErrorMessage = "Niepoprawna wartość")]
        public string Name { get; set; }

    }
}