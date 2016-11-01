using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WCFCis2AvtodictorContract.Contract;
using WCFCis2AvtodictorContract.DataContract;


namespace MainExample.ClientWCF
{
    /// <summary>
    /// Клиент для общения с CIS.
    /// </summary>
    public class CisClient : IDisposable, INotifyCollectionChanged
    {
        private readonly Timer _timer;
        private const double PeriodTimer = 2000;
        private uint _tickCounter;
        private List<OperativeScheduleData> _operativeScheduleDatas;

        #region prop

        private ChannelFactory<IServerContract> ChannelFactory { get; }
        private IServerContract Proxy { get; set; }
        public bool IsConnect { get; private set; }
        public bool IsStart { get; private set; }

        public List<OperativeScheduleData> OperativeScheduleDatas
        {
            get
            {
                return _operativeScheduleDatas;
            }
            private set
            {

                _operativeScheduleDatas = value;
            }
        }

        public List<RegulatoryScheduleData> RegulatoryScheduleDatas { get; private set; }


        //TODO: добавить лог. Который хранит список строк а запись на диск осушенсвялет по команде.

        #endregion




        #region Ctor

        public CisClient(EndpointAddress endpointAddress)
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
        }

        #endregion





        //TODO: что будет при закрытии сервера с объектом Proxy.
        private async void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            //Отсчет тиков
            if (++_tickCounter >= uint.MaxValue)
                _tickCounter = 0;


            try
            {
                //ВРЕМЕННОЙ УРОВЕНЬ 1 мин
                if ((_tickCounter > 0) && ((_tickCounter % 1) == 0))
                {
                    OperativeScheduleDatas= new List<OperativeScheduleData>(await Proxy.GetOperativeSchedules("Вокзал 3"));
                    IsConnect = true;
                    //Log.Add("Оперативное расписание полученно", Info);
                }

                //ВРЕМЕННОЙ УРОВЕНЬ 10 мин
                if ((_tickCounter > 0) && ((_tickCounter % 60) == 0))
                {

                }

                //ВРЕМЕННОЙ УРОВЕНЬ 1 час
                if ((_tickCounter > 0) && ((_tickCounter % 360) == 0))
                {

                }

                //ВРЕМЕННОЙ УРОВЕНЬ 12 часов
                if ((_tickCounter % 4320) == 0)
                {

                }

                //ВРЕМЕННОЙ УРОВЕНЬ 1 сутки
                if ((_tickCounter % 8640) == 0)
                {

                }
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
        }






        #region Event

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion





        #region Disposable

        public void Dispose()
        {
            ChannelFactory?.Close();
            _timer?.Dispose();
        }

        #endregion
    }
}
