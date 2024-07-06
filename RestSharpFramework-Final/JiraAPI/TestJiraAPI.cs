using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using RestSharp;
using RestSharpFramework_Final.JiraAPI.Request;
using RestSharpFramework_Final.JiraAPI.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpFramework_Final.JiraAPI
{
    [TestClass]
    public class TestJiraAPI
    {
        Random random = new Random();
        public static string baseurl = "http://localhost:8080";
        public static string endPoint = @"/rest/auth/1/session";
        public string createProjectEndpoint = "/rest/api/2/project";
        public static IRestClient client;
        public static IRestResponse<Root> loginResponse;
        [ClassInitialize]
        public static void TestAPI(TestContext context)
        {
            LoginRequestBody requestBody = new LoginRequestBody()
            {
                username = "sanket2733",
                password = "Sanket@2799"
            };
            client = new RestClient()
            {
                BaseUrl = new Uri(baseurl)
            };

            IRestRequest request = new RestRequest()
            {
                Resource = endPoint
            };
            request.RequestFormat = DataFormat.Json;
            request.AddBody(requestBody);

            loginResponse = client.Post<Root>(request);
            Console.WriteLine("Login status Code: " + (int)loginResponse.StatusCode);
            Assert.AreEqual(200, (int)loginResponse.StatusCode);
        }

        [TestMethod]
        public void TestCreateProject()
        {
            CreateProjectBody body = new CreateProjectBody()
            {
                key = "EX" + random.Next(1000),
                name = "Example" + random.Next(1000),
                projectTypeKey = "business",
                projectTemplateKey = "com.atlassian.jira-core-project-templates:jira-core-project-management",
                description = "Example Project Description",
                lead = "Charlie",
                url = "http://atlassian.com",
                assigneeType = "PROJECT_LEAD",
                avatarId = 10200,
                /*  issueSecurityScheme = 10001,
                  permissionScheme = 10011,
                  notificationScheme = 10021,
                  categoryId = 10120,*/

            };
            IRestRequest restRequest = new RestRequest()
            {
                Resource = createProjectEndpoint
            };
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddBody(body);
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddCookie(loginResponse.Data.session.name, loginResponse.Data.session.value);
            var response = client.Post<CreateProjectResponse>(restRequest);
            Console.WriteLine("Create Project status Code: " + (int)response.StatusCode);
            Assert.AreEqual(201, (int)response.StatusCode);
        }

        [ClassCleanup]
        public static void Logout()
        {
            IRestRequest logoutRequest = new RestRequest()
            {
                Resource = endPoint
            };

            logoutRequest.AddCookie(loginResponse.Data.session.name, loginResponse.Data.session.value);
            var response = client.Delete(logoutRequest);
            Console.WriteLine("Logout status Code: " + (int)response.StatusCode);
            Assert.AreEqual(204, (int)response.StatusCode);

        }
    }
}
