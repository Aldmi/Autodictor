﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Castle.Windsor;
using Communication.SerialPort;
using Communication.Settings;
using CommunicationDevices.Behavior;
using CommunicationDevices.Behavior.BindingBehavior;
using CommunicationDevices.Behavior.BindingBehavior.ToPath;
using CommunicationDevices.Behavior.SerialPortBehavior;
using CommunicationDevices.ClientWCF;
using CommunicationDevices.Devices;
using CommunicationDevices.DI;
using CommunicationDevices.Infrastructure;
using CommunicationDevices.Settings;
using Library.Logs;
using Library.Xml;


namespace CommunicationDevices.Model
{
    public class ExchangeModel : IDisposable 
    {
        #region field

        private readonly IWindsorContainer _container = new WindsorContainer();

        #endregion





        #region Prop

        public CisClient CisClient { get; private set; }

        public List<MasterSerialPort> MasterSerialPorts { get; set; } = new List<MasterSerialPort>();
        public List<Device> Devices { get; set; } = new List<Device>();
        public ICollection<IBinding2PathBehavior> BindingBehaviors { get; set; } = new List<IBinding2PathBehavior>();

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
        public Log Log { get; set; }
        public List<Task> BackGroundTasks { get; set; } = new List<Task>();

        #endregion






        #region ctor

        public ExchangeModel()
        {
            _container.Install(new WindsorConfig());
        }

        #endregion






        public void CreateCisClient(EndpointAddress endpointAddress)
        {
            CisClient= new CisClient(endpointAddress, Devices);
        }


        public void StartCisClient()
        {
            CisClient.Start();
        }


        public void StopCisClient()
        {
            CisClient.Stop();
        }


        public async void LoadSetting()
        {
            //ЗАГРУЗКА НАСТРОЕК----------------------------------------------------------------------------------------------------------------------------
            List<XmlSerialSettings> xmlSerialPorts;
            XmlLogSettings xmlLog;
            List<XmlDeviceSerialPortSettings> xmlDeviceSpSettings;
            try
            {
                var xmlFile = XmlWorker.LoadXmlFile("Settings", "Setting.xml"); //все настройки в одном файле
                if (xmlFile == null)
                    return;

                xmlSerialPorts = XmlSerialSettings.LoadXmlSetting(xmlFile);
                xmlLog = XmlLogSettings.LoadXmlSetting(xmlFile);
                xmlDeviceSpSettings = XmlDeviceSerialPortSettings.LoadXmlSetting(xmlFile);
            }
            catch (FileNotFoundException ex)
            {
                ErrorString = "Файл Setting.xml не найденн";
                return;
            }
            catch (Exception ex)
            {
                ErrorString = "ОШИБКА в узлах дерева XML файла настроек:  " + ex;
                return;
            }


            //СОЗДАНИЕ ЛОГА------------------------------------------------------------------------------------------------------------------------------
            Log = new Log("CommunicationLog.txt", xmlLog);


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
                        Devices.Add(new Device(xmlDeviceSp, behavior));

                        //создание поведения привязка табло к пути.
                        if(xmlDeviceSp.BindingType == BindingType.ToPath)
                           BindingBehaviors.Add(new Binding2PathBehavior(Devices.Last(), xmlDeviceSp.PathNumbers));
                       
                        //создание поведения привязка табло к главному расписанию
                        if (xmlDeviceSp.BindingType == BindingType.ToGeneral)
                            ;

                        //создание поведения привязка табло к системе отправление/прибытие поездов
                        if (xmlDeviceSp.BindingType == BindingType.ToGeneral)
                            ;

                        //добавим все функции циклического опроса
                       // Devices.Last().AddCycleFunc();
                         break;


                    case "Vidor":
                        maxCountFaildRespowne = 3;
                        behavior = new VidorExchangeBehavior(MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber), xmlDeviceSp.TimeRespone, maxCountFaildRespowne);
                        Devices.Add(new Device(xmlDeviceSp, behavior));

                        //создание поведения привязка табло к пути.
                        if (xmlDeviceSp.BindingType == BindingType.ToPath)
                            BindingBehaviors.Add(new Binding2PathBehavior(Devices.Last(), xmlDeviceSp.PathNumbers));

                        //создание поведения привязка табло к главному расписанию
                        if (xmlDeviceSp.BindingType == BindingType.ToGeneral)
                            ;

                        //создание поведения привязка табло к системе отправление/прибытие поездов
                        if (xmlDeviceSp.BindingType == BindingType.ToGeneral)
                            ;

                        //добавим все функции циклического опроса
                       // Devices.Last().AddCycleFunc();
                        break;



                    case "VidorTable8":
                        maxCountFaildRespowne = 3;
                        behavior = new VidorTableExchangeBehavior(MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber), xmlDeviceSp.TimeRespone, maxCountFaildRespowne, 8);
                        Devices.Add(new Device(xmlDeviceSp, behavior));

                        //создание поведения привязка табло к пути.
                        if (xmlDeviceSp.BindingType == BindingType.ToPath)
                            BindingBehaviors.Add(new Binding2PathBehavior(Devices.Last(), xmlDeviceSp.PathNumbers));

                        //создание поведения привязка табло к главному расписанию
                        if (xmlDeviceSp.BindingType == BindingType.ToGeneral)
                            ;

                        //создание поведения привязка табло к системе отправление/прибытие поездов
                        if (xmlDeviceSp.BindingType == BindingType.ToArrivalAndDeparture)
                            ;

                        //добавим все функции циклического опроса
                       // Devices.Last().AddCycleFunc();
                        break;

                    default:
                        throw new Exception($" Устройсвто с именем {xmlDeviceSp.Name} не найденно");
                }
            }


            //Все порты которые используют устройства откроем и запустим.
            foreach (var devSp in Devices.GroupBy(d=> d.ExhBehavior.NumberSp).Select(g=> g.First()))
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
