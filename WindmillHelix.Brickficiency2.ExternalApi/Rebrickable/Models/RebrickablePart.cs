using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WindmillHelix.Brickficiency2.ExternalApi.Rebrickable.Models
{
    [DataContract]
    public class RebrickablePart
    {
        [DataMember(Name = "part_num")]
        public string PartNumber { get; set; }

        [DataMember(Name = "part_img_url")]
        public string ImageUrl { get; set; }

        [DataMember(Name = "external_ids")]
        public Dictionary<string, List<string>> ExternalIds { get; set; }
    }
}
