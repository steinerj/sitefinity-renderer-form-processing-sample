using System;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Progress.Sitefinity.RestSdk.Clients.Forms;

[ApiController]
[Route("intercept-client/forms/submit")]
public class FormSubmissionClientController : ControllerBase
{
    private readonly IFormSubmissionClient submissionClient;
    private readonly ILogger<FormSubmissionClientController> logger;

    public FormSubmissionClientController(
        IFormSubmissionClient submissionClient,
        ILogger<FormSubmissionClientController> logger)
    {
        this.submissionClient = submissionClient;
        this.logger = logger;
    }

    [HttpPost("{formName}/{culture}")]
    public async Task<IActionResult> Submit(
        string formName,
        string culture)
    {
        var form = await this.Request.ReadFormAsync(this.HttpContext.RequestAborted);

        var customerNumber = form["CustomerNumber"].FirstOrDefault();
        if (customerNumber == "INVALID")
        {
            return this.UnprocessableEntity(new
            {
                success = false,
                error = "Custom processing failed"
            });
        }

        var formData = form
            .Where(field => !string.Equals(field.Key, "sf_antiforgery", StringComparison.OrdinalIgnoreCase))
            .ToDictionary(field => field.Key, field => field.Value.ToString());

        var fileData = form.Files
            .GroupBy(file => file.Name)
            .ToDictionary(
                group => group.Key,
                group => group.Select(file => new FileData
                {
                    FileName = file.FileName,
                    ContentType = string.IsNullOrWhiteSpace(file.ContentType)
                        ? MediaTypeNames.Application.Octet
                        : file.ContentType,
                    FileStream = file.OpenReadStream()
                }).ToArray());

        try
        {
            var result = await this.submissionClient.SubmitForm(formName, formData, fileData);

            if (result.Success)
            {
                // Perform success postprocessing here
            }

            return this.Ok(result);
        }
        catch (FormSubmissionException exception)
        {
            return this.BadRequest(new
            {
                success = false,
                error = exception.Message
            });
        }
        catch (HttpRequestException exception)
        {
            this.logger.LogError(exception, "Could not submit form {FormName} to Sitefinity", formName);

            return this.StatusCode(StatusCodes.Status502BadGateway, new
            {
                success = false,
                error = "Sitefinity form submission is unavailable"
            });
        }
    }
}