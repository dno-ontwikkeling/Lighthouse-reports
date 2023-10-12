using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.Data.Interfaces.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LightHouseReports.UI.Pages;

public partial class ReportDetails
{
    [Parameter] public Guid Id { get; set; }
    private ViewModel _model = new();
    private bool _isLoading;

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;
        await InvokeAsync(StateHasChanged);
        var result = await Mediator.Request<GetUrlReportDataModel, Result<UrlReportDataModel>>(new GetUrlReportDataModel(Id));
        if (result.IsSuccess) _model = new ViewModel(result.Value);
        _isLoading = false;
        await InvokeAsync(StateHasChanged);
        await base.OnInitializedAsync();
    }

    private string OpenReport(Preset preset)
    {
        var path = $"\\Reports\\{_model.ReportId}\\{_model.UrlId}\\{preset.ToString().ToLowerInvariant()}.report.html";
        return path;
    }

    private Task DeleteReport(Guid contextResultId)
    {
        throw new NotImplementedException();
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
        public Guid WebsiteId { get; set; }
        public Guid UrlId { get; set; }
        public Guid ReportId { get; set; }
        public string Website { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string PageUrl { get; set; }

        public List<TableData> TableDatas { get; set; }

        public ViewModel()
        {
        }

        public ViewModel(UrlReportDataModel dataModel)
        {
            WebsiteId = dataModel.Report.WebsiteDataModel.Id;
            Website = dataModel.Report.WebsiteDataModel.WebisteUrl;
            UrlId = dataModel.Id;
            ReportId = dataModel.Report.Id;
            TimeStamp = dataModel.Report.TimeStamp;
            PageUrl = dataModel.Adres;
            TableDatas = dataModel.Results.Select(x => new TableData(x.Id, x.Preset, x.Performance, x.Accessibility, x.BestPractices, x.Seo, $"\\Reports\\{ReportId}\\{UrlId}\\{x.Preset.ToString().ToLowerInvariant()}.report.html")).ToList();
        }
    }

    public class TableData
    {
        public TableData(Guid resultId, Preset preset, int performance, int accessibility, int bestPractices, int seo, string href)
        {
            Preset = preset;
            Performance = performance;
            Accessibility = accessibility;
            BestPractices = bestPractices;
            Seo = seo;
            Href = href;
            ResultId = resultId;
        }

        public Guid ResultId { get; set; }
        public Preset Preset { get; set; }
        public int Performance { get; set; }
        public int Accessibility { get; set; }
        public int BestPractices { get; set; }
        public int Seo { get; set; }
        public string Href { get; set; }
    }
}