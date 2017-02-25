using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunicationDevices.Infrastructure;
using CommunicationDevices.Infrastructure.VidorDataProvider;

namespace CommunicationDevices.Behavior.TcpIpBehavior
{
    public class VidorTableExchangeTcpIpBehavior : BaseExhangeTcpIpBehavior
    {
        #region fields

        private readonly byte _countRow;                                 //кол-во строк на табло
        public List<string> InternalAddressCollection { get; set; }      //адресс уст-ва нужный для протокола обмена.

        #endregion





        #region ctor

        public VidorTableExchangeTcpIpBehavior(string connectionString, List<string> internalAddress, byte maxCountFaildRespowne, int timeRespown, byte countRow)
            : base(connectionString, maxCountFaildRespowne, timeRespown, 12000)
        {
            _countRow = countRow;
            InternalAddressCollection = internalAddress;
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
                //Вывод на табличное табло построчной информации
                if (inData?.TableData != null)
                {
                    var filteredData = inData.TableData;
                    //фильтрация по ближайшему времени к текущему времени.
                    var timeSampling = inData.TableData.Count > _countRow ? UniversalInputType.GetFilteringByDateTimeTable(_countRow, filteredData) : filteredData;

                    //Отправляем информацию для всех устройств, подключенных к данному TCP конвертору.
                    foreach (var internalAddr in InternalAddressCollection)
                    {
                        timeSampling.ForEach(t => t.AddressDevice = internalAddr);
                        for (byte i = 0; i < _countRow; i++)
                        {
                            var writeTableProvider = (i < timeSampling.Count) ?
                                new PanelVidorTableWriteDataProvider { InputData = timeSampling[i], CurrentRow = (byte)(i + 1) } :                                          // Отрисовка строк
                                new PanelVidorTableWriteDataProvider { InputData = new UniversalInputType { AddressDevice = internalAddr }, CurrentRow = (byte)(i + 1) };   // Обнуление строк

                            DataExchangeSuccess = await MasterTcpIp.RequestAndRespoune(writeTableProvider);
                            LastSendData = writeTableProvider.InputData;

                            await Task.Delay(500);
                        }

                        //Запрос синхронизации времени
                        var syncTimeProvider = new PanelVidorTableWriteDataProvider { InputData = new UniversalInputType { AddressDevice = internalAddr }, CurrentRow = 0xFF };
                        DataExchangeSuccess = await MasterTcpIp.RequestAndRespoune(syncTimeProvider);

                        await Task.Delay(500);
                    }
                }
            }

            _sendLock = false;
        }
    }
}