using Microsoft.AspNetCore.Mvc;
using RedisCache;
using RedisProject.Managers;
using RedisProject.Models;
using RedisProject.Services;
using StackExchange.Redis;

namespace RedisProject.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductManager _productManager;

        public ProductController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            return Ok(await _productManager.GetListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _productManager.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            return Created(string.Empty, await _productManager.AddAsync(product));
        }
    }
}