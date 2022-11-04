namespace GermonenkoBy.Common.Utils;

public static class ObjectsExtensions
{
    public static Dictionary<string, string?> ToDictionary(this object source) => source.GetType()
        .GetProperties()
        .Where(prop => prop.CanRead)
        .ToDictionary(prop => prop.Name, prop => prop.GetValue(source)?.ToString());
}