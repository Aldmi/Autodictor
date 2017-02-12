using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using CommunicationDevices.ClientWCF;
using MainExample.Extension;


namespace MainExample
{
    public enum ТипПоезда
    {
        НеОпределен = 0,
        Пассажирский = 1,
        Пригородный = 2,
        Фирменный = 3,
        Скорый = 4,
        Скоростной = 5,
        Ласточка = 6,
        РЭКС = 7
    };
    






    public struct TrainTableRecord
    {
        public int ID;                    //Id
        public string Num;                //Номер поезда
        public string Name;               //Название
        public string ArrivalTime;        //прибытие
        public string StopTime;           //стоянка
        public string DepartureTime;      //отправление
        public string Days;               //дни следования
        public bool Active;               //активность, отмека галочкой
        public string SoundTemplates;     //
        public byte TrainPathDirection;
        public string TrainPathNumber;
        public ТипПоезда ТипПоезда;
        public string Примечание;
        public DateTime ВремяНачалаДействияРасписания;
        public DateTime ВремяОкончанияДействияРасписания;
    };



    public partial class TrainTable : Form
    {
        public CisClient CisClient { get; private set; }
        public IDisposable DispouseCisClientIsConnectRx { get; set; }
        
        public static List<TrainTableRecord> TrainTableRecords = new List<TrainTableRecord>();
        private static int ID = 0;
        static public TrainTable myMainForm = null;


        public TrainTable(CisClient cisClient)
        {
            if (myMainForm != null)
                return;

            myMainForm = this;

            InitializeComponent();
            ОбновитьДанныеВСписке();

            btnLoad_Click(null, EventArgs.Empty);  //загрузка по умолчанию 


            CisClient = cisClient;
            if (CisClient.IsConnect)
            {
                pnСостояниеCIS.InvokeIfNeeded(() => pnСостояниеCIS.BackColor = Color.LightGreen);
                lblСостояниеCIS.InvokeIfNeeded(() => lblСостояниеCIS.Text = "ЦИС на связи");
            }
            else
            {
                pnСостояниеCIS.InvokeIfNeeded(() => pnСостояниеCIS.BackColor = Color.Orange);
                lblСостояниеCIS.InvokeIfNeeded(() => lblСостояниеCIS.Text = "ЦИС НЕ на связи");
            }

            DispouseCisClientIsConnectRx = CisClient.IsConnectChange.Subscribe(isConnect =>
            {
                if (isConnect)
                {
                    pnСостояниеCIS.InvokeIfNeeded(() => pnСостояниеCIS.BackColor = Color.LightGreen);
                    lblСостояниеCIS.InvokeIfNeeded(() => lblСостояниеCIS.Text = "ЦИС на связи");
                }
                else
                {
                    pnСостояниеCIS.InvokeIfNeeded(() => pnСостояниеCIS.BackColor = Color.Orange);
                    lblСостояниеCIS.InvokeIfNeeded(() => lblСостояниеCIS.Text = "ЦИС НЕ на связи");
                }
            });
        }

        private void ОбновитьДанныеВСписке()
        {
            listView1.Items.Clear();

            foreach (var Данные in TrainTableRecords)
            {
                string СтрокаОписанияРасписания = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(Данные.Days).ПолучитьСтрокуОписанияРасписания();

                ListViewItem lvi = new ListViewItem(new string[] { Данные.ID.ToString(), Данные.Num, Данные.Name, Данные.ArrivalTime, Данные.StopTime, Данные.DepartureTime, СтрокаОписанияРасписания });
                lvi.Tag = Данные;
                lvi.BackColor = Данные.Active ? Color.LightGreen : Color.LightGray;
                this.listView1.Items.Add(lvi);
            }
        }

        private void ОбновитьСостояниеАктивностиВТаблице()
        {
            for (int item = 0; item < this.listView1.Items.Count; item++)
            {
                if (item <= TrainTableRecords.Count)
                {
                    try
                    {
                        TrainTableRecord record = (TrainTableRecord)this.listView1.Items[item].Tag;
                        this.listView1.Items[item].BackColor = record.Active ? Color.LightGreen : Color.LightGray;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

            foreach (int item in sic)
            {
                int ID = 0;
                if (int.TryParse(this.listView1.Items[item].SubItems[0].Text, out ID) == true)
                {
                    for (int i = 0; i < TrainTableRecords.Count; i++)
                    {
                        if (TrainTableRecords[i].ID == ID)
                        {
                            TrainTableRecord Данные;

                            Данные = TrainTableRecords[i];
                            ПланРасписанияПоезда ТекущийПланРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(Данные.Days);
                            ТекущийПланРасписанияПоезда.УстановитьНомерПоезда(Данные.Num);
                            ТекущийПланРасписанияПоезда.УстановитьНазваниеПоезда(Данные.Name);

                            Расписание расписание = new Расписание(ТекущийПланРасписанияПоезда);
                            расписание.ShowDialog();
                            if (расписание.DialogResult == System.Windows.Forms.DialogResult.OK)
                                Данные.Days = расписание.ПолучитьПланРасписанияПоезда().ПолучитьСтрокуРасписания();

                            TrainTableRecords[i] = Данные;
                            ОбновитьДанныеВСписке();
                            break;
                        }
                    }
                }
            }
        }


        private void btn_ДобавитьЗапись_Click(object sender, EventArgs e)
        {
            TrainTableRecord Данные;
            Данные.ID = ++ID;
            Данные.Num = "";
            Данные.Name = "";
            Данные.ArrivalTime = "00:00";
            Данные.StopTime = "00:00";
            Данные.DepartureTime = "00:00";
            Данные.Days = "";
            Данные.Active = true;
            Данные.SoundTemplates = "";
            Данные.TrainPathDirection = 0x01;
            Данные.ТипПоезда = ТипПоезда.НеОпределен;
            Данные.TrainPathNumber = "";
            Данные.Примечание = "";
            Данные.ВремяНачалаДействияРасписания = new DateTime(1900, 1, 1);
            Данные.ВремяОкончанияДействияРасписания = new DateTime(2100, 1, 1);

            /*
            Данные.ID = ++ID;
            Данные.Num = tB_Номер.Text;
            Данные.Name = tB_Название.Text;
            Данные.ArrivalTime = cB_Прибытие.Checked ? dTP_Прибытие.Value.Hour.ToString("00") + ":" + dTP_Прибытие.Value.Minute.ToString("00") : "";
            int ВремяОстановки = (dTP_Отправление.Value.Hour * 60 + dTP_Отправление.Value.Minute) - (dTP_Прибытие.Value.Hour * 60 + dTP_Прибытие.Value.Minute);
            if (ВремяОстановки < 0) ВремяОстановки += 24 * 60;
            Данные.StopTime = cB_Прибытие.Checked && cB_Отправление.Checked ? ВремяОстановки.ToString() : "";
            Данные.DepartureTime = cB_Отправление.Checked ? dTP_Отправление.Value.Hour.ToString("00") + ":" + dTP_Отправление.Value.Minute.ToString("00") : "";
            Данные.Days = "";
            Данные.Active = true;
            Данные.SoundTemplates = "::::::::::::::";


            if (rB_Нумерация_СГоловы.Checked == true)
                Данные.TrainPathDirection = 0x01;
            else if (rB_Нумерация_СХвоста.Checked == true)
                Данные.TrainPathDirection = 0x02;
            else
                Данные.TrainPathDirection = 0x00;

            if (rBОтображениеОтсутствует.Checked == true)
                Данные.ShowInPanels = 0x00;
            else if (rBОтображениеДальнегоСледования.Checked == true)
                Данные.ShowInPanels = 0x01;
            else
                Данные.ShowInPanels = 0x02;

            if ((cB_НомерПути.SelectedIndex < 0) || (cB_НомерПути.SelectedIndex > 14))
                Данные.TrainPathNumber = 0;
            else
                Данные.TrainPathNumber = (byte)cB_НомерПути.SelectedIndex;

            Данные.Примечание = tBПримечание.Text;
            */
            TrainTableRecords.Add(Данные);

            ОбновитьДанныеВСписке();
        }


        private void btn_УдалитьЗапись_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

            foreach (int item in sic)
            {
                int ID = 0;
                if (int.TryParse(this.listView1.Items[item].SubItems[0].Text, out ID) == true)
                {
                    for (int i = 0; i < TrainTableRecords.Count; i++)
                    {
                        if (TrainTableRecords[i].ID == ID)
                        {
                            TrainTableRecords.RemoveAt(i);
                            break;
                        }
                    }

                    ОбновитьДанныеВСписке();
                }
            }
        }

        private void btn_Сохранить_Click(object sender, EventArgs e)
        {
            СохранитьСписок();
        }

        public static void ЗагрузитьСписок()
        {
            TrainTableRecords.Clear();

            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader("TableRecords.ini"))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] Settings = line.Split(';');
                        if ((Settings.Length == 13) || (Settings.Length == 15))
                        {
                            TrainTableRecord Данные;

                            Данные.ID = int.Parse(Settings[0]);
                            Данные.Num = Settings[1];
                            Данные.Name = Settings[2];
                            Данные.ArrivalTime = Settings[3];
                            Данные.StopTime = Settings[4];
                            Данные.DepartureTime = Settings[5];
                            Данные.Days = Settings[6];
                            Данные.Active = Settings[7] == "1" ? true : false;
                            Данные.SoundTemplates = Settings[8];
                            Данные.TrainPathDirection = byte.Parse(Settings[9]);
                            Данные.TrainPathNumber = Settings[10];

                            ТипПоезда типПоезда = ТипПоезда.НеОпределен;
                            try
                            {
                                типПоезда = (ТипПоезда)Enum.Parse(typeof(ТипПоезда), Settings[11]);
                            }
                            catch (ArgumentException) { }
                            Данные.ТипПоезда = типПоезда;

                            Данные.Примечание = Settings[12];

                            if (Данные.TrainPathDirection > 2)
                                Данные.TrainPathDirection = 0;

                            if (Program.НомераПутей.Contains(Данные.TrainPathNumber) == false)
                                Данные.TrainPathNumber = "";

                            DateTime НачалоДействия = new DateTime(1900, 1, 1);
                            DateTime КонецДействия = new DateTime(2100, 1, 1);
                            if (Settings.Length == 15)
                            {
                                DateTime.TryParse(Settings[13], out НачалоДействия);
                                DateTime.TryParse(Settings[14], out КонецДействия);
                            }

                            Данные.ВремяНачалаДействияРасписания = НачалоДействия;
                            Данные.ВремяОкончанияДействияРасписания = КонецДействия;
                            TrainTableRecords.Add(Данные);

                            Program.НомераПоездов.Add(Данные.Num);

                            if (Данные.ID > ID)
                                ID = Данные.ID;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void СохранитьСписок()
        {
            try
            {
                using (StreamWriter DumpFile = new StreamWriter("TableRecords.ini"))
                {

                    for (int i = 0; i < TrainTableRecords.Count; i++)
                    {
                        string line = (i + 1).ToString() + ";" +
                            TrainTableRecords[i].Num + ";" +
                            TrainTableRecords[i].Name + ";" +
                            TrainTableRecords[i].ArrivalTime + ";" +
                            TrainTableRecords[i].StopTime + ";" +
                            TrainTableRecords[i].DepartureTime + ";" +
                            TrainTableRecords[i].Days + ";" +
                            (TrainTableRecords[i].Active ? "1" : "0") + ";" +
                            TrainTableRecords[i].SoundTemplates + ";" +
                            TrainTableRecords[i].TrainPathDirection.ToString() + ";" +
                            TrainTableRecords[i].TrainPathNumber.ToString() + ";" +
                            TrainTableRecords[i].ТипПоезда.ToString() + ";" +
                            TrainTableRecords[i].Примечание + ";" +
                            TrainTableRecords[i].ВремяНачалаДействияРасписания.ToString("dd.MM.yyyy HH:mm:ss") + ";" +
                            TrainTableRecords[i].ВремяОкончанияДействияРасписания.ToString("dd.MM.yyyy HH:mm:ss");

                        DumpFile.WriteLine(line);
                    }

                    DumpFile.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void btn_ШаблонОповещения_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

            foreach (int item in sic)
            {
                int ID = 0;
                if (int.TryParse(this.listView1.Items[item].SubItems[0].Text, out ID) == true)
                {
                    for (int i = 0; i < TrainTableRecords.Count; i++)
                    {
                        if (TrainTableRecords[i].ID == ID)
                        {
                            TrainTableRecord Данные;

                            Данные = TrainTableRecords[i];
                            ПланРасписанияПоезда ТекущийПланРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(Данные.Days);
                            ТекущийПланРасписанияПоезда.УстановитьНомерПоезда(Данные.Num);
                            ТекущийПланРасписанияПоезда.УстановитьНазваниеПоезда(Данные.Name);

                            Оповещение оповещение = new Оповещение(Данные);
                            оповещение.ShowDialog();
                            if (оповещение.DialogResult == System.Windows.Forms.DialogResult.OK)
                                Данные.SoundTemplates = оповещение.ПолучитьШаблоныОповещения();

                            TrainTableRecords[i] = Данные;
                            ОбновитьДанныеВСписке();
                            break;
                        }
                    }
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (myMainForm == this)
                myMainForm = null;

            DispouseCisClientIsConnectRx.Dispose();
            base.OnClosing(e);
        }



        //Загрузка расписание из выбранного источника
        private void btnLoad_Click(object sender, EventArgs e)
        {
            SouceLoadMainList();
            ОбновитьДанныеВСписке();
        }


        public void SouceLoadMainList()
        {
            if (rbSourseSheduleLocal.Checked)
            {
                ЗагрузитьСписок();
            }
            else
            {
                LoadListFromCis();
            }
        }


        private void LoadListFromCis()
        {
            var isOperShLoad = CisClient.OperativeScheduleDatas != null && CisClient.OperativeScheduleDatas.Any();
            var isRegShLoad = CisClient.RegulatoryScheduleDatas != null && CisClient.RegulatoryScheduleDatas.Any();

            if (!isRegShLoad && !isOperShLoad)
            {
                MessageBox.Show("ОПЕРАТИВНОЕ И РЕГУЛЯРНОЕ РАСПИСАНИЕ ОТ СЕРВЕРА НЕ ПОЛУЧЕННО");
                return;
            }

            if (!isOperShLoad)
            {
                MessageBox.Show("ОПЕРАТИВНОЕ РАСПИСАНИЕ ОТ СЕРВЕРА НЕ ПОЛУЧЕННО");
                return;
            }

            if (!isRegShLoad)
            {
                MessageBox.Show("РЕГУЛЯРНОЕ РАСПИСАНИЕ ОТ СЕРВЕРА НЕ ПОЛУЧЕННО");
                return;
            }

            bool tryLoad;
            if (CisClient.IsConnect)
            {
                tryLoad = true;
            }
            else
            {
                tryLoad = MessageBox.Show("Продолжить загрузку данных с ЦИС ", "ЦИС не на связи", MessageBoxButtons.YesNo) == DialogResult.Yes;
            }

            if (tryLoad)
            {
                TrainTableRecords.Clear();

                //Заполним строки
                foreach (var op in CisClient.OperativeScheduleDatas)
                {
                    TrainTableRecord Данные;

                    Данные.ID = op.Id;
                    Данные.Num = op.NumberOfTrain;
                    Данные.Name = op.RouteName;
                    Данные.ArrivalTime = op.ArrivalTime?.ToLongTimeString() ?? "Не указанно";


                    if (op.ArrivalTime.HasValue && op.DepartureTime.HasValue)
                    {
                        var stopTime = (op.ArrivalTime.Value.Subtract(op.DepartureTime.Value));
                        Данные.StopTime = stopTime.TotalMilliseconds > 0 ? new DateTime(stopTime.Ticks).ToString("HH:mm:ss") : "---";
                    }
                    else
                    {
                        Данные.StopTime = "---";
                    }


                    Данные.DepartureTime = op.DepartureTime?.ToLongTimeString() ?? "Не указанно";
                    Данные.Days = CisClient.RegulatoryScheduleDatas.FirstOrDefault(reg=> reg.NumberOfTrain == op.NumberOfTrain)?.DaysFollowing;                                              //заполняется из регулярного расписания
                    Данные.Active = false;
                    Данные.SoundTemplates = "";
                    Данные.TrainPathDirection = 1;                                   //заполняется из информации
                    Данные.TrainPathNumber = "";                                      //заполняется из информации
                    Данные.ТипПоезда = ТипПоезда.НеОпределен;
                    Данные.Примечание = "";

                    if (Данные.TrainPathDirection > 2)
                        Данные.TrainPathDirection = 0;

                    if (Program.НомераПутей.Contains(Данные.TrainPathNumber) == false)
                        Данные.TrainPathNumber = "";


                    Данные.ВремяНачалаДействияРасписания = new DateTime(1900, 1, 1);
                    Данные.ВремяОкончанияДействияРасписания = new DateTime(2100, 1, 1);

                    TrainTableRecords.Add(Данные);

                    if (Данные.ID > ID)
                        ID = Данные.ID;
                }
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

            foreach (int item in sic)
            {
                int ID = 0;
                if (int.TryParse(this.listView1.Items[item].SubItems[0].Text, out ID) == true)
                {
                    for (int i = 0; i < TrainTableRecords.Count; i++)
                    {
                        if (TrainTableRecords[i].ID == ID)
                        {
                            TrainTableRecord Данные;

                            Данные = TrainTableRecords[i];
                            ПланРасписанияПоезда ТекущийПланРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(Данные.Days);
                            ТекущийПланРасписанияПоезда.УстановитьНомерПоезда(Данные.Num);
                            ТекущийПланРасписанияПоезда.УстановитьНазваниеПоезда(Данные.Name);

                            Оповещение оповещение = new Оповещение(Данные);
                            оповещение.ShowDialog();
                            Данные.Active = !оповещение.cBБлокировка.Checked;
                            if (оповещение.DialogResult == System.Windows.Forms.DialogResult.OK)
                            {
                                Данные = оповещение.РасписаниеПоезда;
                                this.listView1.Items[item].SubItems[1].Text = Данные.Num;
                                this.listView1.Items[item].SubItems[2].Text = Данные.Name;
                                this.listView1.Items[item].SubItems[3].Text = Данные.ArrivalTime;
                                this.listView1.Items[item].SubItems[4].Text = Данные.StopTime;
                                this.listView1.Items[item].SubItems[5].Text = Данные.DepartureTime;

                                string СтрокаОписанияРасписания = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(Данные.Days).ПолучитьСтрокуОписанияРасписания();
                                this.listView1.Items[item].SubItems[6].Text = СтрокаОписанияРасписания;

                                this.listView1.Items[item].BackColor = Данные.Active ? Color.LightGreen : Color.LightGray;
                            }

                            TrainTableRecords[i] = Данные;
                            //ОбновитьСостояниеАктивностиВТаблице();
                            break;
                        }
                    }
                }
            }

        }
    }
}
