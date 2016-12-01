using System.Linq;
using CommunicationDevices.Behavior;
using CommunicationDevices.Behavior.SerialPortBehavior;
using CommunicationDevices.Infrastructure;
using CommunicationDevices.Settings;


namespace CommunicationDevices.Devices
{
    public class Device
    {
        #region Prop

        public int Id { get; private set; }
        public string Address { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public BindingType BindingType { get; private set; }

        public IExhangeBehavior ExhBehavior { get; }        //TODO: вынести в отдельный класс. чтобы поведение включало в себя Device. (по аналогии с ToPAthbehavior) 

        #endregion




        #region ctor

        protected Device(int id, string address, string name, string description, IExhangeBehavior behavior)
        {
            Id = id;
            Address = address;
            Name = name;
            Description = description;
            ExhBehavior = behavior;
        }

        public Device(XmlDeviceSerialPortSettings xmlSet, IExhangeBehavior behavior) : this(xmlSet.Id, xmlSet.Address.ToString(), xmlSet.Name, xmlSet.Description, behavior)
        {
            BindingType = xmlSet.BindingType;
        }

        #endregion




        #region Methode

        public void AddOneTimeSendData(UniversalInputType inData)
        {
            inData.AddressDevice= Address;
            ExhBehavior.AddOneTimeSendData(inData);
        }


        public void AddCycleFunc()
        {
            if (ExhBehavior.Data4CycleFunc != null && ExhBehavior.Data4CycleFunc.Any())
            {

                ExhBehavior.Data4CycleFunc[0].AddressDevice = Address; //передадим данные для 1-ой циклической функции 
                ExhBehavior.AddCycleFunc();
            }
        }

        #endregion
    }
}
