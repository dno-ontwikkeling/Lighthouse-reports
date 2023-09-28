using FluentResults;
using LightHouseReports.Common.Mediator;
using LightHouseReports.Core.Interfaces;
using LightHouseReports.Core.Interfaces.Models;
using LightHouseReports.Data.Interfaces;
using LightHouseReports.Data.Interfaces.Models;
using MassTransit.Mediator;

namespace LightHouseReports.Core.Consumers;

public class GetWebsiteCoreModelsConsumer : DataRequestConsumer<GetWebsiteCoreModels, Result<List<WebsiteCoreModel>>>
{
    private readonly IMediator _mediator;

    public GetWebsiteCoreModelsConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected override async Task<Result<List<WebsiteCoreModel>>> Consume(GetWebsiteCoreModels message, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Request<GetWebsiteDataModels, Result<List<WebsiteDataModel>>>(new GetWebsiteDataModels(), cancellationToken);
            if (result.IsFailed) return result.ToResult();

            var websitesData = result.Value;
            var coreModels = new List<WebsiteCoreModel>();

            foreach (var dataModel in websitesData)
            {
                var progress = await _mediator.Request<GetWebsiteProgressCoreModel, Result<ProgressCoreModel>>(new GetWebsiteProgressCoreModel(dataModel.Id), cancellationToken);
                coreModels.Add(progress.IsSuccess ? new WebsiteCoreModel(dataModel, progress.Value) : new WebsiteCoreModel(dataModel, null));
            }

            return Result.Ok(coreModels);
        }
        catch (Exception e)
        {
            return Result.Fail(e.GetBaseException().Message);
        }
    }
}