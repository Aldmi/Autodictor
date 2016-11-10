using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using CommunicationDevices.Infrastructure;

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

        public Queue<UniversalInputType> InDataQueue { get; set; } = new Queue<UniversalInputType>();
        public MasterSerialPort Port { get; set; }

        public bool IsConnect { get; set; }

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




        #region Methode

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
            var inData = (InDataQueue != null && InDataQueue.Any()) ? InDataQueue.Dequeue() : null;
            var writeProvider = new PanelMg6587WriteDataProvider() {InputData = inData };
            DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeProvider, ct);

            if (!IsConnect)
            {
                //Сработка события DisconectHandling()
            }

            //if (writeProvider.IsOutDataValid)
            //{
            //    //с outData девайс разберется сам writeProvider.OutputData
            //}
        }

        #endregion
    }
}