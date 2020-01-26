﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WindmillHelix.Brickficiency2.ExternalApi.Rebrickable.Models
{
    [DataContract]
    public class PartSearchResult
    {
        [DataMember(Name = "count")]
        public int Count { get; set; }

        [DataMember(Name = "results")]
        public List<RebrickablePart> Results { get; set; }
    }
}
