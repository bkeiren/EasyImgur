using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;

namespace EasyImgur
{
    public class ImageConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Bitmap) || objectType == typeof(Image);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(reader.Value == null)
                return null;

            var m = new MemoryStream(Convert.FromBase64String((string)reader.Value));
            return (Bitmap)Bitmap.FromStream(m);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Bitmap img = value as Bitmap;
            MemoryStream m = new MemoryStream();
            img.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);

            writer.WriteValue(Convert.ToBase64String(m.ToArray()));
        }
    }
}
