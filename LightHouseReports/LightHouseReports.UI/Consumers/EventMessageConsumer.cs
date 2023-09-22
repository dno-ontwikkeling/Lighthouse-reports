using EventAggregator.Blazor;
using LightHouseReports.Common.Mediator;
using LightHouseReports.UI.Interfaces.Events;

namespace LightHouseReports.UI.Consumers;

public class EventMessageConsumer : CommandRequestConsumer<EventMessageCommand>
{
    private readonly IEventAggregator _eventAggregator;

    public EventMessageConsumer(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }

    protected override async Task Consume(EventMessageCommand message, CancellationToken cancellationToken)
    {
        await _eventAggregator.PublishAsync(message.Event);
    }
}