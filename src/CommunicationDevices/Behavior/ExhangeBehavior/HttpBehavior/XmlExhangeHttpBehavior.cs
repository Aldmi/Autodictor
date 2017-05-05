using System.Collections.Generic;
using System.Collections.ObjectModel;
using Communication.Interfaces;
using CommunicationDevices.DataProviders;


namespace CommunicationDevices.Behavior.ExhangeBehavior.HttpBehavior
{
    public class XmlExhangeHttpBehavior : BaseExhangeHttpBehavior
    {
        #region Prop

        public IExchangeDataProvider<UniversalInputType, byte> XmlExcangeDataProvider { get; set; }

        #endregion






        #region ctor

        public XmlExhangeHttpBehavior(string connectionString, byte maxCountFaildRespowne, int timeRespown, double taimerPeriod, IExchangeDataProvider<UniversalInputType, byte> xmlExcangeDataProvider)
            : base(connectionString, maxCountFaildRespowne, timeRespown, taimerPeriod)
        {
            Data4CycleFunc = new ReadOnlyCollection<UniversalInputType>(new List<UniversalInputType> { new UniversalInputType { TableData = new List<UniversalInputType>() } });  //данные для 1-ой циклической функции
            XmlExcangeDataProvider = xmlExcangeDataProvider;
        }

        #endregion



        private bool _sendLock;
        public override void AddOneTimeSendData(UniversalInputType inData)
        {
            if (_sendLock)
                return;

            _sendLock = true;


            if (inData?.TableData != null)
            {

                XmlExcangeDataProvider.InputData = inData;
                //DataExchangeSuccess = //await MasterTcpIp.RequestAndRespoune(forTableViewDataProvide);
                LastSendData = XmlExcangeDataProvider.InputData;
            }


            _sendLock = false;
        }
    }
}