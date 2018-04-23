using Ekipa.Models.DB;
using Ekipa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Ekipa.Models.ViewModel.Company
{
    public class CompanyInfoVM
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string CompanyName { get; set; }
        public string Speciality { get; set; }
        public string Services { get; set; }
        public string Pricing { get; set; }
        public string PhoneNumer { get; set; }
        public List<Tag> CompanyTagList { get; set; }
        public CompanyTermsVM CompanyTerms { get; set; }
        public List<Image> CompanyImageList { get; set; }
        // Tutaj dodamy jeszcze rezerwacje i opinie
        
    }
}