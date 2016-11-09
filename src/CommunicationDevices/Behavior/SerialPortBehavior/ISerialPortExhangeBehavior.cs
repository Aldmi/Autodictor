using Communication.SerialPort;
using CommunicationDevices.Infrastructure;

namespace CommunicationDevices.Behavior.SerialPortBehavior
{
    public interface ISerialPortExhangeBehavior
    {
         UniversalInputType InData { get; set; }
         MasterSerialPort Port { get; set; }
         bool IsConnect { get;  set; }
         bool DataExchangeSuccess { get; set; }

         void AddOneTimeSendData(UniversalInputType inData);


        //TODO: Добавить Rx событие  IsConnectChange.
    }
}