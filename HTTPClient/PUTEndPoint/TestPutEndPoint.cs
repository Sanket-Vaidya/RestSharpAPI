using HTTPClient.Helper;
using HTTPClient.Model;
using HTTPClient.Model.JSONModel;
using HTTPClient.Model.XMLModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.PUTEndPoint
{
    [TestClass]
    public class TestPutEndPoint
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private string getURL = "http://localhost:8080/laptop-bag/webapi/api/find/";
        private string putURL = "http://localhost:8080/laptop-bag/webapi/api/update";

        private RestResponse restResponse;
        private RestResponse xmlRestResponse;
        private RestResponse getRestResponse;

        private Random random = new Random();

        private string jsonMediaType = "application/json";
        private string xmlMediaType = "application/xml";

        [TestMethod]
        public void TestPut()
        {
            int id = random.Next(1000);
            string jsonData =
    "{" +
      "\"BrandName\" :\"Alienware\"," +
      "\"Features\":{" +
        "\"Feature\" : [" +
          "\"200th Generation Intel® Core™ i5-8300H\" ," +
          "\"Windows 10 Home 64-bit English\"," +
          "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
          "\"32GB, 2x4GB, DDR4, 2666MHz\"" +
        "]" +
      "}" + "," +
      "\"Id\" :" + id + "," +
      "\"LaptopName\":\"Alienware M17\"" +
    "}";
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");
            restResponse = HTTPClientHelper.PerformPostrequest(postUrl, jsonData, jsonMediaType, httpHeader);
            Assert.AreEqual(200, restResponse.StatusCode);


            string jsonDataUpdation =
   "{" +
     "\"BrandName\" :\"Alienware\"," +
     "\"Features\":{" +
       "\"Feature\" : [" +
         "\"200th Generation Intel® Core™ i5-8300H\" ," +
         "\"Windows 10 Home 64-bit English\"," +
         "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
         "\"32GB, 2x4GB, DDR4, 2666MHz\"," +
         "\"Made In India\"" +
       "]" +
     "}" + "," +
     "\"Id\" :" + id + "," +
     "\"LaptopName\":\"Alienware M17\"" +
   "}";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent content = new StringContent(jsonDataUpdation, Encoding.UTF8, jsonMediaType);
                Task<HttpResponseMessage> httpResponseMessage = httpClient.PutAsync(putURL, content);

                HttpResponseMessage httpResponse = httpResponseMessage.Result;
                restResponse = new RestResponse((int)httpResponse.StatusCode, httpResponse.Content.ReadAsStringAsync().Result);

                Console.WriteLine(restResponse.ResponseData.ToString());
            }

            restResponse = HTTPClientHelper.PerformGetRequest(getURL + id, httpHeader);
            JSONRootObject jSONRootObject = ResponseDataHelp.DeserializeJsonresponse<JSONRootObject>(restResponse.ResponseData);

            Assert.AreEqual("Made In India", jSONRootObject.Features.Feature[4]);
        }

        [TestMethod]
        public void TestPutWithXML()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/xml");
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

            restResponse = HTTPClientHelper.PerformPostrequest(postUrl, xmlData, xmlMediaType, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            string xmlDataUpdated = "<Laptop>\r\n" +
                                    "<BrandName>Alienware</BrandName>\r\n" +
                                    " <Features>\r\n" +
                                    "<Feature>8th Generation Intel® Core™ i5-8300H</Feature>\r\n" +
                                    "<Feature>Windows 10 Home 64-bit English</Feature>\r\n" +
                                    "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>\r\n" +
                                    "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>\r\n" +
                                    "<Feature>Made in India</Feature>\r\n" +
                                    "</Features>\r\n" +
                                   $"<Id>{id}</Id>\r\n" +
                                    "<LaptopName>Alienware M17</LaptopName>\r\n" +
                                    "</Laptop>";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent content = new StringContent(xmlDataUpdated, Encoding.UTF8, xmlMediaType);
                Task<HttpResponseMessage> httpResponseMessage = httpClient.PutAsync(putURL, content);

                restResponse = new RestResponse((int)httpResponseMessage.Result.StatusCode, httpResponseMessage.Result.Content.ReadAsStringAsync().Result);

                Assert.AreEqual(200, restResponse.StatusCode);
            }

            restResponse = HTTPClientHelper.PerformGetRequest(getURL + id, headers);

            Laptop laptop = ResponseDataHelp.DeserializeXMLResponse<Laptop>(restResponse.ResponseData);
            Assert.AreEqual("Made in India", laptop.Features.Feature[4]);

        }

        [TestMethod]
        public void TestPutWithhelperClass_JSON()
        {
            int id = random.Next(1000);
            string jsonData =
    "{" +
      "\"BrandName\" :\"Alienware\"," +
      "\"Features\":{" +
        "\"Feature\" : [" +
          "\"200th Generation Intel® Core™ i5-8300H\" ," +
          "\"Windows 10 Home 64-bit English\"," +
          "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
          "\"32GB, 2x4GB, DDR4, 2666MHz\"" +
        "]" +
      "}" + "," +
      "\"Id\" :" + id + "," +
      "\"LaptopName\":\"Alienware M17\"" +
    "}";
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");
            restResponse = HTTPClientHelper.PerformPostrequest(postUrl, jsonData, jsonMediaType, httpHeader);
            Assert.AreEqual(200, restResponse.StatusCode);


            string jsonDataUpdation =
   "{" +
     "\"BrandName\" :\"Alienware\"," +
     "\"Features\":{" +
       "\"Feature\" : [" +
         "\"200th Generation Intel® Core™ i5-8300H\" ," +
         "\"Windows 10 Home 64-bit English\"," +
         "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
         "\"32GB, 2x4GB, DDR4, 2666MHz\"," +
         "\"Made In India\"" +
       "]" +
     "}" + "," +
     "\"Id\" :" + id + "," +
     "\"LaptopName\":\"Alienware M17\"" +
   "}";

            restResponse = HTTPClientHelper.PerformPutRequest(putURL, jsonDataUpdation, jsonMediaType, httpHeader);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HTTPClientHelper.PerformGetRequest(getURL + id, httpHeader);
            JSONRootObject jSONRootObject = ResponseDataHelp.DeserializeJsonresponse<JSONRootObject>(restResponse.ResponseData);

            Assert.AreEqual("Made In India", jSONRootObject.Features.Feature[4]);
        }
        [TestMethod]
        public void TestPutWithXMLWithClientHelper()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/xml");
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

            restResponse = HTTPClientHelper.PerformPostrequest(postUrl, xmlData, xmlMediaType, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            string xmlDataUpdated = "<Laptop>\r\n" +
                                    "<BrandName>Alienware</BrandName>\r\n" +
                                    " <Features>\r\n" +
                                    "<Feature>8th Generation Intel® Core™ i5-8300H</Feature>\r\n" +
                                    "<Feature>Windows 10 Home 64-bit English</Feature>\r\n" +
                                    "<Feature>NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6</Feature>\r\n" +
                                    "<Feature>8GB, 2x4GB, DDR4, 2666MHz</Feature>\r\n" +
                                    "<Feature>Made in India</Feature>\r\n" +
                                    "</Features>\r\n" +
                                   $"<Id>{id}</Id>\r\n" +
                                    "<LaptopName>Alienware M17</LaptopName>\r\n" +
                                    "</Laptop>";

            restResponse = HTTPClientHelper.PerformPutRequest(putURL, xmlDataUpdated, xmlMediaType, headers);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HTTPClientHelper.PerformGetRequest(getURL + id, headers);

            Laptop laptop = ResponseDataHelp.DeserializeXMLResponse<Laptop>(restResponse.ResponseData);
            Assert.AreEqual("Made in India", laptop.Features.Feature[4]);

        }

    }
}
