using System.ComponentModel;
using System.Runtime.CompilerServices;
using Communication.Annotations;
using Communication.Interfaces;
using CommunicationDevices.Behavior.ExchangeRules;


namespace CommunicationDevices.DataProviders.BuRuleDataProvider
{
    public class ByRuleWriteDataProvider : IExchangeDataProvider<UniversalInputType, byte>
    {
        #region Prop

        public int CountGetDataByte { get; }
        public int CountSetDataByte { get; }

        public UniversalInputType InputData { get; set; }
        public byte OutputData { get; }

        public bool IsOutDataValid { get; }

        public RequestRule RequestRule { get; set; }
        public ResponseRule ResponseRule { get; set; }

        #endregion






        public byte[] GetDataByte()
        {
            var requestFillBody = RequestRule.GetFillBody(InputData);


            return new byte[10];
        }




        public bool SetDataByte(byte[] data)
        {
            var responseFillBody = ResponseRule.GetFillBody(InputData);

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