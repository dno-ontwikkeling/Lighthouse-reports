using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Core.Interfaces;
using LightHouseReports.Core.Interfaces.Models;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.Data.Interfaces.Models;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace LightHouseReports.UI.Components;

public partial class AddWebsite
{
    [CascadingParameter] public MudDialogInstance MudDialog { get; set; } = null!;
    private EditForm _editFrom = null!;
    private bool _isValidating;
    private Input _input = new();

    private async Task AddWebsiteModel()
    {
        try
        {
            var result = await ValidateInput();
            if (result.IsValid)
            {
                await Mediator.Send(new AddWebsiteDataModel(_input.ToNewDataModel()));
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
        var website = _input.Website;
        var name = _input.Name;
        var sitemaps = _input.Sitemaps;
        _input = new Input { Website = website, Name = name, Sitemaps = sitemaps };
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
        public string Name { get; set; }
        public string Website { get; set; } = string.Empty;
        public string Sitemaps { get; set; } = string.Empty;
        public string[] ParsedSitemaps => Sitemaps.Split(",", StringSplitOptions.RemoveEmptyEntries);

        public int FoundUrls { get; set; }

        public WebsiteDataModel ToNewDataModel()
        {
            return new WebsiteDataModel(Guid.NewGuid(), Name, Website, ParsedSitemaps, FoundUrls);
        }
    }

    public class Validator : AbstractValidator<Input>
    {
        public Validator(IMediator mediator)
        {
            RuleFor(x => x.Website).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Website).CustomAsync(async (website, context, _) =>
            {
                try
                {
                    var baseUri = new UriBuilder(website).Uri;
                    var siteMapUri = new Uri(baseUri, "sitemap.xml");
                    var result = await mediator.Request<GetSitemapCoreModel, Result<SitemapCoreModel>>(new GetSitemapCoreModel(siteMapUri.ToString()));
                    if (result.IsFailed) context.AddFailure("Can't access the sitemap");
                    else
                        context.InstanceToValidate.FoundUrls += result.Value.Locs.Count;
                }
                catch (Exception)
                {
                    context.AddFailure("Invaild url");
                }
            }).When(x => string.IsNullOrWhiteSpace(x.Sitemaps));

            RuleFor(x => x.Sitemaps).CustomAsync(async (website, context, _) =>
            {
                var sitemaps = website.Split(",").Select(x => x.Trim());

                try
                {
                    foreach (var sitemap in sitemaps)
                    {
                        var baseUri = new UriBuilder(website).Uri;
                        var siteMapUri = new Uri(baseUri, "sitemap.xml");
                        var result = await mediator.Request<GetSitemapCoreModel, Result<SitemapCoreModel>>(new GetSitemapCoreModel(siteMapUri.ToString()));
                        if (result.IsFailed) context.AddFailure($"Can't access the sitemap {sitemap}");
                        else
                            context.InstanceToValidate.FoundUrls += result.Value.Locs.Count;
                    }
                }
                catch (Exception)
                {
                    context.AddFailure("Invaild url");
                }
            }).When(x => !string.IsNullOrWhiteSpace(x.Sitemaps));
        }
    }
}