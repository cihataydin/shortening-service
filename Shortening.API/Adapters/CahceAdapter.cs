using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Shortening.API.Caching.Abstracts;
using Shortening.API.Constants;

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

        public string GetCachedOriginalUrl(string shortenedUrl)
        {
            var cacheObject = this.GetData<object>(shortenedUrl);

            if (cacheObject is not null)
            {
                return ((JObject)cacheObject)[CachingConstants.ORIGINAL_URL].ToString();
            }

            return default;
        }

        public bool CacheOriginalUrl(string code, string originalUrl)
        {
            if (string.IsNullOrWhiteSpace(code) || originalUrl is null)
                return false;

            var value = new Dictionary<string, string>
            {
                { CachingConstants.ORIGINAL_URL, originalUrl }
            };

            return this.SetData<object>(code, value, DateTimeOffset.Now.AddMinutes(5));
        }
    }
}
