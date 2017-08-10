using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MainExample
{
    public partial class TrainTableGrid : Form
    {
        public static TrainTableGrid MyMainForm = null;
        private readonly List<CheckBox> _checkBoxes;


        //public List<TableRec> TableRecs { get; set; } = new List<TableRec>
        //{
        //    new TableRec { Id = 1, Number = "558", ArrivalTime = "00:07", DepartureTime = "00:30", Route = "Череповец-Адлер", DaysFollowing = "Выборочные дни: Май:26"},
        //    new TableRec { Id = 2, Number = "516", ArrivalTime = "00:17", DepartureTime = "01:30", Route = "Сыктывкар-Адлер", DaysFollowing = "Выборочные дни: Движение отсутсвует"},
        //    new TableRec { Id = 3, Number = "698", ArrivalTime = "13:07", DepartureTime = "14:30", Route = "Кострома-Москва", DaysFollowing = "Выборочные дни: Ежедневно"},
        //    new TableRec { Id = 4, Number = "496", ArrivalTime = "14:07", DepartureTime = "15:30", Route = "Архангельск-Адлер", DaysFollowing = "Выборочные дни: Ежедневно"},
        //    new TableRec { Id = 5, Number = "386", ArrivalTime = "15:07", DepartureTime = "17:55", Route = "Череповец-Адлер", DaysFollowing = "Выборочные дни: Движение отсутсвует"},
        //    new TableRec { Id = 4, Number = "666", ArrivalTime = "14:07", DepartureTime = "15:30", Route = "Архангельск-Москва", DaysFollowing = "Выборочные дни: Ежедневно"},
        //};

        public DataTable DataTable { get; set; }
        public DataView DataView { get; set; }




        public TrainTableGrid()
        {
            if (MyMainForm != null)
                return;

            MyMainForm = this;

            InitializeComponent();

            _checkBoxes = new List<CheckBox> { chb_Id, chb_Номер, chb_ВремяПрибытия, chb_ВремяОтпр, chb_Маршрут, chb_ДниСледования };
            Model2Controls();
        }



       private void CreateDataTable()
        {
            //Создание  таблицы
            DataTable = new DataTable("MAIN_TABLE");
            List<DataColumn> columns = new List<DataColumn>
            {
                new DataColumn("Id", typeof(int)),
                new DataColumn("Номер", typeof(string)),
                new DataColumn("ВремяПрибытия", typeof(string)),
                new DataColumn("ВремяОтправления", typeof(string)),
                new DataColumn("Маршрут", typeof(string)),
                new DataColumn("ДниСледования", typeof(string))
            };
            DataTable.Columns.AddRange(columns.ToArray());

            DataView = new DataView(DataTable);
            dgv_TrainTable.DataSource = DataView;


            //форматирование DataGridView----------------------------
            for (int i = 0; i < dgv_TrainTable.Columns.Count; i++)
            {
                var col = dgv_TrainTable.Columns[i];
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                switch (col.Name)
                {
                    case "Id":
                        col.HeaderText = @"Id";
                        break;

                    case "Номер":
                        col.HeaderText = @"Номер";
                        break;

                    case "ВремяПрибытия":
                        col.HeaderText = @"Время прибытия";
                        break;

                    case "ВремяОтправления":
                        col.HeaderText = @"Время отправления";
                        break;

                    case "Маршрут":
                        col.HeaderText = @"Маршрут";
                        break;

                    case "ДниСледования":
                        col.HeaderText = @"Дни следования";
                        break;
                }
            }
        }



        private void Model2Controls()
        {
            CreateDataTable();

            LoadSettings();

            //Заполнение ChBox---------------------------------------
            for (var i = 0; i < dgv_TrainTable.Columns.Count; i++)
            {
                var chBox = _checkBoxes.FirstOrDefault(ch => (string)ch.Tag == dgv_TrainTable.Columns[i].Name);
                if (chBox != null)
                {
                    chBox.Checked = dgv_TrainTable.Columns[i].Visible;
                }
            }

            //Заполнение таблицы данными-------------------
            //foreach (var rec in TableRecs)
            //{
            //    var row = DataTable.NewRow();
            //    row["Id"] = rec.Id;
            //    row["Номер"] = rec.Number;
            //    row["ВремяПрибытия"] = rec.ArrivalTime;
            //    row["ВремяОтправления"] = rec.DepartureTime;
            //    row["Маршрут"] = rec.Route;
            //    row["ДниСледования"] = rec.DaysFollowing;

            //    DataTable.Rows.Add(row);
            //}
        }



        /// <summary>
        /// Фильтрация таблицы
        /// </summary>
        private void btn_Filter_Click(object sender, EventArgs e)
        {
            string filter= String.Empty;

            if (!(string.IsNullOrEmpty(tb_НомерПоезда.Text) || string.IsNullOrWhiteSpace(tb_НомерПоезда.Text)))
            {
                filter = $"Номер = '{tb_НомерПоезда.Text}'";
            }

            if (!(string.IsNullOrEmpty(tb_ВремяПриб.Text) || string.IsNullOrWhiteSpace(tb_ВремяПриб.Text)))
            {
                if (string.IsNullOrEmpty(filter))
                {
                    filter = $"ВремяПрибытия  = '{tb_ВремяПриб.Text}'";
                }
                else
                {
                    filter += $" and ВремяПрибытия  = '{tb_ВремяПриб.Text}'";
                }
            }

            if (!(string.IsNullOrEmpty(tb_ВремяОтпр.Text) || string.IsNullOrWhiteSpace(tb_ВремяОтпр.Text)))
            {
                if (string.IsNullOrEmpty(filter))
                {
                    filter = $"ВремяОтправления  = '{tb_ВремяОтпр.Text}'";
                }
                else
                {
                    filter += $" and ВремяОтправления  = '{tb_ВремяОтпр.Text}'";
                }
            }

            if (!(string.IsNullOrEmpty(tb_ДниСлед.Text) || string.IsNullOrWhiteSpace(tb_ДниСлед.Text)))
            {
                if (string.IsNullOrEmpty(filter))
                {
                    filter = $"ДниСледования  = '{tb_ДниСлед.Text}'";
                }
                else
                {
                    filter += $" and ДниСледования  = '{tb_ДниСлед.Text}'";
                }
            }

            DataView.RowFilter = filter;
        }



        /// <summary>
        /// Вкл/Выкл колонок
        /// </summary>
        private void chb_CheckedChanged(object sender, EventArgs e)
        {
            var chb = sender as CheckBox;
            if (chb != null)
            {
                for (var i = 0; i < dgv_TrainTable.Columns.Count; i++)
                {
                    if (dgv_TrainTable.Columns[i].Name == (string)chb.Tag)
                    {
                        dgv_TrainTable.Columns[i].Visible = chb.Checked;
                        return;
                    }
                }
            }
        }




        /// <summary>
        /// Сохранить форматирование грида в файл.
        /// </summary>
        private void SaveSettings()
        {
            try
            {
                using (StreamWriter dumpFile = new StreamWriter("GridTableRec.ini"))
                {
                    for (var i = 0; i < dgv_TrainTable.Columns.Count; i++)
                    {
                        string line = dgv_TrainTable.Columns[i].Name + ";" +
                                      dgv_TrainTable.Columns[i].Visible + ";" +
                                      dgv_TrainTable.Columns[i].DisplayIndex;

                        dumpFile.WriteLine(line);
                    }

                    dumpFile.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Ошибка сохранения настроек в файл: ""{ex.Message}""");
            }
        }



        private void LoadSettings()
        {
            try
            {
                using (StreamReader file = new StreamReader("GridTableRec.ini"))
                {
                    string line;
                    int numberLine = 0;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] settings = line.Split(';');
                        if (settings.Length == 3)
                        {
                            if (dgv_TrainTable.Columns[numberLine].Name == settings[0])
                            {
                                dgv_TrainTable.Columns[numberLine].Visible = bool.Parse(settings[1]);
                                dgv_TrainTable.Columns[numberLine].DisplayIndex = int.Parse(settings[2]);
                            }

                            if (numberLine++ >= dgv_TrainTable.ColumnCount)
                                return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Ошибка загрузки настроек из файла: ""{ex.Message}""");
            }
        }



        private void btn_SaveTableFormat_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }




        /// <summary>
        /// Обравботчик события перемешения колонки. Первую колонку нельзя отключать.
        /// </summary>
        private void dgv_TrainTable_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            string col0 = string.Empty;
            for (var i = 0; i < dgv_TrainTable.Columns.Count; i++)
            {
                var col = dgv_TrainTable.Columns[i];
                if (col.DisplayIndex == 0)
                    col0 = col.Name;
            }

            foreach (var chBox in _checkBoxes)
            {
                chBox.Enabled = (string)chBox.Tag != col0;
            }
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            if (MyMainForm == this)
                MyMainForm = null;

            //DispouseCisClientIsConnectRx.Dispose();
            base.OnClosing(e);
        }

    }
}
