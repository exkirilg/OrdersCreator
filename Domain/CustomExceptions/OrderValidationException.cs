using System.ComponentModel.DataAnnotations;

namespace Domain.CustomExceptions;

public class OrderValidationException : Exception
{
    public OrderValidationException(IEnumerable<ValidationResult> validationResults)
    {
        ValidationResults = validationResults;
    }

    public IEnumerable<ValidationResult> ValidationResults { get; private set; }
}
