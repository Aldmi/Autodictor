using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Communication.Annotations;
using Communication.Interfaces;

namespace CommunicationDevices.DataProviders.InformSvyzDataProvider
{

    public class InformSvyazOutput
    {
        private byte _errorCode;
        public byte ErrorCode
        {
            get { return _errorCode; }
            set
            {
                _errorCode = value;

                switch (ErrorCode)
                {
                    case 0x00:
                        ErrorMessage = string.Empty;
                        break;

                    case 0x01:
                        ErrorMessage = "Несуществующий код команды";
                        break;

                    case 0x02:
                        ErrorMessage = "Ошибка четности";
                        break;

                    case 0x04:
                        ErrorMessage = "Зависание линии в старте";
                        break;

                    case 0x08:
                        ErrorMessage = "Прерывание по таймауту";
                        break;

                    case 0x10:
                        ErrorMessage = "Неверные данные";
                        break;

                    case 0x20:
                        ErrorMessage = "Ошибка КС";
                        break;

                    case 0x40:
                        ErrorMessage = "Ошибка длинны пакета";
                        break;

                    default:
                        ErrorMessage = "Несколько ошибок сразу!!!";
                        break;
                }
            }
        }

        public string ErrorMessage { get; set; }
    }



    public class PanelInformSvyazWriteDataProvider : IExchangeDataProvider<UniversalInputType, InformSvyazOutput>
    {
        #region Prop

        public int CountGetDataByte { get; private set; }    //вычисляется при отправке
        public int CountSetDataByte { get; } = 5;

        public UniversalInputType InputData { get; set; }
        public InformSvyazOutput OutputData { get; }

        public bool IsOutDataValid { get; private set; }

        #endregion




        /// <summary>
        /// Данные запроса по записи строки на табло.
        /// байт[0]= адресс   0..0xFF
        /// байт[1]= длинна пакета.
        /// байт[2]= код команды (0х03 - данные).
        /// байт[3...X]= информационная часть ( X= макисмум 250байт). Строка в кодировке OEM866.
        /// байт[i]= КС. арифметическая сумма по MOD256 всех переданных данных.
        /// </summary>
        public byte[] GetDataByte()
        {
            var testStr = "qwerty";

            var encoding = Encoding.GetEncoding(866);
            var messageBuf = encoding.GetBytes(testStr);      

            CountGetDataByte = 3 + messageBuf.Length + 1;
            var buf= new byte[CountGetDataByte];

            buf[0] = byte.Parse(InputData.AddressDevice);
            buf[1] = (byte) CountGetDataByte;
            buf[2] = 0x03;

            messageBuf.CopyTo(buf, 3);

            var ks = (byte)((buf.Take(CountGetDataByte - 1).Sum(b => b)) / 256);
            buf[CountGetDataByte - 1] = ks;

            return buf;   
        }

        /// <summary>
        /// Данные ответа по записи строки на табло.
        /// байт[0]= адресс   (0х00 .. 0xFF).
        /// байт[1]= длинна пакета.
        /// байт[2]= код ответа (0x80 - ошибка приема (смотри код ошибки), 0х83- данные приняты, 0х85 - значение освещенности, 0х86 - яркость свечения)
        /// байт[3]= Байт ошибоки (если код ответа = 80)
        /// байт[4]= КС. арифметическая сумма по MOD256 всех переданных данных
        /// </summary>
        public bool SetDataByte(byte[] data)
        {
            if (data == null || data.Length != CountSetDataByte)
            {
                IsOutDataValid = false;
                return false;
            }

            if (data[0] == byte.Parse(InputData.AddressDevice) &&
                data[1] == CountSetDataByte)              
            {
                if (data[2] == 0x83)                         //успешно приняты
                {
                    IsOutDataValid = true;
                    return true;
                }
           
                if(data[2] == 0x80)                          //ошибка приема
                {
                    OutputData.ErrorCode = data[3];
                }
            }

            IsOutDataValid = false;
            return false;
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
