using LightHouseReports.Common.Events;
using LightHouseReports.Common.Mediator;

namespace LightHouseReports.UI.Interfaces.Events;

public record EventMessageCommand(IEventMessage Event) : ICommandRequest;