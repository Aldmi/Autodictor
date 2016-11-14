using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Communication.SerialPort;
using CommunicationDevices.Infrastructure;

namespace CommunicationDevices.Behavior.SerialPortBehavior
{
    public interface ISerialPortExhangeBehavior
    {
        Queue<UniversalInputType> InDataQueue { get; set; }
        UniversalInputType LastSendData { get; set; }

        byte NumberSp { get; }
        bool IsOpenSp { get; }
        bool IsConnect { get;  set; }
        bool DataExchangeSuccess { get; set; }

        void PortCycleReConnect(ICollection<Task> backGroundTasks = null);
        void AddOneTimeSendData(UniversalInputType inData);


        ISubject<ISerialPortExhangeBehavior> IsDataExchangeSuccessChange { get; }
        ISubject<ISerialPortExhangeBehavior> IsConnectChange { get; }

        //TODO: Добавить Rx событие  IsConnectChange.
    }
}