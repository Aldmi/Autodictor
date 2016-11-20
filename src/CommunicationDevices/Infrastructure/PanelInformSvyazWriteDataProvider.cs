using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Communication.Annotations;
using Communication.Interfaces;


namespace CommunicationDevices.Infrastructure
{

    public class Mg6587Output
    {
        public byte Var22 { get; set; }
        public byte Var23 { get; set; }
    }



    public class PanelInformSvyazWriteDataProvider : IExchangeDataProvider<UniversalInputType, Mg6587Output>
    {
        #region field

        private const ushort StartAddresWrite = 0x0002;
        private const ushort NWriteRegister = 0x0001;

        #endregion




        #region Prop

        public int CountGetDataByte { get; private set; } = 4;
        public int CountSetDataByte { get; private set; } = 2;

        public UniversalInputType InputData { get; set; }
        public Mg6587Output OutputData { get; }

        public bool IsOutDataValid { get; }

        #endregion




        /// <summary>
        /// Данные запроса по записи информации о билете (функц 0x10):
        /// байт[0]= адресс   0..0xFF
        /// байт[1]= длинна пакета
        /// байт[2]= код команды (0х03 - данные, 0х05- запрос освещенности, 0х06- установка яркости свечения)
        /// байт[3...X]= информационная часть ( X= макисмум 250байт). Для команды 0х03 строка в кодировке OEM866 
        /// байт[i]= КС. арифметическая сумма по MOD256 всех переданных данных
        /// </summary>
        public byte[] GetDataByte()
        {
            var testStr = "qwerty";//DEBUG  OEM866: 113, 119, 101, 114, 116, 121

            var encoding = Encoding.GetEncoding(866);
            var messageBuf = encoding.GetBytes(testStr);      

            CountGetDataByte = 3 + messageBuf.Length + 1;
            var buf= new byte[CountGetDataByte];

            buf[0] = byte.Parse(InputData.Address);
            buf[1] = (byte) CountGetDataByte;
            buf[2] = 0x03;

            buf[3] = 113;
            buf[4] = 119;
            buf[5] = 101;
            buf[6] = 114;
            buf[7] = 116;
            buf[8] = 121;

            var ks = (byte)((buf.Take(CountGetDataByte - 1).Sum(b => b)) / 256);
            buf[CountGetDataByte - 1] = ks;
            return buf;   
        }


        /// <summary>
        /// Обработка ответа на данные записи информации о билете (функц 0x10):
        /// байт[0]= InputData.Сashbox
        /// байт[1]= 0x10
        /// байт[2]= Адр. Ст.
        /// байт[3]= Адр. Мл.
        /// байт[4]= Кол-во. рег. Ст.
        /// байт[5]= Кол-во. рег. Мл.
        /// байт[6]= CRC Мл.
        /// байт[7]= CRC Ст.
        /// </summary>
        public bool SetDataByte(byte[] data)
        {
            throw new NotImplementedException();
        }




        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
