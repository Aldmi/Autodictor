using System;
using System.Collections;
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
            if (InputData.ViewBag == null || !InputData.ViewBag.ContainsKey("SoundChanelManagmentEventPlaying"))
                return null;

            var ev = InputData.ViewBag["SoundChanelManagmentEventPlaying"] as string;

            //Вычисляем команду.
            byte[] command; 
            switch (ev)
            {
                case "InitSoundChanelDevice_step1":
                    command = new byte[1];
                    command[0] = 0x56;
                    break;


                case "InitSoundChanelDevice_step2":
                    command = new byte[1];
                    command[0] = 0x43;
                    break;


                case "StartPlaying":
                    //var chanelFlags = InputData.SoundChanels.ToArray();
                    //List<bool> configChFlags= new List<bool>();
                    //for (int i = 0, j = 0; i < 24; i++)
                    //{
                    //    if (i == 7 || i == 15 || i == 23)              //0x80,0x80,0x80 - старший бит выставленн в 3-ех битах
                    //    {
                    //        configChFlags.Add(true);
                    //        continue;
                    //    }

                    //    if(j < chanelFlags.Length)
                    //      configChFlags.Add(chanelFlags[j++]);
                    //}

                    //BitArray bitArray = new BitArray(configChFlags.ToArray());

                    command = new byte[4];
                    command[0] = 0x57;
                    command[1] = 0xFF;
                    command[2] = 0x80;
                    command[3] = 0x80;
                    //bitArray.CopyTo(command, 1);
                    break;



                case "StopPlaying":                   //<STX><ESC>#43<ETX>#58<US>
                    command = new byte[1];   
                    command[0] = 0x43;            
                    break;

                default:
                    return null;
            }


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
            if (data == null || !data.Any())
                return false;


            if (data[0] == 0x06) //ASC
                return true;

            if (data.Length > 2)
            {
                if (data[0] == 0x53 && data[1] == 0x31) //  Ответ на запрос инициализации
                    return true;
            }


            return false;
        }



        private byte CalcXor(IReadOnlyList<byte> arr)
        {
            var xor = arr[0];
            for (var i = 1; i < arr.Count; i++)
            {
                xor ^= arr[i];
            }
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