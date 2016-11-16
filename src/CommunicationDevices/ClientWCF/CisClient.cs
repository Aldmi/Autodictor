using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.ServiceModel;
using System.Timers;
using CommunicationDevices.Devices;
using WCFCis2AvtodictorContract.Contract;
using WCFCis2AvtodictorContract.DataContract;


namespace CommunicationDevices.ClientWCF
{
    /// <summary>
    /// Клиент для общения с CIS.
    /// </summary>
    public class CisClient : IDisposable
    {
        #region Fields

        private const double PeriodTimer = 5000;
        private const uint MinutLevel = (uint)(5000 / PeriodTimer); //60000
        private const uint TenMinutLevel = (uint)((60000 * 10) / PeriodTimer);
        private const uint HouerLevel = (uint)((60000 * 60) / PeriodTimer);
        private const uint TvelwHouerLevel = (uint)((60000 * 60 * 12) / PeriodTimer);
        private const uint DayLevel = (uint)((60000 * 60 * 24) / PeriodTimer);

        private readonly Timer _timer;
        private uint _tickCounter;

        private List<OperativeScheduleData> _operativeScheduleDatas;
        private bool _isConnect;

        #endregion




        #region prop

        private ChannelFactory<IServerContract> ChannelFactory { get; }
        private IServerContract Proxy { get; set; }

        public bool IsConnect
        {
            get { return _isConnect; }
            private set
            {
                if (value == _isConnect) return;
                _isConnect = value;
                IsConnectChange.OnNext(IsConnect);
            }
        }
        public bool IsStart { get; private set; }

        public List<OperativeScheduleData> OperativeScheduleDatas
        {
            get
            {
                return _operativeScheduleDatas;
            }
            private set
            {
                if (value == _operativeScheduleDatas) return;
                _operativeScheduleDatas = value;
                OperativeScheduleDatasChange.OnNext(OperativeScheduleDatas);
            }
        }

        public IEnumerable<DeviceSp> Devices { get; set; }

        //TODO: добавить лог. Который хранит список строк а запись на диск осушенсвялет по команде.

        #endregion




        #region Ctor

        public CisClient(EndpointAddress endpointAddress, IEnumerable<DeviceSp> devices)
        {
            BasicHttpBinding binding = new BasicHttpBinding
            {
                OpenTimeout = new TimeSpan(0, 0, 10),
                CloseTimeout = new TimeSpan(0, 0, 10),
                SendTimeout = new TimeSpan(0, 0, 30)
            };

            ChannelFactory = new ChannelFactory<IServerContract>(binding, endpointAddress);
            Proxy = ChannelFactory.CreateChannel();
            _timer = new Timer(PeriodTimer);
            _timer.Elapsed += OnTimedEvent;

            Devices = devices;
        }

        #endregion




        #region Rx

        public ISubject<bool> IsConnectChange { get; } = new Subject<bool>();
        public ISubject<List<OperativeScheduleData>> OperativeScheduleDatasChange { get; } = new Subject<List<OperativeScheduleData>>();

        #endregion




        private async void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            try
            {
                //ВРЕМЕННОЙ УРОВЕНЬ 1 мин
                if (((_tickCounter % MinutLevel) == 0))
                {
                    OperativeScheduleDatas = new List<OperativeScheduleData>(await Proxy.GetOperativeSchedules("Вокзал 1"));
                    IsConnect = true;
                    //Log.Add("Оперативное расписание полученно", Info);


                    //Pull модель опроса списка устройств. Перебираем список всех устройств (скрытых под интерфесом, ограничивающий доступ только к нужным данным)
                    // На каждое диагностируемое сво-во устройства формирум DiagnosticData и помещем в список.
                    var listDiagnostic = Devices?.Select(d => new DiagnosticData
                    {
                        DeviceNumber = d.Id,
                        DeviceName = d.Name,
                        Fault = d.SpExhBehavior.IsConnect ? "Нормальная работа" : "НЕ на связи",
                        Status = d.SpExhBehavior.IsConnect ? 100 : -100,
                    }).ToList();
                    Proxy.SetDiagnostics("Вокзал 1", listDiagnostic);
                }

                //ВРЕМЕННОЙ УРОВЕНЬ 10 мин
                if (((_tickCounter % TenMinutLevel) == 0))
                {

                }

                //ВРЕМЕННОЙ УРОВЕНЬ 1 час
                if (((_tickCounter % HouerLevel) == 0))
                {

                }

                //ВРЕМЕННОЙ УРОВЕНЬ 12 часов
                if ((_tickCounter % TvelwHouerLevel) == 0)
                {

                }

                //ВРЕМЕННОЙ УРОВЕНЬ 1 сутки
                if ((_tickCounter % DayLevel) == 0)
                {

                }

                //Отсчет тиков
                if (++_tickCounter >= uint.MaxValue)
                    _tickCounter = 0;
            }
            catch (EndpointNotFoundException ex)             //Конечная точка не найденна.
            {
                IsConnect = false;
                //Log.Add("ex.ToString()", Error);
            }
            catch (Exception ex)
            {
                //Log.Add("ex.ToString()", Error);
            }
        }




        public void Start()
        {
            _timer.Enabled = true;
            IsStart = true;
        }


        public void Stop()
        {
            _timer.Enabled = false;
            IsStart = false;
            IsConnect = false;
        }





        #region Disposable

        public void Dispose()
        {
            ChannelFactory?.Close();
            _timer?.Dispose();
        }

        #endregion
    }
}
