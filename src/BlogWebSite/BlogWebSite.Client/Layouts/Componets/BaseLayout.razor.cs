using Microsoft.AspNetCore.Components;

namespace BlogWebSite.Client.Layouts.Componets
{
    public partial class BaseLayout : Masa.Blazor.Core.MasaComponentBase
    {
        [Parameter]
        public bool NavBarApp { get; set; }

        [Parameter]
        public bool FooterApp { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            MasaBlazor.Application.PropertyChanged += Application_PropertyChanged;
        }

        private void Application_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var propName = e.PropertyName;
            StateHasChanged();
        }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        string wasm = "not";
        string server = "not";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (IRenderMode.IsWasm())
            {
                wasm = "Is";
                await CheckTheme();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender) // 只在第一次渲染后调用
            {
                if (IRenderMode.IsWasm() is false)
                {
                    await CheckTheme();
                    StateHasChanged();
                }
            }
        }

        async Task CheckTheme()
        {
            var themeName = (await MasaLS.GetItemAsync("masablazor@theme"));

            MasaBlazor.SetTheme(themeName is "dark");
        }
    }
}
