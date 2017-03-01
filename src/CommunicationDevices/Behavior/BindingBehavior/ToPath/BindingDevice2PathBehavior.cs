using System;
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
    /// Если список путей пуст, то привязка считается ко всем путям и обслуживается как вывод табличной информации.
    /// </summary>
    public class Binding2PathBehavior : IBinding2PathBehavior
    {
        private readonly Device _device;
        public IEnumerable<byte> CollectionPathNumber { get; }
        public UniversalInputType Contrains { get; }
        public string GetDeviceName => _device.Name;
        public int GetDeviceId => _device.Id;
        public string GetDeviceAddress => _device.Address;



        public Binding2PathBehavior(Device device, IEnumerable<byte> pathNumbers, UniversalInputType contrains)
        {
            _device = device;
            CollectionPathNumber = pathNumbers;
            Contrains = contrains;
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




        public void SendMessage4Path(UniversalInputType inData, string numberOfTrain, Func<UniversalInputType, bool> checkContrains)
        {
            //проверка ограничения
            if (!checkContrains(inData))
                return;

            //привязка на несколько путей
            if (!CollectionPathNumber.Any() || CollectionPathNumber.Count() > 1)
            {
                switch (inData.Command)
                { 
                    //ДОБАВИТЬ В ТАБЛ.
                    case Command.View:             
                        _device.ExhBehavior.GetData4CycleFunc[0].TableData.Add(inData);  // Изменили данные для циклического опроса
                        break;

                    //УДАЛИТЬ ИЗ ТАБЛ.
                    case Command.Clear:
                        var removeItem = _device.ExhBehavior.GetData4CycleFunc[0].TableData.FirstOrDefault(p => p.PathNumber == inData.PathNumber.ToString() && (p.NumberOfTrain == numberOfTrain));
                        if (removeItem != null)
                        {
                            _device.ExhBehavior.GetData4CycleFunc[0].TableData.Remove(removeItem);
                        }
                        break;

                    // ОБНОВИТЬ В ТАБЛ.
                    case Command.Update:
                        var updateItem = _device.ExhBehavior.GetData4CycleFunc[0].TableData.FirstOrDefault(p => p.PathNumber == inData.PathNumber.ToString() && (p.NumberOfTrain == numberOfTrain));
                        if (updateItem != null)
                        {
                            var indexUpdateItem = _device.ExhBehavior.GetData4CycleFunc[0].TableData.IndexOf(updateItem);
                            _device.ExhBehavior.GetData4CycleFunc[0].TableData[indexUpdateItem] = inData;
                        }
                        break;
                        
                }
            }
            else
            {
                //привязка на указанные пути
                _device.AddCycleFuncData(0, inData);
            }

            _device.AddOneTimeSendData(_device.ExhBehavior.GetData4CycleFunc[0]); // Отправили однократный запрос (выставили запрос сразу на выполнение)
        }



        /// <summary>
        /// Проверка ограничения првязки.
        /// </summary>
        public bool CheckContrains(UniversalInputType inData)
        {
            if (Contrains == null)
                return true;

            return inData.TypeTrain != Contrains.TypeTrain &&
                   inData.Event != Contrains.Event;
        }



        /// <summary>
        /// Инициализация начальной строки вывода на дисплей.
        /// Из всех привязанных путей берется первый путь для отображения.
        /// </summary>
        public void InitializeDevicePathInfo()
        {
            var inData = new UniversalInputType
            {
                NumberOfTrain = "   ",
                PathNumber = (CollectionPathNumber != null && CollectionPathNumber.Any()) ? CollectionPathNumber.First().ToString() : "   ",
                Event = "   ",
                Time = DateTime.MinValue,
                Stations = "   ",
                Note = "   ",
                TypeTrain = TypeTrain.None
            };
            inData.Message = $"ПОЕЗД:{inData.NumberOfTrain}, ПУТЬ:{inData.PathNumber}, СОБЫТИЕ:{inData.Event}, СТАНЦИИ:{inData.Stations}, ВРЕМЯ:{inData.Time.ToShortTimeString()}";


            if (_device.ExhBehavior.GetData4CycleFunc[0].TableData != null)
            {
                _device.ExhBehavior.GetData4CycleFunc[0].TableData.Clear();
            }
            else
            {
                _device.AddCycleFuncData(0, inData);
            }
        }
    }
}