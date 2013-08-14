using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyImgur.APIResponses
{
    public class ImageResponse : BaseResponse
    {
        public class Data : BaseResponse.BaseData
        {
            public string id;
            public string title;
            public string description;
            public string datetime;
            public string type;
            public bool animated;
            public int width;
            public int height;
            public int size;
            public int views;
            public int bandwidth;
            public bool favorite;
            public string nsfw;
            public string section;
            public string deletehash;
            public string link;
        }
        public Data data;
    }
}
