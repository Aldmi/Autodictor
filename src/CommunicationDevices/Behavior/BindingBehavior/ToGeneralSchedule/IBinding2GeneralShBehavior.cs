﻿using System;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;

namespace CommunicationDevices.Behavior.BindingBehavior.ToGeneralSchedule
{   
    public enum SourceLoad                       // источник загрузки
    {
        None,
        MainWindow,                              // Из главного окна (расписание на сутки)
        Shedule                                  // Из окна Распсиание
    }

    public interface IBinding2GeneralSchedule
    {
        bool IsPaging { get; }
        SourceLoad SourceLoad { get; set; }
        void InitializePagingBuffer(UniversalInputType inData, Func<UniversalInputType, bool> checkContrains);
        bool CheckContrains(UniversalInputType inData);

        DeviceSetting GetDeviceSetting { get; }
    }
}