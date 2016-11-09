using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Xml.Linq;
using Communication.Annotations;

namespace Communication.Settings
{
    public class XmlSerialSettings
    {
        #region prop

        public string Port { get; }
        public int BaudRate { get; }
        public int DataBits { get; }
        public StopBits StopBits { get; set; }

        #endregion




        #region ctor

        private XmlSerialSettings(string port, string baudRate, string dataBits, string stopBits)
        {
            Port = port;
            BaudRate = int.Parse(baudRate);
            DataBits = int.Parse(dataBits);
            StopBits = (int.Parse(stopBits) == 1) ? StopBits.One : StopBits.Two;
        }

        #endregion




        #region Methode

        /// <summary>
        /// Обязательно вызывать в блоке try{}
        /// </summary>
        public static List<XmlSerialSettings> LoadXmlSetting(XElement xml)
        {
            var sett = from el in xml?.Element("SerialPorts")?.Elements("Serial")
                       select new XmlSerialSettings(
                         (string)el.Element("Port"),
                         (string)el.Element("BaudRate"),
                         (string)el.Element("DataBits"),
                         (string)el.Element("StopBits") );
                        

            return sett.ToList();
        }

        #endregion
    }
}