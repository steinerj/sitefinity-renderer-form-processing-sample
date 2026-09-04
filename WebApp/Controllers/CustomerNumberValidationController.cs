using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("validation/customer-number")]
public sealed class CustomerNumberValidationController : ControllerBase
{
    private readonly ICustomerNumberSystem customerNumberSystem;

    public CustomerNumberValidationController(ICustomerNumberSystem customerNumberSystem)
    {
        this.customerNumberSystem = customerNumberSystem;
    }

    [HttpGet]
    public async Task<ActionResult<CustomerNumberValidationResult>> Validate(
        [FromQuery] string value,
        CancellationToken cancellationToken)
    {
        var result = await this.customerNumberSystem.ValidateAsync(value, cancellationToken);
        return this.Ok(result);
    }
}