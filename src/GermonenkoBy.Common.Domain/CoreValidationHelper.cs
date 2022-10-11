using System.ComponentModel.DataAnnotations;

using GermonenkoBy.Common.Domain.Exceptions;

namespace GermonenkoBy.Common.Domain;

public static class CoreValidationHelper
{
    public static void EnsureEntityIsValid(object entity, string defaultErrorMessage = "Ошибка валидации.")
    {
        var validationResult = new List<ValidationResult>();
        Validator.TryValidateObject(entity, new ValidationContext(entity), validationResult, true);
        if (validationResult.Any())
        {
            throw new CoreLogicException(validationResult.First().ErrorMessage ?? defaultErrorMessage);
        }
    }
}