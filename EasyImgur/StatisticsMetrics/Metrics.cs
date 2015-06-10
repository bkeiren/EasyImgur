using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace EasyImgur.StatisticsMetrics
{
    class MetricHistorySize : StatisticsMetric
    {
        protected override Object Gather()
        {
            return History.count;
        }
    }

    class MetricHistoryAnonymousUploads : StatisticsMetric
    {
        protected override Object Gather()
        {
            return History.anonymousCount;
        }
    }

    class MetricAuthorized : StatisticsMetric
    {
        protected override Object Gather()
        {
            return ImgurAPI.HasBeenAuthorized();
        }
    }

    class MetricOperatingSystem : StatisticsMetric
    {
        protected override Object Gather()
        {
            return Environment.OSVersion;
        }
    }

    class MetricCLRVersion : StatisticsMetric
    {
        protected override Object Gather()
        {
            return Environment.Version;            
        }
    }

    class MetricLanguageFull : StatisticsMetric
    {
        protected override Object Gather()
        {
            return CultureInfo.CurrentUICulture.EnglishName;
        }
    }

    class MetricLanguageISO : StatisticsMetric
    {
        protected override Object Gather()
        {
            return CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName;
        }
    }

    class MetricPortableMode : StatisticsMetric
    {
        protected override Object Gather()
        {
            return Program.InPortableMode;
        }
    }

    class MetricMachineID : StatisticsMetric
    {
        protected override Object Gather()
        {
            // The way we attempt to identify a unique machine is by combining and hashing a number of parameters.
            // The reason why we need more than one is that none of these individually can reliably tell us 
            // some interesting. We hope that by combining them all we increase our chances of identifying
            // the same machine every time this metric is gathered on that machine (and without it clashing with other machines).
            // Unfortunately the way this is set up means that if any of these change, we will no longer be able to uniquely identify
            // the machine (for example, if the users replaces their CPU).

            return GetInt64HashCode(GetUserNameString() + GetNICString() + GetBaseBoardString() + GetCPUSerialNumberString() + GetHardDriveSerialNumberString());
        }

        // Code obtained from http://www.codeproject.com/Articles/34309/Convert-String-to-64bit-Integer (Accessed 13-03-2015 @ 23:25).
        private static Int64 GetInt64HashCode(string strText)
        {
            Int64 hashCode = 0;
            if (!string.IsNullOrEmpty(strText))
            {
                //Unicode Encode Covering all characterset
                byte[] byteContents = Encoding.Unicode.GetBytes(strText);
                SHA256 hash = new SHA256CryptoServiceProvider();
                byte[] hashText = hash.ComputeHash(byteContents);
                //32Byte hashText separate
                //hashCodeStart = 0~7  8Byte
                //hashCodeMedium = 8~23  8Byte
                //hashCodeEnd = 24~31  8Byte
                //and Fold
                Int64 hashCodeStart = BitConverter.ToInt64(hashText, 0);
                Int64 hashCodeMedium = BitConverter.ToInt64(hashText, 8);
                Int64 hashCodeEnd = BitConverter.ToInt64(hashText, 24);
                hashCode = hashCodeStart ^ hashCodeMedium ^ hashCodeEnd;
            }
            return hashCode;
        }

        private string GetUserNameString()
        {
            return Environment.UserName;
        }

        private string GetNICString()
        {
            // Use the physical address (MAC) of the first network interface that has a non-null MAC address.
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface nic in nics)
            {
                PhysicalAddress mac = nic.GetPhysicalAddress();
                if (!mac.Equals(PhysicalAddress.None))
                    return mac.ToString();
            }

            return string.Empty;
        }

        private string GetBaseBoardString()
        {
            return GetPropertiesOfWMIObjects("Win32_BaseBoard", "SerialNumber");
        }

        private string GetCPUSerialNumberString()
        {
            return GetPropertiesOfWMIObjects("Win32_Processor", "ProcessorId");
        }

        private string GetHardDriveSerialNumberString()
        {
            return GetPropertiesOfWMIObjects("Win32_PhysicalMedia", "SerialNumber");
        }

        private string GetPropertiesOfWMIObjects(string queryTarget, string propertyName)
        {
            try
            {
                var sb = new StringBuilder();

                using (var searcher = new ManagementObjectSearcher("SELECT * FROM " + queryTarget))
                using (ManagementObjectCollection wmiCollection = searcher.Get())
                {
                    foreach (ManagementBaseObject wmiObject in wmiCollection)
                    {
                        Object property = wmiObject[propertyName];
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
        protected override Object Gather()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
