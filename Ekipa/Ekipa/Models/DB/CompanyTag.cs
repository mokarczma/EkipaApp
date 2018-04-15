using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekipa.Models.DB
{
    public class CompanyTag
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int TagId { get; set; }
        public virtual Company Company { get; set; }
        public virtual Tag Tag { get; set; }
    }
}