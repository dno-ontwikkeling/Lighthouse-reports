using Microsoft.AspNetCore.Components;

namespace LightHouseReports.UI.Components;

public partial class DarkModeToggle
{
    [Parameter] public bool IsDarkMode { get; set; }

    /// <summary>
    /// Invoked when the dark mode changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsDarkModeChanged { get; set; }

    private void IsDarkModeChangedTrigger()
    {
        IsDarkModeChanged.InvokeAsync(IsDarkMode);
    }
}