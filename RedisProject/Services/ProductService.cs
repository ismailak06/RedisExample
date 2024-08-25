using Microsoft.EntityFrameworkCore;
using RedisProject.Models;

namespace RedisProject.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _appDbContext;

        public ProductService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Product> AddAsync(Product product)
        {
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();

            return product;
        }

        public async Task<bool> AnyAsync(string name)
        {
            return await _appDbContext.Products.AnyAsync(x => x.Name == name);
        }

        public async Task<Product> GetByIdAsync(int id) => await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<Product>> GetListAsync() => await _appDbContext.Products.ToListAsync();
    }
}