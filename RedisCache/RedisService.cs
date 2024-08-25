using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache
{
    public class RedisService : IRedisService
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public RedisService(string url)
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(url);
        }

        public IDatabase GetDb(int dbIndex)
        {
            return _connectionMultiplexer.GetDatabase(dbIndex) ?? throw new Exception("Belirtilen db bulunamadı.");
        }

        //public async Task<T> LoadCache<T>(T data, int dbIndex, string key)
        //{
        //    var cache = GetDb(dbIndex);
        //
        //    T result;
        //
        //    if (!await cache.KeyExistsAsync(key))
        //        result = ;
        //}
    }
}