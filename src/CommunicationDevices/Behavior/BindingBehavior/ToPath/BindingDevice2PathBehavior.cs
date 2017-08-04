﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;
using CommunicationDevices.Settings;


namespace CommunicationDevices.Behavior.BindingBehavior.ToPath
{

    /// <summary>
    /// привязка устройства к списку путей (1 табло может обслуживать несколько путей)
    /// Если список путей пуст, то привязка считается ко всем путям и обслуживается как вывод табличной информации.
    /// </summary>
    public class Binding2PathBehavior : IBinding2PathBehavior
    {
        private readonly Device _device;
        public IEnumerable<string> CollectionPathNumber { get; }
        public Conditions Conditions { get; }
        public string GetDeviceName => _device.Name;
        public int GetDeviceId => _device.Id;
        public string GetDeviceAddress => _device.Address;
        public DeviceSetting GetDeviceSetting => _device.Setting;




        public Binding2PathBehavior(Device device, IEnumerable<string> pathNumbers, Conditions conditions)
        {
            _device = device;
            CollectionPathNumber = pathNumbers;
            Conditions = conditions;
        }




        public string GetDevicesName4Path(string pathNumber)
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
            //проверка ограничения.
            if (inData.Command != Command.Delete)
            {
                if (!checkContrains(inData))
                    return;
            }

            //привязка на несколько путей
            if (CollectionPathNumber.Any())
            {
                switch (inData.Command)
                { 
                    //ДОБАВИТЬ В ТАБЛ.
                    case Command.View:             
                        _device.ExhBehavior.GetData4CycleFunc[0].TableData.Add(inData);  // Изменили данные для циклического опроса
                        break;

                    //УДАЛИТЬ ИЗ ТАБЛ.
                    case Command.Delete:
                        var removeItem = _device.ExhBehavior.GetData4CycleFunc[0].TableData.FirstOrDefault(p => (p.Id == inData.Id) && (p.NumberOfTrain == numberOfTrain));
                        if (removeItem != null)
                        {
                            _device.ExhBehavior.GetData4CycleFunc[0].TableData.Remove(removeItem);
                        }
                        break;

                    // ОБНОВИТЬ В ТАБЛ.
                    case Command.Update:
                        var updateItem = _device.ExhBehavior.GetData4CycleFunc[0].TableData.FirstOrDefault(p => (p.Id == inData.Id) && (p.NumberOfTrain == numberOfTrain));
                        if (updateItem != null)
                        {
                            var indexUpdateItem = _device.ExhBehavior.GetData4CycleFunc[0].TableData.IndexOf(updateItem);
                            _device.ExhBehavior.GetData4CycleFunc[0].TableData[indexUpdateItem] = inData;
                        }
                        break;
                }
            }

            // Отправили однократный запрос (выставили запрос сразу на выполнение)
            _device.AddOneTimeSendData(_device.ExhBehavior.GetData4CycleFunc[0]); 
        }



        /// <summary>
        /// Проверка ограничения првязки.
        /// </summary>
        public bool CheckContrains(UniversalInputType inData)
        {
            if (!inData.IsActive)
                return false;

            if (Conditions == null)
                return true;

            return Conditions.CheckContrains(inData);
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
                TypeTrain = TypeTrain.None,
                TableData = new List<UniversalInputType>() { new UniversalInputType() }
            };
            inData.Message = $"ПОЕЗД:{inData.NumberOfTrain}, ПУТЬ:{inData.PathNumber}, СОБЫТИЕ:{inData.Event}, СТАНЦИИ:{inData.Stations}, ВРЕМЯ:{inData.Time.ToShortTimeString()}";

            _device.AddCycleFuncData(0, inData);
        }

    }
}