using HTTPClient.Model.JSONModel;
using HTTPClient.Model.XMLModel;
using RestSharp;
using RestSharp.Authenticators;
using RestSharpFramework_Final.Helper.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpFramework_Final.TestSuite
{
    [TestClass]
    public class Test_001
    {
        Random random = new Random();
        string jsonMediaType = "application/json";
        string xmlMediaType = "application/xml";
        Dictionary<string, string> headers = new Dictionary<string, string>();
        string getAllUrl = "http://localhost:8080/laptop-bag/webapi/api/all";
        string postUrl = "http://localhost:8080/laptop-bag/webapi/api/add";
        string putUrl = "http://localhost:8080/laptop-bag/webapi/api/update";
        string deleteUrl = "http://localhost:8080/laptop-bag/webapi/api/delete/";
        private string secureGetUrl = "http://localhost:8080/laptop-bag/webapi/secure/all";
        private string tokenBasedurl = "";

        [TestMethod]
        public void TestGetPointwithJson()
        {
            headers.Add("Accept", "application/json");
            RequestClientHelper requestClientHelper = new RequestClientHelper();

            IRestResponse<List<JSONRootObject>> restResponse = requestClientHelper.PerformGetRequest<List<JSONRootObject>>(getAllUrl, headers);

            Assert.AreEqual(200, (int)restResponse.StatusCode);
        }

        [TestMethod]
        public void TestGetPointwithXml()
        {
            headers.Add("Accept", "application/xml");
            RequestClientHelper requestClientHelper = new RequestClientHelper();

            IRestResponse<LaptopDetails> restResponse = requestClientHelper.PerformGetRequest<LaptopDetails>(getAllUrl, headers);


            Assert.AreEqual(200, (int)restResponse.StatusCode);
        }

        [TestMethod]
        public void TestPostEndPoint()
        {
            int id = random.Next(1000);
            string data = "{" +
      "\"BrandName\" :\"Earthware\"," +
      "\"Features\":{" +
        "\"Feature\" : [" +
          "\"11th Generation Intel® Core™ i5-8300H\" ," +
          "\"Windows 20 Home 64-bit English\"," +
          "\"NVIDIA® GeForce® GTX 1660 Ti 6GB GDDR6\"," +
          "\"32GB, 2x4GB, DDR4, 2666MHz\"" +
        "]" +
      "}" + "," +
      "\"Id\" :" + id + "," +
      "\"LaptopName\":\"Alienware M17\"" +
    "}";
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = postUrl,
                Method = Method.POST,
            };
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddJsonBody(data);
            IRestResponse restResponse = restClient.Post(restRequest);
            Assert.AreEqual(200, (int)restResponse.StatusCode);

            Console.WriteLine(restResponse.Content);
        }

        public JSONRootObject Getlaptop()
        {
            JSONRootObject laptop = new JSONRootObject();
            HTTPClient.Model.JSONModel.Features features = new HTTPClient.Model.JSONModel.Features();
            List<string> feature = new List<string>() { "Powered with AI" };
            features.Feature = feature;
            laptop.Features = features;
            laptop.LaptopName = "Acer";
            laptop.BrandName = "HP";
            laptop.Id = random.Next(1000);

            return laptop;
        }


        public Laptop GetlaptopXml()
        {
            Laptop laptop = new Laptop();
            HTTPClient.Model.XMLModel.Features features = new HTTPClient.Model.XMLModel.Features();
            List<string> feature = new List<string>() { "Powered with AI" };
            features.Feature = feature;
            laptop.Features = features;
            laptop.LaptopName = "Acer";
            laptop.BrandName = "HP";
            laptop.Id = "" + random.Next(1000);

            return laptop;
        }

        [TestMethod]
        public void TestPostEndPointwithGetLaptopMethod()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Resource = postUrl,
                Method = Method.POST,
            };
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddBody(Getlaptop());
            IRestResponse restResponse = restClient.Post(restRequest);
            Assert.AreEqual(200, (int)restResponse.StatusCode);

            Console.WriteLine(restResponse.Content);
        }

        [TestMethod]
        public void TestPostMethUsingRestClientHelper()
        {
            headers.Add("Accept", "application/json");
            RequestClientHelper clientHelper = new RequestClientHelper();
            IRestResponse<JSONRootObject> restResponse = clientHelper.PerformPostRequest<JSONRootObject>(postUrl, headers, Getlaptop(), DataFormat.Json);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Console.WriteLine(restResponse.Content);
        }

        [TestMethod]
        public void TestPostMethUsingRestClientHelperForXml()
        {
            headers.Add("Accept", "application/xml");
            headers.Add("Content-Type", "application/xml");
            RequestClientHelper clientHelper = new RequestClientHelper();
            IRestResponse<Laptop> restResponse = clientHelper.PerformPostRequest<Laptop>(postUrl, headers, GetlaptopXml(), DataFormat.Xml);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Console.WriteLine(restResponse.Content);
        }

        [TestMethod]
        public void TestPutMethodUsingRestClientHelper()
        {
            headers.Add("Accept", "application/json");
            headers.Add("Content-Type", "application/json");
            RequestClientHelper clientHelper = new RequestClientHelper();
            JSONRootObject laptop = Getlaptop();
            IRestResponse<Laptop> restResponse = clientHelper.PerformPostRequest<Laptop>(postUrl, headers, Getlaptop(), DataFormat.Json);
            Assert.AreEqual(200, (int)restResponse.StatusCode);

            string id = restResponse.Data.Id;
            laptop.BrandName = "Dell";
            laptop.LaptopName = "Apple";
            laptop.Id = int.Parse(id);

            IRestClient restClient = new RestClient();
            IRestRequest restRequest1 = new RestRequest()
            {
                Resource = putUrl,
                Method = Method.PUT
            };
            restRequest1.RequestFormat = DataFormat.Json;
            restRequest1.AddBody(laptop);

            IRestResponse<JSONRootObject> putRestResponse = restClient.Put<JSONRootObject>(restRequest1);
            Assert.AreEqual("Dell", putRestResponse.Data.BrandName);
            Console.WriteLine(putRestResponse.Content);

        }

        [TestMethod]
        public void TestPutMethodUsingRestClientHelperWithXml()
        {
            headers.Add("Accept", "application/xml");
            headers.Add("Content-Type", "application/xml");
            RequestClientHelper clientHelper = new RequestClientHelper();
            Laptop laptop = GetlaptopXml();
            IRestResponse<Laptop> restResponse = clientHelper.PerformPostRequest<Laptop>(postUrl, headers, GetlaptopXml(), DataFormat.Xml);
            Assert.AreEqual(200, (int)restResponse.StatusCode);

            string id = restResponse.Data.Id;
            laptop.BrandName = "Dell";
            laptop.LaptopName = "Apple";
            laptop.Id = id;

            IRestClient restClient = new RestClient();
            IRestRequest restRequest1 = new RestRequest()
            {
                Resource = putUrl,
                Method = Method.PUT
            };
            restRequest1.RequestFormat = DataFormat.Xml;
            restRequest1.XmlSerializer = new RestSharp.Serializers.DotNetXmlSerializer();
            restRequest1.AddParameter("xmlBody", restRequest1.XmlSerializer.Serialize(laptop), ParameterType.RequestBody);
            restRequest1.AddHeader("Accept", "application/xml");
            restRequest1.AddHeader("Content-Type", "application/xml");
            IRestResponse<Laptop> putRestResponse = restClient.Put<Laptop>(restRequest1);
            Assert.AreEqual("Dell", putRestResponse.Data.BrandName);
            Console.WriteLine(putRestResponse.Content);

        }


        [TestMethod]
        public void TestPutMethodUsingRestClientHelperWithXmlandRestSharpFrameWork()
        {
            headers.Add("Accept", "application/xml");
            headers.Add("Content-Type", "application/xml");
            RequestClientHelper clientHelper = new RequestClientHelper();
            Laptop laptop = GetlaptopXml();
            IRestResponse<Laptop> restResponse = clientHelper.PerformPostRequest<Laptop>(postUrl, headers, GetlaptopXml(), DataFormat.Xml);
            Assert.AreEqual(200, (int)restResponse.StatusCode);

            string id = restResponse.Data.Id;
            laptop.BrandName = "Dell";
            laptop.LaptopName = "Apple";
            laptop.Id = id;

            IRestResponse<Laptop> putRestResponse = clientHelper.PerformPutRequest<Laptop>(putUrl, headers, laptop, DataFormat.Xml);
            Assert.AreEqual("Dell", putRestResponse.Data.BrandName);
            Console.WriteLine(putRestResponse.Content);

        }

        [TestMethod]
        public void TestDeleteMethod()
        {
            headers.Add("Accept", "application/xml");
            headers.Add("Content-Type", "application/xml");
            RequestClientHelper clientHelper = new RequestClientHelper();
            Laptop laptop = GetlaptopXml();
            IRestResponse<Laptop> restResponse = clientHelper.PerformPostRequest<Laptop>(postUrl, headers, GetlaptopXml(), DataFormat.Xml);
            Assert.AreEqual(200, (int)restResponse.StatusCode);

            string id = restResponse.Data.Id;
            Dictionary<string, string> head = new Dictionary<string, string>();
            head.Add("Accept", "*/*");
            IRestResponse restResponse1 = clientHelper.PerformDeleteRequest(deleteUrl + id, head);
            Assert.AreEqual(200, (int)restResponse1.StatusCode);
            Console.WriteLine(restResponse1.Content);

        }

        [TestMethod]
        public void TestSecureGet()
        {
            IRestClient client = new RestClient();
            client.Authenticator = new HttpBasicAuthenticator("admin", "welcome");
            IRestRequest request = new RestRequest()
            {
                Resource = secureGetUrl,
                Method = Method.GET
            };
            request.AddHeader("Accept", "application/json");
            IRestResponse response = client.Get(request);
            Assert.AreEqual((int)response.StatusCode, 200);
            Console.WriteLine(response.Content);
        }
        [TestMethod]
        public void TokenBasedAuthentication()
        {

        }

    }
}
