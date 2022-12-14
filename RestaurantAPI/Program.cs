using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using NLog.Web;
using Restaurant.DataAccess.DataAccess;
using Restaurant.DataAccess.Seeder;
using Restaurant.DataAccess.Validators;
using Restaurant.Models.Dto;
using Restaurant.Models.Models;
using RestaurantAPI;
using RestaurantAPI.Middleware;
using RestaurantAPI.Repository;
using RestaurantAPI.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddDbContext<RestaurantDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DatabaseConnection")));

var authenticationSettings = new AuthenticationSettings();
builder.Services.AddSingleton(authenticationSettings);

builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = "Bearer";
    opt.DefaultScheme = "Bearer";
    opt.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
    };
});

//builder.Services.AddMvc(opt =>
//{
//    opt.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
//});

builder.Services.AddScoped<DataGenerator>();
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IDishRepository, DishRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetService<DataGenerator>();
seeder.SeedDb();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();
app.UseAuthentication();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant Api");
});

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
//app.UseAuthorization();

app.MapControllers();

app.Run();