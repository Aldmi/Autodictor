using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using CommunicationDevices.Infrastructure;
using System.Reactive.Subjects;
using Castle.Components.DictionaryAdapter;


namespace CommunicationDevices.Behavior.SerialPortBehavior
{

    public abstract class BaseExhangeSpBehavior : ISerialPortExhangeBehavior
    {
        #region Fields

        private readonly byte _maxCountFaildRespowne;
        private byte _countFaildRespowne;
        protected readonly ushort TimeRespone;


        #endregion




        #region Prop

        protected MasterSerialPort Port { get; }

        public Queue<UniversalInputType> InDataQueue { get; set; } = new Queue<UniversalInputType>();

        public UniversalInputType[] Data4CycleFunc { get; set; } = new UniversalInputType[1];        // для каждой циклической функции свои данные. 

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


        private UniversalInputType _lastSendData;
        public UniversalInputType LastSendData
        {
            get { return _lastSendData; }
            set
            {
                _lastSendData = value;
                LastSendDataChange.OnNext(this);
            }
        }

        #endregion




        #region ctor

        protected BaseExhangeSpBehavior(MasterSerialPort port, ushort timeRespone, byte maxCountFaildRespowne)
        {
            Port = port;
            TimeRespone = timeRespone;
            _maxCountFaildRespowne = maxCountFaildRespowne;
        }

        #endregion




        #region Rx

        public ISubject<ISerialPortExhangeBehavior> IsDataExchangeSuccessChange { get; } = new Subject<ISerialPortExhangeBehavior>();
        public ISubject<ISerialPortExhangeBehavior> IsConnectChange { get; } = new Subject<ISerialPortExhangeBehavior>();
        public ISubject<ISerialPortExhangeBehavior> LastSendDataChange { get; } = new Subject<ISerialPortExhangeBehavior>();
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


        /// <summary>
        /// Добавление однократно вызываемых функций
        /// </summary>
        public void AddOneTimeSendData(UniversalInputType inData)
        {
            if (inData != null)
            {
                InDataQueue.Enqueue(inData);
                Port.AddOneTimeSendData(OneTimeExchangeService);
            }
        }


        /// <summary>
        /// Добавление циклических функций.
        /// Поведение устройства определяет нужное количество циклических функций. Добавляются все функции в очередь порта
        /// </summary>
        public void AddCycleFunc()
        {
            ListCycleFuncs?.ForEach(func=> Port.AddCycleFunc(func));
        }

        /// <summary>
        /// Удаление циклических функций.
        /// Удаляются все циклические функции из очереди порта.
        /// </summary>
        public void RemoveCycleFunc()
        {
            ListCycleFuncs?.ForEach(func => Port.RemoveCycleFunc(func));
        }




        protected abstract List<Func<MasterSerialPort, CancellationToken, Task>> ListCycleFuncs { get; set; }
        protected abstract Task OneTimeExchangeService(MasterSerialPort port, CancellationToken ct);

        #endregion
    }
}