using EventAggregator.Blazor;
using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.Data.Interfaces.Models;
using LightHouseReports.UI.Components;
using LightHouseReports.UI.Interfaces.Events;
using MudBlazor;

namespace LightHouseReports.UI.Pages;

public partial class Websites : IHandle<WebsitesUpdate>
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
        var result = await Mediator.Request<GetWebsiteModels, Result<List<Website>>>(new GetWebsiteModels());
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

    private void RunReports()
    {
        throw new NotImplementedException();
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

    private void DeleteWebsites()
    {
        try
        {
            foreach (var modelSelectedTableData in _model.SelectedTableDatas) Mediator.Send(new RemoveWebsiteModel(modelSelectedTableData.Id));
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

    private class ViewModel
    {
        public HashSet<TableData> SelectedTableDatas { get; set; } = new();
        public string SearchString { get; set; }
        public int CountOfWebsites { get; set; }
        public List<TableData> TableDatas { get; set; }

        public ViewModel()
        {
        }

        public ViewModel(List<Website> websiteModels)
        {
            CountOfWebsites = websiteModels.Count();
            TableDatas = websiteModels.Select(x => new TableData(x.Url, x.Id, x.LastRun)).ToList();
        }
    }

    private class TableData
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public DateTimeOffset? LastRun { get; set; }

        public TableData(string url, Guid id, DateTimeOffset? lastRun)
        {
            Url = url;
            Id = id;
            LastRun = lastRun;
        }
    }
}