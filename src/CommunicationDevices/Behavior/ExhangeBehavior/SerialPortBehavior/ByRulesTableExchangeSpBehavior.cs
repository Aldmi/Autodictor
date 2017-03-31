using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using CommunicationDevices.Behavior.ExchangeRules;
using CommunicationDevices.DataProviders;
using CommunicationDevices.DataProviders.BuRuleDataProvider;


namespace CommunicationDevices.Behavior.ExhangeBehavior.SerialPortBehavior
{
    public class ByRulesTableExchangeSpBehavior : BaseExhangeSpBehavior
    {
        #region prop

        public MainRule MainRule { get; }

        #endregion





        #region ctor

        public ByRulesTableExchangeSpBehavior(MasterSerialPort port, ushort timeRespone, byte maxCountFaildRespowne, MainRule mainRule)
            : base(port, timeRespone, maxCountFaildRespowne)
        {
            MainRule = mainRule;

            //добавляем циклические функции
            Data4CycleFunc = new ReadOnlyCollection<UniversalInputType>(new List<UniversalInputType> { new UniversalInputType { TableData = new List<UniversalInputType>() } });  //данные для 1-ой циклической функции
            ListCycleFuncs = new List<Func<MasterSerialPort, CancellationToken, Task>> { CycleExcangeService };                      // 1 циклическая функция
        }

        #endregion





        #region Methode

        private async Task CycleExcangeService(MasterSerialPort port, CancellationToken ct)
        {
            if (!MainRule.ViewType.TableSize.HasValue)
                return;

            var countRow = MainRule.ViewType.TableSize.Value;
            var inData = Data4CycleFunc[0];

            //Вывод на табличное табло построчной информации
            if (inData?.TableData != null)
            {
                //фильтрация по ближайшему времени к текущему времени.
                var filteredData = inData.TableData;
                var timeSampling = inData.TableData.Count > countRow ? UniversalInputType.GetFilteringByDateTimeTable(countRow, filteredData) : filteredData;

                timeSampling.ForEach(t => t.AddressDevice = inData.AddressDevice);
                for (byte i = 0; i < countRow; i++)
                {           
                    var currentRow = (byte)(i + 1);
                    var inputData = (i < timeSampling.Count) ? timeSampling[i] : new UniversalInputType { AddressDevice = inData.AddressDevice };

                    //Определим какие из правил отрисовывают данную строку (вывод информации или пустой строки).
                    foreach (var exchangeRule in MainRule.ExchangeRules)
                    {
                        if (!exchangeRule.IsEnableTableRule(currentRow, timeSampling.Count))   
                            continue;

                        var forTableViewDataProvide = new ByRuleTableWriteDataProvider(exchangeRule)
                        {
                            InputData = inputData,
                            CurrentRow = currentRow
                        };

                        DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, forTableViewDataProvide, ct);
                        LastSendData = forTableViewDataProvide.InputData;
                        await Task.Delay(exchangeRule.ResponseRule.Time, ct);  //задержка для задания периода опроса.    
                    }
                }
            }

            await Task.Delay(1000, ct);  //задержка для задания периода опроса. 
        }

        #endregion





        #region OverrideMembers

        protected override List<Func<MasterSerialPort, CancellationToken, Task>> ListCycleFuncs { get; set; }
        protected override async Task OneTimeExchangeService(MasterSerialPort port, CancellationToken ct)
        {
            //LastSendData = (InDataQueue != null && InDataQueue.Any()) ? InDataQueue.Dequeue() : null;
            //var writeProvider = new PanelVidorWriteDataProvider { InputData = LastSendData };
            //DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeProvider, ct);
            //await Task.Delay(1000, ct);  //задержка для задания периода опроса.  
        }

        #endregion
    }
}