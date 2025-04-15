using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Online_Library.Data;
using OnlineLibrary.Data;
using OnlineLibrary.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Description = "Standard Authorization header using the Bearer scheme (\" bearer {token}\")",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,

            });

            options.OperationFilter<SecurityRequirementsOperationFilter>();
        }); builder.Services.AddDbContext<OnlineLibraryContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        builder.Services.AddScoped<IAuthRepository, AuthRepository>();
        builder.Services.AddScoped(typeof(IDataRepository<>), typeof(Repository<>)); //DI
        builder.Services.AddScoped(typeof(IBorrowedBooksRepository), typeof(BorrowedBooksRepository));
        builder.Services.AddAutoMapper(typeof(Program).Assembly);

        var app = builder.Build();
        app.UseStaticFiles();
         app.UseRouting();

          app.UseEndpoints(endpoints =>
         {
           endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller}/{action=Index}/{id?}");

         endpoints.MapFallbackToFile("index.html");
        });

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
         app.MapControllers();

        app.Run();
    }
}