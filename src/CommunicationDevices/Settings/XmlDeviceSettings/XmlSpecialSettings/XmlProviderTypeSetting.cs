using System.Runtime.InteropServices;


namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings
{

    public enum XmlType {None, XmlTlist, XmlMainWindow, XmlSheduleWindow, XmlStaticWindow }

    public enum DateTimeFormat
    {
        None,
        Sortable,      //2015-07-17T17:04:43
        LinuxTimeStamp
    }



    public class XmlProviderTypeSetting
    {
        #region prop

        public XmlType? XmlType { get; set; }
        public DateTimeFormat DateTimeFormat { get; set; }

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


            if (string.IsNullOrEmpty(timeFormat))
            {
                DateTimeFormat = XmlSpecialSettings.DateTimeFormat.None;
                return;
            }
            if (timeFormat.ToLower().Contains("linuxtimestamp"))
            {
                DateTimeFormat = XmlSpecialSettings.DateTimeFormat.LinuxTimeStamp;
            }
            else
            if (timeFormat.ToLower().Contains("sortable"))
            {
                DateTimeFormat = XmlSpecialSettings.DateTimeFormat.Sortable;
            }
            else
            {
                DateTimeFormat = XmlSpecialSettings.DateTimeFormat.None;
            }
        }

        #endregion
    }
}