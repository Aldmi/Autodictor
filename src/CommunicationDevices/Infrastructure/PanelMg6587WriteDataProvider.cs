using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Communication.Annotations;
using Communication.Interfaces;


namespace CommunicationDevices.Infrastructure
{

    public class Mg6587Input
    {
        public byte Var11 { get; set; }
        public byte Var12 { get; set; }
    }


    public class Mg6587Output
    {
        public byte Var22 { get; set; }
        public byte Var23 { get; set; }
    }



    class PanelMg6587WriteDataProvider : IExchangeDataProvider<UniversalInputType, Mg6587Output>
    {
        #region field

        private const ushort StartAddresWrite = 0x0002;
        private const ushort NWriteRegister = 0x0001;

        #endregion




        #region Prop

        public int CountGetDataByte { get; } = 8;
        public int CountSetDataByte { get; } = 9;

        public UniversalInputType InputData { get; set; }
        public Mg6587Output OutputData { get; }

        public bool IsOutDataValid { get; }

        #endregion




        /// <summary>
        /// Данные запроса по записи информации о билете (функц 0x10):
        /// байт[0]= InputData.Сashbox
        /// байт[1]= 0x10
        /// байт[2]= Адр. Ст.
        /// байт[3]= Адр. Мл.
        /// байт[4]= Кол-во. рег. Ст.
        /// байт[5]= Кол-во. рег. Мл.
        /// байт[6]= Кол-во. байт
        /// байт[7]= Название билета (0...9 бит - число 0...1023) 
        /// байт[8]= Название билета (10...15 бит - буква 0-A  Z-25) 
        /// байт[9]= CRC Мл.
        /// байт[10]= CRC Ст.
        /// </summary>
        public byte[] GetDataByte()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Обработка ответа на данные записи информации о билете (функц 0x10):
        /// байт[0]= InputData.Сashbox
        /// байт[1]= 0x10
        /// байт[2]= Адр. Ст.
        /// байт[3]= Адр. Мл.
        /// байт[4]= Кол-во. рег. Ст.
        /// байт[5]= Кол-во. рег. Мл.
        /// байт[6]= CRC Мл.
        /// байт[7]= CRC Ст.
        /// </summary>
        public bool SetDataByte(byte[] data)
        {
            throw new NotImplementedException();
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
