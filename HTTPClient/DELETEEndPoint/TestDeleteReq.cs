using HTTPClient.Helper;
using HTTPClient.Model;
using HTTPClient.Model.JSONModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.DELETEEndPoint
{
    [TestClass]
    public class TestDeleteReq
    {
        private string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        private string getURL = "http://localhost:8080/laptop-bag/webapi/api/find/";
        private string putURL = "http://localhost:8080/laptop-bag/webapi/api/update";
        private string deleteURL = "http://localhost:8080/laptop-bag/webapi/api/delete/";

        private RestResponse restResponse;
        private RestResponse xmlRestResponse;
        private RestResponse getRestResponse;

        private Random random = new Random();

        private string jsonMediaType = "application/json";
        private string xmlMediaType = "application/xml";
        [TestMethod]
        public void TestDeleteEndPoint()
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

            restResponse = HTTPClientHelper.PerformGetRequest(getURL + id, httpHeader);
            Assert.AreEqual(id, ResponseDataHelp.DeserializeJsonresponse<JSONRootObject>(restResponse.ResponseData).Id);

            using(HttpClient client = new HttpClient())
            {
                Task<HttpResponseMessage> httpResponseMessage = client.DeleteAsync(deleteURL + id);
                HttpResponseMessage httpResponse = httpResponseMessage.Result;
                Assert.AreEqual(200, (int)httpResponse.StatusCode);
            }

            restResponse = HTTPClientHelper.PerformGetRequest(getURL + id, httpHeader);
            try
            {
            int? ID = ResponseDataHelp.DeserializeJsonresponse<JSONRootObject>(restResponse.ResponseData).Id;
                Assert.Fail("Entry not deleted");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(true);
                Console.WriteLine("Entry deleted");
            }

        }

        [TestMethod]
        public void TestDeleteEndPointusingHelperClass()
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

            restResponse = HTTPClientHelper.PerformGetRequest(getURL + id, httpHeader);
            Assert.AreEqual(id, ResponseDataHelp.DeserializeJsonresponse<JSONRootObject>(restResponse.ResponseData).Id);

            restResponse = HTTPClientHelper.PerformdeleteRequest(deleteURL + id);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HTTPClientHelper.PerformGetRequest(getURL + id, httpHeader);
            try
            {
                int? ID = ResponseDataHelp.DeserializeJsonresponse<JSONRootObject>(restResponse.ResponseData).Id;
                Assert.Fail("Entry not deleted");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(true);
                Console.WriteLine("Entry deleted");
            }

        }
    }
}
