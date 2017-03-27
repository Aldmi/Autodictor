namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings
{
    public class XmlExchangeRule
    {

        #region prop

        public string Format { get; set; }

        public int? RequestMaxLenght { get; set; }
        public string RequestBody { get; set; }

        public int? ResponseMaxLenght { get; set; }
        public string ResponseBody { get; set; }
        public int TimeResponse { get; set; }

        public int? RepeatCount { get; set; }
        public int? RepeatDeltaX { get; set; }
        public int? RepeatDeltaY { get; set; }

        #endregion





        #region ctor

        public XmlExchangeRule()
        {
            
        }

       #endregion
    }
}