using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunicationDevices.DataProviders;
using CommunicationDevices.DataProviders.BuRuleDataProvider;
using CommunicationDevices.Rules.ExchangeRules;


namespace CommunicationDevices.Behavior.ExhangeBehavior.TcpIpBehavior
{
    public class ByRulesTableExchangeTcpIpBehavior : BaseExhangeTcpIpBehavior
    {
        #region Prop

        public MainRule MainRule { get; }
        public List<string> InternalAddressCollection { get; set; }      //адресс уст-ва нужный для протокола обмена.
        public int InternalPeriodTimer { get; set; }                     //Период опроса между устройствами подключенными к 1 TCP/Ip соединению.

        #endregion





        #region ctor

        public ByRulesTableExchangeTcpIpBehavior(string connectionString, List<string> internalAddress, byte maxCountFaildRespowne, int timeRespown, MainRule mainRule, int internalPeriodTimer)
            : base(connectionString, maxCountFaildRespowne, timeRespown, 12000)
        {
            InternalAddressCollection = internalAddress;
            InternalPeriodTimer = internalPeriodTimer;
            MainRule = mainRule;
            Data4CycleFunc = new ReadOnlyCollection<UniversalInputType>(new List<UniversalInputType> { new UniversalInputType { TableData = new List<UniversalInputType>() } });  //данные для 1-ой циклической функции
        }

        #endregion




        private bool _sendLock;
        public override async void AddOneTimeSendData(UniversalInputType inData)
        {
            if (_sendLock)
                return;

            _sendLock = true;

            if (MasterTcpIp.IsConnect)
            {
                if (!MainRule.ViewType.TableSize.HasValue)
                    return;

                var countRow = MainRule.ViewType.TableSize.Value;

                //Вывод на табличное табло построчной информации
                if (inData?.TableData != null)
                {
                    //Отправляем информацию для всех устройств, подключенных к данному TCP конвертору.
                    foreach (var internalAddr in InternalAddressCollection)
                    {
                        for (byte i = 0; i < countRow; i++)
                        {
                            //Определим какие из правил отрисовывают данную строку (вывод информации или пустой строки).
                            foreach (var exchangeRule in MainRule.ExchangeRules)
                            {
                                //фильтрация по ближайшему времени к текущему времени.
                                var filteredData = inData.TableData.Where(data => exchangeRule.CheckResolution(data)).ToList();
                                var timeSampling = inData.TableData.Count > countRow ? UniversalInputType.GetFilteringByDateTimeTable(countRow, filteredData) : filteredData;

                                timeSampling.ForEach(t => t.AddressDevice = internalAddr);

                                var currentRow = (byte)(i + 1);
                                var inputData = (i < timeSampling.Count) ? timeSampling[i] : new UniversalInputType { AddressDevice = internalAddr };

                                var forTableViewDataProvide = new ByRuleTableWriteDataProvider(exchangeRule)
                                {
                                    InputData = inputData,
                                    CurrentRow = currentRow
                                };

                                DataExchangeSuccess = await MasterTcpIp.RequestAndRespoune(forTableViewDataProvide);
                                LastSendData = forTableViewDataProvide.InputData; 
                            }

                            await Task.Delay(500, Cts.Token);           //задержка отрисовки строк
                        }

                        await Task.Delay(InternalPeriodTimer);          //задержка отпроса след. устройства.
                    }
                }
            }

            _sendLock = false;
        }
    }
}