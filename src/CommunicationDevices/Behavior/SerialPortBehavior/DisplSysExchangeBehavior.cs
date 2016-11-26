using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using CommunicationDevices.Infrastructure;
using CommunicationDevices.Infrastructure.DisplaySysDataProvider;

namespace CommunicationDevices.Behavior.SerialPortBehavior
{

    public class DisplSysExchangeBehavior : BaseExhangeSpBehavior
    {
        #region ctor

        public DisplSysExchangeBehavior(MasterSerialPort port, ushort timeRespone, byte maxCountFaildRespowne)
            : base(port, timeRespone, maxCountFaildRespowne)
        {
            //добавляем циклические функции
            ListCycleFuncs = new List<Func<MasterSerialPort, CancellationToken, Task>> {CycleCheckConnectService};  
        }

        #endregion




        #region Methode

        private async Task CycleCheckConnectService(MasterSerialPort port, CancellationToken ct)
        {
            var readProvider = new PanelDispSysCheckConnectDataProvider { InputData = Data4CycleFunc[0] };
            DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, readProvider, ct);
            await Task.Delay(4000, ct);  //задержка для задания периода опроса.
        }

        #endregion





        #region OverrideMembers

        protected override sealed List<Func<MasterSerialPort, CancellationToken, Task>> ListCycleFuncs { get; set; }

        protected override async Task OneTimeExchangeService(MasterSerialPort port, CancellationToken ct)
        {
            LastSendData = (InDataQueue != null && InDataQueue.Any()) ? InDataQueue.Dequeue() : null;
            var writeProvider = new PanelDispSysWriteDataProvider { InputData = LastSendData };
            DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeProvider, ct);


            if (writeProvider.IsOutDataValid)
            {
                //с outData девайс разберется сам writeProvider.OutputData
            }
        }

        #endregion
    }
}