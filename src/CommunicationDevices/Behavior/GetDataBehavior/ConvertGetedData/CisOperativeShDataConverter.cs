using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommunicationDevices.DataProviders;
using Domain.Entitys;

namespace CommunicationDevices.Behavior.GetDataBehavior.ConvertGetedData
{
    class CisRegularShDataConverter : IInputDataConverter
    {
        public IEnumerable<UniversalInputType> ParseXml2Uit(XDocument xDoc)
        {
            //Log.log.Trace("xDoc" + xDoc.ToString());//LOG;
            var shedules = new List<UniversalInputType>();

            var lines = xDoc.Element("changes")?.Elements().ToList();
            if (lines != null)
            {
                for (var i = 0; i < lines.Count; i++)
                {
                    var line = lines[i];
                    var uit = new UniversalInputType
                    {
                        ViewBag = new Dictionary<string, dynamic>(),
                        TransitTime = new Dictionary<string, DateTime>()
                    };

                    //Id----------
                    var elem = line?.Element("ID")?.Value ?? string.Empty;
                    var idStr = Regex.Replace(elem, "[\r\n\t]+", "");
                    int id;
                    if (int.TryParse(idStr, out id))
                    {
                        uit.Id = id;
                    }

                    //NumberOfTrain------
                    elem = line?.Element("TrainNumber")?.Value ?? string.Empty;
                    var numberOfTrain1 = Regex.Replace(elem, "[\r\n\t]+", "");
                    elem = line?.Element("SecondTrainNumber")?.Value ?? string.Empty;
                    var numberOfTrain2 = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.NumberOfTrain = (string.IsNullOrEmpty(numberOfTrain2) || string.IsNullOrWhiteSpace(numberOfTrain2))
                                         ? numberOfTrain1
                                         : (numberOfTrain1 + "/" + numberOfTrain2);

                    //Stations------
                    elem = line?.Element("Itenary")?.Value.Replace("\\", "/") ?? string.Empty;
                    var stations = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.Stations = stations;

                    //TransitTime["приб"]-----
                    elem = line?.Element("InDateTime")?.Value ?? string.Empty;
                    elem = Regex.Replace(elem, "[\r\n\t]+", "");
                    DateTime dtPrib;
                    DateTime.TryParse(elem, out dtPrib);
                    uit.TransitTime["приб"] = dtPrib;

                    //TransitTime["отпр"]-----
                    elem = line?.Element("OutDateTime")?.Value ?? string.Empty;
                    elem = Regex.Replace(elem, "[\r\n\t]+", "");
                    DateTime dtOtpr;
                    DateTime.TryParse(elem, out dtOtpr);
                    uit.TransitTime["отпр"] = dtOtpr;

                    //StopTime---------------
                    elem = line?.Element("HereDateTime")?.Value ?? string.Empty;
                    elem = Regex.Replace(elem, "[\r\n\t]+", "");
                    TimeSpan stopTime;
                    if (TimeSpan.TryParse(elem, out stopTime))
                    {
                        uit.StopTime = stopTime;
                    }

                    //DaysFollowing------
                    elem = line?.Element("DayOfGoing")?.Value.Replace("\\", "/") ?? string.Empty;
                    var dayOfGoing = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.DaysFollowing = dayOfGoing;

                    //Enabled------------

                    //SoundTemplate------

                    //VagonDirection------
                    elem = line?.Element("VagonDirection")?.Value.Replace("\\", "/") ?? string.Empty;
                    var vagonDirectionStr = Regex.Replace(elem, "[\r\n\t]+", "");
                    int vagonDirection;
                    if (int.TryParse(vagonDirectionStr, out vagonDirection))
                    {
                        uit.VagonDirection = (VagonDirection)vagonDirection;
                    }

                    //DefaultsPaths-------------
                    elem = line?.Element("DefaultsPaths")?.Value.Replace("\\", "/") ?? string.Empty;
                    var defaultsPaths = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.ViewBag["DefaultsPaths"] = defaultsPaths;

                    //Stops------

                    //ScheduleStartDateTime---------------
                    elem = line?.Element("ScheduleStartDateTime")?.Value.Replace("\\", "/") ?? string.Empty;
                    var scheduleStartDateTime = Regex.Replace(elem, "[\r\n\t]+", "");
                    DateTime dtStartDateTime = DateTime.MinValue;
                    DateTime.TryParse(scheduleStartDateTime, out dtStartDateTime);
                    uit.ViewBag["ScheduleStartDateTime"] = dtStartDateTime;

                    //ScheduleEndDateTime---------------
                    elem = line?.Element("ScheduleEndDateTime")?.Value.Replace("\\", "/") ?? string.Empty;
                    var scheduleEndDateTime = Regex.Replace(elem, "[\r\n\t]+", "");
                    DateTime dtEndDateTime = DateTime.MaxValue;
                    DateTime.TryParse(scheduleEndDateTime, out dtEndDateTime);
                    uit.ViewBag["ScheduleEndDateTime"] = dtEndDateTime;

                    //Addition---------------
                    elem = line?.Element("Addition")?.Value.Replace("\\", "/") ?? string.Empty;
                    var addition = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.Addition = addition;

                    //AdditionSend---------------
                    elem = line?.Element("AdditionSend")?.Value.Replace("\\", "/") ?? string.Empty;
                    var additionSend = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.ViewBag["AdditionSend"] = additionSend;

                    //AdditionSendSound-------------

                    //SoundsType-------------------

                    //ItenaryTime--------------
                    elem = line?.Element("ItenaryTime")?.Value.Replace("\\", "/") ?? string.Empty;
                    var itenaryTime = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.ViewBag["ItenaryTime"] = itenaryTime;

                    //DaysFollowingAlias--------------
                    elem = line?.Element("DaysOfGoingAlias")?.Value.Replace("\\", "/") ?? string.Empty;
                    var daysFollowingAlias = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.DaysFollowingAlias= daysFollowingAlias;

                    //StartStation--------------------
                    elem = line?.Element("StartStation")?.Value.Replace("\\", "/") ?? string.Empty;
                    uit.StationDeparture = new Station
                    {
                        NameRu = Regex.Replace(elem, "[\r\n\t]+", "")
                    };

                    //EndStation-----------------------
                    elem = line?.Element("EndStation")?.Value.Replace("\\", "/") ?? string.Empty;
                    uit.StationArrival = new Station
                    {
                        NameRu = Regex.Replace(elem, "[\r\n\t]+", "")
                    };

                    //DirectionStation-----------------------
                    elem = line?.Element("Direction")?.Value.Replace("\\", "/") ?? string.Empty;
                    var directionStation = Regex.Replace(elem, "[\r\n\t]+", "");
                    uit.DirectionStation = directionStation;

                    //VagonDirectionChanging-----------
                    elem = line?.Element("VagonDirectionChanging")?.Value.Replace("\\", "/") ?? string.Empty;
                    var vagonDirectionChanging = Regex.Replace(elem, "[\r\n\t]+", "");
                    bool changeVagonDirection;
                    if (bool.TryParse(vagonDirectionChanging, out changeVagonDirection))
                    {
                        uit.ChangeVagonDirection = changeVagonDirection;
                    }


                    //elem = line?.Element("LateTime")?.Value ?? string.Empty;
                    //elem = Regex.Replace(elem, "[\r\n\t]+", "");
                    //DateTime dtLate;
                    //if (DateTime.TryParse(elem, out dtLate))
                    //{
                    //    uit.DelayTime = dtLate;
                    //}


                    //elem = line?.Element("EmergencySituation")?.Value ?? string.Empty;
                    //elem = Regex.Replace(elem, "[\r\n\t]+", "");
                    //byte emergencySituation;
                    //byte.TryParse(elem, out emergencySituation);
                    //uit.EmergencySituation = emergencySituation;

                    shedules.Add(uit);
                }
            }


            return shedules;
        }
    }
}
