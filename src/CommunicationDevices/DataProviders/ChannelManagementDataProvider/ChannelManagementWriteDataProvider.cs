using System.Collections.Generic;
using System.ComponentModel;
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





        public byte[] GetDataByte()
        {
            List<byte> bufer = new List<byte>();
            foreach (var inData in InputData.SoundChanels)
            {
                bufer.Add((byte)(inData ? 0x01 : 0x00));
            }

            return bufer.ToArray();
        }



        public bool SetDataByte(byte[] data)
        {
            return true;
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