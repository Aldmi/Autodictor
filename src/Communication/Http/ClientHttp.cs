using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Communication.Annotations;
using Communication.Interfaces;
using Communication.Settings;
using Library.Async;
using Library.Extensions;
using Library.Logs;


namespace Communication.Http
{
    public class ClientHttp : INotifyPropertyChanged, IDisposable
    {
        #region fields

        private const int GetRequestStreamTimeout = 2000;

        private HttpWebResponse _httpWebResponse;


        private string _statusString;
        private bool _isConnect;
        private bool _isRunDataExchange;

        private readonly int _timeRespoune;              //время на ответ
        private readonly byte _numberTryingTakeData;     //кол-во попыток ожидания ответа до переподключения
        private byte _countTryingTakeData;               //счетчик попыток

        #endregion





        #region ctor

        public ClientHttp(string url, string methode, string contentType, int timeRespoune, byte numberTryingTakeData)
        {
            Url = url;
            Method = methode;
            ContentType = contentType;
            _timeRespoune = timeRespoune;
            _numberTryingTakeData = numberTryingTakeData;

            Headers = new Dictionary<string, string>();
        }

        #endregion





        #region prop

        public string Url { get; set; }
        public string Method { get; set; }            //GET;POST;
        public string ContentType { get; set; }



        public Dictionary<string, string> Headers { get; set; }

        public string StatusString
        {
            get { return _statusString; }
            set
            {
                if (value == _statusString) return;
                _statusString = value;
                OnPropertyChanged();
            }
        }

        public bool IsConnect
        {
            get { return _isConnect; }
            set
            {
                if (value == _isConnect) return;
                _isConnect = value;
                OnPropertyChanged();
            }
        }

        public bool IsRunDataExchange
        {
            get { return _isRunDataExchange; }
            set
            {
                if (value == _isRunDataExchange) return;
                _isRunDataExchange = value;
                OnPropertyChanged();
            }
        }

        #endregion




        #region Method

        public async Task ReConnect()
        {
            OnPropertyChanged(nameof(IsConnect));
            IsConnect = false;
            _countTryingTakeData = 0;
            Dispose();

            await ConnectHttp();
        }


        private async Task ConnectHttp()
        {
            while (!IsConnect)
            {
                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
                    httpWebRequest.Method = "GET";

                    //попытка получить поток запроса, для проверки соединения с удаленным сервером.
                    var testConnectResp = (HttpWebResponse)await httpWebRequest.GetResponseAsync().WithTimeout(_timeRespoune);
                    if (testConnectResp != null && testConnectResp.StatusCode == HttpStatusCode.OK)
                    {
                        testConnectResp.Close();
                        testConnectResp.Dispose();

                        IsConnect = true;

                        Log.log.Fatal($"OK  (ConnectHttp)   Message= поток запроса по \"{Url}\" ПОЛУЧЕНН !!!"); //DEBUG_LOG
                        return;
                    }

                    //Log.log.Fatal($"ERROR  (ConnectHttp)   Message= поток запроса по \"{Url}\" НЕ ПОЛУЧЕНН"); //DEBUG_LOG
                    IsConnect = false;
                }
                catch (Exception ex)
                {
                    IsConnect = false;
                    StatusString = $"Ошибка инициализации соединения \"{Url}\": \"{ex.Message}\"";
                    Log.log.Fatal($"ERROR  (ConnectHttp)   Message= {StatusString}"); //DEBUG_LOG
                    Dispose();
                }
            }
        }



        public async Task<bool> RequestAndRespoune(IExchangeDataProviderBase dataProvider)
        {
            if (!IsConnect)
                return false;

            if (dataProvider == null)
                return false;

            IsRunDataExchange = true;
            try
            {
                //DEBUG-------------------
                bool? sendResult = null;//await SendData(dataProvider);
                if(sendResult == null)
                    return false;

                if (sendResult.Value)
                {
                    var data = await TakeData(dataProvider.CountSetDataByte, CancellationToken.None);
                    dataProvider.SetDataByte(data);
                    _countTryingTakeData = 0;
                }
                else //не смогли отрпавить данные.
                {
                    Log.log.Fatal($"ERROR  (ReConnect)   Message= Данные не отправелнны"); //DEBUG_LOG
                    if (++_countTryingTakeData > _numberTryingTakeData)
                        ReConnect();

                    return false;
                }
            }
            catch(OperationCanceledException)
            {
                StatusString = "операция  прерванна";

                if (++_countTryingTakeData > _numberTryingTakeData)
                    ReConnect();

                return false;
            }
            catch (TimeoutException)
            {
                StatusString = "Время на ожидание ответа вышло";
                Log.log.Fatal($"ERROR  (RequestAndRespoune) TimeoutException,  Message= {StatusString}"); //DEBUG_LOG
                if (++_countTryingTakeData > _numberTryingTakeData)
                    ReConnect();

                return false;
            }
            catch (WebException we)
            {
                StatusString = $"Неизвестное Исключение: {we.Message}.   Внутренне исключение: {we.InnerException?.Message ?? "" }";
                Log.log.Fatal($"ERROR  (RequestAndRespoune) WebException,  Message= {StatusString}"); //DEBUG_LOG
                if (++_countTryingTakeData > _numberTryingTakeData)
                    ReConnect();
            }
            catch(Exception ex)
            {
                StatusString = $"Неизвестное Исключение: {ex.Message}.   Внутренне исключение: {ex.InnerException?.Message ?? "" }";
                Log.log.Fatal($"ERROR  (RequestAndRespoune) Exception,  Message= {StatusString}"); //DEBUG_LOG
                ReConnect();
                return false;
            }
            IsRunDataExchange = false;
            return true;
        }




        public async Task<bool?> SendData(IExchangeDataProviderBase dataProvider)
        {
            byte[] buffer = dataProvider.GetDataByte();

            if (buffer == null)
                return null;


            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
            httpWebRequest.Method = Method;
            httpWebRequest.ContentLength = buffer.Length;
            httpWebRequest.ContentType = ContentType;
            httpWebRequest.KeepAlive = true;

            StatusString = $"Отправка запроса.... на \"{Url}\"";
            Log.log.Fatal($"OK  (SendData)   Message= {StatusString}"); //DEBUG_LOG

            //Получить поток запроса
            using (var requestStream = await httpWebRequest.GetRequestStreamAsync().WithTimeout(GetRequestStreamTimeout))   //блокирующий вызов
            {
                if (requestStream == null)
                {
                    Log.log.Fatal($"ERROR  (SendData)   Message= поток запроса по \"{Url}\" НЕ ПОЛУЧЕНН"); //DEBUG_LOG
                    return false;
                }

                //Записать буффер в поток
                await requestStream.WriteAsync(buffer, 0, buffer.Length);
                StatusString = "запись байт в поток....";
                Log.log.Fatal($"OK  (SendData)   Message= {StatusString}"); //DEBUG_LOG
                requestStream.Close();

                //Отправка данных и ожидание ответа
                _httpWebResponse = null;
                _httpWebResponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync().WithTimeout(_timeRespoune);
                Log.log.Fatal($"OK  (SendData)   Message= запрос успешно отправленн"); //DEBUG_LOG
                return true;
            }
        }



        public async Task<byte[]> TakeData(int nbytes, CancellationToken ct)
        {
            if (_httpWebResponse == null)
                return null;

            try
            {
                if (_httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    //if (_httpWebResponse.ContentLength != nbytes)
                    //    return null;

                    StatusString = "Ответ получен !!!";
                    Log.log.Fatal($"OK  (TakeData)   Message= {StatusString}"); //DEBUG_LOG
                    return new byte[] { 0xAA, 0xBB }; //псевдовалидные данные для DataProvider
                    //using (var responeStream = new StreamReader(_httpWebResponse.GetResponseStream(), Encoding.UTF8))
                    //{
                    //    var bData = await responeStream.ReadToEndAsync(); //TODO:преобразовать в массив байт и вернуть
                    //    return new byte[] { 0xAA, 0xBB };
                    //}
                }
            }
            finally
            {
                _httpWebResponse?.Close();
                _httpWebResponse?.Dispose();
            }

            return null;
        }

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
            _httpWebResponse?.Close();
            _httpWebResponse?.Dispose();
            _httpWebResponse = null;
        }

        #endregion
    }
}