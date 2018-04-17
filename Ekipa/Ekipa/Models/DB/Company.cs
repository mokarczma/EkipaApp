using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekipa.Models.DB
{
    public class Company
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Speciality { get; set; }
        public string Services { get; set; }
        public string Pricing { get; set; }
        public int CityId { get; set; }
        public int RoleId { get; set; }
        public bool IsDelete { get; set; }
        public string PhoneNumer { get; set; }

        public virtual ICollection<CompanyTag> Tags { get; set; }
        public virtual List<CompanyTerm> CompanyTerms { get; set; }
        public virtual List<Image> Images { get; set; }

        public virtual Role Role { get; set; }
        public virtual City City { get; set; }
    }
}