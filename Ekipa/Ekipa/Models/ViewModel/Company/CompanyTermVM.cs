using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ekipa.Models.ViewModel.Company

{
    public class CompanyTermVM
    {
        public int ID { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateStart { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime DateStop {get; set; }
        public string Start { get; set; }
        public string Stop { get; set; }
        public int ?CustomerID { get; set; }
        public bool Actual { get; set; }
        public bool Accepted { get; set; }
        public int ReservationId { get; set; }
    }
}