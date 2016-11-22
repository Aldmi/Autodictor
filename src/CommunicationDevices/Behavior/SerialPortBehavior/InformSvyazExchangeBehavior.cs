using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using CommunicationDevices.Infrastructure;


namespace CommunicationDevices.Behavior.SerialPortBehavior
{

    public class InformSvyazExchangeBehavior : BaseExhangeSpBehavior
    {
        #region ctor

        public InformSvyazExchangeBehavior(MasterSerialPort port, ushort timeRespone, byte maxCountFaildRespowne)
            : base(port, timeRespone, maxCountFaildRespowne)
        {
            ListCycleFuncs = new List<Func<MasterSerialPort, CancellationToken, Task>> {CycleCheckConnectService}; //добавляем циклические функции
        }

        #endregion




        #region Methode

        private async Task CycleCheckConnectService(MasterSerialPort port, CancellationToken ct)
        {
            var readProvider = new PanelInformSvyazCheckConnectDataProvider { InputData = Data4CycleFunc[0] };
            DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, readProvider, ct);
            await Task.Delay(4000, ct);  //задержка для задания периода опроса.
        }

        #endregion




        #region OverrideMembers

        protected override sealed List<Func<MasterSerialPort, CancellationToken, Task>> ListCycleFuncs { get; set; }

        protected override async Task OneTimeExchangeService(MasterSerialPort port, CancellationToken ct)
        {
            LastSendData = (InDataQueue != null && InDataQueue.Any()) ? InDataQueue.Dequeue() : null;
            var writeProvider = new PanelInformSvyazWriteDataProvider {InputData = LastSendData};
            DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeProvider, ct);


            if (writeProvider.IsOutDataValid)
            {
                //с outData девайс разберется сам writeProvider.OutputData
            }
        }

        #endregion
    }
}