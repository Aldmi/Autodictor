using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using CommunicationDevices.Infrastructure;
using CommunicationDevices.Settings;


namespace CommunicationDevices.Devices.SerialPortDevices
{
    public abstract class DeviceSp
    {
        #region Fields

        private readonly byte _maxCountFaildRespowne;
        private byte _countFaildRespowne;
        protected readonly ushort TimeRespone;


        #endregion




        #region Prop

        public int Id { get; private set; }
        public string Address { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsConnect { get; private set; }

        private bool _dataExchangeSuccess;
        protected bool DataExchangeSuccess
        {
            get { return _dataExchangeSuccess; }
            set
            {
                _dataExchangeSuccess = value;
                if (_dataExchangeSuccess)
                {
                    _countFaildRespowne = 0;
                    IsConnect = true;
                }
                else
                {
                    if (_countFaildRespowne++ >= _maxCountFaildRespowne)
                    {
                        _countFaildRespowne = 0;
                        IsConnect = false;
                    }
                }
            }
        }


        public MasterSerialPort Port { get; set; }
        protected UniversalInputType InData { get; set; }
        #endregion




        #region ctor

        protected DeviceSp(int id, string address, string name, string description, byte maxCountFaildRespowne,
            ushort timeRespone)
        {
            Id = id;
            Address = address;
            Name = name;
            Description = description;
            _maxCountFaildRespowne = maxCountFaildRespowne;
            TimeRespone = timeRespone;
        }

        protected DeviceSp(XmlDeviceSerialPortSettings xmlSet, MasterSerialPort port) : this(xmlSet.Id, xmlSet.Address.ToString(), xmlSet.Name, xmlSet.Description, 5, xmlSet.TimeRespone)
        {
            Port = port;
        }

        #endregion




        #region Methode

        public void AddOneTimeSendData(UniversalInputType inData)
        {
            InData = inData;
            Port.AddOneTimeSendData(ExchangeService);
        }

        protected abstract Task ExchangeService(MasterSerialPort port, CancellationToken ct);

        #endregion
    }
}
