using Microsoft.EntityFrameworkCore;
using MyAzureApi.Data;
using Azure.Identity;

namespace MyAzureApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var keyVaultUrl = new Uri("https://myfirstazurekeyvault.vault.azure.net/");

            Azure.Core.TokenCredential credential;

            if (builder.Environment.IsDevelopment())
            {
                credential = new VisualStudioCredential();
            }
            else
            {
                credential = new DefaultAzureCredential();
            }

            builder.Configuration.AddAzureKeyVault(keyVaultUrl, credential);



            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DBContext>(options =>
            {
                var sqlConnectionString = builder.Configuration["azuresqlconnection"];
                options.UseSqlServer(sqlConnectionString);
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
