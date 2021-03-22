using Microsoft.Extensions.Configuration;

namespace Votacao.Extensions
{
    public static class ConfigurationExtension
    {
        public static TConfig GetSection<TConfig>(this IConfiguration configuration)
        {
            return configuration.GetSection(typeof(TConfig).Name).Get<TConfig>();
        }
    }
}
