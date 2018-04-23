using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekipa.Models.ViewModel
{
    public class LogedUserVM
    {
        public bool Loged { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int UserRole{ get; set;}

    }
}
