using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using CommunicationDevices.Infrastructure;
using CommunicationDevices.Infrastructure.VidorDataProvider;
using MoreLinq;


namespace CommunicationDevices.Behavior.SerialPortBehavior
{

    /// <summary>
    /// ПОВЕДЕНИЕ ОБМЕНА ДАННЫМИ МНОГОСТРОЧНОГО ТАБЛО "ДИСПЛЕЙНЫХ СИСТЕМ" ПО ПОСЛЕД. ПОРТУ
    /// </summary>
    public class VidorTableMinExchangeBehavior : BaseExhangeSpBehavior
    {
        #region fields

        private readonly byte _countRow; //кол-во строк на табло

        #endregion





        #region ctor

        public VidorTableMinExchangeBehavior(MasterSerialPort port, ushort timeRespone, byte maxCountFaildRespowne, byte countRow)
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
                //TableData содержит всю информацию про поезда выставленные на привязанные к устройству пути.
                // т.е. больше _countRow записей. Нужно выводить _countRow ближайших по времени к текущему времени записей.
                // если выбранных записей меньше _countRow, то выводить _countRow и  очищать остальные строки.

                //Исключаем прибывающие поезда
                //фильтрация по ближайшему времени к текущему времени.
                var filteredData = inData.TableData.Where(d => d.Event != "ПРИБ.").ToList();
                var timeSampling = inData.TableData.Count > _countRow ? UniversalInputType.GetFilteringByDateTimeTable(_countRow, filteredData) : filteredData;

                timeSampling.ForEach(t => t.AddressDevice = inData.AddressDevice);
                for (byte i = 0; i < _countRow; i++)
                {
                    var writeTableProvider = (i < timeSampling.Count) ?
                        new PanelVidorTableMinWriteDataProvider { InputData = timeSampling[i], CurrentRow = (byte)(i + 1) } :                                           // Отрисовка строк
                        new PanelVidorTableMinWriteDataProvider { InputData = new UniversalInputType { AddressDevice = inData.AddressDevice }, CurrentRow = (byte)(i + 1) };   // Обнуление строк

                    DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeTableProvider, ct);
                    LastSendData = writeTableProvider.InputData;

                    await Task.Delay(500, ct);
                }
            }

            await Task.Delay(10000, ct);  //задержка для задания периода опроса.    
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
                //TableData содержит всю информацию про поезда выставленные на привязанные к устройству пути.
                // т.е. больше _countRow записей. Нужно выводить _countRow ближайших по времени к текущему времени записей.
                // если выбранных записей меньше _countRow, то выводить _countRow и  очищать остальные строки.

                //Исключаем прибывающие поезда
                //фильтрация по ближайшему времени к текущему времени.
                var excludingArrival = inData.TableData.Where(d => d.Event != "ПРИБ.").ToList();
                var filtredCollection = inData.TableData.Count > _countRow ? UniversalInputType.GetFilteringByDateTimeTable(2, excludingArrival) : excludingArrival;

                filtredCollection.ForEach(t => t.AddressDevice = inData.AddressDevice);
                for (byte i = 0; i < _countRow; i++)
                {
                    var writeTableProvider = (i < filtredCollection.Count) ?
                        new PanelVidorTableMinWriteDataProvider { InputData = filtredCollection[i], CurrentRow = (byte)(i + 1) } :                                           // Отрисовка строк
                        new PanelVidorTableMinWriteDataProvider { InputData = new UniversalInputType { AddressDevice = inData.AddressDevice }, CurrentRow = (byte)(i + 1) };   // Обнуление строк

                    DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeTableProvider, ct);
                    LastSendData = writeTableProvider.InputData;

                    await Task.Delay(300, ct);
                }
            }

            await Task.Delay(10000, ct);  //задержка для задания периода опроса. 
        }

        #endregion
    }
}