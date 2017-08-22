using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Castle.Components.DictionaryAdapter;
using CommunicationDevices.DataProviders;
using MoreLinq;


namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings
{

    public class XmlConditionsSetting
    {
        #region prop

        public Conditions Conditions { get; }

        #endregion




        #region ctor

        public XmlConditionsSetting(string contrains)
        {
            var contr = contrains.Split(';');
            if (contr.Any())
            {
                Conditions = new Conditions
                {
                    TypeTrain = new List<TypeTrain>(),
                    Event = new List<string>()
                };
                foreach (var s in contr)
                {
                    var matchString = Regex.Match(s, "Пассажирский\\+ПУТЬ:(.*)").Groups[1].Value;
                    if (!string.IsNullOrEmpty(matchString))
                    {
                        Conditions.PassengerPaths= new List<string>(matchString.Split(','));
                        continue;
                    }

                    matchString = Regex.Match(s, "Пригородный\\+ПУТЬ:(.*)").Groups[1].Value;
                    if (!string.IsNullOrEmpty(matchString))
                    {
                        Conditions.SuburbanPaths = new List<string>(matchString.Split(','));
                        continue;
                    }

                    matchString = Regex.Match(s, "Фирменный\\+ПУТЬ:(.*)").Groups[1].Value;
                    if (!string.IsNullOrEmpty(matchString))
                    {
                        Conditions.CorporatePaths = new List<string>(matchString.Split(','));
                        continue;
                    }

                    matchString = Regex.Match(s, "Скорый\\+ПУТЬ:(.*)").Groups[1].Value;
                    if (!string.IsNullOrEmpty(matchString))
                    {
                        Conditions.ExpressPaths = new List<string>(matchString.Split(','));
                        continue;
                    }

                    matchString = Regex.Match(s, "Скоростной\\+ПУТЬ:(.*)").Groups[1].Value;
                    if (!string.IsNullOrEmpty(matchString))
                    {
                        Conditions.HighSpeedPaths = new List<string>(matchString.Split(','));
                        continue;
                    }

                    matchString = Regex.Match(s, "Ласточка\\+ПУТЬ:(.*)").Groups[1].Value;
                    if (!string.IsNullOrEmpty(matchString))
                    {
                        Conditions.SwallowPaths = new List<string>(matchString.Split(','));
                        continue;
                    }

                    matchString = Regex.Match(s, "РЭКС\\+ПУТЬ:(.*)").Groups[1].Value;
                    if (!string.IsNullOrEmpty(matchString))
                    {
                        Conditions.RexPaths = new List<string>(matchString.Split(','));
                        continue;
                    }

                    matchString = Regex.Match(s, "ПРИБ.\\+ПУТЬ:(.*)").Groups[1].Value;
                    if (!string.IsNullOrEmpty(matchString))
                    {
                        Conditions.ArrivalPaths = new List<string>(matchString.Split(','));
                        continue;
                    }

                    matchString = Regex.Match(s, "ОТПР.\\+ПУТЬ:(.*)").Groups[1].Value;
                    if (!string.IsNullOrEmpty(matchString))
                    {
                        Conditions.DeparturePaths = new List<string>(matchString.Split(','));
                        continue;
                    }

                    matchString = Regex.Match(s, "ЛимитСтрокТаблицы\\:(.*)").Groups[1].Value;
                    if (!string.IsNullOrEmpty(matchString))
                    {
                        int limitRow;
                        Conditions.LimitNumberRows = (int?)(int.TryParse(matchString, out limitRow) ? (ValueType) limitRow : null);
                        continue;
                    }

                    matchString = Regex.Match(s, "ДельтаТекВремени\\:(.*)").Groups[1].Value;
                    if (!string.IsNullOrEmpty(matchString))
                    {
                        var deltaTime = matchString.Split('|');
                        if (deltaTime.Length == 2)
                        {
                            int minMinute;
                            int maxMinute;
                            if (int.TryParse(deltaTime[0], out minMinute) && int.TryParse(deltaTime[1], out maxMinute))
                            {
                                Conditions.DeltaCurrentTime = new Dictionary<string, TimeSpan>
                                {
                                    ["-"] = new TimeSpan(0, 0, minMinute, 0),
                                    ["+"] = new TimeSpan(0, 0, maxMinute, 0)
                                };
                            }
                        }

                        int limitRow;
                        Conditions.LimitNumberRows = (int?)(int.TryParse(matchString, out limitRow) ? (ValueType)limitRow : null);
                        continue;
                    }


                    matchString = Regex.Match(s, "Направление\\:(.*)").Groups[1].Value;
                    if (!string.IsNullOrEmpty(matchString))
                    {
                        var directions = matchString.Split('|').Select(item=> item.ToLower()).ToList();
                        Conditions.DirectionStations = directions;
                        continue;
                    }


                    switch (s)
                    {
                        case "ПРИБ.":
                            Conditions.Event.Add(s);
                            break;

                        case "ОТПР.":
                            Conditions.Event.Add(s);
                            break;

                        case "ТРАНЗ.":
                            Conditions.Event.Add("СТОЯНКА");
                            break;

                        case "Пассажирский":
                            Conditions.TypeTrain.Add(TypeTrain.Passenger);
                            break;

                        case "Пригородный":
                            Conditions.TypeTrain.Add(TypeTrain.Suburban);
                            break;

                        case "Фирменный":
                            Conditions.TypeTrain.Add(TypeTrain.Corporate);
                            break;

                        case "Скорый":
                            Conditions.TypeTrain.Add(TypeTrain.Express);
                            break;

                        case "Скоростной":
                            Conditions.TypeTrain.Add(TypeTrain.HighSpeed);
                            break;

                        case "Ласточка":
                            Conditions.TypeTrain.Add(TypeTrain.Swallow);
                            break;

                        case "РЭКС":
                            Conditions.TypeTrain.Add(TypeTrain.Rex);
                            break;


                        case "Пассажирский+ПРИБ.":
                            Conditions.PassengerArrival = true;
                            break;

                        case "Пассажирский+ОТПР.":
                            Conditions.PassengerDepart = true;
                            break;

                        case "Пригородный+ПРИБ.":
                            Conditions.SuburbanArrival = true;
                            break;

                        case "Пригородный+ОТПР.":
                            Conditions.SuburbanDepart = true;
                            break;


                        case "Фирменный+ПРИБ.":
                            Conditions.CorporateArrival = true;
                            break;

                        case "Фирменный+ОТПР.":
                            Conditions.CorporateDepart = true;
                            break;

                        case "Скорый+ПРИБ.":
                            Conditions.ExpressArrival = true;
                            break;

                        case "Скорый+ОТПР.":
                            Conditions.ExpressDepart = true;
                            break;


                        case "Скоростной+ПРИБ.":
                            Conditions.HighSpeedArrival = true;
                            break;

                        case "Скоростной+ОТПР.":
                            Conditions.HighSpeedDepart = true;
                            break;

                        case "Ласточка+ПРИБ.":
                            Conditions.SwallowArrival = true;
                            break;

                        case "Ласточка+ОТПР.":
                            Conditions.SwallowDepart = true;
                            break;

                        case "РЭКС+ПРИБ.":
                            Conditions.RexArrival = true;
                            break;

                        case "РЭКС+ОТПР.":
                            Conditions.RexDepart = true;
                            break;


                        case "МеньшеТекВремени":
                            Conditions.LowCurrentTime = true;
                            break;

                        case "БольшеТекВремени":
                            Conditions.HightCurrentTime = true;
                            break;


                        case "Отменен_БлокВремОгр":
                            Conditions.EmergencySituationCanceled = true;
                            break;

                        case "ЗадержкаПрибытия_БлокВремОгр":
                            Conditions.EmergencySituationDelayArrival = true;
                            break;

                        case "ЗадержкаОтправления_БлокВремОгр":
                            Conditions.EmergencySituationDelayDepart = true;
                            break;



                        case "КомандаОчистки":
                            Conditions.Command = Command.Clear;
                            break;

                        case "КомандаПерезагрузки":
                            Conditions.Command = Command.Restart;
                            break;



                        default:
                            Conditions = null;
                            return;
                    }
                }
            }
        }

        #endregion
    }
}