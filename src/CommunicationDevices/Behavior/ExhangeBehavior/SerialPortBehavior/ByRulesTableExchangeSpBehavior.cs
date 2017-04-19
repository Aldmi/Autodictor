using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using CommunicationDevices.DataProviders;
using CommunicationDevices.DataProviders.BuRuleDataProvider;
using CommunicationDevices.Rules.ExchangeRules;


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
                for (byte i = 0; i < countRow; i++)
                {
                    //Определим какие из правил отрисовывают данную строку (вывод информации или пустой строки).
                    foreach (var exchangeRule in MainRule.ExchangeRules)
                    {
                        //фильтрация по ближайшему времени к текущему времени.
                        var filteredData = inData.TableData.Where(data => exchangeRule.CheckResolution(data));
                        var timeSampling = inData.TableData.Count > countRow ? UniversalInputType.GetFilteringByDateTimeTable(countRow, filteredData) : filteredData;
                        var orderSampling =  timeSampling.OrderBy(date => date.Time).ToList();//TODO:фильтровать при заполнении TableData.

                        orderSampling.ForEach(t => t.AddressDevice = inData.AddressDevice);

                        var currentRow = (byte)(i + 1);
                        var inputData = (i < orderSampling.Count) ? orderSampling[i] : new UniversalInputType { AddressDevice = inData.AddressDevice };


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
            if (!MainRule.ViewType.TableSize.HasValue)
                return;

            var countRow = MainRule.ViewType.TableSize.Value;
            var inData = (InDataQueue != null && InDataQueue.Any()) ? InDataQueue.Dequeue() : null;


            //Вывод на табличное табло построчной информации
            if (inData?.TableData != null)
            {
                for (byte i = 0; i < countRow; i++)
                {
                    //Определим какие из правил отрисовывают данную строку (вывод информации или пустой строки).
                    foreach (var exchangeRule in MainRule.ExchangeRules)
                    {
                        //фильтрация по ближайшему времени к текущему времени.
                        var filteredData = inData.TableData.Where(data => exchangeRule.CheckResolution(data)).ToList();
                        var timeSampling = inData.TableData.Count > countRow ? UniversalInputType.GetFilteringByDateTimeTable(countRow, filteredData) : filteredData;
                        var orderSampling = timeSampling.OrderBy(date => date.Time).ToList();//TODO:фильтровать при заполнении TableData.

                        orderSampling.ForEach(t => t.AddressDevice = inData.AddressDevice);

                        var currentRow = (byte)(i + 1);
                        var inputData = (i < orderSampling.Count) ? orderSampling[i] : new UniversalInputType { AddressDevice = inData.AddressDevice };

                        //if (!exchangeRule.IsEnableTableRule(currentRow, timeSampling.Count))   
                        //    continue;

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
    }
}