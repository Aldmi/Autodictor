using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Communication.Interfaces;
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
        #region prop

        public IExchangeDataProvider<UniversalInputType, byte> WriteProvider { get; set; }

        #endregion




        #region ctor

        public ChannelManagementExchangeBehavior(MasterSerialPort port, ushort timeRespone, byte maxCountFaildRespowne, IExchangeDataProvider<UniversalInputType, byte> provider)
            : base(port, timeRespone, maxCountFaildRespowne)
        {
            WriteProvider = provider;
        }

        #endregion




        #region OverrideMembers

        protected override List<Func<MasterSerialPort, CancellationToken, Task>> ListCycleFuncs { get; set; }
        protected override async Task OneTimeExchangeService(MasterSerialPort port, CancellationToken ct)
        {
            var inData = (InDataQueue != null && InDataQueue.Any()) ? InDataQueue.Dequeue() : null;  //хранит адресс устройства.
            if (inData != null)
            {
                WriteProvider.InputData = inData;
                DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, WriteProvider, ct);

                if (WriteProvider.IsOutDataValid)
                {
                    // Log.log.Trace(""); //TODO: возможно передавать в InputData ID устройства и имя.
                }
            }
        }

        #endregion
    }
}