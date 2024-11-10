using Masa.Blazor.Extensions;
using Masa.Blazor.JSInterop;

namespace BlogWebSite.Client.Extensions.Masa
{
    internal static class MyJsRuntimeExtensions
    {
        public const string Window = "window";

        internal static Task AddScrollEventListener(this IJSRuntime js, string? selector, Func<Task> callback, EventListenerExtras? extras = null) =>
            js.AddHtmlElementEventListener(selector ?? Window, "scroll", callback, false, extras ?? new EventListenerExtras(0, 100));

        internal static Task RemoveScrollEventListener(this IJSRuntime js, string? selector) => js.RemoveHtmlElementEventListener(selector ?? Window, "scroll");

        public const string Document = "document";

        internal static ValueTask ScrollToTopAsync(this IJSRuntime js, string? selector, StringNumber px, ScrollBehavior? behavior = ScrollBehavior.Smooth) =>
            js.InvokeVoidAsync(JsInteropConstants.ScrollTo, selector ?? Document, new { Top = px.ToUnit(), Behavior = behavior.ToString()!.ToLower(), });

        internal static async Task<double> GetSelectorPageYOffset(this IJSRuntime js, string? selector)
        {
            selector ??= Window;

            if (selector is Window)
            {
                return (await js.GetWindowPageYOffsetAsync()) ?? 0;
            }
            else
            {
                return await js.InvokeAsync<double>(JsInteropConstants.GetProp, [selector, "scrollTop"]);
            }
        }
    }
}
