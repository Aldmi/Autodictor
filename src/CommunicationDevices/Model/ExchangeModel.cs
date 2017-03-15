using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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
using CommunicationDevices.DataProviders;
using CommunicationDevices.DataProviders.VidorDataProvider;
using CommunicationDevices.Devices;
using CommunicationDevices.DI;
using CommunicationDevices.Settings;
using CommunicationDevices.Settings.XmlCisSettings;
using CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings;
using CommunicationDevices.Settings.XmlDeviceSettings.XmlTransportSettings;

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
            List<XmlSpSetting> xmlDeviceSpSettings;
            List<XmlPcSetting> xmlDevicePcSettings;
            List<XmlTcpIpSetting> xmlDeviceTcpIpSettings;
            XmlCisSetting xmlCisSetting;

            try
            {
                var xmlFile = XmlWorker.LoadXmlFile("Settings", "Setting.xml"); //все настройки в одном файле
                if (xmlFile == null)
                    return;

                xmlSerialPorts = XmlSerialSettings.LoadXmlSetting(xmlFile);
                xmlDeviceSpSettings = XmlSettingFactory.CreateXmlSpSetting(xmlFile);
                xmlDevicePcSettings = XmlSettingFactory.CreateXmlPcSetting(xmlFile);
                xmlDeviceTcpIpSettings = XmlSettingFactory.CreateXmlTcpIpSetting(xmlFile);
                xmlCisSetting = XmlCisSetting.LoadXmlSetting(xmlFile);
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

                XmlBindingSetting binding = null;
                XmlContrainsSetting contrains = null;
                XmlPagingSetting paging = null;
                XmlCountRowSetting countRow = null;
                XmlPathPermissionSetting pathPermission = null;

                if (xmlDeviceSp.SpecialDictionary.ContainsKey("Binding"))
                {
                    binding = xmlDeviceSp.SpecialDictionary["Binding"] as XmlBindingSetting;
                }

                if (xmlDeviceSp.SpecialDictionary.ContainsKey("Contrains"))
                {
                    contrains = xmlDeviceSp.SpecialDictionary["Contrains"] as XmlContrainsSetting;
                }

                if (xmlDeviceSp.SpecialDictionary.ContainsKey("Paging"))
                {
                    paging = xmlDeviceSp.SpecialDictionary["Paging"] as XmlPagingSetting;
                }

                if (xmlDeviceSp.SpecialDictionary.ContainsKey("CountRow"))
                {
                    countRow = xmlDeviceSp.SpecialDictionary["CountRow"] as XmlCountRowSetting;
                }

                if (xmlDeviceSp.SpecialDictionary.ContainsKey("PathPermission"))
                {
                    pathPermission = xmlDeviceSp.SpecialDictionary["PathPermission"] as XmlPathPermissionSetting;
                }

                var setting = new DeviceSetting
                {
                    PathPermission = pathPermission?.Enable ?? true
                };

                //привязка обязательный параметр
                if (binding == null)
                {
                    MessageBox.Show($"Не указанны настройки привязки у ус-ва {xmlDeviceSp.Id}");
                    return;
                }

                switch (xmlDeviceSp.Name)
                {
                    case "DispSys":
                        maxCountFaildRespowne = 3;
                        behavior = new DisplSysExchangeBehavior(MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber), xmlDeviceSp.TimeRespone, maxCountFaildRespowne);
                        Devices.Add(new Device(xmlDeviceSp.Id, xmlDeviceSp.Address, xmlDeviceSp.Name, xmlDeviceSp.Description, behavior, binding.BindingType, setting));

                        //создание поведения привязка табло к пути.
                        if (binding.BindingType == BindingType.ToPath)
                        {
                            var bindingBeh = new Binding2PathBehavior(Devices.Last(), binding.PathNumbers, contrains?.Contrains);
                            Binding2PathBehaviors.Add(bindingBeh);
                            bindingBeh.InitializeDevicePathInfo();                       //Вывод номера пути в пустом сообщении
                        }

                        //создание поведения привязка табло к главному расписанию
                        if (binding.BindingType == BindingType.ToGeneral)
                            ;

                        //создание поведения привязка табло к системе отправление/прибытие поездов
                        if (binding.BindingType == BindingType.ToArrivalAndDeparture)
                            ;

                        //добавим все функции циклического опроса
                        Devices.Last().AddCycleFunc();
                        break;


                    case "Vidor":
                        maxCountFaildRespowne = 3;
                        behavior = new VidorExchangeBehavior(MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber), xmlDeviceSp.TimeRespone, maxCountFaildRespowne);
                        Devices.Add(new Device(xmlDeviceSp.Id, xmlDeviceSp.Address, xmlDeviceSp.Name, xmlDeviceSp.Description, behavior, binding.BindingType, setting));

                        //создание поведения привязка табло к пути.
                        if (binding.BindingType == BindingType.ToPath)
                        {
                            var bindingBeh = new Binding2PathBehavior(Devices.Last(), binding.PathNumbers, contrains?.Contrains);
                            Binding2PathBehaviors.Add(bindingBeh);
                            bindingBeh.InitializeDevicePathInfo();                      //Вывод номера пути в пустом сообщении
                        }

                        //создание поведения привязка табло к главному расписанию
                        if (binding.BindingType == BindingType.ToGeneral)
                            ;

                        //создание поведения привязка табло к системе отправление/прибытие поездов
                        if (binding.BindingType == BindingType.ToArrivalAndDeparture)
                            ;

                        //добавим все функции циклического опроса
                        Devices.Last().AddCycleFunc();
                        break;


                    case "VidorTableStr1":
                        maxCountFaildRespowne = 3;

                        // кол-во строк обязательный параметр
                        if (countRow == null)
                        {
                            MessageBox.Show($"Не указанны кол-во строк у многострочного табло {xmlDeviceSp.Id}");
                            return;
                        }

                        var behTable8 = new VidorTableLineByLineExchangeSpBehavior(MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber), xmlDeviceSp.TimeRespone, maxCountFaildRespowne, countRow.CountRow, true, 1000)
                        {
                            ForTableViewDataProvider = new PanelVidorTable1StrWriteDataProvider()
                        };
                        Devices.Add(new Device(xmlDeviceSp.Id, xmlDeviceSp.Address, xmlDeviceSp.Name, xmlDeviceSp.Description, behTable8, binding.BindingType, setting));

                        //создание поведения привязка табло к пути.
                        if (binding.BindingType == BindingType.ToPath)
                            Binding2PathBehaviors.Add(new Binding2PathBehavior(Devices.Last(), binding.PathNumbers, contrains?.Contrains));

                        //создание поведения привязка табло к главному расписанию
                        if (binding.BindingType == BindingType.ToGeneral)
                            ;

                        //создание поведения привязка табло к системе отправление/прибытие поездов
                        if (binding.BindingType == BindingType.ToArrivalAndDeparture)
                            ;

                        //добавим все функции циклического опроса
                        Devices.Last().AddCycleFunc();
                        break;


                    case "VidorTableStr2":
                        maxCountFaildRespowne = 3;

                        // кол-во строк обязательный параметр
                        if (countRow == null)
                        {
                            MessageBox.Show($"Не указанны кол-во строк у многострочного табло {xmlDeviceSp.Id}");
                            return;
                        }

                        var behTableMin2 = new VidorTableLineByLineExchangeSpBehavior(MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber), xmlDeviceSp.TimeRespone, maxCountFaildRespowne, countRow.CountRow, true, 10000)
                        {
                            ForTableViewDataProvider = new PanelVidorTable2StrWriteDataProvider()
                        };
                        Devices.Add(new Device(xmlDeviceSp.Id, xmlDeviceSp.Address, xmlDeviceSp.Name, xmlDeviceSp.Description, behTableMin2, binding.BindingType, setting));

                        //создание поведения привязка табло к пути.
                        if (binding.BindingType == BindingType.ToPath)
                            Binding2PathBehaviors.Add(new Binding2PathBehavior(Devices.Last(), binding.PathNumbers, contrains?.Contrains));


                        //создание поведения привязка табло к главному расписанию
                        if (binding.BindingType == BindingType.ToGeneral)
                            ;

                        //создание поведения привязка табло к системе отправление/прибытие поездов
                        if (binding.BindingType == BindingType.ToArrivalAndDeparture)
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

                XmlBindingSetting binding = null;
                XmlContrainsSetting contrains = null;
                XmlPagingSetting paging = null;
                XmlCountRowSetting countRow = null;
                XmlPathPermissionSetting pathPermission = null;

                if (xmlDevicePc.SpecialDictionary.ContainsKey("Binding"))
                {
                    binding = xmlDevicePc.SpecialDictionary["Binding"] as XmlBindingSetting;
                }

                if (xmlDevicePc.SpecialDictionary.ContainsKey("Contrains"))
                {
                    contrains = xmlDevicePc.SpecialDictionary["Contrains"] as XmlContrainsSetting;
                }

                if (xmlDevicePc.SpecialDictionary.ContainsKey("Paging"))
                {
                    paging = xmlDevicePc.SpecialDictionary["Paging"] as XmlPagingSetting;
                }

                if (xmlDevicePc.SpecialDictionary.ContainsKey("CountRow"))
                {
                    countRow = xmlDevicePc.SpecialDictionary["CountRow"] as XmlCountRowSetting;
                }

                if (xmlDevicePc.SpecialDictionary.ContainsKey("PathPermission"))
                {
                    pathPermission = xmlDevicePc.SpecialDictionary["PathPermission"] as XmlPathPermissionSetting;
                }

                var setting = new DeviceSetting
                {
                    PathPermission = pathPermission?.Enable ?? true
                };

                //привязка обязательный параметр
                if (binding == null)
                {
                    MessageBox.Show($"Не указанны настройки привязки у ус-ва {xmlDevicePc.Id}");
                    return;
                }

                switch (xmlDevicePc.Name)
                {
                    case "PcTable":
                        maxCountFaildRespowne = 3;

                        // кол-во строк обязательный параметр
                        if (paging == null)
                        {
                            MessageBox.Show($"Не указанны настройки paging у PcTable табло {xmlDevicePc.Id}");
                            return;
                        }

                        behavior = new ArivDepartExhangePcBehavior(xmlDevicePc.Address, maxCountFaildRespowne);
                        Devices.Add(new Device(xmlDevicePc.Id, xmlDevicePc.Address, xmlDevicePc.Name, xmlDevicePc.Description, behavior, binding.BindingType, setting));

                        //создание поведения привязка табло к пути.
                        if (binding.BindingType == BindingType.ToPath)
                        {
                            Binding2PathBehaviors.Add(new Binding2PathBehavior(Devices.Last(), binding.PathNumbers, contrains?.Contrains));
                            Devices.Last().AddCycleFunc(); //добавим все функции циклического опроса
                        }

                        //создание поведения привязка табло к главному расписанию
                        if (binding.BindingType == BindingType.ToGeneral)
                        {
                            Binding2GeneralSchedules.Add(new BindingDevice2GeneralShBehavior(Devices.Last(), binding.SourceLoad, contrains?.Contrains, paging.CountPage, paging.TimePaging));
                            //Если отключен пагинатор, то работаем по таймеру ExchangeBehavior ус-ва.
                            if (!Binding2GeneralSchedules.Last().IsPaging)
                            {
                                Devices.Last().AddCycleFunc();//добавим все функции циклического опроса
                            }
                        }

                        //создание поведения привязка табло к системе отправление/прибытие поездов
                        if (binding.BindingType == BindingType.ToArrivalAndDeparture)
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

                XmlBindingSetting binding = null;
                XmlContrainsSetting contrains = null;
                XmlPagingSetting paging = null;
                XmlCountRowSetting countRow = null;
                XmlPathPermissionSetting pathPermission = null;

                if (xmlDeviceTcpIp.SpecialDictionary.ContainsKey("Binding"))
                {
                    binding = xmlDeviceTcpIp.SpecialDictionary["Binding"] as XmlBindingSetting;
                }

                if (xmlDeviceTcpIp.SpecialDictionary.ContainsKey("Contrains"))
                {
                    contrains = xmlDeviceTcpIp.SpecialDictionary["Contrains"] as XmlContrainsSetting;
                }

                if (xmlDeviceTcpIp.SpecialDictionary.ContainsKey("Paging"))
                {
                    paging = xmlDeviceTcpIp.SpecialDictionary["Paging"] as XmlPagingSetting;
                }

                if (xmlDeviceTcpIp.SpecialDictionary.ContainsKey("CountRow"))
                {
                    countRow = xmlDeviceTcpIp.SpecialDictionary["CountRow"] as XmlCountRowSetting;
                }

                if (xmlDeviceTcpIp.SpecialDictionary.ContainsKey("PathPermission"))
                {
                    pathPermission = xmlDeviceTcpIp.SpecialDictionary["PathPermission"] as XmlPathPermissionSetting;
                }

                var setting = new DeviceSetting
                {
                    PathPermission = pathPermission?.Enable ?? true
                };

                //привязка обязательный параметр
                if (binding == null)
                {
                    MessageBox.Show($"Не указанны настройки привязки у ус-ва {xmlDeviceTcpIp.Id}");
                    return;
                }


                switch (xmlDeviceTcpIp.Name)
                {
                    case "VidorTableStr1":
                        maxCountFaildRespowne = 3;

                        // кол-во строк обязательный параметр
                        if (countRow == null)
                        {
                            MessageBox.Show($"Не указанны кол-во строк у многострочного табло {xmlDeviceTcpIp.Id}");
                            return;
                        }

                        var behTable8 = new VidorTableLineByLineExchangeTcpIpBehavior(xmlDeviceTcpIp.Address, xmlDeviceTcpIp.DeviceAdress, maxCountFaildRespowne, xmlDeviceTcpIp.TimeRespone, countRow.CountRow, true, 1000)
                        {
                            ForTableViewDataProvider = new PanelVidorTable1StrWriteDataProvider()
                        };

                        Devices.Add(new Device(xmlDeviceTcpIp.Id, xmlDeviceTcpIp.Address, xmlDeviceTcpIp.Name, xmlDeviceTcpIp.Description, behTable8, binding.BindingType, setting));

                        //создание поведения привязка табло к пути.
                        if (binding.BindingType == BindingType.ToPath)
                            Binding2PathBehaviors.Add(new Binding2PathBehavior(Devices.Last(), binding.PathNumbers, contrains?.Contrains));

                        //создание поведения привязка табло к главному расписанию
                        if (binding.BindingType == BindingType.ToGeneral)
                            Binding2GeneralSchedules.Add(new BindingDevice2GeneralShBehavior(Devices.Last(), binding.SourceLoad, contrains?.Contrains, paging.CountPage, paging.TimePaging));

                        //создание поведения привязка табло к системе отправление/прибытие поездов
                        if (binding.BindingType == BindingType.ToArrivalAndDeparture)
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
            MasterSerialPorts?.ForEach(s => s.Dispose());
        }
    }
}
