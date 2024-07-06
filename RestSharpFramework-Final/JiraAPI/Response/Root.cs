using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpFramework_Final.JiraAPI.Response
{
    public class Root
    {
        public Session session { get; set; }
        public LoginInfo loginInfo { get; set; }
    }
}
