using System;
using CaioOliveira.CepFinder.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CaioOliveira.CepFinder.ViaCep.Configuration
{
    public static class Configuration
    {
        public static void UseViaCep(this IServiceCollection services)
        {
            var configuration = new ServiceConfiguration
            {
                BaseUrl = "https://viacep.com.br/ws"
            };

            Configure(services, configuration);
        }

        public static void UseViaCep(this IServiceCollection services, Action<ServiceConfiguration> options)
        {
            var configuration = new ServiceConfiguration();
            options(configuration);

            Configure(services, configuration);
        }

        private static void Configure(IServiceCollection services, ServiceConfiguration configuration)
        {
            services.AddScoped<ICepFinderService, Provider>();
            services.Configure<ServiceConfiguration>(x => x.Bind(configuration));
        }
    }
}