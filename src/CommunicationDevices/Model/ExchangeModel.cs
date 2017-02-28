﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Windsor;
using Communication.SerialPort;
using Communication.Settings;
using CommunicationDevices.Behavior;
using CommunicationDevices.Behavior.BindingBehavior.ToGeneralSchedule;
using CommunicationDevices.Behavior.BindingBehavior.ToPath;
using CommunicationDevices.Behavior.ExhangeBehavior;
using CommunicationDevices.Behavior.ExhangeBehavior.PcBehavior;
using CommunicationDevices.Behavior.ExhangeBehavior.SerialPortBehavior;
using CommunicationDevices.Behavior.ExhangeBehavior.TcpIpBehavior;
using CommunicationDevices.ClientWCF;
using CommunicationDevices.DataProviders.VidorDataProvider;
using CommunicationDevices.Devices;
using CommunicationDevices.DI;
using CommunicationDevices.Infrastructure;
using CommunicationDevices.Settings;
using Library.Logs;
using Library.Xml;
using WCFAvtodictor2PcTableContract.DataContract;


namespace CommunicationDevices.Model
{
    /// <summary>
    /// ОСНОВНОЙ КЛАСС БИЗНЕСС ЛОГИКИ.
    /// СОДЕРЖИТ ВСЕ УСТРОЙСТВА, СЕРВИСЫ, ПОВЕДЕНИЯ НАД УСТРОЙСТВАМИ
    /// </summary>
    public class ExchangeModel : IDisposable 
    {
        #region field

        public static string NameRailwayStation;

        private readonly IWindsorContainer _container = new WindsorContainer();

        #endregion





        #region Prop

        public CisClient CisClient { get; private set; }

        public List<MasterSerialPort> MasterSerialPorts { get; set; } = new List<MasterSerialPort>();
        public List<Device> Devices { get; set; } = new List<Device>();

        public ICollection<IBinding2PathBehavior> Binding2PathBehaviors { get; set; } = new List<IBinding2PathBehavior>();
        public ICollection<IBinding2GeneralSchedule> Binding2GeneralSchedules { get; set; } = new List<IBinding2GeneralSchedule>();


        private string _errorString;
        public string ErrorString
        {
            get { return _errorString; }
            set
            {
                if (value == _errorString) return;
                _errorString = value;
                //сработка события
            }
        }

        public List<Task> BackGroundTasks { get; set; } = new List<Task>();

        #endregion






        #region ctor

        public ExchangeModel()
        {
            //РЕГИСТРАЦИЯ DI
            _container.Install(new WindsorConfig());

            //РЕГИСТРАЦИЯ МАППИНГА
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<UniversalInputType, UniversalDisplayType>();
                cfg.CreateMap<UniversalDisplayType, UniversalInputType>();
            });
        }

        #endregion





        public void StartCisClient()
        {
            CisClient?.Start();
        }


        public void StopCisClient()
        {
            CisClient?.Stop();
        }


        public async void LoadSetting()
        {
            //ЗАГРУЗКА НАСТРОЕК----------------------------------------------------------------------------------------------------------------------------
            List<XmlSerialSettings> xmlSerialPorts;
            List<XmlDeviceSerialPortSettings> xmlDeviceSpSettings;
            List<XmlDevicePcSettings> xmlDevicePcSettings;
            List<XmlDeviceTcpIpSettings> xmlDeviceTcpIpSettings;
            XmlCisSettings xmlCisSetting;

            try
            {
                var xmlFile = XmlWorker.LoadXmlFile("Settings", "Setting.xml"); //все настройки в одном файле
                if (xmlFile == null)
                    return;

                xmlSerialPorts = XmlSerialSettings.LoadXmlSetting(xmlFile);
                xmlDeviceSpSettings = XmlDeviceSerialPortSettings.LoadXmlSetting(xmlFile);
                xmlDevicePcSettings = XmlDevicePcSettings.LoadXmlSetting(xmlFile);
                xmlDeviceTcpIpSettings = XmlDeviceTcpIpSettings.LoadXmlSetting(xmlFile);
                xmlCisSetting= XmlCisSettings.LoadXmlSetting(xmlFile);
            }
            catch (FileNotFoundException ex)
            {
                ErrorString = "Файл Setting.xml не найденн";
                Log.log.Error(ErrorString);
                return;
            }
            catch (Exception ex)
            {
                ErrorString = "ОШИБКА в узлах дерева XML файла настроек:  " + ex;
                Log.log.Error(ErrorString);
                return;
            }


            //СОЗДАНИЕ КЛИЕНТА ЦИС---------------------------------------------------------------------------------------------------------
            NameRailwayStation = xmlCisSetting.Name;
            CisClient = new CisClient(new EndpointAddress(xmlCisSetting.EndpointAddress), Devices);



            //СОЗДАНИЕ ПОСЛЕДОВАТЕЛЬНЫХ ПОРТОВ----------------------------------------------------------------------------------------------------------
            foreach (var sp in xmlSerialPorts.Select(xmlSp => new MasterSerialPort(xmlSp)))
            {
                MasterSerialPorts.Add(sp);
            }


            //СОЗДАНИЕ УСТРОЙСТВ С ПОСЛЕДОВАТЕЛЬНЫМ ПОРТОМ------------------------------------------------------------------------------------------------
            foreach (var xmlDeviceSp in xmlDeviceSpSettings)
            {
                IExhangeBehavior behavior;
                byte maxCountFaildRespowne;
                switch (xmlDeviceSp.Name)
                {
                    case "DispSys":                      
                        maxCountFaildRespowne = 3;
                        behavior = new DisplSysExchangeBehavior(MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber), xmlDeviceSp.TimeRespone, maxCountFaildRespowne);
                        Devices.Add(new Device(xmlDeviceSp.Id, xmlDeviceSp.Address, xmlDeviceSp.Name, xmlDeviceSp.Description, behavior, xmlDeviceSp.BindingType));

                        //создание поведения привязка табло к пути.
                        if (xmlDeviceSp.BindingType == BindingType.ToPath)
                        {
                            var bindingBeh = new Binding2PathBehavior(Devices.Last(), xmlDeviceSp.PathNumbers, xmlDeviceSp.Contrains);
                            Binding2PathBehaviors.Add(bindingBeh);
                            bindingBeh.InitializeDevicePathInfo();                       //Вывод номера пути в пустом сообщении
                        }

                        //создание поведения привязка табло к главному расписанию
                        if (xmlDeviceSp.BindingType == BindingType.ToGeneral)
                            ;

                        //создание поведения привязка табло к системе отправление/прибытие поездов
                        if (xmlDeviceSp.BindingType == BindingType.ToArrivalAndDeparture)
                            ;

                        //добавим все функции циклического опроса
                        Devices.Last().AddCycleFunc();
                         break;


                    case "Vidor":
                        maxCountFaildRespowne = 3;
                        behavior = new VidorExchangeBehavior(MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber), xmlDeviceSp.TimeRespone, maxCountFaildRespowne);
                        Devices.Add(new Device(xmlDeviceSp.Id, xmlDeviceSp.Address, xmlDeviceSp.Name, xmlDeviceSp.Description, behavior, xmlDeviceSp.BindingType));

                        //создание поведения привязка табло к пути.
                        if (xmlDeviceSp.BindingType == BindingType.ToPath)
                        {
                            var bindingBeh = new Binding2PathBehavior(Devices.Last(), xmlDeviceSp.PathNumbers, xmlDeviceSp.Contrains);
                            Binding2PathBehaviors.Add(bindingBeh);
                            bindingBeh.InitializeDevicePathInfo();                      //Вывод номера пути в пустом сообщении
                        }

                        //создание поведения привязка табло к главному расписанию
                        if (xmlDeviceSp.BindingType == BindingType.ToGeneral)
                            ;

                        //создание поведения привязка табло к системе отправление/прибытие поездов
                        if (xmlDeviceSp.BindingType == BindingType.ToArrivalAndDeparture)
                            ;

                        //добавим все функции циклического опроса
                        Devices.Last().AddCycleFunc();
                        break;


                    case "VidorTable8":
                        maxCountFaildRespowne = 3;
                        var behTable8 = new VidorTableLineByLineExchangeSpBehavior(MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber), xmlDeviceSp.TimeRespone, maxCountFaildRespowne, 8, true, 1000)
                        {
                            ForTableViewDataProvider = new PanelVidorTableWriteDataProvider()
                        };
                        Devices.Add(new Device(xmlDeviceSp.Id, xmlDeviceSp.Address, xmlDeviceSp.Name, xmlDeviceSp.Description, behTable8, xmlDeviceSp.BindingType));

                        //создание поведения привязка табло к пути.
                        if (xmlDeviceSp.BindingType == BindingType.ToPath)
                            Binding2PathBehaviors.Add(new Binding2PathBehavior(Devices.Last(), xmlDeviceSp.PathNumbers, xmlDeviceSp.Contrains));

                        //создание поведения привязка табло к главному расписанию
                        if (xmlDeviceSp.BindingType == BindingType.ToGeneral)
                            ;

                        //создание поведения привязка табло к системе отправление/прибытие поездов
                        if (xmlDeviceSp.BindingType == BindingType.ToArrivalAndDeparture)
                            ;

                        //добавим все функции циклического опроса
                        Devices.Last().AddCycleFunc();
                        break;


                    case "VidorTableMin2":
                        maxCountFaildRespowne = 3;
                        var behTableMin2 = new VidorTableLineByLineExchangeSpBehavior(MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber),xmlDeviceSp.TimeRespone, maxCountFaildRespowne, 2, false, 10000)
                        {
                           ForTableViewDataProvider = new PanelVidorTableMinWriteDataProvider()
                        };
                        Devices.Add(new Device(xmlDeviceSp.Id, xmlDeviceSp.Address, xmlDeviceSp.Name, xmlDeviceSp.Description, behTableMin2, xmlDeviceSp.BindingType));

                        //создание поведения привязка табло к пути.
                        if (xmlDeviceSp.BindingType == BindingType.ToPath)
                            Binding2PathBehaviors.Add(new Binding2PathBehavior(Devices.Last(), xmlDeviceSp.PathNumbers, xmlDeviceSp.Contrains));

                        //создание поведения привязка табло к главному расписанию
                        if (xmlDeviceSp.BindingType == BindingType.ToGeneral)
                
                            

                        //создание поведения привязка табло к системе отправление/прибытие поездов
                        if (xmlDeviceSp.BindingType == BindingType.ToArrivalAndDeparture)
                            ;

                        //добавим все функции циклического опроса
                        Devices.Last().AddCycleFunc();
                        break;

                    default:
                        ErrorString = $" Устройсвто с именем {xmlDeviceSp.Name} не найденно";
                        Log.log.Error(ErrorString);
                        throw new Exception(ErrorString);
                }
            }



            //СОЗДАНИЕ УСТРОЙСТВ С PC ------------------------------------------------------------------------------------------------
            foreach (var xmlDevicePc in xmlDevicePcSettings)
            {
                IExhangeBehavior behavior;
                byte maxCountFaildRespowne;
                switch (xmlDevicePc.Name)
                {
                    case "PcTable":
                        maxCountFaildRespowne = 3;
                        behavior = new ArivDepartExhangePcBehavior(xmlDevicePc.Address, maxCountFaildRespowne);
                        Devices.Add(new Device(xmlDevicePc.Id, xmlDevicePc.Address, xmlDevicePc.Name, xmlDevicePc.Description, behavior, xmlDevicePc.BindingType));

                        //создание поведения привязка табло к пути.
                        if (xmlDevicePc.BindingType == BindingType.ToPath)
                        {
                            Binding2PathBehaviors.Add(new Binding2PathBehavior(Devices.Last(), xmlDevicePc.PathNumbers, xmlDevicePc.Contrains));
                            Devices.Last().AddCycleFunc(); //добавим все функции циклического опроса
                        }

                        //создание поведения привязка табло к главному расписанию
                        if (xmlDevicePc.BindingType == BindingType.ToGeneral)
                        {
                            Binding2GeneralSchedules.Add(new BindingDevice2GeneralShBehavior(Devices.Last(), xmlDevicePc.SourceLoad, xmlDevicePc.Contrains, xmlDevicePc.CountPage, xmlDevicePc.TimePaging));
                            //Если отключен пагинатор, то работаем по таймеру ExchangeBehavior ус-ва.
                            if (!Binding2GeneralSchedules.Last().IsPaging)
                            {
                                Devices.Last().AddCycleFunc();//добавим все функции циклического опроса
                            }
                        }

                        //создание поведения привязка табло к системе отправление/прибытие поездов
                        if (xmlDevicePc.BindingType == BindingType.ToArrivalAndDeparture)
                            ;

                 
                        break;


                    default:
                        ErrorString = $" Устройсвто с именем {xmlDevicePc.Name} не найденно";
                        Log.log.Error(ErrorString);
                        throw new Exception(ErrorString);
                }
            }




            //СОЗДАНИЕ УСТРОЙСТВ С TcpIp ------------------------------------------------------------------------------------------------
            foreach (var xmlDeviceTcpIp in xmlDeviceTcpIpSettings)
            {
                byte maxCountFaildRespowne;
                switch (xmlDeviceTcpIp.Name)
                {
                    case "VidorTable8":
                        maxCountFaildRespowne = 3;
                        var behTable8 = new VidorTableLineByLineExchangeTcpIpBehavior(xmlDeviceTcpIp.Address, xmlDeviceTcpIp.DeviceAdress, maxCountFaildRespowne, xmlDeviceTcpIp.TimeRespone, 8, false, 1000)
                        {
                            ForTableViewDataProvider = new PanelVidorTableWriteDataProvider()
                        };
                    
                        Devices.Add(new Device(xmlDeviceTcpIp.Id, xmlDeviceTcpIp.Address, xmlDeviceTcpIp.Name, xmlDeviceTcpIp.Description, behTable8, xmlDeviceTcpIp.BindingType));

                        //создание поведения привязка табло к пути.
                        if (xmlDeviceTcpIp.BindingType == BindingType.ToPath)
                            Binding2PathBehaviors.Add(new Binding2PathBehavior(Devices.Last(), xmlDeviceTcpIp.PathNumbers, xmlDeviceTcpIp.Contrains));

                        //создание поведения привязка табло к главному расписанию
                        if (xmlDeviceTcpIp.BindingType == BindingType.ToGeneral)
                            Binding2GeneralSchedules.Add(new BindingDevice2GeneralShBehavior(Devices.Last(), xmlDeviceTcpIp.SourceLoad, xmlDeviceTcpIp.Contrains, xmlDeviceTcpIp.CountPage, xmlDeviceTcpIp.TimePaging));

                        //создание поведения привязка табло к системе отправление/прибытие поездов
                        if (xmlDeviceTcpIp.BindingType == BindingType.ToArrivalAndDeparture)
                            ;

                        //добавим все функции циклического опроса
                        Devices.Last().AddCycleFunc();
                        break;


                     default:
                        ErrorString = $" Устройсвто с именем {xmlDeviceTcpIp.Name} не найденно";
                        Log.log.Error(ErrorString);
                        throw new Exception(ErrorString);
                }
            }


             //ЗАПУТИМ ФОНОВЫЕ ЗАДАЧИ ПО ПОДКЛЮЧЕНИЮ К УСТРО-ВАМ
             //Защита от повторного открытия одного и тогоже порта разными ус-вами.   
            var serialPortDev = Devices.Where(d => d.ExhBehavior is BaseExhangeSpBehavior).ToList();
            foreach (var devSp in serialPortDev.GroupBy(d => d.ExhBehavior.NumberPort).Select(g => g.First()))
            {
                devSp.ExhBehavior.CycleReConnect(BackGroundTasks);
            }

            var otherDev = Devices.Except(serialPortDev).ToList();
            foreach (var devSp in otherDev)
            {
                devSp.ExhBehavior.CycleReConnect(BackGroundTasks);
            }
        }



        public void Dispose()
        {
            CisClient?.Dispose();
            MasterSerialPorts?.ForEach(s=> s.Dispose());
        }
    }
}
