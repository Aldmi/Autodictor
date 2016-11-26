using System.Collections.Generic;
using System.Linq;
using CommunicationDevices.Devices;
using CommunicationDevices.Infrastructure;

namespace CommunicationDevices.Behavior.BindingBehavior.ToPath
{

    /// <summary>
    /// привязка устройства к списку путей (1 табло может обслуживать несколько путей)
    /// </summary>
    public class Binding2PathDevice2PathBehavior : IBinding2PathBehavior
    {
        private readonly Device _device;
        public IEnumerable<byte> CollectionPathNumber { get; }

        public string GetDeviceName => _device.Name;


        public Binding2PathDevice2PathBehavior(Device device, IEnumerable<byte> pathNumbers)
        {
            _device = device;
            CollectionPathNumber = pathNumbers;
        }


        public string GetDevicesName4Path(byte pathNumber)
        {
            //привязка на все пути
            if (!CollectionPathNumber.Any())                              
                return $"{_device.Name}";

            //привязка на указанные пути
            var result = CollectionPathNumber.Contains(pathNumber) ? $"{_device.Name}" : null;
            return result;
        }


        public void SendMessage4Path(string message)
        {
            _device.AddOneTimeSendData(new UniversalInputType {Message = message});
            
        }
    }
}