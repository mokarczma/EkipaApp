using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekipa.Models.DB
{
    public class Reservation
    {
        public int Id { get; set; }
        public string DescriptionCustomer { get; set; }
        public string DescriptionCompany { get; set; }
        public bool CompanyAccept { get; set; }
        public bool IsDelete { get; set; }

        public virtual ICollection<Opinion> Opinions { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }




    }
}