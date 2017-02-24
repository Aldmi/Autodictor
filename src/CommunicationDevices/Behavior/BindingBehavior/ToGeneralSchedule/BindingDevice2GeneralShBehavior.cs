using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Timers;
using CommunicationDevices.Behavior.BindingBehavior.Helpers;
using CommunicationDevices.Devices;
using CommunicationDevices.Infrastructure;


namespace CommunicationDevices.Behavior.BindingBehavior.ToGeneralSchedule
{
    public class BindingDevice2GeneralShBehavior : IBinding2GeneralSchedule, IDisposable
    {
        private readonly bool _isPaging;
        private readonly Device _device;




        public UniversalInputType Contrains { get; }
        public PaggingHelper PagingHelper { get; set; }
        public IDisposable DispousePagingListSendRx { get; set; }




        #region ctor

        public BindingDevice2GeneralShBehavior(Device device, UniversalInputType contrains, int countPage, int timePaging)
        {
            Contrains = contrains;
            _device = device;

            //если указанны настройки пагинатора.
            if (countPage > 0 && timePaging > 0)
            {
                _isPaging = true;
                PagingHelper = new PaggingHelper(timePaging * 1000, countPage);

                DispousePagingListSendRx = PagingHelper.PagingListSend.Subscribe(OnNext);     //подписка на отправку сообщений пагинатором
            }
        }

        #endregion




        private void OnNext(PagingList pagingList)
        {
            var inData = new UniversalInputType
            { TableData = pagingList.List,
              Note = pagingList.CurrentPage.ToString()};

            _device.ExhBehavior.AddOneTimeSendData(inData);
            _device.ExhBehavior.GetData4CycleFunc[0].Initialize(inData);
        }


        public void InitializePagingBuffer(UniversalInputType inData, Func<UniversalInputType, bool> checkContrains)
        {
            var filteredTable= inData.TableData.Where(checkContrains).ToList();

            if (_isPaging)
            {
                PagingHelper.PagingBuffer = filteredTable;
            }
            else
            {
                inData.TableData = filteredTable;
                inData.Note = String.Empty;
                _device.ExhBehavior.AddOneTimeSendData(inData);
                _device.ExhBehavior.GetData4CycleFunc[0].Initialize(inData);
            }
        }





        /// <summary>
        /// Проверка ограничения привязки.
        /// </summary>
        public bool CheckContrains(UniversalInputType inData)
        {
            if (Contrains == null)
                return true;

            return inData.TypeTrain != Contrains.TypeTrain &&
                   inData.Event != Contrains.Event;
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