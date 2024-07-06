using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpFramework_Final.JiraAPI.Request
{
    public class LoginRequestBody
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
