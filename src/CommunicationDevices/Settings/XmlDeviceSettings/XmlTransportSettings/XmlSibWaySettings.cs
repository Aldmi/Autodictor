using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Communication.SibWayApi;
using CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings;


namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlTransportSettings
{
    public class XmlSibWaySettings
    {
        #region prop

        //Настройки Exchange
        public int Id { get; set; }
        public long Period { get; set; }
        public string Description { get; set; }
        public Dictionary<string, object> SpecialDictionary { get; set; } = new Dictionary<string, object>(); //Специальные настройки ус-ва
        
        //Настройки SibWay api
        public SettingSibWay SettingSibWay { get; set; }  

        #endregion




        #region Methode

        /// <summary>
        /// Обязательно вызывать в блоке try{}
        /// </summary>
        public static List<XmlSibWaySettings> LoadXmlSetting(XElement xml)
        {
            var devSibWay = xml?.Element("DevicesWithSibWayApi")?.Elements("SibWay").ToList();
            var listHttpSett = new List<XmlSibWaySettings>();


            if (devSibWay == null || !devSibWay.Any())
                return listHttpSett;


            foreach (var el in devSibWay)
            {
                var id= (string) el.Attribute("Id");
                var ip= (string) el.Attribute("Ip");
                var port= (string) el.Attribute("Port");
                var path2FontFile= (string) el.Attribute("Path2FontFile");
                var fontSize= (string) el.Attribute("FontSize");
                var period= (string) el.Attribute("Period");
                var timeDelayReconnect= (string) el.Attribute("TimeDelayReconnect");
                var timeRespone= (string) el.Attribute("TimeRespone");
                var description= (string) el.Attribute("Description");

                var xmlSibWaySett = new XmlSibWaySettings
                {
                    Id= int.Parse(id),
                    Description= description,
                    Period= long.Parse(period),
                    SettingSibWay= new SettingSibWay(ip, port, path2FontFile, fontSize, timeRespone, timeDelayReconnect)
                };

                var bind= (string)el.Attribute("Binding");
                if (bind != null)
                {
                    xmlSibWaySett.SpecialDictionary.Add("Binding", new XmlBindingSetting(bind));
                }

                var contrains= (string)el.Attribute("Contrains");
                if (contrains != null)
                {
                    xmlSibWaySett.SpecialDictionary.Add("Contrains", new XmlConditionsSetting(contrains));
                }

                var paging= (string)el.Attribute("Paging");
                if (paging != null)
                {
                    xmlSibWaySett.SpecialDictionary.Add("Paging", new XmlPagingSetting(paging));
                }


                //ЭКРАНЫ-----------------
                var screens= el.Elements("Screen").ToList();
                foreach (var scr in screens)
                {
                    var number= (string)scr.Attribute("Number");
                    var columnName= (string)scr.Attribute("ColumnName");
                    var width = (string)scr.Attribute("Width");
                    var height = (string)scr.Attribute("Height");
                    var effect = (string)scr.Attribute("Effect");
                    var textHAlign = (string)scr.Attribute("TextHAlign");
                    var textVAlign = (string)scr.Attribute("TextVAlign");
                    var displayTime = (string)scr.Attribute("DisplayTime");
                    var delayBetweenSending = (string)scr.Attribute("DelayBetweenSending");
                    var colorBytes = (string)scr.Attribute("ColorBytes");

                    xmlSibWaySett.SettingSibWay.WindowSett= new List<WindowSett>
                    {
                        new WindowSett(number, columnName, width, height, effect, textHAlign, textVAlign, displayTime, delayBetweenSending, colorBytes)
                    };
                }
            }



            //foreach (var el in devHttp)
        //{
        //    var httpSett = new XmlHttpSetting(
        //        (string)el.Attribute("Id"),
        //        (string)el.Attribute("Name"),
        //        (string)el.Attribute("Address"),
        //        (string)el.Attribute("Period"),
        //        (string)el.Attribute("TimeRespone"),
        //        (string)el.Attribute("Description"));

        //    var bind = (string)el.Attribute("Binding");
        //    if (bind != null)
        //    {
        //        httpSett.SpecialDictionary.Add("Binding", new XmlBindingSetting(bind));
        //    }

        //    var contrains = (string)el.Attribute("Contrains");
        //    if (contrains != null)
        //    {
        //        httpSett.SpecialDictionary.Add("Contrains", new XmlConditionsSetting(contrains));
        //    }

        //    var paging = (string)el.Attribute("Paging");
        //    if (paging != null)
        //    {
        //        httpSett.SpecialDictionary.Add("Paging", new XmlPagingSetting(paging));
        //    }

        //    var countRow = (string)el.Attribute("CountRow");
        //    if (countRow != null)
        //    {
        //        httpSett.SpecialDictionary.Add("CountRow", new XmlCountRowSetting(countRow));
        //    }

        //    var providerType = (string)el.Attribute("ProviderType");
        //    if (providerType != null)
        //    {
        //        httpSett.SpecialDictionary.Add("ProviderType", new XmlProviderTypeSetting(providerType));
        //    }





        //    var headers = (string)el.Attribute("Headers");
        //    if (headers != null)
        //    {
        //        var pair = headers.Split('+');
        //        foreach (var p in pair)
        //        {
        //            if (!p.Contains(":"))
        //                continue;

        //            var keyValue = p.Split(':');
        //            if (keyValue.Length == 2)
        //            {
        //                httpSett.Headers.Add(keyValue[0].Trim(), keyValue[1].Trim());
        //            }
        //        }
        //    }

        //    listHttpSett.Add(httpSett);
        //}

        //return listHttpSett;


        return null;
    }
        #endregion
    }
}