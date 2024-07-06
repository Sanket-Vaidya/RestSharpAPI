using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HTTPClient.Helper
{
    public class ResponseDataHelp
    {
        public static T DeserializeJsonresponse<T>(string responseData)where T:class
        {
            return JsonConvert.DeserializeObject<T>(responseData);
        }

        public static T DeserializeXMLResponse<T>(string responseData) where T : class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            TextReader textReader=new StringReader(responseData);
            return (T)xmlSerializer.Deserialize(textReader);
        }
    }
}
