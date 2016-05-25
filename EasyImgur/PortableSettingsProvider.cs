// PortableSettingsProvider code obtained from http://stackoverflow.com/a/2579399 (Accessed 02-01-2014 @ 17:04).

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace EasyImgur
{
    public class PortableSettingsProvider : SettingsProvider
    {
        /// <summary>
        /// XML root node name.
        /// </summary>
        private const string SettingsRootNode = "Settings";
        /// <summary>
        /// File name of the settings file.
        /// </summary>
        private readonly string fileName;

        private XmlDocument settingsXml;

        private XmlDocument SettingsXml
        {
            get
            {
                //If we dont hold an xml document, try opening one.  
                //If it doesnt exist then create a new one ready.
                if (this.settingsXml != null)
                    return this.settingsXml;

                this.settingsXml = new XmlDocument();
                try
                {
                    this.settingsXml.Load(Path.Combine(this.GetAppSettingsPath(), this.GetAppSettingsFilename()));
                }
                catch (Exception)
                {
                    //Create new document
                    XmlDeclaration dec = this.settingsXml.CreateXmlDeclaration("1.0", "utf-8", "");
                    this.settingsXml.AppendChild(dec);

                    XmlNode nodeRoot = this.settingsXml.CreateNode(XmlNodeType.Element, SettingsRootNode, "");
                    this.settingsXml.AppendChild(nodeRoot);
                }

                return this.settingsXml;
            }
        }

        public PortableSettingsProvider(string fileName)
        {
            this.fileName = fileName;
        }

        public override void Initialize(string name, NameValueCollection col)
        {
            base.Initialize(this.ApplicationName, col);
        }

        public override string ApplicationName
        {
            get
            {
                if (Application.ProductName.Trim().Length > 0)
                {
                    return Application.ProductName;
                }
                var fi = new FileInfo(Application.ExecutablePath);
                return fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length);
            }
            set { } // Do nothing
        }

        public override string Name
        {
            get { return "PortableSettingsProvider"; }
        }

        /// <summary>
        /// Used to determine the filename to store the settings
        /// </summary>
        /// <returns></returns>
        public virtual string GetAppSettingsFilename()
        {
            //return ApplicationName + ".settings";
            return this.fileName;
        }

        /// <summary>
        /// Used to determine where to store the settings
        /// </summary>
        /// <returns></returns>
        public virtual string GetAppSettingsPath()
        {
            var fi = new FileInfo(Application.ExecutablePath);
            return fi.DirectoryName;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection propvals)
        {
            //Iterate through the settings to be stored
            //Only dirty settings are included in propvals, and only ones relevant to this provider
            foreach (SettingsPropertyValue propval in propvals)
            {
                this.SetValue(propval);
            }

            try
            {
                this.SettingsXml.Save(Path.Combine(this.GetAppSettingsPath(), this.GetAppSettingsFilename()));
            }
            catch (Exception ex)
            {
                Log.Error("Exception in PortableSettingsProvider: " + ex);
            }
            //Ignore if cant save, device been ejected
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection props)
        {
            //Create new collection of values
            var values = new SettingsPropertyValueCollection();

            //Iterate through the settings to be retrieved
            foreach (SettingsProperty setting in props)
            {
                var value = new SettingsPropertyValue(setting);
                value.IsDirty = false;
                value.SerializedValue = this.GetValue(setting);
                values.Add(value);
            }
            return values;
        }

        private string GetValue(SettingsProperty setting)
        {
            string ret = "";

            try
            {
                if (IsRoaming(setting))
                {
                    ret = this.SettingsXml.SelectSingleNode(SettingsRootNode + "/" + setting.Name).InnerText;
                }
                else
                {
                    ret = this.SettingsXml.SelectSingleNode(SettingsRootNode + "/" + Environment.MachineName + "/" + setting.Name).InnerText;
                }
            }

            catch (Exception)
            {
                ret = (setting.DefaultValue != null) ? setting.DefaultValue.ToString() : "";
            }

            return ret;
        }

        private void SetValue(SettingsPropertyValue propVal)
        {
            XmlElement settingNode;

            //Determine if the setting is roaming.
            //If roaming then the value is stored as an element under the root
            //Otherwise it is stored under a machine name node 
            try
            {
                if (IsRoaming(propVal.Property))
                {
                    settingNode = (XmlElement)this.SettingsXml.SelectSingleNode(SettingsRootNode + "/" + propVal.Name);
                }
                else
                {
                    settingNode = (XmlElement)this.SettingsXml.SelectSingleNode(SettingsRootNode + "/" + Environment.MachineName + "/" + propVal.Name);
                }
            }
            catch (Exception)
            {
                settingNode = null;
            }

            //Check to see if the node exists, if so then set its new value
            if ((settingNode != null))
            {
                settingNode.InnerText = propVal.SerializedValue.ToString();
            }
            else
            {
                if (IsRoaming(propVal.Property))
                {
                    //Store the value as an element of the Settings Root Node
                    settingNode = this.SettingsXml.CreateElement(propVal.Name);
                    settingNode.InnerText = propVal.SerializedValue.ToString();
                    this.SettingsXml.SelectSingleNode(SettingsRootNode).AppendChild(settingNode);
                }
                else
                {
                    // It's machine specific, store as an element of the machine name node,
                    // creating a new machine name node if one doesnt exist.
                    XmlElement machineNode;
                    try
                    {

                        machineNode = (XmlElement)this.SettingsXml.SelectSingleNode(SettingsRootNode + "/" + Environment.MachineName);
                    }
                    catch (Exception)
                    {
                        machineNode = this.SettingsXml.CreateElement(Environment.MachineName);
                        this.SettingsXml.SelectSingleNode(SettingsRootNode).AppendChild(machineNode);
                    }

                    if (machineNode == null)
                    {
                        machineNode = this.SettingsXml.CreateElement(Environment.MachineName);
                        this.SettingsXml.SelectSingleNode(SettingsRootNode).AppendChild(machineNode);
                    }

                    settingNode = this.SettingsXml.CreateElement(propVal.Name);
                    settingNode.InnerText = propVal.SerializedValue.ToString();
                    machineNode.AppendChild(settingNode);
                }
            }
        }

        private static bool IsRoaming(SettingsProperty prop)
        {
            // If running in portable mode, all settings must be NON-machine specific and must thus be considered roaming.
            if (Program.InPortableMode)
                return true;

            //Determine if the setting is marked as Roaming
            foreach (DictionaryEntry d in prop.Attributes)
            {
                var a = (Attribute)d.Value;
                if (a is SettingsManageabilityAttribute)
                {
                    return true;
                }
            }
            return false;
        }
    }
}