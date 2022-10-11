using System.ComponentModel.DataAnnotations;

namespace GermonenkoBy.Common.Domain.DataAnnotation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class StringValuesAttribute : ValidationAttribute
{
    private readonly string[] _allowedValues;

    public StringComparison Comparison { get; set; }

    public StringValuesAttribute(params string[] allowedValues)
    {
        _allowedValues = allowedValues;
    }

    public override bool IsValid(object? value)
    {
        var stringValue = value?.ToString();
        // Automatically pass if value is null or empty. RequiredAttribute should be used to assert a value is not empty.
        if (stringValue is null)
        {
            return true;
        }

        return _allowedValues.Any(val => val.Equals(stringValue, Comparison));
    }
}