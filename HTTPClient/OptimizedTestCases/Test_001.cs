using HTTPClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTTPClient.Helper;
using HTTPClient.Model.JSONModel;
using Newtonsoft.Json;

namespace HTTPClient.OptimizedTestCases
{
    [TestClass]
    public class Test_001
    {
        public string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";

        public string getURLById = "http://localhost:8080/laptop-bag/webapi/api/find/";
        public string getURLAll = "http://localhost:8080/laptop-bag/webapi/api/all";

        private RestResponse restResponse;
        private RestResponse xmlRestResponse;
        private RestResponse getRestResponse;

        private Random random = new Random();

        private string jsonMediaType = "application/json";
        private string xmlMediaType = "application/xml";

        [TestMethod]
        public void GetUsingHelperMethod()
        {
            Dictionary<string, string> httpHeader = new Dictionary<string, string>();
            httpHeader.Add("Accept", "application/json");

            restResponse = HTTPClientHelper.PerformGetRequest(getURLAll, httpHeader);

            List<JSONRootObject> jSONRootObject = JsonConvert.DeserializeObject<List<JSONRootObject>>(restResponse.ResponseData);
            Console.WriteLine(jSONRootObject[0].ToString());
        }

        [TestMethod]
        public void PostUsingHelper()
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
            restResponse = HTTPClientHelper.PerformPostrequest(postUrl, jsonData, jsonMediaType, httpHeader);

            Assert.AreEqual(200, restResponse.StatusCode);

            JSONRootObject jsonRootObject = ResponseDataHelp.DeserializeJsonresponse<JSONRootObject>(restResponse.ResponseData);
            Console.WriteLine(jsonRootObject.BrandName);
        }
    }
}
