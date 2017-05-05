namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings
{

    public enum XmlType {None, XmlTlist }
    
    


    public class XmlSendingTypeSetting
    {
        #region prop

        public XmlType? XmlType { get; set; }

        #endregion





        #region ctor

        public XmlSendingTypeSetting(string sendingType)
        {
            if (string.IsNullOrEmpty(sendingType))
            {
                XmlType = null;
            }
            else
            if (sendingType.ToLower().Contains("xml_tlist"))
            {
                XmlType= XmlSpecialSettings.XmlType.XmlTlist;            
            }
        }

        #endregion
    }
}