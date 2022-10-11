namespace GermonenkoBy.Common.Domain.DataAnnotation;

public enum MimeTypes
{
    Image = 0x00000001,
    Document = 0x00000010,
    All = Image | Document
}