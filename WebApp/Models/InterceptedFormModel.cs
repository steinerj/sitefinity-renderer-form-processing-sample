using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Progress.Sitefinity.AspNetCore.Configuration;
using Progress.Sitefinity.AspNetCore.ViewComponents;
using Progress.Sitefinity.AspNetCore.Web;
using Progress.Sitefinity.AspNetCore.Widgets.Models.Form;
using Progress.Sitefinity.AspNetCore.Widgets.ViewComponents.Common;
using Progress.Sitefinity.RestSdk.OData;

public class InterceptedFormModel : FormModel
{
    public InterceptedFormModel(
        IODataRestClient restService,
        IRequestContext requestContext,
        IStyleClassesProvider styles,
        IViewComponentTreeBuilder treeBuilder,
        IRenderContext renderContext,
        IStringLocalizer<FormModel> localizer,
        ISitefinityConfig sfConfig)
        : base(
            restService,
            requestContext,
            styles,
            treeBuilder,
            renderContext,
            localizer,
            sfConfig)
    {
    }

    public override async Task<FormViewModel> InitializeViewModel(
        FormEntity entity,
        IQueryCollection query)
    {
        var model = await base.InitializeViewModel(entity, query);

        // path /forms/submit/MyForm/en becomesintercept/forms/submit/MyForm/en with custom controller
        // change to "/intercept-client" if form submission postprocessing is needed in the same controller
        model.SubmitUrl = "/intercept" + model.SubmitUrl;

        return model;
    }
}