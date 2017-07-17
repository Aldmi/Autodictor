using System.Collections.Generic;
using System.Linq;
using CommunicationDevices.Behavior.BindingBehavior.ToGeneralSchedule;

namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings
{

    //ToPath: 1,2 - привязка к пути с перечислением номеров путей
    //ToPath: Все - привязка ко всем путям
    //ToGeneral - привязка к главному табло с расписанием
    //ToArrivalAndDeparture - привязка к табло отправление / прибытие поездов
    //ToStatic - привязка к форме отправки статической информации.
    public enum BindingType { None, ToPath, ToGeneral, ToArrivalAndDeparture, ToStatic }

    public class XmlBindingSetting
    {
        #region prop

        public BindingType BindingType { get; }
        public IEnumerable<string> PathNumbers { get; }          // при привязке на путь
        public SourceLoad SourceLoad { get; }                  // при привязке к расписанию

        #endregion




        #region ctor

        public XmlBindingSetting(string binding)
        {
            if (string.IsNullOrEmpty(binding))
            {
                BindingType = BindingType.None;
            }
            else
            if (binding.ToLower().Contains("togeneral"))
            {
                BindingType = BindingType.ToGeneral;
                if (binding.ToLower().Contains("главноеокно"))
                {
                    SourceLoad = SourceLoad.MainWindow;
                }
                else
                if (binding.ToLower().Contains("окнорасписанияоперативное"))
                {
                    SourceLoad = SourceLoad.SheduleOperative;
                }
                else
                if (binding.ToLower().Contains("окнорасписания"))
                {
                    SourceLoad = SourceLoad.Shedule;
                }
            }
            else
            if (binding.ToLower() == "toarrivalanddeparture")
            {
                BindingType = BindingType.ToArrivalAndDeparture;
            }
            else
            if (binding.ToLower().Contains("topath:"))
            {
                BindingType = BindingType.ToPath;
                var pathNumbers = new string(binding.SkipWhile(c => c != ':').Skip(1).ToArray()).Split(',');
                PathNumbers = (pathNumbers.First() == string.Empty) ? new List<string>() : pathNumbers.ToList();
            }
            else
            if (binding.ToLower().Contains("tostatic:"))
            {
                BindingType = BindingType.ToStatic;
            }

        }

        #endregion
    }
}