using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces.Models;

namespace LightHouseReports.Data.Interfaces;

public record GetWebsiteDataModels : IDataRequest<Result<List<WebsiteDataModel>>>;

public record GetWebsiteDataModel(Guid Id) : IDataRequest<Result<WebsiteDataModel>>;

public record AddWebsiteDataModel(WebsiteDataModel WebsiteDataModel) : ICommandRequest;

public record ArchiveWebsiteDataModel(Guid Id) : ICommandRequest;

public record UpdateWebsiteDataModel(WebsiteDataModel WebsiteDataModel) : ICommandRequest;

public record GetUrlReportDataModels : IDataRequest<Result<List<UrlReportDataModel>>>;

public record GetUrlReportDataModelsForReport(Guid ReportId) : IDataRequest<List<UrlReportDataModel>>;

public record GetUrlReportDataModel(Guid Id) : IDataRequest<Result<UrlReportDataModel>>;

public record AddUrlReportDataModel(UrlReportDataModel ReportDataModel) : ICommandRequest;

public record DeleteUrlReportDataModel(Guid Id) : ICommandRequest;

public record DeleteReportDataModel(Guid Id) : ICommandRequest;