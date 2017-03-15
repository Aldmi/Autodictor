using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings;
using CommunicationDevices.Settings.XmlDeviceSettings.XmlTransportSettings;



namespace CommunicationDevices.Settings
{
    public static class XmlSettingFactory
    {

        #region Methode

        /// <summary>
        /// Создание списка настроек для устройств с послед. портом
        /// </summary>
        public static List<XmlSpSetting> CreateXmlSpSetting(XElement xml)
        {
            var devSp = xml?.Element("DevicesWithSP")?.Elements("DeviceSp").ToList();
            var listSpSett = new List<XmlSpSetting>();


            if (devSp == null || !devSp.Any())
                return listSpSett;


            foreach (var el in devSp)
            {
                var spSett = new XmlSpSetting(
                                   (string)el.Attribute("Id"),
                                   (string)el.Attribute("Name"),
                                   (string)el.Attribute("Port"),
                                   (string)el.Attribute("Address"),
                                   (string)el.Attribute("TimeRespone"),
                                   (string)el.Attribute("Description"));

                var bind = (string)el.Attribute("Binding");
                if (bind != null)
                {
                    spSett.SpecialDictionary.Add("Binding", new XmlBindingSetting(bind));
                }

                var contrains = (string)el.Attribute("Contrains");
                if (contrains != null)
                {
                    spSett.SpecialDictionary.Add("Contrains", new XmlContrainsSetting(contrains));
                }

                var paging = (string)el.Attribute("Paging");
                if (paging != null)
                {
                    spSett.SpecialDictionary.Add("Paging", new XmlPagingSetting(paging));
                }

                var countRow = (string)el.Attribute("CountRow");
                if (countRow != null)
                {
                    spSett.SpecialDictionary.Add("CountRow", new XmlCountRowSetting(countRow));
                }

                if (el.Element("Settings") != null)
                {
                    var pathPermissionElem = el.Element("Settings")?.Element("PathPermission");
                    if (pathPermissionElem != null)
                    {
                        var pathPermissionEnable= (string)pathPermissionElem.Attribute("Enable");
                        spSett.SpecialDictionary.Add("PathPermission", new XmlPathPermissionSetting(pathPermissionEnable));
                    }
                }



                listSpSett.Add(spSett);
            }

            return listSpSett;
        }



        /// <summary>
        /// Создание списка настроек для устройств подключенных по TCP/Ip
        /// </summary>
        public static List<XmlTcpIpSetting> CreateXmlTcpIpSetting(XElement xml)
        {
            var devTcpIp = xml?.Element("DevicesWithTcpIp")?.Elements("DeviceTcpIp").ToList();
            var listTcpIpSett = new List<XmlTcpIpSetting>();


            if (devTcpIp == null || !devTcpIp.Any())
                return listTcpIpSett;


            foreach (var el in devTcpIp)
            {
                var tcpIpSett = new XmlTcpIpSetting(
                                   (string)el.Attribute("Id"),
                                   (string)el.Attribute("Name"),
                                   (string)el.Attribute("Address"),
                                   (string)el.Attribute("DeviceAddress"),
                                   (string)el.Attribute("TimeRespone"),
                                   (string)el.Attribute("Description"));

                var bind = (string)el.Attribute("Binding");
                if (bind != null)
                {
                    tcpIpSett.SpecialDictionary.Add("Binding", new XmlBindingSetting(bind));
                }

                var contrains = (string)el.Attribute("Contrains");
                if (contrains != null)
                {
                    tcpIpSett.SpecialDictionary.Add("Contrains", new XmlContrainsSetting(contrains));
                }

                var paging = (string)el.Attribute("Paging");
                if (paging != null)
                {
                    tcpIpSett.SpecialDictionary.Add("Paging", new XmlPagingSetting(paging));
                }

                var countRow = (string)el.Attribute("CountRow");
                if (countRow != null)
                {
                    tcpIpSett.SpecialDictionary.Add("CountRow", new XmlCountRowSetting(countRow));
                }

                listTcpIpSett.Add(tcpIpSett);
            }

            return listTcpIpSett;
        }



        /// <summary>
        /// Создание списка настроек для устройств работающих под ОС Windows.
        /// Использующих WCF для обмена данными
        /// </summary>
        public static List<XmlPcSetting> CreateXmlPcSetting(XElement xml)
        {
            var devPc = xml?.Element("DevicesWithPC")?.Elements("DevicePc").ToList();
            var listPcSett = new List<XmlPcSetting>();


            if (devPc == null || !devPc.Any())
                return listPcSett;


            foreach (var el in devPc)
            {
                var pcSett = new XmlPcSetting(
                                   (string)el.Attribute("Id"),
                                   (string)el.Attribute("Name"),
                                   (string)el.Attribute("Address"),
                                   (string)el.Attribute("TimeRespone"),
                                   (string)el.Attribute("Description"));

                var bind = (string)el.Attribute("Binding");
                if (bind != null)
                {
                    pcSett.SpecialDictionary.Add("Binding", new XmlBindingSetting(bind));
                }

                var contrains = (string)el.Attribute("Contrains");
                if (contrains != null)
                {
                    pcSett.SpecialDictionary.Add("Contrains", new XmlContrainsSetting(contrains));
                }

                var paging = (string)el.Attribute("Paging");
                if (paging != null)
                {
                    pcSett.SpecialDictionary.Add("Paging", new XmlPagingSetting(paging));
                }

                var countRow = (string)el.Attribute("CountRow");
                if (countRow != null)
                {
                    pcSett.SpecialDictionary.Add("CountRow", new XmlCountRowSetting(countRow));
                }

                listPcSett.Add(pcSett);
            }

            return listPcSett;
        }

        #endregion

    }
}