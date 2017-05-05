using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Communication.Annotations;
using Communication.TcpIp;
using CommunicationDevices.DataProviders;
using Timer = System.Timers.Timer;

namespace CommunicationDevices.Behavior.ExhangeBehavior.HttpBehavior
{
    public abstract class BaseExhangeHttpBehavior : IExhangeBehavior, IDisposable
    {
        #region Fields

        private readonly Timer _timer;

        #endregion




        #region prop

        public ReadOnlyCollection<UniversalInputType> Data4CycleFunc { get; set; }
        public int NumberPort { get; }

        public bool IsOpen => true;          //???

        private bool _isConnect;
        public bool IsConnect
        {
            get { return _isConnect; }
            set
            {
                if (_isConnect == value)
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

        public CancellationTokenSource Cts { get; set; } = new CancellationTokenSource();

        #endregion




        #region ctor

        protected BaseExhangeHttpBehavior(string connectionString, byte maxCountFaildRespowne, int timeRespown, double taimerPeriod)
        {
            string ip = null;
            var strArr = connectionString.Split(':');
            if (strArr.Length == 2)
            {
                ip = strArr[0];
                NumberPort = int.Parse(strArr[1]);
            }

            //MasterTcpIp = new MasterTcpIp(ip, NumberPort, timeRespown, maxCountFaildRespowne);
            //MasterTcpIp.PropertyChanged += MasterTcpIp_PropertyChanged;

            _timer = new Timer(taimerPeriod);
            _timer.Elapsed += OnTimedEvent;
        }

        #endregion





        #region Rx

        public ISubject<IExhangeBehavior> IsDataExchangeSuccessChange { get; } = new Subject<IExhangeBehavior>();
        public ISubject<IExhangeBehavior> IsConnectChange { get; } = new Subject<IExhangeBehavior>();
        public ISubject<IExhangeBehavior> LastSendDataChange { get; } = new Subject<IExhangeBehavior>();

        #endregion




        #region Methode

        public void CycleReConnect(ICollection<Task> backGroundTasks = null)
        {
            //
        }



        public void StartCycleExchange()
        {
            _timer.Enabled = true;
        }



        public void StopCycleExchange()
        {
            _timer.Enabled = false;
        }



        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            AddOneTimeSendData(GetData4CycleFunc[0]);
        }



        public abstract void AddOneTimeSendData(UniversalInputType inData);



        /// <summary>
        /// Изменение данных для циклических функций
        /// </summary>
        public ReadOnlyCollection<UniversalInputType> GetData4CycleFunc => Data4CycleFunc;


        #endregion





        #region IDisposable

        public void Dispose()
        {
            _timer?.Dispose();
            //MasterTcpIp.PropertyChanged -= MasterTcpIp_PropertyChanged;
            //MasterTcpIp.Dispose();
        }

        #endregion
    }
}