using Microsoft.EntityFrameworkCore;
using RedisCache;
using RedisProject.Managers;
using RedisProject.Models;
using RedisProject.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSingleton<IRedisService, RedisService>(sp =>
{
    return new RedisService(builder.Configuration.GetConnectionString("Redis"));
});
builder.Services.AddScoped<IProductManager, ProductManager>();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseInMemoryDatabase("MyDataBase");
});

builder.Services.AddSingleton<IDatabase>(sp =>
{
    var redisService = sp.GetRequiredService<IRedisService>();
    return redisService.GetDb(0);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
