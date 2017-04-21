using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Communication.Annotations;
using Communication.Interfaces;

namespace CommunicationDevices.DataProviders.ChannelManagementDataProvider
{
    public class ChannelManagementWriteDataProvider : IExchangeDataProvider<UniversalInputType, byte>
    {
        #region Prop

        public int CountGetDataByte { get; private set; } //вычисляется при отправке
        public int CountSetDataByte { get; } = 8;

        public UniversalInputType InputData { get; set; }
        public byte OutputData { get; }

        public bool IsOutDataValid { get; private set; }

        public IEnumerable<bool> ChanelSwitches { get; }

        #endregion





        #region ctor

        public ChannelManagementWriteDataProvider(IEnumerable<bool> chanelSwitches)
        {
            ChanelSwitches = chanelSwitches;
        }

        #endregion




        /// <summary>
        /// формат посылки<STX><ESC> КОМАНДА <ETX> CRC <US>
        /// CRC - контрольный код, считаемый XORом всех данных, расположенных среди<STX>...<ETX>.
        /// </summary>
        public byte[] GetDataByte()
        {
            //Вычисляем команду.
            byte[] command = new byte[1];
            command[0] = 0x56;                      //DEBUG

            //Вычислим CRC
            var xorBytes = new List<byte> {0x1B};
            xorBytes.AddRange(command);
            byte xor = CalcXor(xorBytes);

            //Сформируем конечный буффер
            byte[] bufer = new byte[5 + command.Length];
            bufer[0] = 0x02;
            bufer[1] = 0x1B;
            Array.Copy(command, 0, bufer, 2, command.Length);
            bufer[2 + command.Length] = 0x03;
            bufer[3 + command.Length] = xor;
            bufer[4 + command.Length] = 0x1F;

            return bufer;
        }



        public bool SetDataByte(byte[] data)
        {
            return true;
        }



        private byte CalcXor(IReadOnlyList<byte> arr)
        {
            var xor = arr[0];
            for (var i = 1; i < arr.Count; i++)
            {
                xor ^= arr[i];
            }
            //xor ^= 0xFF;

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