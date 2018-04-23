using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekipa.Models.ViewModel.Company

{
    public class CompanyAddTermVM
    {
        public int ID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int CustomerID { get; set; }
        public int YearFrom  { get; set; }
        public string MonthFrom { get; set; }
        public int DayFrom { get; set; }
        public int YearTo { get; set; }
        public string MonthTo { get; set; }
        public int DayTo { get; set; }

    }
}