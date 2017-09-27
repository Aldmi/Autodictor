using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunicationDevices.Behavior.ExhangeBehavior;
using Domain.Entitys.ApkDk;
using Library.Logs;
using MainExample.Extension;

namespace MainExample.Services.GetDataService
{
    public class ApkDkVolgogradGetSheduleService : IDisposable
    {
        #region field

        private readonly ISubject<IEnumerable<ApkDkVolgogradShedule>> _sheduleGetRx;
        private readonly SortedDictionary<string, SoundRecord> _soundRecords;

        #endregion




        #region prop

        public ISubject<IExhangeBehavior> ConnectChangeRx { get; }
        public ISubject<IExhangeBehavior> DataExchangeSuccessChangeRx { get; }

        public IDisposable DispouseSheduleGetRx { get; set; }
        public IDisposable DispouseConnectChangeRx { get; set; }
        public IDisposable DispouseDataExchangeSuccessChangeRx { get; set; }

        public bool Enable { get; set; }

        #endregion




        #region ctor

        public ApkDkVolgogradGetSheduleService(ISubject<IEnumerable<ApkDkVolgogradShedule>> sheduleGetRx,
            ISubject<IExhangeBehavior> connectChangeRx,
            ISubject<IExhangeBehavior> dataExchangeSuccessChangeRx, 
            SortedDictionary<string, SoundRecord> soundRecords)
        {
            _sheduleGetRx = sheduleGetRx;
            ConnectChangeRx = connectChangeRx;
            DataExchangeSuccessChangeRx = dataExchangeSuccessChangeRx;
            _soundRecords = soundRecords;
        }

        #endregion





        #region Methode

        /// <summary>
        /// Подписать все события и запустить
        /// </summary>
        public void SubscribeAndStart(Control control)
        {
            DispouseSheduleGetRx= _sheduleGetRx?.Subscribe(GetApkDkVolgorgadSheduleRxEventHandler);
            DispouseConnectChangeRx= ConnectChangeRx.Subscribe(behavior => control.Enabled = behavior.IsConnect);  //контролл не активен, если нет связи
            DispouseDataExchangeSuccessChangeRx = DataExchangeSuccessChangeRx.Subscribe(behavior =>
            {
                var colorYes = Color.GreenYellow;
                var colorError = Color.Red;
                var colorNo = Color.White;
                control.BackColor = (behavior.DataExchangeSuccess) ? colorYes : colorError;
                Task.Delay(1000).ContinueWith(task =>
                {
                    control.InvokeIfNeeded(() =>
                    {
                        control.BackColor = colorNo;
                    });
                });
            });
        }


        /// <summary>
        /// Обработка полученных данных
        /// </summary>
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
                    //var str = $"N= {tr.Ntrain}  Путь= {tr.Put}  Дата отпр={tr.DtOtpr:d}  Время отпр={tr.TmOtpr:g}  Дата приб={tr.DtPrib:d} Время приб={tr.TmPrib:g}  Ст.Приб {tr.StFinish}   Ст.Отпр {tr.StDeparture}";
                    //Log.log.Fatal("ПОЕЗД ИЗ ПОЛУЧЕННОГО СПСИКА" + str);
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
                               // Log.log.Fatal("ТРАНЗИТ: " + numberOfTrain);//DEBUG

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
                                //Log.log.Fatal("ПРИБ: " + rec.НомерПоезда);//DEBUG

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
                               // Log.log.Fatal("ОТПР: " + rec.НомерПоезда);//DEBUG

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

        #endregion










        #region Disposable

        public void Dispose()
        {
            DispouseSheduleGetRx?.Dispose();
            DispouseConnectChangeRx?.Dispose();
            DispouseDataExchangeSuccessChangeRx?.Dispose();
        }

        #endregion
    }
}