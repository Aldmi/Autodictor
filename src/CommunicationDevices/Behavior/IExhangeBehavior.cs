using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using CommunicationDevices.Infrastructure;

namespace CommunicationDevices.Behavior
{
    public interface IExhangeBehavior
    {
        UniversalInputType LastSendData { get; set; }
        ReadOnlyCollection<UniversalInputType> GetData4CycleFunc { get; }

        byte NumberSp { get; }                      //TODO: заменить на string
        bool IsOpen { get; }
        bool IsConnect { get;  set; }
        bool DataExchangeSuccess { get; set; }

        void CycleReConnect(ICollection<Task> backGroundTasks = null);
        void AddCycleFunc();
        void RemoveCycleFunc();

        void AddOneTimeSendData(UniversalInputType inData);


        ISubject<IExhangeBehavior> IsDataExchangeSuccessChange { get; }
        ISubject<IExhangeBehavior> IsConnectChange { get; }
        ISubject<IExhangeBehavior> LastSendDataChange { get; }
    }
}