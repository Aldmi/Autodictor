using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunicationDevices.ClientWCF;
using CommunicationDevices.Devices;
using MainExample.Extension;
using WCFCis2AvtodictorContract.DataContract;
using CommunicationDevices.Behavior.SerialPortBehavior;
using CommunicationDevices.Infrastructure;
using CommunicationDevices.Settings;
using MainExample.Properties;


namespace MainExample
{
    public partial class BoardForm : Form
    {
        static public BoardForm MyBoardForm = null;
        private readonly IEnumerable<Device> _devises;




        public List<IDisposable> DispouseIsDataExchangeSuccessChangeRx { get; set; }= new List<IDisposable>();
        public List<IDisposable> DispouseIsConnectChangeRx { get; set; } = new List<IDisposable>();
        public List<IDisposable> DispouseLastSendDataChangeRx { get; set; } = new List<IDisposable>();



        public BoardForm(IEnumerable<Device> devices)
        {
            if (MyBoardForm != null)
                return;
            MyBoardForm = this;

            InitializeComponent();

            _devises = devices;
            if (_devises != null && _devises.Any())
                FillBoardsDataGrid(_devises);


            dataGridViewBoards.CellClick += dataGridView1_CellClick;

            foreach (var devise in _devises)
            {
                var disp= devise.ExhBehavior.IsDataExchangeSuccessChange.Subscribe(async exc =>
                {
                    var dev= _devises.FirstOrDefault(d => d.ExhBehavior.Equals(exc));
                    var row = _devises.ToList().IndexOf(dev);
                    dataGridViewBoards[7, row].Value = exc.DataExchangeSuccess ? Resources.ping_YES__ : Resources.ping_Error__;
                    await Task.Delay(300);
                    dataGridViewBoards[7, row].Value = Resources.ping_NO;
                });
                DispouseIsDataExchangeSuccessChangeRx.Add(disp);

                disp = devise.ExhBehavior.IsConnectChange.Subscribe(exc =>
                {
                    var dev = _devises.FirstOrDefault(d => d.ExhBehavior.Equals(exc));
                    var row = _devises.ToList().IndexOf(dev);
                    dataGridViewBoards[6, row].Value = exc.IsConnect ? Resources.OkImg : Resources.CancelImg;
                });
                DispouseIsConnectChangeRx.Add(disp);

                disp = devise.ExhBehavior.LastSendDataChange.Subscribe(exc =>
                {
                    var dev = _devises.FirstOrDefault(d => d.ExhBehavior.Equals(exc));
                    var row = _devises.ToList().IndexOf(dev);
                    dataGridViewBoards[8, row].Value = exc.LastSendData.Message;
                });
                DispouseLastSendDataChangeRx.Add(disp);
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var dataGridViewColumn = dataGridViewBoards.Columns["Action"];
            if (dataGridViewColumn != null && e.ColumnIndex == dataGridViewColumn.Index && e.RowIndex >= 0)
            {
                var sendStr = (string)dataGridViewBoards[e.ColumnIndex - 1, e.RowIndex].FormattedValue;
                var type= (string)dataGridViewBoards[e.ColumnIndex - 6, e.RowIndex].FormattedValue;

                var inData = new UniversalInputType { Message = sendStr };

                switch (type)
                {
                    case "Путевое":                        //TODO: парсить Message для заполненния полей inData.
                        //inData.NumberOfTrain = "111";
                        //inData.PathNumber = "12";
                        //inData.Event = "ПРИБ.";
                        //inData.Time = new DateTime(2016, 11, 30, 18, 49, 0);  //16:20
                        //inData.Stations = "НОВОСИБИРСК";
                        //inData.Message = $"ПОЕЗД:{inData.NumberOfTrain}, ПУТЬ:{inData.NumberOfTrain}, СОБЫТИЕ:{inData.Event}, СТАНЦИИ:{inData.Stations}, ВРЕМЯ:{inData.Time.ToShortTimeString()}";


                        inData.NumberOfTrain = "Э/П";
                        inData.PathNumber = "6";
                        inData.Event = "";
                        inData.Time = new DateTime(2016, 11, 30, 16, 50, 00);  //16:50
                        inData.Stations = "КРЮКОВО-ЛАСТОЧКА";
                        inData.Message = $"ПОЕЗД:{inData.NumberOfTrain}, ПУТЬ:{inData.NumberOfTrain}, СОБЫТИЕ:{inData.Event}, СТАНЦИИ:{inData.Stations}, ВРЕМЯ:{inData.Time.ToShortTimeString()}";

                        if (sendStr == "AddRow")
                        {
                            if (_devises.ToList()[e.RowIndex].ExhBehavior.GetData4CycleFunc[0].TableData != null)
                            {
                                _devises.ToList()[e.RowIndex].ExhBehavior.GetData4CycleFunc[0].TableData.Add(inData);                             // Изменили данные для циклического опроса
                                _devises.ToList()[e.RowIndex].AddOneTimeSendData(_devises.ToList()[e.RowIndex].ExhBehavior.GetData4CycleFunc[0]); // Отправили однократный запрос
                            }
                        }
                        else
                        if(sendStr == "RemoveRow")
                        {
                            var delRow = _devises.ToList()[e.RowIndex].ExhBehavior.GetData4CycleFunc[0].TableData.LastOrDefault();
                            if (delRow != null)
                            {
                                _devises.ToList()[e.RowIndex].ExhBehavior.GetData4CycleFunc[0].TableData.Remove(delRow);                            // Изменили данные для циклического опроса
                                _devises.ToList()[e.RowIndex].AddOneTimeSendData(_devises.ToList()[e.RowIndex].ExhBehavior.GetData4CycleFunc[0]);   // Отправили однократный запрос
                            }
                        }

                        break;


                    case "Основное":
                        break;

                    case "Прибытие/Отправление":
                        break;

                    default:
                       break;
                }

               // _devises.ToList()[e.RowIndex].AddOneTimeSendData(inData);

                //_devises.ToList()[e.RowIndex].AddCycleFuncData(0, inData);

               // _devises.ToList()[e.RowIndex].ExhBehavior.GetData4CycleFunc[0].TableData.Add(inData);
            }
        }


        private void FillBoardsDataGrid(IEnumerable<Device> dev)
        {
            foreach (var d in dev)
            {
                string bindType;
                switch (d.BindingType)
                {
                   case BindingType.ToPath:
                        bindType = "Путевое";
                        break;

                    case BindingType.ToGeneral:
                        bindType = "Основное";
                        break;

                    case BindingType.ToArrivalAndDeparture:
                        bindType = "Прибытие/Отправление";
                        break;

                    default:
                        bindType = "НЕИЗВЕСТНО";
                        break;
                }

                object[] row =
                {
                    d.Id.ToString(),
                    d.Address,
                    d.Name,
                    bindType,
                    d.Description,
                    $"Порт {d.ExhBehavior.NumberSp} : {(d.ExhBehavior.IsOpen ? "Открыт" : "Закрыт")}",
                    d.ExhBehavior.IsConnect ? Resources.OkImg : Resources.CancelImg,
                    Resources.ping_NO,
                    d.ExhBehavior.LastSendData == null ? String.Empty : d.ExhBehavior.LastSendData.Message,
                };
                this.InvokeIfNeeded(() => dataGridViewBoards.Rows.Add(row));
            }
        }


        protected override void OnClosed(EventArgs e)
        {
            if (MyBoardForm == this)
                MyBoardForm = null;

            DispouseIsDataExchangeSuccessChangeRx.ForEach(disp => disp.Dispose());
            DispouseIsConnectChangeRx.ForEach(disp => disp.Dispose());
            DispouseLastSendDataChangeRx.ForEach(disp => disp.Dispose());

            dataGridViewBoards.CellClick -= dataGridView1_CellClick;

            base.OnClosed(e);
        }
    }
}
