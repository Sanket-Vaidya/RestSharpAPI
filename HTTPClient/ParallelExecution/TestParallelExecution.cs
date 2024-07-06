using HTTPClient.Helper;
using HTTPClient.Model.JSONModel;
using HTTPClient.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using HTTPClient.Model.XMLModel;
using System.Xml.Serialization;

namespace HTTPClient.ParallelExecution
{
    [TestClass]
    public class TestParallelExecution
    {
        public string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";

        public string getURLById = "http://localhost:8080/laptop-bag/webapi/api/find/";
        public string getURLAll = "http://localhost:8080/laptop-bag/webapi/api/all";
        RestResponse restResponse;
        RestResponse getResponse;
        RestResponse postResponse;
        Random random = new Random();
        private string jsonMediaType = "application/json";
        private string xmlMediaType = "application/xml";
        private HTTPClientAsyncHelper asyncHelper = new HTTPClientAsyncHelper();

        public RestResponse GetUsingHelperMethod()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", xmlMediaType);
            RestResponse restResponse = asyncHelper.PerformGetRequest(getURLAll, httpHeader).GetAwaiter().GetResult();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LaptopDetails));

            TextReader reader = new StringReader(restResponse.ResponseData);

            LaptopDetails xmlData = (LaptopDetails)xmlSerializer.Deserialize(reader);
            return restResponse;
        }


        public RestResponse PostUsingHelper()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");

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
            RestResponse restResponse = asyncHelper.PerformPostrequest(postUrl, jsonData, jsonMediaType, httpHeader).GetAwaiter().GetResult();
            Assert.AreEqual(200, restResponse.StatusCode);

            JSONRootObject jsonRootObject = ResponseDataHelp.DeserializeJsonresponse<JSONRootObject>(restResponse.ResponseData);
            Console.WriteLine(jsonRootObject.BrandName);
            return restResponse;
        }

        [TestMethod]
        public void TestTask()
        {
            Task post = new Task(() =>
            {
                postResponse = PostUsingHelper();
            });
            post.Start();

            Task get = new Task(() =>
            {
                getResponse = GetUsingHelperMethod();
            });
            get.Start();

            post.Wait();
            get.Wait();
        }
        [TestMethod]
        public void TestTaskWithTaskFactory()
        {
            var postTask = Task.Factory.StartNew(() =>
            {
                postResponse = PostUsingHelper();
            });
            var getTask = Task.Factory.StartNew(() =>
            {
                getResponse = GetUsingHelperMethod();
            });

            postTask.Wait();
            getTask.Wait();

        }

        [TestMethod]
        public void TestTaskWithTaskFactorywithReturnRestResponse()
        {
            Task<RestResponse>postTask= Task.Factory.StartNew<RestResponse>(() =>
            {
               return PostUsingHelper();
            });
            Task<RestResponse> getTask = Task.Factory.StartNew<RestResponse>(() =>
            {
                return GetUsingHelperMethod();
            });
            Console.Write(postTask.Result.ResponseData);
            Console.WriteLine(getTask.Result.ResponseData);

       }

    }
}
