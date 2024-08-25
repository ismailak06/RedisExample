using RedisCache;
using RedisProject.Models;
using RedisProject.Services;
using StackExchange.Redis;
using System.Diagnostics.Eventing.Reader;
using System.Text.Json;

namespace RedisProject.Managers
{
    public class ProductManager : IProductManager
    {
        private const string PRODUCTKEY = "productCaches";
        private readonly IProductService _productService;
        private readonly IRedisService _redisService;
        private readonly IDatabase _redisCache;
        public ProductManager(IProductService productService, IRedisService redisService)
        {
            _productService = productService;
            _redisService = redisService;
            _redisCache = _redisService.GetDb(1);
        }

        public async Task<Product> AddAsync(Product product)
        {
            var newProduct = product;
            if (!await _productService.AnyAsync(newProduct.Name))
            {
                newProduct = await _productService.AddAsync(product);
            }
            if (!await _redisCache.HashExistsAsync(PRODUCTKEY, newProduct.Id))
            {
                await _redisCache.HashSetAsync(PRODUCTKEY, newProduct.Id, JsonSerializer.Serialize(newProduct));
            }

            return newProduct;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = new Product();
            if (await _redisCache.KeyExistsAsync(PRODUCTKEY))
            {
                var cacheProduct = await _redisCache.HashGetAsync(PRODUCTKEY, id);
                product = cacheProduct.HasValue ? JsonSerializer.Deserialize<Product>(cacheProduct) : null;
            }

            product ??= await _productService.GetByIdAsync(id);

            return product;
        }

        public async Task<List<Product>> GetListAsync()
        {
            if (!await _redisCache.KeyExistsAsync(PRODUCTKEY))
                return await LoadCacheFromDbAsync();

            var cacheProducts = await _redisCache.HashGetAllAsync(PRODUCTKEY);
            List<Product> products = new List<Product>();

            cacheProducts.ToList().ForEach(hash =>
            {
                products.Add(JsonSerializer.Deserialize<Product>(hash.Value));
            });
            //List<Product> products = (await _redisCache.HashGetAllAsync(PRODUCTKEY)).ToList().Select(x => JsonSerializer.Deserialize<Product>(x.Value)).ToList();

            return products;
        }

        private async Task<List<Product>> LoadCacheFromDbAsync()
        {
            var products = await _productService.GetListAsync();

            products.ForEach(product =>
            {
                _redisCache.HashSetAsync(PRODUCTKEY, product.Id, JsonSerializer.Serialize(product));
            });

            return products;
        }
    }
}
