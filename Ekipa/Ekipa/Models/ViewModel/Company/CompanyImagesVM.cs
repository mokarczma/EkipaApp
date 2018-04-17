using Ekipa.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekipa.Models.ViewModel.Company
{
    public class CompanyImagesVM
    {
        public int ID { get; set; }
        public List<ImageVM> ImageList { get; set; } 
        public string MainPicturePath { get; set; }
    }
}