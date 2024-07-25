namespace PaymentGateway.Api.Code.Extensions;

public static class ConfigurationSectionExtensions
{
    public static T Bind<T>(this IConfigurationSection configurationSection) where T : new()
    {
        var configuration = new T();
        configurationSection.Bind(configuration);

        return configuration;
    }
}
