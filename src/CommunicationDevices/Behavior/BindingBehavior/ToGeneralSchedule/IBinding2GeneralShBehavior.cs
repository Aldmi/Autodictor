using System;
using CommunicationDevices.Infrastructure;

namespace CommunicationDevices.Behavior.BindingBehavior.ToGeneralSchedule
{
    public interface IBinding2GeneralSchedule
    {
        bool IsPaging { get; }
        void InitializePagingBuffer(UniversalInputType inData, Func<UniversalInputType, bool> checkContrains);
        bool CheckContrains(UniversalInputType inData);
    }
}