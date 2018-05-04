using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekipa.Models.ViewModel.Company
{
    public class BasicCompanyInfoListVM
    {
        public List<BasicCompanyInfoVM> basicCompanyInfoVMlist { get; set; }
        public int ListCount { get; set; }
    }
}