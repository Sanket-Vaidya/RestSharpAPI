using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpFramework_Final.Helper.Request
{
    public class RequestClientHelper
    {
        private IRestClient GetRestClient()
        {
            IRestClient client = new RestClient();
            return client;
        }

        private IRestRequest CreateRestRequest(string requestUrl, Dictionary<string, string> headers, Method method, object body, DataFormat format)
        {
            IRestRequest restRequest = new RestRequest()
            {
                Resource = requestUrl,
                Method = method,
            };

            foreach (string key in headers.Keys)
            {
                restRequest.AddHeader(key, headers[key]);
            }

            if (body != null)
            {
                restRequest.RequestFormat = format;
                switch (format)
                {
                    case DataFormat.Json: 
                        restRequest.AddBody(body);
                        break;

                        case DataFormat.Xml:
                        restRequest.XmlSerializer=new RestSharp.Serializers.DotNetXmlSerializer();
                        restRequest.AddParameter("xmlData",body.GetType().Equals(typeof(string))?body:restRequest.XmlSerializer.Serialize(body),ParameterType.RequestBody);
                        break;
                }
                
            }
            return restRequest;
        }

        private IRestResponse SendRequest(IRestRequest restRequest)
        {
            IRestClient restClient = GetRestClient();
            IRestResponse restResponse = restClient.Execute(restRequest);
            return restResponse;
        }

        private IRestResponse<T> SendRequest<T>(IRestRequest restRequest) where T : new()
        {
            IRestClient restClient = GetRestClient();
            IRestResponse<T> restResponse = restClient.Execute<T>(restRequest);
            if (restResponse.ContentType.Equals("application/xml"))
            {
                var deserializer = new RestSharp.Deserializers.DotNetXmlDeserializer();
                restResponse.Data = deserializer.Deserialize<T>(restResponse);
            }
            return restResponse;
        }

        public IRestResponse PerformGetRequest(string url, Dictionary<string, string> headers)
        {
            IRestRequest request = CreateRestRequest(url, headers, Method.GET, null, DataFormat.None);
            IRestResponse response = SendRequest(request);
            return response;
        }

        public IRestResponse<T> PerformGetRequest<T>(string url, Dictionary<string, string> headers) where T : new()
        {
            IRestRequest request = CreateRestRequest(url, headers, Method.GET, null, DataFormat.None);
            IRestResponse<T> response = SendRequest<T>(request);
            return response;
        }

        public IRestResponse PerformPostRequest(string url, Dictionary<string, string> header, object body, DataFormat dataFormat)
        {
            IRestRequest restRequest = CreateRestRequest(url, header, Method.POST, body, dataFormat);
            IRestResponse restResponse = SendRequest(restRequest);
            return restResponse;
        }

        public IRestResponse<T> PerformPostRequest<T>(string url, Dictionary<string, string> header, object body, DataFormat dataFormat) where T : new()
        {
            IRestRequest restRequest = CreateRestRequest(url, header, Method.POST, body, dataFormat);
            IRestResponse<T> restResponse = SendRequest<T>(restRequest);
            return restResponse;
        }

        public IRestResponse PerformPutRequest(string url, Dictionary<string, string> header, object body, DataFormat dataFormat)
        {
            IRestRequest restRequest = CreateRestRequest(url, header, Method.PUT, body, dataFormat);
            IRestResponse restResponse = SendRequest(restRequest);
            return restResponse;
        }

        public IRestResponse<T> PerformPutRequest<T>(string url, Dictionary<string, string> header, object body, DataFormat dataFormat) where T : new()
        {
            IRestRequest restRequest = CreateRestRequest(url, header, Method.PUT, body, dataFormat);
            IRestResponse<T> restResponse = SendRequest<T>(restRequest);
            return restResponse;
        }

        public IRestResponse PerformDeleteRequest(string url, Dictionary<string, string> header)
        {
            IRestRequest request = CreateRestRequest(url, header, Method.DELETE, null, DataFormat.None);
            IRestResponse response = SendRequest(request);
            return response;
        }

        public IRestResponse<T> PerformDeleteRequest<T>(string url, Dictionary<string, string> header) where T : new()
        {
            IRestRequest request = CreateRestRequest(url, header, Method.DELETE, null, DataFormat.None);
            IRestResponse<T> response = SendRequest<T>(request);
            return response;
        }
    }
}
