using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Newtonsoft.Json;

namespace EasyImgur
{
    public class ImageConverter : JsonConverter
    {
        private static readonly Bitmap FallbackImage;

        static ImageConverter()
        {
            FallbackImage = new Bitmap(new MemoryStream(Convert.FromBase64String(
                "iVBORw0KGgoAAAANSUhEUgAAAIwAAAASCAMAAAC+csY/AAAAGXRFWHRTb2Z0d2Fy" +
                "ZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAADBQTFRF////8+OtmJiYZ2Vi6eno061e" +
                "AAAAuH0J8dJOxpU0+O3CwogU/f39xMTE8fDv38OEErswEgAAAPdJREFUeNrslNEW" +
                "hBAQhhGjEO//tkvEKLY93XCxf53GoRlfPyJkKu0saZ8AhpmkGWiY4YcMn4CmwExA" +
                "g2DG02SYY9+UfggiEFoEh9iA1BO7zgeU3BKgDrkApClaMNx5IIZHcO4VJlU8R6rx" +
                "RtK3gLUYawOLEIKb5QEmfuIvMOQ1jMdxmxWuAROdzLnxyssEdeE2THeZejBWUqXk" +
                "ozNofmROBu45U1e5v3mBcUJJJdwjzO2GvJHgxfo0YbhW1HujezAEu3o5TRimOkb1" +
                "mWtVasMIp9Z1VU4O/s8EGKo9i/Q0Gx0MQ4zxxtCobfAfePGSSZqSv6I+AgwA748L" +
                "4j8tunoAAAAASUVORK5CYII=")));
            // Image preview: http://i.imgur.com/O3eiYoo.png
            // Icon readme & credits:
            //      You can do whatever you want with these icons (use on web or in desktop applications) as long as you
            //      don’t pass them off as your own and remove this readme file.
            //
            //      A credit statement and a link back to http://led24.de/iconset/ or http://led24.de/ would be appreciated.
            //      Follow us on twitter http://twitter.com/gasyoun or email leds24@gmail.com
            //      512 icons 20/05/2009

        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Bitmap) || objectType == typeof(Image);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;

            Image ret;
            try
            {
                var bytes = Convert.FromBase64String((string) reader.Value);
                using (var m = new MemoryStream(bytes))
                {
                    ret = Image.FromStream(m);
                }
            }
            catch
            {
                // Either couldn't parse the base-64 string or 
                // it contained an invalid image.
                ret = new Bitmap(FallbackImage);
            }
            return ret;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            using (var img = new Bitmap(value as Bitmap)) // new Bitmap fixes occasional generic GDI+ external exceptions on Windows 8.1
            using (var m = new MemoryStream())
            {
                img.Save(m, ImageFormat.Jpeg);
                writer.WriteValue(Convert.ToBase64String(m.ToArray()));
            }
        }
    }
}
