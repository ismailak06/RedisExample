using RedisProject.Models;

namespace RedisProject.Managers
{
    public interface IProductManager
    {
        Task<List<Product>> GetListAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
    }
}
