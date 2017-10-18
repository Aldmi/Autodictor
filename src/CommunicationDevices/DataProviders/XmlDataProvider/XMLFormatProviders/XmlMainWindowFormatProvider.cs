﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings;
using Library.Convertion;

namespace CommunicationDevices.DataProviders.XmlDataProvider.XMLFormatProviders
{

    //<? xml version="1.0" encoding="utf-8" standalone="yes"?>
    //<mainWindow>
    //  <t>
    //    <ScheduleId>6396</ScheduleId>
    //    <TrainNumber>10</TrainNumber>
    //    <TrainType>0</TrainType>
    //    <DirectionStation>Курское</DirectionStation>
    //    <StartStation>Крюково</StartStation>
    //    <EndStation> </EndStation>
    //    <StartStationENG>Крюково</StartStationENG>
    //    <EndStationENG> </EndStationENG>	
    //    <RecDateTime></RecDateTime>
    //    <SndDateTime>2017-06-17T00:34:00</SndDateTime>
    //    <EvRecTime></EvRecTime>
    //    <EvSndTime>2017-06-17T00:34:00</EvSndTime>
    //    <LateTime>12:20</LateTime>                                 //час:мин
    //    <HereDateTime>15</HereDateTime>                            //время стоянки

    //    <DaysOfGoing>Ежедневно</DaysOfGoing>                       //Дни след
    //    <DaysOfGoingAlias></DaysOfGoingAlias>                      //Дни след. заданные вручную

    //    <TrackNumber></TrackNumber>
    //    <TrackNumberWithoutAutoReset></TrackNumberWithoutAutoReset>
    //    <Direction>1</Direction>
    //    <EvTrackNumber></EvTrackNumber>
    //    <State>0</State>
    //    <VagonDirection>0</VagonDirection>
    //    <Enabled>1</Enabled>
    //    <EmergencySituation> </EmergencySituation>    //Нешатная ситуация (бит 0 - Отмена, бит 1 - задержка прибытия, бит 2 - задержка отправления, бит 3 - отправление по готовности)
    //	  <TypeName>Пригородный</TypeName>
    //	  <TypeAlias>приг</TypeAlias>
    //	  <Addition>Поле дополнения</Addition>
    //	  <Note>Поле дополнения</Note>                 // список остановок
    //  </t>
    //</mainWindow>


    public class XmlMainWindowFormatProvider : IFormatProvider
    {
        private readonly DateTimeFormat _dateTimeFormat;
        private readonly TransitSortFormat _transitSortFormat;



        public XmlMainWindowFormatProvider(DateTimeFormat dateTimeFormat, TransitSortFormat transitSortFormat)
        {
            _dateTimeFormat = dateTimeFormat;
            _transitSortFormat = transitSortFormat;
        }



        public string CreateDoc(IEnumerable<UniversalInputType> tables)
        {
            if (tables == null || !tables.Any())
                return null;

            //Сортировка транзитов
            if (_transitSortFormat != TransitSortFormat.None)
            {
                tables = tables.OrderBy(train =>
                {
                    switch (train.Event)
                    {
                        case "СТОЯНКА":
                            switch (_transitSortFormat)
                            {
                                case TransitSortFormat.Arrival:
                                    return  train.TransitTime["приб"];

                                case TransitSortFormat.Departure:
                                    return train.TransitTime["отпр"];
 
                                default:
                                    return train.Time;
                            }
                        default:
                            return train.Time;
                    }
                });
            }
            
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
                        typeName = "Скорый";
                        typeNameShort = "скор";
                        break;

                    case TypeTrain.HighSpeed:
                        trainType = "2";
                        typeName = "Скоростной";
                        typeNameShort = "скорост";
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
                        typeName = "Экспресс";
                        typeNameShort = "эксп";
                        break;

                    case TypeTrain.Rex:
                        trainType = "5";
                        typeName = "Экспресс";
                        typeNameShort = "эксп";
                        break;
                }

                var timeArrival = string.Empty;
                var timeDepart = string.Empty;
                byte direction = 0;
                switch (uit.Event)
                {
                    case "ПРИБ.":
                        switch (_dateTimeFormat)
                        {
                            case DateTimeFormat.None:
                                timeArrival = uit.Time.ToString("s");
                                break;

                            case DateTimeFormat.Sortable:
                                timeArrival = uit.Time.ToString("s");
                                break;

                            case DateTimeFormat.LinuxTimeStamp:
                                timeArrival = DateTimeConvertion.ConvertToUnixTimestamp(uit.Time).ToString(CultureInfo.InvariantCulture);
                                break;
                        }
                        direction = 0;
                        break;

                    case "ОТПР.":
                        switch (_dateTimeFormat)
                        {
                            case DateTimeFormat.None:
                                timeDepart = uit.Time.ToString("s");
                                break;

                            case DateTimeFormat.Sortable:
                                timeDepart = uit.Time.ToString("s");
                                break;

                            case DateTimeFormat.LinuxTimeStamp:
                                timeDepart = DateTimeConvertion.ConvertToUnixTimestamp(uit.Time).ToString(CultureInfo.InvariantCulture);
                                break;
                        }
                        direction = 1;
                        break;

                    case "СТОЯНКА":
                        switch (_dateTimeFormat)
                        {
                            case DateTimeFormat.None:
                            case DateTimeFormat.Sortable:
                                timeArrival = uit.TransitTime.ContainsKey("приб") ? uit.TransitTime["приб"].ToString("s") : String.Empty;
                                timeDepart = uit.TransitTime.ContainsKey("отпр") ? uit.TransitTime["отпр"].ToString("s") : String.Empty;
                                break;

                            case DateTimeFormat.LinuxTimeStamp:
                                timeArrival = uit.TransitTime.ContainsKey("приб") ? DateTimeConvertion.ConvertToUnixTimestamp(uit.TransitTime["приб"]).ToString(CultureInfo.InvariantCulture) : String.Empty;
                                timeDepart = uit.TransitTime.ContainsKey("отпр") ? DateTimeConvertion.ConvertToUnixTimestamp(uit.TransitTime["отпр"]).ToString(CultureInfo.InvariantCulture) : String.Empty;
                                break;
                        }
                        direction = 2;
                        break;
                }

                var lateTime = uit.DelayTime?.ToString("HH:mm") ?? string.Empty;

                var stopTime = (uit.StopTime.HasValue) ? uit.StopTime.Value.Hours.ToString("D2") + ":" + uit.StopTime.Value.Minutes.ToString("D2") : string.Empty;

                xDoc.Root?.Add(
                        new XElement("t",
                        new XElement("ScheduleId", uit.ScheduleId),
                        new XElement("TrainNumber", uit.NumberOfTrain),
                        new XElement("TrainType", trainType),
                        new XElement("DirectionStation", uit.DirectionStation),

                        new XElement("StartStation", uit.StationDeparture?.NameRu ?? string.Empty),
                        new XElement("EndStation", uit.StationArrival?.NameRu ?? string.Empty),
                        new XElement("StartStationENG", uit.StationDeparture?.NameEng ?? string.Empty),
                        new XElement("EndStationENG", uit.StationArrival?.NameEng ?? string.Empty),
                        new XElement("StartStationCH", uit.StationDeparture?.NameCh ?? string.Empty),
                        new XElement("EndStationCH", uit.StationArrival?.NameCh ?? string.Empty),


                        new XElement("RecDateTime", timeArrival),                //время приб
                        new XElement("SndDateTime", timeDepart),                 //время отпр
                        new XElement("EvRecTime", timeArrival),
                        new XElement("EvSndTime", timeDepart),
                        new XElement("LateTime", lateTime),                      //время задержки
                        new XElement("HereDateTime", stopTime),                  //время стоянки

                        new XElement("DaysOfGoing", uit.DaysFollowing),            //дни след
                        new XElement("DaysOfGoingAlias", uit.DaysFollowingAlias),  //дни след заданные в ручную

                        new XElement("TrackNumber", uit.PathNumber),
                        new XElement("TrackNumberWithoutAutoReset", uit.PathNumberWithoutAutoReset),
                        new XElement("Direction", direction),
                        new XElement("EvTrackNumber", uit.PathNumber),
                        new XElement("State", 0),
                        new XElement("VagonDirection", (byte)uit.VagonDirection),
                        new XElement("Enabled", (uit.EmergencySituation & 0x01) == 0x01 ? 0 : 1),
                        new XElement("EmergencySituation", uit.EmergencySituation),
                        new XElement("TypeName", typeName),
                        new XElement("TypeAlias", typeNameShort),
                        new XElement("Addition", uit.Addition),
                        new XElement("Note", uit.Note)                            //станции следования
                    ));
            }



            //DEBUG------------------------
            string path = Application.StartupPath + @"/StaticTableDisplay" + @"/xDocMainWindow.info";
            xDoc.Save(path);
            //-----------------------------

            return xDoc.ToString();
        }
    }
}