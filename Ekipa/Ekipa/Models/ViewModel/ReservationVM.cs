using Ekipa.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ekipa.Models.ViewModel
{
    public class ReservationVM
    {
        public int Id { get; set; }

        [StringLength(1000)]
        [Display(Name = "Wiadomość dla firmy dotycząca tworzonej reazerwacji")]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~\s`]{2,1000}$")]
        public string DescriptionCustomer { get; set; }

        [StringLength(1000)]
        [Display(Name = "Wiadomość dla kienta dotycząca tworzonej reazerwacji")]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~\s`]{2,1000}$")]
        public string DescriptionCompany { get; set; }

        [Display(Name = "Czy potwierdzasz rezerwację?")]
        public bool CompanyAccept { get; set; }

        public int TermId { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public bool OpinionAdded { get; set; }
        public bool IsDelete { get; set; }


        public DateTime DateStart{ get; set; }
        public DateTime DateStop { get; set; }
    }
}