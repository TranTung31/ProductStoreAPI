using Microsoft.EntityFrameworkCore;
using ProductStoreAPI.Application.Interfaces;
using ProductStoreAPI.Application.Interfaces.Catalog.Orders;
using ProductStoreAPI.Application.Mappings;
using ProductStoreAPI.Infrastructure;
using ProductStoreAPI.Infrastructure.Context;
using ProductStoreAPI.Infrastructure.Implement.Catalog.Categories;
using ProductStoreAPI.Infrastructure.Implement.Catalog.Orders;
using ProductStoreAPI.Infrastructure.Implement.Catalog.Products;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext.
builder.Services.AddDbContext<ProductStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProductStoreDB")));

// Add DI
//builder.Services.AddScoped<ICategoryService, CategorySevice>();
//builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddInfrastructure(builder.Configuration);

// Add AutoMapper.
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var app = builder.Build();

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
