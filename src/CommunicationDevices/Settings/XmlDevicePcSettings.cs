using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Xml.Linq;
using Library.Logs;



namespace CommunicationDevices.Settings
{

    public class XmlDevicePcSettings
    {
        #region prop

        public int Id { get; }
        public string Name { get; }
        //public int PortNumber { get; }
        public string Address { get; }
        public ushort TimeRespone { get;}
        public string Description { get; }


        public BindingType BindingType { get; set; }
        public IEnumerable<byte> PathNumbers { get; }


        #endregion




        #region ctor

        private XmlDevicePcSettings(string id, string name, string address, string timeRespone, string description, string binding)
        {
            Id = int.Parse(id);
            Name = name;
            Address = address;
            TimeRespone = ushort.Parse(timeRespone);
            Description = description;

            if (string.IsNullOrEmpty(binding))
            {
                BindingType= BindingType.None;
            }
            else
            if(binding.ToLower() == "togeneral")
            {
                BindingType = BindingType.ToGeneral;
            }
            else
            if (binding.ToLower() == "toarrivalanddeparture")
            {
                BindingType = BindingType.ToArrivalAndDeparture;
            }
            else
            if (binding.ToLower().Contains("topath:"))
            {
                BindingType = BindingType.ToPath;
                var pathNumbers = new string(binding.SkipWhile(c => c != ':').Skip(1).ToArray()).Split(',');
                PathNumbers= (pathNumbers.First() == String.Empty) ? new List<byte>() : pathNumbers.Select(byte.Parse).ToList();      
            }
        }

        #endregion




        #region Methode

        /// <summary>
        /// Обязательно вызывать в блоке try{}
        /// </summary>
        public static List<XmlDevicePcSettings> LoadXmlSetting(XElement xml)
        {
            var sett =
                from el in xml?.Element("DevicesWithPC")?.Elements("DevicePc")
                select new XmlDevicePcSettings(
                           (string)el.Attribute("Id"),
                           (string)el.Attribute("Name"),
                           (string)el.Attribute("Address"),
                           (string)el.Attribute("TimeRespone"),
                           (string)el.Attribute("Description"),
                           (string)el.Attribute("Binding"));

            return sett.ToList();
        }

        #endregion
    }
}