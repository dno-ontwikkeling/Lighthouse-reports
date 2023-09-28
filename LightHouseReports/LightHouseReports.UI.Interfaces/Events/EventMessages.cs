using LightHouseReports.Common.Events;

namespace LightHouseReports.UI.Interfaces.Events;

public record WebsitesUpdate : IEventMessage;

public record ReportUpdate : IEventMessage;

public record ReportProgressUpdate() : IEventMessage;