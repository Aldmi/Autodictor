using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Communication.Annotations;
using LedScreenLibNetWrapper;
using LedScreenLibNetWrapper.Impl;

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
        #region prop

        public DisplayDriver DisplayDriver { get; set; } = new DisplayDriver();
        public SettingSibWay SettingSibWay { get; set; }

        private string _statusString;
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

        private bool _isConnect;
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

        private bool _isRunDataExchange;
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




        #region ctor

        public SibWay(SettingSibWay settingSibWay)
        {
            SettingSibWay = settingSibWay;
        }

        #endregion




        #region Method

        public async Task ReConnect()
        {
            IsConnect = false;
            OnPropertyChanged(nameof(IsConnect));
            Dispose();

            await Connect();
        }


        private async Task Connect()
        {
            while (!IsConnect)
            {
                try
                {
                    DisplayDriver.Initialize(SettingSibWay.Ip, SettingSibWay.Port);
                    var errorCode = await OpenConnectionAsync();
                    IsConnect = (errorCode == ErrorCode.ERROR_SUCCESS);
                    IsConnect = true;//DEBUG!!!!!!!!!!!!!!!
                    StatusString = $"Conect to {SettingSibWay.Ip} : {SettingSibWay.Port} ...";
                    await Task.Delay(SettingSibWay.Time2Reconnect);
                }
                catch (Exception ex)
                {
                    IsConnect = false;
                    StatusString = $"Ошибка инициализации соединения: \"{ex.Message}\"";
                    //LogException.WriteLog("Инициализация: ", ex, LogException.TypeLog.TcpIp);
                    Dispose();
                }
            }
            StatusString = $"Conect Sucsess: {SettingSibWay.Ip} : {SettingSibWay.Port} ...";
        }


        /// <summary>
        /// Не блокирующая операция открытия соедининия. 
        /// </summary>
        public async Task<ErrorCode> OpenConnectionAsync()
        {
            return await Task<ErrorCode>.Factory.StartNew(() =>
            (ErrorCode)DisplayDriver.OpenConection());
        }



        public async Task<bool> SendData(IList<ItemSibWay> sibWayItems)
        {
            if (!IsConnect)
            {
                return false;
            }

            IsRunDataExchange = true;
            try
            {
                //Отправка информации каждому окну---------------------------------------
                foreach (var winSett in SettingSibWay.WindowSett)
                {
                    //Ограничим кол-во строк для окна.
                    var maxWindowHeight = winSett.Height;
                    var fontSize = SettingSibWay.FontSize;
                    var nItems = maxWindowHeight / fontSize;
                    var items = sibWayItems.Take(nItems).ToList();

                    //Сформируем список строк.
                    var sendingStrings = CreateListSendingStrings(winSett, items)?.ToList();

                    //Отправим список строк.
                    if (sendingStrings != null && sendingStrings.Any())
                    {
                        var result = await SendMessageAsync(winSett, sendingStrings, fontSize);
                        if (result == false) //Если в результате отправки даных окну возникла ошибка, то уходим на цикл ReConnect и прерываем отправку данных.
                        {
                            ReConnect();
                            return false;
                        }
                        await Task.Delay(winSett.DelayBetweenSending);
                    }
                }
            }
            catch (Exception ex)
            {
                // rtb_Status.Text += ex + "\n";
            }
            finally
            {
                IsRunDataExchange = false;
            }

            return true;
        }



        private IEnumerable<string> CreateListSendingStrings(WindowSett winSett, IList<ItemSibWay> items)
        {
            //Создаем список строк отправки для каждого окна------------------------------
            var listString= new List<string>();
            foreach (var sh in items)
            {
                string trimStr = null;
                switch (winSett.ColumnName)
                {
                        
                    case nameof(sh.TypeTrain):
                        trimStr = TrimStrOnWindowWidth(sh.TypeTrain, winSett.Width);
                        break;

                    case nameof(sh.NumberOfTrain):
                        trimStr = TrimStrOnWindowWidth(sh.NumberOfTrain, winSett.Width);
                        break;

                    case nameof(sh.PathNumber):
                        trimStr = TrimStrOnWindowWidth(sh.PathNumber, winSett.Width);
                        break;

                    case nameof(sh.Event):
                        trimStr = TrimStrOnWindowWidth(sh.Event, winSett.Width);
                        break;

                    case nameof(sh.Addition):
                        trimStr = TrimStrOnWindowWidth(sh.Addition, winSett.Width);
                        break;

                    case nameof(sh.Stations):
                        trimStr = TrimStrOnWindowWidth(sh.Stations, winSett.Width);
                        break;

                    case nameof(sh.DirectionStation):
                        trimStr = TrimStrOnWindowWidth(sh.DirectionStation, winSett.Width);
                        break;

                    case nameof(sh.Note):
                        trimStr = TrimStrOnWindowWidth(sh.Note, winSett.Width);
                        break;

                    case nameof(sh.DaysFollowingAlias):
                        trimStr = TrimStrOnWindowWidth(sh.DaysFollowingAlias, winSett.Width);
                        break;

                    case nameof(sh.TimeDeparture):
                        trimStr = TrimStrOnWindowWidth(sh.TimeDeparture.ToString("HH:mm"), winSett.Width);
                        break;

                    case nameof(sh.TimeArrival):
                        trimStr = TrimStrOnWindowWidth(sh.TimeArrival.ToString("HH:mm"), winSett.Width);
                        break;

                    case nameof(sh.DelayTime):
                        trimStr = TrimStrOnWindowWidth(sh.DelayTime?.ToString("HH:mm") ?? string.Empty , winSett.Width);
                        break;

                    case nameof(sh.ExpectedTime):
                        trimStr = TrimStrOnWindowWidth(sh.ExpectedTime.ToString("HH:mm"), winSett.Width);
                        break;

                    case nameof(sh.StopTime):
                        trimStr = TrimStrOnWindowWidth(sh.StopTime?.ToString("HH:mm") ?? string.Empty, winSett.Width);
                        break;
                }

                if (trimStr != null)
                {
                    listString.Add(trimStr);
                }
            }
            

            return listString;
        }



        private async Task<bool> SendMessageAsync(WindowSett winSett, IEnumerable<string> sendingStrings, int fontSize)
        {
            uint colorRgb = BitConverter.ToUInt32(winSett.ColorBytes, 0);
            string text = GetResultString(sendingStrings);

            var textHeight = DisplayTextHeight.px8;
            switch (fontSize)
            {
                case 8:
                    textHeight = DisplayTextHeight.px8;
                    break;
                case 12:
                    textHeight = DisplayTextHeight.px12;
                    break;
                case 16:
                    textHeight = DisplayTextHeight.px16;
                    break;
                case 24:
                    textHeight = DisplayTextHeight.px24;
                    break;
                case 32:
                    textHeight = DisplayTextHeight.px32;
                    break;
            }


            StatusString = "Отправка на экран " + winSett.Number + "\n" + text + "\n";
            var err = await Task<ErrorCode>.Factory.StartNew(() => (ErrorCode)DisplayDriver.SendMessage(
                 winSett.Number,
                 winSett.Effect,
                 winSett.TextHAlign,
                 winSett.TextVAlign,
                 winSett.DisplayTime,
                 textHeight,
                 colorRgb,
                 text));

            StatusString = "Отправка на экран " + winSett.Number + "errorCode= " + err + "\n";
            return (err == ErrorCode.ERROR_SUCCESS);// TODO: В случае верного ответа отправить true.
        }



        private string TrimStrOnWindowWidth(string str, int width)
        {
            var path = SettingSibWay.Path2FontFile;
            if (File.Exists(path))
            {
                //Измерим в пикселях размер текста
                using (var tu = new TextUtility())
                {
                    tu.Initialize(path);
                    while (tu.MeasureString(str) > width)
                    {
                        str = str.Remove(str.Length - 1);
                    }
                    return str;
                }
            }
            return str;
        }


        private string GetResultString(IEnumerable<string> list)
        {
            var strBuilder = new StringBuilder();
            foreach (var l in list)
            {
                strBuilder.Append(l);
                strBuilder.Append("\n");
            }

            return strBuilder.Remove(strBuilder.Length - 1, 1).ToString(); //удалить послдений символ \n
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
            DisplayDriver?.CloseConection();
            DisplayDriver?.Dispose();
        }

        #endregion
    }
}