using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using Communication.TcpIp;
using CommunicationDevices.DataProviders;
using CommunicationDevices.DataProviders.VidorDataProvider;
using CommunicationDevices.Infrastructure;

namespace CommunicationDevices.Behavior.ExhangeBehavior.SerialPortBehavior
{

    /// <summary>
    /// ПОВЕДЕНИЕ ОБМЕНА ДАННЫМИ МНОГОСТРОЧНОГО ТАБЛО "ДИСПЛЕЙНЫХ СИСТЕМ" ПО ПОСЛЕД. ПОРТУ
    /// </summary>
    public class VidorTableLineByLineExchangeSpBehavior : BaseExhangeSpBehavior
    {
        #region fields

        private readonly byte _countRow; //кол-во строк на табло

        #endregion




        #region prop

        public ILineByLineDrawingTableDataProvider ForTableViewDataProvider { get; set; }

        #endregion




        #region ctor

        public VidorTableLineByLineExchangeSpBehavior(MasterSerialPort port, ushort timeRespone, byte maxCountFaildRespowne, byte countRow)
            : base(port, timeRespone, maxCountFaildRespowne)
        {
            _countRow = countRow;
            //добавляем циклические функции
            Data4CycleFunc = new ReadOnlyCollection<UniversalInputType>(new List<UniversalInputType> { new UniversalInputType { TableData = new List<UniversalInputType>() } });  //данные для 1-ой циклической функции
            ListCycleFuncs = new List<Func<MasterSerialPort, CancellationToken, Task>> { CycleExcangeService };                      // 1 циклическая функция
        }

        #endregion




        #region Methode

        private async Task CycleExcangeService(MasterSerialPort port, CancellationToken ct)
        {
            var inData = Data4CycleFunc[0];

            //Вывод на табличное табло построчной информации
            if (inData?.TableData != null)
            {
                //фильтрация по ближайшему времени к текущему времени.
                var filteredData = inData.TableData;
                var timeSampling = inData.TableData.Count > _countRow ? UniversalInputType.GetFilteringByDateTimeTable(_countRow, filteredData) : filteredData;

                timeSampling.ForEach(t => t.AddressDevice = inData.AddressDevice);
                for (byte i = 0; i < _countRow; i++)
                {
                    ForTableViewDataProvider.CurrentRow = (byte)(i + 1);                                                                                                        // Отрисовка строк
                    ForTableViewDataProvider.InputData = (i < timeSampling.Count) ? timeSampling[i] : new UniversalInputType { AddressDevice = inData.AddressDevice };          // Обнуление строк

                    DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, ForTableViewDataProvider, ct);
                    LastSendData = ForTableViewDataProvider.InputData;

                    await Task.Delay(500, ct);
                }


                //Запрос синхронизации времени
                //var syncTimeProvider = new PanelVidorTableWriteDataProvider { InputData = new UniversalInputType { AddressDevice = inData.AddressDevice }, CurrentRow = 0xFF };
                //DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, syncTimeProvider, ct);

                ForTableViewDataProvider.CurrentRow = 0xFF;
                ForTableViewDataProvider.InputData = new UniversalInputType { AddressDevice = inData.AddressDevice };
                DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, ForTableViewDataProvider, ct);
            }



            await Task.Delay(1000, ct);  //задержка для задания периода опроса.    
        }

        #endregion





        #region OverrideMembers

        protected override sealed List<Func<MasterSerialPort, CancellationToken, Task>> ListCycleFuncs { get; set; }

        protected override async Task OneTimeExchangeService(MasterSerialPort port, CancellationToken ct)
        {
            var inData = (InDataQueue != null && InDataQueue.Any()) ? InDataQueue.Dequeue() : null;

            //Вывод на табличное табло построчной информации
            if (inData?.TableData != null)
            {        
                var filteredData = inData.TableData;
                 //фильтрация по ближайшему времени к текущему времени.
                var timeSampling = inData.TableData.Count > _countRow ? UniversalInputType.GetFilteringByDateTimeTable(_countRow, filteredData) : filteredData;

                timeSampling.ForEach(t => t.AddressDevice = inData.AddressDevice);
                //for (byte i = 0; i < _countRow; i++)
                //{
                //    var writeTableProvider = (i < timeSampling.Count) ?
                //        new PanelVidorTableWriteDataProvider { InputData = timeSampling[i], CurrentRow = (byte)(i + 1) } :                                                  // Отрисовка строк
                //        new PanelVidorTableWriteDataProvider { InputData = new UniversalInputType { AddressDevice = inData.AddressDevice }, CurrentRow = (byte)(i + 1) };   // Обнуление строк

                //    DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeTableProvider, ct);
                //    LastSendData = writeTableProvider.InputData;

                //    await Task.Delay(1000, ct);
                //}

                for (byte i = 0; i < _countRow; i++)
                {
                    ForTableViewDataProvider.CurrentRow = (byte)(i + 1);                                                                                                        // Отрисовка строк
                    ForTableViewDataProvider.InputData = (i < timeSampling.Count) ? timeSampling[i] : new UniversalInputType { AddressDevice = inData.AddressDevice };          // Обнуление строк

                    DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, ForTableViewDataProvider, ct);
                    LastSendData = ForTableViewDataProvider.InputData;

                    await Task.Delay(500, ct);
                }


                //Запрос синхронизации времени
                //var syncTimeProvider = new PanelVidorTableWriteDataProvider { InputData = new UniversalInputType { AddressDevice = inData.AddressDevice }, CurrentRow = 0xFF };
                //DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, syncTimeProvider, ct);

                ForTableViewDataProvider.CurrentRow = 0xFF;
                ForTableViewDataProvider.InputData = new UniversalInputType { AddressDevice = inData.AddressDevice };
                DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, ForTableViewDataProvider, ct);
            }

            await Task.Delay(500, ct);  //задержка для задания периода опроса. 
        }

        #endregion
    }
}