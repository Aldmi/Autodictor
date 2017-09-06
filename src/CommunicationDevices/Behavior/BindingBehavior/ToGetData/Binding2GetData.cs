﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;

namespace CommunicationDevices.Behavior.BindingBehavior.ToGetData
{
    public class Binding2GetData : IBinding2GetData
    {
        #region prop

        private readonly Device _device;
        public string GetDeviceName => _device.Name;
        public int GetDeviceId => _device.Id;
        public string GetDeviceAddress => _device.Address;
        public DeviceSetting GetDeviceSetting => _device.Setting;

        public Subject<Stream> OutputDataChangeRx => _device.GetOutputDataChangeRx;

        #endregion




        #region ctor

        public Binding2GetData(Device device)
        {
            _device = device;
        }

        #endregion





        #region Metode

        public void SendMessage(UniversalInputType inData)
        {
            _device.AddCycleFuncData(0, inData);
            //_device.AddOneTimeSendData(_device.ExhBehavior.GetData4CycleFunc[0]);
        }

        #endregion
    }
}