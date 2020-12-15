using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace ApiRechargement.Web.Common
{
    public static class JsRuntimeExtensions
    {
        public static ValueTask<bool> Confirm(this IJSRuntime jsRuntime, string message)
        {
            return jsRuntime.InvokeAsync<bool>("confirm", message);
        }
    }
}
