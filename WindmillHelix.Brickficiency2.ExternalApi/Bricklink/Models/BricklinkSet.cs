using System.Xml.Serialization;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink.Models
{
    public class BricklinkSet
    {
        [XmlElement(ElementName = "ITEMID")]
        public string SetId { get; set; }

        [XmlElement(ElementName = "ITEMTYPE")]
        public string ItemTypeCode { get; set; }

        [XmlElement(ElementName = "ITEMNAME")]
        public string Name { get; set; }

        [XmlElement(ElementName = "CATEGORY")]
        public int CategoryId { get; set; }

        [XmlElement(ElementName = "ITEMYEAR")]
        public string Year { get; set; }
    }
}
