using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using CommunicationDevices.Infrastructure;

namespace CommunicationDevices.Behavior
{
    public interface IExhangeBehavior
    {
        Queue<UniversalInputType> InDataQueue { get; set; }
        UniversalInputType LastSendData { get; set; }

        ReadOnlyCollection<UniversalInputType> Data4CycleFunc { get; set; }

        byte NumberSp { get; }                      //заменить на string
        bool IsOpenSp { get; }
        bool IsConnect { get;  set; }
        bool DataExchangeSuccess { get; set; }

        void PortCycleReConnect(ICollection<Task> backGroundTasks = null);
        void AddOneTimeSendData(UniversalInputType inData);
        void AddCycleFunc();
        void RemoveCycleFunc();


        ISubject<IExhangeBehavior> IsDataExchangeSuccessChange { get; }
        ISubject<IExhangeBehavior> IsConnectChange { get; }
        ISubject<IExhangeBehavior> LastSendDataChange { get; }
    }
}