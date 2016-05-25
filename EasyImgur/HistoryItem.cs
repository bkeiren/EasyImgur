using System;
using System.Drawing;
using Newtonsoft.Json;

namespace EasyImgur
{
    class HistoryItem
    {
        // Databound control: textBoxID
        [JsonProperty("id")]
        public string Id { get; set; }

        // Databound control: textBoxLink
        [JsonProperty("link")]
        public string Link { get; set; }

        // Databound control: textBoxDeleteHash
        [JsonProperty("deletehash")]
        public string Deletehash { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        // Databound control: pictureBoxHistoryThumb
        [JsonProperty("thumbnail")]
        public Image Thumbnail { get; set; }

        [JsonProperty("anonymous")]
        public bool Anonymous { get; set; }

        // Databound control: checkBoxAlbum
        [JsonProperty("album")]
        public bool Album { get; set; }

        // Databound control: textBoxTimestamp
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        // Databound control: checkBoxTiedToAccount
        [JsonIgnore]
        public bool TiedToAccount
        {
            get { return !this.Anonymous; }
        }

        [JsonIgnore]
        public string ListName
        {
            get
            {
                return this.Title + "//" + this.Id;
            }
        }
    }
}
