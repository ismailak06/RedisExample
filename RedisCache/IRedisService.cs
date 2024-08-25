using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache
{
    public interface IRedisService
    {
        IDatabase GetDb(int dbIndex);
        //Task<T> LoadCache<T>(T data, int dbIndex, string key);
    }
}
