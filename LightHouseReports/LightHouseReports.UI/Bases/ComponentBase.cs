using EventAggregator.Blazor;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using LightHouseReports.UI.Resources;
using MudBlazor;

namespace LightHouseReports.UI.Bases;

public class ComponentBase : Microsoft.AspNetCore.Components.ComponentBase, IDisposable
{
    [Inject] public IStringLocalizer<App> Localizer { get; set; } = null!;
    [Inject] public IEventAggregator EventAggregator { get; set; } = null!;
    [Inject] public IMediator Mediator { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);
        if (firstRender) EventAggregator.Subscribe(this);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        EventAggregator.Unsubscribe(this);
    }
}