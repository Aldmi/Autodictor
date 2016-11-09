using System.ComponentModel;


namespace Communication.Interfaces
{
    public interface IExchangeDataProvider<in TInput, out TOutput> : IExchangeDataProviderBase, INotifyPropertyChanged
    {
        TInput InputData { set; }          //передача входных даных внешним кодом.
        TOutput OutputData { get; }        //возврат выходных данных во внешний код.
        bool IsOutDataValid { get; }       // флаг валидности выходных данных (OutputData)
    }
}