using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace NetcoreApiTemplate.Utilities
{
    public static class HttpExtensions
    {
        private static ActionContextAccessor _actionContextAccessor = new ActionContextAccessor();

        private static HttpRequest? Request => _actionContextAccessor?.ActionContext?.HttpContext.Request;
        public static string WebBaseUrl
        {
            get
            {
                var request = Request;
                var result = $"{request?.Scheme}://{request?.Host}{request?.PathBase}";
                return result;
            }
        }
        public static string? ClientIpAddress
        {
            get
            {
                var ip = "";
                if (!string.IsNullOrEmpty(Request?.Headers["X-Forwarded-For"]))
                {
                    ip = Request?.Headers["X-Forwarded-For"];
                }
                else
                {
                    ip = Request?.HttpContext?.Features?.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();
                }
                //ip = ip == "::1" ? "localhost" : ip;
                if (ip == "::1")
                    ip = ClientIpAddressLocalhost;
                return ip;
            }
        }
        public static string? ClientIpAddressLocalhost
        {
            get
            {
                IPAddress remoteIpAddress = Request?.HttpContext?.Connection?.RemoteIpAddress;
                string result = "";
                if (remoteIpAddress != null)
                {
                    // If we got an IPV6 address, then we need to ask the network for the IPV4 address 
                    // This usually only happens when the browser is on the same machine as the server.
                    if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        remoteIpAddress = System.Net.Dns.GetHostEntry(remoteIpAddress).AddressList
                .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                    }
                    result = remoteIpAddress.ToString();

                }
                return result;
            }
        }
        public static string AbsoluteContent(this IUrlHelper url, string contentPath)
        {
            var request = url.ActionContext.HttpContext.Request;
            return new Uri(new Uri(request.Scheme + "://" + request.Host.Value), url.Content(contentPath)).ToString();
        }
        public static string IsSelected(this IHtmlHelper htmlHelper, string controllers, string actions, string cssClass = "selected")
        {
            string? currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
            string? currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;

            IEnumerable<string> acceptedActions = (actions).Split(',');
            IEnumerable<string> acceptedControllers = (controllers).Split(',');
            //return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
            //    cssClass : String.Empty;

            var res = string.Empty;

            if (string.IsNullOrEmpty(actions))
            {
                res = acceptedControllers.Contains(currentController) ?
                cssClass : String.Empty;
            }
            else
            {
                res = acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
cssClass : String.Empty;
            }


            return res;
        }
    }
}
