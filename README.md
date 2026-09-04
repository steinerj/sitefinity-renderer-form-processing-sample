# Sitefinity complex form processing sample

An ASP.NET Core Renderer sample for Sitefinity forms that require custom business rules beyond built-in field validation.

The sample implements a CustomerNumbe` field and shows:

- A custom Sitefinity form widget and Razor field template
- Asynchronous client-side validation on blur and submit
- Server-side validation through a renderer API
- Submission interception with revalidation before forwarding to Sitefinity
- An alternative IFormSubmissionClient path for post-processing successful submissions

Sample values for customer accepts `CUST-1000` and `CUST-2000`. Replace it with the relevant CRM, ERP, or external API.

## Demo files

- `WebApp/ViewComponents/CustomerNumberViewComponent.cs` registers the form widget.
- `WebApp/Views/Shared/Components/CustomerNumber/Default.cshtml` renders the semantic field.
- `WebApp/wwwroot/scripts/scripts.js` provides client validation.
- `WebApp/Services/CustomerNumberSystem.cs` contains the shared validation contract.
- `WebApp/Controllers/FormInterceptController.cs` validates and forwards submissions.
- `WebApp/Controllers/FormSubmissionClientController.cs` demonstrates direct SDK submission and post-processing.
- `WebApp/Models/InterceptedFormModel.cs` redirects generated form actions through the interceptor.

## Run

Configure the `Sitefinity` section in `WebApp/appsettings.json`, then run:

```console
dotnet run --project WebApp/RendererFormsTest.csproj
```

In Sitefinity, add the **Customer number** widget to a form and use `CustomerNumber` as its field name. The renderer runs on `http://localhost:5000` and `https://localhost:5001` by default.
