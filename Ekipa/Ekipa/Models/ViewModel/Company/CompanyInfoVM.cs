using Ekipa.Models.DB;
using Ekipa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Ekipa.Models.ViewModel.Company
{
    public class CompanyInfoVM : BasicCompanyInfoVM
    {
        public string Pricing { get; set; }
        public string PhoneNumer { get; set; }
        public List<Image> CompanyImageList { get; set; }
        public List<CompanyTermVM> CompanyTermVMList { get; set; }
        public List<Reservation> CompanyReservationList { get; set; }
        public List<OpinionVM> CompanyOpinionList { get; set; }
        public bool TagExist { get; set; }

    }
}