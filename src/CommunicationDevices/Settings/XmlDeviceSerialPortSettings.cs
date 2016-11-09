using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Library.Logs;



namespace CommunicationDevices.Settings
{
    public class XmlDeviceSerialPortSettings
    {
        #region prop

        public int Id { get; }
        public string Name { get; }
        public int PortNumber { get; }
        public byte Address { get; }
        public ushort TimeRespone { get;}
        public string Description { get; }


        #endregion




        #region ctor

        private XmlDeviceSerialPortSettings(string id, string name, string port, string address, string timeRespone, string description)
        {
            Id = int.Parse(id);
            Name = name;
            PortNumber = int.Parse(port);
            Address = byte.Parse(address);
            TimeRespone = ushort.Parse(timeRespone);
            Description = description;
        }

        #endregion




        #region Methode

        /// <summary>
        /// Обязательно вызывать в блоке try{}
        /// </summary>
        public static List<XmlDeviceSerialPortSettings> LoadXmlSetting(XElement xml)
        {
            var sett =
                from el in xml?.Element("DevicesWithSP")?.Elements("DeviceSp")
                select new XmlDeviceSerialPortSettings(
                           (string)el.Attribute("Id"),
                           (string)el.Attribute("Name"),
                           (string)el.Attribute("Port"),
                           (string)el.Attribute("Address"),
                           (string)el.Attribute("TimeRespone"),
                           (string)el.Attribute("Description"));

            return sett.ToList();
        }


        //public static List<XmlCashierSettings> LoadXmlSetting(XElement xml)
        //{
        //    var sett =
        //        from el in xml?.Element("Cashiers")?.Elements("Cashier")
        //        select new XmlCashierSettings(
        //                   (string)el.Attribute("Id"),
        //                   (string)el.Attribute("PortNumber"),
        //                   (string)el.Attribute("Prefix"),
        //                   (string)el.Attribute("MaxCountTryHanding"));

        //    return sett.ToList();
        //}

        #endregion
    }
}