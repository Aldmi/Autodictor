using System;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutodictorBL.Entites;
using AutodictorBL.Settings.XmlSound;
using AutodictorBL.Sound.Converters;
using Library.Logs;
using PRAESIDEOOPENINTERFACECOMSERVERLib;
using Domain.Entitys;

namespace AutodictorBL.Sound
{

    public enum StartCreatedCallState { Error, Timeout, Ok };



    public class PlayerOmneo : ISoundPlayer
    {
        #region Fileld

        private readonly PraesideoOpenInterface_V0430 _praesideoOi;
        private int? _currentCallId;
        private readonly string _ip;
        private readonly int _port;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _defaultZoneNames;          //Название маршрутов по умолчанию (через запятую). Если в ВоспроизводимоеСообщение не указанн маршрут, пользуемя маршрутом по умолчанию
        private readonly int _timeDelayReconnect;
        private readonly int _timeResponse;                 //Время контроллера  на ответ о начале проигрывания файла.
        private SoundPlayerStatus _soundPlayerStatus;

        #endregion




        #region prop

        public SoundPlayerType PlayerType { get; }

        private string _statusString;
        public string StatusString
        {
            get { return _statusString; }
            set
            {
                if (value == _statusString) return;
                _statusString = value;
                StatusStringChangeRx.OnNext(_statusString);
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
                IsConnectChangeRx.OnNext(_isConnect);
            }
        }


        public IFileNameConverter FileNameConverter => new Omneo8CharacterFileNameConverter();

        #endregion




        #region ctor

        public PlayerOmneo(SoundPlayerType playerType, string ip, int port, string userName, string password, string defaultZoneNames, int timeDelayReconnect, int timeResponse)
        {
            _ip = ip;
            _port = port;
            _userName = userName;
            _password = password;
            _defaultZoneNames = defaultZoneNames;
            _timeDelayReconnect = timeDelayReconnect;
            _timeResponse = timeResponse;
            PlayerType = playerType;

            _praesideoOi = new PraesideoOpenInterface_V0430();
            _praesideoOi.connectionBroken += PraesideoOiOnConnectionBroken;
            _praesideoOi.callState += PraesideoOI_callState;

            //DEBUG--------------------------------------
            _praesideoOi.diagEvent += _praesideoOi_diagEvent;
            _praesideoOi.resourceFaultState += _praesideoOi_resourceFaultState;
            _praesideoOi.resourceState += _praesideoOi_resourceState;
            _praesideoOi.unitCount += _praesideoOi_unitCount;
            _praesideoOi.virtualControlInputState += _praesideoOi_virtualControlInputState;
        }


        private void _praesideoOi_virtualControlInputState(string virtualControlInputs, bool active)
        {
            Log.log.Info($"virtualControlInputState: virtualControlInputs= {virtualControlInputs} active= {active}   "); //DEBUG_LOG
        }


        //DEBUG--------------------------------------
        private void _praesideoOi_diagEvent(int eventId, [ComAliasName("PRAESIDEOOPENINTERFACECOMSERVERLib.TOIActionType")] TOIActionType action, object pDiagEvent)
        {
            Log.log.Info($"diagEvent: eventId= {eventId}   action= {action}  pDiagEvent= {pDiagEvent ?? "empty"} "); //DEBUG_LOG
        }

        private void _praesideoOi_resourceFaultState(string resources, [ComAliasName("PRAESIDEOOPENINTERFACECOMSERVERLib.TOIResourceFaultState")] TOIResourceFaultState faultState)
        {
            Log.log.Info($"resourceFaultState: resources= {resources}   faultState= {faultState}"); //DEBUG_LOG
        }

        private void _praesideoOi_resourceState(string resources, [ComAliasName("PRAESIDEOOPENINTERFACECOMSERVERLib.TOIResourceState")] TOIResourceState state, int priority, int callId)
        {
            Log.log.Info($"resourceState: resources= {resources}   state= {state}   priority= {priority}   callId= {callId}"); //DEBUG_LOG
        }

        private void _praesideoOi_unitCount(int unitCount)
        {
            Log.log.Info($"unitCount: unitCount= {unitCount} "); //DEBUG_LOG
        }

        #endregion




        #region RxEvent

        public Subject<string> StatusStringChangeRx { get; } = new Subject<string>(); //Изменение StatusString
        public Subject<bool> IsConnectChangeRx { get; } = new Subject<bool>(); //Изменение IsConnect

        #endregion



        #region Methode

        /// <summary>
        /// Получить информацию о плеере
        /// </summary>
        public string GetInfo()
        {
            var getAudioInputNames = _praesideoOi.getAudioInputNames() ?? "NULL";
            var getBgmChannelNames = _praesideoOi.getBgmChannelNames() ?? "NULL";
            var getChimeNames = _praesideoOi.getChimeNames() ?? "NULL";
            var getConfigId = _praesideoOi.getConfigId();
            var getConfiguredUnits = _praesideoOi.getConfiguredUnits() ?? "NULL";
            var getConnectedUnits = _praesideoOi.getConnectedUnits() ?? "NULL";
            var getMessageNames = _praesideoOi.getMessageNames() ?? "NULL";
            var getNcoVersion = _praesideoOi.getNcoVersion() ?? "NULL";
            var getVersion = _praesideoOi.getVersion() ?? "NULL";
            var getVirtualControlInputNames = _praesideoOi.getVirtualControlInputNames() ?? "NULL";
            var getZoneGroupNames = _praesideoOi.getZoneGroupNames() ?? "NULL";          //Зоны объединяются в группы, имена групп зон и есть маршрут. (пустая строка для всех зон)


            return $@"getAudioInputNames = {getAudioInputNames}" + "\n\n" +
                            $"getBgmChannelNames = {getBgmChannelNames}" + "\n\n" +
                            $"getChimeNames = {getChimeNames}" + "\n\n" +                //имя файла стартового звонка (перед основным сообщением)
                            $"getConfigId = {getConfigId}" + "\n\n" +
                            $"getConfiguredUnits = {getConfiguredUnits}" + "\n\n" +
                            $"getConnectedUnits = {getConnectedUnits}" + "\n\n" +
                            $"getMessageNames = {getMessageNames}" + "\n\n" +
                            $"getNcoVersion = {getNcoVersion}" + "\n\n" +
                            $"getVersion = {getVersion}" + "\n\n" +
                            $"getVirtualControlInputNames = {getVirtualControlInputNames}" + "\n\n" +
                            $"getZoneGroupNames = {getZoneGroupNames}";
        }



        private bool _isRunningReconnect = false;
        public async Task ReConnect()
        {
            if (!_isRunningReconnect)
            {
                _isRunningReconnect = true;
                Log.log.Info($"ERROR OMNEO  (ReConnect Start)"); //DEBUG_LOG
                await Disconnect();
                await Connect();
            }
        }


        private async Task Connect()
        {
            while (!IsConnect)
            {
                try
                {
                    await Task.Factory.StartNew(async () =>
                    {
                        _praesideoOi.connect(_ip, _port, _userName, _password);
                        await Task.Delay(1000);
                    });

                    _soundPlayerStatus = SoundPlayerStatus.Idle;
                    IsConnect = true;
                }
                catch (COMException ex)
                {
                    await Disconnect();
                    IsConnect = false;
                    StatusString = $"Exception Ошибка подключения: \"{ex.Message}\"  \"{ex.InnerException?.Message}\"";
                    Log.log.Error($"ERROR OMNEO  (Connect)   Message= {StatusString}"); //DEBUG_LOG
                }
                catch (Exception ex)
                {
                    await Disconnect();
                    IsConnect = false;
                    StatusString = $"Exception НЕИЗВЕСТНАЯ Ошибка подключения: \"{ex.Message}\"  \"{ex.InnerException?.Message}\"";
                    Log.log.Error($"ERROR OMNEO  (Connect)   Message= {StatusString}"); //DEBUG_LOG
                }
                finally
                {
                    await Task.Delay(_timeDelayReconnect);
                }
            }
        }



        public async Task Disconnect()
        {
            try
            {
                _soundPlayerStatus = SoundPlayerStatus.Error;
                _praesideoOi.disconnect();
                await Task.Delay(1000);
                IsConnect = false;
            }
            catch (Exception)
            {
                // ignored
            }
        }



        public async Task<bool> PlayFile(ВоспроизводимоеСообщение soundMessage, bool useFileNameConverter = true)
        {
            if (!IsConnect)
                return false;

            try
            {
                _soundPlayerStatus = SoundPlayerStatus.Playing;

                if (soundMessage == null)
                {
                    if (_currentCallId.HasValue && _soundPlayerStatus == SoundPlayerStatus.Playing)
                    {
                        Log.log.Error($"PlayFile:СТОП>>>>>>---------------------------"); //DEBUG_LOG
                        _praesideoOi.stopCall(_currentCallId.Value);
                    }
                    return false;
                }

                //-----
                string langPostfix = string.Empty;
                switch (soundMessage.Язык)
                {
                    case NotificationLanguage.Eng:
                        langPostfix = "_" + NotificationLanguage.Eng;
                        break;
                }
                var track = soundMessage.ИмяВоспроизводимогоФайла;
                track += langPostfix;
                //------

                Log.log.Error($"PlayFile:ПОПЫТКА ЗАПУСКА>>>>>> ИмяВоспроизводимогоФайла = {track}"); //DEBUG_LOG


                int priority = 100;
                bool bPartial = false;
                string startChime = string.Empty;
                string endChime = string.Empty;
                bool bLiveSpeech = false;
                string audioInput = string.Empty;
                string messages = useFileNameConverter ? FileNameConverter.Convert(track) : track;
                int repeat = 0;
                _currentCallId = _praesideoOi.createCall(_defaultZoneNames, priority, bPartial, startChime, endChime, bLiveSpeech, audioInput, messages, repeat);
                await Play();
                return true;
            }
            catch (Exception ex) //НЕ найден файл на OMNEO
            {
                _soundPlayerStatus = SoundPlayerStatus.Idle;
                StatusString = @"Ошибка получения CallId файла: " + ex.Message;
                Log.log.Error($"ERROR OMNEO: Message= {StatusString}"); //DEBUG_LOG
                return false;
            }
        }



        public async Task<bool> Play()
        {
            if (!IsConnect)
                return false;

            Log.log.Error($"PlayFile:ПОПЫТКА ПРОИГРЫВАНИЯ>>>>>>"); //DEBUG_LOG
            var resul = await PlayWithControl();
            switch (resul)
            {
                case StartCreatedCallState.Ok:
                    _soundPlayerStatus = SoundPlayerStatus.Playing;
                    return true;

                case StartCreatedCallState.Timeout:
                    _soundPlayerStatus = SoundPlayerStatus.Idle;
                    Log.log.Error($"ERROR OMNEO  (Play.Timeout)  Ответ не полученн за {_timeResponse}"); //DEBUG_LOG
                    break;

                case StartCreatedCallState.Error:
                    _soundPlayerStatus = SoundPlayerStatus.Error;
                    ReConnect();
                    break;
            }
            return false;
        }


        /// <summary>
        /// Запускает задачу _tcs которая длится _timeResponse (мсек)  - допустимое время на оповещение.
        /// Усилитель оповестит о реальном запуске файла для проигрывания в обработчике события PraesideoOI_callState.
        /// Это завершит задачу, раньше чем пройдет 3 сек или возникнет Exception внутри PlayWithControl.
        /// </summary>
        private TaskCompletionSource<StartCreatedCallState> _tcs;
        private Task<StartCreatedCallState> PlayWithControl()
        {
            _tcs = new TaskCompletionSource<StartCreatedCallState>();
            Task.Run(async () =>
            {
                if (_currentCallId != null)
                {
                    try
                    {
                        _praesideoOi.startCreatedCall(_currentCallId.Value);
                        await Task.Delay(_timeResponse);
                        _tcs.TrySetResult(StartCreatedCallState.Timeout);
                    }
                    catch (Exception ex)
                    {
                        Log.log.Error($"ERROR OMNEO  (PlayWithControl)  {ex}");
                        _tcs.TrySetResult(StartCreatedCallState.Error);
                    }
                }
            });

            return _tcs.Task;
        }


        public void Pause()
        {
            if (!IsConnect)
                return;

            if (_currentCallId != null)
            {
                try
                {
                    _praesideoOi.stopCall(_currentCallId.Value);
                }
                catch (Exception ex)
                {
                    Log.log.Error($"ERROR OMNEO  (Pause)  {ex}"); //DEBUG_LOG
                }
            }
        }



        public float GetDuration()
        {
            if (!IsConnect)
                return 0;

            throw new NotImplementedException();
        }



        public int GetCurrentPosition()
        {
            if (!IsConnect)
                return 0;

            throw new NotImplementedException();
        }



        public SoundPlayerStatus GetPlayerStatus()
        {
            if (!IsConnect)
                return SoundPlayerStatus.Error;

            return _soundPlayerStatus;
        }



        public int GetVolume()
        {
            if (!IsConnect)
                return 0;

            throw new NotImplementedException();
        }



        public void SetVolume(int volume)
        {
            if (!IsConnect)
                return;

            _praesideoOi.setBgmVolume(_defaultZoneNames, volume);
        }

        #endregion





        #region EventHandler

        /// <summary>
        /// вызывается при разрыве соединения
        /// </summary>
        private async void PraesideoOiOnConnectionBroken()
        {
            Log.log.Info($"ERROR OMNEO  (PraesideoOiOnConnectionBroken)"); //DEBUG_LOG
            await ReConnect();
        }


        /// <summary>
        /// вызывается при изменении состояния вызова
        /// OICS_IDLE -> OICS_START -> OICS_MESSAGES -> OICS_END. 
        /// </summary>
        private void PraesideoOI_callState(int callId, [System.Runtime.InteropServices.ComAliasName("PRAESIDEOOPENINTERFACECOMSERVERLib.TOICallState")] TOICallState state)
        {
            switch (state)
            {
                case TOICallState.OICS_START:
                    _tcs.TrySetResult(StartCreatedCallState.Ok);
                    _soundPlayerStatus = SoundPlayerStatus.Playing;
                    StatusString = "СТАРТ проигрывания";
                    break;

                case TOICallState.OICS_STARTCHIME:
                    _soundPlayerStatus = SoundPlayerStatus.Playing;
                    break;

                case TOICallState.OICS_MESSAGES:
                    _soundPlayerStatus = SoundPlayerStatus.Playing;
                    StatusString = "СООБЩЕНИЕ";
                    break;

                case TOICallState.OICS_LIVESPEECH:
                    _soundPlayerStatus = SoundPlayerStatus.Playing;
                    break;

                case TOICallState.OICS_END:
                    _soundPlayerStatus = SoundPlayerStatus.Idle;
                    StatusString = "СТОП проигрывания";
                    break;

                case TOICallState.OICS_ABORT:
                    _tcs.TrySetResult(StartCreatedCallState.Ok);
                    _soundPlayerStatus = SoundPlayerStatus.Stop;
                    StatusString = "ОТМЕНА";
                    break;

                case TOICallState.OICS_IDLE:
                    _soundPlayerStatus = SoundPlayerStatus.Idle;
                    StatusString = "ПРОСТОЙ";
                    break;

                case TOICallState.OICS_REPLAY:
                    _soundPlayerStatus = SoundPlayerStatus.Playing;
                    StatusString = "REPLAY";
                    break;
            }
            Log.log.Error($"callState: StatusString= {StatusString}");
        }

        #endregion





        #region Dispouse

        public void Dispose()
        {
            Disconnect();
        }

        #endregion
    }
}