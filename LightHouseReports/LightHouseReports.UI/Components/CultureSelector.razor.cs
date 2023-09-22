using LightHouseReports.UI.Options;

namespace LightHouseReports.UI.Components;

public partial class CultureSelector
{
    private SupportedCulture _selectedCulture = null!;
    private List<SupportedCulture> _cultures = new();


    protected override Task OnInitializedAsync()
    {
        var cultureInfo = Thread.CurrentThread.CurrentCulture;
        _cultures = SupportedCulturesOptions.Value.SupportedCultures;
        var clientCulture = _cultures.FirstOrDefault(x => x.Culture == cultureInfo.Name);
        if (clientCulture is null)
        {
            _selectedCulture = SupportedCulturesOptions.Value.SupportedCultures.First();
            RequestCultureChange();
        }
        else
        {
            _selectedCulture = clientCulture;
        }

        InvokeAsync(StateHasChanged);
        return base.OnInitializedAsync();
    }

    private void RequestCultureChange()
    {
        var uri = new Uri(NavigationManager.Uri).GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
        var query = $"?culture={Uri.EscapeDataString(_selectedCulture.Culture)}&redirectUri={Uri.EscapeDataString(uri)}";
        NavigationManager.NavigateTo("/Culture/SetCulture" + query, true);
    }

    private string SupportedCultureToString(SupportedCulture? supportedCulture)
    {
        return supportedCulture is null ? string.Empty : supportedCulture.Display;
    }
}