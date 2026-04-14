using backend.DataAccess;
using backend.DataAccess.Repository;
using backend.Domain.Contract;
using backend.Domain.Repository;
using backend.Domain.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<MrpContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(MrpContext))));

builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IBomRepository, BomRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IBomService, BomService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ITreeService, TreeService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "http://127.0.0.1:5173",
                "http://localhost:4173",
                "http://127.0.0.1:4173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
