using a._PakClassified.WebApp.Entities.AppDbContext;
using b._PakClassified.WebApp.Services.Auth.Services;
using b._PakClassified.WebApp.Services.Enitities.Services.Location.Services;
using b._PakClassified.WebApp.Services.Enitities.Services.PakClassified.Services;
using b._PakClassified.WebApp.Services.Enitities.Services.RoleServices;
using b._PakClassified.WebApp.Services.Enitities.Services.UserServices;
using b._PakClassified.WebApp.Services.z._ModelHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection on DBContext
builder.Services.AddDbContext<AppDBContext>
    (options => { options.UseSqlServer(builder.Configuration.GetConnectionString("default")); });

// Dependency Injection on Services
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IProvinceService, ProvinceService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ICityAreaService, CityAreaService>();

builder.Services.AddScoped<IAdvertisementService, AdvertisementService>();
builder.Services.AddScoped<IAdvertisementImageService, AdvertisementImageService>();
builder.Services.AddScoped<IAdvertisementTagService, AdvertisementTagService>();
builder.Services.AddScoped<IAdvertisementTypeService, AdvertisementTypeService>();
builder.Services.AddScoped<IAdvertisementCategoryService, AdvertisementCategoryService>();
builder.Services.AddScoped<IAdvertisementSubCategoryService, AdvertisementSubCategoryService>();
builder.Services.AddScoped<IAdvertisementStatusService, AdvertisementStatusService>();

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();

//builder.Services.AddScoped<IAuthService, AuthService>();




builder.Services.AddAutoMapper(cfg => { }, typeof(AutoMapperProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pak Classified API v1");
            c.RoutePrefix = "swagger"; // default
        });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
