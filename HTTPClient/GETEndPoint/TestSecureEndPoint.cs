using HTTPClient.Helper;
using HTTPClient.Model.JSONModel;
using HTTPClient.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HTTPClient.Helper.Authentication;

namespace HTTPClient.GETEndPoint
{
    [TestClass]
    public class TestSecureEndPoint
    {
        private string secureGetAllRequest = "http://localhost:8080/laptop-bag/webapi/secure/all";
        private string secureGetRequest = "http://localhost:8080/laptop-bag/webapi/secure/find/";
        private string securePostRequest = "http://localhost:8080/laptop-bag/webapi/secure/add";
        private string securePutRequest = "http://localhost:8080/laptop-bag/webapi/secure/update";
        private string secureDeleteRequest = "http://localhost:8080/laptop-bag/webapi/secure/delete/";
        private string jsonMediaType = "application/json";
        RestResponse restResponse;
        Random random = new Random();
        [TestMethod]
        public void TestGetAllEndPointResponseSecure()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");
            httpHeader.Add("Authorization", "Basic YWRtaW46d2VsY29tZQ==");
            restResponse = HTTPClientHelper.PerformGetRequest(secureGetAllRequest, httpHeader);

            List<JSONRootObject> jSONRootObject = JsonConvert.DeserializeObject<List<JSONRootObject>>(restResponse.ResponseData);
            Console.WriteLine(jSONRootObject[0].ToString());
        }

        [TestMethod]
        public void TestGetAllEndPointResponseSecurewithBase64()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");
            // httpHeader.Add("Authorization", "Basic YWRtaW46d2VsY29tZQ==");
            string authHeader = Base64StringConverter.GetBase64String("admin", "welcome");
            authHeader = "Basic " + authHeader;
            httpHeader.Add("Authorization", authHeader);
            restResponse = HTTPClientHelper.PerformGetRequest(secureGetAllRequest, httpHeader);

            List<JSONRootObject> jSONRootObject = JsonConvert.DeserializeObject<List<JSONRootObject>>(restResponse.ResponseData);
            Console.WriteLine(jSONRootObject[0].ToString());
        }

        [TestMethod]
        public void PostUsingHelper()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");
            string authHeader = Base64StringConverter.GetBase64String("admin", "welcome");
            authHeader = "Basic " + authHeader;
            httpHeader.Add("Authorization", authHeader);
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
            restResponse = HTTPClientHelper.PerformPostrequest(securePostRequest, jsonData, jsonMediaType, httpHeader);

            Assert.AreEqual(200, restResponse.StatusCode);

            JSONRootObject jsonRootObject = ResponseDataHelp.DeserializeJsonresponse<JSONRootObject>(restResponse.ResponseData);
            Console.WriteLine(jsonRootObject.BrandName);
        }

        [TestMethod]
        public void TestSecurePutWithhelperClass_JSON()
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
            string authHeader = Base64StringConverter.GetBase64String("admin", "welcome");
            authHeader = "Basic " + authHeader;
            httpHeader.Add("Authorization", authHeader);
            restResponse = HTTPClientHelper.PerformPostrequest(securePostRequest, jsonData, jsonMediaType, httpHeader);
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

            restResponse = HTTPClientHelper.PerformPutRequest(securePutRequest, jsonDataUpdation, jsonMediaType, httpHeader);
            Assert.AreEqual(200, restResponse.StatusCode);
        }

        [TestMethod]
        public void TestSecureDeleteEndPointusingHelperClass()
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
            Dictionary<string, string> httpDeleteHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");
            string authHeader = Base64StringConverter.GetBase64String("admin", "welcome");
            authHeader = "Basic " + authHeader;
            httpHeader.Add("Authorization", authHeader);
            restResponse = HTTPClientHelper.PerformPostrequest(securePostRequest, jsonData, jsonMediaType, httpHeader);
            Assert.AreEqual(200, restResponse.StatusCode);

            restResponse = HTTPClientHelper.PerformGetRequest(secureGetRequest + id, httpHeader);
            Assert.AreEqual(id, ResponseDataHelp.DeserializeJsonresponse<JSONRootObject>(restResponse.ResponseData).Id);
            httpDeleteHeader.Add("Authorization", authHeader);
            restResponse = HTTPClientHelper.PerformdeleteRequest(secureDeleteRequest + id, httpDeleteHeader);
            Assert.AreEqual(200, restResponse.StatusCode);


        }
    }
}
