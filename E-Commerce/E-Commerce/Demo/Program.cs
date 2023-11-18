using Core;
using Demo.Helper;
using Demo.Middlewares;
using Microsoft.EntityFrameworkCore;
using Demo.Extensions;
using StackExchange.Redis;
using Core.DbContexts;
using Core.IdentityEntities;

namespace Demo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            builder.Services.AddApplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
           // builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerDocumentation();

            builder.Services.AddCors(policy =>
            {
                policy.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });

            var app = builder.Build();

            await ApplySeeding.ApplySeedingAsync(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseMiddleware<ExceptionMiddleware>();
            }

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}