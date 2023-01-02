using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ExternalServices.Configurations
{
    public class CacheServiceSetting
    {
        public int DefaultExpiryTimeInMinutes { get; set; }
        public int DefaultAllowedInActiveTimeInMinutes { get; set; }
    }
}
