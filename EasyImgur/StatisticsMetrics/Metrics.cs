using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyImgur.StatisticsMetrics
{
    class MetricHistorySize    : StatisticsMetric
    {
        protected override Object Gather()
        {
            return History.count;
        }
    }

    class MetricHistoryAnonymousUploads    : StatisticsMetric
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
            return System.Environment.OSVersion;
        }
    }

    class MetricCLRVersion : StatisticsMetric
    {
        protected override Object Gather()
        {
            return System.Environment.Version;            
        }
    }

    class MetricLanguageFull : StatisticsMetric
    {
        protected override Object Gather()
        {
            return System.Globalization.CultureInfo.CurrentUICulture.EnglishName;
        }
    }

    class MetricLanguageISO : StatisticsMetric
    {
        protected override Object Gather()
        {
            return System.Globalization.CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName;
        }
    }

    class MetricPortableMode : StatisticsMetric
    {
        protected override Object Gather()
        {
            return EasyImgur.Program.InPortableMode;
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
        private Int64 GetInt64HashCode(string strText)
        {
            Int64 hashCode = 0;
            if (!string.IsNullOrEmpty(strText))
            {
                //Unicode Encode Covering all characterset
                byte[] byteContents = Encoding.Unicode.GetBytes(strText);
                System.Security.Cryptography.SHA256 hash =
                new System.Security.Cryptography.SHA256CryptoServiceProvider();
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
            return (hashCode);
        }

        private string GetUserNameString()
        {
            return System.Environment.UserName;
        }

        private string GetNICString()
        {
            // Use the physical address (MAC) of the first network interface that has a non-null MAC address.
            System.Net.NetworkInformation.NetworkInterface[] nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            foreach (System.Net.NetworkInformation.NetworkInterface nic in nics)
            {
                System.Net.NetworkInformation.PhysicalAddress mac = nic.GetPhysicalAddress();
                if (mac != System.Net.NetworkInformation.PhysicalAddress.None)
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

        private string GetPropertiesOfWMIObjects(string _QueryTarget, string _PropertyName)
        {
            try
            {
                System.Management.ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher("SELECT * FROM " + _QueryTarget);

                string propertystring = string.Empty;
                foreach (System.Management.ManagementObject wmi_object in searcher.Get())
                {
                    Object property = wmi_object[_PropertyName];
                    if (property != null)
                        propertystring += property.ToString();
                }

                return propertystring;
            }
            catch (System.Exception ex)
            {
                // Do nothing except log.
                Log.Error("An exception occurred while trying to obtain some WMI object properties: " + ex.Message);
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
