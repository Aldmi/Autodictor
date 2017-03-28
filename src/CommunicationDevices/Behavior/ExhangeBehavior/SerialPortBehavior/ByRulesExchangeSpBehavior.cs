using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using CommunicationDevices.Behavior.ExchangeRules;
using CommunicationDevices.DataProviders;
using CommunicationDevices.DataProviders.BuRuleDataProvider;


namespace CommunicationDevices.Behavior.ExhangeBehavior.SerialPortBehavior
{
    /// <summary>
    /// ПОВЕДЕНИЕ ОБМЕНА ДАННЫМИ ПО ПРАВИЛАМ ЗАДАНЫМ ИЗВНЕ ПО ПОСЛЕД. ПОРТУ
    /// </summary>
    public sealed class ByRulesExchangeSpBehavior : BaseExhangeSpBehavior
    {
        #region prop

        public IEnumerable<BaseExchangeRule> ExchangeRules { get; }

        #endregion





        #region ctor

        public ByRulesExchangeSpBehavior(MasterSerialPort port, ushort timeRespone, byte maxCountFaildRespowne, IEnumerable<BaseExchangeRule> exchangeRules)
            : base(port, timeRespone, maxCountFaildRespowne)
        {
            ExchangeRules = exchangeRules;
            //добавляем циклические функции
            Data4CycleFunc = new ReadOnlyCollection<UniversalInputType>(new List<UniversalInputType> { new UniversalInputType { Event = "  ", NumberOfTrain = "  ", PathNumber = "  ", Stations = "  ", Time = DateTime.MinValue } });  //данные для 1-ой циклической функции
            ListCycleFuncs = new List<Func<MasterSerialPort, CancellationToken, Task>> { CycleExcangeService };                      // 1 циклическая функция
        }

        #endregion





        #region Methode

        private async Task CycleExcangeService(MasterSerialPort port, CancellationToken ct)
        {
            foreach (var exchangeRule in ExchangeRules)
            {
               var writeProvider = new ByRuleWriteDataProvider { InputData = Data4CycleFunc[0], RequestRule = exchangeRule.RequestRule, ResponseRule = exchangeRule.ResponseRule };
               DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeProvider, ct);
               LastSendData = writeProvider.InputData;
               await Task.Delay(exchangeRule.ResponseRule.Time, ct);  //задержка для задания периода опроса.    

               if (writeProvider.IsOutDataValid)
               {
                    // Log.log.Trace("");
               }
            }
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