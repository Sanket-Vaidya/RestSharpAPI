using HTTPClient.Model;
using HTTPClient.Model.JSONModel;
using HTTPClient.Model.XMLModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HTTPClient.GETEndPoint
{
    [TestClass]
    public class TestGetEndPoint
    {
        private string GetUrl = "http://localhost:8080/laptop-bag/webapi/api/all";


        [TestMethod]
        public void TestGetAllEndPoint()
        {
            HttpClient client = new HttpClient();
            client.GetAsync(GetUrl);
            client.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndPointWithURI()
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri(GetUrl);
            client.GetAsync(uri);
            client.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndPointResponse()
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri(GetUrl);
            Task<HttpResponseMessage> httpResponseMessage = client.GetAsync(uri);
            HttpResponseMessage response = httpResponseMessage.Result;
            Console.WriteLine(response.ToString());
            HttpStatusCode statusCode = response.StatusCode;
            Console.WriteLine("Status Code=> " + statusCode);
            Console.WriteLine("Status Code=> " + (int)statusCode);
            client.Dispose();
        }

        [TestMethod]
        public void TestGetAllEndPointResponseContent()
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri(GetUrl);
            Task<HttpResponseMessage> httpResponseMessage = client.GetAsync(uri);
            HttpResponseMessage response = httpResponseMessage.Result;
            HttpContent responseContent = response.Content;
            Task<string> content = responseContent.ReadAsStringAsync();
            Console.WriteLine(content.Result);
            client.Dispose();
        }
        [TestMethod]
        public void TestGetAllEndPointInJSONFormat()
        {
            MediaTypeWithQualityHeaderValue jsonHeader = new MediaTypeWithQualityHeaderValue("application/json");
            HttpClient client = new HttpClient();
            Uri uri = new Uri(GetUrl);
            HttpRequestHeaders headers = client.DefaultRequestHeaders;
            // headers.Add("accept","application/xml");
            headers.Accept.Add(jsonHeader);
            Task<HttpResponseMessage> httpResponseMessage = client.GetAsync(uri);
            HttpResponseMessage response = httpResponseMessage.Result;
            HttpContent responseContent = response.Content;
            Task<string> content = responseContent.ReadAsStringAsync();
            Console.WriteLine(content.Result);
            client.Dispose();
        }

        [TestMethod]
        public void TestSendAsync()
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.RequestUri = new Uri(GetUrl);
            httpRequestMessage.Method = HttpMethod.Get;
            httpRequestMessage.Headers.Add("Accept", "application/json");

            HttpClient httpClient = new HttpClient();
            Task<HttpResponseMessage> httpResponse = httpClient.SendAsync(httpRequestMessage);
            HttpResponseMessage httpResponseMessage = httpResponse.Result;
            HttpContent content = httpResponseMessage.Content;
            Task<string> data = content.ReadAsStringAsync();
            string responseData = data.Result;
            HttpStatusCode statusCode = httpResponseMessage.StatusCode;
            int statusCodeVal = (int)statusCode;
            Console.WriteLine("Staus Code=> " + statusCodeVal);
            Console.WriteLine(responseData);
        }

        [TestMethod]
        public void TestUsing()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(GetUrl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    Task<HttpResponseMessage> httpResponse = client.SendAsync(httpRequestMessage);
                    HttpResponseMessage httpResponseMessage = httpResponse.Result;
                    HttpContent content = httpResponseMessage.Content;
                    Task<string> contentData = content.ReadAsStringAsync();
                    string data = contentData.Result;
                    HttpStatusCode statusCode = httpResponseMessage.StatusCode;
                    int statusCodeVal = (int)statusCode;

                    Console.WriteLine("Status Code=> " + statusCodeVal);
                    Console.WriteLine(data);

                }
            }
        }

        [TestMethod]
        public void TestWithCustomClass()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(GetUrl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Add("Accept", "application/xml");

                    Task<HttpResponseMessage> httpResponse = client.SendAsync(httpRequestMessage);
                    HttpResponseMessage httpResponseMessage = httpResponse.Result;
                    HttpContent content = httpResponseMessage.Content;
                    Task<string> contentData = content.ReadAsStringAsync();
                    string data = contentData.Result;
                    HttpStatusCode statusCode = httpResponseMessage.StatusCode;
                    int statusCodeVal = (int)statusCode;

                    RestResponse restResponse = new RestResponse(statusCodeVal, data);
                    Console.WriteLine(restResponse.ToString());

                }
            }
        }

        [TestMethod]
        public void TestWithJSONDeserialize()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(GetUrl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Add("Accept", "application/json");

                    Task<HttpResponseMessage> httpResponse = client.SendAsync(httpRequestMessage);
                    HttpResponseMessage httpResponseMessage = httpResponse.Result;
                    HttpContent content = httpResponseMessage.Content;
                    Task<string> contentData = content.ReadAsStringAsync();
                    string data = contentData.Result;
                    HttpStatusCode statusCode = httpResponseMessage.StatusCode;
                    int statusCodeVal = (int)statusCode;

                    RestResponse restResponse = new RestResponse(statusCodeVal, data);
                    //Console.WriteLine(restResponse.ToString());

                    List<JSONRootObject> jsonRootObj = JsonConvert.DeserializeObject<List<JSONRootObject>>(restResponse.ResponseData);
                    Console.WriteLine(jsonRootObj[0].ToString());
                }
            }
        }
        [TestMethod]
        public void TestWithXMLDeserialize()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(GetUrl);
                    httpRequestMessage.Method = HttpMethod.Get;
                    httpRequestMessage.Headers.Add("Accept", "application/xml");

                    Task<HttpResponseMessage> httpResponse = client.SendAsync(httpRequestMessage);
                    HttpResponseMessage httpResponseMessage = httpResponse.Result;
                    HttpContent content = httpResponseMessage.Content;
                    Task<string> contentData = content.ReadAsStringAsync();
                    string data = contentData.Result;
                    HttpStatusCode statusCode = httpResponseMessage.StatusCode;
                    int statusCodeVal = (int)statusCode;

                    RestResponse restResponse = new RestResponse(statusCodeVal, data);
                    //Console.WriteLine(restResponse.ToString());

                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(LaptopDetails));

                    TextReader reader = new StringReader(restResponse.ResponseData);

                    LaptopDetails xmlData = (LaptopDetails)xmlSerializer.Deserialize(reader);

                    Console.WriteLine(xmlData.ToString());
                    Assert.AreEqual(200, restResponse.StatusCode);

                    Assert.IsNotNull(restResponse.ResponseData);

                    Assert.IsTrue(xmlData.Laptop.Features.Feature.Contains("8th Generation Intel® Core™ i5-8300H"), "Item not found");

                    Assert.AreEqual("Alienware", xmlData.Laptop.BrandName);
                }
            }
        }

    }
}
