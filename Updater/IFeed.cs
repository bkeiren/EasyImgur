using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Updater
{
    enum Flag
    {
        NotBackwardsCompatible
    }

    interface IFeed
    {
        bool Parse(Stream stream);
        
        int VersionMajor { get; }
        int VersionMinor { get; }
        int VersionPatch { get; }
        string Link { get; }
        bool HasFlag(Flag flag);
    }
}
