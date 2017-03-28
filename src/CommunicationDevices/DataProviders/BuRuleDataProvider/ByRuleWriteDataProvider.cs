using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
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

            var nbyteIndex = requestFillBody.IndexOf("Nbyte", StringComparison.Ordinal);
            var crcxorIndex = requestFillBody.IndexOf("CRCXor", StringComparison.Ordinal);

            if (requestFillBody.Contains("Nbyte"))
            {
                var startIndex = nbyteIndex;
                var lastIndex = (crcxorIndex > 0) ? crcxorIndex : requestFillBody.Length;

                var infoStr = requestFillBody.Substring(startIndex, lastIndex - startIndex);

            }

            var lenght = 154;  //вычислили длинну строгки между Nbyte и CRC


            var subStr = requestFillBody.Split('}');
            StringBuilder resStr = new StringBuilder();
            foreach (var s in subStr)
            {
                var replaseStr = s + "}";
                if (replaseStr.Contains("Nbyte"))
                {
                    var formatStr = string.Format(replaseStr.Replace("Nbyte", "0"), lenght);
                    resStr.Append(formatStr);
                }
                else
                {
                    resStr.Append(replaseStr);
                }
            }

            var resultStr = resStr.ToString();

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