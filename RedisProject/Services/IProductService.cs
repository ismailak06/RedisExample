using RedisProject.Models;

namespace RedisProject.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetListAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<bool> AnyAsync(string name);
    }
}
