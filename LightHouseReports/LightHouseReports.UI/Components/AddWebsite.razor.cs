using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Core.Interfaces;
using LightHouseReports.Core.Interfaces.Models;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.Data.Interfaces.Models;
using LightHouseReports.UI.Pages;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace LightHouseReports.UI.Components;

public partial class AddWebsite
{
    [CascadingParameter] public MudDialogInstance MudDialog { get; set; } = null!;
    private EditForm _editFrom = null!;
    private bool _isValidating = false;
    private Input _input = new();

    private async Task AddWebsiteModel()
    {
        try
        {
            var result = await ValidateInput();
            if (result is not null && result.IsValid)
            {
                var baseUri = new UriBuilder(_input.Website).Uri;
                var siteMapUri = new Uri(baseUri!, "sitemap.xml");
                var sitmapResult = await Mediator.Request<GetSitemapCoreModel, Result<SitemapCoreModel>>(new GetSitemapCoreModel(siteMapUri.ToString()));

                await Mediator.Send(new AddWebsiteDataModel(new WebsiteDataModel(Guid.NewGuid(), _input.Website, sitmapResult.Value.Locs.Count)));
                _input = new Input();
                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                AddValidationMessages(result);
            }
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private void CloseDialog()
    {
        try
        {
            MudDialog.Cancel();
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private void ClearValidationMessages()
    {
        var value = _input.Website;
        _input = new Input { Website = value };
    }

    private async Task<ValidationResult> ValidateInput()
    {
        ClearValidationMessages();
        _isValidating = true;
        await InvokeAsync(StateHasChanged);
        var validator = new Validator(Mediator);
        var result = await validator.ValidateAsync(_input);
        _isValidating = false;
        await InvokeAsync(StateHasChanged);
        return result;
    }

    private void AddValidationMessages(ValidationResult? result)
    {
        var editContext = _editFrom.EditContext;
        var validationMessageStore = new ValidationMessageStore(editContext!);

        foreach (var error in result!.Errors!)
        {
            var fieldIdentifier = new FieldIdentifier(_input, error.PropertyName);
            validationMessageStore.Add(fieldIdentifier, error.ErrorMessage);
        }

        editContext?.NotifyValidationStateChanged();
    }

    public class Input
    {
        public string Website { get; set; } = string.Empty;
    }

    public class Validator : AbstractValidator<Input>
    {
        public Validator(IMediator mediator)
        {
            RuleFor(x => x.Website).NotEmpty();
            RuleFor(x => x.Website).CustomAsync(async (website, context, ct) =>
            {
                try
                {
                    var baseUri = new UriBuilder(website).Uri;
                    var siteMapUri = new Uri(baseUri!, "sitemap.xml");
                    var result = await mediator.Request<GetSitemapCoreModel, Result<SitemapCoreModel>>(new GetSitemapCoreModel(siteMapUri.ToString()));
                    if (result.IsFailed) context.AddFailure("Can't access the sitemap");
                }
                catch (Exception e)
                {
                    context.AddFailure("Invaild url");
                }
            });
        }
    }
}