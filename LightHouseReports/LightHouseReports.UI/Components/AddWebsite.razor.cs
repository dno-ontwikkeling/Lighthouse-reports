using FluentValidation;
using FluentValidation.Results;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.Data.Interfaces.Models;
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
                await Mediator.Send(new AddWebsiteModel(new Website(_input.Website, Guid.NewGuid())));
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
        var validator = new Validator();
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
        public Validator()
        {
            RuleFor(x => x.Website).NotEmpty();
            RuleFor(x => x.Website).CustomAsync(async (website, context, ct) =>
            {
                try
                {
                    var client = new HttpClient();
                    var test = Uri.CheckHostName(website);
                    var baseUri = new UriBuilder(website).Uri;

                    var siteMapUri = new Uri(baseUri!, "sitemap.xml");
                    var result = await client.GetAsync(siteMapUri);
                    if (!result.IsSuccessStatusCode) context.AddFailure("Can't access the sitemap");
                }
                catch (Exception e)
                {
                    context.AddFailure("Invaild url");
                }
            });
        }
    }
}