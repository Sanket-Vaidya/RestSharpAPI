namespace HTTPClient
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            HttpClient client = new HttpClient();
            client.Dispose();
        }
    }
}