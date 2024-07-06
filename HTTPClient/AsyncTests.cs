using HTTPClient.Helper;
using HTTPClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient
{
    [TestClass]
    public class AsyncTests
    {
        //private string asynGetrequest = "https://freetestapi.com/api/v1/authors";
        private string asynGetrequest = "http://localhost:8080/laptop-bag/webapi/api/all";
        public RestResponse restResponse;

        [TestMethod]
        public void TestAsyncGetRequest()
        {
            Dictionary<string, string> httpHeaders = new Dictionary<string, string>();
            httpHeaders.Add("Accept", "application/xml");
            restResponse = HTTPClientHelper.PerformGetRequest(asynGetrequest, httpHeaders);
            Console.WriteLine("Request Commpleted");
            restResponse = HTTPClientHelper.PerformGetRequest(asynGetrequest, httpHeaders);
            Console.WriteLine("Request Commpleted");
            restResponse = HTTPClientHelper.PerformGetRequest(asynGetrequest, httpHeaders);
            Console.WriteLine("Request Commpleted");
            restResponse = HTTPClientHelper.PerformGetRequest(asynGetrequest, httpHeaders);
            Console.WriteLine("Request Commpleted");
            Console.WriteLine(restResponse.ResponseData);
            Console.WriteLine(restResponse.StatusCode);
            //Assert.AreEqual(200, restResponse.StatusCode);
        }

        [TestMethod]
        public void TestAsyncGetRequestTask()
        {
            Task T1 = new Task(GetEndPoint());
            T1.Start();
            Task T2 = new Task(GetEndPoint());
            T2.Start();
            Task T3 = new Task(GetEndPoint());
            T3.Start();
            Task T4 = new Task(GetEndPointFailed());
            T4.Start();
            T1.Wait();
            T2.Wait();
            T3.Wait();
            T4.Wait();
        }

        public Action GetEndPoint()
        {
            Dictionary<string, string> httpHeaders = new Dictionary<string, string>();
            httpHeaders.Add("Accept", "application/xml");
            return new Action(() => { restResponse = HTTPClientHelper.PerformGetRequest(asynGetrequest, httpHeaders);
                Assert.AreEqual(200,restResponse.StatusCode);
            });

        }

        public Action GetEndPointFailed()
        {
            Dictionary<string, string> httpHeaders = new Dictionary<string, string>();
            httpHeaders.Add("Accept", "application/xml");
            return new Action(() => {
                restResponse = HTTPClientHelper.PerformGetRequest(asynGetrequest, httpHeaders);
                Assert.AreEqual(201, restResponse.StatusCode);
            });

        }

        [TestMethod]
        public void Test()
        {
            Task t1 = new Task(method1());
            t1.Start();
            Task t2 = new Task(method2());
            t2.Start();
            Task t3 = new Task(method3());
            t3.Start();
            Task t4 = new Task(method4());
            t4.Start();
            t1.Wait();
            t2.Wait();
            t3.Wait();
            t4.Wait();
        }
        public Action method1()
        {
            return new Action(() =>
            {
                Console.WriteLine("Line1");
                Thread.Sleep(10000);
            });
        }

        public Action method2()
        {
            return new Action(() =>
            {
                Console.WriteLine("Line2");
                Thread.Sleep(5000);
            });
        }

        public Action method3()
        {
            return new Action(() =>
            {
                Console.WriteLine("Line3");
                Thread.Sleep(3000);
            });
        }

        public Action method4()
        {
            return new Action(() => { Console.WriteLine("Line4"); });
        }

    }
}
