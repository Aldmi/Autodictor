﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Timers;
using CommunicationDevices.Behavior.BindingBehavior.Helpers;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;
using CommunicationDevices.Settings;


namespace CommunicationDevices.Behavior.BindingBehavior.ToGeneralSchedule
{
    public class BindingDevice2GeneralShBehavior : IBinding2GeneralSchedule, IDisposable
    {
        #region fields

        private readonly Device _device;

        #endregion





        #region prop

        public bool IsPaging { get; }
        public SourceLoad SourceLoad { get; set; }
        public Conditions Conditions { get; }
        public PaggingHelper PagingHelper { get; set; }
        public IDisposable DispousePagingListSendRx { get; set; }

        public DeviceSetting GetDeviceSetting => _device.Setting;

        #endregion





        #region ctor

        public BindingDevice2GeneralShBehavior(Device device, SourceLoad source, Conditions conditions, int countPage, int timePaging)
        {
            Conditions = conditions;
            _device = device;
            SourceLoad = source;

            //если указанны настройки пагинатора.
            if (countPage > 0 && timePaging > 0)
            {
                IsPaging = true;
                PagingHelper = new PaggingHelper(timePaging * 1000, countPage);
                DispousePagingListSendRx = PagingHelper.PagingListSend.Subscribe(OnNext);     //подписка на отправку сообщений пагинатором
            }
        }

        #endregion





        private void OnNext(PagingList pagingList)
        {
            var inData = new UniversalInputType
            {
                TableData = pagingList.List,
                Note = pagingList.CurrentPage.ToString()
            };

            _device.ExhBehavior.AddOneTimeSendData(inData);
        }


        public void InitializePagingBuffer(UniversalInputType inData, Func<UniversalInputType, bool> checkContrains)
        {
            var filteredTable = inData.TableData.Where(checkContrains).ToList();

            if (IsPaging)
            {
                PagingHelper.PagingBuffer = filteredTable;
            }
            else
            {
                inData.TableData = filteredTable;
                inData.Note = String.Empty;
                _device.AddCycleFuncData(0, inData);
            }
        }





        /// <summary>
        /// Проверка ограничения привязки.
        /// </summary>
        public bool CheckContrains(UniversalInputType inData)
        {
            if (Conditions == null)
                return true;

            return Conditions.CheckContrains(inData);
        }




        #region Disposable

        public void Dispose()
        {
            DispousePagingListSendRx?.Dispose();
            PagingHelper?.Dispose();
        }

        #endregion
    }
}