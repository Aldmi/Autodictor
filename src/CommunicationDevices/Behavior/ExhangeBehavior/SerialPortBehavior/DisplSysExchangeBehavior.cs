using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using CommunicationDevices.DataProviders.DisplaySysDataProvider;
using CommunicationDevices.Infrastructure;


namespace CommunicationDevices.Behavior.ExhangeBehavior.SerialPortBehavior
{

    /// <summary>
    /// ПОВЕДЕНИЕ ОБМЕНА ДАННЫМИ ТАБЛО "ДИСПЛЕЙНЫХ СИСТЕМ" ПО ПОСЛЕД. ПОРТУ
    /// </summary>
    public class DisplSysExchangeBehavior : BaseExhangeSpBehavior
    {
        #region ctor

        public DisplSysExchangeBehavior(MasterSerialPort port, ushort timeRespone, byte maxCountFaildRespowne)
            : base(port, timeRespone, maxCountFaildRespowne)
        {
            //добавляем циклические функции
            Data4CycleFunc = new ReadOnlyCollection<UniversalInputType>(new List<UniversalInputType> { new UniversalInputType { Event = "  ", NumberOfTrain = "  ", PathNumber = "  ", Stations = "  ", Time = DateTime.MinValue } });  //данные для 1-ой циклической функции
            ListCycleFuncs = new List<Func<MasterSerialPort, CancellationToken, Task>> { CycleExcangeService };                      // 1 циклическая функция
        }

        #endregion




        #region Methode

        private async Task CycleExcangeService(MasterSerialPort port, CancellationToken ct)
        {
            var writeProvider = new PanelDispSysWriteDataProvider { InputData = Data4CycleFunc[0] };
            DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeProvider, ct);
            await Task.Delay(1000, ct);  //задержка для задания периода опроса.


            if (writeProvider.IsOutDataValid)
            {
                // Log.log.Trace(""); //TODO: возможно передавать в InputData ID устройства и имя.
            }
        }

        #endregion





        #region OverrideMembers

        protected override sealed List<Func<MasterSerialPort, CancellationToken, Task>> ListCycleFuncs { get; set; }

        protected override async Task OneTimeExchangeService(MasterSerialPort port, CancellationToken ct)
        {
            LastSendData = (InDataQueue != null && InDataQueue.Any()) ? InDataQueue.Dequeue() : null;
            var writeProvider = new PanelDispSysWriteDataProvider { InputData = LastSendData };
            DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeProvider, ct);
            await Task.Delay(1000, ct);  //задержка для задания периода опроса.  
        }

        #endregion
    }
}