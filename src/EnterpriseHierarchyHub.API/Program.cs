
using EnterpriseHierarchyHub.Core.Config;
using EnterpriseHierarchyHub.Core.Features.SearchUnits;
using EnterpriseHierarchyHub.Core.Services;
using EnterpriseHierarchyHub.Core.Services.Impl;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EnterpriseHierarchyHub.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Unique Setup For This Project

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SearchUnitsQueryHandler).Assembly));

            builder.Services.AddTransient<IOrganizationService, OrganizationService>();

            builder.Services.Configure<StorageSettings>(builder.Configuration.GetSection("StorageSettings"));
            
            builder.Services.AddSingleton<IMongoDatabase>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<StorageSettings>>().Value;
                var client = new MongoClient(settings.ConnectionString);
                return client.GetDatabase(settings.DatabaseName);
            });

            #endregion

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

            var mongoIndexMigrator = app.Services.GetRequiredService<IOrganizationService>();
            var logger = app.Services.GetService<ILogger>();
            mongoIndexMigrator.EnsureIndexesCreated();
            logger.LogInformation($"Mongo Indexes Migration Status - OK!");
        }
    }
}