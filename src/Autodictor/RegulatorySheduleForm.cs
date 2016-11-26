using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunicationDevices.ClientWCF;
using MainExample.Extension;
using WCFCis2AvtodictorContract.DataContract;


namespace MainExample
{
    public partial class RegulatorySheduleForm : Form
    {
        static public RegulatorySheduleForm MyRegulatorySheduleForm = null;

        public CisClient CisClient { get; set; }
        public IDisposable DispouseChangeRegulatorySheduleRx { get; set; }





        public RegulatorySheduleForm(CisClient cisClient)
        {
            if (MyRegulatorySheduleForm != null)
                return;
            MyRegulatorySheduleForm = this;

            InitializeComponent();

            CisClient = cisClient;
            if (CisClient.RegulatoryScheduleDatas != null && CisClient.RegulatoryScheduleDatas.Any())
                FillListView(CisClient.RegulatoryScheduleDatas);

            DispouseChangeRegulatorySheduleRx = CisClient.RegulatoryScheduleDatasChange.Subscribe(op =>
            {
                ClearListView();
                FillListView(op);
            });
        }


        private void ClearListView()
        {
            this.InvokeIfNeeded(() => listRegSh.Items.Clear());
        }


        private void FillListView(IEnumerable<RegulatoryScheduleData> op)
        {
            var row = op.Select(str => new[]
            {
                str.NumberOfTrain.ToString(),
                str.RouteName.ToString(),
                str.DepartureTime.ToLongTimeString(),
                str.ArrivalTime.ToLongTimeString(),
                str.DispatchStation.Name,
                str.StationOfDestination.Name,
                str.DaysFollowing
            }).Select(s => new ListViewItem(s)).ToArray();

            this.InvokeIfNeeded(() => listRegSh.Items.AddRange(row));
        }





        protected override void OnClosed(EventArgs e)
        {
            if (MyRegulatorySheduleForm == this)
                MyRegulatorySheduleForm = null;

            DispouseChangeRegulatorySheduleRx.Dispose();
            base.OnClosed(e);
        }
    }
}
