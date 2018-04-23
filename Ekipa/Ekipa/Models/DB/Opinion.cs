using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekipa.Models.DB
{
    public class Opinion
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int GradeValue { get; set; }
        public bool AdminAccept { get; set; }
        public bool IsDelete { get; set; }
        public int CompanyId { get; set; }
        public int RezervationId { get; set; }
        public virtual Reservation Rezervation { get; set; }
    }
}