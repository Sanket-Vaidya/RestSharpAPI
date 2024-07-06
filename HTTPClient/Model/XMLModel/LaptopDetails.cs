using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HTTPClient.Model.XMLModel
{
    [XmlRoot(ElementName = "laptopDetailss")]
    public class LaptopDetails
    {
        [XmlElement(ElementName = "Laptop")]
        public Laptop Laptop { get; set; }
    }
}
