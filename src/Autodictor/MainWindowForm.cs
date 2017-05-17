using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using CommunicationDevices.Behavior.BindingBehavior.ToGeneralSchedule;
using CommunicationDevices.Behavior.BindingBehavior.ToPath;
using CommunicationDevices.ClientWCF;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;
using MainExample.Entites;
using MainExample.Extension;
using MainExample.Infrastructure;


namespace MainExample
{
    public enum SoundRecordStatus { Выключена = 0, ОжиданиеВоспроизведения, Воспроизведение, Воспроизведена };
    public enum TableRecordStatus { Выключена = 0, ОжиданиеОтображения, Отображение, Обновление, Очистка };
    public enum SoundRecordType { Обычное = 0, ДвижениеПоезда, ДвижениеПоездаНеПодтвержденное, Предупредительное, Важное };
    public enum PathPermissionType { ИзФайлаНастроек = 0, Отображать, НеОтображать };

    public struct SoundRecord
    {
        public int ID;
        public string НомерПоезда;
        public string НазваниеПоезда;
        public string СтанцияОтправления;
        public string СтанцияНазначения;
        public DateTime Время;
        public DateTime ВремяПрибытия;
        public string Дополнение;                            //свободная переменная для ввода 
        public bool ИспользоватьДополнение;
        public uint ВремяСтоянки;
        public DateTime ВремяОтправления;
        public string ДниСледования;
        public bool Активность;
        public string ШаблонВоспроизведенияСообщений;
        public byte НумерацияПоезда;
        public string НомерПути;
        public ТипПоезда ТипПоезда;
        public string Примечание;                             //С остановками....
        public string Описание;
        public SoundRecordStatus Состояние;
        public SoundRecordType ТипСообщения;
        public byte БитыАктивностиПолей;
        public string[] НазванияТабло;                      
        public TableRecordStatus СостояниеОтображения;      
        public PathPermissionType РазрешениеНаОтображениеПути;
        public string[] ИменаФайлов;
        public byte КоличествоПовторений;
        public List<СостояниеФормируемогоСообщенияИШаблон> СписокФормируемыхСообщений;
        public byte СостояниеКарточки;
        public string ОписаниеСостоянияКарточки;
        public byte БитыНештатныхСитуаций; // бит 0 - Отмена, бит 1 - задержка прибытия, бит 2 - задержка отправления
        public uint ТаймерПовторения;
    };

    public struct СтатическоеСообщение
    {
        public int ID;
        public DateTime Время;
        public string НазваниеКомпозиции;
        public string ОписаниеКомпозиции;
        public SoundRecordStatus СостояниеВоспроизведения;
        public bool Активность;
    };

    public struct СостояниеФормируемогоСообщенияИШаблон
    {
        public bool Активность;
        public bool Воспроизведен;
        public int ВремяСмещения;
        public string НазваниеШаблона;
        public string Шаблон;
        public int ПривязкаКВремени;
        public List<NotificationLanguage> ЯзыкиОповещения;
    };

    public struct ОписаниеСобытия
    {
        public DateTime Время;
        public string Описание;
        public byte НомерСписка;            // 0 - Динамические сообщения, 1 - статические звуковые сообщения
        public string Ключ;
        public byte СостояниеСтроки;        // 0 - Выключена, 1 - движение поезда, 2 - статическое сообщение, 3 - аварийное сообщение, 4 - воспроизведение
        public string ШаблонИлиСообщение;   //текст стат. сообщения, или номер шаблона в динам. сообщении
    };


    public partial class MainWindowForm : Form
    {
        private bool РазрешениеРаботы = false;

        public static SortedDictionary<string, SoundRecord> SoundRecords = new SortedDictionary<string, SoundRecord>();
        public static SortedDictionary<string, SoundRecord> SoundRecordsOld = new SortedDictionary<string, SoundRecord>();

        public static SortedDictionary<string, СтатическоеСообщение> СтатическиеЗвуковыеСообщения = new SortedDictionary<string, СтатическоеСообщение>();

        public static SortedDictionary<string, ОписаниеСобытия> СписокБлижайшихСобытий = new SortedDictionary<string, ОписаниеСобытия>();


        private static int ID = 1;
        private bool ОбновлениеСписка = false;

        public static MainWindowForm myMainForm = null;

        public static List<ВоспроизводимоеСообщение> ОчередьВоспроизводимыхЗвуковыхСообщений = new List<ВоспроизводимоеСообщение>();
        private int VisibleMode = 0;

        public CisClient CisClient { get; }
        public static IEnumerable<IBinding2PathBehavior> Binding2PathBehaviors { get; set; }
        public static IEnumerable<IBinding2GeneralSchedule> Binding2GeneralScheduleBehaviors { get; set; }
        public Device SoundChanelManagment { get; }
        public IDisposable DispouseCisClientIsConnectRx { get; set; }


        public int ВремяЗадержкиМеждуСообщениями = 0;
        private int ТекущаяСекунда = 0;
        public static bool ФлагОбновитьСписокЗвуковыхСообщений = false;

        public static byte РаботаПоНомеруДняНедели = 7;
        public static bool ФлагОбновитьСписокЖелезнодорожныхСообщенийПоДнюНедели = false;

        public static bool ФлагОбновитьСписокЖелезнодорожныхСообщенийВТаблице = false;

        private string КлючВыбранныйМеню = "";

        private uint _tickCounter = 0;

        private ToolStripMenuItem[] СписокПолейПути;


        // Конструктор
        public MainWindowForm(CisClient cisClient, IEnumerable<IBinding2PathBehavior> binding2PathBehaviors, IEnumerable<IBinding2GeneralSchedule> binding2GeneralScheduleBehaviors, Device soundChanelManagment)
        {
            if (myMainForm != null)
                return;

            myMainForm = this;

            InitializeComponent();

            tableLayoutPanel1.Visible = false;

            CisClient = cisClient;

            Binding2PathBehaviors = binding2PathBehaviors;
            Binding2GeneralScheduleBehaviors = binding2GeneralScheduleBehaviors;
            SoundChanelManagment = soundChanelManagment;

            MainForm.Воспроизвести.Click += new System.EventHandler(this.btnВоспроизвести_Click);
            MainForm.Включить.Click += new System.EventHandler(this.btnБлокировка_Click);
            MainForm.ОбновитьСписок.Click += new System.EventHandler(this.btnОбновитьСписок_Click);

            СписокПолейПути = new ToolStripMenuItem[] { путь0ToolStripMenuItem, путь1ToolStripMenuItem, путь2ToolStripMenuItem, путь3ToolStripMenuItem, путь4ToolStripMenuItem, путь5ToolStripMenuItem, путь6ToolStripMenuItem, путь7ToolStripMenuItem, путь8ToolStripMenuItem, путь9ToolStripMenuItem, путь10ToolStripMenuItem, путь11ToolStripMenuItem, путь12ToolStripMenuItem, путь13ToolStripMenuItem, путь14ToolStripMenuItem, путь15ToolStripMenuItem, путь16ToolStripMenuItem, путь17ToolStripMenuItem, путь18ToolStripMenuItem, путь19ToolStripMenuItem, путь20ToolStripMenuItem, путь21ToolStripMenuItem, путь22ToolStripMenuItem, путь23ToolStripMenuItem, путь24ToolStripMenuItem, путь25ToolStripMenuItem };


            if (CisClient.IsConnect)
            {
                MainForm.СвязьСЦис.Text = "ЦИС на связи";
                MainForm.СвязьСЦис.BackColor = Color.LightGreen;
            }
            else
            {
                MainForm.СвязьСЦис.Text = "ЦИС НЕ на связи";
                MainForm.СвязьСЦис.BackColor = Color.Orange;
            }

            DispouseCisClientIsConnectRx = CisClient.IsConnectChange.Subscribe(isConnect =>
             {
                 if (isConnect)
                 {
                     MainForm.СвязьСЦис.Text = "ЦИС на связи";
                     MainForm.СвязьСЦис.BackColor = Color.LightGreen;
                 }
                 else
                 {
                     MainForm.СвязьСЦис.Text = "ЦИС НЕ на связи";
                     MainForm.СвязьСЦис.BackColor = Color.Orange;
                 }
             });

            MainForm.Включить.BackColor = Color.Orange;
            Program.ЗаписьЛога("Системное сообщение", "Программный комплекс включен");
        }



        // Обработка таймера 100 мс для воспроизведения звуковых сообщений
        private void timer1_Tick(object sender, EventArgs e)
        {
            ОбработкаЗвуковогоПотка();
            ОпределитьИнформациюДляОтображенияНаТабло();

            if (VisibleMode != MainForm.VisibleStyle)
            {
                VisibleMode = MainForm.VisibleStyle;
                if (VisibleMode == 0)
                {
                    listView1.Visible = true;
                    tableLayoutPanel1.Visible = false;
                }
                else
                {
                    listView1.Visible = false;
                    tableLayoutPanel1.Visible = true;
                }
            }

            if (ФлагОбновитьСписокЗвуковыхСообщений == true)
            {
                ФлагОбновитьСписокЗвуковыхСообщений = false;
                ОбновитьСписокЗвуковыхСообщенийВТаблицеСтатическихСообщений();
            }

            if (ФлагОбновитьСписокЖелезнодорожныхСообщенийПоДнюНедели == true)
            {
                ФлагОбновитьСписокЖелезнодорожныхСообщенийПоДнюНедели = false;
                btnОбновитьСписок_Click(null, null);
            }

            if (Program.Настройки.РазрешениеАвтообновленияРасписания &&
                (DateTime.Now >= Program.Настройки.ВремяАвтообновленияРасписания) && (DateTime.Now <= Program.Настройки.ВремяАвтообновленияРасписания.AddMilliseconds(150)))
            {
                btnОбновитьСписок_Click(null, null);
            }

            if (ФлагОбновитьСписокЖелезнодорожныхСообщенийВТаблице == true)
            {
                ФлагОбновитьСписокЖелезнодорожныхСообщенийВТаблице = false;
                ОбновитьСписокЗвуковыхСообщенийВТаблице();
            }
        }



        // Обработка нажатия кнопки блокировки/разрешения работы
        private void btnБлокировка_Click(object sender, EventArgs e)
        {
            РазрешениеРаботы = !РазрешениеРаботы;

            if (РазрешениеРаботы == true)
            {
                MainForm.Включить.Text = "ОТКЛЮЧИТЬ";
                MainForm.Включить.BackColor = Color.LightGreen;
                Program.ЗаписьЛога("Действие оператора", "Работа разрешена");
            }
            else
            {
                MainForm.Включить.Text = "ВКЛЮЧИТЬ";
                MainForm.Включить.BackColor = Color.Orange;
                Program.ЗаписьЛога("Действие оператора", "Работа запрещена");
            }
        }



        // Обновление списка вопроизведения сообщений при нажатии кнопки на панели
        private void btnОбновитьСписок_Click(object sender, EventArgs e)
        {
            ОбновитьСписокЗвуковыхСообщений(sender, e);
            ОбновитьСписокЗвуковыхСообщенийВТаблице();
            ОбновитьСписокЗвуковыхСообщенийВТаблицеСтатическихСообщений();
            ОбновитьСостояниеЗаписейТаблицы();

            ОчиститьВсеТабло();
            ИнициализироватьВсеТабло();
        }



        private void ОчиститьВсеТабло()
        {
            foreach (var beh in Binding2PathBehaviors)
            {
                beh.InitializeDevicePathInfo();
            }
        }



        private void ИнициализироватьВсеТабло()
        {
            for (var i = 0; i < SoundRecords.Count; i++)
            {
                var данные = SoundRecords.ElementAt(i).Value;
                var номерПути = Program.ПолучитьНомерПути(данные.НомерПути);
                if (номерПути > 0)
                {
                    var key = SoundRecords.Keys.ElementAt(i);
                    данные.СостояниеОтображения = TableRecordStatus.Отображение;
                    данные.ТипСообщения = SoundRecordType.ДвижениеПоезда;
                    SoundRecords[key] = данные;
                    SendOnPathTable(SoundRecords[key]);
                }
            }
        }



        // Формирование списка воспроизведения
        public void ОбновитьСписокЗвуковыхСообщений(object sender, EventArgs e)
        {
            SoundRecords.Clear();
            SoundRecordsOld.Clear();
            СтатическиеЗвуковыеСообщения.Clear();

            СозданиеЗвуковыхФайловРасписанияЖдТранспорта(DateTime.Now, null);                                        // на тек. сутки
            СозданиеЗвуковыхФайловРасписанияЖдТранспорта(DateTime.Now.AddDays(1), hour => (hour >= 0 && hour <= 2)); // на след. сутки на 2 первых часа

            СозданиеСтатическихЗвуковыхФайлов();
        }



        private void СозданиеЗвуковыхФайловРасписанияЖдТранспорта(DateTime день, Func<int, bool> ограничениеВремениПоЧасам)
        {
            ID = 1;

            foreach (TrainTableRecord Config in TrainTable.TrainTableRecords)
            {
                SoundRecord Record;

                if (Config.Active == false && Program.Настройки.РазрешениеДобавленияЗаблокированныхПоездовВСписок == false)
                    continue;


                Record.НомерПоезда = Config.Num;
                Record.НазваниеПоезда = Config.Name;
                Record.Дополнение = Config.Addition;
                Record.ИспользоватьДополнение = !string.IsNullOrEmpty(Record.Дополнение);
                Record.СтанцияОтправления = "";
                Record.СтанцияНазначения = "";
                Record.ДниСледования = Config.Days;
                Record.Активность = Config.Active;
                Record.ШаблонВоспроизведенияСообщений = Config.SoundTemplates;
                Record.НомерПути = Config.TrainPathNumber;
                Record.НумерацияПоезда = Config.TrainPathDirection;
                Record.Примечание = Config.Примечание;
                Record.ТипПоезда = Config.ТипПоезда;
                Record.Состояние = SoundRecordStatus.ОжиданиеВоспроизведения;
                Record.ТипСообщения = SoundRecordType.ДвижениеПоездаНеПодтвержденное;
                Record.Описание = "";
                Record.КоличествоПовторений = 1;
                Record.СостояниеКарточки = 0;
                Record.ОписаниеСостоянияКарточки = "";
                Record.БитыНештатныхСитуаций = 0x00;
                Record.ТаймерПовторения = 0;
                Record.РазрешениеНаОтображениеПути = PathPermissionType.ИзФайлаНастроек;

                Record.ИменаФайлов = new string[0];




                ПланРасписанияПоезда планРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(Config.Days);
                if ((РаботаПоНомеруДняНедели == 7) || (планРасписанияПоезда.ПолучитьРежимРасписания() != РежимРасписанияДвиженияПоезда.ПоДням) || (Record.ТипПоезда == ТипПоезда.Пассажирский) || (Record.ТипПоезда == ТипПоезда.Скоростной) || (Record.ТипПоезда == ТипПоезда.Скорый))
                {
                    var активностьНаДень = планРасписанияПоезда.ПолучитьАктивностьДняДвижения((byte)(день.Month - 1), (byte)(день.Day - 1));
                    if (активностьНаДень == false)
                        continue;

                    if (ограничениеВремениПоЧасам != null)
                    {
                        DateTime времяПрибытия;
                        if (DateTime.TryParse(Config.ArrivalTime, out времяПрибытия))
                        {
                            if (!ограничениеВремениПоЧасам(времяПрибытия.Hour))
                                continue;
                        }

                        DateTime времяОтправления;
                        if (DateTime.TryParse(Config.DepartureTime, out времяОтправления))
                        {
                            if (!ограничениеВремениПоЧасам(времяОтправления.Hour))
                                continue;
                        }
                    }
                }
                else
                {
                    if (планРасписанияПоезда.ПолучитьАктивностьДняДвижения((byte)4, (byte)РаботаПоНомеруДняНедели) == false)
                        continue;
                }



                string[] НазванияСтанций = Config.Name.Split('-');
                if (НазванияСтанций.Length == 2)
                {
                    Record.СтанцияОтправления = НазванияСтанций[0].Trim();
                    Record.СтанцияНазначения = НазванияСтанций[1].Trim();
                }
                else if (НазванияСтанций.Length == 1)
                    Record.СтанцияНазначения = НазванияСтанций[0].Trim();

                int Часы = 0;
                int Минуты = 0;

                DateTime ВремяПрибытия = new DateTime(2000, 1, 1, 0, 0, 0);
                DateTime ВремяОтправления = new DateTime(2000, 1, 1, 0, 0, 0);

                Record.ВремяПрибытия = DateTime.Now;
                Record.ВремяОтправления = DateTime.Now;

                byte НомерСписка = 0x00;
                // бит 0 - задан номер пути
                // бит 1 - задана нумерация поезда
                // бит 2 - прибытие
                // бит 3 - стоянка
                // бит 4 - отправления

                if (Config.ArrivalTime != "")
                {
                    string[] SubStrings = Config.ArrivalTime.Split(':');

                    if (int.TryParse(SubStrings[0], out Часы) && int.TryParse(SubStrings[1], out Минуты))
                    {
                        ВремяПрибытия = new DateTime(день.Year, день.Month, день.Day, Часы, Минуты, 0);
                        Record.ВремяПрибытия = ВремяПрибытия;
                        НомерСписка |= 0x04;
                    }
                }

                if (Config.DepartureTime != "")
                {
                    string[] SubStrings = Config.DepartureTime.Split(':');

                    if (int.TryParse(SubStrings[0], out Часы) && int.TryParse(SubStrings[1], out Минуты))
                    {
                        ВремяОтправления = new DateTime(день.Year, день.Month, день.Day, Часы, Минуты, 0);
                        Record.ВремяОтправления = ВремяОтправления;
                        НомерСписка |= 0x10;
                    }
                }

                int ВремяСтоянки = 0;
                if (НомерСписка == 0x14)
                {
                    if (ВремяОтправления >= ВремяПрибытия)
                        ВремяСтоянки = (ВремяОтправления - ВремяПрибытия).Minutes;
                    else
                        ВремяСтоянки = 1440 - ВремяПрибытия.Hour * 60 - ВремяПрибытия.Minute + ВремяОтправления.Hour * 60 + ВремяОтправления.Minute;

                    НомерСписка |= 0x08;
                }

                Record.ВремяСтоянки = (uint)ВремяСтоянки;
                Record.БитыАктивностиПолей = НомерСписка;
                Record.БитыАктивностиПолей |= 0x03;

                Record.ID = ID++;

                byte НомерПути = (byte)(Program.НомераПутей.IndexOf(Record.НомерПути) + 1);
                Record.НазванияТабло = Record.НомерПути != "0" ? Binding2PathBehaviors.Select(beh => beh.GetDevicesName4Path(НомерПути)).Where(str => str != null).ToArray() : null;
                Record.СостояниеОтображения = TableRecordStatus.Выключена;


                if ((НомерСписка & 0x04) != 0x00)
                    Record.Время = Record.ВремяПрибытия;
                else
                    Record.Время = Record.ВремяОтправления;


                // Шаблоны оповещения
                Record.СписокФормируемыхСообщений = new List<СостояниеФормируемогоСообщенияИШаблон>();
                string[] ШаблонОповещения = Record.ШаблонВоспроизведенияСообщений.Split(':');
                int ПривязкаВремени = 0;
                if ((ШаблонОповещения.Length % 3) == 0)
                {
                    bool АктивностьШаблоновДанногоПоезда = false;
                    if (Record.ТипПоезда == ТипПоезда.Пассажирский && Program.Настройки.АвтФормСообщНаПассажирскийПоезд) АктивностьШаблоновДанногоПоезда = true;
                    if (Record.ТипПоезда == ТипПоезда.Пригородный && Program.Настройки.АвтФормСообщНаПригородныйЭлектропоезд) АктивностьШаблоновДанногоПоезда = true;
                    if (Record.ТипПоезда == ТипПоезда.Скоростной && Program.Настройки.АвтФормСообщНаСкоростнойПоезд) АктивностьШаблоновДанногоПоезда = true;
                    if (Record.ТипПоезда == ТипПоезда.Скорый && Program.Настройки.АвтФормСообщНаСкорыйПоезд) АктивностьШаблоновДанногоПоезда = true;
                    if (Record.ТипПоезда == ТипПоезда.Ласточка && Program.Настройки.АвтФормСообщНаЛасточку) АктивностьШаблоновДанногоПоезда = true;
                    if (Record.ТипПоезда == ТипПоезда.Фирменный && Program.Настройки.АвтФормСообщНаФирменный) АктивностьШаблоновДанногоПоезда = true;
                    if (Record.ТипПоезда == ТипПоезда.РЭКС && Program.Настройки.АвтФормСообщНаРЭКС) АктивностьШаблоновДанногоПоезда = true;

                    for (int i = 0; i < ШаблонОповещения.Length / 3; i++)
                    {
                        bool НаличиеШаблона = false;
                        string Шаблон = "";
                        foreach (var Item in DynamicSoundForm.DynamicSoundRecords)
                            if (Item.Name == ШаблонОповещения[3 * i + 0])
                            {
                                НаличиеШаблона = true;
                                Шаблон = Item.Message;
                                break;
                            }

                        if (НаличиеШаблона == true)
                        {
                            int.TryParse(ШаблонОповещения[3 * i + 2], out ПривязкаВремени);

                            string[] ВремяАктивацииШаблона = ШаблонОповещения[3 * i + 1].Replace(" ", "").Split(',');
                            if (ВремяАктивацииШаблона.Length > 0)
                            {
                                for (int j = 0; j < ВремяАктивацииШаблона.Length; j++)
                                {
                                    int ВремяСмещения = 0;
                                    if ((int.TryParse(ВремяАктивацииШаблона[j], out ВремяСмещения)) == true)
                                    {
                                        СостояниеФормируемогоСообщенияИШаблон НовыйШаблон;

                                        НовыйШаблон.Активность = АктивностьШаблоновДанногоПоезда;
                                        НовыйШаблон.Воспроизведен = false;
                                        НовыйШаблон.ВремяСмещения = ВремяСмещения;
                                        НовыйШаблон.НазваниеШаблона = ШаблонОповещения[3 * i + 0];
                                        НовыйШаблон.Шаблон = Шаблон;
                                        НовыйШаблон.ПривязкаКВремени = ПривязкаВремени;
                                        НовыйШаблон.ЯзыкиОповещения = new List<NotificationLanguage> {NotificationLanguage.Ru, NotificationLanguage.Eng};  //TODO:Брать из ШаблонОповещения полученого из TrainTable.

                                        Record.СписокФормируемыхСообщений.Add(НовыйШаблон);
                                    }
                                }
                            }
                        }
                    }
                }


                //СБРОСИТЬ НОМЕР ПУТИ, НА ВРЕМЯ БОЛЬШЕ ТЕКУЩЕГО
                if (!(Record.Время >= DateTime.Now))
                {
                    Record.НомерПути = String.Empty;
                }


                int TryCounter = 50;
                while (--TryCounter > 0)
                {
                    string Key = Record.Время.ToString("yy.MM.dd  HH:mm:ss");
                    string[] SubKeys = Key.Split(':');
                    if (SubKeys[0].Length == 1)
                        Key = "0" + Key;

                    if (SoundRecords.ContainsKey(Key) == false)
                    {
                        SoundRecords.Add(Key, Record);
                        SoundRecordsOld.Add(Key, Record);
                        break;
                    }

                    Record.Время = Record.Время.AddSeconds(1);
                }
            }
        }



        private void СозданиеСтатическихЗвуковыхФайлов()
        {
            foreach (SoundConfigurationRecord Config in SoundConfiguration.SoundConfigurationRecords)
            {
                СтатическоеСообщение statRecord;
                statRecord.СостояниеВоспроизведения = SoundRecordStatus.ОжиданиеВоспроизведения;

                if (Config.Enable == true)
                {
                    if (Config.EnablePeriodic == true)
                    {
                        statRecord.ОписаниеКомпозиции = Config.Name;
                        statRecord.НазваниеКомпозиции = Config.Name;

                        if (statRecord.НазваниеКомпозиции == string.Empty)
                            continue;

                        string[] Times = Config.MessagePeriodic.Split(',');
                        if (Times.Length != 3)
                            continue;

                        DateTime НачалоИнтервала2 = DateTime.Parse(Times[0]), КонецИнтервала2 = DateTime.Parse(Times[1]);
                        int Интервал = int.Parse(Times[2]);

                        while (НачалоИнтервала2 < КонецИнтервала2)
                        {
                            statRecord.ID = ID++;
                            statRecord.Время = НачалоИнтервала2;
                            statRecord.Активность = true;

                            int ПопыткиВставитьСообщение = 5;
                            while (ПопыткиВставитьСообщение-- > 0)
                            {
                                string Key = statRecord.Время.ToString("yy.MM.dd  HH:mm:ss");
                                string[] SubKeys = Key.Split(':');
                                if (SubKeys[0].Length == 1)
                                    Key = "0" + Key;

                                if (СтатическиеЗвуковыеСообщения.ContainsKey(Key))
                                {
                                    statRecord.Время = statRecord.Время.AddSeconds(1);
                                    continue;
                                }

                                СтатическиеЗвуковыеСообщения.Add(Key, statRecord);
                                break;
                            }

                            НачалоИнтервала2 = НачалоИнтервала2.AddMinutes(Интервал);
                        }
                    }


                    if (Config.EnableSingle == true)
                    {
                        statRecord.ОписаниеКомпозиции = Config.Name;
                        statRecord.НазваниеКомпозиции = Config.Name;

                        if (statRecord.НазваниеКомпозиции == string.Empty)
                            continue;

                        string[] Times = Config.MessageSingle.Split(',');

                        foreach (string time in Times)
                        {
                            statRecord.ID = ID++;
                            statRecord.Время = DateTime.Parse(time);
                            statRecord.Активность = true;

                            int ПопыткиВставитьСообщение = 5;
                            while (ПопыткиВставитьСообщение-- > 0)
                            {
                                string Key = statRecord.Время.ToString("yy.MM.dd  HH:mm:ss");
                                string[] SubKeys = Key.Split(':');
                                if (SubKeys[0].Length == 1)
                                    Key = "0" + Key;

                                if (СтатическиеЗвуковыеСообщения.ContainsKey(Key))
                                {
                                    statRecord.Время = statRecord.Время.AddSeconds(1);
                                    continue;
                                }

                                СтатическиеЗвуковыеСообщения.Add(Key, statRecord);
                                break;
                            }
                        }
                    }
                }
            }
        }



        // Отображение сформированного списка воспроизведения в таблицу
        private void ОбновитьСписокЗвуковыхСообщенийВТаблице()
        {
            ОбновлениеСписка = true;

            listView1.InvokeIfNeeded(() =>
            {
                listView1.Items.Clear();
                lVПрибытие.Items.Clear();
                lVТранзит.Items.Clear();
                lVОтправление.Items.Clear();


                for (int i = 0; i < SoundRecords.Count; i++)
                {
                    var Данные = SoundRecords.ElementAt(i);

                    string ВремяОтправления = "";
                    string ВремяПрибытия = "";
                    if ((Данные.Value.БитыАктивностиПолей & 0x04) != 0x00) ВремяПрибытия = Данные.Value.ВремяПрибытия.ToString("HH:mm:ss");
                    if ((Данные.Value.БитыАктивностиПолей & 0x10) != 0x00) ВремяОтправления = Данные.Value.ВремяОтправления.ToString("HH:mm:ss");

                    ListViewItem lvi1 = new ListViewItem(new string[] {Данные.Value.Время.ToString("yy.MM.dd  HH:mm:ss"),
                                                                       Данные.Value.НомерПоезда.Replace(':', ' '),
                                                                       Данные.Value.НомерПути.ToString(),
                                                                       Данные.Value.НазваниеПоезда,
                                                                       ВремяПрибытия,
                                                                       ВремяОтправления,
                                                                       Данные.Value.Примечание });
                    lvi1.Tag = Данные.Value.ID;
                    lvi1.Checked = Данные.Value.Состояние == SoundRecordStatus.Выключена ? false : true;
                    this.listView1.Items.Add(lvi1);

                    if ((Данные.Value.БитыАктивностиПолей & 0x14) == 0x04)
                    {
                        ListViewItem lvi2 = new ListViewItem(new string[] {Данные.Value.Время.ToString("yy.MM.dd  HH:mm:ss"),
                                                                       Данные.Value.НомерПоезда.Replace(':', ' '),
                                                                       Данные.Value.НомерПути.ToString(),
                                                                       Данные.Value.ВремяПрибытия.ToString("yy.MM.dd  HH:mm:ss"),
                                                                       Данные.Value.НазваниеПоезда });
                        lvi2.Tag = Данные.Value.ID;
                        lvi2.Checked = Данные.Value.Состояние == SoundRecordStatus.Выключена ? false : true;
                        this.lVПрибытие.Items.Add(lvi2);
                    }

                    if ((Данные.Value.БитыАктивностиПолей & 0x14) == 0x14)
                    {
                        ListViewItem lvi3 = new ListViewItem(new string[] {Данные.Value.Время.ToString("yy.MM.dd  HH:mm:ss"),
                                                                       Данные.Value.НомерПоезда.Replace(':', ' '),
                                                                       Данные.Value.НомерПути.ToString(),
                                                                       Данные.Value.ВремяПрибытия.ToString("yy.MM.dd  HH:mm:ss"),
                                                                       Данные.Value.ВремяОтправления.ToString("yy.MM.dd  HH:mm:ss"),
                                                                       Данные.Value.НазваниеПоезда });
                        lvi3.Tag = Данные.Value.ID;
                        lvi3.Checked = Данные.Value.Состояние == SoundRecordStatus.Выключена ? false : true;
                        this.lVТранзит.Items.Add(lvi3);
                    }

                    if ((Данные.Value.БитыАктивностиПолей & 0x14) == 0x10)
                    {
                        ListViewItem lvi4 = new ListViewItem(new string[] {Данные.Value.Время.ToString("yy.MM.dd  HH:mm:ss"),
                                                                       Данные.Value.НомерПоезда.Replace(':', ' '),
                                                                       Данные.Value.НомерПути.ToString(),
                                                                       Данные.Value.ВремяОтправления.ToString("yy.MM.dd  HH:mm:ss"),
                                                                       Данные.Value.НазваниеПоезда });
                        lvi4.Tag = Данные.Value.ID;
                        lvi4.Checked = Данные.Value.Состояние == SoundRecordStatus.Выключена ? false : true;
                        this.lVОтправление.Items.Add(lvi4);
                    }
                }
            });

            ОбновлениеСписка = false;
        }



        private void ОбновитьСписокЗвуковыхСообщенийВТаблицеСтатическихСообщений()
        {
            ОбновлениеСписка = true;

            int НомерСтроки = 0;
            foreach (var Данные in СтатическиеЗвуковыеСообщения)
            {
                if (НомерСтроки >= lVСтатическиеСообщения.Items.Count)
                {
                    ListViewItem lvi1 = new ListViewItem(new string[] {Данные.Value.Время.ToString("yy.MM.dd  HH:mm:ss"),
                                                                       Данные.Value.НазваниеКомпозиции });
                    lvi1.Tag = НомерСтроки;
                    lvi1.Checked = Данные.Value.Активность;
                    lVСтатическиеСообщения.Items.Add(lvi1);
                }
                else
                {
                    if (lVСтатическиеСообщения.Items[НомерСтроки].SubItems[0].Text != Данные.Value.Время.ToString("yy.MM.dd  HH:mm:ss"))
                        lVСтатическиеСообщения.Items[НомерСтроки].SubItems[0].Text = Данные.Value.Время.ToString("yy.MM.dd  HH:mm:ss");
                    if (lVСтатическиеСообщения.Items[НомерСтроки].SubItems[1].Text != Данные.Value.НазваниеКомпозиции)
                        lVСтатическиеСообщения.Items[НомерСтроки].SubItems[1].Text = Данные.Value.НазваниеКомпозиции;
                }

                НомерСтроки++;
            }

            while (НомерСтроки < lVСтатическиеСообщения.Items.Count)
                lVСтатическиеСообщения.Items.RemoveAt(НомерСтроки);

            ОбновлениеСписка = false;
        }



        // Раскрасить записи в соответствии с состоянием
        private void ОбновитьСостояниеЗаписейТаблицы()
        {
            #region Обновление списков поездов
            ОбновлениеРаскраскиСписка(this.listView1);
            ОбновлениеРаскраскиСписка(this.lVПрибытие);
            ОбновлениеРаскраскиСписка(this.lVТранзит);
            ОбновлениеРаскраскиСписка(this.lVОтправление);
            #endregion

            #region Обновление списка окна статических звуковых сообщений
            for (int item = 0; item < this.lVСтатическиеСообщения.Items.Count; item++)
            {
                string Key = this.lVСтатическиеСообщения.Items[item].SubItems[0].Text;

                if (СтатическиеЗвуковыеСообщения.Keys.Contains(Key) == true)
                {
                    СтатическоеСообщение Данные = СтатическиеЗвуковыеСообщения[Key];

                    if (Данные.Активность == false)
                    {
                        if (this.lVСтатическиеСообщения.Items[item].BackColor != Color.LightGray)
                            this.lVСтатическиеСообщения.Items[item].BackColor = Color.LightGray;
                    }
                    else
                    {
                        switch (Данные.СостояниеВоспроизведения)
                        {
                            default:
                            case SoundRecordStatus.Выключена:
                            case SoundRecordStatus.Воспроизведена:
                                if (this.lVСтатическиеСообщения.Items[item].BackColor != Color.LightGray)
                                    this.lVСтатическиеСообщения.Items[item].BackColor = Color.LightGray;
                                break;

                            case SoundRecordStatus.ОжиданиеВоспроизведения:
                                if (this.lVСтатическиеСообщения.Items[item].BackColor != Color.LightGreen)
                                    this.lVСтатическиеСообщения.Items[item].BackColor = Color.LightGreen;
                                break;

                            case SoundRecordStatus.Воспроизведение:
                                if (this.lVСтатическиеСообщения.Items[item].BackColor != Color.LightBlue)
                                    this.lVСтатическиеСообщения.Items[item].BackColor = Color.LightBlue;
                                break;
                        }
                    }
                }
            }
            #endregion

        }



        void ОбновлениеРаскраскиСписка(ListView lv)
        {
            for (int item = 0; item < lv.Items.Count; item++)
            {
                if (item <= SoundRecords.Count)
                {
                    try
                    {
                        string Key = lv.Items[item].SubItems[0].Text;

                        if (SoundRecords.Keys.Contains(Key) == true)
                        {
                            SoundRecord Данные = SoundRecords[Key];
                            switch (Данные.СостояниеКарточки)
                            {
                                default:
                                case 0: // Выключен или не актуален
                                    if (lv.Items[item].ForeColor != Program.Настройки.НастройкиЦветов[0])
                                        lv.Items[item].ForeColor = Program.Настройки.НастройкиЦветов[0];
                                    if (lv.Items[item].BackColor != Program.Настройки.НастройкиЦветов[1])
                                        lv.Items[item].BackColor = Program.Настройки.НастройкиЦветов[1];
                                    break;

                                case 1: // Отсутствую шаблоны оповещения
                                    if (lv.Items[item].ForeColor != Program.Настройки.НастройкиЦветов[2])
                                        lv.Items[item].ForeColor = Program.Настройки.НастройкиЦветов[2];
                                    if (lv.Items[item].BackColor != Program.Настройки.НастройкиЦветов[3])
                                        lv.Items[item].BackColor = Program.Настройки.НастройкиЦветов[3];
                                    break;

                                case 2: // Время не подошло (за 30 минут)
                                    if (lv.Items[item].ForeColor != Program.Настройки.НастройкиЦветов[4])
                                        lv.Items[item].ForeColor = Program.Настройки.НастройкиЦветов[4];
                                    if (lv.Items[item].BackColor != Program.Настройки.НастройкиЦветов[5])
                                        lv.Items[item].BackColor = Program.Настройки.НастройкиЦветов[5];
                                    break;

                                case 3: // Не установлен путь
                                    if (lv.Items[item].ForeColor != Program.Настройки.НастройкиЦветов[6])
                                        lv.Items[item].ForeColor = Program.Настройки.НастройкиЦветов[6];
                                    if (lv.Items[item].BackColor != Program.Настройки.НастройкиЦветов[7])
                                        lv.Items[item].BackColor = Program.Настройки.НастройкиЦветов[7];
                                    break;

                                case 4: // Не полностью включены все галочки
                                    if (lv.Items[item].ForeColor != Program.Настройки.НастройкиЦветов[8])
                                        lv.Items[item].ForeColor = Program.Настройки.НастройкиЦветов[8];
                                    if (lv.Items[item].BackColor != Program.Настройки.НастройкиЦветов[9])
                                        lv.Items[item].BackColor = Program.Настройки.НастройкиЦветов[9];
                                    break;

                                case 5: // Полностью включены все галочки
                                    if (lv.Items[item].ForeColor != Program.Настройки.НастройкиЦветов[10])
                                        lv.Items[item].ForeColor = Program.Настройки.НастройкиЦветов[10];
                                    if (lv.Items[item].BackColor != Program.Настройки.НастройкиЦветов[11])
                                        lv.Items[item].BackColor = Program.Настройки.НастройкиЦветов[11];
                                    break;

                                case 6: // Задержка
                                    if (lv.Items[item].ForeColor != Program.Настройки.НастройкиЦветов[12])
                                        lv.Items[item].ForeColor = Program.Настройки.НастройкиЦветов[12];
                                    if (lv.Items[item].BackColor != Program.Настройки.НастройкиЦветов[13])
                                        lv.Items[item].BackColor = Program.Настройки.НастройкиЦветов[13];
                                    break;
                            }

                            //Обновить номер пути
                            if (lv.Items[item].SubItems[2].Text != Данные.НомерПути.ToString())
                                lv.Items[item].SubItems[2].Text = Данные.НомерПути.ToString();

                            if (lv.Name == "listView1")
                                if (lv.Items[item].SubItems[6].Text != Данные.Примечание)
                                    lv.Items[item].SubItems[6].Text = Данные.Примечание;
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
            bool СообщениеИзменено;

            СписокБлижайшихСобытий.Clear();


            #region Определить композицию для запуска статических сообщений
            for (int i = 0; i < СтатическиеЗвуковыеСообщения.Count(); i++)
            {
                string Key = СтатическиеЗвуковыеСообщения.ElementAt(i).Key;
                СтатическоеСообщение Сообщение = СтатическиеЗвуковыеСообщения.ElementAt(i).Value;
                СообщениеИзменено = false;


                if (DateTime.Now < Сообщение.Время)
                {
                    if (Сообщение.СостояниеВоспроизведения != SoundRecordStatus.ОжиданиеВоспроизведения)
                    {
                        Сообщение.СостояниеВоспроизведения = SoundRecordStatus.ОжиданиеВоспроизведения;
                        СообщениеИзменено = true;
                    }
                }
                else if (DateTime.Now >= Сообщение.Время.AddMinutes(2))
                {
                    if (Сообщение.СостояниеВоспроизведения != SoundRecordStatus.Воспроизведена)
                    {
                        Сообщение.СостояниеВоспроизведения = SoundRecordStatus.Воспроизведена;
                        СообщениеИзменено = true;
                    }
                }
                else if (Сообщение.СостояниеВоспроизведения == SoundRecordStatus.ОжиданиеВоспроизведения)
                {
                    СообщениеИзменено = true;
                    Сообщение.СостояниеВоспроизведения = SoundRecordStatus.Воспроизведена;
                    if (Сообщение.Активность == true)
                        foreach (var Sound in StaticSoundForm.StaticSoundRecords)
                        {
                            if (Sound.Name == Сообщение.НазваниеКомпозиции)
                            {
                                if (РазрешениеРаботы == true)
                                {
                                    Program.ЗаписьЛога("Автоматическое воспроизведение звукового сообщения", Сообщение.НазваниеКомпозиции);
                                    var воспроизводимоеСообщение = new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = Sound.Name, Язык = NotificationLanguage.Ru };
                                    MainWindowForm.ОчередьВоспроизводимыхЗвуковыхСообщений.Add(воспроизводимоеСообщение);
                                }
                                break;
                            }
                        }
                }

                if (СообщениеИзменено == true)
                    СтатическиеЗвуковыеСообщения[Key] = Сообщение;


                //==================================================================================
                if (DateTime.Now < Сообщение.Время.AddSeconds(20) && DateTime.Now > Сообщение.Время.AddMinutes(-30))
                {
                    var statSound = StaticSoundForm.StaticSoundRecords.FirstOrDefault(sound => sound.Name == Сообщение.НазваниеКомпозиции);

                    ОписаниеСобытия событие;
                    событие.НомерСписка = 1;
                    событие.СостояниеСтроки = 2;
                    событие.Описание = Сообщение.НазваниеКомпозиции;
                    событие.Время = Сообщение.Время;
                    событие.Ключ = Key;
                    событие.ШаблонИлиСообщение = statSound.Message;

                    if (DateTime.Now >= Сообщение.Время)
                        событие.СостояниеСтроки = 4;

                    if (Сообщение.Активность == false)
                        событие.СостояниеСтроки = 0;

                    int КоличествоПопыток = 0;
                    string Ключ;
                    while (КоличествоПопыток++ < 10)
                    {
                        Ключ = событие.Время.ToString("yy.MM.dd  HH:mm:ss");
                        string[] parts = Ключ.Split(':');
                        if (parts[0].Length == 1) Ключ = "0" + Ключ;

                        if (СписокБлижайшихСобытий.ContainsKey(Ключ) == false)
                        {
                            СписокБлижайшихСобытий.Add(Ключ, событие);
                            break;
                        }

                        событие.Время.AddSeconds(10);
                    }
                }
            }
            #endregion


            #region Определить композицию для запуска сообщений о движении поездов
            DateTime ТекущееВремя = DateTime.Now;
            bool ВнесеныИзменения = false;
            for (int i = 0; i < SoundRecords.Count; i++)
            {
                var Данные = SoundRecords.ElementAt(i).Value;
                ВнесеныИзменения = false;

                while (true)
                {
                    if (Данные.Активность == true)
                    {
                        // Проверка на нештатные ситуации
                        if ((Данные.БитыНештатныхСитуаций & 0x07) != 0x00)
                        {
                            if (Данные.СостояниеКарточки != 6)
                            {
                                Данные.СостояниеКарточки = 6;
                                if ((Данные.БитыНештатныхСитуаций & 0x01) != 0x00) Данные.ОписаниеСостоянияКарточки = "Поезд отменен";
                                if ((Данные.БитыНештатныхСитуаций & 0x02) != 0x00) Данные.ОписаниеСостоянияКарточки = "Задержка прибытия поезда";
                                if ((Данные.БитыНештатныхСитуаций & 0x04) != 0x00) Данные.ОписаниеСостоянияКарточки = "Задержка отправления поезда";
                                ВнесеныИзменения = true;
                            }


                            DateTime ВременноеВремяСобытия = (Данные.БитыАктивностиПолей & 0x04) != 0x00 ? Данные.ВремяПрибытия : Данные.ВремяОтправления;
                            string ФормируемоеСообщение = "";
                            if (DateTime.Now < ВременноеВремяСобытия)
                            {
                                Данные.ТаймерПовторения++;
                                ВнесеныИзменения = true; //для сохранения отсчета таймера
                                int ТипПоезда = (int)Данные.ТипПоезда;

                                if ((Данные.БитыНештатныхСитуаций & 0x01) != 0x00)
                                    if (Данные.ТаймерПовторения >= (uint)(Program.Настройки.ИнтервалМеждуОповещениемОбОтменеПоезда * 60.0))
                                    {
                                        Данные.ТаймерПовторения = 0;
                                        ФормируемоеСообщение = Program.ШаблонОповещенияОбОтменеПоезда[ТипПоезда];
                                    }

                                if ((Данные.БитыНештатныхСитуаций & 0x02) != 0x00)
                                    if (Данные.ТаймерПовторения >= (uint)(Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда * 60.0))
                                    {
                                        Данные.ТаймерПовторения = 0;
                                        ФормируемоеСообщение = Program.ШаблонОповещенияОЗадержкеПрибытияПоезда[ТипПоезда];
                                    }

                                if ((Данные.БитыНештатныхСитуаций & 0x04) != 0x00)
                                    if (Данные.ТаймерПовторения >= (uint)(Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда * 60.0))
                                    {
                                        Данные.ТаймерПовторения = 0;
                                        ФормируемоеСообщение = Program.ШаблонОповещенияОЗадержкеОтправленияПоезда[ТипПоезда];
                                    }


                                if (ФормируемоеСообщение != "")
                                {
                                    bool НаличиеШаблона = false;
                                    foreach (var Item in DynamicSoundForm.DynamicSoundRecords)
                                        if (Item.Name == ФормируемоеСообщение)
                                        {
                                            НаличиеШаблона = true;
                                            ФормируемоеСообщение = Item.Message;
                                            break;
                                        }

                                    if (РазрешениеРаботы && НаличиеШаблона)
                                    {
                                        СостояниеФормируемогоСообщенияИШаблон шаблонФормируемогоСообщения = new СостояниеФормируемогоСообщенияИШаблон
                                        {
                                            Шаблон = ФормируемоеСообщение,
                                            ЯзыкиОповещения = new List<NotificationLanguage> { NotificationLanguage.Ru, NotificationLanguage.Eng }, //TODO: вычислять языки оповещения 
                                            НазваниеШаблона = "Авария"
                                        };
                                        MainWindowForm.ВоспроизвестиШаблонОповещения("Автоматическое воспроизведение сообщения о внештатной ситуации", Данные, шаблонФормируемогоСообщения);
                                        //Debug.WriteLine(DateTime.Now.ToString("t") + "Автоматическое воспроизведение сообщения о внештатной ситуации"); //DEBUG
                                    }
                                }
                            }


                            //==================================================================================

                            if (DateTime.Now < ВременноеВремяСобытия && DateTime.Now > ВременноеВремяСобытия.AddMinutes(-30))
                            {
                                ОписаниеСобытия событие;
                                событие.НомерСписка = 0;
                                событие.СостояниеСтроки = 3;
                                событие.Описание = Данные.НомерПоезда + " " + Данные.НазваниеПоезда + ": " + Данные.ОписаниеСостоянияКарточки;
                                событие.Время = ВременноеВремяСобытия;
                                событие.Ключ = SoundRecords.ElementAt(i).Key;
                                событие.ШаблонИлиСообщение = ФормируемоеСообщение;

                                int КоличествоПопыток = 0;
                                string Ключ;
                                while (КоличествоПопыток++ < 10)
                                {
                                    Ключ = событие.Время.ToString("yy.MM.dd  HH:mm:ss");
                                    string[] parts = Ключ.Split(':');
                                    if (parts[0].Length == 1) Ключ = "0" + Ключ;

                                    if (СписокБлижайшихСобытий.ContainsKey(Ключ) == false)
                                    {
                                        СписокБлижайшихСобытий.Add(Ключ, событие);
                                        break;
                                    }

                                    событие.Время.AddSeconds(10);
                                }
                            }
                            break;
                        }
                        else
                            Данные.ТаймерПовторения = 0;

                        // Проверка на наличие шаблонов оповещения
                        if (Данные.СписокФормируемыхСообщений.Count == 0)
                        {
                            if (Данные.СостояниеКарточки != 1)
                            {
                                Данные.СостояниеКарточки = 1;
                                Данные.ОписаниеСостоянияКарточки = "Нет шаблонов оповещения";
                                ВнесеныИзменения = true;
                            }

                            break;
                        }

                        // Проверка на приближения времени оповещения (за 30 минут) и завершения выполнения (после 3 минут)
                        DateTime СамоеРаннееВремя = DateTime.Now, СамоеПозднееВремя = DateTime.Now;
                        for (int j = 0; j < Данные.СписокФормируемыхСообщений.Count; j++)
                        {
                            var ФормируемоеСообщение = Данные.СписокФормируемыхСообщений[j];
                            DateTime ВремяСобытия = ФормируемоеСообщение.ПривязкаКВремени == 0 ? Данные.ВремяПрибытия : Данные.ВремяОтправления;
                            ВремяСобытия = ВремяСобытия.AddMinutes(ФормируемоеСообщение.ВремяСмещения);
                            if (j == 0)
                            {
                                СамоеРаннееВремя = СамоеПозднееВремя = ВремяСобытия;
                            }
                            else
                            {
                                if (ВремяСобытия < СамоеРаннееВремя)
                                    СамоеРаннееВремя = ВремяСобытия;

                                if (ВремяСобытия > СамоеПозднееВремя)
                                    СамоеПозднееВремя = ВремяСобытия;
                            }
                        }

                        if (DateTime.Now < СамоеРаннееВремя.AddMinutes(-30))
                        {
                            if (Данные.СостояниеКарточки != 2)
                            {
                                Данные.СостояниеКарточки = 2;
                                Данные.ОписаниеСостоянияКарточки = "Рано";
                                ВнесеныИзменения = true;
                            }

                            break;
                        }

                        if (DateTime.Now > СамоеПозднееВремя.AddMinutes(3))
                        {
                            if (Данные.СостояниеКарточки != 0)
                            {
                                Данные.СостояниеКарточки = 0;
                                Данные.ОписаниеСостоянияКарточки = "Поздно";
                                ВнесеныИзменения = true;
                            }

                            break;
                        }


                        // Проверка на установку пути
                        if (Данные.НомерПути == "")
                        {
                            if (Данные.СостояниеКарточки != 3)
                            {
                                Данные.СостояниеКарточки = 3;
                                Данные.ОписаниеСостоянияКарточки = "Нет пути";
                                ВнесеныИзменения = true;
                            }
                            break;
                        }

                        // Проверка на наличие всех включенных галочек
                        int КоличествоВключенныхГалочек = 0;
                        for (int j = 0; j < Данные.СписокФормируемыхСообщений.Count; j++)
                        {
                            var ФормируемоеСообщение = Данные.СписокФормируемыхСообщений[j];
                            if (ФормируемоеСообщение.Активность == true)
                            {
                                КоличествоВключенныхГалочек++;
                                if (ФормируемоеСообщение.Воспроизведен == false)
                                {
                                    DateTime ВремяСобытия = ФормируемоеСообщение.ПривязкаКВремени == 0 ? Данные.ВремяПрибытия : Данные.ВремяОтправления;
                                    ВремяСобытия = ВремяСобытия.AddMinutes(ФормируемоеСообщение.ВремяСмещения);
                                    if ((ТекущееВремя.Hour == ВремяСобытия.Hour) && (ТекущееВремя.Minute == ВремяСобытия.Minute) && (ТекущееВремя.Second == ВремяСобытия.Second))
                                    {
                                        Данные.СписокФормируемыхСообщений[j] = ФормируемоеСообщение;
                                        ВнесеныИзменения = true;

                                        if (РазрешениеРаботы == true)
                                            MainWindowForm.ВоспроизвестиШаблонОповещения("Автоматическое воспроизведение расписания", Данные, ФормируемоеСообщение);
                                    }


                                    //Динамическое сообщение попадет в список если ФормируемоеСообщение еще не воспроезведенно  и не прошло 1мин с момента попадания в список.
                                    //==================================================================================
                                    if (DateTime.Now < ВремяСобытия.AddSeconds(50) && DateTime.Now > ВремяСобытия.AddMinutes(-30))
                                    {
                                        ОписаниеСобытия событие;
                                        событие.НомерСписка = 0;
                                        событие.СостояниеСтроки = 1;
                                        событие.Описание = Данные.НомерПоезда + " " + Данные.НазваниеПоезда + ": " + ФормируемоеСообщение.НазваниеШаблона;
                                        событие.Время = ВремяСобытия;
                                        событие.Ключ = SoundRecords.ElementAt(i).Key;
                                        событие.ШаблонИлиСообщение = ФормируемоеСообщение.Шаблон;

                                        if (DateTime.Now >= ВремяСобытия)           //TODO: Убрать. Нужно ориентироваться на флаг воспроизведения данного шаблона в данных: запуск на воспроизведение СостояниеСтроки=4 (выделение и отображение субтитров), окончание воспроизведения СостояниеСтроки=0
                                            событие.СостояниеСтроки = 4;


                                        int КоличествоПопыток = 0;
                                        string Ключ;
                                        while (КоличествоПопыток++ < 10)
                                        {
                                            Ключ = событие.Время.ToString("yy.MM.dd  HH:mm:ss");
                                            string[] parts = Ключ.Split(':');
                                            if (parts[0].Length == 1) Ключ = "0" + Ключ;

                                            if (СписокБлижайшихСобытий.ContainsKey(Ключ) == false)
                                            {
                                                СписокБлижайшихСобытий.Add(Ключ, событие);
                                                break;
                                            }

                                            событие.Время.AddSeconds(10);
                                        }
                                    }

                                }
                            }
                        }

                        if (КоличествоВключенныхГалочек < Данные.СписокФормируемыхСообщений.Count)
                        {
                            if (Данные.СостояниеКарточки != 4)
                            {
                                Данные.СостояниеКарточки = 4;
                                Данные.ОписаниеСостоянияКарточки = "Не все шаблоны разрешены";
                                ВнесеныИзменения = true;
                            }
                        }
                        else
                        {
                            if (Данные.СостояниеКарточки != 5)
                            {
                                Данные.СостояниеКарточки = 5;
                                Данные.ОписаниеСостоянияКарточки = "Все шаблоны разрешены";
                                ВнесеныИзменения = true;
                            }
                        }
                    }
                    else
                    {
                        if (Данные.СостояниеКарточки != 0)
                        {
                            Данные.СостояниеКарточки = 0;
                            Данные.ОписаниеСостоянияКарточки = "Отключена";
                            ВнесеныИзменения = true;
                        }
                    }

                    break;
                }

                if (ВнесеныИзменения == true)
                {
                    string Key = SoundRecords.ElementAt(i).Key;
                    SoundRecords.Remove(Key);
                    SoundRecords.Add(Key, Данные);
                }
            }
            #endregion


            ОставитьОднуАктивнуюСтрокуВСпискеБлижайшихСобытий();


            lVСобытия_ОбновитьСостояниеТаблицы();


            // ОтобразитьСубтитры();
        }


        // Определение информации для вывода на табло
        private void ОпределитьИнформациюДляОтображенияНаТабло()
        {
            #region ВЫВОД РАСПИСАНИЯ НА ТАБЛО (из главного окна или из окна расписания)

            if (Binding2GeneralScheduleBehaviors != null && Binding2GeneralScheduleBehaviors.Any())
            {
                if (_tickCounter++ > 50)
                {
                    _tickCounter = 0;

                    var binding2MainWindow = Binding2GeneralScheduleBehaviors.Where(b => b.SourceLoad == SourceLoad.MainWindow).ToList();
                    var binding2Shedule = Binding2GeneralScheduleBehaviors.Where(b => b.SourceLoad == SourceLoad.Shedule).ToList();

                    Func<string, string, DateTime> timePars = (arrival, depart) =>
                    {
                        DateTime outData;

                        if (DateTime.TryParse(arrival, out outData))
                            return outData;

                        if (DateTime.TryParse(depart, out outData))
                            return outData;

                        return DateTime.MinValue;
                    };

                    //Отправить расписание из окна РАСПИАНИЕ
                    if (binding2Shedule.Any())
                    {
                        if (TrainTable.TrainTableRecords != null && TrainTable.TrainTableRecords.Any())
                        {
                            var table = TrainTable.TrainTableRecords.Select(t => new UniversalInputType
                            {
                                Event = (string.IsNullOrEmpty(t.ArrivalTime)) ? "ОТПР." : "ПРИБ.",
                                TypeTrain = (t.ТипПоезда == ТипПоезда.Пассажирский) ? TypeTrain.Passenger :
                                            (t.ТипПоезда == ТипПоезда.Пригородный) ? TypeTrain.Suburban :
                                            (t.ТипПоезда == ТипПоезда.Фирменный) ? TypeTrain.Corporate :
                                            (t.ТипПоезда == ТипПоезда.Скорый) ? TypeTrain.Express :
                                            (t.ТипПоезда == ТипПоезда.Скоростной) ? TypeTrain.HighSpeed :
                                            (t.ТипПоезда == ТипПоезда.Ласточка) ? TypeTrain.Swallow :
                                            (t.ТипПоезда == ТипПоезда.РЭКС) ? TypeTrain.Rex : TypeTrain.None,    
                                Note = t.Примечание, //C остановками: ...
                                PathNumber = t.TrainPathNumber,
                                NumberOfTrain = t.Num,
                                Stations = t.Name,
                                Time = timePars(t.ArrivalTime, t.DepartureTime),
                                DaysFollowing = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(t.Days).ПолучитьСтрокуОписанияРасписания(),
                                EmergencySituation = 0x00
                            }).ToList();

                            table.ForEach(t=> t.Message = $"ПОЕЗД:{t.NumberOfTrain}, ПУТЬ:{t.PathNumber}, СОБЫТИЕ:{t.Event}, СТАНЦИИ:{t.Stations}, ВРЕМЯ:{t.Time.ToShortTimeString()}");

                            var inData = new UniversalInputType { TableData = table };
                            foreach (var beh in binding2Shedule)
                            {
                                beh.InitializePagingBuffer(inData, beh.CheckContrains);
                            }
                        }
                    }
                    //Отправить расписание из ГЛАВНОГО окна
                    else
                    if (binding2MainWindow.Any())
                    {
                        if (SoundRecords != null && SoundRecords.Any())
                        {
                            foreach (var beh in binding2MainWindow)
                            {
                                var table = SoundRecords.Select(t => MapSoundRecord2UniveralInputType(t.Value, beh.GetDeviceSetting.PathPermission, false)).ToList();
                                table.ForEach(t => t.Message = $"ПОЕЗД:{t.NumberOfTrain}, ПУТЬ:{t.PathNumber}, СОБЫТИЕ:{t.Event}, СТАНЦИИ:{t.Stations}, ВРЕМЯ:{t.Time.ToShortTimeString()}");
                                var inData = new UniversalInputType { TableData = table };
                                beh.InitializePagingBuffer(inData, beh.CheckContrains);
                            }
                        }
                    }
                }
            }

            #endregion


            #region ВЫВОД НА ПУТЕВЫЕ ТАБЛО

            for (var i = 0; i < SoundRecords.Count; i++)
            {
                try
                {
                    var key = SoundRecords.Keys.ElementAt(i);
                    var данные = SoundRecords.ElementAt(i).Value;
                    var данныеOld = SoundRecordsOld.ElementAt(i).Value;

                    var _checked = данные.Состояние != SoundRecordStatus.Выключена;


                    if (_checked && (данные.ТипСообщения == SoundRecordType.ДвижениеПоезда))
                    {
                        //ВЫВОД НА ПУТЕВЫЕ ТАБЛО
                        byte номерПути = Program.ПолучитьНомерПути(данные.НомерПути);
                        byte номерПутиOld = Program.ПолучитьНомерПути(данныеOld.НомерПути);

                        if (номерПути > 0 || (номерПути == 0 && номерПутиOld > 0))
                        {
                            //ПОМЕНЯЛИ ПУТЬ
                            if (номерПути != номерПутиOld)
                            {
                                //очистили старый путь, если он не "0";
                                if (номерПутиOld > 0)
                                {
                                    данныеOld.СостояниеОтображения = TableRecordStatus.Очистка;
                                    SendOnPathTable(данныеOld);
                                }

                                //вывод на новое табло
                                данные.СостояниеОтображения = TableRecordStatus.Отображение;
                                SendOnPathTable(данные);
                            }
                            else
                            {
                                //ИЗДАНИЕ СОБЫТИЯ ИЗМЕНЕНИЯ ДАННЫХ В ЗАПИСИ SoundRecords.
                                if (!StructCompare.SoundRecordComparer(ref данные, ref данныеOld))
                                {
                                    данные.СостояниеОтображения = TableRecordStatus.Обновление;
                                    SendOnPathTable(данные);
                                }
                            }


                            //ТРАНЗИТНЫЕ
                            if ((данные.БитыАктивностиПолей & 0x14) == 0x14)
                            {

                            }
                            else
                            {
                                //ПРИБЫТИЕ
                                if ((данные.БитыАктивностиПолей & 0x04) == 0x04)
                                {
                                    //ОЧИСТИТЬ если нет нештатных ситуаций на момент прибытия
                                    if ((DateTime.Now >= данные.ВремяПрибытия.AddMinutes(10) &&        //10
                                        (DateTime.Now <= данные.ВремяПрибытия.AddMinutes(10.02))))
                                    {
                                        if ((данные.БитыНештатныхСитуаций & 0x07) == 0x00)
                                            if (данные.СостояниеОтображения == TableRecordStatus.Отображение ||
                                               (данные.СостояниеОтображения == TableRecordStatus.Обновление))
                                            {
                                                данные.СостояниеОтображения = TableRecordStatus.Очистка;
                                                данные.НомерПути = "0";

                                                var данныеОчистки = данные;
                                                данныеОчистки.НомерПути = данныеOld.НомерПути;
                                                SendOnPathTable(данныеОчистки);
                                            }
                                    }


                                    //ОЧИСТИТЬ если убрали нештатные ситуации
                                    if (((данные.БитыНештатныхСитуаций & 0x07) == 0x00)
                                        && ((данныеOld.БитыНештатныхСитуаций & 0x07) != 0x00)
                                        && (DateTime.Now >= данные.ВремяПрибытия.AddMinutes(10)))
                                    {
                                        if (данные.СостояниеОтображения == TableRecordStatus.Отображение ||
                                           (данные.СостояниеОтображения == TableRecordStatus.Обновление))
                                        {
                                            данные.СостояниеОтображения = TableRecordStatus.Очистка;
                                            данные.НомерПути = "0";

                                            var данныеОчистки = данные;
                                            данныеОчистки.НомерПути = данныеOld.НомерПути;
                                            SendOnPathTable(данныеОчистки);
                                        }
                                    }
                                }
                                else //ОТПРАВЛЕНИЕ
                                if ((данные.БитыАктивностиПолей & 0x10) == 0x10)
                                {
                                    //ОЧИСТИТЬ если нет нештатных ситуаций на момент отправления
                                    if ((DateTime.Now >= данные.ВремяОтправления.AddMinutes(1) &&       //1
                                         (DateTime.Now <= данные.ВремяОтправления.AddMinutes(1.02))))
                                    {
                                        if ((данные.БитыНештатныхСитуаций & 0x07) == 0x00)
                                            if (данные.СостояниеОтображения == TableRecordStatus.Отображение ||
                                              (данные.СостояниеОтображения == TableRecordStatus.Обновление))
                                            {
                                                данные.СостояниеОтображения = TableRecordStatus.Очистка;
                                                данные.НомерПути = "0";

                                                var данныеОчистки = данные;
                                                данныеОчистки.НомерПути = данныеOld.НомерПути;
                                                SendOnPathTable(данныеОчистки);
                                            }
                                    }

                                    //ОЧИСТИТЬ если убрали нештатные ситуации
                                    if (((данные.БитыНештатныхСитуаций & 0x07) == 0x00)
                                        && ((данныеOld.БитыНештатныхСитуаций & 0x07) != 0x00)
                                        && (DateTime.Now >= данные.ВремяОтправления.AddMinutes(1)))
                                    {
                                        if (данные.СостояниеОтображения == TableRecordStatus.Отображение ||
                                           (данные.СостояниеОтображения == TableRecordStatus.Обновление))
                                        {
                                            данные.СостояниеОтображения = TableRecordStatus.Очистка;
                                            данные.НомерПути = "0";

                                            var данныеОчистки = данные;
                                            данныеОчистки.НомерПути = данныеOld.НомерПути;
                                            SendOnPathTable(данныеОчистки);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    SoundRecords[key] = данные;
                    SoundRecordsOld[key] = данные;
                }
                catch (Exception)
                {
                    // ignored
                }
            }

          #endregion
        }


        // Формирование очереди воспроизведения звуковых файлов, вызывается таймером каждые 100 мс.
        private bool _isEmptyOldОчередьВоспроизводимыхЗвуковыхСообщений;
        private bool _isEmptyRaiseОчередьВоспроизводимыхЗвуковыхСообщений;
        private void ОбработкаЗвуковогоПотка()
        {
            int СекундаТекущегоВремени = DateTime.Now.Second;
            if (СекундаТекущегоВремени != ТекущаяСекунда)
            {
                ТекущаяСекунда = СекундаТекущегоВремени;
                ОпределитьКомпозициюДляЗапуска();
            }


            ОбновитьСостояниеЗаписейТаблицы();

            SoundFileStatus status = Player.GetFileStatus();

            if (MainForm.Воспроизвести.Text == "Остановить")
                if ((status != SoundFileStatus.Playing) && (!ОчередьВоспроизводимыхЗвуковыхСообщений.Any()))
                    MainForm.Воспроизвести.Text = "Воспроизвести выбранную запись";


            if (ОчередьВоспроизводимыхЗвуковыхСообщений.Any() && !_isEmptyOldОчередьВоспроизводимыхЗвуковыхСообщений)
            {
                СобытиеНачалоПроигрыванияОчередиЗвуковыхСообщений();
            }

            if (!ОчередьВоспроизводимыхЗвуковыхСообщений.Any() && _isEmptyOldОчередьВоспроизводимыхЗвуковыхСообщений)
            {
                _isEmptyRaiseОчередьВоспроизводимыхЗвуковыхСообщений = true;
            }
            if (!ОчередьВоспроизводимыхЗвуковыхСообщений.Any() && _isEmptyRaiseОчередьВоспроизводимыхЗвуковыхСообщений && (status != SoundFileStatus.Playing))
            {
                _isEmptyRaiseОчередьВоспроизводимыхЗвуковыхСообщений = false;
                СобытиеКонецПроигрыванияОчередиЗвуковыхСообщений();
            }

            _isEmptyOldОчередьВоспроизводимыхЗвуковыхСообщений = ОчередьВоспроизводимыхЗвуковыхСообщений.Any();


            if (status != SoundFileStatus.Playing)
            {
                if (ВремяЗадержкиМеждуСообщениями > 0)
                {
                    ВремяЗадержкиМеждуСообщениями--;
                }
                else
                if (ОчередьВоспроизводимыхЗвуковыхСообщений.Any())
                {
                    var воспроизводимоеСообщение = ОчередьВоспроизводимыхЗвуковыхСообщений[0];
                    ОчередьВоспроизводимыхЗвуковыхСообщений.RemoveAt(0);

                    var названиеФайла = воспроизводимоеСообщение.ИмяВоспроизводимогоФайла;
                    var язык = воспроизводимоеСообщение.Язык;

                    if (воспроизводимоеСообщение.ВремяПаузы.HasValue)
                    {
                        ВремяЗадержкиМеждуСообщениями = воспроизводимоеСообщение.ВремяПаузы.Value;
                        return;
                    }

                    if (названиеФайла.Contains(".wav") == false)
                        названиеФайла = Program.GetFileName(названиеФайла, язык);

                    if (Player.PlayFile(названиеФайла) == true)
                        MainForm.Воспроизвести.Text = "Остановить";
                }
            }
            else
            {
                //СобытиеПроцессПроигрыванияФайлаОчередиЗвуковыхСообщений();
            }
        }


        private void СобытиеНачалоПроигрыванияОчередиЗвуковыхСообщений()
        {
            //Debug.WriteLine("НАЧАЛО ПРОИГРЫВАНИЯ");//DEBUG

            if (SoundChanelManagment != null)
            {
                var soundChUit = new UniversalInputType { SoundChanels = Program.Настройки.КаналыДальнегоСлед.ToList(), ViewBag = new Dictionary<string, dynamic>() };
                soundChUit.ViewBag["SoundChanelManagmentEventPlaying"] = "StartPlaying";

                SoundChanelManagment.AddOneTimeSendData(soundChUit); //период отсыла регулируется TimeRespone.
            }
        }


        private void СобытиеКонецПроигрыванияОчередиЗвуковыхСообщений()
        {
            Debug.WriteLine("КОНЕЦ ПРОИГРЫВАНИЯ");//DEBUG

            if (SoundChanelManagment != null)
            {
                var soundChUit = new UniversalInputType { SoundChanels = Program.Настройки.КаналыДальнегоСлед.ToList(), ViewBag = new Dictionary<string, dynamic>() };
                soundChUit.ViewBag["SoundChanelManagmentEventPlaying"] = "StopPlaying";

                SoundChanelManagment.AddOneTimeSendData(soundChUit); //период отсыла регулируется TimeRespone.
            }
        }


        private void СобытиеПроцессПроигрыванияФайлаОчередиЗвуковыхСообщений()
        {
           // Debug.WriteLine("ПРОЦЕСС ПРОИГРЫВАНИЯ");//DEBUG
        }



        // Воспроизведение выбраной в таблице записи
        private void btnВоспроизвести_Click(object sender, EventArgs e)
        {
            if (MainForm.Воспроизвести.Text == "Остановить")
            {
                ОчередьВоспроизводимыхЗвуковыхСообщений.Clear();
                Player.PlayFile(string.Empty);
                MainForm.Воспроизвести.Text = "Воспроизвести выбранную запись";
                return;
            }
        }

        // Обработка закрытия основной формы
        private void MainWindowForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (myMainForm == this)
                myMainForm = null;
        }


        // Блокировка/разблокировка сообщения при нажатии на CheckBox
        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (ОбновлениеСписка)
                return;

            if ((sender as ListView).PointToClient(Cursor.Position).X > 22)
                e.NewValue = e.CurrentValue;
        }



        //Отправка сообшений на табло
        private void SendOnPathTable(SoundRecord data)
        {
            if (data.СостояниеОтображения == TableRecordStatus.Выключена || data.СостояниеОтображения == TableRecordStatus.ОжиданиеОтображения)
                return;

            if (data.НазванияТабло == null)
                return;


            var devicesId = data.НазванияТабло.Select(s => new string(s.TakeWhile(c => c != ':').ToArray())).Select(int.Parse).ToList();
            foreach (var devId in devicesId)
            {
                var beh = Binding2PathBehaviors.FirstOrDefault(b => b.GetDeviceId == devId);
                if (beh != null)
                {
                    var inData = MapSoundRecord2UniveralInputType(data, beh.GetDeviceSetting.PathPermission, true);
                    inData.Message = $"ПОЕЗД:{inData.NumberOfTrain}, ПУТЬ:{inData.PathNumber}, СОБЫТИЕ:{inData.Event}, СТАНЦИИ:{inData.Stations}, ВРЕМЯ:{inData.Time.ToShortTimeString()}";

                    beh.SendMessage4Path(inData, data.НомерПоезда, beh.CheckContrains);
                    //Debug.WriteLine($" ТАБЛО= {beh.GetDeviceName}: {beh.GetDeviceId} для ПУТИ {data.НомерПути}.  Сообшение= {inData.Message}  ");
                }
            }
        }



        private static string CreateActStr(SoundRecord Данные)
        {
            string actStr = "   ";
            if ((Данные.БитыАктивностиПолей & 0x14) == 0x14)
            {
                actStr = $"Приб.{Данные.ВремяПрибытия.ToString("HH:mm")} Отпр.:{Данные.ВремяОтправления.ToString("HH:mm")}";
            }
            else if ((Данные.БитыАктивностиПолей & 0x04) == 0x04)
            {
                actStr = $"Приб.{Данные.ВремяПрибытия.ToString("HH:mm")}";
            }
            else if ((Данные.БитыАктивностиПолей & 0x10) == 0x10)
            {
                actStr = $"Отпр.:{Данные.ВремяОтправления.ToString("HH:mm")}";
            }

            return actStr;
        }



        private static UniversalInputType MapSoundRecord2UniveralInputType(SoundRecord data, bool pathPermission, bool isShow)
        {
            string actStr = "   ";
            if ((data.БитыАктивностиПолей & 0x14) == 0x14)
            {
                actStr = "СТОЯНКА";
            }
            else if ((data.БитыАктивностиПолей & 0x04) == 0x04)
            {
                actStr = "ПРИБ.";
            }
            else if ((data.БитыАктивностиПолей & 0x10) == 0x10)
            {
                actStr = "ОТПР.";
            }

            TypeTrain typeTrain;
            switch (data.ТипПоезда)
            {
                case ТипПоезда.Пассажирский:
                    typeTrain = TypeTrain.Passenger;
                    break;

                case ТипПоезда.Пригородный:
                    typeTrain = TypeTrain.Suburban;
                    break;

                case ТипПоезда.Фирменный:
                    typeTrain = TypeTrain.Corporate;
                    break;

                case ТипПоезда.Скорый:
                    typeTrain = TypeTrain.Express;
                    break;

                case ТипПоезда.Скоростной:
                    typeTrain = TypeTrain.HighSpeed;
                    break;

                case ТипПоезда.Ласточка:
                    typeTrain = TypeTrain.Swallow;
                    break;

                case ТипПоезда.РЭКС:
                    typeTrain = TypeTrain.Rex;
                    break;

                default:
                    typeTrain = TypeTrain.None;
                    break;
            }

            var command = Command.None;
            switch (data.СостояниеОтображения)
            {
                case TableRecordStatus.Отображение:
                    command = Command.View;
                    break;

                case TableRecordStatus.Очистка:
                    command = Command.Delete;
                    break;

                case TableRecordStatus.Обновление:
                    command = Command.Update;
                    break;
            }

            var номерПути = string.Empty;
            switch (data.РазрешениеНаОтображениеПути)
            {
                case PathPermissionType.ИзФайлаНастроек:
                    номерПути = pathPermission ? data.НомерПути : "   ";
                    break;

                case PathPermissionType.Отображать:
                    номерПути = data.НомерПути;
                    break;

                case PathPermissionType.НеОтображать:
                    номерПути = "   ";
                    break;
            }


            UniversalInputType mapData;
            if (isShow)
            {
                mapData = new UniversalInputType
                {
                    Id = data.ID,
                    NumberOfTrain = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? data.НомерПоезда : "   ",
                    PathNumber = номерПути,
                    Event = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? actStr : "   ",
                    Time = ((actStr == "ПРИБ.") ? data.ВремяПрибытия : data.ВремяОтправления),
                    Stations = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? data.НазваниеПоезда : "   ",
                    Note = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? data.Примечание : "   ",
                    TypeTrain = typeTrain,
                    Addition = (data.ИспользоватьДополнение) ? data.Дополнение : string.Empty,
                    Command = command,
                    EmergencySituation = data.БитыНештатныхСитуаций
                };
            }
            else
            {
                mapData = new UniversalInputType
                {
                    Id = data.ID,
                    NumberOfTrain = data.НомерПоезда,
                    PathNumber = номерПути,
                    Event = actStr,
                    Time = ((actStr == "ПРИБ.") ? data.ВремяПрибытия : data.ВремяОтправления),
                    Stations = data.НазваниеПоезда,
                    Note = data.Примечание,
                    TypeTrain = typeTrain,
                    Addition = (data.ИспользоватьДополнение) ? data.Дополнение : string.Empty,
                    Command = command,
                    EmergencySituation = data.БитыНештатныхСитуаций
                };
            }

            return mapData;
        }



        protected override void OnClosed(EventArgs e)
        {
            DispouseCisClientIsConnectRx.Dispose();
            base.OnClosed(e);
        }



        private void listView6_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                ListView.SelectedIndexCollection sic = this.lVСтатическиеСообщения.SelectedIndices;

                foreach (int item in sic)
                {
                    if (item <= СтатическиеЗвуковыеСообщения.Count)
                    {
                        string Key = this.lVСтатическиеСообщения.Items[item].SubItems[0].Text;

                        if (СтатическиеЗвуковыеСообщения.Keys.Contains(Key) == true)
                        {
                            СтатическоеСообщение Данные = СтатическиеЗвуковыеСообщения[Key];

                            КарточкаСтатическогоЗвуковогоСообщения Карточка = new КарточкаСтатическогоЗвуковогоСообщения(Данные);
                            if (Карточка.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                Данные = Карточка.ПолучитьИзмененнуюКарточку();

                                string Key2 = Данные.Время.ToString("HH:mm:ss");
                                string[] SubKeys = Key.Split(':');
                                if (SubKeys[0].Length == 1)
                                    Key2 = "0" + Key2;

                                if (Key == Key2)
                                {
                                    СтатическиеЗвуковыеСообщения[Key] = Данные;
                                    this.lVСтатическиеСообщения.Items[item].SubItems[1].Text = Данные.НазваниеКомпозиции;
                                }
                                else
                                {
                                    СтатическиеЗвуковыеСообщения.Remove(Key);

                                    int ПопыткиВставитьСообщение = 5;
                                    while (ПопыткиВставитьСообщение-- > 0)
                                    {
                                        Key2 = Данные.Время.ToString("HH:mm:ss");
                                        SubKeys = Key2.Split(':');
                                        if (SubKeys[0].Length == 1)
                                            Key2 = "0" + Key2;

                                        if (СтатическиеЗвуковыеСообщения.ContainsKey(Key2))
                                        {
                                            Данные.Время = Данные.Время.AddSeconds(20);
                                            continue;
                                        }

                                        СтатическиеЗвуковыеСообщения.Add(Key2, Данные);
                                        break;
                                    }

                                    ОбновитьСписокЗвуковыхСообщенийВТаблицеСтатическихСообщений();
                                }
                            }

                            ОбновитьСостояниеЗаписейТаблицы();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        // Обработка двойного нажатия на сообщение (вызов формы сообщения)
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView listView = sender as ListView;

            try
            {
                ListView.SelectedIndexCollection sic = listView.SelectedIndices;


                foreach (int item in sic)
                {
                    if (item <= SoundRecords.Count)
                    {
                        string Key = listView.Items[item].SubItems[0].Text;
                        string actStr = "";

                        if (SoundRecords.Keys.Contains(Key) == true)
                        {
                            SoundRecord Данные = SoundRecords[Key];

                            КарточкаДвиженияПоезда Карточка = new КарточкаДвиженияПоезда(Данные);
                            if (Карточка.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                SoundRecord СтарыеДанные = Данные;
                                Данные = Карточка.ПолучитьИзмененнуюКарточку();
                                Данные = ИзменениеДанныхВКарточке(СтарыеДанные, Данные, Key);


                                //Изменение названия поезда
                                switch (listView.Name)
                                {
                                    case "listView1":
                                        if (listView.Items[item].SubItems[3].Text != Данные.НазваниеПоезда)
                                            listView.Items[item].SubItems[3].Text = Данные.НазваниеПоезда;
                                        break;

                                    case "lVПрибытие":
                                    case "lVОтправление":
                                        if (listView.Items[item].SubItems[4].Text != Данные.НазваниеПоезда)
                                            listView.Items[item].SubItems[4].Text = Данные.НазваниеПоезда;
                                        break;

                                    case "lVТранзит":
                                        if (listView.Items[item].SubItems[5].Text != Данные.НазваниеПоезда)
                                            listView.Items[item].SubItems[5].Text = Данные.НазваниеПоезда;
                                        break;
                                }

                                //Обновить Время ПРИБ
                                if ((Данные.БитыАктивностиПолей & 0x04) != 0x00)
                                {
                                    actStr = Данные.ВремяПрибытия.ToString("HH:mm:ss");
                                    switch (listView.Name)
                                    {
                                        case "listView1":
                                            if (listView.Items[item].SubItems[4].Text != actStr)
                                                listView.Items[item].SubItems[4].Text = actStr;
                                            break;

                                        case "lVПрибытие":
                                        case "lVТранзит":
                                            if (listView.Items[item].SubItems[3].Text != actStr)
                                                listView.Items[item].SubItems[3].Text = actStr;
                                            break;
                                    }
                                }

                                //Обновить Время ОТПР
                                if ((Данные.БитыАктивностиПолей & 0x10) != 0x00)
                                {
                                    actStr = Данные.ВремяОтправления.ToString("HH:mm:ss");
                                    switch (listView.Name)
                                    {
                                        case "listView1":
                                            if (listView.Items[item].SubItems[5].Text != actStr)
                                                listView.Items[item].SubItems[5].Text = actStr;
                                            break;

                                        case "lVТранзит":
                                            if (listView.Items[item].SubItems[4].Text != actStr)
                                                listView.Items[item].SubItems[4].Text = actStr;
                                            break;

                                        case "lVОтправление":
                                            if (listView.Items[item].SubItems[3].Text != actStr)
                                                listView.Items[item].SubItems[3].Text = actStr;
                                            break;
                                    }
                                }


                                if (SoundRecords.ContainsKey(Key) == false)  // поменяли время приб. или отпр. т.е. изменили ключ записи. 
                                {
                                    ОбновитьСписокЗвуковыхСообщенийВТаблице(); //Перерисуем список на UI.
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



        private void lVПрибытие_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.ContextMenuStrip != null)
                this.ContextMenuStrip = null;

            if (e.Button == MouseButtons.Right)
            {
                ListView list = sender as ListView;
                if ((list.Name == "lVПрибытие") || (list.Name == "lVТранзит") || (list.Name == "lVОтправление") || (list.Name == "listView1"))
                {
                    if (list.SelectedIndices.Count > 0)
                    {
                        this.ContextMenuStrip = this.contextMenuStrip1;
                        try
                        {
                            ListView.SelectedIndexCollection sic = list.SelectedIndices;

                            foreach (int item in sic)
                            {
                                if (item <= SoundRecords.Count)
                                {
                                    string Key = list.Items[item].SubItems[0].Text;

                                    if (SoundRecords.Keys.Contains(Key) == true)
                                    {
                                        SoundRecord Данные = SoundRecords[Key];
                                        КлючВыбранныйМеню = Key;


                                        for (int i = 0; i < СписокПолейПути.Length - 1; i++)
                                        {
                                            if (i < Program.НомераПутей.Count)
                                            {
                                                СписокПолейПути[i + 1].Text = Program.НомераПутей[i];
                                                СписокПолейПути[i + 1].Visible = true;
                                            }
                                            else
                                            {
                                                СписокПолейПути[i + 1].Visible = false;
                                            }
                                        }

                                        foreach (ToolStripMenuItem t in СписокПолейПути)
                                            t.Checked = false;

                                        int НомерПути = Program.НомераПутей.IndexOf(Данные.НомерПути) + 1;
                                        if (НомерПути >= 1 && НомерПути < СписокПолейПути.Length)
                                            СписокПолейПути[НомерПути].Checked = true;
                                        else
                                            СписокПолейПути[0].Checked = true;


                                        ToolStripMenuItem[] СписокНумерацииВагонов = new ToolStripMenuItem[] { отсутсвуетToolStripMenuItem, сГоловыСоставаToolStripMenuItem, сХвостаСоставаToolStripMenuItem };
                                        for (int i = 0; i < СписокНумерацииВагонов.Length; i++)
                                            СписокНумерацииВагонов[i].Checked = false;

                                        if (Данные.НумерацияПоезда <= 2)
                                            СписокНумерацииВагонов[Данные.НумерацияПоезда].Checked = true;


                                        ToolStripMenuItem[] СписокКоличестваПовторов = new ToolStripMenuItem[] { null, повтор1ToolStripMenuItem, повтор2ToolStripMenuItem, повтор3ToolStripMenuItem };
                                        for (int i = 1; i < СписокКоличестваПовторов.Length; i++)
                                            СписокКоличестваПовторов[i].Checked = false;

                                        if (Данные.КоличествоПовторений >= 1 && Данные.КоличествоПовторений <= 3)
                                            СписокКоличестваПовторов[Данные.КоличествоПовторений].Checked = true;


                                        var вариантыОтображенияПути = Табло_отображениеПутиToolStripMenuItem.DropDownItems;
                                        for (int i = 0; i < вариантыОтображенияПути.Count; i++)
                                        {
                                            var menuItem = вариантыОтображенияПути[i] as ToolStripMenuItem;
                                            if (menuItem != null)
                                            {
                                                menuItem.Checked = (i == (int)Данные.РазрешениеНаОтображениеПути);
                                            }
                                        }



                                        шаблоныОповещенияToolStripMenuItem1.DropDownItems.Clear();
                                        for (int i = 0; i < Данные.СписокФормируемыхСообщений.Count(); i++)
                                        {
                                            var Сообщение = Данные.СписокФормируемыхСообщений[i];
                                            ToolStripMenuItem tsmi = new ToolStripMenuItem(Сообщение.НазваниеШаблона);
                                            tsmi.Size = new System.Drawing.Size(165, 22);
                                            tsmi.Name = "ШаблонОповещения" + i.ToString();
                                            tsmi.Checked = Сообщение.Активность;
                                            tsmi.Click += new System.EventHandler(this.путь1ToolStripMenuItem_Click);
                                            шаблоныОповещенияToolStripMenuItem1.DropDownItems.Add(tsmi);
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
                }
                else if (list.Name == "lVСтатическиеСообщения")
                {
                    if (list.SelectedIndices.Count > 0)
                    {
                        this.ContextMenuStrip = this.contextMenuStrip2;
                        try
                        {
                            ListView.SelectedIndexCollection sic = this.lVСтатическиеСообщения.SelectedIndices;

                            foreach (int item in sic)
                            {
                                if (item <= СтатическиеЗвуковыеСообщения.Count)
                                {
                                    string Key = this.lVСтатическиеСообщения.Items[item].SubItems[0].Text;

                                    if (СтатическиеЗвуковыеСообщения.Keys.Contains(Key) == true)
                                    {
                                        СтатическоеСообщение Данные = СтатическиеЗвуковыеСообщения[Key];
                                        включитьToolStripMenuItem.Text = Данные.Активность == true ? "Отключить" : "Включить";
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                //                contextMenuStrip1.Items.Add(list.Name);
            }
        }



        public static void ВоспроизвестиШаблонОповещения(string ТипСообщения, SoundRecord Record, СостояниеФормируемогоСообщенияИШаблон формируемоеСообщение)
        {
            string Text;
            string logMessage = "";

            string[] НазваниеФайловПутей = new string[] { "",   "На 1ый путь", "На 2ой путь", "На 3ий путь", "На 4ый путь", "На 5ый путь", "На 6ой путь", "На 7ой путь", "На 8ой путь", "На 9ый путь", "На 10ый путь", "На 11ый путь", "На 12ый путь", "На 13ый путь", "На 14ый путь", "На 15ый путь", "На 16ый путь", "На 17ый путь", "На 18ый путь", "На 19ый путь", "На 20ый путь", "На 21ый путь", "На 22ой путь", "На 23ий путь", "На 24ый путь", "На 25ый путь",
                                                                "На 1ом пути", "На 2ом пути", "На 3ем пути", "На 4ом пути", "На 5ом пути", "На 6ом пути", "На 7ом пути", "На 8ом пути", "На 9ом пути", "На 10ом пути", "На 11ом пути", "На 12ом пути", "На 13ом пути", "На 14ом пути", "На 15ом пути", "На 16ом пути", "На 17ом пути", "На 18ом пути", "На 19ом пути", "На 20ом пути", "На 21ом пути", "На 22ом пути", "На 23им пути", "На 24ом пути", "На 25ом пути",
                                                                "С 1ого пути", "С 2ого пути", "С 3его пути", "С 4ого пути", "С 5ого пути", "С 6ого пути", "С 7ого пути", "С 8ого пути", "С 9ого пути", "С 10ого пути", "С 11ого пути", "С 12ого пути", "С 13ого пути", "С 14ого пути", "С 15ого пути", "С 16ого пути", "С 17ого пути", "С 18ого пути", "С 19ого пути", "С 20ого пути", "С 21ого пути", "С 22ого пути", "С 23его пути", "С 24ого пути", "С 25ого пути" };

            string[] ФайлыМинут = new string[] { "00 минут", "01 минута", "02 минуты", "03 минуты", "04 минуты", "05 минут", "06 минут", "07 минут", "08 минут",
                        "09 минут", "10 минут", "11 минут", "12 минут", "13 минут", "14 минут", "15 минут", "16 минут", "17 минут",
                        "18 минут", "19 минут", "20 минут", "21 минута", "22 минуты", "23 минуты", "24 минуты", "25 минут", "26 минут",
                        "27 минут", "28 минут", "29 минут", "30 минут", "31 минута", "32 минуты", "33 минуты", "34 минуты", "35 минут",
                        "36 минут", "37 минут", "38 минут", "39 минут", "40 минут", "41 минута", "42 минуты", "43 минуты", "44 минуты",
                        "45 минут", "46 минут", "47 минут", "48 минут", "49 минут", "50 минут", "51 минута", "52 минуты", "53 минуты",
                        "54 минуты", "55 минут", "56 минут", "57 минут", "58 минут", "59 минут" };


            string[] ФайлыЧасов = new string[] { "В 00 часов", "В 01 час", "В 02 часа", "В 03 часа", "В 04 часа", "В 05 часов", "В 06 часов", "В 07 часов",
                                                                                        "В 08 часов", "В 09 часов", "В 10 часов", "В 11 часов", "В 12 часов", "В 13 часов", "В 14 часов", "В 15 часов",
                                                                                        "В 16 часов", "В 17 часов", "В 18 часов", "В 19 часов", "В 20 часов", "В 21 час", "В 22 часа", "В 23 часа" };

            string[] НазваниеФайловНумерацииПутей = new string[] { "", "Нумерация с головы", "Нумерация с хвоста" };

            var воспроизводимыеСообщения = new List<ВоспроизводимоеСообщение>();

            //удалить англ. язык, если запрешенно произношения на аннглийском для данного типа поезда.
            if (!((Record.ТипПоезда == ТипПоезда.Пассажирский && Program.Настройки.EngСообщНаПассажирскийПоезд) ||
                (Record.ТипПоезда == ТипПоезда.Пригородный && Program.Настройки.EngСообщНаПригородныйЭлектропоезд) ||
                (Record.ТипПоезда == ТипПоезда.Скоростной && Program.Настройки.EngСообщНаСкоростнойПоезд) ||
                (Record.ТипПоезда == ТипПоезда.Скорый && Program.Настройки.EngСообщНаСкорыйПоезд) ||
                (Record.ТипПоезда == ТипПоезда.Ласточка && Program.Настройки.EngСообщНаЛасточку) ||
                (Record.ТипПоезда == ТипПоезда.Фирменный && Program.Настройки.EngСообщНаФирменный) ||
                (Record.ТипПоезда == ТипПоезда.РЭКС && Program.Настройки.EngСообщНаРЭКС)))
            {
                формируемоеСообщение.ЯзыкиОповещения.Remove(NotificationLanguage.Eng);
            }




            string[] элементыШаблона = формируемоеСообщение.Шаблон.Split('|');
            foreach (var язык in формируемоеСообщение.ЯзыкиОповещения)
            {
                foreach (string шаблон in элементыШаблона)
                {
                    int ВидНомерацииПути = 0;
                    switch (шаблон)
                    {
                        case "НА НОМЕР ПУТЬ":
                        case "НА НОМЕРом ПУТИ":
                        case "С НОМЕРого ПУТИ":
                            if (шаблон == "НА НОМЕРом ПУТИ") ВидНомерацииПути = 1;
                            if (шаблон == "С НОМЕРого ПУТИ") ВидНомерацииПути = 2;
                            if (Program.НомераПутей.Contains(Record.НомерПути))
                            {
                                Text = НазваниеФайловПутей[Program.НомераПутей.IndexOf(Record.НомерПути) + 1 + ВидНомерацииПути * 25];
                                logMessage += Text + " ";
                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = Text, Язык = язык });
                            }
                            break;


                        case "СТ.ОТПРАВЛЕНИЯ":
                            Text = Record.СтанцияОтправления;
                            logMessage += Text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = Text, Язык = язык });
                            break;


                        case "НОМЕР ПОЕЗДА":
                            Text = Record.НомерПоезда;
                            logMessage += Text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = Text, Язык = язык });
                            break;


                        case "ДОПОЛНЕНИЕ":
                            if (Record.ИспользоватьДополнение)
                            {
                                Text = Record.Дополнение;
                                logMessage += Text + " ";
                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                {
                                    ИмяВоспроизводимогоФайла = Text,
                                    Язык = язык
                                });
                            }
                            break;


                        case "СТ.ПРИБЫТИЯ":
                            Text = Record.СтанцияНазначения;
                            logMessage += Text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = Text, Язык = язык });
                            break;


                        case "ВРЕМЯ ПРИБЫТИЯ":
                            logMessage += "Время прибытия: ";
                            Text = Record.ВремяПрибытия.ToString("HH:mm");
                            logMessage += Text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = ФайлыЧасов[Record.ВремяПрибытия.Hour], Язык = язык });
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = ФайлыМинут[Record.ВремяПрибытия.Minute], Язык = язык });
                            continue;


                        case "ВРЕМЯ СТОЯНКИ":
                            logMessage += "Стоянка: ";
                            Text = Record.ВремяСтоянки.ToString() + " минут";
                            logMessage += Text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = ФайлыМинут[Record.ВремяСтоянки % 60], Язык = язык });
                            continue;


                        case "ВРЕМЯ ОТПРАВЛЕНИЯ":
                            logMessage += "Время отправления: ";
                            Text = Record.ВремяОтправления.ToString("HH:mm");
                            logMessage += Text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = ФайлыЧасов[Record.ВремяОтправления.Hour], Язык = язык });
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = ФайлыМинут[Record.ВремяОтправления.Minute], Язык = язык });
                            continue;


                        case "НУМЕРАЦИЯ СОСТАВА":
                            if ((Record.НумерацияПоезда > 0) && (Record.НумерацияПоезда <= 2))
                            {
                                Text = НазваниеФайловНумерацииПутей[Record.НумерацияПоезда];
                                logMessage += Text + " ";
                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = Text, Язык = язык });
                                continue;
                            }
                            break;


                        case "СТАНЦИИ":
                            if ((Record.ТипПоезда == ТипПоезда.Пригородный) || (Record.ТипПоезда == ТипПоезда.Ласточка) ||
                                (Record.ТипПоезда == ТипПоезда.РЭКС))
                            {
                                if (Record.Примечание.Contains("Со всеми остановками"))
                                {
                                    logMessage += "Электропоезд движется со всеми остановками ";
                                    if (Program.FilesFolder.Contains("СоВсемиОстановками"))
                                        воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = "СоВсемиОстановками", Язык = язык });
                                }
                                else if (Record.Примечание.Contains("С остановк"))
                                {
                                    logMessage += "Электропоезд движется с остановками на станциях: ";
                                    foreach (var Станция in Program.Станции)
                                        if (Record.Примечание.Contains(Станция))
                                            logMessage += Станция + " ";

                                    if (Program.FilesFolder.Contains("СОстановками"))
                                        воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = "СОстановками", Язык = язык });

                                    foreach (var Станция in Program.Станции)
                                        if (Record.Примечание.Contains(Станция))
                                            if (Program.FilesFolder.Contains(Станция))
                                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = Станция, Язык = язык });
                                }
                                else if (Record.Примечание.Contains("Кроме"))
                                {
                                    logMessage += "Электропоезд движется с остановками кроме станций: ";
                                    foreach (var Станция in Program.Станции)
                                        if (Record.Примечание.Contains(Станция))
                                            logMessage += Станция + " ";

                                    if (Program.FilesFolder.Contains("СОстановкамиКроме"))
                                        воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = "СОстановкамиКроме", Язык = язык });

                                    foreach (var Станция in Program.Станции)
                                        if (Record.Примечание.Contains(Станция))
                                            if (Program.FilesFolder.Contains(Станция))
                                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = Станция, Язык = язык });
                                }
                            }
                            break;


                        default:
                            logMessage += шаблон + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = шаблон, Язык = язык });
                            break;
                    }
                }

                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = "СТОП ", Язык = язык, ВремяПаузы = (int)(Program.Настройки.ЗадержкаМеждуЗвуковымиСообщениями * 10.0) });
            }

            for (int i = 0; i < Record.КоличествоПовторений; i++)
                for (int j = 0; j < воспроизводимыеСообщения.Count(); j++)
                    ОчередьВоспроизводимыхЗвуковыхСообщений.Add(воспроизводимыеСообщения[j]);

            Program.ЗаписьЛога(ТипСообщения, "Формирование звукового сообщения: " + logMessage + ". Повтор " + Record.КоличествоПовторений.ToString() + " раз.");
        }



        private void listView5_Enter(object sender, EventArgs e)
        {
            if (this.ContextMenuStrip != null)
                this.ContextMenuStrip = null;
        }



        private void воспроизвестиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ListView.SelectedIndexCollection sic = this.lVСтатическиеСообщения.SelectedIndices;

                foreach (int item in sic)
                {
                    if (item <= СтатическиеЗвуковыеСообщения.Count)
                    {
                        string Key = this.lVСтатическиеСообщения.Items[item].SubItems[0].Text;

                        if (СтатическиеЗвуковыеСообщения.Keys.Contains(Key) == true)
                        {
                            СтатическоеСообщение Данные = СтатическиеЗвуковыеСообщения[Key];
                            foreach (var Sound in StaticSoundForm.StaticSoundRecords)
                            {
                                if (Sound.Name == Данные.НазваниеКомпозиции)
                                {
                                    Program.ЗаписьЛога("Действие оператора", "Воспроизведение звукового сообщения: " + Sound.Name);
                                    var воспроизводимоеСообщение = new ВоспроизводимоеСообщение { ИмяВоспроизводимогоФайла = Sound.Name, Язык = NotificationLanguage.Ru };
                                    MainWindowForm.ОчередьВоспроизводимыхЗвуковыхСообщений.Add(воспроизводимоеСообщение);
                                    break;
                                }
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



        private void включитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ListView.SelectedIndexCollection sic = this.lVСтатическиеСообщения.SelectedIndices;

                foreach (int item in sic)
                {
                    if (item <= СтатическиеЗвуковыеСообщения.Count)
                    {
                        string Key = this.lVСтатическиеСообщения.Items[item].SubItems[0].Text;

                        if (СтатическиеЗвуковыеСообщения.Keys.Contains(Key) == true)
                        {
                            СтатическоеСообщение Данные = СтатическиеЗвуковыеСообщения[Key];
                            Данные.Активность = !Данные.Активность;
                            Program.ЗаписьЛога("Действие оператора", (Данные.Активность ? "Включение " : "Отключение ") + "звукового сообщения: \"" + Данные.НазваниеКомпозиции + "\" (" + Данные.Время.ToString("HH:mm") + ")");
                            СтатическиеЗвуковыеСообщения[Key] = Данные;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        private void путь1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;

            try
            {
                if (SoundRecords.Keys.Contains(КлючВыбранныйМеню) == true)
                {
                    SoundRecord Данные = SoundRecords[КлючВыбранныйМеню];
                    SoundRecord НеИзмененныеДанные = Данные;

                    for (int i = 0; i < СписокПолейПути.Length; i++)
                        if (СписокПолейПути[i].Name == tsmi.Name)
                        {
                            string СтарыйНомерПути = Данные.НомерПути;
                            Данные.НомерПути = i == 0 ? "" : Program.НомераПутей[i - 1];
                            if (СтарыйНомерПути != Данные.НомерПути) Program.ЗаписьЛога("Действие оператора", "Изменение настроек поезда: " + Данные.НомерПоезда + " " + Данные.НазваниеПоезда + ": " + "Путь: " + СтарыйНомерПути + " -> " + Данные.НомерПути + "; ");

                            Данные.ТипСообщения = SoundRecordType.ДвижениеПоезда;
                            byte номерПути = Program.ПолучитьНомерПути(Данные.НомерПути);
                            Данные.НазванияТабло = номерПути != 0 ? MainWindowForm.Binding2PathBehaviors.Select(beh => beh.GetDevicesName4Path((byte)номерПути)).Where(str => str != null).ToArray() : null;

                            SoundRecords[КлючВыбранныйМеню] = Данные;
                            return;
                        }


                    ToolStripMenuItem[] СписокНумерацииВагонов = new ToolStripMenuItem[] { отсутсвуетToolStripMenuItem, сГоловыСоставаToolStripMenuItem, сХвостаСоставаToolStripMenuItem };
                    string[] СтроковыйСписокНумерацииВагонов = new string[] { "отсутсвуетToolStripMenuItem", "сГоловыСоставаToolStripMenuItem", "сХвостаСоставаToolStripMenuItem" };
                    if (СтроковыйСписокНумерацииВагонов.Contains(tsmi.Name))
                        for (int i = 0; i < СтроковыйСписокНумерацииВагонов.Length; i++)
                            if (СтроковыйСписокНумерацииВагонов[i] == tsmi.Name)
                            {
                                byte СтараяНумерацияПоезда = Данные.НумерацияПоезда;
                                Данные.НумерацияПоезда = (byte)i;
                                if (СтараяНумерацияПоезда != Данные.НумерацияПоезда) Program.ЗаписьЛога("Действие оператора", "Изменение настроек поезда: " + Данные.НомерПоезда + " " + Данные.НазваниеПоезда + ": " + "Нум.пути: " + СтараяНумерацияПоезда.ToString() + " -> " + Данные.НумерацияПоезда.ToString() + "; ");
                                SoundRecords[КлючВыбранныйМеню] = Данные;
                                return;
                            }


                    ToolStripMenuItem[] СписокКоличестваПовторов = new ToolStripMenuItem[] { повтор1ToolStripMenuItem, повтор2ToolStripMenuItem, повтор3ToolStripMenuItem };
                    string[] СтроковыйСписокКоличестваПовторов = new string[] { "повтор1ToolStripMenuItem", "повтор2ToolStripMenuItem", "повтор3ToolStripMenuItem" };
                    if (СтроковыйСписокКоличестваПовторов.Contains(tsmi.Name))
                        for (int i = 0; i < СтроковыйСписокКоличестваПовторов.Length; i++)
                            if (СтроковыйСписокКоличестваПовторов[i] == tsmi.Name)
                            {
                                byte СтароеКоличествоПовторений = Данные.КоличествоПовторений;
                                Данные.КоличествоПовторений = (byte)(i + 1);
                                if (СтароеКоличествоПовторений != Данные.КоличествоПовторений) Program.ЗаписьЛога("Действие оператора", "Изменение настроек поезда: " + Данные.НомерПоезда + " " + Данные.НазваниеПоезда + ": " + "Кол.повт.: " + СтароеКоличествоПовторений.ToString() + " -> " + Данные.КоличествоПовторений.ToString() + "; ");
                                SoundRecords[КлючВыбранныйМеню] = Данные;
                                return;
                            }


                    if (шаблоныОповещенияToolStripMenuItem1.DropDownItems.Contains(tsmi))
                    {
                        int ИндексШаблона = шаблоныОповещенияToolStripMenuItem1.DropDownItems.IndexOf(tsmi);
                        if (ИндексШаблона >= 0 && ИндексШаблона < 10 && ИндексШаблона < Данные.СписокФормируемыхСообщений.Count)
                        {
                            var ФормируемоеСообщение = Данные.СписокФормируемыхСообщений[ИндексШаблона];
                            ФормируемоеСообщение.Активность = !tsmi.Checked;
                            Данные.СписокФормируемыхСообщений[ИндексШаблона] = ФормируемоеСообщение;
                            SoundRecords[КлючВыбранныйМеню] = Данные;
                            return;
                        }
                    }


                    if (Табло_отображениеПутиToolStripMenuItem.DropDownItems.Contains(tsmi))
                    {
                        int индексВарианта = Табло_отображениеПутиToolStripMenuItem.DropDownItems.IndexOf(tsmi);
                        if (индексВарианта >= 0)
                        {
                            Данные.РазрешениеНаОтображениеПути = (PathPermissionType)индексВарианта;
                            SoundRecords[КлючВыбранныйМеню] = Данные;
                            return;
                        }
                    }


                    ОбновитьСостояниеЗаписейТаблицы();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void ОставитьОднуАктивнуюСтрокуВСпискеБлижайшихСобытий()
        {
            var последнееАктивноеСобытие = СписокБлижайшихСобытий.LastOrDefault(ev => ev.Value.СостояниеСтроки == 4);

            for (int i = 0; i < СписокБлижайшихСобытий.Count; i++)
            {
                var key = СписокБлижайшихСобытий.ElementAt(i).Key;
                var событие = СписокБлижайшихСобытий.ElementAt(i).Value;

                //Сбросить ранее выставленные события на дефолтное состояние
                if (событие.СостояниеСтроки == 4 && key != последнееАктивноеСобытие.Key)
                {
                    событие.СостояниеСтроки = (byte)((событие.СостояниеСтроки == 0) ? 1 : 2);
                    СписокБлижайшихСобытий[key] = событие;
                }
            }
        }



        private void lVСобытия_ОбновитьСостояниеТаблицы()
        {
            int НомерСтроки = 0;
            foreach (var Данные in СписокБлижайшихСобытий)
            {
                if (НомерСтроки >= lVСобытия.Items.Count)
                {
                    ListViewItem lvi1 = new ListViewItem(new string[] { Данные.Key, Данные.Value.Описание });
                    switch (Данные.Value.СостояниеСтроки)
                    {
                        case 0: lvi1.BackColor = Color.LightGray; break;
                        case 1: lvi1.BackColor = Color.White; break;
                        case 2: lvi1.BackColor = Color.LightGreen; break;
                        case 3: lvi1.BackColor = Color.Orange; break;
                        case 4: lvi1.BackColor = Color.CadetBlue; break;
                    }
                    lVСобытия.Items.Add(lvi1);
                }
                else
                {
                    if (lVСобытия.Items[НомерСтроки].SubItems[0].Text != Данные.Key)
                        lVСобытия.Items[НомерСтроки].SubItems[0].Text = Данные.Key;
                    if (lVСобытия.Items[НомерСтроки].SubItems[1].Text != Данные.Value.Описание)
                        lVСобытия.Items[НомерСтроки].SubItems[1].Text = Данные.Value.Описание;
                    switch (Данные.Value.СостояниеСтроки)
                    {
                        case 0: if (lVСобытия.Items[НомерСтроки].BackColor != Color.LightGray) lVСобытия.Items[НомерСтроки].BackColor = Color.LightGray; break;
                        case 1: if (lVСобытия.Items[НомерСтроки].BackColor != Color.White) lVСобытия.Items[НомерСтроки].BackColor = Color.White; break;
                        case 2: if (lVСобытия.Items[НомерСтроки].BackColor != Color.LightGreen) lVСобытия.Items[НомерСтроки].BackColor = Color.LightGreen; break;
                        case 3: if (lVСобытия.Items[НомерСтроки].BackColor != Color.Orange) lVСобытия.Items[НомерСтроки].BackColor = Color.Orange; break;
                        case 4: if (lVСобытия.Items[НомерСтроки].BackColor != Color.CadetBlue) lVСобытия.Items[НомерСтроки].BackColor = Color.CadetBlue; break;
                    }
                }

                НомерСтроки++;
            }

            while (НомерСтроки < lVСобытия.Items.Count)
                lVСобытия.Items.RemoveAt(НомерСтроки);
        }



        private void ОтобразитьСубтитры()
        {
            var subtaitles = СписокБлижайшихСобытий.Values.FirstOrDefault(ev => ev.СостояниеСтроки == 4);

            if (subtaitles.СостояниеСтроки == 4)
            {
                if (subtaitles.НомерСписка == 1) //статические звуковые сообщения
                {
                    if (СтатическиеЗвуковыеСообщения.Keys.Contains(subtaitles.Ключ))
                    {
                        rtb_subtaitles.Text = subtaitles.ШаблонИлиСообщение;
                    }
                }
                else
                if (subtaitles.НомерСписка == 0) //динамические звуковые сообщения
                {
                    if (string.IsNullOrEmpty(rtb_subtaitles.Text))
                    {
                        var card = new КарточкаДвиженияПоезда(SoundRecords[subtaitles.Ключ]);
                        card.ОтобразитьШаблонОповещенияНаRichTb(subtaitles.ШаблонИлиСообщение, rtb_subtaitles);
                    }
                }
            }
            else
            {
                rtb_subtaitles.Text = string.Empty;
            }
        }



        private void lVСобытия_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                string Key = lVСобытия.SelectedItems[0].SubItems[0].Text;
                if (СписокБлижайшихСобытий.ContainsKey(Key))
                {
                    ОписаниеСобытия ДанныеСтроки = СписокБлижайшихСобытий[Key];
                    if (ДанныеСтроки.НомерСписка == 1)
                    {
                        Key = ДанныеСтроки.Ключ;
                        if (СтатическиеЗвуковыеСообщения.Keys.Contains(Key))
                        {
                            СтатическоеСообщение Данные = СтатическиеЗвуковыеСообщения[Key];
                            КарточкаСтатическогоЗвуковогоСообщения Карточка = new КарточкаСтатическогоЗвуковогоСообщения(Данные);
                            if (Карточка.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                Данные = Карточка.ПолучитьИзмененнуюКарточку();

                                string Key2 = Данные.Время.ToString("HH:mm:ss");
                                string[] SubKeys = Key.Split(':');
                                if (SubKeys[0].Length == 1)
                                    Key2 = "0" + Key2;

                                if (Key == Key2)
                                {
                                    СтатическиеЗвуковыеСообщения[Key] = Данные;
                                    for (int i = 0; i < lVСтатическиеСообщения.Items.Count; i++)
                                        if (lVСтатическиеСообщения.Items[i].SubItems[0].Text == Key)
                                            if (lVСтатическиеСообщения.Items[i].SubItems[1].Text != Данные.НазваниеКомпозиции)
                                            {
                                                lVСтатическиеСообщения.Items[i].SubItems[1].Text = Данные.НазваниеКомпозиции;
                                                break;
                                            }
                                }
                                else
                                {
                                    СтатическиеЗвуковыеСообщения.Remove(Key);

                                    int ПопыткиВставитьСообщение = 5;
                                    while (ПопыткиВставитьСообщение-- > 0)
                                    {
                                        Key2 = Данные.Время.ToString("HH:mm:ss");
                                        SubKeys = Key2.Split(':');
                                        if (SubKeys[0].Length == 1)
                                            Key2 = "0" + Key2;

                                        if (СтатическиеЗвуковыеСообщения.ContainsKey(Key2))
                                        {
                                            Данные.Время = Данные.Время.AddSeconds(20);
                                            continue;
                                        }

                                        СтатическиеЗвуковыеСообщения.Add(Key2, Данные);
                                        break;
                                    }

                                    ОбновитьСписокЗвуковыхСообщенийВТаблицеСтатическихСообщений();
                                }
                            }
                        }
                    }
                    else // Динамические сообщения
                    {
                        Key = ДанныеСтроки.Ключ;
                        if (SoundRecords.Keys.Contains(Key) == true)
                        {
                            SoundRecord Данные = SoundRecords[Key];
                            КарточкаДвиженияПоезда Карточка = new КарточкаДвиженияПоезда(Данные);
                            if (Карточка.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                SoundRecord СтарыеДанные = Данные;
                                Данные = Карточка.ПолучитьИзмененнуюКарточку();
                                Данные = ИзменениеДанныхВКарточке(СтарыеДанные, Данные, Key);
                                ОбновитьСостояниеЗаписейТаблицы();
                            }
                        }
                    }

                    ОбновитьСостояниеЗаписейТаблицы();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private SoundRecord ИзменениеДанныхВКарточке(SoundRecord СтарыеДанные, SoundRecord Данные, string Key)
        {
            Данные.ТипСообщения = SoundRecordType.ДвижениеПоезда;

            string НомерПоезда = Данные.НомерПоезда;
            string НомерПути = Данные.НомерПути;
            string[] НазванияТабло = Данные.НазванияТабло;


            //если Поменяли время--------------------------------------------------------
            if ((СтарыеДанные.ВремяПрибытия != Данные.ВремяПрибытия) ||
                (СтарыеДанные.ВремяОтправления != Данные.ВремяОтправления))
            {
                Данные.Время = (Данные.БитыАктивностиПолей & 0x04) != 0x00 ? Данные.ВремяПрибытия : Данные.ВремяОтправления;

                //Если ключ с таким временем присутствует, изменить ключ добавляя секунды.
                int tryCounter = 50;
                while (--tryCounter > 0)
                {
                    string keyNew = Данные.Время.ToString("yy.MM.dd  HH:mm:ss");
                    string[] SubKeys = keyNew.Split(':');
                    if (SubKeys[0].Length == 1)
                        keyNew = "0" + keyNew;

                    if (SoundRecords.ContainsKey(keyNew) == false)       //Если с таким временем уже есть запись.
                    {
                        SoundRecords.Remove(Key);           //удалим старую запись
                        SoundRecordsOld.Remove(Key);
                        SoundRecords.Add(keyNew, Данные);   //Добавим запись под новым ключем
                        SoundRecordsOld.Add(keyNew, СтарыеДанные);

                        break;
                    }

                    Данные.Время = Данные.Время.AddSeconds(1);
                }
            }
            else
            {
                SoundRecords[Key] = Данные;
            }

            string СообщениеОбИзменениях = "";
            if (СтарыеДанные.НазваниеПоезда != Данные.НазваниеПоезда) СообщениеОбИзменениях += "Поезд: " + СтарыеДанные.НазваниеПоезда + " -> " + Данные.НазваниеПоезда + "; ";
            if (СтарыеДанные.НомерПоезда != Данные.НомерПоезда) СообщениеОбИзменениях += "№Поезда: " + СтарыеДанные.НомерПоезда + " -> " + Данные.НомерПоезда + "; ";
            if (СтарыеДанные.НомерПути != Данные.НомерПути) СообщениеОбИзменениях += "Путь: " + СтарыеДанные.НомерПути + " -> " + Данные.НомерПути + "; ";
            if (СтарыеДанные.НумерацияПоезда != Данные.НумерацияПоезда) СообщениеОбИзменениях += "Нум.пути: " + СтарыеДанные.НумерацияПоезда.ToString() + " -> " + Данные.НумерацияПоезда.ToString() + "; ";
            if (СтарыеДанные.СтанцияОтправления != Данные.СтанцияОтправления) СообщениеОбИзменениях += "Ст.Отпр.: " + СтарыеДанные.СтанцияОтправления + " -> " + Данные.СтанцияОтправления + "; ";
            if (СтарыеДанные.СтанцияНазначения != Данные.СтанцияНазначения) СообщениеОбИзменениях += "Ст.Назн.: " + СтарыеДанные.СтанцияНазначения + " -> " + Данные.СтанцияНазначения + "; ";
            if ((СтарыеДанные.БитыАктивностиПолей & 0x04) != 0x00) if (СтарыеДанные.ВремяПрибытия != Данные.ВремяПрибытия) СообщениеОбИзменениях += "Прибытие: " + СтарыеДанные.ВремяПрибытия.ToString("HH:mm") + " -> " + Данные.ВремяПрибытия.ToString("HH:mm") + "; ";
            if ((СтарыеДанные.БитыАктивностиПолей & 0x10) != 0x00) if (СтарыеДанные.ВремяОтправления != Данные.ВремяОтправления) СообщениеОбИзменениях += "Отправление: " + СтарыеДанные.ВремяОтправления.ToString("HH:mm") + " -> " + Данные.ВремяОтправления.ToString("HH:mm") + "; ";
            if (СообщениеОбИзменениях != "")
                Program.ЗаписьЛога("Действие оператора", "Изменение настроек поезда: " + СтарыеДанные.НомерПоезда + " " + СтарыеДанные.НазваниеПоезда + ": " + СообщениеОбИзменениях);

            return Данные;
        }
    }
}
