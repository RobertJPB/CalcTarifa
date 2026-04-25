using Microsoft.Extensions.DependencyInjection;
using CalcTarifa.BusinessApplication.Interfaces.UseCases;
using CalcTarifa.BusinessApplication.UseCases;

namespace CalcTarifa.BusinessApplication.IoC
{
    public static class BusinessDependencyInjection
    {
        public static IServiceCollection AddBusinessApplication(this IServiceCollection services)
        {
            // Estrategias de cálculo (Componentes específicos)
            services.AddScoped<CalcTarifa.Domain.DomainServices.Strategies.ICalculoTarifaStrategy, CalcTarifa.Domain.DomainServices.Strategies.CalculoIndia>();
            services.AddScoped<CalcTarifa.Domain.DomainServices.Strategies.ICalculoTarifaStrategy, CalcTarifa.Domain.DomainServices.Strategies.CalculoUS>();
            services.AddScoped<CalcTarifa.Domain.DomainServices.Strategies.ICalculoTarifaStrategy, CalcTarifa.Domain.DomainServices.Strategies.CalculoUK>();

            // Resolutor y Servicio de Cálculo
            services.AddScoped<CalcTarifa.Domain.DomainServices.ITaxRateResolver, CalcTarifa.Domain.DomainServices.TaxRateResolver>();
            services.AddScoped<CalcTarifa.Domain.DomainServices.CalculationService>();

            services.AddScoped<ICalcularEnvioUseCase,    CalcularEnvioUseCase>();
            services.AddScoped<IConsultarTarifasUseCase, ConsultarTarifasUseCase>();
            services.AddScoped<IObtenerUltimosHistorialUseCase, ObtenerUltimosHistorialUseCase>();
            services.AddScoped<IObtenerHistorialPorEmailUseCase, ObtenerHistorialPorEmailUseCase>();
            return services;
        }
    }
}
