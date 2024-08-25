using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RedisCache.DataAccess
{
    public class BaseRepository<T1> : IBaseRepository<T1, int> where T1 : class, new()
    {

        protected readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _redis;
        private readonly string KEY = "";
        public BaseRepository(ConnectionMultiplexer connectionMultiplexer, string key, int dbIndex)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _redis = GetDb(dbIndex);
            KEY = key;
        }

        public async Task<T1> AddAsync(T1 entity, int dbIndex)
        {
            if (!await _redis.KeyExistsAsync(KEY))
                await _redis.HashSetAsync(KEY, (int)typeof(T1).GetProperty("Id").GetValue(entity), JsonSerializer.Serialize(entity));

            return entity;
        }

        public async Task<bool> Any(T1 entity, int dbIndex = 0)
        {
            return await _redis.HashExistsAsync(KEY, (int)typeof(T1).GetProperty("Id").GetValue(entity));
        }

        public Task<bool> Delete(T1 entity, int dbIndex)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T1>> GetAll(T1 entity, int dbIndex = 0)
        {
            return (await _redis.HashGetAllAsync(KEY)).ToList().Select(x => JsonSerializer.Deserialize<T1>(x.Value)).ToList();
        }

        public async Task<T1> GetAsync(T1 entity, int dbIndex)
        {
            var cache = await _redis.HashGetAsync(KEY, (int)typeof(T1).GetProperty("Id").GetValue(entity));
            return cache.HasValue ? JsonSerializer.Deserialize<T1>(cache) : new T1();
        }
        private IDatabase GetDb(int dbIndex)
        {
            return _connectionMultiplexer.GetDatabase(dbIndex);
        }
    }
}
