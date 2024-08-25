using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RedisCache.DataAccess
{
    public interface IBaseRepository<T1, T2> where T1 : class, new()
    {
        Task<T1> GetAsync(T1 entity, T2 dbIndex);
        Task<List<T1>> GetAll(T1 entity, T2 dbIndex);
        Task<T1> AddAsync(T1 entity, T2 dbIndex);
        Task<bool> Delete(T1 entity, T2 dbIndex);
        Task<bool> Any(T1 entity, T2 dbIndex);
    }
}