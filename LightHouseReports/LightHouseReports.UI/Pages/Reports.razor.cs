using EventAggregator.Blazor;
using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Core.Interfaces;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.Data.Interfaces.Models;
using LightHouseReports.UI.Interfaces.Events;
using MudBlazor;

namespace LightHouseReports.UI.Pages;

public partial class Reports : IHandle<ReportUpdate>
{
    private bool _isLoading;
    private ViewModel _model = new();

    private readonly TableGroupDefinition<TableData> _groupDefinition = new()
    {
        Indentation = false,
        Expandable = true,
        Selector = (e) => e.ReportId
    };

    private string _preset = "All";

    protected override async Task OnInitializedAsync()
    {
        await ReloadData();
        await base.OnInitializedAsync();
    }

    private async Task ReloadData(string preset = "All")
    {
        _preset = preset;
        _isLoading = true;
        await InvokeAsync(StateHasChanged);
        var result = await Mediator.Request<GetUrlReportDataModels, Result<List<UrlReportDataModel>>>(new GetUrlReportDataModels());

        if (result.IsFailed)
        {
            //TODO
        }
        else
        {
            _model = new ViewModel(result.Value, preset);
            _isLoading = false;
        }
    }

    private async Task DeleteReport(Guid reportId)
    {
        try
        {
            await Mediator.Send(new DeleteReportAndCleanUpFiles(reportId));
        }
        catch (Exception e)
        {
        }
    }

    public async Task HandleAsync(ReportUpdate message)
    {
        await ReloadData(_preset);
    }

    private Color GetColorBasedOnValue(double value)
    {
        switch (value)
        {
            case >= 90:
                return Color.Success;
            case >= 50:
                return Color.Warning;
            case 0:
                return Color.Default;
            case < 50:
                return Color.Error;
        }

        return Color.Default;
    }

    public class ViewModel
    {
        public List<TableData> TableDatas { get; set; }

        public ViewModel()
        {
        }

        public ViewModel(List<UrlReportDataModel> dataModel, string preset)
        {
            TableDatas = dataModel.Select(x =>
            {
                var takeAll = !Enum.TryParse(preset, true, out Preset presetValue);
                var performance = (int)Math.Round(x.Results.Where(x => takeAll || x.Preset == presetValue).Average(x => x.Performance), 0, MidpointRounding.AwayFromZero);
                var accessibility = (int)Math.Round(x.Results.Where(x => takeAll || x.Preset == presetValue).Average(x => x.Accessibility), 0, MidpointRounding.AwayFromZero);
                var bestPractices = (int)Math.Round(x.Results.Where(x => takeAll || x.Preset == presetValue).Average(x => x.BestPractices), 0, MidpointRounding.AwayFromZero);
                var seo = (int)Math.Round(x.Results.Where(x => takeAll || x.Preset == presetValue).Average(x => x.Seo), 0, MidpointRounding.AwayFromZero);

                return new TableData(x.Report.TimeStamp,
                    x.Report.WebsiteDataModel.Id,
                    x.Report.WebsiteDataModel.Url,
                    x.Report.Id,
                    x.Id,
                    x.Adres,
                    performance,
                    accessibility,
                    bestPractices,
                    seo);
            }).ToList();
        }
    }

    public class TableData
    {
        public Guid WebsiteId { get; set; }
        public Guid UrlId { get; set; }
        public Guid ReportId { get; set; }
        public string Website { get; set; }
        public DateTimeOffset TimeStamp { get; set; }

        public string PageUrl { get; set; }

        public int Performance { get; set; }

        public int Accessibility { get; set; }

        public int BestPractices { get; set; }

        public int Seo { get; set; }

        public TableData(DateTimeOffset timeStamp, Guid websiteId, string website, Guid reportId, Guid urlId, string pageUrl, int performance, int accessibility, int bestPractices, int seo)
        {
            PageUrl = pageUrl;
            WebsiteId = websiteId;
            Performance = performance;
            Accessibility = accessibility;
            BestPractices = bestPractices;
            Seo = seo;
            ReportId = reportId;
            TimeStamp = timeStamp;
            Website = website;
            UrlId = urlId;
        }
    }

    private void OpenDetails(TableRowClickEventArgs<TableData> args)
    {
        NavigationManager.NavigateTo($"/report/detail/{args.Item.UrlId}");
    }
}