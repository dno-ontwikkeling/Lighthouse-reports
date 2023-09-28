using EventAggregator.Blazor;
using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Core.Interfaces;
using LightHouseReports.Core.Interfaces.Models;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.Data.Interfaces.Models;
using LightHouseReports.UI.Components;
using LightHouseReports.UI.Interfaces.Events;
using MudBlazor;

namespace LightHouseReports.UI.Pages;

public partial class Websites : IHandle<WebsitesUpdate>, IHandle<ReportProgressUpdate>
{
    private ViewModel _model = new();
    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        await ReloadData();

        await base.OnInitializedAsync();
    }

    private async Task ReloadData()
    {
        _isLoading = true;
        await InvokeAsync(StateHasChanged);
        var result = await Mediator.Request<GetWebsiteCoreModels, Result<List<WebsiteCoreModel>>>(new GetWebsiteCoreModels());
        if (result.IsFailed)
        {
            //TODO
        }
        else
        {
            _model = new ViewModel(result.Value);
            _isLoading = false;
        }
    }

    private async Task RunReport(Guid id)
    {
        try
        {
            await Mediator.Send(new RunLighthouseWebsiteReport(id));
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private void GoToAddWebsite()
    {
        try
        {
            var options = new DialogOptions { CloseOnEscapeKey = true };
            DialogService.Show<AddWebsite>("Add Website", options);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private async Task ArchiveWebsite(Guid id)
    {
        try
        {
            await Mediator.Send(new ArchiveWebsiteDataModel(id));
        }
        catch (Exception)
        {
            // ignored
        }
    }

    public async Task HandleAsync(WebsitesUpdate message)
    {
        await ReloadData();
    }

    public async Task HandleAsync(ReportProgressUpdate message)
    {
        await ReloadData();
    }

    private class ViewModel
    {
        public string SearchString { get; set; }
        public int CountOfWebsites { get; set; }
        public List<TableData> TableDatas { get; set; }

        public ViewModel()
        {
        }

        public ViewModel(List<WebsiteCoreModel> websiteModels)
        {
            CountOfWebsites = websiteModels.Count();
            TableDatas = websiteModels.Select(x => new TableData(x.Url, x.Id, x.LastRun, x.FoundUrls, x.ProgressReport)).ToList();
        }
    }

    private class TableData
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public int UrlsFound { get; set; }
        public DateTimeOffset? LastRun { get; set; }
        public ProgressCoreModel? ProgressReport { get; set; }

        public TableData(string url, Guid id, DateTimeOffset? lastRun, int urlsFound, ProgressCoreModel? progressReport)
        {
            Url = url;
            Id = id;
            LastRun = lastRun;
            UrlsFound = urlsFound;
            ProgressReport = progressReport;
        }
    }
}