using CommunicationDevices.Behavior;
using CommunicationDevices.Behavior.BindingBehavior;
using CommunicationDevices.Behavior.SerialPortBehavior;
using CommunicationDevices.Infrastructure;
using CommunicationDevices.Settings;


namespace CommunicationDevices.Devices
{
    public class DeviceSp
    {
        #region Prop

        public int Id { get; private set; }
        public string Address { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public ISerialPortExhangeBehavior SpExhBehavior { get; }        //TODO: вынести в отдельный класс. чтобы поведение включало в себя Device. (по аналогии с ToPAthbehavior) 

        #endregion




        #region ctor

        protected DeviceSp(int id, string address, string name, string description, ISerialPortExhangeBehavior behavior)
        {
            Id = id;
            Address = address;
            Name = name;
            Description = description;
            SpExhBehavior = behavior;
        }

        public DeviceSp(XmlDeviceSerialPortSettings xmlSet, ISerialPortExhangeBehavior behavior) : this(xmlSet.Id, xmlSet.Address.ToString(), xmlSet.Name, xmlSet.Description, behavior)
        {
        }

        #endregion




        #region Methode

        public void AddOneTimeSendData(UniversalInputType inData)
        {
            inData.Address = Address;
            SpExhBehavior.AddOneTimeSendData(inData);
        }


        public void AddCycleFunc()
        {
            SpExhBehavior.AddCycleFunc();
        }

        #endregion
    }
}
