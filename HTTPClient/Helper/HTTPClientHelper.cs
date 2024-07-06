using HTTPClient.Model;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.Helper
{
    public class HTTPClientHelper
    {
        private static HttpClient httpClient;
        private static HttpRequestMessage httpRequestMessage;
        private static RestResponse restResponse;

        private static HttpClient AddHeadersAndCreateHttpClient(Dictionary<string, string> httpHeader)
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

        private static HttpRequestMessage CreateHttpRequestMessage(string requestUrl, HttpMethod method, HttpContent content)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, requestUrl);
            if (!(method == HttpMethod.Get) || (!(method == HttpMethod.Delete)))
            {
                httpRequestMessage.Content = content;
            }
            return httpRequestMessage;
        }

        private static RestResponse SendRequest(string requestUrl, HttpMethod method, HttpContent content, Dictionary<string, string> httpheader)
        {
            httpClient = AddHeadersAndCreateHttpClient(httpheader);
            httpRequestMessage = CreateHttpRequestMessage(requestUrl, method, content);

            try
            {
                Task<HttpResponseMessage> httpResponseMessage = httpClient.SendAsync(httpRequestMessage);
                restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                restResponse = new RestResponse(500, ex.Message);
            }
            finally
            {
                httpRequestMessage?.Dispose();
                httpClient?.Dispose();
            }
            return restResponse;
        }

        public static RestResponse PerformGetRequest(string requestUrl, Dictionary<string, string> httpHeader)
        {
            return SendRequest(requestUrl, HttpMethod.Get, null, httpHeader);
        }

        public static RestResponse PerformPostrequest(string requestUrl, HttpContent httpContent, Dictionary<string, string> httpHeader)
        {
            return SendRequest(requestUrl, HttpMethod.Post, httpContent, httpHeader);
        }

        public static RestResponse PerformPostrequest(string requestUrl, string data, string mediaType, Dictionary<string, string> httpHeader)
        {
            HttpContent content = new StringContent(data, Encoding.UTF8, mediaType);

            return PerformPostrequest(requestUrl, content, httpHeader);
        }

        public static RestResponse PerformPutRequest(string requestUrl, string content, string mediaType, Dictionary<string, string> httpHeaders)
        {
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, mediaType);
            return SendRequest(requestUrl, HttpMethod.Put, httpContent, httpHeaders);
        }

        public static RestResponse PerformPutRequest(string requestUrl, HttpContent httpContent, Dictionary<string, string> httpHeader)
        {
            return SendRequest(requestUrl, HttpMethod.Post, httpContent, httpHeader);
        }

        public static RestResponse PerformdeleteRequest(string deleteUrl)
        {
            return SendRequest(deleteUrl, HttpMethod.Delete, null, null);
        }

        public static RestResponse PerformdeleteRequest(string deleteUrl,Dictionary<string,string>httpHeaders)
        {
            return SendRequest(deleteUrl, HttpMethod.Delete, null, httpHeaders);
        }
    }
}
