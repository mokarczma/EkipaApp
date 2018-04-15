using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekipa.Models.DB
{ 
    public class CompanyTerm
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public virtual ICollection<Company> Company { get; set; }
        public virtual ICollection<Customer> Customer { get; set; }
    }
}