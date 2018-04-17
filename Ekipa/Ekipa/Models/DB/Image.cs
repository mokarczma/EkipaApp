using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekipa.Models.DB
{
    public class Image
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public bool IsDelete { get; set; }
        public bool MainPicture { get; set; }
        public virtual ICollection<Company> Company { get; set; }
               
    }
}