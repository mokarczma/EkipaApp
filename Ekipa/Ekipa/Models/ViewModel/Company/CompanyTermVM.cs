using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekipa.Models.ViewModel.Company

{
    public class CompanyTermVM
    {
        public int ID { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateStop {get; set; }
        public int ?CustomerID { get; set; }
        public bool Actual { get; set; }
        public bool Accepted { get; set; }
        public int ReservationId { get; set; }
    }
}