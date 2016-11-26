using System.Collections.Generic;
using CommunicationDevices.Devices;



namespace CommunicationDevices.Behavior.BindingBehavior.ToGeneralSchedule
{
    public class BindingDevice2GeneralShBehavior : IBinding2GeneralSchedule
    {
        private readonly Device _device;
        public string GetDeviceName => _device.Name;


        //TODO: передать список общего расписания. 
        public BindingDevice2GeneralShBehavior(Device device)
        { 
            _device = device;
        }

        //TODO: написать Пагинатор для списка. С кодом подписки на событие генерации N элементов списка.
        //TODO: Издатель события (Rx) срабатывает при обновлении списка расписания и передает это расписание.  
    }
}