using System.Collections.Generic;

namespace CommunicationDevices.Behavior.BindingBehavior.ToPath
{
    public interface IBinding2PathBehavior
    {
         string GetDevicesName4Path(byte pathNumber);

         IEnumerable<byte> CollectionPathNumber { get; }
         string GetDeviceName { get; }

         void SendMessage4Path(string message);
    }
}