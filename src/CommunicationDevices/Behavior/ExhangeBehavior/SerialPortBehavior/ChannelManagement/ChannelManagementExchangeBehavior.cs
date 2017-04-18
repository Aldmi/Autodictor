using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using CommunicationDevices.DataProviders;
using CommunicationDevices.DataProviders.ChannelManagementDataProvider;

namespace CommunicationDevices.Behavior.ExhangeBehavior.SerialPortBehavior.ChannelManagement
{

    /// <summary>
    /// ПОВЕДЕНИЕ ОБМЕНА ДАННЫМИ УСТРОЙТСВА КОНФИГУРИРОВАНИЯ ЗВУКОВОГО ОБОРУДОВАНИЯ ПО ПОСЛЕД. ПОРТУ
    /// </summary>
    public class ChannelManagementExchangeBehavior : BaseExhangeSpBehavior
    {
        #region ctor

        public ChannelManagementExchangeBehavior(MasterSerialPort port, ushort timeRespone, byte maxCountFaildRespowne)
            : base(port, timeRespone, maxCountFaildRespowne)
        {
        }

        #endregion




        #region OverrideMembers

        protected override List<Func<MasterSerialPort, CancellationToken, Task>> ListCycleFuncs { get; set; }
        protected override async Task OneTimeExchangeService(MasterSerialPort port, CancellationToken ct)
        {
            var inData = (InDataQueue != null && InDataQueue.Any()) ? InDataQueue.Dequeue() : null;  //хранит адресс устройства.
            if (inData != null)
            {
                var writeProvider = new ChannelManagementWriteDataProvider(inData.SoundChanels) {InputData = inData};
                DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeProvider, ct);

                // LastSendData = writeProvider.InputData;

                if (writeProvider.IsOutDataValid)
                {
                    // Log.log.Trace(""); //TODO: возможно передавать в InputData ID устройства и имя.
                }

                //await Task.Delay(600, ct);  //задержка для задания периода опроса. 
            }
        }

        #endregion
    }
}