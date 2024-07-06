using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.Model
{
    public class RestResponse
    {
        private int statusCode;
        private string responseData;

        public RestResponse(int statusCode, string responseData)
        {
            this.statusCode = statusCode;
            this.responseData = responseData;
        }

        public int StatusCode { get { return statusCode; } }
        public string ResponseData { get { return responseData; } }

        public override string ToString()
        {
            return string.Format("Staus Code: {0} Response Data: {1}", statusCode, responseData);
        }
    }
}
