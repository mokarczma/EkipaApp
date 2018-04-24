﻿using Ekipa.Models.DB;
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
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~`]{2,1000}$")]
        public string DescriptionCustomer { get; set; }

        [StringLength(1000)]
        [Display(Name = "Wiadomość dla firmy dotycząca tworzonej reazerwacji")]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~`]{2,1000}$")]
        public string DescriptionCompany { get; set; }

        public bool CompanyAccept { get; set; }
        public Term Term { get; set; }

    }
}