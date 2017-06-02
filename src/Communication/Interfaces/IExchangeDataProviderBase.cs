
using System.IO;

namespace Communication.Interfaces
{
    public interface IExchangeDataProviderBase
    {
        byte[] GetDataByte();            //сформировать буфер для отправки.
        bool SetDataByte(byte[] data);   //получить принятый буфер.

        Stream GetStream();              // получить поток отправки


        int CountGetDataByte { get; }    //кол-во байт для отправки.
        int CountSetDataByte { get; }    //кол-во байт для приема.
    }
}
