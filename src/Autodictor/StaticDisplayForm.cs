using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunicationDevices.Behavior.BindingBehavior.ToStatic;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;


namespace MainExample
{
    public partial class StaticDisplayForm : Form
    {
        public static StaticDisplayForm MyStaticDisplayForm = null;

        private ICollection<IBinding2StaticFormBehavior> _binding2StaticFormBehaviors;







        #region ctor

        private StaticDisplayForm()
        {
            if (MyStaticDisplayForm != null)
                return;
            MyStaticDisplayForm = this;
        
            InitializeComponent();

            dataGridView1.CellClick += dataGridView1_CellClick;
        }


        public StaticDisplayForm(ICollection<IBinding2StaticFormBehavior> binding2StaticFormBehaviors) : this()
        {
            this._binding2StaticFormBehaviors = binding2StaticFormBehaviors;
        }


        #endregion






        protected override void OnLoad(EventArgs e)
        {
            //загрузка названий устройств со статической привязкой
            if (_binding2StaticFormBehaviors != null)
            {
                foreach (var binding2StaticFormBehavior in _binding2StaticFormBehaviors)
                {
                    string[] row =
                    {
                        binding2StaticFormBehavior.GetDeviceId.ToString(),
                        binding2StaticFormBehavior.GetDeviceName
                    };
                    var listViewItem = new ListViewItem(row);
                    lv_Devices.Items.Add(listViewItem);
                }
            }

            //TODO: загружать из файла сохраненные строки.
        }




        /// <summary>
        /// Удаление строки
        /// </summary>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //var dataGridViewColumn = dataGridView1.Columns["Action"];
            //if (dataGridViewColumn != null && e.ColumnIndex == dataGridViewColumn.Index && e.RowIndex >= 0)
            //{
            //    dataGridView1.Rows.RemoveAt(e.RowIndex);
            //}

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = (i+1).ToString();
            }
        }



        private void btn_Show_Click(object sender, EventArgs e)
        {
            if (_binding2StaticFormBehaviors == null || !_binding2StaticFormBehaviors.Any())
                return;


            var resultUit= new UniversalInputType {TableData = new List<UniversalInputType>()};

            //формирование таблицу отправки
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                var uit= new UniversalInputType();

                var numberOfTrain = dataGridView1.Rows[i].Cells["cl_NumbOfTrain"]?.EditedFormattedValue.ToString().Trim();
                var numberOfPath = dataGridView1.Rows[i].Cells["cl_numberOfPath"]?.EditedFormattedValue.ToString().Trim();
                var stations = dataGridView1.Rows[i].Cells["cl_Stations"]?.EditedFormattedValue.ToString().Trim();
                var time = dataGridView1.Rows[i].Cells["cl_time"]?.EditedFormattedValue.ToString().Trim();
                var note = dataGridView1.Rows[i].Cells["cl_note"]?.EditedFormattedValue.ToString().Trim();

                uit.NumberOfTrain = numberOfTrain;
                uit.PathNumber = numberOfPath;
                uit.Stations = stations;
                uit.Note = note;
                DateTime outTimeVal;               
                if(!DateTime.TryParse(time, out outTimeVal))
                    continue;
                uit.Time = outTimeVal;

                uit.Message =  $"ПОЕЗД:{uit.NumberOfTrain}, ПУТЬ:{uit.PathNumber}, СОБЫТИЕ:{uit.Event}, СТАНЦИИ:{uit.Stations}, ВРЕМЯ:{uit.Time.ToShortTimeString()}";
                resultUit.TableData.Add(uit);
            }

            //отправляем таблицу все ус-вам.
            foreach (var binding2StaticFormBehavior in _binding2StaticFormBehaviors)
            {
                binding2StaticFormBehavior.SendMessage(resultUit);
            }
        }



        protected override void OnClosing(CancelEventArgs e)
        {
            if (MyStaticDisplayForm == this)
                MyStaticDisplayForm = null;

            base.OnClosing(e);
        }


    }
}
