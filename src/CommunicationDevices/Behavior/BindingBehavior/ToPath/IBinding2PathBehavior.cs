using System.Collections.Generic;
using CommunicationDevices.Infrastructure;

namespace CommunicationDevices.Behavior.BindingBehavior.ToPath
{
    public interface IBinding2PathBehavior
    {
        string GetDevicesName4Path(byte pathNumber);

        IEnumerable<byte> CollectionPathNumber { get; }
        string GetDeviceName { get; }
        int GetDeviceId { get; }

        void SendMessage4Path(UniversalInputType inData, byte pathNumber);
    }
}