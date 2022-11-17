using Microsoft.Extensions.Configuration;

namespace GermonenkoBy.Common.Web.Extensions;

public static class ConfigurationManagerExtensions
{
    public static T GetValueUnsafe<T>(this ConfigurationManager configurationManager, string key)
        => configurationManager.GetValue<T>(key)
           ?? throw new Exception("Configuration key is not found.");
}