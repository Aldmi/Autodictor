using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using CommunicationDevices.ClientWCF;
using MainExample.Extension;


namespace MainExample
{
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
        public byte TrainPathNumber;
        public byte ShowInPanels;
        public string Примечание;
    };




    public partial class TrainTable : Form
    {
        public CisClient CisClient { get; private set; }
        public IDisposable DispouseCisClientIsConnectRx { get; set; }


        public static List<TrainTableRecord> TrainTableRecords = new List<TrainTableRecord>();
        private static int ID = 0;
        //        private bool ОбновлениеСписка = false;

        public TrainTable(CisClient cisClient)
        {
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
            //            ОбновлениеСписка = true;

            foreach (var Данные in TrainTableRecords)
            {
                ListViewItem lvi = new ListViewItem(new string[] { Данные.ID.ToString(), Данные.Num, Данные.Name, Данные.ArrivalTime, Данные.StopTime, Данные.DepartureTime, Данные.Days.Replace(":", "; ") });
                lvi.Tag = Данные.ID;
                lvi.Checked = Данные.Active;
                this.listView1.Items.Add(lvi);
            }

            //            ОбновлениеСписка = false;
        }

        public static void ЗагрузитьСписок()
        {
            TrainTableRecords.Clear();

            System.IO.StreamReader file = null;


            try
            {
                file = new System.IO.StreamReader("TableRecords.ini");

                string line;

                while ((line = file.ReadLine()) != null)
                {
                    string[] Settings = line.Split(';');
                    if (Settings.Length == 13)
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
                        Данные.TrainPathNumber = byte.Parse(Settings[10]);
                        Данные.ShowInPanels = byte.Parse(Settings[11]);
                        Данные.Примечание = Settings[12];

                        if (Данные.TrainPathDirection > 2)
                            Данные.TrainPathDirection = 0;

                        if (Данные.TrainPathNumber > 14)
                            Данные.TrainPathNumber = 0;

                        TrainTableRecords.Add(Данные);

                        if (Данные.ID > ID)
                            ID = Данные.ID;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (file != null)
                    file.Close();
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


        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
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

                            this.tB_Номер.Text = this.listView1.Items[item].SubItems[1].Text;
                            this.tB_Название.Text = this.listView1.Items[item].SubItems[2].Text;

                            if (this.listView1.Items[item].SubItems[3].Text == "")
                            {
                                this.dTP_Прибытие.Value = new DateTime(2000, 1, 1, 0, 0, 0);
                                this.cB_Прибытие.Checked = false;
                            }
                            else
                            {
                                string[] SubStrings = this.listView1.Items[item].SubItems[3].Text.Split(':');
                                int Часы = 0;
                                int Минуты = 0;

                                if (int.TryParse(SubStrings[0], out Часы) && int.TryParse(SubStrings[1], out Минуты))
                                {
                                    DateTime ВремяПрибытия = new DateTime(2000, 1, 1, Часы, Минуты, 0);
                                    dTP_Прибытие.Value = ВремяПрибытия;
                                    this.cB_Прибытие.Checked = true;
                                }
                            }

                            if (this.listView1.Items[item].SubItems[5].Text == "")
                            {
                                this.dTP_Отправление.Value = new DateTime(2000, 1, 1, 0, 0, 0);
                                this.cB_Отправление.Checked = false;
                            }
                            else
                            {
                                string[] SubStrings = this.listView1.Items[item].SubItems[5].Text.Split(':');
                                int Часы = 0;
                                int Минуты = 0;

                                if (int.TryParse(SubStrings[0], out Часы) && int.TryParse(SubStrings[1], out Минуты))
                                {
                                    DateTime ВремяОтправления = new DateTime(2000, 1, 1, Часы, Минуты, 0);
                                    dTP_Отправление.Value = ВремяОтправления;
                                    this.cB_Отправление.Checked = true;
                                }
                            }

                            if (Данные.TrainPathDirection == 0x01)
                            {
                                rB_Нумерация_Отсутствует.Checked = false;
                                rB_Нумерация_СГоловы.Checked = true;
                                rB_Нумерация_СХвоста.Checked = false;
                            }
                            else if (Данные.TrainPathDirection == 0x02)
                            {
                                rB_Нумерация_Отсутствует.Checked = false;
                                rB_Нумерация_СГоловы.Checked = false;
                                rB_Нумерация_СХвоста.Checked = true;
                            }
                            else
                            {
                                rB_Нумерация_Отсутствует.Checked = true;
                                rB_Нумерация_СГоловы.Checked = false;
                                rB_Нумерация_СХвоста.Checked = false;
                            }

                            if (Данные.ShowInPanels == 0x01)
                            {
                                rBОтображениеДальнегоСледования.Checked = true;
                                rBОтображениеПригород.Checked = false;
                                rBОтображениеОтсутствует.Checked = false;
                            }
                            else if (Данные.ShowInPanels == 0x02)
                            {
                                rBОтображениеДальнегоСледования.Checked = false;
                                rBОтображениеПригород.Checked = true;
                                rBОтображениеОтсутствует.Checked = false;
                            }
                            else
                            {
                                rBОтображениеДальнегоСледования.Checked = false;
                                rBОтображениеПригород.Checked = false;
                                rBОтображениеОтсутствует.Checked = true;
                            }

                            cB_НомерПути.SelectedIndex = Данные.TrainPathNumber;
                            tBПримечание.Text = Данные.Примечание;
                        }
                    }
                }
            }
        }


        private void btn_ДобавитьЗапись_Click(object sender, EventArgs e)
        {
            TrainTableRecord Данные;

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

            TrainTableRecords.Add(Данные);

            ОбновитьДанныеВСписке();
        }

        private void btn_ИзменитьЗапись_Click(object sender, EventArgs e)
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

                            Данные.Num = tB_Номер.Text;
                            Данные.Name = tB_Название.Text;
                            Данные.ArrivalTime = cB_Прибытие.Checked ? dTP_Прибытие.Value.Hour.ToString("00") + ":" + dTP_Прибытие.Value.Minute.ToString("00") : "";
                            int ВремяОстановки = (dTP_Отправление.Value.Hour * 60 + dTP_Отправление.Value.Minute) - (dTP_Прибытие.Value.Hour * 60 + dTP_Прибытие.Value.Minute);
                            if (ВремяОстановки < 0) ВремяОстановки += 24 * 60;
                            Данные.StopTime = cB_Прибытие.Checked && cB_Отправление.Checked ? ВремяОстановки.ToString() : "";
                            Данные.DepartureTime = cB_Отправление.Checked ? dTP_Отправление.Value.Hour.ToString("00") + ":" + dTP_Отправление.Value.Minute.ToString("00") : "";

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

                            TrainTableRecords[i] = Данные;
                            break;
                        }
                    }

                    ОбновитьДанныеВСписке();
                }
            }
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

        private void btn_Вверх_Click(object sender, EventArgs e)
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
                            if (i > 0)
                            {
                                TrainTableRecord Данные1 = TrainTableRecords[i];
                                TrainTableRecord Данные2 = TrainTableRecords[i - 1];

                                int TempID = Данные1.ID;
                                Данные1.ID = Данные2.ID;
                                Данные2.ID = TempID;

                                TrainTableRecords[i - 1] = Данные1;
                                TrainTableRecords[i] = Данные2;

                                ОбновитьДанныеВСписке();
                                this.listView1.Select();
                                this.listView1.Items[i - 1].Selected = true;
                            }

                            break;
                        }
                    }
                }
            }
        }

        private void btn_Вниз_Click(object sender, EventArgs e)
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
                            if (i < (TrainTableRecords.Count - 1))
                            {
                                TrainTableRecord Данные1 = TrainTableRecords[i];
                                TrainTableRecord Данные2 = TrainTableRecords[i + 1];

                                int TempID = Данные1.ID;
                                Данные1.ID = Данные2.ID;
                                Данные2.ID = TempID;

                                TrainTableRecords[i + 1] = Данные1;
                                TrainTableRecords[i] = Данные2;

                                ОбновитьДанныеВСписке();
                                this.listView1.Select();
                                this.listView1.Items[i + 1].Selected = true;
                            }

                            break;
                        }
                    }
                }
            }
        }

        private void btn_Сохранить_Click(object sender, EventArgs e)
        {
            СохранитьСписок();
        }

        private void СохранитьСписок()
        {
            try
            {
                StreamWriter DumpFile = new StreamWriter("TableRecords.ini");

                for (int i = 0; i < TrainTableRecords.Count; i++)
                {
                    DumpFile.WriteLine((i + 1).ToString() + ";" +
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
                        TrainTableRecords[i].ShowInPanels.ToString() + ";" +
                        TrainTableRecords[i].Примечание
                        );
                }

                DumpFile.Flush();
                DumpFile.Dispose();
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

        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            /*            if (ОбновлениеСписка)
                            return;

                        if ((sender as ListView).PointToClient(Cursor.Position).X > 22)
                            e.NewValue = e.CurrentValue;
             */
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //            ОбновитьДанныеВСписке();
        }

        private void gBОтображениеРасписания_CursorChanged(object sender, EventArgs e)
        {

        }


        protected override void OnClosed(EventArgs e)
        {
            DispouseCisClientIsConnectRx.Dispose();
            base.OnClosed(e);
        }


        //Загрузка расписание из выбранного источника
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (rbSourseSheduleLocal.Checked)
            {
                ЗагрузитьСписок();
            }
            else
            {
                LoadListFromCis();
            }

            ОбновитьДанныеВСписке();
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
                    Данные.ArrivalTime = op.ArrivalTime.ToLongTimeString();


                    var stopTime = (op.ArrivalTime.Subtract(op.DepartureTime));
                    Данные.StopTime = stopTime.TotalMilliseconds > 0 ? new DateTime(stopTime.Ticks).ToString("HH:mm:ss") : "---";


                    Данные.DepartureTime = op.DepartureTime.ToLongTimeString();
                    Данные.Days = CisClient.RegulatoryScheduleDatas.FirstOrDefault(reg=> reg.NumberOfTrain == op.NumberOfTrain)?.DaysFollowing;                                              //заполняется из регулярного расписания
                    Данные.Active = false;
                    Данные.SoundTemplates = "";
                    Данные.TrainPathDirection = 1;                                   //заполняется из информации
                    Данные.TrainPathNumber = 0;                                      //заполняется из информации
                    Данные.ShowInPanels = 0;
                    Данные.Примечание = "";

                    if (Данные.TrainPathDirection > 2)
                        Данные.TrainPathDirection = 0;

                    if (Данные.TrainPathNumber > 10)
                        Данные.TrainPathNumber = 0;

                    TrainTableRecords.Add(Данные);

                    if (Данные.ID > ID)
                        ID = Данные.ID;
                }
            }

        }


    }
}
