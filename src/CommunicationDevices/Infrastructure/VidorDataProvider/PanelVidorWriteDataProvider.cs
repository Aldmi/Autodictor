using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Communication.Annotations;
using Communication.Interfaces;


namespace CommunicationDevices.Infrastructure.VidorDataProvider
{
    public class PanelVidorWriteDataProvider : IExchangeDataProvider<UniversalInputType, byte>
    {
        #region Prop

        public int CountGetDataByte { get; private set; } //вычисляется при отправке
        public int CountSetDataByte { get; } = 8;

        public UniversalInputType InputData { get; set; }
        public byte OutputData { get; }

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
            try
            {
                byte address = byte.Parse(InputData.AddressDevice);

                string numberOfTrain = string.IsNullOrEmpty(InputData.NumberOfTrain) ? " " : InputData.NumberOfTrain;
                string numberOfPath = string.IsNullOrEmpty(InputData.PathNumber) ? " " : InputData.PathNumber;
                string ev = string.IsNullOrEmpty(InputData.Event) ? " " : InputData.Event;
                string stations = string.IsNullOrEmpty(InputData.Stations) ? " " : InputData.Stations;
                string time = (InputData.Time == DateTime.MinValue) ? " " : InputData.Time.ToShortTimeString();




                // %30 - синхр часов
                // [3..8] - 5байт (hex) время в сек.   
                var timeNow = DateTime.Now.Hour.ToString() +  DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                string format1 = "%30";
                string message1 = $"{timeNow}";
                string result1 = format1 + message1;


                // %01 - задание формата вывода ПУТИ
                // 001 - Х1
                // 146 - X2
                // 014 - Y
                // аттриб = 4 (бег.стр.)
                string format2 = "%000011460144";
                string message2 = $"%10$18$00$60$t2ПУТЬ №{numberOfPath}";
                string result2 = format2 + message2;

                // %01 - задание формата вывода события
                // 152 - Х1
                // 192 - X2
                // 014 - Y
                // аттриб = 4 (бег.стр.)
                string format3 = "%001521920144";
                string message3 = $"%10$18$00$60$t3{ev}";
                string result3 = format3 + message3;

                // %01 - задание формата вывода слова ПОЕЗД№
                // 152 - Х1
                // 192 - X2
                // 014 - Y
                // аттриб = 4 (бег.стр.)
                string format4 = "%000010750314";
                string message4 = "%10$18$00$60$t3Поезд №";
                string result4 = format4 + message4;

                // %42 - вывод верт. линии
                // 001 - Х1
                // 192 - Y1
                // 016 - Y2
                // 016 - ширина
                string message5 = "%42001192016016";
                string result5 = message5;

                // %01 - задание формата номера поезда
                // 077 - Х1
                // 146 - X2
                // 031 - Y1
                // аттриб = 4 (бег.стр.)
                string format6 = "%000771460314";
                string message6 =$"%10$18$00$60$t3{numberOfTrain}";
                string result6 = format6 + message6;

                // %01 - задание формата вывода станции
                // 077 - Х1
                // 146 - X2
                // 046 - Y1
                // аттриб = 4 (бег.стр.)
                string format7 = "%000011460464";
                string message7 = $"%10$18$00$60$t3{stations}";
                string result7 = format7 + message7;

                // %01 - задание формата вывода времени
                // 152 - Х1
                // 192 - X2
                // 046 - Y1
                // аттриб = 4 (бег.стр.)
                string format8 = "%001521920464";
                string message8 = $"%10$18$00$60$t3{time}";
                string result8 = format8 + message8;



                //формируем КОНЕЧНУЮ строку
                var sumResult = result1 + result2 + result3 + result4 + result5 + result6 + result7 + result8;
                var resultstring = address.ToString("X2") + sumResult.Length.ToString("X2") + sumResult;

                //вычисляем CRC
                byte[] xorBytes = Encoding.GetEncoding("Windows-1251").GetBytes(resultstring);
                byte xor = CalcXor(xorBytes);
                resultstring += xor.ToString("X2");


                //Преобразовываем КОНЕЧНУЮ строку в массив байт
                var resultBuffer = Encoding.GetEncoding("Windows-1251").GetBytes(resultstring).ToList();
                resultBuffer.Insert(0, 0x02); //STX
                resultBuffer.Add(0x03); //ETX

                return resultBuffer.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
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