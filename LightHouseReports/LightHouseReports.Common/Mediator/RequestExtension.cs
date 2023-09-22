using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Mediator;

namespace LightHouseReports.Common.Mediator;

public static class RequestExtension
{
    public static async Task<TResponse> Request<TRequest, TResponse>(this IMediator mediator, TRequest request, CancellationToken cancellation = default,
        RequestTimeout timeout = default) where TRequest : class, IDataRequest<TResponse> where TResponse : class
    {
        var client = mediator.CreateRequestClient<TRequest>();
        var response = await client.GetResponse<TResponse>(request, cancellation);
        var result = response.Message;
        return result ?? throw new NullReferenceException();
    }
}