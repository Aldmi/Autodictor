using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using CommunicationDevices.Infrastructure;
using System.Reactive.Subjects;


namespace CommunicationDevices.Behavior.SerialPortBehavior
{
    public class SpMg6587ExhangeBehavior : ISerialPortExhangeBehavior
    {
        #region Fields

        private readonly byte _maxCountFaildRespowne;
        private byte _countFaildRespowne;
        protected readonly ushort TimeRespone;


        #endregion




        #region Prop

        private MasterSerialPort Port { get; }

        public Queue<UniversalInputType> InDataQueue { get; set; } = new Queue<UniversalInputType>();
        public UniversalInputType LastSendData { get; set; }

        public byte NumberSp => Port.PortNumber;
        public bool IsOpenSp => Port.IsOpen;

        private bool _isConnect;
        public bool IsConnect
        {
            get { return _isConnect; }
            set
            {
                if(_isConnect == value)
                    return;

                _isConnect = value;
                IsConnectChange.OnNext(this);
            }
        }


        private bool _dataExchangeSuccess;
        public bool DataExchangeSuccess
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

                IsDataExchangeSuccessChange.OnNext(this);
            }
        }

        #endregion




        #region ctor

        public SpMg6587ExhangeBehavior(MasterSerialPort port, ushort timeRespone, byte maxCountFaildRespowne)
        {
            Port = port;
            TimeRespone = timeRespone;
            _maxCountFaildRespowne = maxCountFaildRespowne;
        }

        #endregion




        #region Rx

        public ISubject<ISerialPortExhangeBehavior> IsDataExchangeSuccessChange { get; } = new Subject<ISerialPortExhangeBehavior>();
        public ISubject<ISerialPortExhangeBehavior> IsConnectChange { get; } = new Subject<ISerialPortExhangeBehavior>();
        #endregion




        #region Methode

        public void PortCycleReConnect(ICollection<Task> backGroundTasks = null)
        {
            if (Port != null)
            {
                var taskSerialPort = Task.Factory.StartNew(async () =>
                {
                    if (await Port.CycleReConnect())
                    {
                        var taskPortEx = Port.RunExchange();
                        backGroundTasks?.Add(taskPortEx);
                    }
                });
                backGroundTasks?.Add(taskSerialPort);
            }
        }


        public void AddOneTimeSendData(UniversalInputType inData)
        {
            if (inData != null)
            {
                InDataQueue.Enqueue(inData);
                Port.AddOneTimeSendData(ExchangeService);
            }
        }


        private async Task ExchangeService(MasterSerialPort port, CancellationToken ct)
        {
            LastSendData = (InDataQueue != null && InDataQueue.Any()) ? InDataQueue.Dequeue() : null;
            var writeProvider = new PanelMg6587WriteDataProvider() {InputData = LastSendData };
            //DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeProvider, ct);
            DataExchangeSuccess = true;//!DataExchangeSuccess;//DEBUG


            //if (writeProvider.IsOutDataValid)
            //{
            //    //с outData девайс разберется сам writeProvider.OutputData
            //}
        }

        #endregion
    }
}