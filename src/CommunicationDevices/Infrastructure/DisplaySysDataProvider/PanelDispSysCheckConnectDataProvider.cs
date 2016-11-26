using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Communication.Annotations;
using Communication.Interfaces;

namespace CommunicationDevices.Infrastructure.DisplaySysDataProvider
{
    public class PanelDispSysCheckConnectDataProvider : IExchangeDataProvider<UniversalInputType, bool>
    {
        #region Prop

        public int CountGetDataByte { get; private set; } //вычисляется при отправке
        public int CountSetDataByte { get; } = 5;

        public UniversalInputType InputData { get; set; }
        public bool OutputData { get; }

        public bool IsOutDataValid { get; private set; }

        #endregion




        /// <summary>
        /// Данные запроса по записи строки на табло. Пакет формируется в строковом типе, затем переводится в массив байт в кодировке win-1251.
        /// байт[0]= STX   (0x02)
        /// байт[1]=       адресс
        /// байт[2]=       адресс
        /// байт[3]=       N байт пакета (без CRC и ETX)
        /// байт[4]=       N байт пакета (без CRC и ETX)
        /// байт[..]=       формат1  (% ... )
        /// байт[..]=       строка1  (% ... )
        /// байт[..]=       формат2  (% ... )
        /// байт[..]=       строка2  (% ... )
        /// байт[..]=       формат3  (% ... )
        /// байт[..]=       строка3  (% ... )
        /// байт[..]=       CRC
        /// байт[..]=       CRC
        /// байт[..]=       ETX   (0x03)
        /// </summary>
        public byte[] GetDataByte()
        {
            //"%0100202A00200C00040019%102402Лианозово, Марк, Долгопрудный, Лобня";

            byte address = byte.Parse(InputData.Address);
            byte numberOfTrain = byte.Parse(InputData.NumberOfTrain);
            byte numberOfPath = byte.Parse(InputData.PathNumber);

            string addressStr = address.ToString("X2");
            string numberOfTrainStr = "Поезд №" + numberOfTrain.ToString("X2");
            string numberOfPathStr = "Путь №" + numberOfPath.ToString("X2");


            // %01 - задание формата вывода строки 1
            // 002 - Х1
            // 02А - X2
            // 002 - Y1
            // 00С - Y2
            // 0004 - условно стат поле
            // 0019 - скорость 25 кад/с
            string format1 = "%0100202A00200C00040019";
            string message1 = $"%10{numberOfTrainStr.Length}02{numberOfTrainStr}";
            string result1 = format1 + message1;

            // %01 - задание формата вывода строки 2
            // 002 - Х1
            // 02А - X2
            // 00E - Y1
            // 013 - Y2
            // 0004 - условно стат поле
            // 0019 - скорость 25 кад/с
            string format2 = "%0100202A00E01300040019";
            string message2 = $"%10{numberOfPathStr.Length}02{numberOfPathStr}";
            string result2 = format2 + message2;

            // %42 - задание формата вывода строки 3 (горизонт. лин.)
            // 002 - Х1
            // 02А - X2
            // 014 - Y1
            // 014 - Y2
            // 00 - доп парам.
            string format_horizLine = "%4200202A01401400";



            //формируем КОНЕЧНУЮ строку
            var sumResult = result1 + result2 + format_horizLine;
            var resultstring = addressStr + sumResult.Length.ToString("X2") + sumResult;

            //вычисляем CRC
            byte[] xorBytes = Encoding.GetEncoding("Windows-1251").GetBytes(resultstring);
            byte xor = CalcXor(xorBytes);
            resultstring += xor.ToString("X2");


            //Преобразовываем КОНЕЧНУЮ строку в массив байт
            var resultBuffer = Encoding.GetEncoding("Windows-1251").GetBytes(resultstring).ToList();
            resultBuffer.Insert(0, 0x02);   //STX
            resultBuffer.Add(0x03);         //ETX

            return resultBuffer.ToArray();
        }



        /// <summary>
        /// Данные ответа по записи строки на табло.
        /// байт[0]= 
        /// байт[1]= 
        /// байт[2]= 
        /// байт[3]=
        /// байт[4]= 
        /// </summary>
        public bool SetDataByte(byte[] data)
        {
            if (data == null || data.Length != CountSetDataByte)
            {
                IsOutDataValid = false;
                return false;
            }


            IsOutDataValid = true;
            return true;


            //if (data[0] == byte.Parse(InputData.Address) &&
            //    data[1] == CountSetDataByte)
            //{
            //    if (data[2] == 0x83)                         //успешно приняты
            //    {
            //        IsOutDataValid = true;
            //        return true;
            //    }

            //    if (data[2] == 0x80)                          //ошибка приема
            //    {
            //        OutputData.ErrorCode = data[3];
            //    }
            //}

            //IsOutDataValid = false;
            //return false;
        }



        private byte CalcXor(IReadOnlyList<byte> arr)
        {
            var xor = arr[0];
            for (var i = 1; i < arr.Count; i++)
            {
                xor ^= arr[i];
            }
            xor ^= 0xFF;

            return xor;
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