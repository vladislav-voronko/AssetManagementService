using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AssetManagementService.Application.Extensions
{
    public static class ValidationServiceRegistration
    {
        public static IServiceCollection AddApplicationValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateAssetRequestValidator>();

            return services;
        }
    }
}
