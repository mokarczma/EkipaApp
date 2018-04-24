using Ekipa.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekipa.Models.ViewModel.Company
{
    public class BasicCompanyInfoVM
    {
        public int IdCompany { get; set; }
        public string CityName { get; set; }
        public string CompanyName { get; set; }
        public string Speciality { get; set; }
        public string Services { get; set; }
        public List<Tag> CompanyTagList { get; set; }
        public Term NearestFreeDate { get; set; }
        public List<Term> CompanyTermList { get; set; }
        public Image CompanyMainImage { get; set; }
        public double AverageRating { get; set; }
    }
}