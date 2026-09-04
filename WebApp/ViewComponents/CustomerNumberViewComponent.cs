using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Progress.Sitefinity.AspNetCore.FormWidgets.Models.TextField;
using Progress.Sitefinity.AspNetCore.ViewComponents;
using Progress.Sitefinity.AspNetCore.Web;
using Progress.Sitefinity.Renderer.Forms;

[SitefinityFormWidget(FormFieldType.ShortText, Title = "Customer number")]
[ViewComponent(Name = "SitefinityCustomerNumber")]
public sealed class CustomerNumberViewComponent : ViewComponent
{
    private readonly ITextFieldModel textFieldModel;

    public CustomerNumberViewComponent(ITextFieldModel textFieldModel)
    {
        this.textFieldModel = textFieldModel;
    }

    public async Task<IViewComponentResult> InvokeAsync(
        IViewComponentContext<CustomerNumberEntity> context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var viewModel = await this.textFieldModel.InitializeViewModel(context.Entity);
        return this.View(context.Entity.SfViewName, viewModel);
    }
}