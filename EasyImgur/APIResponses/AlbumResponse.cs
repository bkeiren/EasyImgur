using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EasyImgur.APIResponses
{
    public class AlbumResponse : BaseResponse
    {
        public class Data : BaseResponse.BaseData
        {
            public string id;
            public string title;
            public string description;
            public int datetime;
            public string cover;
            public int cover_width;
            public int cover_height;
            public string account_url;
            public string privacy;
            public string layout;
            public int views;
            public string link;
            public string deletehash;
            public int images_count;
            public ImageResponse.Data[] images;
        }
        public Data data;
        public Image CoverImage; 
    }
}
