using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace ApiRechargement.Web.Pages.Base
{
    public abstract class BasePage : ComponentBase
    {
        [Inject]
        protected IJSRuntime _jsRuntime { get; set; }

        [Inject] protected ILogger<BasePage> Logger { get; set; }

        public bool IsLoading { get; set; } = true;
    }
}
