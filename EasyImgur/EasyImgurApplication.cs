using System;
using EasyImgur.Properties;

namespace EasyImgur
{
    public class EasyImgurApplication : IDisposable
    {
        public void Initialize()
        {
            if (Settings.Default.enableContextMenu)
            {
                RegistryHelper.ConfigurationShell(true);
            }
        }

        public void Dispose()
        {
            if (Program.InPortableMode)
            {
                RegistryHelper.ConfigurationShell(false);
            }
        }
    }
}