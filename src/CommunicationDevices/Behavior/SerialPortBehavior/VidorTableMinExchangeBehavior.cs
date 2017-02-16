﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Communication.SerialPort;
using CommunicationDevices.Infrastructure;
using CommunicationDevices.Infrastructure.VidorDataProvider;


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
            Data4CycleFunc= new ReadOnlyCollection<UniversalInputType>(new List<UniversalInputType> {new UniversalInputType {TableData = new List<UniversalInputType>()} }) ;  //данные для 1-ой циклической функции
            ListCycleFuncs = new List<Func<MasterSerialPort, CancellationToken, Task>> {CycleExcangeService};                      // 1 циклическая функция
        }

        #endregion




        #region Methode

        private async Task CycleExcangeService(MasterSerialPort port, CancellationToken ct)
        {
          var inData = Data4CycleFunc[0];
            //Вывод на табличное табло построчной информации
            if (inData?.TableData != null)
            {
                //Ограничим кол-во строк в таблице.
                if (inData.TableData.Count > _countRow)                                 
                {
                    inData.TableData = inData.TableData.Take(_countRow).ToList();
                }

                inData.TableData.ForEach(t=> t.AddressDevice= inData.AddressDevice);
                for (byte i = 0; i < _countRow; i++)
                {
                    //TODO: добавить новый DataProvider (PanelVidorMinTableWriteDataProvider)
                    var writeTableProvider = (i < inData.TableData.Count) ?
                        new PanelVidorTableWriteDataProvider { InputData = inData.TableData[i], CurrentRow = (byte) (i+1) } :                                           // Отрисовка строк
                        new PanelVidorTableWriteDataProvider { InputData = new UniversalInputType {AddressDevice = inData.AddressDevice}, CurrentRow = (byte)(i+1) };   // Обнуление строк

                    DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeTableProvider, ct);
                    LastSendData = writeTableProvider.InputData;
                }

                ////Запрос синхронизации времени
                //var syncTimeProvider = new PanelVidorTableWriteDataProvider { InputData = new UniversalInputType { AddressDevice = inData.AddressDevice }, CurrentRow = 0xFF };
                //DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, syncTimeProvider, ct);
            }

            await Task.Delay(500, ct);  //задержка для задания периода опроса.    
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
                inData.TableData.ForEach(t => t.AddressDevice = inData.AddressDevice);
                for (byte i = 0; i < _countRow; i++)
                {
                    var writeTableProvider = (i < inData.TableData.Count) ?
                       new PanelVidorTableWriteDataProvider { InputData = inData.TableData[i], CurrentRow = (byte)(i + 1) } :                                              // Отрисовка строк
                       new PanelVidorTableWriteDataProvider { InputData = new UniversalInputType { AddressDevice = inData.AddressDevice }, CurrentRow = (byte)(i + 1) };   // Обнуление строк

                    DataExchangeSuccess = await Port.DataExchangeAsync(TimeRespone, writeTableProvider, ct);
                    LastSendData = writeTableProvider.InputData;
                }
            }

            await Task.Delay(500, ct);  //задержка для задания периода опроса. 
        }

        #endregion
    }
}