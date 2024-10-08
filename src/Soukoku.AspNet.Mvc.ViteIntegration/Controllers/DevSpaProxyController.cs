﻿using System.Net.Http;
using System.Web.Mvc;

namespace Soukoku.AspNet.Mvc.ViteIntegration.Controllers
{
    /// <summary>
    /// Debug-time only controller for proxying requests to vite's dev server during development.
    /// This is only used during development.
    /// </summary>
    public class DevSpaProxyController : Controller
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
        public async Task<ActionResult> SpaDevServerProxy(
            string? vitePath = null,
            string? idPath = null,
            string? srcPath = null,
            string? nmPath = null,
            string? assetPath = null)
        {
            if (__proxyClient == null) return new HttpNotFoundResult();

            var url = vitePath != null ? $"@vite/{vitePath}" :
                idPath != null ? $"@id/{idPath}" :
                srcPath != null ? $"src/{srcPath}" :
                nmPath != null ? $"node_modules/{nmPath}" :
                assetPath != null ? $"assets/{assetPath}" : "";
            url += Request.Url.Query;

            var resp = await __proxyClient.GetAsync(url).ConfigureAwait(false);
            return new HttpResponseMessageResult(resp);
        }
    }

    class HttpResponseMessageResult : ActionResult
    {
        private readonly HttpResponseMessage _responseMessage;

        public HttpResponseMessageResult(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage; // could add throw if null
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();


            if (_responseMessage == null)
            {
                var message = "Response message cannot be null";

                throw new InvalidOperationException(message);
            }

            using (_responseMessage)
            {
                response.TrySkipIisCustomErrors = true;
                response.StatusCode = (int)_responseMessage.StatusCode;

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
                    response.Headers.Set(header.Key, header.Value.FirstOrDefault());
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
                        response.Headers.Set(header.Key, header.Value.FirstOrDefault());
                    }
                    if (_responseMessage.Content.Headers.ContentType != null)
                        response.ContentType = _responseMessage.Content.Headers.ContentType.ToString();
                    _responseMessage.Content.CopyToAsync(response.OutputStream).ConfigureAwait(false).GetAwaiter().GetResult();
                    response.End();
                }
            }
        }
    }
}