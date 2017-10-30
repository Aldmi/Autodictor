using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Domain.Entitys;

namespace MainExample
{
    public enum WeekDays { Постоянно, Пн, Вт, Ср, Чт, Пт, Сб, Вс }
    public enum SourceData { Local, RemoteCis }


    public struct TrainTableRecord
    {
        public int ID;                    //Id
        public string Num;                //Номер поезда
        public string Num2;               //Номер поезда 2 для транзита
        public string Name;               //Название поезда
        public string Direction;          //направление
        public string StationDepart;      //станция отправления
        public string StationArrival;     //станция прибытия
        public string ArrivalTime;        //прибытие
        public string StopTime;           //стоянка
        public string DepartureTime;      //отправление
        public string FollowingTime;      //время следования (время в пути)
        public string Days;               //дни следования
        public string DaysAlias;          //дни следования (строка заполняется в ручную)
        public bool Active;               //активность, отмека галочкой
        public string SoundTemplates;     //
        public byte TrainPathDirection;
        public bool ChangeTrainPathDirection;      //смена направления (для трназитов)
        public Dictionary<WeekDays, string> TrainPathNumber;      //Пути по дням недели или постоянно
        public bool PathWeekDayes;                                //true- установленны пути по дням недели, false - путь установленн постоянно
        public ТипПоезда ТипПоезда;
        public string Примечание;
        public DateTime ВремяНачалаДействияРасписания;
        public DateTime ВремяОкончанияДействияРасписания;
        public string Addition;                                   //Дополнение
        public Dictionary<string, bool> ИспользоватьДополнение;   //[звук] - использовать дополнение для звука.  [табло] - использовать дополнение для табло.
        public bool Автомат;                                      // true - поезд обрабатывается в автомате.
        public bool ОграничениеОтправки;                          // true - поезд обрабатывается в автомате.
    };




    public partial class TrainTableGrid : Form
    {
        #region Field

        private static object _lockObj= new object();

        private const string PathGridSetting = "UISettings/GridTableRec.ini";
        private const string FileNameLocalTableRec = @"TableRecords.ini";
        private const string FileNameRemoteCisTableRec = @"TableRecordsRemoteCis.ini";

        public static TrainTableGrid MyMainForm = null;
        private readonly List<CheckBox> _checkBoxes;

        public static List<TrainTableRecord> TrainTableRecords = new List<TrainTableRecord>();
        private static SourceData _sourceLoad = SourceData.Local;

        #endregion





        #region prop

        public DataTable DataTable { get; set; }
        public DataView DataView { get; set; }


        #endregion





        #region ctor

        public TrainTableGrid()
        {
            if (MyMainForm != null)
                return;
            MyMainForm = this;

            InitializeComponent();

            _checkBoxes = new List<CheckBox> { chb_Id, chb_Номер, chb_ВремяПрибытия, chb_Стоянка, chb_ВремяОтпр, chb_Маршрут, chb_ДниСледования };
            Model2Controls();

            rbSourseSheduleCis.Checked = (_sourceLoad == SourceData.RemoteCis);
        }

        #endregion






        #region Methods

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
                new DataColumn("Стоянка", typeof(string)),
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
                switch (col.Name)
                {
                    case "Id":
                        col.HeaderText = @"Id";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        break;

                    case "Номер":
                        col.HeaderText = @"Номер";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        break;

                    case "ВремяПрибытия":
                        col.HeaderText = @"Время прибытия";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        break;

                    case "Стоянка":
                        col.HeaderText = @"Стоянка";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        break;

                    case "ВремяОтправления":
                        col.HeaderText = @"Время отправления";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        break;

                    case "Маршрут":
                        col.HeaderText = @"Маршрут";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        break;

                    case "ДниСледования":
                        col.HeaderText = @"Дни следования";
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        break;
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
                using (StreamWriter dumpFile = new StreamWriter(PathGridSetting))
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


        /// <summary>
        /// Загрузить форматирование грида из файла.
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                using (StreamReader file = new StreamReader(PathGridSetting))
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



        public static void ЗагрузитьСписок(string fileName)
        {
            lock (_lockObj)
            {
                TrainTableRecords.Clear();

                try
                {
                    using (StreamReader file = new StreamReader(fileName))
                    {
                        string line;
                        while ((line = file.ReadLine()) != null)
                        {
                            string[] settings = line.Split(';');
                            if ((settings.Length == 13) || (settings.Length == 15) || (settings.Length >= 16))
                            {
                                TrainTableRecord данные;

                                данные.ID = int.Parse(settings[0]);
                                данные.Num = settings[1];
                                данные.Name = settings[2];
                                данные.ArrivalTime = settings[3];
                                данные.StopTime = settings[4];
                                данные.DepartureTime = settings[5];
                                данные.Days = settings[6];
                                данные.Active = settings[7] == "1" ? true : false;
                                данные.SoundTemplates = settings[8];
                                данные.TrainPathDirection = byte.Parse(settings[9]);
                                данные.TrainPathNumber = LoadPathFromFile(settings[10], out данные.PathWeekDayes);
                                данные.ИспользоватьДополнение = new Dictionary<string, bool>()
                                {
                                    ["звук"] = false,
                                    ["табло"] = false
                                };
                               

                                ТипПоезда типПоезда = ТипПоезда.НеОпределен;
                                try
                                {
                                    типПоезда = (ТипПоезда)Enum.Parse(typeof(ТипПоезда), settings[11]);
                                }
                                catch (ArgumentException) { }
                                данные.ТипПоезда = типПоезда;

                                данные.Примечание = settings[12];

                                if (данные.TrainPathDirection > 2)
                                    данные.TrainPathDirection = 0;

                                var path = Program.PathWaysRepository.List().FirstOrDefault(p => p.Name == данные.TrainPathNumber[WeekDays.Постоянно]);
                                if (path == null)
                                    данные.TrainPathNumber[WeekDays.Постоянно] = "";

                                DateTime началоДействия = new DateTime(1900, 1, 1);
                                DateTime конецДействия = new DateTime(2100, 1, 1);
                                if (settings.Length >= 15)
                                {
                                    DateTime.TryParse(settings[13], out началоДействия);
                                    DateTime.TryParse(settings[14], out конецДействия);
                                }
                                данные.ВремяНачалаДействияРасписания = началоДействия;
                                данные.ВремяОкончанияДействияРасписания = конецДействия;


                                var addition = "";
                                if (settings.Length >= 16)
                                {
                                    addition = settings[15];
                                }
                                данные.Addition = addition;


                                if (settings.Length >= 18)
                                {
                                    данные.ИспользоватьДополнение["табло"] = settings[16] == "1";
                                    данные.ИспользоватьДополнение["звук"] = settings[17] == "1";
                                }

                                данные.Автомат = true;
                                if (settings.Length >= 19)
                                {
                                    данные.Автомат = (string.IsNullOrEmpty(settings[18]) || settings[18] == "1"); // по умолчанию true
                                }


                                данные.Num2 = String.Empty;
                                данные.FollowingTime = String.Empty;
                                данные.DaysAlias = String.Empty;
                                if (settings.Length >= 22)
                                {
                                    данные.Num2 = settings[19];
                                    данные.FollowingTime = settings[20];
                                    данные.DaysAlias = settings[21];
                                }


                                данные.StationDepart = String.Empty;
                                данные.StationArrival = String.Empty;
                                if (settings.Length >= 23)
                                {
                                    данные.StationDepart = settings[22];
                                    данные.StationArrival = settings[23];
                                }

                                данные.Direction = String.Empty;
                                if (settings.Length >= 25)
                                {
                                    данные.Direction = settings[24];
                                }

                                данные.ChangeTrainPathDirection = false;
                                if (settings.Length >= 26)
                                {
                                    bool changeDirection;
                                    bool.TryParse(settings[25], out changeDirection);
                                    данные.ChangeTrainPathDirection = changeDirection;
                                }

                                данные.ОграничениеОтправки = false;
                                if (settings.Length >= 27)
                                {
                                    bool ограничениеОтправки;
                                    bool.TryParse(settings[26], out ограничениеОтправки);
                                    данные.ОграничениеОтправки = ограничениеОтправки;
                                }


                                TrainTableRecords.Add(данные);
                                Program.НомераПоездов.Add(данные.Num);
                                if (!string.IsNullOrEmpty(данные.Num2))
                                    Program.НомераПоездов.Add(данные.Num2);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }



        public static void СохранитьСписокРегулярноеРасписаниеЦис(IList<TrainTableRecord> trainTableRecords)
        {
            СохранитьСписок(trainTableRecords, FileNameRemoteCisTableRec);
        }



        /// <summary>
        /// Сохранить список в файл
        /// </summary>
        private static void СохранитьСписок(IList<TrainTableRecord> trainTableRecords, string fileName)
        {
            try
            {
                lock (_lockObj)
                {
                    using (StreamWriter dumpFile = new StreamWriter(fileName))
                    {
                        for (int i = 0; i < trainTableRecords.Count; i++)
                        {
                            string line = trainTableRecords[i].ID + ";" +
                                          trainTableRecords[i].Num + ";" +
                                          trainTableRecords[i].Name + ";" +
                                          trainTableRecords[i].ArrivalTime + ";" +
                                          trainTableRecords[i].StopTime + ";" +
                                          trainTableRecords[i].DepartureTime + ";" +
                                          trainTableRecords[i].Days + ";" +
                                          (trainTableRecords[i].Active ? "1" : "0") + ";" +
                                          trainTableRecords[i].SoundTemplates + ";" +
                                          trainTableRecords[i].TrainPathDirection.ToString() + ";" +
                                          SavePath2File(trainTableRecords[i].TrainPathNumber,
                                              trainTableRecords[i].PathWeekDayes) + ";" +
                                          trainTableRecords[i].ТипПоезда.ToString() + ";" +
                                          trainTableRecords[i].Примечание + ";" +
                                          trainTableRecords[i].ВремяНачалаДействияРасписания.ToString("dd.MM.yyyy HH:mm:ss") + ";" +
                                          trainTableRecords[i].ВремяОкончанияДействияРасписания.ToString("dd.MM.yyyy HH:mm:ss") + ";" +
                                          trainTableRecords[i].Addition + ";" +
                                          (trainTableRecords[i].ИспользоватьДополнение["табло"] ? "1" : "0") + ";" +
                                          (trainTableRecords[i].ИспользоватьДополнение["звук"] ? "1" : "0") + ";" +
                                          (trainTableRecords[i].Автомат ? "1" : "0") + ";" +

                                          trainTableRecords[i].Num2 + ";" +
                                          trainTableRecords[i].FollowingTime + ";" +
                                          trainTableRecords[i].DaysAlias + ";" +

                                          trainTableRecords[i].StationDepart + ";" +
                                          trainTableRecords[i].StationArrival + ";" +
                                          trainTableRecords[i].Direction + ";" +
                                          trainTableRecords[i].ChangeTrainPathDirection + ";"+
                                          trainTableRecords[i].ОграничениеОтправки;

                            dumpFile.WriteLine(line);
                        }

                        dumpFile.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }



        private void РаскраситьСписок()
        {
            for (var i = 0; i < dgv_TrainTable.Rows.Count; i++)
            {
                var row = dgv_TrainTable.Rows[i];
                var id = (int)row.Cells[0].Value;
                var firstOrDefault = TrainTableRecords.FirstOrDefault(t => t.ID == id);

                dgv_TrainTable.Rows[i].DefaultCellStyle.BackColor = firstOrDefault.Active ? Color.LightGreen : Color.LightGray;
                dgv_TrainTable.Rows[i].Tag = firstOrDefault.ID;
            }


            dgv_TrainTable.AllowUserToResizeColumns = true;
        }



        private static Dictionary<WeekDays, string> LoadPathFromFile(string str, out bool pathWeekDayes)
        {
            Dictionary<WeekDays, string> pathDictionary = new Dictionary<WeekDays, string>
            {
                [WeekDays.Постоянно] = String.Empty,
                [WeekDays.Пн] = String.Empty,
                [WeekDays.Вт] = String.Empty,
                [WeekDays.Ср] = String.Empty,
                [WeekDays.Ср] = String.Empty,
                [WeekDays.Чт] = String.Empty,
                [WeekDays.Пт] = String.Empty,
                [WeekDays.Сб] = String.Empty,
                [WeekDays.Вс] = String.Empty
            };
            pathWeekDayes = false;

            if (!string.IsNullOrEmpty(str) && str.Contains("|") && str.Contains(":"))
            {
                var pairs = str.Split('|');
                if (pairs.Length == 9)
                {
                    foreach (var pair in pairs)
                    {
                        var keyVal = pair.Split(':');

                        var value = (keyVal[1] == "Не определен") ? string.Empty : keyVal[1];
                        switch (keyVal[0])
                        {
                            case "Постоянно":
                                pathDictionary[WeekDays.Постоянно] = value;
                                break;

                            case "Пн":
                                pathDictionary[WeekDays.Пн] = value;
                                break;

                            case "Вт":
                                pathDictionary[WeekDays.Вт] = value;
                                break;

                            case "Ср":
                                pathDictionary[WeekDays.Ср] = value;
                                break;

                            case "Чт":
                                pathDictionary[WeekDays.Чт] = value;
                                break;

                            case "Пт":
                                pathDictionary[WeekDays.Пт] = value;
                                break;

                            case "Сб":
                                pathDictionary[WeekDays.Сб] = value;
                                break;

                            case "Вс":
                                pathDictionary[WeekDays.Вс] = value;
                                break;

                            case "ПутиПоДням":
                                pathWeekDayes = (keyVal[1] == "1");
                                break;
                        }
                    }
                }
            }

            return pathDictionary;
        }



        private static string SavePath2File(Dictionary<WeekDays, string> pathDictionary, bool pathWeekDayes)
        {
            StringBuilder strBuild = new StringBuilder();
            foreach (var keyVal in pathDictionary)
            {
                var value = (keyVal.Value == "Не определен") ? string.Empty : keyVal.Value;
                strBuild.Append(keyVal.Key).Append(":").Append(value).Append("|");
            }
            strBuild.Append("ПутиПоДням").Append(":").Append(pathWeekDayes ? "1" : "0");

            return strBuild.ToString();
        }



        /// <summary>
        /// Выбор источника загрузки
        /// </summary>
        public static void SourceLoadMainList()
        {
           if (Enum.TryParse(Program.Настройки.SourceTrainTableRecordLoad, out _sourceLoad))
              ЗагрузитьСписок(_sourceLoad == SourceData.Local ? FileNameLocalTableRec : FileNameRemoteCisTableRec);
        }



        private void ОбновитьДанныеВСписке()
        {        
            DataTable.Rows.Clear();
            for (var i = 0; i < TrainTableRecords.Count; i++)
            {
                var данные = TrainTableRecords[i];
                string строкаОписанияРасписания = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(данные.Days).ПолучитьСтрокуОписанияРасписания();

                var row = DataTable.NewRow();
                row["Id"] = данные.ID;
                row["Номер"] = данные.Num;
                row["ВремяПрибытия"] = данные.ArrivalTime;
                row["Стоянка"] = данные.StopTime;
                row["ВремяОтправления"] = данные.DepartureTime;
                row["Маршрут"] = данные.Name;
                row["ДниСледования"] = строкаОписанияРасписания;
                DataTable.Rows.Add(row);

                dgv_TrainTable.Rows[i].DefaultCellStyle.BackColor = данные.Active ? Color.LightGreen : Color.LightGray;
                dgv_TrainTable.Rows[i].Tag = данные.ID;
            }

           РаскраситьСписок();
        }



        /// <summary>
        /// Редактирование элемента
        /// </summary>
        /// <param name="index">Если указанн индекс то элемент уже есть в списке, если равен null, то это новый элемент добавленный в конец списка</param>
        private TrainTableRecord? EditData(TrainTableRecord данные, int? index = null)
        {
            ПланРасписанияПоезда текущийПланРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(данные.Days);
            текущийПланРасписанияПоезда.УстановитьНомерПоезда(данные.Num);
            текущийПланРасписанияПоезда.УстановитьНазваниеПоезда(данные.Name);

            Оповещение оповещение = new Оповещение(данные);
            оповещение.ShowDialog();
            данные.Active = !оповещение.cBБлокировка.Checked;
            if (оповещение.DialogResult == DialogResult.OK)
            {
                данные = оповещение.РасписаниеПоезда;
                var строкаОписанияРасписания = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(данные.Days).ПолучитьСтрокуОписанияРасписания();
                if (index != null)
                {
                    var row = DataTable.Rows[index.Value];
                    row["Номер"] = данные.Num;
                    row["ВремяПрибытия"] = данные.ArrivalTime;
                    row["Стоянка"] = данные.StopTime;
                    row["ВремяОтправления"] = данные.DepartureTime;
                    row["Маршрут"] = данные.Name;
                    row["ДниСледования"] = строкаОписанияРасписания;
                }
                else
                {
                    var row = DataTable.NewRow();
                    row["Id"] = данные.ID;
                    row["Номер"] = данные.Num;
                    row["ВремяПрибытия"] = данные.ArrivalTime;
                    row["Стоянка"] = данные.StopTime;
                    row["ВремяОтправления"] = данные.DepartureTime;
                    row["Маршрут"] = данные.Name;
                    row["ДниСледования"] = строкаОписанияРасписания;
                    DataTable.Rows.Add(row);

                    dgv_TrainTable.Rows[dgv_TrainTable.Rows.Count - 1].DefaultCellStyle.BackColor = данные.Active ? Color.LightGreen : Color.LightGray;
                    dgv_TrainTable.Rows[dgv_TrainTable.Rows.Count - 1].Tag = данные.ID;
                }
                return данные;
            }

            return null;
        }

        #endregion





        #region EventHandler

        protected override void OnLoad(EventArgs e)
        {
            //Заполнение таблицы данными-------------------
            btnLoad_Click(null, EventArgs.Empty);
        }



        /// <summary>
        /// Фильтрация таблицы
        /// </summary>
        private void btn_Filter_Click(object sender, EventArgs e)
        {
            string filter = String.Empty;

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

            РаскраситьСписок();
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
        /// Сохранение форатирования таблицы
        /// </summary>
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



        /// <summary>
        /// Загрузить расписание
        /// </summary>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            SourceLoadMainList();
            ОбновитьДанныеВСписке();
        }



        /// <summary>
        /// Добавить
        /// </summary>
        private void dgv_TrainTable_DoubleClick(object sender, EventArgs e)
        {
            var selected = dgv_TrainTable.SelectedRows[0];
            if (selected == null)
                return;

            for (int i = 0; i < TrainTableRecords.Count; i++)
            {
                if (TrainTableRecords[i].ID == (int)selected.Tag)
                {
                    var данные = EditData(TrainTableRecords[i], i);
                    if (данные != null)
                    {
                        TrainTableRecords[i] = данные.Value;
                    }

                    break;
                }
            }
        }



        /// <summary>
        /// Удалить
        /// </summary>
        private void btn_УдалитьЗапись_Click(object sender, EventArgs e)
        {
            var selected = dgv_TrainTable.SelectedRows[0];
            if (selected == null)
                return;

            var delItem = TrainTableRecords.FirstOrDefault(t => t.ID == (int)selected.Tag);
            TrainTableRecords.Remove(delItem);
            ОбновитьДанныеВСписке();
        }



        /// <summary>
        /// Добавить
        /// </summary>
        private void btn_ДобавитьЗапись_Click(object sender, EventArgs e)
        {
            int maxId = TrainTableRecords.Max(t => t.ID);

            //создали новый элемент
            TrainTableRecord Данные;
            Данные.ID = ++maxId;
            Данные.Num = "";
            Данные.Num2 = "";
            Данные.Addition = "";
            Данные.Name = "";
            Данные.StationArrival = "";
            Данные.StationDepart = "";
            Данные.Direction = "";
            Данные.ArrivalTime = "00:00";
            Данные.StopTime = "00:00";
            Данные.DepartureTime = "00:00";
            Данные.FollowingTime = "00:00";
            Данные.Days = "";
            Данные.DaysAlias = "";
            Данные.Active = true;
            Данные.SoundTemplates = "";
            Данные.TrainPathDirection = 0x01;
            Данные.ChangeTrainPathDirection = false;
            Данные.ТипПоезда = ТипПоезда.НеОпределен;
            Данные.TrainPathNumber = new Dictionary<WeekDays, string>
            {
                [WeekDays.Постоянно] = String.Empty,
                [WeekDays.Пн] = String.Empty,
                [WeekDays.Вт] = String.Empty,
                [WeekDays.Ср] = String.Empty,
                [WeekDays.Ср] = String.Empty,
                [WeekDays.Чт] = String.Empty,
                [WeekDays.Пт] = String.Empty,
                [WeekDays.Сб] = String.Empty,
                [WeekDays.Вс] = String.Empty
            };
            Данные.PathWeekDayes = false;
            Данные.Примечание = "";
            Данные.ВремяНачалаДействияРасписания = new DateTime(1900, 1, 1);
            Данные.ВремяОкончанияДействияРасписания = new DateTime(2100, 1, 1);
            Данные.Addition = "";
            Данные.ИспользоватьДополнение = new Dictionary<string, bool>
            {
                ["звук"] = false,
                ["табло"] = false
            };
            Данные.Автомат = true;

            Данные.ОграничениеОтправки = false;

            //Добавили в список
            TrainTableRecords.Add(Данные);

            //Отредактировали добавленный элемент
            int lastIndex = TrainTableRecords.Count - 1;
            var данные = EditData(TrainTableRecords[lastIndex]);
            if (данные != null)
            {
                TrainTableRecords[lastIndex] = данные.Value;
            }
        }



        /// <summary>
        /// Сохранить
        /// </summary>
        private void btn_Сохранить_Click(object sender, EventArgs e)
        {
            СохранитьСписок(TrainTableRecords, FileNameLocalTableRec);
        }



        /// <summary>
        /// Сортировка спсиска
        /// </summary>
        private void dgv_TrainTable_Sorted(object sender, EventArgs e)
        {
            РаскраситьСписок();
        }



        protected override void OnClosing(CancelEventArgs e)
        {
            if (MyMainForm == this)
                MyMainForm = null;

            //DispouseCisClientIsConnectRx.Dispose();
            base.OnClosing(e);
        }

        #endregion


        /// <summary>
        /// Источник изменения загрузки расписания
        /// </summary>
        private void rbSourseSheduleLocal_CheckedChanged(object sender, EventArgs e)
        {
            var rb = sender as RadioButton;
            if (rb != null)
            {
                _sourceLoad= (rb.Name == "rbSourseSheduleLocal" && rb.Checked) ? SourceData.Local : SourceData.RemoteCis;
                Program.Настройки.SourceTrainTableRecordLoad = _sourceLoad.ToString();
                ОкноНастроек.СохранитьНастройки();

                SourceLoadMainList();
                ОбновитьДанныеВСписке();
            }
        }
    }
}
