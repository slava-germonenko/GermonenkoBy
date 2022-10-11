namespace GermonenkoBy.Common.Domain.DataAnnotation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class MimeTypesAttribute : StringValuesAttribute
{
    public MimeTypesAttribute(MimeTypes validMimeTypes)
        : base(MapMimeTypesToStringRepresentation(validMimeTypes))
    {
        Comparison = StringComparison.InvariantCultureIgnoreCase;
    }

    private static string[] MapMimeTypesToStringRepresentation(MimeTypes validMimeTypes)
    {
        if (validMimeTypes.HasFlag(MimeTypes.All))
        {
            return MimeTypesMap.Select(pair => pair.Value).ToArray();
        }

        var mimes = new List<string>(MimeTypesMap.Count);

        if (validMimeTypes.HasFlag(MimeTypes.Image))
        {
            mimes.AddRange(GetMimeTypeStringRepresentation(MimeTypes.Image));
        }

        if (validMimeTypes.HasFlag(MimeTypes.Document))
        {
            mimes.AddRange(GetMimeTypeStringRepresentation(MimeTypes.Document));
        }

        return mimes.ToArray();
    }

    private static string[] GetMimeTypeStringRepresentation(MimeTypes mimeType) => MimeTypesMap
        .Where(m => m.Key == mimeType)
        .Select(m => m.Value)
        .ToArray();

    private static readonly List<KeyValuePair<MimeTypes, string>> MimeTypesMap =
        new ()
        {
            new(MimeTypes.Image, "image/avif"),
            new(MimeTypes.Image, "image/bmp"),
            new(MimeTypes.Image, "image/vnd.microsoft.icon"),
            new(MimeTypes.Image, "image/jpeg"),
            new(MimeTypes.Image, "image/png"),
            new(MimeTypes.Image, "image/svg+xml"),
            new(MimeTypes.Image, "image/tiff"),
            new(MimeTypes.Image, "image/webp"),
            new(MimeTypes.Image, "image/gif"),
            new(MimeTypes.Document, "application/msword"),
            new(MimeTypes.Document, "application/vnd.openxmlformats-officedocument.wordprocessingml.document"),
            new(MimeTypes.Document, "application/epub+zip"),
            new(MimeTypes.Document, "application/vnd.oasis.opendocument.presentation"),
            new(MimeTypes.Document, "application/vnd.oasis.opendocument.spreadsheet"),
            new(MimeTypes.Document, "application/vnd.oasis.opendocument.text"),
            new(MimeTypes.Document, "application/pdf"),
            new(MimeTypes.Document, "application/vnd.ms-powerpoint"),
            new(MimeTypes.Document, "application/vnd.openxmlformats-officedocument.presentationml.presentation"),
            new(MimeTypes.Document, "application/vnd.ms-excel"),
            new(MimeTypes.Document, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"),
        };
}