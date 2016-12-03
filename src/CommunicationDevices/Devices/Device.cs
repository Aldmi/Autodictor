using System.Linq;
using Castle.Core.Internal;
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

        public IExhangeBehavior ExhBehavior { get; }       

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


        public void AddCycleFuncData(int index, UniversalInputType inData)
        {
            inData.AddressDevice = Address;
            ExhBehavior.GetData4CycleFunc[index].Initialize(inData);     
        }



        public void AddCycleFunc()
        {
            ExhBehavior.GetData4CycleFunc.ForEach(c=> c.AddressDevice = Address);       //Добавить во все данные циклического обмена адресс.
            ExhBehavior.AddCycleFunc();
        }






        public void RemoveCycleFunc()
        {
            ExhBehavior.RemoveCycleFunc();
        }

        #endregion
    }
}
