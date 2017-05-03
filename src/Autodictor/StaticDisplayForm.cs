using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        private readonly ICollection<IBinding2StaticFormBehavior> _binding2StaticFormBehaviors;
        private int _currentSelectIndex = -1;




        #region prop

        public Dictionary<byte, List<string[]>> Tables { get; set; } = new Dictionary<byte, List<string[]>>();

        #endregion






        #region ctor

        private StaticDisplayForm()
        {
            if (MyStaticDisplayForm != null)
                return;
            MyStaticDisplayForm = this;
        
            InitializeComponent();

            dgv_main.CellClick += dataGridView1_CellClick;
        }


        public StaticDisplayForm(ICollection<IBinding2StaticFormBehavior> binding2StaticFormBehaviors) : this()
        {
            this._binding2StaticFormBehaviors = binding2StaticFormBehaviors;
        }


        #endregion






        protected override void OnLoad(EventArgs e)
        {
          
            if (_binding2StaticFormBehaviors != null)
            {
                //загрузка спсика устройств со статической привязкой--------------------------------------------
                foreach (var binding2StaticFormBehavior in _binding2StaticFormBehaviors)
                {
                    string[] row =
                    {
                        binding2StaticFormBehavior.GetDeviceId.ToString(),
                        binding2StaticFormBehavior.GetDeviceName
                    };
                    var listViewItem = new ListViewItem(row);
                    lv_select.Items.Add(listViewItem);
                }


                //инициализация таблиц.
                for (byte i = 0; i < _binding2StaticFormBehaviors.Count; i++)
                {
                    List<string[]> table = new List<string[]>();

                    string[] array;
                    if (File.Exists(""))     //DEBUG иммитация загруженных данных
                    {
                        array = new[] { (100 + i).ToString(), ("NAme" + i) };
                        table.Add(array);
                    }
                    else
                    {
                        array = new string[dgv_main.ColumnCount];
                        table.Add(array);
                    }
                    Tables[i] = table;
                }
            }
        }




        /// <summary>
        /// Удаление строки
        /// </summary>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dgv_main.RowCount; i++)
            {
                dgv_main.Rows[i].HeaderCell.Value = (i+1).ToString();
            }
        }



        /// <summary>
        /// Выбор таблицы для ус-ва
        /// </summary>
        private void lv_select_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lv_select.SelectedItems.Count == 0)
                return;

            var selectIndex = lv_select.SelectedIndices[0];
            if (!Tables.ContainsKey((byte)selectIndex))
                return;

            //сохраним данные текущей таблице------------------------
            if (_currentSelectIndex >= 0)
            {
                var currentTable1 = Tables[(byte)_currentSelectIndex];
                currentTable1.Clear();
                for (int i = 0; i < dgv_main.Rows.Count - 1; i++)
                {
                    List<string> rowVal = new List<string>();
                    for (int j = 0; j < dgv_main.Columns.Count; j++)
                    {
                        rowVal.Add(dgv_main[j, i].FormattedValue as string);
                    }

                    currentTable1.Add(rowVal.ToArray());
                }
            }
            _currentSelectIndex = selectIndex;

            //отобразим новую таблицу-----------------------------------
            dgv_main.Rows.Clear();
            var currentTable = Tables[(byte)selectIndex];
            foreach (object[] row in currentTable)
            {
                dgv_main.Rows.Add(row);
            }
        }




        private void btn_Show_Click(object sender, EventArgs e)
        {
            if (_binding2StaticFormBehaviors == null || !_binding2StaticFormBehaviors.Any())
                return;

            if(_currentSelectIndex < 0)
                return;

            var resultUit= new UniversalInputType {TableData = new List<UniversalInputType>()};

            //формирование таблицу отправки------------------------------
            var currentTable = Tables[(byte)_currentSelectIndex];
            foreach (var row in currentTable)
            {
                var uit = new UniversalInputType();

                var numberOfTrain = row[0]?.Trim();
                var numberOfPath = row[1]?.Trim();
                var stations = row[2]?.Trim();
                var time = row[3]?.Trim();
                var note = row[4]?.Trim();

                uit.NumberOfTrain = numberOfTrain;
                uit.PathNumber = numberOfPath;
                uit.Stations = stations;
                uit.Note = note;
                DateTime outTimeVal;
                if (!DateTime.TryParse(time, out outTimeVal))
                    continue;
                uit.Time = outTimeVal;

                uit.Message = $"ПОЕЗД:{uit.NumberOfTrain}, ПУТЬ:{uit.PathNumber}, СОБЫТИЕ:{uit.Event}, СТАНЦИИ:{uit.Stations}, ВРЕМЯ:{uit.Time.ToShortTimeString()}";
                resultUit.TableData.Add(uit);
            }


            //отправляем таблицу все ус-вам.-----------------------------
            var currentbehavior = _binding2StaticFormBehaviors.ElementAt(_currentSelectIndex);
            currentbehavior.SendMessage(resultUit);     
        }







        protected override void OnClosing(CancelEventArgs e)
        {
            if (MyStaticDisplayForm == this)
                MyStaticDisplayForm = null;

            base.OnClosing(e);
        }

    }
}
