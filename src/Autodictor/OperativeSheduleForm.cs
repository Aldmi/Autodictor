using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MainExample.ClientWCF;
using MainExample.Extencions;


namespace MainExample
{
    public partial class OperativeSheduleForm : Form
    {
        static public OperativeSheduleForm MyOperativeSheduleForm = null;

        public CisClient CisClient { get; set; }
        public IDisposable DispouseChangeOperativeSheduleRx { get; set; }





        public OperativeSheduleForm(CisClient cisClient)
        {
            if (MyOperativeSheduleForm != null)
                return;
            MyOperativeSheduleForm = this;

            InitializeComponent();

            CisClient = cisClient;
            DispouseChangeOperativeSheduleRx = CisClient.OperativeScheduleDatasChange.Subscribe(op =>
            {
                //очистить ListView
                //заполнить значениями
                FillListView();
            });
        }



        private void FillListView()
        {
            string[] arr = new string[4];
            ListViewItem itm;

            //Add first item
            arr[0] = "product_1";
            arr[1] = "100";
            arr[2] = "10";
            itm = new ListViewItem(arr);
            this.InvokeIfNeeded(() => listOperSh.Items.Add(itm));
        }



        protected override void OnClosed(EventArgs e)
        {
            DispouseChangeOperativeSheduleRx.Dispose();
            base.OnClosed(e);
        }
    }
}
