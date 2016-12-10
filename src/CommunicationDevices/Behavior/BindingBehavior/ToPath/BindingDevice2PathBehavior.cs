using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using CommunicationDevices.Devices;
using CommunicationDevices.Infrastructure;



namespace CommunicationDevices.Behavior.BindingBehavior.ToPath
{

    /// <summary>
    /// привязка устройства к списку путей (1 табло может обслуживать несколько путей)
    /// Если список путей пуст, то привязка считается ко всем путям и обслудивается как вывод табличной информации.
    /// </summary>
    public class Binding2PathBehavior : IBinding2PathBehavior
    {
        private readonly Device _device;
        public IEnumerable<byte> CollectionPathNumber { get; }
        public string GetDeviceName => _device.Name;
        public int GetDeviceId=> _device.Id;




        public Binding2PathBehavior(Device device, IEnumerable<byte> pathNumbers)
        {
            _device = device;
            CollectionPathNumber = pathNumbers;
        }




        public string GetDevicesName4Path(byte pathNumber)
        {
            //привязка на все пути
            if (!CollectionPathNumber.Any())                              
                return $"{GetDeviceId}: {_device.Name}";

            //привязка на указанные пути
            var result = CollectionPathNumber.Contains(pathNumber) ? $"{GetDeviceId}: {_device.Name}" : null;
            return result;
        }




        public void SendMessage4Path(UniversalInputType inData, byte pathNumber)
        {
            //привязка на несколько путей
            if (!CollectionPathNumber.Any() || CollectionPathNumber.Count() > 1)
            {
                if (!string.IsNullOrWhiteSpace(inData.Event))               //ДОБАВИТЬ В ТАБЛ.
                {
                    _device.ExhBehavior.GetData4CycleFunc[0].TableData.Add(inData);  // Изменили данные для циклического опроса
                      
                }
                else                                                         //УДАЛИТЬ ИЗ ТАБЛ.
                {
                    var removeItem = _device.ExhBehavior.GetData4CycleFunc[0].TableData.FirstOrDefault(p => p.PathNumber == pathNumber.ToString());
                    if (removeItem != null)
                    {
                        _device.ExhBehavior.GetData4CycleFunc[0].TableData.Remove(removeItem);
                    }
                }
            }
            else
            {
                //привязка на указанные пути
                _device.AddCycleFuncData(0, inData);
            }
            _device.AddOneTimeSendData(_device.ExhBehavior.GetData4CycleFunc[0]); // Отправили однократный запрос (выставили запрос сразу на выполнение)
        }
    }
}