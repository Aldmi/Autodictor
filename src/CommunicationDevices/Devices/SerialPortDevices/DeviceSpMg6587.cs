using System;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using Castle.Windsor;
using Communication.Interfaces;
using Communication.SerialPort;
using CommunicationDevices.Infrastructure;
using CommunicationDevices.Settings;


namespace CommunicationDevices.Devices.SerialPortDevices
{
    public class DeviceSpMg6587 : DeviceSp
    {
        #region ctor

        public DeviceSpMg6587(XmlDeviceSerialPortSettings xmlSet, MasterSerialPort port) : base(xmlSet, port)
        {
          
        }

        #endregion




        protected override async Task ExchangeService(MasterSerialPort port, CancellationToken ct)
        {
            var writeProvider = new PanelMg6587WriteDataProvider() {InputData = InData};
            DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeProvider, ct);

            if (!IsConnect)
            {
                //Сработка события DisconectHandling()
            }

            if (writeProvider.IsOutDataValid)
            {
                //с outData девайс разберется сам writeProvider.OutputData
            }
        }

    }
}