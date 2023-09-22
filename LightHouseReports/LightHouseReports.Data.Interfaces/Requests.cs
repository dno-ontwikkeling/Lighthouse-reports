using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Data.Interfaces.Models;

namespace LightHouseReports.Data.Interfaces;

public record GetWebsiteModels : IDataRequest<Result<List<Website>>>;

public record GetWebsiteModel(Guid Id) : IDataRequest<Result<Website>>;

public record AddWebsiteModel(Website Website) : ICommandRequest;

public record RemoveWebsiteModel(Guid Id) : ICommandRequest;

public record UpdateWebsiteModel(Website Website) : ICommandRequest;