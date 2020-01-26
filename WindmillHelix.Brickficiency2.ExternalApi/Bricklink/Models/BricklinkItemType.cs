using System.Xml.Serialization;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink.Models
{
    public class BricklinkItemType
    {
        [XmlElement(ElementName = "ITEMTYPE")]
        public string ItemTypeCode { get; set; }

        [XmlElement(ElementName = "ITEMTYPENAME")]
        public string Name { get; set; }
    }
}
