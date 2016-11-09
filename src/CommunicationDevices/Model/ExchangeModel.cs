using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Castle.Windsor;
using Communication.SerialPort;
using Communication.Settings;
using CommunicationDevices.Devices;
using CommunicationDevices.Devices.SerialPortDevices;
using CommunicationDevices.DI;
using CommunicationDevices.Infrastructure;
using CommunicationDevices.Settings;
using Library.Logs;
using Library.Xml;


namespace CommunicationDevices.Model
{
    public class ExchangeModel
    {
        #region field

        private readonly IWindsorContainer _container = new WindsorContainer();

        #endregion





        #region Prop

        public List<MasterSerialPort> MasterSerialPorts { get; set; } = new List<MasterSerialPort>();
        public List<DeviceSp> Devices { get; set; } = new List<DeviceSp>();

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





        public ExchangeModel()
        {
            _container.Install(new WindsorConfig());
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
            //foreach (var dev in xmlDeviceSpSettings.Select(xmlDev => new Devices(xmlDev)))
            //{
            //    Devices.Add(dev);
            //}



            //перебираем xml настройки Device, для каждого уcтройства


            foreach (var xmlDeviceSp in xmlDeviceSpSettings)
            {
                switch (xmlDeviceSp.Name)
                {
                    case "MG6587":
                        Devices.Add(new DeviceSpMg6587(xmlDeviceSp, MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber)));
                        break;

                    case "MG7777":
                        Devices.Add(new DeviceSpMg6587(xmlDeviceSp, MasterSerialPorts.FirstOrDefault(s => s.PortNumber == xmlDeviceSp.PortNumber)));
                        break;

                    default:
                        throw new Exception($" Устройсвто с именем {xmlDeviceSp.Name} не найденно");
                }
            }

            //Использование------------------------------------------------------------

            //Все порты которые используют устройства откроем и запустим.
            foreach (var devSp in Devices)
            {
                if (devSp.Port != null)
                {
                    var taskSerialPort = Task.Factory.StartNew(async () =>
                    {
                        if (await devSp.Port.CycleReConnect())
                        {
                            var taskCashierEx = devSp.Port.RunExchange();
                            BackGroundTasks.Add(taskCashierEx);
                        }
                    });
                    BackGroundTasks.Add(taskSerialPort);
                }
            }

            var dev = Devices.FirstOrDefault(n=> n.Name == "MG6587");
            dev.AddOneTimeSendData(new UniversalInputType { Message = "Поезд 1 прибывает на 2 путь в 19:56" });

            //Devices.Add(new DeviceSpMg6587<byte,bool>(xmlDeviceSpSettings.First()));
            //var dev = Devices.First(d => d.Id == 1);
            //var port = MasterSerialPorts.First();
            //dev?.ExchangeService(port, CancellationToken.None, new Mg6587Input {Var11 = 10, Var12 = 58});


            //СОЗДАНИЕ ПОСЛЕД. ПОРТА ДЛЯ ОПРОСА КАССИРОВ-----------------------------------------------------------------------
            //MasterSerialPort = new MasterSerialPort(xmlSerial);
            //CashierExchangeService = new CashierExchangeService(DeviceCashiers, xmlSerial.TimeRespoune);
            //MasterSerialPort.AddFunc(CashierExchangeService.ExchangeService);
            //MasterSerialPort.PropertyChanged += (o, e) =>
            //{
            //    var port = o as MasterSerialPort;
            //    if (port != null)
            //    {
            //        if (e.PropertyName == "StatusString")
            //        {
            //            ErrorString = port.StatusString;
            //        }
            //    }
            //};
        }
    }
}
