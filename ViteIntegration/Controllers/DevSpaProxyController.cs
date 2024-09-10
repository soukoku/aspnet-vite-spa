using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace WebApp.Controllers
{
    /// <summary>
    /// Debug-time only controller for proxying requests to vite's dev server during development.
    /// This is only used during development.
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DevSpaProxyController : ControllerBase
    {
        internal static void SetDevTimeUrl(string? devTimeUrl)
        {
            if (string.IsNullOrEmpty(devTimeUrl))
            {
                __proxyClient?.Dispose();
                __proxyClient = null;
            }
            else
            {
                __proxyClient = new HttpClient { BaseAddress = new Uri(devTimeUrl) };
            }
        }


        // port is set in vite.config.ts
        private static HttpClient? __proxyClient;

        /// <summary>
        /// Proxies requests to vite's dev server during dev time.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("@vite/{*vitePath}")]
        [Route("@id/{*idPath}")]
        [Route("src/{*srcPath}")]
        [Route("node_modules/{*nmPath}")]
        [Route("assets/{*assetPath}")]
        public async Task<IActionResult> SpaDevServerProxy(
            [FromServices] IWebHostEnvironment environment,
            string? vitePath = null,
            string? idPath = null,
            string? srcPath = null,
            string? nmPath = null,
            string? assetPath = null)
        {
            if (__proxyClient == null || !environment.IsDevelopment()) return NotFound();

            var url = vitePath != null ? $"@vite/{vitePath}" :
                idPath != null ? $"@id/{idPath}" :
                srcPath != null ? $"src/{srcPath}" :
                nmPath != null ? $"node_modules/{nmPath}" :
                assetPath != null ? $"assets/{assetPath}" : "";

            return new HttpResponseMessageResult(await __proxyClient.GetAsync(url + Request.QueryString));
        }
    }

    class HttpResponseMessageResult : IActionResult
    {
        private readonly HttpResponseMessage _responseMessage;

        public HttpResponseMessageResult(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage; // could add throw if null
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;


            if (_responseMessage == null)
            {
                var message = "Response message cannot be null";

                throw new InvalidOperationException(message);
            }

            using (_responseMessage)
            {
                response.StatusCode = (int)_responseMessage.StatusCode;

                var responseFeature = context.HttpContext.Features.Get<IHttpResponseFeature>();
                if (responseFeature != null)
                {
                    responseFeature.ReasonPhrase = _responseMessage.ReasonPhrase;
                }

                var responseHeaders = _responseMessage.Headers;

                // Ignore the Transfer-Encoding header if it is just "chunked".
                // We let the host decide about whether the response should be chunked or not.
                if (responseHeaders.TransferEncodingChunked == true &&
                    responseHeaders.TransferEncoding.Count == 1)
                {
                    responseHeaders.TransferEncoding.Clear();
                }

                foreach (var header in responseHeaders)
                {
                    response.Headers.Append(header.Key, header.Value.ToArray());
                }

                if (_responseMessage.Content != null)
                {
                    var contentHeaders = _responseMessage.Content.Headers;

                    // Copy the response content headers only after ensuring they are complete.
                    // We ask for Content-Length first because HttpContent lazily computes this
                    // and only afterwards writes the value into the content headers.
                    var unused = contentHeaders.ContentLength;

                    foreach (var header in contentHeaders)
                    {
                        response.Headers.Append(header.Key, header.Value.ToArray());
                    }

                    await _responseMessage.Content.CopyToAsync(response.Body);
                }
            }
        }
    }
}