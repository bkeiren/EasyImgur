using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyImgur
{
    class HistoryItem
    {
        public string id;
        public string link;
        public string deletehash;
        public string title;
        public string description;
        public System.Drawing.Image thumbnail;

        public string listName
        {
            get
            {
                return title + "//" + id;
            }
        }
    }
}
