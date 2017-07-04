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
using System.Windows.Forms.VisualStyles;


namespace MainExample
{
    public partial class TrainTableOperative : Form
    {
        #region fields

        private const string TableFileName = "TableRecordsOperative.ini";
        public static List<TrainTableRecord> TrainTableRecords = new List<TrainTableRecord>();
        private static int _id = 0;
        public static TrainTableOperative myMainForm = null;

        #endregion




        #region ctor

        public TrainTableOperative()
        {
            if (myMainForm != null)
                return;

            myMainForm = this;

            InitializeComponent();

           // ОбновитьДанныеВСписке();
            btnLoad_Click(null, EventArgs.Empty);  //загрузка по умолчанию 
        }

        #endregion




        #region EventHandlers

        private void btn_ДобавитьЗапись_Click(object sender, EventArgs e)
        {
            var form = new OperativeTableAddItemForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                //SoundRecord Record = окно.Record;

                //int TryCounter = 50;
                //while (--TryCounter > 0)
                //{
                //    string Key = Record.Время.ToString("yy.MM.dd  HH:mm:ss");
                //    string[] SubKeys = Key.Split(':');
                //    if (SubKeys[0].Length == 1)
                //        Key = "0" + Key;

                //    if (MainWindowForm.SoundRecords.ContainsKey(Key) == false)
                //    {
                //        MainWindowForm.SoundRecords.Add(Key, Record);
                //        MainWindowForm.SoundRecordsOld.Add(Key, Record);
                //        break;
                //    }

                //    Record.Время = Record.Время.AddSeconds(1);
                //}
            }



            //TrainTableRecord Данные;
            //Данные.ID = ++_id;
            //Данные.Num = "";
            //Данные.Num2 = "";
            //Данные.Addition = "";
            //Данные.Name = "";
            //Данные.StationArrival = "";
            //Данные.StationDepart = "";
            //Данные.Direction = "";
            //Данные.ArrivalTime = "00:00";
            //Данные.StopTime = "00:00";
            //Данные.DepartureTime = "00:00";
            //Данные.FollowingTime = "00:00";
            //Данные.Days = "";
            //Данные.DaysAlias = "";
            //Данные.Active = true;
            //Данные.SoundTemplates = "";
            //Данные.TrainPathDirection = 0x01;
            //Данные.ТипПоезда = ТипПоезда.НеОпределен;
            //Данные.TrainPathNumber = new Dictionary<WeekDays, string>
            //{
            //    [WeekDays.Постоянно] = String.Empty,
            //    [WeekDays.Пн] = String.Empty,
            //    [WeekDays.Вт] = String.Empty,
            //    [WeekDays.Ср] = String.Empty,
            //    [WeekDays.Ср] = String.Empty,
            //    [WeekDays.Чт] = String.Empty,
            //    [WeekDays.Пт] = String.Empty,
            //    [WeekDays.Сб] = String.Empty,
            //    [WeekDays.Вс] = String.Empty
            //};
            //Данные.PathWeekDayes = false;
            //Данные.Примечание = "";
            //Данные.ВремяНачалаДействияРасписания = new DateTime(1900, 1, 1);
            //Данные.ВремяОкончанияДействияРасписания = new DateTime(2100, 1, 1);
            //Данные.Addition = "";
            //Данные.ИспользоватьДополнение = new Dictionary<string, bool>
            //{
            //    ["звук"] = false,
            //    ["табло"] = false
            //};
            //Данные.Автомат = true;

            //TrainTableRecords.Add(Данные);
            //ОбновитьДанныеВСписке();
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



        //Загрузка расписание из выбранного источника
        private void btnLoad_Click(object sender, EventArgs e)
        {
            ЗагрузитьСписок();
            ОбновитьДанныеВСписке();
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

        #endregion





        #region Methode

        private void ОбновитьДанныеВСписке()
        {
            listView1.Items.Clear();

            foreach (var данные in TrainTableRecords)
            {
                string строкаОписанияРасписания = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(данные.Days).ПолучитьСтрокуОписанияРасписания();

                ListViewItem lvi = new ListViewItem(new string[] { данные.ID.ToString(), данные.Num, данные.Name, данные.ArrivalTime, данные.StopTime, данные.DepartureTime, строкаОписанияРасписания });
                lvi.Tag = данные;
                lvi.BackColor = данные.Active ? Color.LightGreen : Color.LightGray;
                this.listView1.Items.Add(lvi);
            }
        }



        public static void ЗагрузитьСписок()
        {
            TrainTableRecords.Clear();

            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader(TableFileName))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] Settings = line.Split(';');
                        if ((Settings.Length == 13) || (Settings.Length == 15) || (Settings.Length >= 16))
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
                            Данные.TrainPathNumber = LoadPathFromFile(Settings[10], out Данные.PathWeekDayes);
                            Данные.ИспользоватьДополнение = new Dictionary<string, bool>()
                            {
                                ["звук"] = false,
                                ["табло"] = false
                            };
                            Данные.Автомат = true;

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

                            if (Program.НомераПутей.Contains(Данные.TrainPathNumber[WeekDays.Постоянно]) == false)
                                Данные.TrainPathNumber[WeekDays.Постоянно] = "";

                            DateTime НачалоДействия = new DateTime(1900, 1, 1);
                            DateTime КонецДействия = new DateTime(2100, 1, 1);
                            if (Settings.Length >= 15)
                            {
                                DateTime.TryParse(Settings[13], out НачалоДействия);
                                DateTime.TryParse(Settings[14], out КонецДействия);
                            }
                            Данные.ВремяНачалаДействияРасписания = НачалоДействия;
                            Данные.ВремяОкончанияДействияРасписания = КонецДействия;


                            var addition = "";
                            if (Settings.Length >= 16)
                            {
                                addition = Settings[15];
                            }
                            Данные.Addition = addition;


                            if (Settings.Length >= 18)
                            {
                                Данные.ИспользоватьДополнение["табло"] = Settings[16] == "1";
                                Данные.ИспользоватьДополнение["звук"] = Settings[17] == "1";
                            }

                            if (Settings.Length >= 19)
                            {
                                Данные.Автомат = (string.IsNullOrEmpty(Settings[18]) || Settings[18] == "1"); // по умолчанию true
                            }


                            Данные.Num2 = String.Empty;
                            Данные.FollowingTime = String.Empty;
                            Данные.DaysAlias = String.Empty;
                            if (Settings.Length >= 22)
                            {
                                Данные.Num2 = Settings[19];
                                Данные.FollowingTime = Settings[20];
                                Данные.DaysAlias = Settings[21];
                            }


                            Данные.StationDepart = String.Empty;
                            Данные.StationArrival = String.Empty;
                            if (Settings.Length >= 23)
                            {
                                Данные.StationDepart = Settings[22];
                                Данные.StationArrival = Settings[23];
                            }

                            Данные.Direction = String.Empty;
                            if (Settings.Length >= 24)
                            {
                                Данные.StationDepart = Settings[24];
                            }


                            TrainTableRecords.Add(Данные);
                            Program.НомераПоездов.Add(Данные.Num);
                            if (!string.IsNullOrEmpty(Данные.Num2))
                                Program.НомераПоездов.Add(Данные.Num2);

                            if (Данные.ID > _id)
                                _id = Данные.ID;
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
                            SavePath2File(TrainTableRecords[i].TrainPathNumber, TrainTableRecords[i].PathWeekDayes) + ";" +
                            TrainTableRecords[i].ТипПоезда.ToString() + ";" +
                            TrainTableRecords[i].Примечание + ";" +
                            TrainTableRecords[i].ВремяНачалаДействияРасписания.ToString("dd.MM.yyyy HH:mm:ss") + ";" +
                            TrainTableRecords[i].ВремяОкончанияДействияРасписания.ToString("dd.MM.yyyy HH:mm:ss") + ";" +
                            TrainTableRecords[i].Addition + ";" +
                            (TrainTableRecords[i].ИспользоватьДополнение["табло"] ? "1" : "0") + ";" +
                            (TrainTableRecords[i].ИспользоватьДополнение["звук"] ? "1" : "0") + ";" +
                            (TrainTableRecords[i].Автомат ? "1" : "0") + ";" +

                            TrainTableRecords[i].Num2 + ";" +
                            TrainTableRecords[i].FollowingTime + ";" +
                            TrainTableRecords[i].DaysAlias + ";" +

                            TrainTableRecords[i].StationDepart + ";" +
                            TrainTableRecords[i].StationArrival + ";" +
                            TrainTableRecords[i].Direction;

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

        #endregion




        protected override void OnClosing(CancelEventArgs e)
        {
            if (myMainForm == this)
                myMainForm = null;

            base.OnClosing(e);
        }
    }
}
