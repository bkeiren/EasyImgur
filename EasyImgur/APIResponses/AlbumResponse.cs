using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace EasyImgur.APIResponses
{
    public class AlbumResponse : BaseResponse
    {
        public class Data : BaseResponse.BaseData
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("datetime")]
            public int Datetime { get; set; }

            [JsonProperty("cover")]
            public string Cover { get; set; }

            [JsonProperty("cover_width")]
            public int CoverWidth { get; set; }

            [JsonProperty("cover_height")]
            public int CoverHeight { get; set; }

            [JsonProperty("account_url")]
            public string AccountUrl { get; set; }

            [JsonProperty("privacy")]
            public string Privacy { get; set; }

            [JsonProperty("layout")]
            public string Layout { get; set; }

            [JsonProperty("views")]
            public int Views { get; set; }

            [JsonProperty("link")]
            public string Link { get; set; }

            [JsonProperty("deletehash")]
            public string DeleteHash { get; set; }

            [JsonProperty("images_count")]
            public int ImageCount { get; set; }

            [JsonProperty("images")]
            public ImageResponse.Data[] Images { get; set; }
        }
        [JsonProperty("data")]
        public Data ResponseData { get; set; }
        public Image CoverImage { get; set; } 
    }
}
