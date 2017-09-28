﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunicationDevices.Behavior.ExhangeBehavior;
using CommunicationDevices.Behavior.GetDataBehavior;
using CommunicationDevices.DataProviders;
using MainExample.Extension;

namespace MainExample.Services.GetDataService
{
    public abstract class GetSheduleAbstract : IDisposable
    {
        #region field

        private readonly ISubject<IEnumerable<UniversalInputType>> _sheduleGetRx;
        protected readonly SortedDictionary<string, SoundRecord> _soundRecords;

        #endregion




        #region prop

        public ISubject<IExhangeBehavior> ConnectChangeRx { get; }
        public ISubject<IExhangeBehavior> DataExchangeSuccessRx { get; }

        public IDisposable DispouseSheduleGetRx { get; set; }
        public IDisposable DispouseConnectChangeRx { get; set; }
        public IDisposable DispouseDataExchangeSuccessChangeRx { get; set; }

        public bool Enable { get; set; }

        #endregion




        #region ctor

        protected GetSheduleAbstract(BaseGetDataBehavior baseGetDataBehavior, SortedDictionary<string, SoundRecord> soundRecords)
        {
            _sheduleGetRx = baseGetDataBehavior.ConvertedDataChangeRx;
            ConnectChangeRx = baseGetDataBehavior.ConnectChangeRx;
            DataExchangeSuccessRx = baseGetDataBehavior.DataExchangeSuccessRx;
            _soundRecords = soundRecords;
        }

        #endregion





        #region Methode

        /// <summary>
        /// Подписать все события и запустить
        /// </summary>
        public void SubscribeAndStart(Control control)
        {
            DispouseSheduleGetRx= _sheduleGetRx?.Subscribe(GetaDataRxEventHandler);
            DispouseConnectChangeRx= ConnectChangeRx.Subscribe(behavior => control.Enabled = behavior.IsConnect);  //контролл не активен, если нет связи
            DispouseDataExchangeSuccessChangeRx = DataExchangeSuccessRx.Subscribe(behavior =>
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
        /// Обработка полученных данных.
        /// </summary>
        protected abstract void GetaDataRxEventHandler(IEnumerable<UniversalInputType> data);

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