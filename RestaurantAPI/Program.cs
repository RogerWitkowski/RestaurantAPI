using Microsoft.EntityFrameworkCore;
using Restaurant.DataAccess.DataAccess;
using Restaurant.DataAccess.Seeder;
using RestaurantAPI.Repository;
using RestaurantAPI.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<RestaurantDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DatabaseConnection")));

builder.Services.AddScoped<DataGenerator>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetService<DataGenerator>();
seeder.SeedDb();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();