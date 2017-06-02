namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings
{

    public enum XmlType {None, XmlTlist }
    
    


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
        }

        #endregion
    }
}