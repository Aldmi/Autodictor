using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Subjects;
using System.Xml.Linq;
using CommunicationDevices.Behavior.ExhangeBehavior;
using CommunicationDevices.Behavior.GetDataBehavior.ConvertGetedData;
using CommunicationDevices.DataProviders;
using Library.Logs;

namespace CommunicationDevices.Behavior.GetDataBehavior
{
    public class BaseGetDataBehavior : IDisposable
    {
        #region prop
        //название поведения получения данных

        public string Name { get; set; }     

        //издатель события "данные получены и преобразованны в IEnumerable<UniversalInputType>"
        public ISubject<IEnumerable<UniversalInputType>> ConvertedDataChangeRx { get; } = new Subject<IEnumerable<UniversalInputType>>();

        //издатель события "изменения состояния соединения с сервером"
        public ISubject<IExhangeBehavior> ChangeConnectRx { get; }

        //издатель события "изменения состояния обмена данными"
        public ISubject<IExhangeBehavior> DataExchangeSuccessRx { get; }

        //конвертер в XDocument -> IEnumerable<UniversalInputType>
        public IInputDataConverter InputConverter { get; }

        public IDisposable GetStreamRxHandlerDispose { get; set; }

        #endregion




        #region ctor

        public BaseGetDataBehavior(string name, ISubject<IExhangeBehavior> changeConnectRx,
                                                ISubject<IExhangeBehavior> dataExchangeSuccessRx,
                                                ISubject<Stream> getStreamRx,
                                                IInputDataConverter inputConverter)
        {
            Name = name;
            ChangeConnectRx = changeConnectRx;
            DataExchangeSuccessRx = dataExchangeSuccessRx;
            GetStreamRxHandlerDispose = getStreamRx.Subscribe(GetStreamRxHandler);      //подписка на событие получения потока данных
            InputConverter = inputConverter;
        }

        #endregion




        /// <summary>
        /// Обработчик события получения потока данных от сервера апк-дк ВОЛГОГРАД (GET запрос)
        /// </summary>
        private void GetStreamRxHandler(Stream stream)
        {
            try
            {
                StreamReader reader = new StreamReader(stream);
                string text = reader.ReadToEnd();
                XDocument xDoc = XDocument.Parse(text);
                var apkDkVolgogradShedule = InputConverter.ParseXml2ApkDkschedule(xDoc);
                ConvertedDataChangeRx.OnNext(apkDkVolgogradShedule);
            }
            catch (Exception ex)
            {
                Log.log.Error($"метод ApkDkGetStreamChangesRx:  {ex.Message}");
            }
        }




        public void Dispose()
        {
            GetStreamRxHandlerDispose?.Dispose();
        }
    }
}