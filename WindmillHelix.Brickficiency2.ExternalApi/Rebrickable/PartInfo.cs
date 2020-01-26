using System.Collections.Generic;

namespace WindmillHelix.Brickficiency2.ExternalApi.Rebrickable
{
    public class PartInfo
    {
        public string PartId { get; set; }

        public List<string> BricklinkItemIds { get; set; }

        public List<string> RebrickablePartIds { get; set; }

        public string ImageUrl { get; set; }
    }
}
