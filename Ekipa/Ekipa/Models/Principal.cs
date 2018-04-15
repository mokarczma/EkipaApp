using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Ekipa.Models
{
    public class MPrincipal : GenericPrincipal
    {
        public MPrincipal(IIdentity identity, string[] roles)
            : base(identity, roles)
        {
        }
        public AuthUser UserDetails { get; set; }

    }
}