using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Infrastructure;
using Microsoft.FeatureManagement;
using Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Repositories;

namespace UserManagement.Infrastructure
{
    public static class InfraDI
    {
        public static WebApplicationBuilder AddInfra(this WebApplicationBuilder builder)
        {
            _ = builder
                .AddFeatureManagement()
                .AddPersistenceAsync();
            

            return builder;
        }


        private static async Task<WebApplicationBuilder> AddPersistenceAsync(this WebApplicationBuilder builder)
        {
            // Register EFCore DbContext
            builder.Services.AddDbContext<LocalDbContext>();

            var featureManager = builder.Services.BuildServiceProvider().GetRequiredService<IFeatureManager>();

            builder.Services.AddTransient<IUserRepository, UserEfCoreRepository>();
            

            return builder;
        }

        private static WebApplicationBuilder AddFeatureManagement(this WebApplicationBuilder builder)
        {
            builder.Services.AddFeatureManagement();

            return builder;
        }

        public static void UseEFCoreMigrations(this WebApplication app)
        {
            using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            dbContext.Database.Migrate();
        }



    }

}
