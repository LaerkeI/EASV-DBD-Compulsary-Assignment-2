using BookStoreApp.Controllers;
using MongoDB.Driver;
using StackExchange.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;

namespace BookStoreApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Add MongoDB service
            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var mongoConnectionString = configuration.GetConnectionString("MongoDBConnection");
                return new MongoClient(mongoConnectionString);
            });

            // Add Redis service
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var redisConnectionString = configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(redisConnectionString);
            });

            // Configure SqlConnection for MSSQL
            builder.Services.AddTransient<SqlConnection>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("MSSQLConnection");
                return new SqlConnection(connectionString);
            });

            // Add Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
        }
    }
}
