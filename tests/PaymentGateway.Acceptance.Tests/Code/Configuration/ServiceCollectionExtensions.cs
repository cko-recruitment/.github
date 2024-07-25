using Microsoft.Extensions.DependencyInjection;

namespace PaymentGateway.Acceptance.Tests.Code.Configuration;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection ReplaceScoped<T>(this IServiceCollection services, Func<IServiceProvider, object> factory)
        where T : class
    {
        var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(T));

        if (descriptor != null)
            services.Remove(descriptor);

        services.AddScoped(typeof(T), factory);

        return services;
    }
}
