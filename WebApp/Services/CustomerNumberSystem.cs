using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public interface ICustomerNumberSystem
{
    Task<CustomerNumberValidationResult> ValidateAsync(
    string customerNumber,
        CancellationToken cancellationToken);
}

public sealed record CustomerNumberValidationResult(bool IsValid, string Error);

public sealed class PocCustomerNumberSystem : ICustomerNumberSystem
{
    private static readonly HashSet<string> KnownCustomerNumbers = new(StringComparer.OrdinalIgnoreCase)
    {
        "CUST-1000",
        "CUST-2000"
    };

    public Task<CustomerNumberValidationResult> ValidateAsync(
        string customerNumber,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var isValid = customerNumber != null && KnownCustomerNumbers.Contains(customerNumber.Trim());
        var error = isValid ? string.Empty : "Unknown customer number. Try CUST-1000.";

        return Task.FromResult(new CustomerNumberValidationResult(isValid, error));
    }
}