using Microsoft.JSInterop;

namespace HaloStats.Web.Client.Infrastructure;

public static class JsInteropHelper
{
    public static ValueTask<string> ToLocalDateStringAsync(IJSRuntime js, DateTime utcDate)
    {
        // Pass the UTC date as an ISO string
        return js.InvokeAsync<string>("toLocalDateString", utcDate.ToString("o"));
    }
}
