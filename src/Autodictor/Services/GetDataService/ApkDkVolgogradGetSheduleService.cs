using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Windows.Forms;
using CommunicationDevices.Behavior.ExhangeBehavior;
using Domain.Entitys.ApkDk;
using Library.Logs;

namespace MainExample.Services.GetDataService
{
    public class ApkDkVolgogradGetSheduleService : IDisposable
    {
        private readonly ISubject<IEnumerable<ApkDkVolgogradShedule>> _sheduleChangeRx;
        private readonly SortedDictionary<string, SoundRecord> _soundRecords;

        public ISubject<IExhangeBehavior> ConnectChangeRx { get; }
        public ISubject<IExhangeBehavior> DataExchangeSuccessChangeRx { get; }

        public IDisposable DispouseSheduleChangeRx { get; set; }
        public IDisposable DispouseConnectChangeRx { get; set; }
        public IDisposable DispouseDataExchangeSuccessChangeRx { get; set; }

        public bool Enable { get; set; }




        public ApkDkVolgogradGetSheduleService(ISubject<IEnumerable<ApkDkVolgogradShedule>> sheduleChangeRx,
                                               ISubject<IExhangeBehavior> connectChangeRx,
                                               ISubject<IExhangeBehavior> dataExchangeSuccessChangeRx, 
                                               SortedDictionary<string, SoundRecord> soundRecords)
        {
            _sheduleChangeRx = sheduleChangeRx;
            ConnectChangeRx = connectChangeRx;
            DataExchangeSuccessChangeRx = dataExchangeSuccessChangeRx;
            _soundRecords = soundRecords;
        }




        public void Subscribe(CheckBox controlCheckBox)
        {
            DispouseSheduleChangeRx= _sheduleChangeRx?.Subscribe(GetApkDkVolgorgadSheduleRxEventHandler);
            DispouseConnectChangeRx= ConnectChangeRx.Subscribe(behavior => controlCheckBox.Enabled = behavior.IsConnect);
            DispouseDataExchangeSuccessChangeRx = DataExchangeSuccessChangeRx.Subscribe();
        }




        private void GetApkDkVolgorgadSheduleRxEventHandler(IEnumerable<ApkDkVolgogradShedule> apkDkVolgogradShedules)
        {
            if(!Enable)
              return;

            if (apkDkVolgogradShedules != null && apkDkVolgogradShedules.Any())
            {
                var trainWithPut = apkDkVolgogradShedules.Where(sh => !(string.IsNullOrEmpty(sh.Put) || string.IsNullOrWhiteSpace(sh.Put))).ToList();
                foreach (var tr in trainWithPut)
                {
                    //DEBUG------------------------------------------------------
                    var str = $"N= {tr.Ntrain}  Путь= {tr.Put}  Дата отпр={tr.DtOtpr:d}  Время отпр={tr.TmOtpr:g}  Дата приб={tr.DtPrib:d} Время приб={tr.TmPrib:g}  Ст.Приб {tr.StFinish}   Ст.Отпр {tr.StDeparture}";
                    Log.log.Fatal("ПОЕЗД ИЗ ПОЛУЧЕННОГО СПСИКА" + str);
                    //DEBUG-----------------------------------------------------

                    for (int i = 0; i < _soundRecords.Count; i++)
                    {
                        var key = _soundRecords.Keys.ElementAt(i);
                        var rec = _soundRecords.ElementAt(i).Value;

                        //ТРАНЗИТ
                        if (tr.DtPrib != DateTime.MinValue && tr.DtOtpr != DateTime.MinValue)
                        {
                            var numberOfTrain = (string.IsNullOrEmpty(rec.НомерПоезда2) || string.IsNullOrWhiteSpace(rec.НомерПоезда2)) ? rec.НомерПоезда : (rec.НомерПоезда + "/" + rec.НомерПоезда2);
                            if (tr.Ntrain == numberOfTrain &&
                                tr.DtPrib == rec.IdTrain.DayArrival &&
                                tr.DtOtpr == rec.IdTrain.DayDepart &&
                                (tr.StDeparture.ToLower().Contains(rec.СтанцияОтправления.ToLower()) || rec.СтанцияОтправления.ToLower().Contains(tr.StDeparture.ToLower())) &&
                                (tr.StFinish.ToLower().Contains(rec.СтанцияНазначения.ToLower()) || rec.СтанцияНазначения.ToLower().Contains(tr.StFinish.ToLower())))
                            {
                                Log.log.Fatal("ТРАНЗИТ: " + numberOfTrain);//DEBUG

                                rec.НомерПути = tr.Put;
                                _soundRecords[key] = rec;
                                // SendOnPathTable(SoundRecords[key]);
                                break;
                            }
                        }
                        //ПРИБ.
                        else
                        if (tr.DtPrib != DateTime.MinValue && tr.DtOtpr == DateTime.MinValue)
                        {
                            if (tr.Ntrain == rec.НомерПоезда &&
                                tr.DtPrib == rec.IdTrain.DayArrival &&
                                (tr.StDeparture.ToLower().Contains(rec.СтанцияОтправления.ToLower()) || rec.СтанцияОтправления.ToLower().Contains(tr.StDeparture.ToLower())) &&
                                (tr.StFinish.ToLower().Contains(rec.СтанцияНазначения.ToLower()) || rec.СтанцияНазначения.ToLower().Contains(tr.StFinish.ToLower())))
                            {
                                Log.log.Fatal("ПРИБ: " + rec.НомерПоезда);//DEBUG

                                rec.НомерПути = tr.Put;
                                _soundRecords[key] = rec;
                                // SendOnPathTable(SoundRecords[key]);
                                break;
                            }
                        }
                        //ОТПР.
                        else
                        if (tr.DtOtpr != DateTime.MinValue && tr.DtPrib == DateTime.MinValue)
                        {
                            if (tr.Ntrain == rec.НомерПоезда &&
                                tr.DtOtpr == rec.IdTrain.DayDepart &&
                                (tr.StDeparture.ToLower().Contains(rec.СтанцияОтправления.ToLower()) || rec.СтанцияОтправления.ToLower().Contains(tr.StDeparture.ToLower())) &&
                                (tr.StFinish.ToLower().Contains(rec.СтанцияНазначения.ToLower()) || rec.СтанцияНазначения.ToLower().Contains(tr.StFinish.ToLower())))
                            {
                                Log.log.Fatal("ОТПР: " + rec.НомерПоезда);//DEBUG

                                rec.НомерПути = tr.Put;
                                _soundRecords[key] = rec;
                                // SendOnPathTable(SoundRecords[key]);
                                break;
                            }
                        }
                    }
                }
            }
        }










        #region Disposable

        public void Dispose()
        {
        }

        #endregion
    }
}