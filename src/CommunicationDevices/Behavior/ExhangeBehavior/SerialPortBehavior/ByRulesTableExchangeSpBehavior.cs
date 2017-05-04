using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Castle.Core.Internal;
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
                //DEBUG---------------------------------------
                CreateXmlRequest(inData?.TableData);
                //DEBUG---------------------------------------


                for (byte i = 0; i < countRow; i++)
                {
                    //фильтрация по ближайшему времени к текущему времени.
                    var filteredData = inData.TableData;
                    var timeSampling = inData.TableData.Count > countRow ? UniversalInputType.GetFilteringByDateTimeTable(countRow, filteredData) : filteredData;
                    var orderSampling = timeSampling.OrderBy(date => date.Time).ToList();//TODO:фильтровать при заполнении TableData.

                    orderSampling.ForEach(t => t.AddressDevice = inData.AddressDevice);

                    var currentRow = (byte)(i + 1);
                    var inputData = (i < orderSampling.Count) ? orderSampling[i] : new UniversalInputType { AddressDevice = inData.AddressDevice };


                    //------------------
                    //Выбрать правила для отрисовки
                    var selectedRules = MainRule.ExchangeRules.Where(rule => rule.CheckResolution(inputData)).ToList();
                    //Если выбранно хотя бы 1 правило с условием, то оставляем толкьо эти правила.
                    //Если все правила безусловные то отрисовываем последовательно, каждым правилом.
                    if (selectedRules.Any(d => d.Resolution != null))
                    {
                        selectedRules = selectedRules.Where(rule => rule.Resolution != null).ToList();
                    }

                    //Определим какие из правил отрисовывают данную строку (вывод информации или пустой строки).
                    foreach (var exchangeRule in selectedRules)
                    {
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
                    //фильтрация по ближайшему времени к текущему времени.
                    var filteredData = inData.TableData;
                    var timeSampling = inData.TableData.Count > countRow ? UniversalInputType.GetFilteringByDateTimeTable(countRow, filteredData) : filteredData;
                    var orderSampling = timeSampling.OrderBy(date => date.Time).ToList();//TODO:фильтровать при заполнении TableData.

                    orderSampling.ForEach(t => t.AddressDevice = inData.AddressDevice);

                    var currentRow = (byte)(i + 1);
                    var inputData = (i < orderSampling.Count) ? orderSampling[i] : new UniversalInputType { AddressDevice = inData.AddressDevice };

                    //Выбрать правила для отрисовки
                    var selectedRules = MainRule.ExchangeRules.Where(rule => rule.CheckResolution(inputData)).ToList();
                    //Если выбранно хотя бы 1 правило с условием, то оставляем толкьо эти правила.
                    //Если все правила безусловные то отрисовываем последовательно, каждым правилом.
                    if (selectedRules.Any(d => d.Resolution != null))
                    {
                        selectedRules = selectedRules.Where(rule => rule.Resolution != null).ToList();
                    }

                    //Определим какие из правил отрисовывают данную строку (вывод информации или пустой строки).
                    foreach (var exchangeRule in selectedRules)
                    {
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



        private void CreateXmlRequest(IEnumerable<UniversalInputType> tables)
        {
            if (tables == null || !tables.Any())
                return;

            var xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), new XElement("tlist"));
            //-----------------------------DEBUG
            foreach (var uit in tables)
            {
                string trainType = String.Empty;
                string typeName = String.Empty;
                string typeNameShort = String.Empty;
                switch (uit.TypeTrain)
                {
                    case TypeTrain.None:
                        trainType = String.Empty;
                        typeName = String.Empty;
                        break;

                    case TypeTrain.Suburban:
                        trainType = "0";
                        typeName = "Пригородный";
                        typeNameShort = "приг";
                        break;

                    case TypeTrain.Express:
                        trainType = "1";
                        typeName = "Экспресс";
                        typeNameShort = "экспресс";
                        break;

                    case TypeTrain.HighSpeed:
                        trainType = "2";
                        typeName = "Скорый";
                        typeNameShort = "скор";
                        break;

                    case TypeTrain.Corporate:
                        trainType = "3";
                        typeName = "Фирменный";
                        typeNameShort = "фирм";
                        break;

                    case TypeTrain.Passenger:
                        trainType = "4";
                        typeName = "Пассажирский";
                        typeNameShort = "пасс";
                        break;

                    case TypeTrain.Swallow:
                        trainType = "5";
                        typeName = "Скоростной";
                        typeNameShort = "скоростной";
                        break;

                    case TypeTrain.Rex:
                        trainType = "5";
                        typeName = "Скоростной";
                        typeNameShort = "скоростной";
                        break;
                }

                string startSt;
                string endSt;
                var stations = uit.Stations.Split('-').Select(s => s.Trim()).ToList();
                if (stations.Count == 2)
                {
                   startSt = stations[0];
                   endSt = stations[1];
                }
                else
                {
                    startSt = (uit.Event == "ОТПР.") ? stations[0] : " ";
                    endSt = (uit.Event == "ПРИБ.") ? stations[0] : " ";
                }

                var time = uit.Time.ToString("s");


                xDoc.Root?.Add(
                    new XElement("t",
                    new XElement("TrainNumber", uit.NumberOfTrain),
                    new XElement("TrainType", trainType),
                    new XElement("StartStation", startSt),
                    new XElement("EndStation", endSt),
                    new XElement("RecDateTime", time),
                    new XElement("SndDateTime", time),
                    new XElement("EvRecTime", time),
                    new XElement("EvSndTime", time),
                    new XElement("TrackNumber", uit.PathNumber),
                    new XElement("Direction", (uit.Event == "ПРИБ.") ? 0 : 1),
                    new XElement("EvTrackNumber", uit.PathNumber),
                    new XElement("State", 0),
                    new XElement("VagonDirection", (byte)uit.VagonDirection),
                    new XElement("Enabled", (uit.EmergencySituation & 0x01) == 0x01 ? 0 : 1),

                    new XElement("tt",
                    new XElement("TypeName", typeName),
                    new XElement("TypeAlias", typeNameShort))
                    ));
            }


            string path = Application.StartupPath + @"/StaticTableDisplay" + @"/xDoc.info";
            xDoc.Save(path);
        }


    }
}