using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Updater
{
    class FeedInfo
    {
        public class VersionInfo
        {
            [JsonProperty("major")]
            public int Major { get; set; }
            
            [JsonProperty("minor")]
            public int Minor { get; set; }
            
            [JsonProperty("patch")]
            public int Patch { get; set; }
        }

        [JsonProperty("version")]
        public VersionInfo Version { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("flags")]
        public List<Flag> Flags { get; set; }
    }

    class JsonFeed : IFeed
    {
        bool IFeed.Parse(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                info = null;
                string jsonString = reader.ReadToEnd();
                try
                {
                    info = JsonConvert.DeserializeObject<FeedInfo>(jsonString);
                }
                catch
                {
                    // Do nothing because we don't care if it doesn't work.
                }
                return info != null;
            }
        }

        private FeedInfo info = new FeedInfo();

        public int VersionMajor 
        {
            get
            {
                return info.Version.Major;
            }
        }

        public int VersionMinor 
        {
            get
            {
                return info.Version.Minor;
            }
        }

        public int VersionPatch
        {
            get
            {
                return info.Version.Patch;
            }
        }

        public string Link
        {
            get
            {
                return info.Link;
            }
        }

        public bool HasFlag(Flag flag)
        {
            foreach (Flag stored_flag in info.Flags)
                if (stored_flag == flag)
                    return true;
            return false;
        }
    }
}
