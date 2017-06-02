﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
    public class MyHttpResponse
    {
        public HttpResponseHeaders Headers { get; set; }
        public string Body { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }



    public class ClientHttp : INotifyPropertyChanged, IDisposable
    {
        #region fields

        private const int GetRequestStreamTimeout = 2000;

        private string _statusString;
        private bool _isConnect;
        private bool _isRunDataExchange;

        private readonly int _timeRespoune;              //время на ответ
        private readonly byte _numberTryingTakeData;     //кол-во попыток ожидания ответа до переподключения
        private byte _countTryingTakeData;               //счетчик попыток

        #endregion





        #region ctor

        public ClientHttp(string url, Dictionary<string, string> headers, int timeRespoune, byte numberTryingTakeData)
        {
            Url = url;
            _timeRespoune = timeRespoune;
            _numberTryingTakeData = numberTryingTakeData;
            Headers = headers;
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
                    StatusString = $"Ошибка инициализации соединения \"{Url}\": \"{ex.Message}\"   \"{ex.InnerException?.Message}\"";
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

            bool isValidOutDate;
            IsRunDataExchange = true;
            try
            {
                var response = await SendData(dataProvider);
                var responseBody =  TakeData(response);
                if (responseBody != null)
                {
                    var bytes = Encoding.UTF8.GetBytes(responseBody);
                     isValidOutDate= dataProvider.SetDataByte(bytes);
                    _countTryingTakeData = 0;
                }
                else //не смогли получить ответ ОК от сервера.
                {
                    Log.log.Fatal($"ERROR  (ReConnect)   Message= Данные не отправелнны"); //DEBUG_LOG
                    if (++_countTryingTakeData > _numberTryingTakeData)
                        ReConnect();

                    return false;
                }
            }
            catch (OperationCanceledException)
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

                return false;
            }
            catch (Exception ex)
            {
                StatusString = $"Неизвестное Исключение: {ex.Message}.   Внутренне исключение: {ex.InnerException?.Message ?? "" }";
                Log.log.Fatal($"ERROR  (RequestAndRespoune) Exception,  Message= {StatusString}"); //DEBUG_LOG
                ReConnect();
                return false;
            }
            IsRunDataExchange = false;
            return isValidOutDate;
        }




        public async Task<MyHttpResponse> SendData(IExchangeDataProviderBase dataProvider)
        {
            Stream stream = dataProvider.GetStream();
            if (stream == null)
                return null;

            if (Headers.ContainsKey("Methode") && Headers.ContainsKey("Content-Type"))
            {
                //ОБМЕН ДАННЫМИ POST multipart
                if (Headers["Methode"] == "POST" && Headers["Content-Type"] == "multipart/form-data")
                {
                    var boundary = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
                    return await SendMultipartHttp(Url, stream, boundary);
                }
            }

            return null;
        }




        /// <summary>
        /// Отправка потока через HttpClient POST Multipart.
        /// </summary>
        public async Task<MyHttpResponse> SendMultipartHttp(string uri, Stream stream, string boundary)
        {
            string mimeName = String.Empty;
            string mimeFileName = String.Empty;
            if (Headers.ContainsKey("Content-Type"))
            {
                if (Regex.Match(Headers["Content-Disposition"], "name=\"[^\"]*\"").Success)
                {
                    var mathStr = Regex.Match(Headers["Content-Disposition"], "name=\"[^\"]*\"").Groups[0].Value;
                    mimeName = mathStr.Substring(mathStr.IndexOf("=", StringComparison.Ordinal) + 1);
                }

                if (Regex.Match(Headers["Content-Disposition"], "filename=\"[^\"]*\"").Success)
                {
                    var mathStr = Regex.Match(Headers["Content-Disposition"], "filename=\"[^\"]*\"").Groups[0].Value;
                    mimeFileName = mathStr.Substring(mathStr.IndexOf("=", StringComparison.Ordinal) + 1);
                }
            }

            try
            {
                using (var client = new HttpClient())
                {
                    SetHeaders4HttpClient(client);
                    using (var content = new MultipartFormDataContent(boundary))
                    {
                        content.Add(new StreamContent(stream), mimeName, mimeFileName);
                        using (var respone = await client.PostAsync(uri, content).WithTimeout(GetRequestStreamTimeout))
                        {
                            var outputBody = await respone.Content.ReadAsStringAsync();
                            return new MyHttpResponse{Body = outputBody, StatusCode = respone.StatusCode, Headers = respone.Headers};
                        }
                    }
                }
            }
            catch (TimeoutException)
            {
                throw;
            }
            catch (WebException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }



        private void SetHeaders4HttpClient(HttpClient client)
        {
            foreach (var header in Headers)
            {
                switch (header.Key)
                {
                    case "User-Agent":
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        break;

                    case "Accept":
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        break;

                    case "Host":
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        break;

                    case "Connection":
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        break;

                    case "Authorization":
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        break;
                }
            }
        }



        public string TakeData(MyHttpResponse httpResponse)
        {
            if (httpResponse?.StatusCode == HttpStatusCode.OK)
            {
                var outputBody = httpResponse.Body;
                StatusString = $"Ответ получен: {outputBody}";
                return outputBody;
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

        }

        #endregion
    }
}