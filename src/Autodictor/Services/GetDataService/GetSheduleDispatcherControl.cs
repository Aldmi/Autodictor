using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using CommunicationDevices.Behavior.GetDataBehavior;
using CommunicationDevices.DataProviders;
using MainExample.Entites;

namespace MainExample.Services.GetDataService
{
    public class GetSheduleDispatcherControl : GetSheduleAbstract
    {
        #region ctor

        public GetSheduleDispatcherControl(BaseGetDataBehavior baseGetDataBehavior, SortedDictionary<string, SoundRecord> soundRecords) 
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

            if (data != null && data.Any())
            {
                foreach (var tr in data)
                {
                    //DEBUG------------------------------------------------------
                    //var str = $"N= {tr.Ntrain}  Путь= {tr.Put}  Дата отпр={tr.DtOtpr:d}  Время отпр={tr.TmOtpr:g}  Дата приб={tr.DtPrib:d} Время приб={tr.TmPrib:g}  Ст.Приб {tr.StFinish}   Ст.Отпр {tr.StDeparture}";
                    //Log.log.Fatal("ПОЕЗД ИЗ ПОЛУЧЕННОГО СПСИКА" + str);
                    //DEBUG-----------------------------------------------------

                    var dateTimeArrival = tr.TransitTime["приб"];              //день и время приб.
                    var dateTimeDepart = tr.TransitTime["отпр"];               //день и время отпр.
                    var stationArrival = tr.StationArrival.NameRu;             //станция приб.
                    var stationDepart = tr.StationDeparture.NameRu;            //станция отпр.

                    for (int i = 0; i < _soundRecords.Count; i++)
                    {
                        var key = _soundRecords.Keys.ElementAt(i);
                        var rec = _soundRecords.ElementAt(i).Value;
                        var recOld = rec;
                        var idTrain = rec.IdTrain;
                        bool changeFlag = false;

                        //DEBUG------------------------------
                        //if (stationArrival == "АРЧЕДА"  && rec.НомерПоезда == "6805" && rec.НазваниеПоезда == "Волгоград - Арчеда")
                        //{
                        //    var g = 5 + 5;
                        //}
                        //DEBUG------------------------------


                        //ТРАНЗИТ
                        if (dateTimeArrival != DateTime.MinValue && dateTimeDepart != DateTime.MinValue)
                        {
                            var numberOfTrain = (string.IsNullOrEmpty(idTrain.НомерПоезда2) || string.IsNullOrWhiteSpace(idTrain.НомерПоезда2)) ? idTrain.НомерПоезда : (idTrain.НомерПоезда + "/" + idTrain.НомерПоезда2);
                            if (tr.NumberOfTrain == numberOfTrain &&
                                dateTimeArrival.Date == idTrain.ДеньПрибытия &&
                                dateTimeDepart.Date == idTrain.ДеньОтправления &&
                                (stationDepart.ToLower().Contains(idTrain.СтанцияОтправления.ToLower()) || idTrain.СтанцияОтправления.ToLower().Contains(stationDepart.ToLower())) &&
                                (stationArrival.ToLower().Contains(idTrain.СтанцияНазначения.ToLower()) || idTrain.СтанцияНазначения.ToLower().Contains(stationArrival.ToLower())))
                            {
                                // Log.log.Fatal("ТРАНЗИТ: " + numberOfTrain);//DEBUG      
                                if (rec.НомерПути != tr.PathNumber)
                                {
                                    rec.НомерПути = tr.PathNumber;
                                    rec.НомерПутиБезАвтосброса = rec.НомерПути;
                                    changeFlag = true;
                                }

                                if (rec.ВремяПрибытия.ToString("yy.MM.dd  HH:mm") != tr.TransitTime["приб"].ToString("yy.MM.dd  HH:mm"))
                                {
                                    rec.ВремяПрибытия = tr.TransitTime["приб"];
                                    changeFlag = true;
                                }

                                if (rec.ВремяОтправления.ToString("yy.MM.dd  HH:mm") != tr.TransitTime["отпр"].ToString("yy.MM.dd  HH:mm"))
                                {
                                    rec.ВремяОтправления = tr.TransitTime["отпр"];
                                    rec.Время = rec.ВремяОтправления;
                                    changeFlag = true;
                                }

                                Debug.WriteLine($"{rec.НазваниеПоезда} Время= {rec.Время} key= {key} ВремяПрибытия= {rec.ВремяПрибытия}  ВремяОтправления= {rec.ВремяОтправления}");       
                            }
                        }
                        //ПРИБ.
                        else
                        if (dateTimeArrival != DateTime.MinValue && dateTimeDepart == DateTime.MinValue)
                        {
                            if (tr.NumberOfTrain == idTrain.НомерПоезда &&
                                dateTimeArrival.Date == rec.IdTrain.ДеньПрибытия &&
                                (stationDepart.ToLower().Contains(idTrain.СтанцияОтправления.ToLower()) || idTrain.СтанцияОтправления.ToLower().Contains(stationArrival.ToLower())) &&
                                (stationArrival.ToLower().Contains(idTrain.СтанцияНазначения.ToLower()) || idTrain.СтанцияНазначения.ToLower().Contains(stationArrival.ToLower())))
                            {
                                //Log.log.Fatal("ПРИБ: " + rec.НомерПоезда);//DEBUG
                                if (rec.НомерПути != tr.PathNumber)
                                {
                                    rec.НомерПути = tr.PathNumber;
                                    rec.НомерПутиБезАвтосброса = rec.НомерПути;
                                    changeFlag = true;
                                }

                                if (rec.ВремяПрибытия.ToString("yy.MM.dd  HH:mm") != tr.TransitTime["приб"].ToString("yy.MM.dd  HH:mm"))
                                {
                                    rec.ВремяПрибытия = tr.TransitTime["приб"];
                                    rec.Время = rec.ВремяПрибытия;
                                    changeFlag = true;
                                }
                            }
                        }
                        //ОТПР.
                        else
                        if (dateTimeDepart != DateTime.MinValue && dateTimeArrival == DateTime.MinValue)
                        {
                            if (tr.NumberOfTrain == idTrain.НомерПоезда &&
                                dateTimeDepart.Date == rec.IdTrain.ДеньОтправления &&
                                (stationDepart.ToLower().Contains(idTrain.СтанцияОтправления.ToLower()) || idTrain.СтанцияОтправления.ToLower().Contains(stationArrival.ToLower())) &&
                                (stationArrival.ToLower().Contains(idTrain.СтанцияНазначения.ToLower()) || idTrain.СтанцияНазначения.ToLower().Contains(stationArrival.ToLower())))
                            {
                                // Log.log.Fatal("ОТПР: " + rec.НомерПоезда);//DEBUG
                                if (rec.НомерПути != tr.PathNumber)
                                {
                                    rec.НомерПути = tr.PathNumber;
                                    rec.НомерПутиБезАвтосброса = rec.НомерПути;
                                    changeFlag = true;
                                }

                                if (rec.ВремяОтправления.ToString("yy.MM.dd  HH:mm") != tr.TransitTime["отпр"].ToString("yy.MM.dd  HH:mm"))
                                {
                                    rec.ВремяОтправления = tr.TransitTime["отпр"];
                                    rec.Время = rec.ВремяОтправления;
                                    changeFlag = true;
                                }
                            }
                        }

                        if (changeFlag)
                        {
                            SoundRecordChangesRx.OnNext(new SoundRecordChanges { NewRec = rec, Rec = recOld, TimeStamp = DateTime.Now, UserInfo = "Удаленный диспетчер" });
                        }
                    }
                }
            }
        }

        #endregion
    }
}