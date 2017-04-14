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
            Data4CycleFunc = new ReadOnlyCollection<UniversalInputType>(new List<UniversalInputType> { new UniversalInputType { Event = "  ", NumberOfTrain = "  ", PathNumber = "  ", Stations = "  ", Time = DateTime.MinValue, TableData = new List<UniversalInputType>() } });  //данные для 1-ой циклической функции
            ListCycleFuncs = new List<Func<MasterSerialPort, CancellationToken, Task>> { CycleExcangeService };                      // 1 циклическая функция
        }

        #endregion





        #region Methode

        private async Task CycleExcangeService(MasterSerialPort port, CancellationToken ct)
        {
            var inData = Data4CycleFunc[0];
            foreach (var exchangeRule in ExchangeRules)
            {
                if (inData?.TableData != null)
                {
                    //фильтрация по ближайшему времени к текущему времени.
                    var filteredData = inData.TableData;
                    var timeSamplingMessage = UniversalInputType.GetFilteringByDateTimeTable(1, filteredData)?.FirstOrDefault();

                    //вывод пустой строки если в таблице нет данных
                    var emptyMessage = new UniversalInputType
                    {
                        Event = "  ",
                        NumberOfTrain = "  ",
                        PathNumber = "  ",
                        Stations = "  ",
                        Time = DateTime.MinValue,
                        Message = $"ПОЕЗД:{inData.NumberOfTrain}, ПУТЬ:{inData.PathNumber}, СОБЫТИЕ:{inData.Event}, СТАНЦИИ:{inData.Stations}, ВРЕМЯ:{inData.Time.ToShortTimeString()}"
                    };

                    var viewData = timeSamplingMessage ?? emptyMessage;
                    viewData.AddressDevice = inData.AddressDevice;

                    //Вывод на путевое табло
                    var writeProvider = new ByRuleWriteDataProvider(exchangeRule) { InputData = viewData };
                    DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeProvider, ct);

                    LastSendData = writeProvider.InputData;

                    if (writeProvider.IsOutDataValid)
                    {
                        // Log.log.Trace(""); //TODO: возможно передавать в InputData ID устройства и имя.
                    }

                    await Task.Delay(1000, ct);  //задержка для задания периода опроса. 
                }
            }
        }

        #endregion





        #region OverrideMembers

        protected override List<Func<MasterSerialPort, CancellationToken, Task>> ListCycleFuncs { get; set; }
        protected override async Task OneTimeExchangeService(MasterSerialPort port, CancellationToken ct)
        {
            var inData = (InDataQueue != null && InDataQueue.Any()) ? InDataQueue.Dequeue() : null;
            foreach (var exchangeRule in ExchangeRules)
            {
                if (inData?.TableData != null)
                {
                    //фильтрация по ближайшему времени к текущему времени.
                    var filteredData = inData.TableData;
                    var timeSamplingMessage = UniversalInputType.GetFilteringByDateTimeTable(1, filteredData)?.FirstOrDefault();

                    //вывод пустой строки если в таблице нет данных
                    var emptyMessage = new UniversalInputType
                    {
                        Event = "  ",
                        NumberOfTrain = "  ",
                        PathNumber = "  ",
                        Stations = "  ",
                        Time = DateTime.MinValue,
                        Message = $"ПОЕЗД:{inData.NumberOfTrain}, ПУТЬ:{inData.PathNumber}, СОБЫТИЕ:{inData.Event}, СТАНЦИИ:{inData.Stations}, ВРЕМЯ:{inData.Time.ToShortTimeString()}"
                    };

                    var viewData = timeSamplingMessage ?? emptyMessage;
                    viewData.AddressDevice = inData.AddressDevice;

                    //Вывод на путевое табло
                    var writeProvider = new ByRuleWriteDataProvider(exchangeRule) { InputData = viewData };
                    DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeProvider, ct);

                    LastSendData = writeProvider.InputData;

                    if (writeProvider.IsOutDataValid)
                    {
                        // Log.log.Trace(""); //TODO: возможно передавать в InputData ID устройства и имя.
                    }

                    await Task.Delay(1000, ct);  //задержка для задания периода опроса. 
                }
            }
        }

        #endregion
    }
}