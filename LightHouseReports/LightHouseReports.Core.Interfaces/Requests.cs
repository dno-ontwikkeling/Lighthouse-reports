using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Core.Interfaces.Models;

namespace LightHouseReports.Core.Interfaces;

public record RunLighthouseWebsiteReport(Guid WebsiteId) : IDataRequest<Result>;

public record GetWebsiteProgressCoreModel(Guid WebsiteId) : IDataRequest<Result<ProgressCoreModel>>;

public record GetWebsiteCoreModels : IDataRequest<Result<List<WebsiteCoreModel>>>;

public record GetSitemapCoreModel(string SitemapUrl) : IDataRequest<Result<SitemapCoreModel>>;

public record DeleteReportAndCleanUpFiles(Guid ReportIdGuid) : ICommandRequest;

public record DeleteLighthouseResult(Guid LighthouseResutlId) : ICommandRequest;