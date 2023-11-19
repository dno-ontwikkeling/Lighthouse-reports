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
        Selector = e => e.ReportId,
        IsInitiallyExpanded = false
    };

    private string _preset = "All";

    protected override async Task OnInitializedAsync()
    {
        await ReloadData();
        await base.OnInitializedAsync();
    }

    private async Task ReloadData(string preset = "All")
    {
        try
        {
            _preset = preset;
            _isLoading = true;
            await InvokeAsync(StateHasChanged);
            var result = await Mediator.Request<GetUrlReportDataModels, Result<List<UrlReportDataModel>>>(new GetUrlReportDataModels());

            if (result.IsSuccess)
            {
                _model = new ViewModel(result.Value, preset);
                _isLoading = false;
            }
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private async Task DeleteReport(Guid reportId)
    {
        try
        {
            await Mediator.Send(new DeleteReportAndCleanUpFiles(reportId));
        }
        catch (Exception)
        {
            // ignored
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

    private static int Round(UrlReportDataModel data, bool takeAll, Preset presetValue, string valueKey)
    {
        var selection = data.Results.Where(x => takeAll || (x.Preset == presetValue && (int)(x.GetType().GetProperty(valueKey)?.GetValue(x) ?? 0) != 0)).ToList();
        if (selection.Any()) return (int)Math.Round(selection.Average(x => (int)(x.GetType().GetProperty(valueKey)?.GetValue(x) ?? 0)), 0, MidpointRounding.AwayFromZero);

        return 0;
    }

    private static int Round(IEnumerable<TableData> data, string valueKey)
    {
        var selection = data.Where(x => (int)(x.GetType().GetProperty(valueKey)?.GetValue(x) ?? 0) != 0).ToList();
        if (selection.Any()) return (int)Math.Round(selection.Average(x => (int)(x.GetType().GetProperty(valueKey)?.GetValue(x) ?? 0)), 0, MidpointRounding.AwayFromZero);

        return 0;
    }

    public class ViewModel
    {
        public List<TableData>? TableData { get; }

        public ViewModel()
        {
        }

        public ViewModel(IEnumerable<UrlReportDataModel> dataModel, string preset)
        {
            TableData = dataModel.Select(x =>
            {
                var takeAll = !Enum.TryParse(preset, true, out Preset presetValue);
                var performance = Round(x, takeAll, presetValue, "Performance");
                var accessibility = Round(x, takeAll, presetValue, "Accessibility");
                var bestPractices = Round(x, takeAll, presetValue, "BestPractices");
                var seo = Round(x, takeAll, presetValue, "Seo");

                return new TableData(x.Report.TimeStamp,
                    x.Report.WebsiteDataModel.Id,
                    x.Report.WebsiteDataModel.Name,
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