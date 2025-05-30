using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Service.PipelineBehavior;
using System.Reflection;

namespace UserManagement.Service
{
    public static class ServiceDI
    {
        public static WebApplicationBuilder AddPresentation(this WebApplicationBuilder builder)
        {
            builder
                .AddMediatR()
                .AddFluentValidation();

            //builder.Services
            //    .AddFeatureManagement();

            return builder;
        }

        private static WebApplicationBuilder AddMediatR(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            });

            return builder;
        }

        private static WebApplicationBuilder AddFluentValidation(this WebApplicationBuilder builder)
        {
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return builder;
        }
    }
}
