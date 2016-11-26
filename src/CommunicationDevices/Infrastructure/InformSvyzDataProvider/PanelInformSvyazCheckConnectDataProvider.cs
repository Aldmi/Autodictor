using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Communication.Interfaces;

namespace CommunicationDevices.Infrastructure
{
    public class PanelInformSvyazCheckConnectDataProvider : IExchangeDataProvider<UniversalInputType, byte>
    {
        #region prop

        public int CountGetDataByte { get; private set; }
        public int CountSetDataByte { get; }

        public UniversalInputType InputData { get; set; }
        public byte OutputData { get; }
        public bool IsOutDataValid { get; }

        #endregion





        /// <summary>
        /// Данные запроса по записи информации о билете (функц 0x10):
        /// байт[0]= адресс   0..0xFF
        /// байт[1]= длинна пакета.
        /// байт[2]= код команды (0х05 - данные).
        /// байт[3...X]= информационная часть ( X= макисмум 250байт). Строка в кодировке OEM866.
        /// байт[i]= КС. арифметическая сумма по MOD256 всех переданных данных.
        /// </summary>
        public byte[] GetDataByte()
        {
            CountGetDataByte = 4;
            var buf = new byte[CountGetDataByte];

            buf[0] = byte.Parse(InputData.Address);
            buf[1] = (byte)CountGetDataByte;
            buf[2] = 0x05;
            buf[3] = 0x00;
            var ks = (byte)((buf.Take(CountGetDataByte - 1).Sum(b => b)) / 256);
            buf[CountGetDataByte - 1] = ks;

            return buf;
        }





        public bool SetDataByte(byte[] data)
        {
            return true;
        }





        #region Event

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}