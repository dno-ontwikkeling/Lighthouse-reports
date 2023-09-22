using MassTransit;

namespace LightHouseReports.Common.Mediator;

public abstract class DataRequestConsumer<TMessage, TResponse> : IConsumer<TMessage> where TMessage : class, IDataRequest<TResponse>
{
    public async Task Consume(ConsumeContext<TMessage> context)
    {
        var result = await Consume(context.Message, context.CancellationToken);
        await context.RespondAsync(result!);
    }

    protected abstract Task<TResponse> Consume(TMessage message, CancellationToken cancellationToken);
}

public abstract class CommandRequestConsumer<TMessage> : IConsumer<TMessage> where TMessage : class, ICommandRequest
{
    public async Task Consume(ConsumeContext<TMessage> context)
    {
        await Consume(context.Message, context.CancellationToken);
    }

    protected abstract Task Consume(TMessage message, CancellationToken cancellationToken);
}