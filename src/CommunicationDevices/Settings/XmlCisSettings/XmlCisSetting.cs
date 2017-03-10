using System.Xml.Linq;

namespace CommunicationDevices.Settings.XmlCisSettings
{

    public class XmlCisSetting
    {
        #region prop

        public string Name { get; }
        public string EndpointAddress { get; }

        #endregion




        #region ctor

        private XmlCisSetting(string name, string endpointAddress)
        {
            Name = name;
            EndpointAddress = endpointAddress;
        }

        #endregion




        #region Methode

        /// <summary>
        /// Обязательно вызывать в блоке try{}
        /// </summary>
        public static XmlCisSetting LoadXmlSetting(XElement xml)
        {
            return new XmlCisSetting(
                           (string)xml?.Element("CisSetting")?.Element("Name"),
                           (string)xml?.Element("CisSetting")?.Element("EndpointAddress")
                          );
        }

        #endregion
    }
}