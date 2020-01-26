﻿using System.Collections.Generic;

namespace Brickficiency.Classes
{
    public class DBBLItem
    {
        public string id;
        public string number;
        public string type;
        public string name;
        public string catid;
        public Dictionary<string, string> pgpage = new Dictionary<string, string>();
        public string pgcolourspage;
    }
}
