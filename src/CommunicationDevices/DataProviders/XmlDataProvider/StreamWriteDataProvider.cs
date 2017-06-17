using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Linq;
using Communication.Annotations;
using Communication.Interfaces;


namespace CommunicationDevices.DataProviders.XmlDataProvider
{
    public class StreamWriteDataProvider : IExchangeDataProvider<UniversalInputType, byte>
    {
        #region Prop

        public int CountGetDataByte { get; }
        public int CountSetDataByte { get; }

        public UniversalInputType InputData { get; set; }
        public byte OutputData { get; }

        public bool IsOutDataValid { get; private set; }

        public IFormatProvider FormatProvider { get; set; }

        #endregion





        #region ctor

        public StreamWriteDataProvider(IFormatProvider formatProvider)
        {
            FormatProvider = formatProvider;
        }

        #endregion






        public byte[] GetDataByte()
        {
            throw new NotImplementedException();
        }



        public Stream GetStream()
        {
            try
            {
                var xmlRequest = FormatProvider.CreateDoc(InputData?.TableData);
                if (xmlRequest != null)
                {
                    var xmlVersion = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
                    var resultXmlDoc = xmlVersion + xmlRequest;
                    return  GenerateStreamFromString(resultXmlDoc);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }



        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }



        public bool SetDataByte(byte[] data)
        {
            //TODO: преобразовать массив байт обратно в строку и проверить ответ
            if (data != null && data.Length == 15)
            {
                return (data[0] == 78) && (data[1] == 85);
            }

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