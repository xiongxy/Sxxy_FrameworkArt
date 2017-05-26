using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sxxy_FrameworkArt.Models.SystemEntity
{
    public class SystemDomain
    {
        public string DomainName { get; set; }

        public string DomainAddress { get; set; }
        public int? DomainPort { get; set; }
        public string EntryUrl { get; set; }
        public string Address
        {
            get
            {
                var rv = "http://" + DomainAddress;
                if (DomainPort != null)
                {
                    rv += ":" + DomainPort;
                }
                return rv;
            }
        }
    }
}
