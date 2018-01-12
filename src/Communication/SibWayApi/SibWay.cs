using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Communication.Annotations;

namespace Communication.SibWayApi
{

    // Коды ошибок
    public enum ErrorCode
    {
        ERROR_SUCCESS = 0,
        ERROR_GENERAL_ERROR = -1,
        ERROR_CONNECTION_FAILED = -2,
        ERROR_NOT_CONNECTED = -3,
        ERROR_TIMEOUT = -4,
        ERROR_WRONG_RESPONSE = -5,
        ERROR_ALREADY_CONNECTED = -6,
        ERROR_EMPTY_RESPONSE = -7,
        ERROR_WRONG_LENGTH = -8,
        ERROR_CRC_ERROR = -9,
        ERROR_RESPONSE_UNKNOWN = -10,
        ERROR_UNSUPPORTED_RESPONSE = -11,
        ERROR_FILE_NOT_FOUND = -12,
        ERROR_INVALID_XML_CONFIGURATION = -13
    }


    public class SibWay : INotifyPropertyChanged, IDisposable
    {
        #region fields


        #endregion



        #region ctor



        #endregion




        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion




        #region Disposable

        public void Dispose()
        {
            //if (_terminalNetStream != null)
            //{
            //    _terminalNetStream.Close();
            //    StatusString = "Сетевой поток закрыт ...";
            //}

            //_terminalClient?.Client?.Close();
        }

        #endregion
    }
}