namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings
{

    public enum XmlType {None, XmlTlist, XmlMainWindow, XmlSheduleWindow }
    
    


    public class XmlProviderTypeSetting
    {
        #region prop

        public XmlType? XmlType { get; set; }

        #endregion





        #region ctor

        public XmlProviderTypeSetting(string providerType)
        {
            if (string.IsNullOrEmpty(providerType))
            {
                XmlType = null;
            }
            else
            if (providerType.ToLower().Contains("xml_tlist"))
            {
                XmlType= XmlSpecialSettings.XmlType.XmlTlist;            
            }
            else
            if (providerType.ToLower().Contains("xml_mainwindow"))
            {
                XmlType = XmlSpecialSettings.XmlType.XmlMainWindow;
            }
            else
            if (providerType.ToLower().Contains("xml_shedulewindow"))
            {
                XmlType = XmlSpecialSettings.XmlType.XmlSheduleWindow;
            }
        }

        #endregion
    }
}