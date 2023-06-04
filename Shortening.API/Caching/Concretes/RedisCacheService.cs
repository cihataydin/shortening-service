using Newtonsoft.Json;
using Shortening.API.Caching.Abstracts;
using Shortening.API.Constants;
using StackExchange.Redis;
using System;

namespace Shortening.API.Caching.Concretes
{
    public class RedisCacheService : ICacheService
    {
        private IDatabase _db;

        public RedisCacheService()
        {
            _db = ConnectionHelper.Connection.GetDatabase();
        }
        public T GetData<T>(string key)
        {
            var value = _db.StringGet(key);

            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }

            return default;
        }
        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);

            var isSet = _db.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);

            return isSet;
        }
        public object RemoveData(string key)
        {
            bool _isKeyExist = _db.KeyExists(key);

            if (_isKeyExist == true)
            {
                return _db.KeyDelete(key);
            }

            return false;
        }
    }

    public class ConnectionHelper
    {
        static ConnectionHelper()
        {
            ConnectionHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() => {
                return ConnectionMultiplexer.Connect(CachingConstants.REDIS_URL);
            }); 
        }
        private static Lazy<ConnectionMultiplexer> lazyConnection;
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
