using core.application.Behaviors;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace core.application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {

            #region Mapster Configuration

            //configuracion global de Mapster
            var config = TypeAdapterConfig.GlobalSettings;

            //escanea todo el assembly actual (en core.application) para encontrar configs de mapeo
            config.Scan(Assembly.GetExecutingAssembly());

            //registra el mapper como singleton para mayor rendimiento
            services.AddSingleton(config);

            //registra el mapper como scoped para inyeccion de dependencias
            services.AddScoped<IMapper, ServiceMapper>();

            #endregion

            #region validators and mediatr

            //toma todos los validators en el assembly actual (en core.application)
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //registra el comportamiento de validacion para CQRS
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            //toma todos los handlers de los queries o commands CQRS
            //en el assembly actual (en core.application)
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            #endregion
        }
    }
}
