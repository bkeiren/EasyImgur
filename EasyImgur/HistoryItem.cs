using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyImgur
{
    class HistoryItem
    {
        // some fields are properties because only properties (not fields!) can be data bound
        // this particular fact stumped me for a good two hours
        public string id { get; set; }
        public string link { get; set; }
        public string deletehash { get; set; }
        public string title;
        public string description;
        public System.Drawing.Image thumbnail { get; set; }
        public bool anonymous;
        public bool album { get; set; }
        public DateTime timestamp { get; set; }

        // used for data binding
        [JsonIgnore]
        public bool tiedToAccount { get { return !anonymous; } set { anonymous = !value; } }

        [JsonIgnore]
        public string listName
        {
            get
            {
                return title + "//" + id;
            }
        }
    }
}
