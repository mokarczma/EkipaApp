using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekipa.Models.DB
{
    public class Role
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsDelete { get; set; }
        public virtual ICollection<Company> Company { get; set; }
        public virtual ICollection<Customer> Customer { get; set; }

    }
}