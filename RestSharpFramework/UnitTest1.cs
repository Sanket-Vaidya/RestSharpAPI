using RestSharp;

namespace RestSharpFramework
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            /*
             1. Create the client
             2. Create the request
             3. Send the request using client
             4. Capture the response
             */
            IRestClient client = new RestClient();
            IRestRequest request = new RestRequest();

        }
    }
}