using System.Diagnostics;
using Microsoft.Win32;

namespace EasyImgur
{
    public class RegistryHelper
    {
        private const string AnonymousUploadShellName = "imguruploadanonymous";
        private const string UploadShellName = "imgurupload";
        private const string LaunchAtBootRegistryKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private static readonly string[] FileTypes = { ".jpg", ".jpeg", ".png", ".apng", ".bmp",
            ".gif", ".tiff", ".tif", ".pdf", ".xcf", "Directory" };

        public static bool EnableLaunchAtBoot
        {
            get
            {
                if (Program.InPortableMode) return false;

                using (var registryKey = Registry.CurrentUser.OpenSubKey(LaunchAtBootRegistryKey, true))
                {
                    Debug.Assert(registryKey != null);
                    var value = (string)registryKey.GetValue("EasyImgur", null);

                    if (value != null && value != Program.QuotedApplicationPath)
                    {
                        // A key exists, make sure we're using the most up-to-date path
                        registryKey.SetValue("EasyImgur", Program.QuotedApplicationPath);
                    }

                    return value != null;
                }
            }
            set
            {
                if (Program.InPortableMode) return;

                using (var registryKey = Registry.CurrentUser.OpenSubKey(LaunchAtBootRegistryKey, true))
                {
                    Debug.Assert(registryKey != null);
                    if (value)
                    {
                        registryKey.SetValue("EasyImgur", Program.QuotedApplicationPath);
                    }
                    else
                    {
                        registryKey.DeleteValue("EasyImgur", false);
                    }
                }
            }
        }

        public static void ConfigurationShell(bool enable)
        {
            // a note: Directory doesn't work if within SystemFileAssociations, and 
            // the extensions don't work if not inside them. At least, this seems to be the case for me

            // another note: I discovered that I had the logic flipped, and the code actually did the opposite
            // of what I describe in the above note, and it was working. Earlier, though, when I wrote that,
            // it seemed to be true. Either way, the placement (inside or outside of SystemFileAssociations)
            // does affect where in the context menu they show up. Feel free to play with the placement and see
            // if you can get it to work better.

            using (var root = Registry.CurrentUser.OpenSubKey("Software\\Classes", true))
            {
                Debug.Assert(root != null);
                using (var fileAssoc = root.CreateSubKey("SystemFileAssociations"))
                {
                    Debug.Assert(fileAssoc != null);
                    foreach (var fileType in FileTypes)
                    {
                        using (var fileTypeKey = fileType != "Directory" ? fileAssoc.CreateSubKey(fileType) : root.CreateSubKey(fileType))
                        {
                            Debug.Assert(fileTypeKey != null);
                            using (var shell = fileTypeKey.CreateSubKey("shell"))
                            {
                                Debug.Assert(shell != null);
                                if (enable)
                                {
                                    using (var anonHandler = shell.CreateSubKey(AnonymousUploadShellName))
                                        EnableContextMenu(anonHandler, "Upload to Imgur" +
                                            (fileType == "Directory" ? " as album" : "") + " (anonymous)", true);
                                    using (var accHandler = shell.CreateSubKey(UploadShellName))
                                        EnableContextMenu(accHandler, "Upload to Imgur" +
                                            (fileType == "Directory" ? " as album" : ""), false);
                                }
                                else
                                {
                                    shell.DeleteSubKeyTree(AnonymousUploadShellName, false);
                                    shell.DeleteSubKeyTree(UploadShellName, false);
                                }
                            }
                        }
                    }
                }
            }

        }

        private static void EnableContextMenu(RegistryKey key, string commandText, bool anonymous)
        {
            key.SetValue("", commandText);
            key.SetValue("Icon", Program.QuotedApplicationPath);
            using (var subKey = key.CreateSubKey("command"))
            {
                Debug.Assert(subKey != null);
                subKey.SetValue("", Program.QuotedApplicationPath + (anonymous ? " /anonymous" : "") + " \"%1\"");
            }
        }
    }
}