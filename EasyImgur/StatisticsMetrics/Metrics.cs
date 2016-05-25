using System;
using System.Globalization;
using System.Management;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace EasyImgur.StatisticsMetrics
{
    class MetricHistorySize : StatisticsMetric
    {
        protected override object Gather()
        {
            return History.Count;
        }
    }

    class MetricHistoryAnonymousUploads : StatisticsMetric
    {
        protected override object Gather()
        {
            return History.AnonymousCount;
        }
    }

    class MetricAuthorized : StatisticsMetric
    {
        protected override object Gather()
        {
            return ImgurAPI.HasBeenAuthorized();
        }
    }

    class MetricOperatingSystem : StatisticsMetric
    {
        protected override object Gather()
        {
            return Environment.OSVersion;
        }
    }

    class MetricClrVersion : StatisticsMetric
    {
        protected override object Gather()
        {
            return Environment.Version;
        }
    }

    class MetricLanguageFull : StatisticsMetric
    {
        protected override object Gather()
        {
            return CultureInfo.CurrentUICulture.EnglishName;
        }
    }

    class MetricLanguageIso : StatisticsMetric
    {
        protected override object Gather()
        {
            return CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName;
        }
    }

    class MetricPortableMode : StatisticsMetric
    {
        protected override object Gather()
        {
            return Program.InPortableMode;
        }
    }

    class MetricMachineId : StatisticsMetric
    {
        protected override object Gather()
        {
            // The way we attempt to identify a unique machine is by combining and hashing a number of parameters.
            // The reason why we need more than one is that none of these individually can reliably tell us 
            // some interesting. We hope that by combining them all we increase our chances of identifying
            // the same machine every time this metric is gathered on that machine (and without it clashing with other machines).
            // Unfortunately the way this is set up means that if any of these change, we will no longer be able to uniquely identify
            // the machine (for example, if the users replaces their CPU).

            return GetInt64HashCode(GetUserNameString() + GetNicString() + GetBaseBoardString() + GetCpuSerialNumberString() + GetHardDriveSerialNumberString());
        }

        // Code obtained from http://www.codeproject.com/Articles/34309/Convert-String-to-64bit-Integer (Accessed 13-03-2015 @ 23:25).
        private static long GetInt64HashCode(string strText)
        {
            long hashCode = 0;
            if (!string.IsNullOrEmpty(strText))
            {
                //Unicode Encode Covering all characterset
                var byteContents = Encoding.Unicode.GetBytes(strText);
                SHA256 hash = new SHA256CryptoServiceProvider();
                var hashText = hash.ComputeHash(byteContents);
                //32Byte hashText separate
                //hashCodeStart = 0~7  8Byte
                //hashCodeMedium = 8~23  8Byte
                //hashCodeEnd = 24~31  8Byte
                //and Fold
                var hashCodeStart = BitConverter.ToInt64(hashText, 0);
                var hashCodeMedium = BitConverter.ToInt64(hashText, 8);
                var hashCodeEnd = BitConverter.ToInt64(hashText, 24);
                hashCode = hashCodeStart ^ hashCodeMedium ^ hashCodeEnd;
            }
            return hashCode;
        }

        private static string GetUserNameString()
        {
            return Environment.UserName;
        }

        private static string GetNicString()
        {
            // Use the physical address (MAC) of the first network interface that has a non-null MAC address.
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var nic in nics)
            {
                var mac = nic.GetPhysicalAddress();
                if (!mac.Equals(PhysicalAddress.None))
                    return mac.ToString();
            }

            return string.Empty;
        }

        private static string GetBaseBoardString()
        {
            return GetPropertiesOfWmiObjects("Win32_BaseBoard", "SerialNumber");
        }

        private static string GetCpuSerialNumberString()
        {
            return GetPropertiesOfWmiObjects("Win32_Processor", "ProcessorId");
        }

        private static string GetHardDriveSerialNumberString()
        {
            return GetPropertiesOfWmiObjects("Win32_PhysicalMedia", "SerialNumber");
        }

        private static string GetPropertiesOfWmiObjects(string queryTarget, string propertyName)
        {
            try
            {
                var sb = new StringBuilder();

                using (var searcher = new ManagementObjectSearcher("SELECT * FROM " + queryTarget))
                using (var wmiCollection = searcher.Get())
                {
                    foreach (var wmiObject in wmiCollection)
                    {
                        var property = wmiObject[propertyName];
                        if (property != null)
                            sb.Append(property);
                    }
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                // Do nothing except log.
                Log.Error("An exception occurred while trying to obtain some WMI object properties: " + ex);
            }

            return string.Empty;
        }
    }

    class MetricVersion : StatisticsMetric
    {
        protected override object Gather()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
