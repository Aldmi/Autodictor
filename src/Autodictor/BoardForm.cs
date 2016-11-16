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
using MainExample.Properties;


namespace MainExample
{
    public partial class BoardForm : Form
    {
        static public BoardForm MyBoardForm = null;
        private readonly IEnumerable<DeviceSp> _devises;




        public List<IDisposable> DispouseIsDataExchangeSuccessChangeRx { get; set; }= new List<IDisposable>();
        public List<IDisposable> DispouseIsConnectChangeRx { get; set; } = new List<IDisposable>();
        public List<IDisposable> DispouseLastSendDataChangeRx { get; set; } = new List<IDisposable>();



        public BoardForm(IEnumerable<DeviceSp> devices)
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
                var disp= devise.SpExhBehavior.IsDataExchangeSuccessChange.Subscribe(async exc =>
                {
                    var dev= _devises.FirstOrDefault(d => d.SpExhBehavior.Equals(exc));
                    var row = _devises.ToList().IndexOf(dev);
                    dataGridViewBoards[6, row].Value = exc.DataExchangeSuccess ? Resources.ping_YES__ : Resources.ping_Error__;
                    await Task.Delay(300);
                    dataGridViewBoards[6, row].Value = Resources.ping_NO;
                });
                DispouseIsDataExchangeSuccessChangeRx.Add(disp);

                disp = devise.SpExhBehavior.IsConnectChange.Subscribe(exc =>
                {
                    var dev = _devises.FirstOrDefault(d => d.SpExhBehavior.Equals(exc));
                    var row = _devises.ToList().IndexOf(dev);
                    dataGridViewBoards[5, row].Value = exc.IsConnect ? Resources.OkImg : Resources.CancelImg;
                });
                DispouseIsConnectChangeRx.Add(disp);

                disp = devise.SpExhBehavior.LastSendDataChange.Subscribe(exc =>
                {
                    var dev = _devises.FirstOrDefault(d => d.SpExhBehavior.Equals(exc));
                    var row = _devises.ToList().IndexOf(dev);
                    dataGridViewBoards[7, row].Value = exc.LastSendData.Message;
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
                _devises.ToList()[e.RowIndex].AddOneTimeSendData(new UniversalInputType { Message = sendStr });
            }
        }


        private void FillBoardsDataGrid(IEnumerable<DeviceSp> dev)
        {
            foreach (var d in dev)
            {
                object[] row =
                {
                    d.Id.ToString(),
                    d.Address,
                    d.Name,
                    d.Description,
                    $"Порт {d.SpExhBehavior.NumberSp} : {(d.SpExhBehavior.IsOpenSp ? "Открыт" : "Закрыт")}",
                    d.SpExhBehavior.IsConnect ? Resources.OkImg : Resources.CancelImg,
                    Resources.ping_NO,
                    d.SpExhBehavior.LastSendData == null ? String.Empty : d.SpExhBehavior.LastSendData.Message,
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
