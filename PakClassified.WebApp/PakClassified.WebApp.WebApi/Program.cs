using a._PakClassified.WebApp.Entities.AppDbContext;
using b._PakClassified.WebApp.Services.Auth.Services;
using b._PakClassified.WebApp.Services.Enitities.Services.Location.Services;
using b._PakClassified.WebApp.Services.Enitities.Services.PakClassified.Services;
using b._PakClassified.WebApp.Services.Enitities.Services.RoleServices;
using b._PakClassified.WebApp.Services.Enitities.Services.UserServices;
using b._PakClassified.WebApp.Services.z._ModelHelper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PakClassified.WebApp.WebApi.Middlewares;
using Serilog;
using System.Text;

// https://localhost:7053/swagger/index.html

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()                                                    // Configure Error Logs
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(
        path: "logs/log-.txt",
        rollingInterval: RollingInterval.Day
    )
    .CreateLogger();

builder.Host.UseSerilog();





// Add services to the container.
builder.Services.AddControllers();




// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();




builder.Services.AddEndpointsApiExplorer();                                            // Register Authentication-Authorization for Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add JWT Bearer Authorization
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()       
        }
    });
});













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

builder.Services.AddScoped<IAuthService, AuthService>();






//Setting Up Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

                        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                    };
                });

//Setting Up CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:4200") //Audience allowed only from this URL...
               .AllowAnyHeader() // Allow all headers
               .AllowAnyMethod(); // Allow all HTTP methods
    });
});



builder.Services.AddAutoMapper(cfg => { }, typeof(AutoMapperProfile).Assembly);


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

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

app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

