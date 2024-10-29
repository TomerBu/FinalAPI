using DataAccess.Data;
using DataAccess.Models;
using FinalAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FinalAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<DataContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("DataContext") ?? throw new InvalidOperationException("Connection string 'ContextDAL' not found.")));

        //add identity services to the di: (enables us to inject UserManager, RoleManager)
        builder.Services.AddIdentity<AppUser, IdentityRole<int>>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 8;
        }).AddEntityFrameworkStores<DataContext>();


        //get jwtSettings from the appsettings json file:
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");

        //auth setup:
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
            };
        });

        //JWT {Claims}

        // builder.Services.AddScoped<IRepository<Product>, ProductsRepository>();  
        builder.Services.AddScoped<ProductRepository>();
        builder.Services.AddScoped<CategoryRepository>();


        //add our Service to the di container
        builder.Services.AddScoped<JwtService>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        var corsPolicy = "CorsPolicy";

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: corsPolicy, policy =>
            {
                policy.WithOrigins(["http://localhost:3000",
                        "http://localhost:5173",
                        "http://localhost:5174",
                        "https://wonderful-wave-02702e400.5.azurestaticapps.net"
                        ])
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        var app = builder.Build();

        app.UseCors(corsPolicy);

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<DataContext>();
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing migration: " + ex.Message);
            }
        }

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        app.UseSwagger();
        app.UseSwaggerUI();
        //}

        app.UseHttpsRedirection();


        //who is the user?
        app.UseAuthentication();

        //does the user have permission?
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}