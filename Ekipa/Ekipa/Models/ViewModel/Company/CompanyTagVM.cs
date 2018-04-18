using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekipa.Models.ViewModel
{
    public class CompanyTagVM
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public string Name { get; set; }
        public bool DeleteFromCompany { get; set; }
    }
}