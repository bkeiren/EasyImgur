using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace EasyImgur.APIResponses
{
    public class ImageResponse : BaseResponse
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
            public string Datetime { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("animated")]
            public bool Animated { get; set; }

            [JsonProperty("width")]
            public int Width { get; set; }

            [JsonProperty("height")]
            public int Height { get; set; }

            [JsonProperty("size")]
            public int Size { get; set; }

            [JsonProperty("views")]
            public int Views { get; set; }

            [JsonProperty("bandwidth")]
            public int Bandwidth { get; set; }

            [JsonProperty("favorite")]
            public bool Favorite { get; set; }

            [JsonProperty("nsfw")]
            public string Nsfw { get; set; }

            [JsonProperty("section")]
            public string Section { get; set; }

            [JsonProperty("deletehash")]
            public string DeleteHash { get; set; }

            [JsonProperty("link")]
            public string Link { get; set; }
        }
        [JsonProperty("data")]
        public Data ResponseData { get; set; }
    }
}
