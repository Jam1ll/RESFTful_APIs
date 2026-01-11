using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace core.application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            //toma todos los validators en el assembly actual (en core.application)
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //toma todos los handlers de los queries o commands CQRS
            //en el assembly actual (en core.application)
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
        }
    }
}
