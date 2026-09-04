using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("intercept/forms/submit")]
public class FormInterceptController : ControllerBase
{
    [HttpPost("{formName}/{culture}")]
    public async Task<IActionResult> Submit(
        string formName,
        string culture)
    {
        var form = await Request.ReadFormAsync();
        //process stuff HERE e.g.
        var customerNumber = form["CustomerNumber"].FirstOrDefault();
        var success = customerNumber != "INVALID";

        if (!success)
        {
            return UnprocessableEntity(new
            {
                success = false,
                error = "Custom processing failed"
            });
        }

        var sitefinitySubmitUrl = $"/forms/submit/{formName}/{culture}{Request.QueryString}";
        // results in HTTP 307 - not sure if that's best interception action or might lead to browser issues it appears to work fine
        return RedirectPreserveMethod(sitefinitySubmitUrl);
    }
}