using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ExternalServices.DTOs
{
    public class CacheDto
    {
        public CacheDto(object data, DateTime currentDate, DateTimeOffset expiryDate, TimeSpan? absoluteExpirationRelativeTime, TimeSpan? inactiveTime)
        {
            Data = data;
            InitiationTime = currentDate;
            AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeTime;
            AbsoluteExpiration = expiryDate;
            SlidingExpiration = inactiveTime;
        }


        public object Data { get; set; }
        public DateTime InitiationTime { get; set; }
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
        public DateTimeOffset AbsoluteExpiration { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }
    }
}
