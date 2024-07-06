using HTTPClient.Model;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.Helper
{
    public class HTTPClientAsyncHelper
    {
        private HttpClient httpClient;

        private HttpClient AddHeadersAndCreateHttpClient(Dictionary<string, string> httpHeader)
        {
            HttpClient httpClient = new HttpClient();
            if (null != httpHeader)
            {
                foreach (string key in httpHeader.Keys)
                {
                    httpClient.DefaultRequestHeaders.Add(key, httpHeader[key]);
                }
            }

            return httpClient;
        }

        private HttpRequestMessage CreateHttpRequestMessage(string requestUrl, HttpMethod method, HttpContent content)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, requestUrl);
            if (!(method == HttpMethod.Get) || (!(method == HttpMethod.Delete)))
            {
                httpRequestMessage.Content = content;
            }
            return httpRequestMessage;
        }

        public async Task<RestResponse> PerformGetRequest(string requestUrl, Dictionary<string, string> httpHeader)
        {
            httpClient = AddHeadersAndCreateHttpClient(httpHeader);
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUrl);
            return new RestResponse((int)httpResponseMessage.StatusCode, httpResponseMessage.Content.ReadAsStringAsync().Result);
        }

        public async Task<RestResponse> PerformPostrequest(string requestUrl, HttpContent httpContent, Dictionary<string, string> httpHeader)
        {
            httpClient = AddHeadersAndCreateHttpClient(httpHeader);
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(requestUrl, httpContent);
            return new RestResponse((int)httpResponseMessage.StatusCode, httpResponseMessage.Content.ReadAsStringAsync().Result);
        }

        public async Task<RestResponse> PerformPostrequest(string requestUrl, string data, string mediaType, Dictionary<string, string> httpHeader)
        {
            HttpContent content = new StringContent(data, Encoding.UTF8, mediaType);

            httpClient = AddHeadersAndCreateHttpClient(httpHeader);
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(requestUrl, content);
            return new RestResponse((int)httpResponseMessage.StatusCode, httpResponseMessage.Content.ReadAsStringAsync().Result);
        }

        public async Task<RestResponse> PerformPutRequest(string requestUrl, string content, string mediaType, Dictionary<string, string> httpHeader)
        {
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, mediaType);
            httpClient = AddHeadersAndCreateHttpClient(httpHeader);
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync(requestUrl, httpContent);
            return new RestResponse((int)httpResponseMessage.StatusCode, httpResponseMessage.Content.ReadAsStringAsync().Result);
        }

        public async Task<RestResponse> PerformPutRequest(string requestUrl, HttpContent httpContent, Dictionary<string, string> httpHeader)
        {
            httpClient = AddHeadersAndCreateHttpClient(httpHeader);
            HttpResponseMessage httpResponseMessage = await httpClient.PutAsync(requestUrl, httpContent);
            return new RestResponse((int)httpResponseMessage.StatusCode, httpResponseMessage.Content.ReadAsStringAsync().Result);
        }

        public async Task<RestResponse> PerformdeleteRequest(string deleteUrl)
        {
            httpClient = AddHeadersAndCreateHttpClient(null);
            HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync(deleteUrl);
            return new RestResponse((int)httpResponseMessage.StatusCode, httpResponseMessage.Content.ReadAsStringAsync().Result);
        }

        public async Task<RestResponse> PerformdeleteRequest(string deleteUrl, Dictionary<string, string> httpHeaders)
        {
            httpClient = AddHeadersAndCreateHttpClient(httpHeaders);
            HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync(deleteUrl);
            return new RestResponse((int)httpResponseMessage.StatusCode, httpResponseMessage.Content.ReadAsStringAsync().Result);
        }
    }
}
