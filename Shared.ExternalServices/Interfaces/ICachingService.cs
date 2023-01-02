using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ExternalServices.Interfaces
{
    public interface ICachingService
    {
        Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default);
        Task SetAsync<T>(string key, T data, TimeSpan? absoluteExpireTimeInMinutes = null,
                                                   TimeSpan? slidingExpireTimeInMinutes = null, CancellationToken cancellationToken = default);
        Task UpdateAsync<T>(string key, T data, bool requiresTimeReset = true, TimeSpan? absoluteExpireTimeInMinutes = null, TimeSpan? slidingExpireTimeInMinutes = null, CancellationToken cancellationToken = default);
        Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    }
}
