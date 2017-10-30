﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunicationDevices.Behavior.GetDataBehavior;
using CommunicationDevices.DataProviders;
using MainExample.Entites;
using MainExample.Mappers;

namespace MainExample.Services.GetDataService
{
    class GetCisRegSh : GetSheduleAbstract
    {
        #region ctor

        public GetCisRegSh(BaseGetDataBehavior baseGetDataBehavior, SortedDictionary<string, SoundRecord> soundRecords) 
            : base(baseGetDataBehavior, soundRecords)
        {

        }

        #endregion




        #region Methode

        /// <summary>
        /// Обработка полученных данных
        /// </summary>
        protected override void GetaDataRxEventHandler(IEnumerable<UniversalInputType> data)
        {
            if (!Enable)
                return;

            var universalInputTypes = data as IList<UniversalInputType> ?? data.ToList();
            if (universalInputTypes.Any())
            {
                var localTable = TrainSheduleTable.ЗагрузитьРасписаниеЛокальное();
                if (localTable == null)
                    return;

                var tableRecords = new List<TrainTableRecord>();
                foreach (var uit in universalInputTypes)
                {
                    //var trTable= Mapper.MapUniversalInputType2TrainTableRecord(uit);
                    var tableRec = localTable.FirstOrDefault(tr => tr.Num == uit.NumberOfTrain);
                    if (!string.IsNullOrEmpty(tableRec.Num))
                    {
                        tableRec.Name = uit.Stations;
                        tableRec.ArrivalTime = (uit.TransitTime["приб"] == DateTime.MinValue) ? string.Empty : uit.TransitTime["приб"].ToString("HH:mm");
                        tableRec.DepartureTime = (uit.TransitTime["отпр"] == DateTime.MinValue) ? string.Empty : uit.TransitTime["отпр"].ToString("HH:mm");
                        tableRec.StopTime = uit.StopTime?.ToString("t") ?? string.Empty;
                        tableRec.TrainPathDirection = (byte) uit.VagonDirection;

                        tableRec.ВремяНачалаДействияРасписания = uit.ViewBag.ContainsKey("ScheduleStartDateTime")
                            ? uit.ViewBag["ScheduleStartDateTime"]
                            : new DateTime(1900, 1, 1);

                        tableRec.ВремяОкончанияДействияРасписания = uit.ViewBag.ContainsKey("ScheduleEndDateTime")
                            ? uit.ViewBag["ScheduleEndDateTime"]
                            : new DateTime(2100, 1, 1);

                        tableRec.Addition = uit.Addition;
                        tableRec.DaysAlias = uit.DaysFollowingAlias;
                        tableRec.StationArrival = uit.StationArrival.NameRu;
                        tableRec.StationDepart = uit.StationDeparture.NameRu;

                        tableRec.Direction =  string.IsNullOrEmpty(uit.DirectionStation) ? tableRec.Direction : uit.DirectionStation;
                        tableRec.ChangeTrainPathDirection = uit.ChangeVagonDirection;

                        tableRecords.Add(tableRec);
                    }
                }

                if (tableRecords.Any())
                {
                    TrainSheduleTable.СохранитьИПрименитьСписокРегулярноеРасписаниеЦис(tableRecords);
                }    
            }
        }

        #endregion
    }
}
