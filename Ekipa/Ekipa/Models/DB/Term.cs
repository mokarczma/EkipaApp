using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekipa.Models.DB
{ 
    public class Term
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public virtual Company Company { get; set; }
        public virtual Customer Customer { get; set; }

    }
}