using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CommunicationDevices.DataProviders.XmlDataProvider.XMLFormatProviders
{

//<? xml version="1.0" encoding="utf-8" standalone="yes"?>
//<mainWindow>
//  <t>
//    <TrainNumber>6396</TrainNumber>
//    <TrainType>0</TrainType>
//    <StartStation>Крюково</StartStation>
//    <EndStation> </EndStation>
//    <RecDateTime></RecDateTime>
//    <SndDateTime>2017-06-17T00:34:00</SndDateTime>
//    <EvRecTime></EvRecTime>
//    <EvSndTime>2017-06-17T00:34:00</EvSndTime>
//    <TrackNumber></TrackNumber>
//    <Direction>1</Direction>
//    <EvTrackNumber></EvTrackNumber>
//    <State>0</State>
//    <VagonDirection>0</VagonDirection>
//    <Enabled>1</Enabled>
//	<TypeName>Пригородный</TypeName>
//	<TypeAlias>приг</TypeAlias>
//	<Addition>Поле дополнения</Addition>
//  </t>
//</mainWindow>



    public class XmlMainWindowFormatProvider : IFormatProvider
    {
        public string CreateDoc(IEnumerable<UniversalInputType> tables)
        {
            if (tables == null || !tables.Any())
                return null;

            var xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), new XElement("tlist"));
            foreach (var uit in tables)
            {
                string trainType = String.Empty;
                string typeName = String.Empty;
                string typeNameShort = String.Empty;
                switch (uit.TypeTrain)
                {
                    case TypeTrain.None:
                        trainType = String.Empty;
                        typeName = String.Empty;
                        break;

                    case TypeTrain.Suburban:
                        trainType = "0";
                        typeName = "Пригородный";
                        typeNameShort = "приг";
                        break;

                    case TypeTrain.Express:
                        trainType = "1";
                        typeName = "Экспресс";
                        typeNameShort = "экспресс";
                        break;

                    case TypeTrain.HighSpeed:
                        trainType = "2";
                        typeName = "Скорый";
                        typeNameShort = "скор";
                        break;

                    case TypeTrain.Corporate:
                        trainType = "3";
                        typeName = "Фирменный";
                        typeNameShort = "фирм";
                        break;

                    case TypeTrain.Passenger:
                        trainType = "4";
                        typeName = "Пассажирский";
                        typeNameShort = "пасс";
                        break;

                    case TypeTrain.Swallow:
                        trainType = "5";
                        typeName = "Скоростной";
                        typeNameShort = "скоростной";
                        break;

                    case TypeTrain.Rex:
                        trainType = "5";
                        typeName = "Скоростной";
                        typeNameShort = "скоростной";
                        break;
                }

                string startSt;
                string endSt;
                var stations = uit.Stations.Split('-').Select(s => s.Trim()).ToList();
                if (stations.Count == 2)
                {
                    startSt = stations[0];
                    endSt = stations[1];
                }
                else
                {
                    startSt = (uit.Event == "ОТПР.") ? stations[0] : " ";
                    endSt = (uit.Event == "ПРИБ.") ? stations[0] : " ";
                }

                var timeArrival = string.Empty;
                var timeDepart = string.Empty;
                byte direction = 0;
                switch (uit.Event)
                {
                    case "ПРИБ.":
                        timeArrival = uit.Time.ToString("s");
                        direction = 0;
                        break;

                    case "ОТПР.":
                        timeDepart = uit.Time.ToString("s");
                        direction = 1;
                        break;

                    case "СТОЯНКА":
                        timeArrival = uit.TransitTime.ContainsKey("приб") ? uit.TransitTime["приб"].ToString("s") : String.Empty;
                        timeDepart = uit.TransitTime.ContainsKey("отпр") ? uit.TransitTime["отпр"].ToString("s") : String.Empty;
                        direction = 2;
                        break;
                }


                xDoc.Root?.Add(
                        new XElement("t",
                        new XElement("TrainNumber", uit.NumberOfTrain),
                        new XElement("TrainType", trainType),
                        new XElement("StartStation", startSt),
                        new XElement("EndStation", endSt),
                        new XElement("RecDateTime", timeArrival),                //время приб
                        new XElement("SndDateTime", timeDepart),                 //время отпр
                        new XElement("EvRecTime", timeArrival),
                        new XElement("EvSndTime", timeDepart),
                        new XElement("TrackNumber", uit.PathNumber),
                        new XElement("Direction", direction),
                        new XElement("EvTrackNumber", uit.PathNumber),
                        new XElement("State", 0),
                        new XElement("VagonDirection", (byte)uit.VagonDirection),
                        new XElement("Enabled", (uit.EmergencySituation & 0x01) == 0x01 ? 0 : 1),
                        new XElement("TypeName", typeName),
                        new XElement("TypeAlias", typeNameShort),
                        new XElement("Addition", uit.Addition)
                    ));
            }



            //DEBUG------------------------
            //string path = Application.StartupPath + @"/StaticTableDisplay" + @"/xDocMainWindow.info";
            //xDoc.Save(path);
            //-----------------------------

            return xDoc.ToString();
        }
    }
}