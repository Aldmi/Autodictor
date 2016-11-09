using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.ServiceModel;
using MainExample.ClientWCF;
using MainExample.Extension;
using WCFCis2AvtodictorContract.Contract;


namespace MainExample
{
    public enum SoundRecordStatus { Выключена = 0, ОжиданиеВоспроизведения, Воспроизведение, Воспроизведена };
    public enum SoundRecordType { Обычное = 0, ДвижениеПоезда, ДвижениеПоездаНеПодтвержденное, Предупредительное, Важное };
    public struct SoundRecord
    {
        public int ID;
        public DateTime Время;
        public float Длительность;
        public string Описание;
        public string[] ИменаФайлов;
        public SoundRecordStatus Состояние;
        public SoundRecordType ТипСообщения;
        public byte НомерПути;
        public byte НумерацияПоезда;
        public DateTime ВремяПрибытия;
        public uint ВремяСтоянки;
        public DateTime ВремяОтправления;
        public byte БитыАктивностиПолей;
        public string НомерПоезда;
        public byte ШаблонВоспроизведенияПути;
        public string НазваниеПоезда;
        public byte ОтображениеВТаблицах;
        public string Примечание;
    };


    public partial class MainWindowForm : Form
    {
        private bool РазрешениеРаботы = false;
        static public SortedDictionary<string, SoundRecord> SoundRecords = new SortedDictionary<string, SoundRecord>();
        static private int ID = 1;
        private int ТекущаяДата = 0;
        private int Volume = 0;
        private bool ОбновлениеСписка = false;
        private bool ВоспроизводитьДвижениеПоездов = true;

        static public MainWindowForm myMainForm = null;

        private Предупреждение ОкноПредупреждения = null;

        private string[] ВоспроизводимыеФайлы;
        private byte НомерВоспроизводимогоТрека = 0;
        private int ОбщееВремяВоспроизведения = 0;
        private float ВремяВоспроизведенияПроигранныхФайлов = 0f;

        public CisClient CisClient { get; private set; }
        public IDisposable DispouseCisClientIsConnectRx { get; set; }


        // Конструктор
        public MainWindowForm(CisClient cisClient)
        {
            if (myMainForm != null)
                return;

            myMainForm = this;

            InitializeComponent();
            this.lblTime.Text = DateTime.Now.ToLongTimeString();

            btnБлокировка.Text = "ВКЛЮЧИТЬ";
            pnСостояние.BackColor = Color.Orange;
            lblСостояние.Text = "ВЫКЛЮЧЕНА";

            lblСостояниеCIS.Text = "ЦИС НЕ на связи";
            pnСостояниеCIS.BackColor = Color.Orange;

            CisClient = cisClient;
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

            //TODO: подписывать через React. И при закрыти окна отписывать.
            //CisClient.PropertyChanged += (o, e) =>                         
            //{
            //    if (e.PropertyName == "IsConnect") //обработчик событи изменени¤ свойства Name
            //    {
            //        //MessageBox.Show(e.PropertyName + "changed");
            //        cisClient = o as CisClient;
            //        if (cisClient != null)
            //        {
            //            if (CisClient.IsConnect)
            //            {
            //                pnСостояниеCIS.InvokeIfNeeded(() => pnСостояниеCIS.BackColor = Color.LightGreen);
            //                lblСостояниеCIS.InvokeIfNeeded(() => lblСостояниеCIS.Text = "ЦИС на связи");
            //            }
            //            else
            //            {
            //                pnСостояниеCIS.InvokeIfNeeded(() => pnСостояниеCIS.BackColor = Color.Orange);
            //                lblСостояниеCIS.InvokeIfNeeded(() => lblСостояниеCIS.Text = "ЦИС НЕ на связи");
            //            }
            //        }
            //    }
            //};
        }

        // Обработка таймера 100 мс для воспроизведения звуковых сообщений
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
            this.lblDate.Text = DateTime.Now.ToString("dd.MM.yyyy");
            this.lblDayOfWeek.Text = DateTime.Now.ToString("dddd");
            if ((DateTime.Now.DayOfWeek == System.DayOfWeek.Saturday) || (DateTime.Now.DayOfWeek == System.DayOfWeek.Sunday))
                this.lblDayOfWeek.ForeColor = Color.Yellow;
            else
                this.lblDayOfWeek.ForeColor = Color.Green;

            //ОбработкаЗвуковогоПотка(); //TODO: включить
        }

        // Обработка нажатия кнопки блокировки/разрешения работы
        private void btnБлокировка_Click(object sender, EventArgs e)
        {
            //TODO: Запускать/останавливать WCF клиента 
            РазрешениеРаботы = !РазрешениеРаботы;

            if (РазрешениеРаботы == true)
            {
                CisClient.Start();

                btnБлокировка.Text = "ОТКЛЮЧИТЬ";
                pnСостояние.BackColor = Color.LightGreen;
                lblСостояние.Text = "РАБОТА";
            }
            else
            {
                CisClient.Stop();

                btnБлокировка.Text = "ВКЛЮЧИТЬ";
                pnСостояние.BackColor = Color.Orange;
                lblСостояние.Text = "ВЫКЛЮЧЕНА";
            }
        }

        // Обновление списка вопроизведения сообщений при нажатии кнопки на панели
        private void btnОбновитьСписок_Click(object sender, EventArgs e)
        {
            ОбновитьСписокЗвуковыхСообщений();
            ОбновитьСписокЗвуковыхСообщенийВТаблице();
        }

        // Формирование списка воспроизведения
        public static void ОбновитьСписокЗвуковыхСообщений()
        {
            SoundRecords.Clear();
            ID = 1;
            
            #region Создание звуковых файлов расписания ЖД транспорта
            foreach (TrainTableRecord Config in TrainTable.TrainTableRecords)
            {
                if (Config.Active == true)
                {
                    {
                        DateTime ТекущееВремя = DateTime.Now;
                        ПланРасписанияПоезда планРасписанияПоезда =  ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(Config.Days);
                        

                        if (планРасписанияПоезда.ПолучитьАктивностьДняДвижения((byte)(ТекущееВремя.Month - 1), (byte)(ТекущееВремя.Day - 1)) == true)
                        {
                            string[] ШаблонОповещения = Config.SoundTemplates.Split(':');

                            if (ШаблонОповещения.Length == 15)
                                for (int j = 0; j < 5; j++)
                                {
                                    foreach (var Item in DynamicSoundForm.DynamicSoundRecords)
                                        if (Item.Name == ШаблонОповещения[3 * j])
                                        {
                                            int ШаблонОповещенияПути = 0;
                                            int.TryParse(ШаблонОповещения[3 * j + 2], out ШаблонОповещенияПути);

                                            string[] ВременаРаботыШаблона = (ШаблонОповещения[3 * j + 1].Replace(" ", "")).Split(',');
                                            if (ВременаРаботыШаблона.Length > 0)
                                            {
                                                foreach (string Time in ВременаРаботыШаблона)
                                                {
                                                    int КоличествоМинут;
                                                    if (int.TryParse(Time, out КоличествоМинут))
                                                    {
                                                        SoundRecord Record;
                                                        Record.Состояние = SoundRecordStatus.ОжиданиеВоспроизведения;
                                                        Record.ТипСообщения = SoundRecordType.ДвижениеПоездаНеПодтвержденное;
                                                        Record.ШаблонВоспроизведенияПути = (byte)(((ШаблонОповещенияПути < 0) || (ШаблонОповещенияПути > 3)) ? 0 : ШаблонОповещенияПути);

                                                        Record.Описание = Item.Name + " (" + КоличествоМинут.ToString() + " минут";
                                                        switch (j)
                                                        {
                                                            case 0: Record.Описание += " до прибытия)";  break;
                                                            case 1: Record.Описание += " после прибытия)"; break;
                                                            case 2: Record.Описание += " до отправления)"; break;
                                                            case 3: Record.Описание += " до отправления)"; break;
                                                            case 4: Record.Описание += " после отправления)"; break;
                                                        }

                                                        Record.ИменаФайлов = new string[1];
                                                        Record.ИменаФайлов[0] = StaticSoundForm.GetFilePath(Record.Описание);
                                                        Record.НомерПоезда = "";
                                                        Record.НазваниеПоезда = "";
                                                        Record.ВремяОтправления = new DateTime(DateTime.Now.Ticks);
                                                        Record.ВремяПрибытия = new DateTime(DateTime.Now.Ticks);

                                                        int Часы = 0;
                                                        int Минуты = 0;
                                                        DateTime ВремяСобытия = new DateTime(2000, 1, 1, 0, 0, 0);
                                                        DateTime ВремяПрибытия = new DateTime(2000, 1, 1, 0, 0, 0);
                                                        DateTime ВремяОтправления = new DateTime(2000, 1, 1, 0, 0, 0);


                                                        if (Config.ArrivalTime != "")
                                                        {
                                                            string[] SubStrings = Config.ArrivalTime.Split(':');

                                                            if (int.TryParse(SubStrings[0], out Часы) && int.TryParse(SubStrings[1], out Минуты))
                                                            {
                                                                ВремяПрибытия = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Часы, Минуты, 0);
                                                                Record.ВремяПрибытия = ВремяПрибытия;
                                                            }
                                                            else
                                                                continue;
                                                        }
                                                        else if (j == 0 || j == 1)
                                                            continue;

                                                        if (Config.DepartureTime != "")
                                                        {
                                                            string[] SubStrings = Config.DepartureTime.Split(':');

                                                            if (int.TryParse(SubStrings[0], out Часы) && int.TryParse(SubStrings[1], out Минуты))
                                                            {
                                                                ВремяОтправления = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Часы, Минуты, 0);
                                                                Record.ВремяОтправления = ВремяОтправления;
                                                            }
                                                            else
                                                                continue;
                                                        }
                                                        else if (j == 2 || j == 3 || j == 4)
                                                            continue;


                                                        Record.БитыАктивностиПолей = 0x00;
                                                        switch (j)
                                                        {
                                                            case 0: // Предупреждение о прибытии
                                                            case 1: // Предупреждение после прибытия
                                                                ВремяСобытия = ВремяПрибытия.AddMinutes(j == 0 ? -КоличествоМинут : КоличествоМинут);
                                                                break;

                                                            case 2: // Стоянка до отправления
                                                            case 3: // Предупреждение до отправления
                                                            case 4: // Предупреждение после отправления
                                                                ВремяСобытия = ВремяОтправления.AddMinutes(j == 4 ? КоличествоМинут : -КоличествоМинут);
                                                                Record.БитыАктивностиПолей |= 0x20;
                                                                break;
                                                        }

                                                        Record.ID = ID++;
                                                        Record.Время = ВремяСобытия;
                                                        List<string> МассивЗвуковыхСообщений = new List<string>();

                                                        if (Item.Message.Contains("НОМЕР ПУТИ"))
                                                            Record.БитыАктивностиПолей |= 0x01;

                                                        if (Item.Message.Contains("НУМЕРАЦИЯ СОСТАВА"))
                                                            Record.БитыАктивностиПолей |= 0x02;

                                                        if (string.IsNullOrEmpty(Config.ArrivalTime) == false)
                                                            Record.БитыАктивностиПолей |= 0x04;

                                                        if (string.IsNullOrEmpty(Config.DepartureTime) == false)
                                                            Record.БитыАктивностиПолей |= 0x10;

                                                        if ((Record.БитыАктивностиПолей & 0x14) == 0x14)
                                                        {
                                                            int ВремяСтоянки = 0;

                                                            if (Record.ВремяОтправления >= Record.ВремяПрибытия)
                                                                ВремяСтоянки = (Record.ВремяОтправления - Record.ВремяПрибытия).Minutes;
                                                            else
                                                                ВремяСтоянки = 1440 - Record.ВремяПрибытия.Hour * 60 - Record.ВремяПрибытия.Minute + Record.ВремяОтправления.Hour * 60 + Record.ВремяОтправления.Minute;

                                                            Record.ВремяСтоянки = (uint)ВремяСтоянки;
                                                            Record.БитыАктивностиПолей |= 0x08;
                                                        }
                                                        else
                                                            Record.ВремяСтоянки = 0;

                                                        string[] ФайлыЧасов = new string[] { "В 00 часов", "В 01 час", "В 02 часа", "В 03 часа", "В 04 часа", "В 05 часов", "В 06 часов", "В 07 часов", 
                                                                                        "В 08 часов", "В 09 часов", "В 10 часов", "В 11 часов", "В 12 часов", "В 13 часов", "В 14 часов", "В 15 часов", 
                                                                                        "В 16 часов", "В 17 часов", "В 18 часов", "В 19 часов", "В 20 часов", "В 21 час", "В 22 часа", "В 23 часа" };

                                                        string[] ФайлыМинут = new string[] { "00 минут", "01 минута", "02 минуты", "03 минуты", "04 минуты", "05 минут", "06 минут", "07 минут", "08 минут",
                                                                                             "09 минут", "10 минут", "11 минут", "12 минут", "13 минут", "14 минут", "15 минут", "16 минут", "17 минут", 
                                                                                             "18 минут", "19 минут", "20 минут", "21 минута", "22 минуты", "23 минуты", "24 минуты", "25 минут", "26 минут",
                                                                                             "27 минут", "28 минут", "29 минут", "30 минут", "31 минута", "32 минуты", "33 минуты", "34 минуты", "35 минут",
                                                                                             "36 минут", "37 минут", "38 минут", "39 минут", "40 минут", "41 минута", "42 минуты", "43 минуты", "44 минуты",
                                                                                             "45 минут", "46 минут", "47 минут", "48 минут", "49 минут", "50 минут", "51 минута", "52 минуты", "53 минуты",
                                                                                             "54 минуты", "55 минут", "56 минут", "57 минут", "58 минут", "59 минут" };

                                                        bool НаличиеВШаблонеНомераПути = false;
                                                        bool НаличиеВШаблонеНумерацииСостава = false;

                                                        string[] ШаблоныОповещения = Item.Message.Split('|');
                                                        foreach (string шаблон in ШаблоныОповещения)
                                                        {
                                                            switch (шаблон)
                                                            {
                                                                case "НОМЕР ПОЕЗДА":
                                                                    if (Program.TrainNumbersFolder.Contains(Config.Num))
                                                                    {
                                                                        Record.НомерПоезда = Config.Num + ": ";
                                                                        Record.НазваниеПоезда = Config.Name;
                                                                        МассивЗвуковыхСообщений.Add(Config.Num);
                                                                    }
                                                                    break;

                                                                case "НОМЕР ПУТИ":
                                                                    МассивЗвуковыхСообщений.Add("НОМЕР ПУТИ");
                                                                    НаличиеВШаблонеНомераПути = true;
                                                                    break;

                                                                case "ВРЕМЯ ПРИБЫТИЯ":
                                                                    МассивЗвуковыхСообщений.Add(ФайлыЧасов[ВремяПрибытия.Hour]);
                                                                    МассивЗвуковыхСообщений.Add(ФайлыМинут[ВремяПрибытия.Minute]);
                                                                    break;

                                                                case "ВРЕМЯ СТОЯНКИ":
                                                                    МассивЗвуковыхСообщений.Add("ВРЕМЯ СТОЯНКИ");
                                                                    break;

                                                                case "ВРЕМЯ ОТПРАВЛЕНИЯ":
                                                                    МассивЗвуковыхСообщений.Add(ФайлыЧасов[ВремяОтправления.Hour]);
                                                                    МассивЗвуковыхСообщений.Add(ФайлыМинут[ВремяОтправления.Minute]);
                                                                    break;

                                                                case "НУМЕРАЦИЯ СОСТАВА":
                                                                    МассивЗвуковыхСообщений.Add("НУМЕРАЦИЯ СОСТАВА");
                                                                    НаличиеВШаблонеНумерацииСостава = true;
                                                                    break;

                                                                default:
                                                                    if (Program.FilesFolder.Contains(шаблон))
                                                                        МассивЗвуковыхСообщений.Add(шаблон);
                                                                    break;
                                                            }
                                                        }

                                                        Record.Длительность = 0f;
                                                        string[] ЗвуковыеФайлы = new string[МассивЗвуковыхСообщений.Count];
                                                        for (int k = 0; k < МассивЗвуковыхСообщений.Count; k++)
                                                        {
                                                            ЗвуковыеФайлы[k] = МассивЗвуковыхСообщений[k];

                                                            Player.PlayFile(Program.GetFileName(ЗвуковыеФайлы[k]));
                                                            Record.Длительность += Player.GetDuration();
                                                            Player.PlayFile(string.Empty);
                                                        }

                                                        Record.НомерПути = Config.TrainPathNumber;
                                                        Record.НумерацияПоезда = Config.TrainPathDirection;
                                                        Record.ИменаФайлов = ЗвуковыеФайлы;

                                                        if (!НаличиеВШаблонеНомераПути || ((Config.TrainPathNumber > 0) && (Config.TrainPathNumber <= 10)))
                                                            if (!НаличиеВШаблонеНумерацииСостава || ((Config.TrainPathDirection > 0) && (Config.TrainPathDirection <= 2)))
                                                                Record.ТипСообщения = SoundRecordType.ДвижениеПоезда;

                                                        string Key = Record.Время.ToString("HH:mm:ss");
                                                        string[] SubKeys = Key.Split(':');
                                                        if (SubKeys[0].Length == 1)
                                                            Key = "0" + Key;

                                                        Record.Примечание = Config.Примечание;
                                                        Record.ОтображениеВТаблицах = Config.ShowInPanels;

                                                        if (SoundRecords.ContainsKey(Key) == false)
                                                            SoundRecords.Add(Key, Record);
                                                    }
                                                }
                                            }
                                        }
                                }
                        }
                    }
                }
            }
            #endregion
            
            #region Создание статических звуковых файлов
            foreach (SoundConfigurationRecord Config in SoundConfiguration.SoundConfigurationRecords)
            {
                SoundRecord Record;
                Record.Состояние = SoundRecordStatus.ОжиданиеВоспроизведения;
                Record.ТипСообщения = SoundRecordType.Обычное;
                Record.ИменаФайлов = new string[1];

                Record.НомерПути = 0;
                Record.НазваниеПоезда = "";
                Record.НумерацияПоезда = 0;
                Record.ВремяПрибытия = new DateTime(2000, 1, 1, 0, 0, 0);
                Record.ВремяСтоянки = 0;
                Record.ВремяОтправления = new DateTime(2000, 1, 1, 0, 0, 0);
                Record.БитыАктивностиПолей = 0x00;
                Record.НомерПоезда = "";
                Record.ШаблонВоспроизведенияПути = 0;

                if (Config.Enable == true)
                {
                    if (Config.EnablePeriodic == true)
                    {
                        Record.Описание = Config.Name;
                        Record.ИменаФайлов[0] = StaticSoundForm.GetFilePath(Record.Описание);

                        if (Record.ИменаФайлов[0] == string.Empty)
                            continue;


                        Player.PlayFile(Record.ИменаФайлов[0]);
                        Record.Длительность = Player.GetDuration();
                        Player.PlayFile(string.Empty);

                        if (Record.Длительность == 0)
                            continue;


                        string[] Times = Config.MessagePeriodic.Split(',');
                        if (Times.Length != 3)
                            continue;

                        try
                        {
                            DateTime НачалоИнтервала = DateTime.Parse(Times[0]), КонецИнтервала = DateTime.Parse(Times[1]);
                            int Интервал = int.Parse(Times[2]);

                            while (НачалоИнтервала < КонецИнтервала)
                            {
                                Record.ID = ID++;
                                Record.Время = НачалоИнтервала;

                                foreach (var СуществующиеИнтервалы in SoundRecords)
                                    if ((НачалоИнтервала >= СуществующиеИнтервалы.Value.Время) && (НачалоИнтервала <= СуществующиеИнтервалы.Value.Время.AddSeconds(СуществующиеИнтервалы.Value.Длительность + SoundConfiguration.МинимальныйИнтервалМеждуОповещениемСекунд)))
                                        goto МеткаВремени0000;

                                string Key = Record.Время.ToString("HH:mm:ss");
                                string[] SubKeys = Key.Split(':');
                                if (SubKeys[0].Length == 1)
                                    Key = "0" + Key;

                                Record.ОтображениеВТаблицах = 0;
                                Record.Примечание = "";

                                SoundRecords.Add(Key, Record);

                            МеткаВремени0000:
                                НачалоИнтервала = НачалоИнтервала.AddMinutes(Интервал);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    if (Config.EnableSingle == true)
                    {
                        Record.Описание = Config.Name;
                        Record.ИменаФайлов[0] = StaticSoundForm.GetFilePath(Record.Описание);

                        if (Record.ИменаФайлов[0] == string.Empty)
                            continue;


                        Player.PlayFile(Record.ИменаФайлов[0]);
                        Record.Длительность = Player.GetDuration();
                        Player.PlayFile(string.Empty);

                        if (Record.Длительность == 0)
                            continue;


                        string[] Times = Config.MessageSingle.Split(',');

                        foreach (string time in Times)
                        {
                            try
                            {
                                Record.ID = ID++;
                                Record.Время = DateTime.Parse(time);
                                Record.Примечание = "";
                                Record.ОтображениеВТаблицах = 0;

                                foreach (var СуществующиеИнтервалы in SoundRecords)
                                    if ((Record.Время >= СуществующиеИнтервалы.Value.Время) && (Record.Время <= СуществующиеИнтервалы.Value.Время.AddSeconds(СуществующиеИнтервалы.Value.Длительность + SoundConfiguration.МинимальныйИнтервалМеждуОповещениемСекунд)))
                                        goto МеткаВремени0001;

                                string[] TimeParts = time.Split(':');
                                if (TimeParts[0].Length == 1)
                                    SoundRecords.Add("0" + time, Record);
                                else
                                    SoundRecords.Add(time, Record);

                            МеткаВремени0001:
                                int i = 0;
                                i++;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
            #endregion

            SoundRecords.OrderBy(key => key.Value);
        }

        // Отображение сформированного списка воспроизведения в таблицу
        private void ОбновитьСписокЗвуковыхСообщенийВТаблице()
        {
            ОбновлениеСписка = true;

            listView1.Items.Clear();

            for (int i = 0; i < SoundRecords.Count; i++)
            {
                var Данные = SoundRecords.ElementAt(i);

                ListViewItem lvi = new ListViewItem(new string[] { Данные.Value.ID.ToString(), Данные.Value.Время.ToString("HH:mm:ss"), (Данные.Value.Длительность / 60).ToString("00") + ":" + (Данные.Value.Длительность % 60).ToString("00"), Данные.Value.НомерПоезда + Данные.Value.Описание });
                lvi.Tag = Данные.Value.ID;
                lvi.Checked = Данные.Value.Состояние == SoundRecordStatus.Выключена ? false : true;
                this.listView1.Items.Add(lvi);
            }

            ОбновлениеСписка = false;
        }

        // Раскрасить записи в соответствии с состоянием
        private void ОбновитьСостояниеЗаписейТаблицы()
        {
            for (int item = 0; item < this.listView1.Items.Count; item++)
            {
                if (item <= SoundRecords.Count)
                {
                    try
                    {
                        string Key = this.listView1.Items[item].SubItems[1].Text;

                        if (SoundRecords.Keys.Contains(Key) == true)
                        {
                            SoundRecord Данные = SoundRecords[Key];
                            switch (Данные.Состояние)
                            {
                                default:
                                case SoundRecordStatus.Выключена:
                                    this.listView1.Items[item].BackColor = Color.LightGray;
                                    break;

                                case SoundRecordStatus.ОжиданиеВоспроизведения:
                                    switch (Данные.ТипСообщения)
                                    {
                                        default:
                                        case SoundRecordType.Обычное:
                                            this.listView1.Items[item].BackColor = Color.LightGreen;
                                            break;

                                        case SoundRecordType.ДвижениеПоезда:
                                            this.listView1.Items[item].BackColor = Color.GreenYellow;
                                            break;

                                        case SoundRecordType.ДвижениеПоездаНеПодтвержденное:
                                            this.listView1.Items[item].BackColor = Color.Yellow;
                                            break;

                                        case SoundRecordType.Важное:
                                            this.listView1.Items[item].BackColor = Color.Red;
                                            break;

                                        case SoundRecordType.Предупредительное:
                                            this.listView1.Items[item].BackColor = Color.Orange;
                                            break;
                                    }
                                    break;

                                case SoundRecordStatus.Воспроизведение:
                                    this.listView1.Items[item].BackColor = Color.LightBlue;
                                    break;

                                case SoundRecordStatus.Воспроизведена:
                                    this.listView1.Items[item].BackColor = Color.LightGray;
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        // Определение композиций для запуска в данный момент времени
        private void ОпределитьКомпозициюДляЗапуска()
        {
            for (int item = 0; item < this.listView1.Items.Count; item++)
            {
                if (item <= SoundRecords.Count)
                {
                    try
                    {
                        string Key = this.listView1.Items[item].SubItems[1].Text;

                        if (SoundRecords.Keys.Contains(Key) == true)
                        {
                            SoundRecord Данные = SoundRecords[Key];

                            if (this.listView1.Items[item].Checked && (Данные.ТипСообщения == SoundRecordType.Важное || (Данные.ТипСообщения == SoundRecordType.ДвижениеПоезда && ВоспроизводитьДвижениеПоездов == true) || Данные.ТипСообщения == SoundRecordType.Обычное || Данные.ТипСообщения == SoundRecordType.Предупредительное))
                            {
                                DateTime НачалоИнтервала = Данные.Время;
                                DateTime КонецИнтервала = НачалоИнтервала.AddSeconds(Данные.Длительность);

                                if ((DateTime.Now >= НачалоИнтервала.AddSeconds(-25)) && (DateTime.Now < НачалоИнтервала.AddSeconds(-20)))
                                {
                                    if (ОкноПредупреждения != null)
                                        ОкноПредупреждения = null;
                                }
                                else if ((DateTime.Now >= НачалоИнтервала.AddSeconds(-20)) && (DateTime.Now < НачалоИнтервала))
                                {
                                    if ((Данные.Состояние == SoundRecordStatus.ОжиданиеВоспроизведения) && (РазрешениеРаботы == true) && (ОкноПредупреждения == null))
                                    {
                                        ОкноПредупреждения = new Предупреждение(this.listView1.Items[item]);
                                        ОкноПредупреждения.StartPosition = FormStartPosition.Manual;
                                        ОкноПредупреждения.Location = new Point(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 301, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 250);
                                        ОкноПредупреждения.Show();
                                        ОкноПредупреждения.WindowState = FormWindowState.Normal;
                                    }

                                    if ((ОкноПредупреждения != null) && this.listView1.Items[item].Checked)
                                        Данные.Состояние = SoundRecordStatus.ОжиданиеВоспроизведения;
                                }
                                else if ((DateTime.Now >= НачалоИнтервала) && (DateTime.Now < КонецИнтервала))
                                {
                                    if ((Данные.Состояние == SoundRecordStatus.ОжиданиеВоспроизведения) && (РазрешениеРаботы == true))
                                    {
                                        for (int i = 0; i < this.listView1.Items.Count; i++)
                                        {
                                            this.listView1.Items[i].Focused = false;
                                            this.listView1.Items[i].Selected = false;
                                        }

                                        this.listView1.Items[item].Focused = true;
                                        this.listView1.Items[item].Selected = true;
                                        btnВоспроизвести_Click(null, null);
                                    }

                                    Данные.Состояние = SoundRecordStatus.Воспроизведение;
                                }
                                else if (DateTime.Now >= КонецИнтервала)
                                {
                                    Данные.Состояние = SoundRecordStatus.Воспроизведена;
                                }
                                else
                                {
                                    Данные.Состояние = SoundRecordStatus.ОжиданиеВоспроизведения;
                                }
                            }
                            else if (this.listView1.Items[item].Checked && (Данные.ТипСообщения == SoundRecordType.ДвижениеПоездаНеПодтвержденное))
                                Данные.Состояние = SoundRecordStatus.ОжиданиеВоспроизведения;
                            else
                                Данные.Состояние = SoundRecordStatus.Выключена;

                            SoundRecords[Key] = Данные;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        // Формирование очереди воспроизведения звуковых файлов
        private void ОбработкаЗвуковогоПотка()
        {
            if (ТекущаяДата != DateTime.Now.Day)
            {
                ТекущаяДата = DateTime.Now.Day;
                if (cBАвтоматическаяГенерация.Checked == true)
                    btnОбновитьСписок_Click(null, null);
            }

            ОпределитьКомпозициюДляЗапуска();
            ОбновитьСостояниеЗаписейТаблицы();

            SoundFileStatus status = Player.GetFileStatus();

            if (btnВоспроизвести.Text == "Остановить")
                if ((status != SoundFileStatus.Playing) && ((ВоспроизводимыеФайлы == null) || ((ВоспроизводимыеФайлы != null) && (НомерВоспроизводимогоТрека >= ВоспроизводимыеФайлы.Length))))
                    btnВоспроизвести.Text = "Воспроизвести выбранную запись";


            if (status == SoundFileStatus.Playing)
            {
                int CurrentPosition = Player.GetCurrentPosition() + (int)ВремяВоспроизведенияПроигранныхФайлов;
                Player_Label.Text = (CurrentPosition / 60).ToString() + ":" + (CurrentPosition % 60).ToString("00") + " / " + (ОбщееВремяВоспроизведения / 60).ToString("0") + ":" + (ОбщееВремяВоспроизведения % 60).ToString("00");
            }
            else
            {
                if (true)//(status == SoundFileStatus.Paused) || (status == SoundFileStatus.Stop))
                {
                    if ((ВоспроизводимыеФайлы != null) && (ВоспроизводимыеФайлы.Length > 0) && (НомерВоспроизводимогоТрека < ВоспроизводимыеФайлы.Length))
                    {
                        ВремяВоспроизведенияПроигранныхФайлов += Player.GetDuration();
                        string НазваниеФайла = ВоспроизводимыеФайлы[НомерВоспроизводимогоТрека++];
                        if (НазваниеФайла.Contains(".wav") == false)
                            НазваниеФайла = Program.GetFileName(НазваниеФайла);

                        if (Player.PlayFile(НазваниеФайла) == true)
                        {
                            Player.SetVolume(Volume);
                            btnВоспроизвести.Text = "Остановить";
                        }
                    }

                    int CurrentPosition = Player.GetCurrentPosition() + (int)ВремяВоспроизведенияПроигранныхФайлов;
                    Player_Label.Text = (CurrentPosition / 60).ToString() + ":" + (CurrentPosition % 60).ToString("00") + " / " + (ОбщееВремяВоспроизведения / 60).ToString("0") + ":" + (ОбщееВремяВоспроизведения % 60).ToString("00");
                }
            }
        }

        // Воспроизведение выбраной в таблице записи
        private void btnВоспроизвести_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

            if (btnВоспроизвести.Text == "Остановить")
            {
                ВоспроизводимыеФайлы = null;
                Player.PlayFile(string.Empty);
                btnВоспроизвести.Text = "Воспроизвести выбранную запись";
                return;
            }

            foreach (int item in sic)
            {
                string Key = this.listView1.Items[item].SubItems[1].Text;

                if (SoundRecords.Keys.Contains(Key) == true)
                {
                    SoundRecord Данные = SoundRecords[Key];
                    if (Данные.ИменаФайлов == null)
                        return;

                    НомерВоспроизводимогоТрека = 0;
                    ВоспроизводимыеФайлы = new string[Данные.ИменаФайлов.Length];
                    for (int i = 0; i < Данные.ИменаФайлов.Length; i++)
                        ВоспроизводимыеФайлы[i] = Данные.ИменаФайлов[i];


                    float ОбщаяДлинаВоспроизведения = 0;
                    for (int i = 0; i < ВоспроизводимыеФайлы.Length; i++)
                    {
                        string FileName = ВоспроизводимыеФайлы[i];

                        string[] НазваниеФайловПутей = new string[] { "", 
                                                                    "На 1ый путь", "На 2ой путь", "На 3ий путь", "На 4ый путь", "На 5ый путь", "На 6ой путь", "На 7ой путь", "На 8ой путь", "На 9ый путь", "На 10ый путь",
                                                                    "На 1ом пути", "На 2ом пути", "На 3ем пути", "На 4ом пути", "На 5ом пути", "На 6ом пути", "На 7ом пути", "На 8ом пути", "На 9ом пути", "На 10ом пути", 
                                                                    "С 1ого пути", "С 2ого пути", "С 3его пути", "С 4ого пути", "С 5ого пути", "С 6ого пути", "С 7ого пути", "С 8ого пути", "С 9ого пути", "С 10ого пути" };

                        if (FileName == "НОМЕР ПУТИ")
                            if ((Данные.НомерПути > 0) && (Данные.НомерПути <= 10))
                            {
                                if ((Данные.ШаблонВоспроизведенияПути == 0) || (Данные.ШаблонВоспроизведенияПути == 1))
                                    FileName = ВоспроизводимыеФайлы[i] = НазваниеФайловПутей[Данные.НомерПути];
                                else if ((Данные.ШаблонВоспроизведенияПути == 2) || (Данные.ШаблонВоспроизведенияПути == 3))
                                    FileName = ВоспроизводимыеФайлы[i] = НазваниеФайловПутей[Данные.НомерПути + (Данные.ШаблонВоспроизведенияПути - 1) * 10];
                            }

                        string[] НазваниеФайловНумерацииПутей = new string[] { "", "Нумерация с головы", "Нумерация с хвоста" };
                        if (FileName == "НУМЕРАЦИЯ СОСТАВА")
                            if ((Данные.НумерацияПоезда > 0) && (Данные.НумерацияПоезда <= 2))
                                FileName = ВоспроизводимыеФайлы[i] = НазваниеФайловНумерацииПутей[Данные.НумерацияПоезда];

                        string[] ФайлыМинут = new string[] { "00 минут", "01 минута", "02 минуты", "03 минуты", "04 минуты", "05 минут", "06 минут", "07 минут", "08 минут",
                            "09 минут", "10 минут", "11 минут", "12 минут", "13 минут", "14 минут", "15 минут", "16 минут", "17 минут", 
                            "18 минут", "19 минут", "20 минут", "21 минута", "22 минуты", "23 минуты", "24 минуты", "25 минут", "26 минут",
                            "27 минут", "28 минут", "29 минут", "30 минут", "31 минута", "32 минуты", "33 минуты", "34 минуты", "35 минут",
                            "36 минут", "37 минут", "38 минут", "39 минут", "40 минут", "41 минута", "42 минуты", "43 минуты", "44 минуты",
                            "45 минут", "46 минут", "47 минут", "48 минут", "49 минут", "50 минут", "51 минута", "52 минуты", "53 минуты",
                            "54 минуты", "55 минут", "56 минут", "57 минут", "58 минут", "59 минут" };
                        if (FileName == "ВРЕМЯ СТОЯНКИ")
                            if ((Данные.ВремяСтоянки > 0) && (Данные.ВремяСтоянки <= 59))
                                FileName = ВоспроизводимыеФайлы[i] = ФайлыМинут[Данные.ВремяСтоянки];

                        if (FileName == "на посадку проходите через тоннель")
                            if (((Данные.БитыАктивностиПолей & 0x10) != 0x00) && (Данные.НомерПути == 0x01))
                                FileName = ВоспроизводимыеФайлы[i] = "на посадку проходите через вестибюль";

                        if (FileName.Contains(".wav") == false)
                            FileName = Program.GetFileName(FileName);

                        Player.PlayFile(FileName);
                        ОбщаяДлинаВоспроизведения += Player.GetDuration();
                        Player.PlayFile(string.Empty);
                    }

                    ВремяВоспроизведенияПроигранныхФайлов = 0f;
                    ОбщееВремяВоспроизведения = (int)ОбщаяДлинаВоспроизведения;


                    if (ВоспроизводимыеФайлы.Length > 0)
                    {
                        string НазваниеФайла = ВоспроизводимыеФайлы[НомерВоспроизводимогоТрека++];
                        if (НазваниеФайла.Contains(".wav") == false)
                            НазваниеФайла = Program.GetFileName(НазваниеФайла);

                        if (Player.PlayFile(НазваниеФайла) == true)
                        {
                            Player.SetVolume(Volume);
                            btnВоспроизвести.Text = "Остановить";
                        }
                    }
                }
            }
        }

        // Управление регулятором громкости
        private void tBРегуляторГромкости_Scroll(object sender, EventArgs e)
        {
            Volume = tBРегуляторГромкости.Value;
            Player.SetVolume(Volume);
        }

        // Обработка закрытия основной формы
        private void MainWindowForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (myMainForm == this)
                myMainForm = null;
        }

        // Обработка отключения сообщения
        public void ОтключитьСообщение(ListViewItem Item)
        {
            Item.Checked = false;
            ОбновитьСостояниеЗаписейТаблицы();
        }

        // Обработка двойного нажатия на сообщение (вызов формы сообщения)
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

                
                foreach (int item in sic)
                {
                    if (item <= SoundRecords.Count)
                    {
                        string Key = this.listView1.Items[item].SubItems[1].Text;

                        if (SoundRecords.Keys.Contains(Key) == true)
                        {
                            SoundRecord Данные = SoundRecords[Key];

                            КарточкаДвиженияПоезда Карточка = new КарточкаДвиженияПоезда(Данные);
                            if (Карточка.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                bool ПрименитьКоВсемСообщениям = Карточка.ПрименитьКоВсемСообщениям;

                                Данные = Карточка.ПолучитьИзмененнуюКарточку();
                                Данные.ТипСообщения = SoundRecordType.ДвижениеПоезда;

                                string НомерПоезда = Данные.НомерПоезда;
                                byte НомерПути = Данные.НомерПути;


                                SoundRecords[Key] = Данные;
                                string time = Данные.Время.ToString("HH:mm:ss");
                                
                                string[] TimeParts = time.Split(':');
                                if (TimeParts[0].Length == 1)
                                    time = "0" + time;


                                if (this.listView1.Items[item].SubItems[1].Text != time)
                                    if (SoundRecords.ContainsKey(time) == false)
                                    {
                                        this.listView1.Items[item].SubItems[1].Text = time;
                                        SoundRecords.Remove(Key);
                                        SoundRecords.Add(time, Данные);
                                    }


                                if (ПрименитьКоВсемСообщениям == true)
                                {
                                    for (int i = 0; i < SoundRecords.Count; i++)
                                    {
                                        var ВременнаяПара = SoundRecords.ElementAt(i);
                                        if (ВременнаяПара.Value.НомерПоезда == НомерПоезда)
                                        {
                                            string Ключ = ВременнаяПара.Key;
                                            SoundRecord НовыеДанные = ВременнаяПара.Value;
                                            НовыеДанные.ТипСообщения = SoundRecordType.ДвижениеПоезда;

                                            НовыеДанные.НомерПути = НомерПути;
                                            НовыеДанные.ВремяПрибытия = Данные.ВремяПрибытия;
                                            НовыеДанные.ВремяОтправления = Данные.ВремяОтправления;
                                            НовыеДанные.ВремяСтоянки = Данные.ВремяСтоянки;
                                            SoundRecords[Ключ] = НовыеДанные;
                                        }
                                    }
                                }

                                ОбновитьСостояниеЗаписейТаблицы();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Блокировка/разблокировка сообщения при нажатии на CheckBox
        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (ОбновлениеСписка)
                return;

            if ((sender as ListView).PointToClient(Cursor.Position).X > 22)
                e.NewValue = e.CurrentValue;
        }

        // Блокировка/разблокирование сообщения при нажатии на CheckBox
        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ОбновитьСостояниеЗаписейТаблицы();
        }

        private void cB_ВоспроизведениеДвиженияПоездов_CheckedChanged(object sender, EventArgs e)
        {
            ВоспроизводитьДвижениеПоездов = cB_ВоспроизведениеДвиженияПоездов.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ОкноРасписания окноРасписания = new ОкноРасписания();
            окноРасписания.Show(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ОкноПрибытияОтправления окноПибытияОтправления = new ОкноПрибытияОтправления();
            окноПибытияОтправления.Show(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ОкноРасписания2 окноРасписания = new ОкноРасписания2();
            окноРасписания.Show(this);
        }

        protected override void OnClosed(EventArgs e)
        {
            DispouseCisClientIsConnectRx.Dispose();
            CisClient.Stop();
            base.OnClosed(e);
        }
    }
}
