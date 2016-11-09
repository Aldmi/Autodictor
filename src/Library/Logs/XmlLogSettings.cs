using System.Xml.Linq;

namespace Library.Logs
{
    public class XmlLogSettings
    {
        #region prop

        public int PortionString { get; }
        public int CountPortion { get; }

        #endregion




        #region ctor

        private XmlLogSettings(string portionString, string countPortion)
        {
            PortionString = int.Parse(portionString);
            CountPortion = int.Parse(countPortion);
        }

        #endregion




        #region Methode

        /// <summary>
        /// Обязательно вызывать в блоке try{}
        /// </summary>
        public static XmlLogSettings LoadXmlSetting(XElement xml)
        {
            XmlLogSettings settings =
                new XmlLogSettings(
                    (string) xml?.Element("LogSetting")?.Element("PortionString"),
                    (string) xml?.Element("LogSetting")?.Element("CountPortion") );
   
            return settings;
        }

        #endregion
    }
}