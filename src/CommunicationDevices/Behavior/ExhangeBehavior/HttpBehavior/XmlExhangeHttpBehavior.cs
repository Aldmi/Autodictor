using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        public XmlExhangeHttpBehavior(string connectionString, string methode, string contentType, byte maxCountFaildRespowne, int timeRespowne, double taimerPeriod, IExchangeDataProvider<UniversalInputType, byte> xmlExcangeDataProvider)
            : base(connectionString, methode, contentType, maxCountFaildRespowne, timeRespowne, taimerPeriod)
        {
            Data4CycleFunc = new ReadOnlyCollection<UniversalInputType>(new List<UniversalInputType> { new UniversalInputType { TableData = new List<UniversalInputType>() } });  //данные для 1-ой циклической функции
            XmlExcangeDataProvider = xmlExcangeDataProvider;
        }

        #endregion






        private bool _sendLock;
        public override async void AddOneTimeSendData(UniversalInputType inData)
        {
            if (_sendLock)
                return;

            _sendLock = true;
            if (inData?.TableData != null)
            {
                XmlExcangeDataProvider.InputData = inData;
                DataExchangeSuccess = await ClientHttp.RequestAndRespoune(XmlExcangeDataProvider);
                LastSendData = XmlExcangeDataProvider.InputData;
            }
            _sendLock = false;
        }
    }
}