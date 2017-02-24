using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Communication.TcpIp;
using CommunicationDevices.Infrastructure;
using CommunicationDevices.Infrastructure.VidorDataProvider;
using Timer = System.Timers.Timer;


namespace CommunicationDevices.Behavior.TcpIpBehavior
{
    public class ExhangeTcpIpBehavior : IExhangeBehavior, IDisposable
    {

        #region Fields

        private const double PeriodTimer = 10000;
        private readonly Timer _timer;


        private readonly byte _countRow = 1; //кол-во строк на табло   DEBUG!!!!!!!!!!!!!

     
        #endregion



        #region prop

        public MasterTcpIp MasterTcpIp { get; set; }    
    
        public ReadOnlyCollection<UniversalInputType> Data4CycleFunc { get; set; }
        public int NumberPort { get; }

        public bool IsOpen => MasterTcpIp.IsConnect;

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

        public ExhangeTcpIpBehavior(string connectionString, byte maxCountFaildRespowne, int timeRespown)
        {
            var strArr = connectionString.Split(':');
            if (strArr.Length != 2)
                return;
           
            var ip = strArr[0];
            NumberPort = int.Parse(strArr[1]);

            MasterTcpIp= new MasterTcpIp(ip, NumberPort, timeRespown, maxCountFaildRespowne);
            MasterTcpIp.PropertyChanged += MasterTcpIp_PropertyChanged;

            Data4CycleFunc = new ReadOnlyCollection<UniversalInputType>(new List<UniversalInputType> { new UniversalInputType { TableData = new List<UniversalInputType>() } });  //данные для 1-ой циклической функции
        }

        #endregion




        #region Rx

        public ISubject<IExhangeBehavior> IsDataExchangeSuccessChange { get; } = new Subject<IExhangeBehavior>();
        public ISubject<IExhangeBehavior> IsConnectChange { get; } = new Subject<IExhangeBehavior>();
        public ISubject<IExhangeBehavior> LastSendDataChange { get; } = new Subject<IExhangeBehavior>();

        #endregion




        #region EventHandler

        private void MasterTcpIp_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var maesteTcpIp = sender as MasterTcpIp;
            if (maesteTcpIp != null)
            {
                if (e.PropertyName == "IsConnect")
                    IsConnect = MasterTcpIp.IsConnect;
            }
        }

        #endregion






        #region Methode

        public void CycleReConnect(ICollection<Task> backGroundTasks = null)
        {
            var task = MasterTcpIp.ReConnect();       //выполняется фоновая задача, пока не подключится к серверу.
            backGroundTasks?.Add(task);
        }


        public void StartCycleExchange()
        {
            return;
        }


        public void StopCycleExchange()
        {
            return;
        }


        public async void AddOneTimeSendData(UniversalInputType inData)
        {
            if (MasterTcpIp.IsConnect)
            {
               // ----------------------
                //Вывод на табличное табло построчной информации
                if (inData?.TableData != null)
                {
                    var filteredData = inData.TableData;
                    //фильтрация по ближайшему времени к текущему времени.
                    var timeSampling = inData.TableData.Count > _countRow ? UniversalInputType.GetFilteringByDateTimeTable(_countRow, filteredData) : filteredData;

                    timeSampling.ForEach(t => t.AddressDevice = "1");  //inData.AddressDevice
                    for (byte i = 0; i < _countRow; i++)
                    {
                        var writeTableProvider = (i < timeSampling.Count) ?
                            new PanelVidorTableWriteDataProvider { InputData = timeSampling[i], CurrentRow = (byte)(i + 1) } :                                                  // Отрисовка строк
                            new PanelVidorTableWriteDataProvider { InputData = new UniversalInputType { AddressDevice = inData.AddressDevice }, CurrentRow = (byte)(i + 1) };   // Обнуление строк

                        DataExchangeSuccess = await MasterTcpIp.RequestAndRespoune(writeTableProvider);
                        LastSendData = writeTableProvider.InputData;

                        await Task.Delay(1000);
                    }

                    //Запрос синхронизации времени
                    //var syncTimeProvider = new PanelVidorTableWriteDataProvider { InputData = new UniversalInputType { AddressDevice = inData.AddressDevice }, CurrentRow = 0xFF };
                    //DataExchangeSuccess = await MasterTcpIp.RequestAndRespoune(syncTimeProvider);
                }
            }

        }


        /// <summary>
        /// Изменение данных для циклических функций
        /// </summary>
        public ReadOnlyCollection<UniversalInputType> GetData4CycleFunc => Data4CycleFunc;

        #endregion




        #region IDisposable

        public void Dispose()
        {
            _timer?.Dispose();
            MasterTcpIp.PropertyChanged -= MasterTcpIp_PropertyChanged;
            MasterTcpIp.Dispose();
        }

        #endregion
    }
}