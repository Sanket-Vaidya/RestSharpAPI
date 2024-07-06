using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTTPClient;
using HTTPClient.Model.JSONModel;
using System.Reflection.Metadata.Ecma335;
using HTTPClient.Model.XMLModel;

namespace RestSharpFramework.RestGetEndPoint
{
    [TestClass]
    public class TestGetEndPoint
    {
        private string getAllUrl = "http://localhost:8080/laptop-bag/webapi/api/all";
        [TestMethod]
        public void TestGetusingRestSharp()
        {
            IRestClient restClient = new RestClient();
            // restClient.AddDefaultHeader("Accept", "application/xml");
            IRestRequest restRequest = new RestRequest(getAllUrl);
            restRequest.AddHeader("Accept", "application/xml");
            IRestResponse restResponse = restClient.Get(restRequest);
            //Console.WriteLine(restResponse.IsSuccessful);
            //Console.WriteLine(restResponse.StatusCode);
            //Console.WriteLine(restResponse.ErrorException);
            //Console.WriteLine(restResponse.ErrorMessage);

            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("Status code: " + restResponse.StatusCode);
                Console.WriteLine("Response Content: " + restResponse.Content);
            }
        }

        [TestMethod]
        public void TestGetJson_Deserailize()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getAllUrl);
            restRequest.AddHeader("Accept", "application/json");
            IRestResponse<List<JSONRootObject>> restResponse = restClient.Get<List<JSONRootObject>>(restRequest);
            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("Status code: " + restResponse.StatusCode);
                Assert.AreEqual(200, (int)restResponse.StatusCode);
                Console.WriteLine(restResponse.Data.Count);
                Console.WriteLine("Response Content: " + restResponse.Content);
                List<JSONRootObject> data = restResponse.Data;
                data.Find((x) =>
                {
                    return x.Id == 1;
                });

                Assert.AreEqual("Alienware", data[0].BrandName);
            }
            else
            {
                Console.WriteLine(restResponse.ErrorException);
                Console.WriteLine(restResponse.ErrorMessage);
            }
        }
        [TestMethod]
        public void TestGetXML_Deserialize()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getAllUrl);
            restRequest.AddHeader("Accept", "application/xml");
            var dotnetXmlDeserializer = new RestSharp.Deserializers.DotNetXmlDeserializer();
            IRestResponse restResponse = restClient.Get(restRequest);
            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("Status Code: " + restResponse.StatusCode);
                Assert.AreEqual(200, (int)restResponse.StatusCode);
                LaptopDetails data = dotnetXmlDeserializer.Deserialize<LaptopDetails>(restResponse);
                Assert.AreEqual("Alienware", data.Laptop.BrandName);

            }
            else
            {
                Console.WriteLine(restResponse.ErrorException);
                Console.WriteLine(restResponse.ErrorMessage);
            }
        }
        [TestMethod]
        public void TestGetWithExecute()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Method = Method.GET,
                Resource = getAllUrl
            };
            restRequest.AddHeader("Accept", "application/json");
            IRestResponse<List<Laptop>> restResponse = restClient.Execute<List<Laptop>>(restRequest);
            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Data, "Response is null");
        }
    }
}
