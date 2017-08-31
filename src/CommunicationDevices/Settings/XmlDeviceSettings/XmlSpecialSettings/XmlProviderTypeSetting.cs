using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings
{

    public enum XmlType {None, XmlTlist, XmlMainWindow, XmlSheduleWindow, XmlStaticWindow, XmlChange, XmlApkDkMoscow }

    public enum DateTimeFormat
    {
        None,
        Sortable,      //формат: 2015-07-17T17:04:43
        LinuxTimeStamp
    }



    public class XmlProviderTypeSetting
    {
        #region prop

        public XmlType? XmlType { get; set; }
        public DateTimeFormat DateTimeFormat { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
        public int EcpCode { get; set; }

        #endregion





        #region ctor

        public XmlProviderTypeSetting(string providerType)
        {
            if (string.IsNullOrEmpty(providerType))
            {
                XmlType = null;
                return;
            }

            var providerSettings= providerType.Split(':');
            var providerName = providerSettings[0];
            var timeFormat = (providerSettings.Length > 1) ? providerSettings[1] : null;

            if (providerName.ToLower().Contains("xml_tlist"))
            {
                XmlType= XmlSpecialSettings.XmlType.XmlTlist;            
            }
            else
            if (providerName.ToLower().Contains("xml_mainwindow"))
            {
                XmlType = XmlSpecialSettings.XmlType.XmlMainWindow;
            }
            else
            if (providerName.ToLower().Contains("xml_shedulewindow"))
            {
                XmlType = XmlSpecialSettings.XmlType.XmlSheduleWindow;
            }
            else
            if (providerName.ToLower().Contains("xml_staticwindow"))
            {
                XmlType = XmlSpecialSettings.XmlType.XmlStaticWindow;
            }
            else
            if (providerName.ToLower().Contains("xml_change"))
            {
                XmlType = XmlSpecialSettings.XmlType.XmlChange;
            }
            else
            if (providerName.ToLower().Contains("xml_apkdkmoscow"))
            {
                XmlType = XmlSpecialSettings.XmlType.XmlApkDkMoscow;
                //парсим информацию в скоюках.
                var regex = Regex.Match(providerName, @"\((.*?)\)"); //@"\((.*?)\)"
                string value = regex.Groups[1].Value;
                if (!string.IsNullOrEmpty(value))
                {
                    var agruments = value.Split(',');
                    if (agruments.Length == 3)
                    {
                        Login = agruments[0];
                        Password = agruments[1];
                        EcpCode = int.Parse(agruments[2]);
                    }
                }
            }


            if (string.IsNullOrEmpty(timeFormat))
            {
                DateTimeFormat = DateTimeFormat.None;
                return;
            }
            if (timeFormat.ToLower().Contains("linuxtimestamp"))
            {
                DateTimeFormat = DateTimeFormat.LinuxTimeStamp;
            }
            else
            if (timeFormat.ToLower().Contains("sortable"))
            {
                DateTimeFormat = DateTimeFormat.Sortable;
            }
            else
            {
                DateTimeFormat = DateTimeFormat.None;
            }
        }

        #endregion
    }
}