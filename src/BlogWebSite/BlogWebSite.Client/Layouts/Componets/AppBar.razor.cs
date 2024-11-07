using Microsoft.AspNetCore.Components;

namespace BlogWebSite.Client.Layouts.Componets
{
    public partial class AppBar
    {
        [Parameter]
        public bool App { get; set; }

        bool? open = false;
    }
}
