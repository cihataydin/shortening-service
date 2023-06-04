using Microsoft.EntityFrameworkCore;
using Shortening.API.Caching.Abstracts;

namespace Shortening.API.Adapters
{
    public class CahceAdapter
    {
        private readonly ICacheService _cacheService;
        private static readonly object _lock = new object();
        public CahceAdapter(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        public T GetData<T>(string key)
        {
            if(!string.IsNullOrWhiteSpace(key))
                return _cacheService.GetData<T>(key);

            return default;
        }

        public object RemoveData(string key)
        {
            if (!string.IsNullOrWhiteSpace(key))
                return _cacheService.RemoveData(key);

            return default;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            if(!string.IsNullOrWhiteSpace(key) && value is not null)
            {
                lock (_lock)
                {
                    return _cacheService.SetData<T>(key, value, expirationTime);
                }
            }

            return false;
        }
    }
}
