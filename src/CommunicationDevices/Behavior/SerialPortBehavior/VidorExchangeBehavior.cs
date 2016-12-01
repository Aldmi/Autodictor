using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using CommunicationDevices.Infrastructure;
using CommunicationDevices.Infrastructure.DisplaySysDataProvider;
using CommunicationDevices.Infrastructure.VidorDataProvider;


namespace CommunicationDevices.Behavior.SerialPortBehavior
{
    public class VidorExchangeBehavior : BaseExhangeSpBehavior
    {
        #region ctor

        public VidorExchangeBehavior(MasterSerialPort port, ushort timeRespone, byte maxCountFaildRespowne)
            : base(port, timeRespone, maxCountFaildRespowne)
        {
            //добавляем циклические функции
            Data4CycleFunc= new ReadOnlyCollection<UniversalInputType>(new List<UniversalInputType> {new UniversalInputType()}) ;  //данные для 1-ой циклической функции
            ListCycleFuncs = new List<Func<MasterSerialPort, CancellationToken, Task>> {CycleExcangeService};                      // 1 циклическая функция
        }

        #endregion




        #region Methode

        private async Task CycleExcangeService(MasterSerialPort port, CancellationToken ct)
        {
            var writeProvider = new PanelVidorWriteDataProvider { InputData = Data4CycleFunc[0] };
            DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeProvider, ct);
            await Task.Delay(4000, ct);  //задержка для задания периода опроса.
        }

        #endregion





        #region OverrideMembers

        protected override sealed List<Func<MasterSerialPort, CancellationToken, Task>> ListCycleFuncs { get; set; }

        protected override async Task OneTimeExchangeService(MasterSerialPort port, CancellationToken ct)
        {
            LastSendData = (InDataQueue != null && InDataQueue.Any()) ? InDataQueue.Dequeue() : null;
            var writeProvider = new PanelVidorWriteDataProvider { InputData = LastSendData };
            DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeProvider, ct);


            if (writeProvider.IsOutDataValid)
            {
                //с outData девайс разберется сам writeProvider.OutputData
            }
        }

        #endregion
    }
}