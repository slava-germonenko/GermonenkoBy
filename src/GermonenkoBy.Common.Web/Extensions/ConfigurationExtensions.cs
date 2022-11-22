using Microsoft.Extensions.Configuration;

namespace GermonenkoBy.Common.Web.Extensions;

public static class ConfigurationExtensions
{
    public static T GetValueUnsafe<T>(this IConfiguration configurationManager, string key)
        => configurationManager.GetValue<T>(key)
           ?? throw new Exception($"Configuration key {key} is not found.");
}