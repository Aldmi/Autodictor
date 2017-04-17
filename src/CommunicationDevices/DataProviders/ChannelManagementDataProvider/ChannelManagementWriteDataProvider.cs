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
           byte[] bytesArrray= new byte[] {0x0A, 0x0B, 0X0C};


            return bytesArrray;
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