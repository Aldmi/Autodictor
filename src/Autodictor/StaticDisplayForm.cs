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
        private readonly IList<IBinding2StaticFormBehavior> _binding2StaticFormBehaviors;
        private int _currentSelectIndex = -1;
        private bool _currentTableChanged;




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
            this._binding2StaticFormBehaviors = binding2StaticFormBehaviors.ToList();
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


                //инициализация таблиц.---------------------------------------------------------------------------
                for (byte i = 0; i < _binding2StaticFormBehaviors.Count; i++)
                {
                    Tables[i] = LoadTableFromFile(GetIndividualFileName(i));
                }
            }
        }


        private string GetIndividualFileName(int bindingId)
        {
            if (bindingId < 0 || bindingId >= _binding2StaticFormBehaviors.Count)
                return null;

            var fileName = _binding2StaticFormBehaviors[bindingId].GetDeviceName + "_" + _binding2StaticFormBehaviors[bindingId].GetDeviceId;
            return fileName + @".info";
        }



        private List<string[]> LoadTableFromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;

            var table = new List<string[]>();
            string[] array;

            string path = Application.StartupPath + @"\StaticTableDisplay" + @"\" + fileName;
            if (File.Exists(path))
            {
                try
                {
                    using (StreamReader file = new StreamReader(path))
                    {
                        string line;
                        while ((line = file.ReadLine()) != null)
                        {
                            array = line.Split(';');
                            table.Add(array);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка чтения файла {path}  ОШИБКА: {ex.Message}");
                }
            }
            else
            {
                array = new string[dgv_main.ColumnCount];
                table.Add(array);
            }

            return table;
        }



        private void SaveTableToFile(List<string[]> table, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            string path = Application.StartupPath + @"\StaticTableDisplay" + @"\" + fileName;
            try
            {
                using (StreamWriter dumpFile = new StreamWriter(path))            //если файла нет, он будет создан
                {
                    foreach (string[] row in table)
                    {
                        StringBuilder line= new StringBuilder();
                        for (int j = 0; j < row.Length; j++)
                        {
                            var spliter = (j < row.Length - 1) ? ";" : string.Empty;
                            line.Append(row[j] + spliter);
                        }
                        dumpFile.WriteLine(line.ToString());
                    }

                    dumpFile.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка записи файла {path}  ОШИБКА: {ex.Message}");
            }
        }



        /// <summary>
        /// Удаление строки
        /// </summary>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            for (int i = 0; i < dgv_main.RowCount; i++)
            {
                dgv_main.Rows[i].HeaderCell.Value = (i + 1).ToString();
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

                //сохранение на диск-----------------------------------
                if (_currentTableChanged)
                {
                    SaveTableToFile(currentTable1, GetIndividualFileName(_currentSelectIndex));
                    _currentTableChanged = false;
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

            if (_currentSelectIndex < 0)
                return;

            var resultUit = new UniversalInputType { TableData = new List<UniversalInputType>() };

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



        private void dgv_main_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_currentSelectIndex >= 0)
            {
                _currentTableChanged = true;
            }
        }


        private void dgv_main_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            dgv_main_CellEndEdit(null, null);
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            if (_currentTableChanged)
            {
                lv_select_SelectedIndexChanged(null, EventArgs.Empty);
                _currentTableChanged = false;
            }

            if (MyStaticDisplayForm == this)
                MyStaticDisplayForm = null;

            base.OnClosing(e);
        }



    }
}
