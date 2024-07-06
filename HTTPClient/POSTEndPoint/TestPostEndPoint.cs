using HTTPClient.Model;
using HTTPClient.Model.JSONModel;
using HTTPClient.Model.XMLModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HTTPClient.POSTEndPoint
{
    [TestClass]
    public class TestPostEndPoint
    {
        public string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";

        public string getURL = "http://localhost:8080/laptop-bag/webapi/api/find/";

        private RestResponse restResponse;
        private RestResponse xmlRestResponse;
        private RestResponse getRestResponse;

        private Random random = new Random();

        private string jsonMediaType = "application/json";
        private string xmlMediaType = "application/xml";
        [TestMethod]
        public void TestPostEndPointWithJson()
        {
            int id = random.Next(1000);
            string jsonData =
    "{" +
      "\"BrandName\" :\"Alienware\"," +
      "\"Features\":{" +
        "\"Feature\" : [" +
          "\"8th Generation Intel® Core™ i5-8300H\" ," +
          "\"Windows 10 Home 64-bit English\"," +
          "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
          "\"8GB, 2x4GB, DDR4, 2666MHz\"" +
        "]" +
      "}" + "," +
      "\"Id\" :" + id + "," +
      "\"LaptopName\":\"Alienware M17\"" +
    "}";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, jsonMediaType);
                Task<HttpResponseMessage> postResponse = client.PostAsync(postUrl, content);
                HttpResponseMessage response = postResponse.Result;

                HttpStatusCode statusCode = response.StatusCode;
                int statusCodeVal = (int)statusCode;

                HttpContent responseContent = response.Content;
                Task<string> responseData = responseContent.ReadAsStringAsync();
                string data = responseData.Result;

                restResponse = new RestResponse(statusCodeVal, data);

                Console.WriteLine(restResponse.ToString());

                Assert.AreEqual(200, statusCodeVal);

                Assert.IsNotNull(responseContent, "The data is empty or null");

                Task<HttpResponseMessage> httpResponse = client.GetAsync(getURL + id);
                HttpResponseMessage httpResponseMessage = httpResponse.Result;

                HttpContent getContent = httpResponseMessage.Content;
                Task<string> responsedata = getContent.ReadAsStringAsync();
                string getResponse = responsedata.Result;

                HttpStatusCode getStatusCode = httpResponseMessage.StatusCode;
                int getStatusCodeVal = (int)getStatusCode;

                getRestResponse = new RestResponse(getStatusCodeVal, getResponse);

                JSONRootObject jSONRootObject = JsonConvert.DeserializeObject<JSONRootObject>(getRestResponse.ResponseData);

                Assert.AreEqual(id, jSONRootObject.Id);

                Assert.AreEqual("Alienware", jSONRootObject.BrandName);

            }
        }

        [TestMethod]
        public void TestXMLData()
        {
            int id = random.Next(1000);

            string xmlData = "<Laptop>\r\n" +
                "<BrandName>Alienware</BrandName>\r\n" +
                " <Features>\r\n" +
                "<Feature>8th Generation Intel® Core™ i5-8300H</Feature>\r\n" +
                "<Feature>Windows 10 Home 64-bit English</Feature>\r\n" +
                "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>\r\n" +
                "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>\r\n" +
                "</Features>\r\n" +
                $"<Id>{id}</Id>\r\n" +
                "<LaptopName>Alienware M17</LaptopName>\r\n" +
                "</Laptop>";
            using (HttpClient client = new HttpClient())
            {
                HttpContent xmlContent = new StringContent(xmlData, Encoding.UTF8, xmlMediaType);
                Task<HttpResponseMessage> hettpResponse = client.PostAsync(postUrl, xmlContent);

                HttpResponseMessage httpResponseMessage = hettpResponse.Result;
                HttpContent content = httpResponseMessage.Content;
                Task<string> httpResponsedata = content.ReadAsStringAsync();
                string data = httpResponsedata.Result;

                HttpStatusCode httpStatusCode = httpResponseMessage.StatusCode;
                int statusCodeval = (int)httpStatusCode;

                xmlRestResponse = new RestResponse(statusCodeval, data);

                Console.WriteLine(xmlRestResponse.ToString());

                Assert.AreEqual(200, statusCodeval);

                Task<HttpResponseMessage> httpResponse = client.GetAsync(getURL + id);
                HttpResponseMessage httpResponseMessageGet = httpResponse.Result;

                HttpContent getContent = httpResponseMessageGet.Content;
                Task<string> responsedata = getContent.ReadAsStringAsync();
                string getResponse = responsedata.Result;

                HttpStatusCode getStatusCode = httpResponseMessageGet.StatusCode;
                int getStatusCodeVal = (int)getStatusCode;

                if (!httpResponseMessageGet.IsSuccessStatusCode)
                {
                    Assert.Fail("Http request fails");
                }

                getRestResponse = new RestResponse(getStatusCodeVal, getResponse);

                XmlSerializer xmlserializer = new XmlSerializer(typeof(Laptop));
                TextReader xmlreader = new StringReader(getRestResponse.ResponseData);

                Laptop xml = (Laptop)xmlserializer.Deserialize(xmlreader);

                Assert.IsTrue(xml.Features.Feature.Contains("8th Generation Intel® Core™ i5-8300H"), "Item not present");

            }
        }

        [TestMethod]
        public void TestSendAsync()
        {
            int id = random.Next(1000);
            string jsonData =
"{" +
"\"BrandName\" :\"Alienware\"," +
"\"Features\":{" +
"\"Feature\" : [" +
"\"8th Generation Intel® Core™ i5-8300H\" ," +
"\"Windows 10 Home 64-bit English\"," +
"\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
"\"8GB, 2x4GB, DDR4, 2666MHz\"" +
"]" +
"}" + "," +
"\"Id\" :" + id + "," +
"\"LaptopName\":\"Alienware M17\"" +
"}";

            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.RequestUri = new Uri(postUrl);
                    httpRequestMessage.Content = new StringContent(jsonData, Encoding.UTF8, jsonMediaType);

                    Task<HttpResponseMessage> httpResponseMessage = client.SendAsync(httpRequestMessage);
                    HttpResponseMessage responseMessage = httpResponseMessage.Result;
                    HttpContent content = responseMessage.Content;
                    Task<string> contentData = content.ReadAsStringAsync();
                    string data = contentData.Result;

                    RestResponse response = new RestResponse((int)responseMessage.StatusCode, data);

                    Assert.AreEqual(200, response.StatusCode);
                    Console.WriteLine(response.ToString());
                }

            }
        }
    }
}
