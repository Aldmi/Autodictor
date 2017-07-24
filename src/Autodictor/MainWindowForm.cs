using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Linq;
using System.Windows.Input;
using CommunicationDevices.Behavior.BindingBehavior.ToGeneralSchedule;
using CommunicationDevices.Behavior.BindingBehavior.ToPath;
using CommunicationDevices.ClientWCF;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;
using CommunicationDevices.Model;
using Domain.Concrete.NoSqlReposutory;
using Domain.Entitys;
using MainExample.Comparers;
using MainExample.Entites;
using MainExample.Extension;
using MainExample.Infrastructure;
using MainExample.Mappers;
using MainExample.Services;
using Library.Logs;
using MoreLinq;


namespace MainExample
{

    public struct SoundRecord
    {
        public int ID;
        public string НомерПоезда;
        public string НомерПоезда2;
        public string НазваниеПоезда;
        public string Направление;
        public string СтанцияОтправления;
        public string СтанцияНазначения;
        public DateTime Время;
        public DateTime ВремяПрибытия;
        public DateTime ВремяОтправления;
        public DateTime? ВремяЗадержки;                      //время задержки в мин. относительно времени прибытия или отправелния
        public DateTime ОжидаемоеВремя;                      //вычисляется ВремяПрибытия или ВремяОтправления + ВремяЗадержки
        public DateTime? ВремяСледования;                    //время в пути
        public TimeSpan? ВремяСтоянки;                       //вычисляется для танзитов (ВремяОтправления - ВремяПрибытия)
        public DateTime? ФиксированноеВремяПрибытия;         // фиксированное время
        public DateTime? ФиксированноеВремяОтправления;      // фиксированное время + время стоянки
        public string Дополнение;                            //свободная переменная для ввода  
        public Dictionary<string, bool> ИспользоватьДополнение; //[звук] - использовать дополнение для звука.  [табло] - использовать дополнение для табло.
        public string ДниСледования;
        public string ДниСледованияAlias;                    // дни следования записанные в ручную
        public bool Активность;
        public bool Автомат;                                 // true - поезд обрабатывается в автомате.
        public string ШаблонВоспроизведенияСообщений;
        public byte НумерацияПоезда;
        public string НомерПути;
        public string НомерПутиБезАвтосброса;                //выставленные пути не обнуляются через определенное время
        public ТипПоезда ТипПоезда;
        public string Примечание;                            //С остановками....
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
        public List<СостояниеФормируемогоСообщенияИШаблон> СписокНештатныхСообщений;
        public byte СостояниеКарточки;
        public string ОписаниеСостоянияКарточки;
        public byte БитыНештатныхСитуаций; // бит 0 - Отмена, бит 1 - задержка прибытия, бит 2 - задержка отправления, бит 3 - отправление по готовности
        public uint ТаймерПовторения;
    };

    public struct СостояниеФормируемогоСообщенияИШаблон
    {
        public int Id;                            // порядковый номер шаблона
        public int SoundRecordId;                 // строка расписания к которой принадлежит данный шаблон
        public bool Активность;
        public Priority Приоритет;
        public bool Воспроизведен;                //???
        public SoundRecordStatus СостояниеВоспроизведения;
        public int ПривязкаКВремени;              // 0 - приб. 1- отпр
        public int ВремяСмещения;
        public string НазваниеШаблона;
        public string Шаблон;
        public List<NotificationLanguage> ЯзыкиОповещения;
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

    public struct ОписаниеСобытия
    {
        public DateTime Время;
        public string Описание;
        public byte НомерСписка;            // 0 - Динамические сообщения, 1 - статические звуковые сообщения
        public string Ключ;
        public byte СостояниеСтроки;        // 0 - Выключена, 1 - движение поезда (динамика), 2 - статическое сообщение, 3 - аварийное сообщение, 4 - воспроизведение, 5 - воспроизведЕН
        public string ШаблонИлиСообщение;   //текст стат. сообщения, или номер шаблона в динам. сообщении (для Субтитров)
    };



    public partial class MainWindowForm : Form
    {
        private const int ВремяЗадержкиВоспроизведенныхСобытий = 20;  //сек


        private bool РазрешениеРаботы = false;

        public static SortedDictionary<string, SoundRecord> SoundRecords = new SortedDictionary<string, SoundRecord>();
        public static SortedDictionary<string, SoundRecord> SoundRecordsOld = new SortedDictionary<string, SoundRecord>();

        public static SortedDictionary<string, СтатическоеСообщение> СтатическиеЗвуковыеСообщения = new SortedDictionary<string, СтатическоеСообщение>();

        public static List<SoundRecordChanges> SoundRecordChanges = new List<SoundRecordChanges>();  //Изменения на тек.сутки + изменения на пред. сутки для поездов ходящих в тек. сутки

        public TaskManagerService TaskManager = new TaskManagerService();


        private bool ОбновлениеСписка = false;

        public static MainWindowForm myMainForm = null;

        public static QueueSoundService QueueSound = new QueueSoundService();

        private int VisibleMode = 0;

        public CisClient CisClient { get; }
        public static IEnumerable<IBinding2PathBehavior> Binding2PathBehaviors { get; set; }
        public static IEnumerable<IBinding2GeneralSchedule> Binding2GeneralScheduleBehaviors { get; set; }
        public Device SoundChanelManagment { get; }

        public IDisposable DispouseCisClientIsConnectRx { get; set; }
        public IDisposable DispouseQueueChangeRx { get; set; }
        public IDisposable DispouseStaticChangeRx { get; set; }
        public IDisposable DispouseTemplateChangeRx { get; set; }


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

            MainForm.Пауза.Click += new System.EventHandler(this.btnПауза_Click);
            MainForm.Остановить.Click += new System.EventHandler(this.btnОстановить_Click);
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


            DispouseQueueChangeRx = QueueSound.QueueChangeRx.Subscribe(status =>
            {
                switch (status)
                {
                    case StatusPlaying.Start:
                        СобытиеНачалоПроигрыванияОчередиЗвуковыхСообщений();
                        break;

                    case StatusPlaying.Stop:
                        СобытиеКонецПроигрыванияОчередиЗвуковыхСообщений();
                        break;
                }
            });

            DispouseStaticChangeRx = QueueSound.StaticChangeRx.Subscribe(StaticChangeRxEventHandler);
            DispouseTemplateChangeRx = QueueSound.TemplateChangeRx.Subscribe(TemplateChangeRxEventHandler);

            QueueSound.StartQueue();

            MainForm.Включить.BackColor = Color.Orange;
            Program.ЗаписьЛога("Системное сообщение", "Программный комплекс включен");
        }




        private void StaticChangeRxEventHandler(StaticChangeValue staticChangeValue)
        {
            for (int i = 0; i < СтатическиеЗвуковыеСообщения.Count(); i++)
            {
                string Key = СтатическиеЗвуковыеСообщения.ElementAt(i).Key;
                СтатическоеСообщение сообщение = СтатическиеЗвуковыеСообщения.ElementAt(i).Value;

                if (сообщение.ID == staticChangeValue.SoundMessage.RootId)
                {
                    switch (staticChangeValue.StatusPlaying)
                    {
                        case StatusPlaying.Start:
                            сообщение.СостояниеВоспроизведения = SoundRecordStatus.ВоспроизведениеАвтомат;
                            break;

                        case StatusPlaying.Stop:
                            сообщение.СостояниеВоспроизведения = SoundRecordStatus.Выключена;
                            break;
                    }
                    СтатическиеЗвуковыеСообщения[Key] = сообщение;
                }
            }
        }



        private void TemplateChangeRxEventHandler(TemplateChangeValue templateChangeValue)
        {
            //ШАБЛОН технического сообщения
            if (templateChangeValue.SoundMessage.ТипСообщения == ТипСообщения.ДинамическоеТехническое)
            {
                var soundRecordTech = TechnicalMessageForm.SoundRecords.FirstOrDefault(rec => rec.ID == templateChangeValue.Template.SoundRecordId);
                if (soundRecordTech.ID > 0)
                {
                    int index = TechnicalMessageForm.SoundRecords.IndexOf(soundRecordTech);
                    var template = soundRecordTech.СписокФормируемыхСообщений.FirstOrDefault(i => i.Id == templateChangeValue.Template.Id);
                    switch (templateChangeValue.StatusPlaying)
                    {
                        case StatusPlaying.Start:
                            template.СостояниеВоспроизведения = SoundRecordStatus.ВоспроизведениеРучное;
                            break;

                        case StatusPlaying.Stop:
                            template.СостояниеВоспроизведения = SoundRecordStatus.Выключена;
                            break;
                    }
                    soundRecordTech.СписокФормируемыхСообщений[0] = template;
                    TechnicalMessageForm.SoundRecords[index] = soundRecordTech;
                }
                return;
            }


            var soundRecord = SoundRecords.FirstOrDefault(rec => rec.Value.ID == templateChangeValue.Template.SoundRecordId);
            //шаблон АВАРИЯ
            if (templateChangeValue.SoundMessage.ТипСообщения == ТипСообщения.ДинамическоеАварийное)
            {
                for (int i = 0; i < soundRecord.Value.СписокНештатныхСообщений.Count; i++)
                {
                    if (soundRecord.Value.СписокНештатныхСообщений[i].Id == templateChangeValue.Template.Id)
                    {
                        var template = soundRecord.Value.СписокНештатныхСообщений[i];
                        switch (templateChangeValue.StatusPlaying)
                        {
                            case StatusPlaying.Start:
                                template.СостояниеВоспроизведения = SoundRecordStatus.ВоспроизведениеАвтомат;
                                break;

                            case StatusPlaying.Stop:
                                template.СостояниеВоспроизведения = SoundRecordStatus.Выключена;
                                break;
                        }
                        soundRecord.Value.СписокНештатныхСообщений[i] = template;
                    }
                }
            }
            //шаблон ДИНАМИКИ
            else
            {
                for (int i = 0; i < soundRecord.Value.СписокФормируемыхСообщений.Count; i++)
                {
                    if (soundRecord.Value.СписокФормируемыхСообщений[i].Id == templateChangeValue.Template.Id)
                    {
                        var template = soundRecord.Value.СписокФормируемыхСообщений[i];
                        switch (templateChangeValue.StatusPlaying)
                        {
                            case StatusPlaying.Start:
                                template.СостояниеВоспроизведения = (template.СостояниеВоспроизведения == SoundRecordStatus.ДобавленВОчередьРучное) ? SoundRecordStatus.ВоспроизведениеРучное : SoundRecordStatus.ВоспроизведениеАвтомат;
                                break;

                            case StatusPlaying.Stop:
                                template.СостояниеВоспроизведения = SoundRecordStatus.Выключена;
                                break;
                        }
                        soundRecord.Value.СписокФормируемыхСообщений[i] = template;
                    }
                }
            }

            if (SoundRecords.ContainsKey(soundRecord.Key))
                SoundRecords[soundRecord.Key] = soundRecord.Value;
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
        public void btnОбновитьСписок_Click(object sender, EventArgs e)
        {
            ОбновитьСписокЗвуковыхСообщений(sender, e);
            //ОбновитьСписокЗвуковыхСообщенийВТаблице();
            ОбновитьСписокЗвуковыхСообщенийВТаблицеСтатическихСообщений();
            ОбновитьСостояниеЗаписейТаблицы();

            ОчиститьВсеТабло();
            ИнициализироватьВсеТабло();

            MainForm.РежимРаботы.BackColor = Color.LightGray;
            MainForm.РежимРаботы.Text = @"Пользовательский";
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
                if (!string.IsNullOrEmpty(данные.НомерПути))
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

            СозданиеРасписанияЖдТранспорта();
            СозданиеСтатическихЗвуковыхФайлов();
        }




        /// <summary>
        /// Созданире обобщенного списка из основного и оперативного расписания
        /// </summary>
        public void СозданиеРасписанияЖдТранспорта()
        {
            int id = 1;

            //загрузим список изменений на текущий день.
            var currentDay = DateTime.Now.Date;
            SoundRecordChanges = Program.SoundRecordChangesDbRepository.List()
                                                                       .Where(p => (p.TimeStamp.Date == currentDay) ||
                                                                                  ((p.TimeStamp.Date == currentDay.AddDays(-1)) && (p.Rec.Время.Date == currentDay )))
                                                                       .Select(Mapper.SoundRecordChangesDb2SoundRecordChanges).ToList();

            //Добавим весь список Оперативного расписания
            СозданиеЗвуковыхФайловРасписанияЖдТранспорта(TrainTableOperative.TrainTableRecords, DateTime.Now, null, ref id);                                         // на тек. сутки
            СозданиеЗвуковыхФайловРасписанияЖдТранспорта(TrainTableOperative.TrainTableRecords, DateTime.Now.AddDays(1), hour => (hour >= 0 && hour <= 11), ref id); // на след. сутки на 2 первых часа

            //Вычтем из Главного расписания элементы оперативного расписания, уже добавленные к списку.
            var differences = TrainTable.TrainTableRecords.Where(l2 =>
                  !SoundRecords.Values.Any(l1 =>
                  l1.НомерПоезда == l2.Num &&
                  l1.НомерПоезда2 == l2.Num2 &&
                  l1.Направление == l2.Direction
                  )).ToList();

            //Добавим оставшиеся записи
            СозданиеЗвуковыхФайловРасписанияЖдТранспорта(differences, DateTime.Now, null, ref id);                                         // на тек. сутки
            СозданиеЗвуковыхФайловРасписанияЖдТранспорта(differences, DateTime.Now.AddDays(1), hour => (hour >= 0 && hour <= 11), ref id); // на след. сутки на 2 первых часа

            //Корректировка записей по изменениям
            КорректировкаЗаписейПоИзменениям();
        }



        private void СозданиеЗвуковыхФайловРасписанияЖдТранспорта(IList<TrainTableRecord> trainTableRecords, DateTime день, Func<int, bool> ограничениеВремениПоЧасам, ref int id)
        {
            var pipelineService = new SchedulingPipelineService();
            for (var index = 0; index < trainTableRecords.Count; index++)
            {
                var config = trainTableRecords[index];
                if (config.Active == false && Program.Настройки.РазрешениеДобавленияЗаблокированныхПоездовВСписок == false)
                    continue;

                if (!pipelineService.CheckTrainActuality(ref config, день, ограничениеВремениПоЧасам, РаботаПоНомеруДняНедели))
                    continue;

                var newId = id++;
                SoundRecord record = Mapper.MapTrainTableRecord2SoundRecord(config, день, newId);


                //выдать список привязанных табло
                record.НазванияТабло = record.НомерПути != "0" ? Binding2PathBehaviors.Select(beh => beh.GetDevicesName4Path(record.НомерПути)).Where(str => str != null).ToArray() : null;
                record.СостояниеОтображения = TableRecordStatus.Выключена;


                //СБРОСИТЬ НОМЕР ПУТИ, НА ВРЕМЯ МЕНЬШЕ ТЕКУЩЕГО
                if (record.Время < DateTime.Now)
                {
                    record.НомерПути = string.Empty;
                    record.НомерПутиБезАвтосброса = string.Empty;
                }


                //Добавление созданной записи
                var newkey = pipelineService.GetUniqueKey(SoundRecords.Keys, record.Время);
                if (!string.IsNullOrEmpty(newkey))
                {
                    record.Время = DateTime.ParseExact(newkey, "yy.MM.dd  HH:mm:ss", new DateTimeFormatInfo());
                    SoundRecords.Add(newkey, record);
                    SoundRecordsOld.Add(newkey, record);
                }

                MainWindowForm.ФлагОбновитьСписокЖелезнодорожныхСообщенийВТаблице = true;
            }
        }



        private void КорректировкаЗаписейПоИзменениям()
        {
            //фильтрация по последним изменениям. среди элементов с одинаковым Id выбрать элементы с большей датой.
            var filtredOnMaxDate = SoundRecordChanges.GroupBy(gr => gr.NewRec.ID)
                .Select(elem => elem.MaxBy(b => b.TimeStamp))
                .ToList();

            for (int i = 0; i < SoundRecords.Count; i++)
            {
                var key = SoundRecords.Keys.ElementAt(i);
                var rec = SoundRecords[key];

                if (rec.НомерПоезда == "6808")
                {
                    var h = 5 + 5;
                }

                var change = filtredOnMaxDate.FirstOrDefault(f => (f.Rec.НомерПоезда == rec.НомерПоезда) &&
                                                                  (f.Rec.НомерПоезда2 == rec.НомерПоезда2)&&
                                                                  (f.Rec.Время.Date == rec.Время.Date));
                if (change != null)
                {
                    var keyNew = change.Rec.Время.ToString("yy.MM.dd  HH:mm:ss");
                    SoundRecords.Remove(keyNew);

                    keyNew = change.NewRec.Время.ToString("yy.MM.dd  HH:mm:ss");
                    SoundRecords[keyNew] = change.NewRec;
                    ФлагОбновитьСписокЖелезнодорожныхСообщенийВТаблице = true;
                }
            }
        }



        public static void СозданиеСтатическихЗвуковыхФайлов()
        {
            int id = 1;
            foreach (SoundConfigurationRecord config in SoundConfiguration.SoundConfigurationRecords)
            {
                var статСообщение = Mapper.MapSoundConfigurationRecord2СтатическоеСообщение(config, ref id);
                if (статСообщение != null && статСообщение.Any())
                {
                    foreach (var стат in статСообщение)
                    {
                        var statRecord = стат;
                        int попыткиВставитьСообщение = 5;
                        while (попыткиВставитьСообщение-- > 0)
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
                    if ((Данные.Value.БитыАктивностиПолей & 0x04) != 0x00) ВремяПрибытия = Данные.Value.ВремяПрибытия.ToString("HH:mm");
                    if ((Данные.Value.БитыАктивностиПолей & 0x10) != 0x00) ВремяОтправления = Данные.Value.ВремяОтправления.ToString("HH:mm");


                    ListViewItem lvi1 = new ListViewItem(new string[] {Данные.Value.Время.ToString("yy.MM.dd  HH:mm:ss"),
                                                                       Данные.Value.НомерПоезда.Replace(':', ' '),
                                                                       Данные.Value.НомерПути.ToString(),
                                                                       Данные.Value.НазваниеПоезда,
                                                                       ВремяПрибытия,
                                                                       ВремяОтправления,
                                                                       Данные.Value.Примечание,
                                                                       Данные.Value.ИспользоватьДополнение["звук"] ? Данные.Value.Дополнение : String.Empty});
                    lvi1.Tag = Данные.Value.ID;
                    lvi1.Checked = Данные.Value.Состояние != SoundRecordStatus.Выключена;
                    this.listView1.Items.Add(lvi1);

                    if ((Данные.Value.БитыАктивностиПолей & 0x14) == 0x04)
                    {
                        ListViewItem lvi2 = new ListViewItem(new string[] {Данные.Value.Время.ToString("yy.MM.dd  HH:mm:ss"),
                                                                       Данные.Value.НомерПоезда.Replace(':', ' '),
                                                                       Данные.Value.НомерПути.ToString(),
                                                                       ВремяПрибытия,
                                                                       Данные.Value.НазваниеПоезда,
                                                                       Данные.Value.ИспользоватьДополнение["звук"] ? Данные.Value.Дополнение : String.Empty});
                        lvi2.Tag = Данные.Value.ID;
                        lvi2.Checked = Данные.Value.Состояние != SoundRecordStatus.Выключена;
                        this.lVПрибытие.Items.Add(lvi2);
                    }

                    if ((Данные.Value.БитыАктивностиПолей & 0x14) == 0x14)
                    {
                        ListViewItem lvi3 = new ListViewItem(new string[] {Данные.Value.Время.ToString("yy.MM.dd  HH:mm:ss"),
                                                                       Данные.Value.НомерПоезда.Replace(':', ' '),
                                                                       Данные.Value.НомерПути.ToString(),
                                                                       ВремяПрибытия,
                                                                       ВремяОтправления,
                                                                       Данные.Value.НазваниеПоезда,
                                                                       Данные.Value.ИспользоватьДополнение["звук"] ? Данные.Value.Дополнение : String.Empty});
                        lvi3.Tag = Данные.Value.ID;
                        lvi3.Checked = Данные.Value.Состояние != SoundRecordStatus.Выключена;
                        this.lVТранзит.Items.Add(lvi3);
                    }

                    if ((Данные.Value.БитыАктивностиПолей & 0x14) == 0x10)
                    {
                        ListViewItem lvi4 = new ListViewItem(new string[] {Данные.Value.Время.ToString("yy.MM.dd  HH:mm:ss"),
                                                                       Данные.Value.НомерПоезда.Replace(':', ' '),
                                                                       Данные.Value.НомерПути.ToString(),
                                                                       ВремяОтправления,
                                                                       Данные.Value.НазваниеПоезда,
                                                                       Данные.Value.Дополнение});
                        lvi4.Tag = Данные.Value.ID;
                        lvi4.Checked = Данные.Value.Состояние != SoundRecordStatus.Выключена;
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

                            case SoundRecordStatus.ВоспроизведениеАвтомат:
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

                                case 7: // Ручной режим за 30 мин до самого ранего события или если не выставленн ПУТЬ
                                    if (lv.Items[item].ForeColor != Program.Настройки.НастройкиЦветов[14])
                                        lv.Items[item].ForeColor = Program.Настройки.НастройкиЦветов[14];
                                    if (lv.Items[item].BackColor != Program.Настройки.НастройкиЦветов[15])
                                        lv.Items[item].BackColor = Program.Настройки.НастройкиЦветов[15];
                                    break;

                                case 8: // Ручной режим
                                    if (lv.Items[item].ForeColor != Color.White)
                                        lv.Items[item].ForeColor = Color.White;
                                    if (lv.Items[item].BackColor != Program.Настройки.НастройкиЦветов[15])
                                        lv.Items[item].BackColor = Program.Настройки.НастройкиЦветов[15];
                                    break;
                            }

                            //Обновить номер пути (текущий номер / предыдущий, до автосброса)
                            var номерПути = (Данные.НомерПути != Данные.НомерПутиБезАвтосброса) ?
                                             $"{Данные.НомерПути} ({Данные.НомерПутиБезАвтосброса})" :
                                             Данные.НомерПути;
                            if (lv.Items[item].SubItems[2].Text != номерПути)
                            {
                                lv.Items[item].SubItems[2].Text = номерПути;
                            }

                            if (lv.Name == "listView1")
                            {
                                string нумерацияПоезда = String.Empty;
                                switch (Данные.НумерацияПоезда)
                                {
                                    case 1:
                                        нумерацияПоезда = "Нумерация поезда с ГОЛОВЫ состава";
                                        break;

                                    case 2:
                                        нумерацияПоезда = "Нумерация поезда с ХВОСТА состава";
                                        break;
                                }


                                if (lv.Items[item].SubItems[6].Text != Данные.Примечание + нумерацияПоезда)
                                    lv.Items[item].SubItems[6].Text = Данные.Примечание + нумерацияПоезда;
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
            bool СообщениеИзменено;

            TaskManager.Clear();



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
                else if (DateTime.Now > Сообщение.Время.AddSeconds(1))
                {
                    if (QueueSound.FindItem(Сообщение.ID, null) == null)            //Если нету элемента в очереди сообщений, то запись уже воспроизведенна.
                    {
                        if (Сообщение.СостояниеВоспроизведения != SoundRecordStatus.Воспроизведена)
                        {
                            Сообщение.СостояниеВоспроизведения = SoundRecordStatus.Воспроизведена;
                            СообщениеИзменено = true;
                        }
                    }
                }
                else if (Сообщение.СостояниеВоспроизведения == SoundRecordStatus.ОжиданиеВоспроизведения)
                {
                    СообщениеИзменено = true;
                    Сообщение.СостояниеВоспроизведения = SoundRecordStatus.ДобавленВОчередьАвтомат;
                    if (Сообщение.Активность == true)
                        foreach (var Sound in StaticSoundForm.StaticSoundRecords)
                        {
                            if (Sound.Name == Сообщение.НазваниеКомпозиции)
                            {
                                if (РазрешениеРаботы == true)
                                {
                                    Program.ЗаписьЛога("Автоматическое воспроизведение статического звукового сообщения", Сообщение.НазваниеКомпозиции);
                                    var воспроизводимоеСообщение = new ВоспроизводимоеСообщение
                                    {
                                        ParentId = null,
                                        RootId = Сообщение.ID,
                                        ТипСообщения = ТипСообщения.Статическое,
                                        ИмяВоспроизводимогоФайла = Sound.Name,
                                        Приоритет = Priority.Low,
                                        Язык = NotificationLanguage.Ru,
                                        ОчередьШаблона = null
                                    };
                                    QueueSound.AddItem(воспроизводимоеСообщение);
                                }
                                break;
                            }
                        }
                }

                if (СообщениеИзменено == true)
                    СтатическиеЗвуковыеСообщения[Key] = Сообщение;


                //Добавление события ===================================================================
                if (DateTime.Now > Сообщение.Время.AddMinutes(-30) &&
                    !(Сообщение.СостояниеВоспроизведения == SoundRecordStatus.Воспроизведена && DateTime.Now > Сообщение.Время.AddSeconds(ВремяЗадержкиВоспроизведенныхСобытий))) //убрать через 5 мин. после воспроизведения
                {
                    byte состояниеСтроки = 0;
                    switch (Сообщение.СостояниеВоспроизведения)
                    {
                        case SoundRecordStatus.Воспроизведена:
                        case SoundRecordStatus.Выключена:
                            состояниеСтроки = 0;
                            break;

                        case SoundRecordStatus.ДобавленВОчередьАвтомат:
                        case SoundRecordStatus.ОжиданиеВоспроизведения:
                            состояниеСтроки = 2;
                            break;

                        case SoundRecordStatus.ВоспроизведениеАвтомат:
                            состояниеСтроки = 4;
                            break;
                    }


                    var statSound = StaticSoundForm.StaticSoundRecords.FirstOrDefault(sound => sound.Name == Сообщение.НазваниеКомпозиции);
                    TaskSound taskSound = new TaskSound
                    {
                        НомерСписка = 1,
                        СостояниеСтроки = состояниеСтроки,
                        Описание = Сообщение.НазваниеКомпозиции,
                        Время = Сообщение.Время,
                        Ключ = Key,
                        ParentId = null,
                        ШаблонИлиСообщение = statSound.Message
                    };

                    if (Сообщение.Активность == false)
                        taskSound.СостояниеСтроки = 0;

                    TaskManager.AddItem(taskSound);
                }
            }
            #endregion



            #region Определить композицию для запуска сообщений о движении поездов
            DateTime ТекущееВремя = DateTime.Now;
            bool внесеныИзменения = false;
            for (int i = 0; i < SoundRecords.Count; i++)
            {
                var Данные = SoundRecords.ElementAt(i).Value;
                var key = SoundRecords.ElementAt(i).Key;
                внесеныИзменения = false;

                while (true)
                {
                    if (Данные.Активность == true)
                    {
                        if ((Данные.БитыНештатныхСитуаций & 0x0F) == 0x00)
                            Данные.СписокНештатныхСообщений.Clear();

                        // Проверка на нештатные ситуации
                        if ((Данные.БитыНештатныхСитуаций & 0x0F) != 0x00)
                        {
                            if (Данные.СостояниеКарточки != 6)
                            {
                                Данные.СостояниеКарточки = 6;
                                if ((Данные.БитыНештатныхСитуаций & 0x01) != 0x00)
                                    Данные.ОписаниеСостоянияКарточки = "Поезд отменен";
                                if ((Данные.БитыНештатныхСитуаций & 0x02) != 0x00)
                                    Данные.ОписаниеСостоянияКарточки = "Задержка прибытия поезда";
                                if ((Данные.БитыНештатныхСитуаций & 0x04) != 0x00)
                                    Данные.ОписаниеСостоянияКарточки = "Задержка отправления поезда";
                                if ((Данные.БитыНештатныхСитуаций & 0x08) != 0x00)
                                    Данные.ОписаниеСостоянияКарточки = "Отправление по готовности поезда";
                                внесеныИзменения = true;
                            }

                            if (Данные.Автомат)
                            {
                                //НЕШТАТНОЕ СОБЫТИЕ========================================================================
                                for (int j = 0; j < Данные.СписокНештатныхСообщений.Count; j++)
                                {
                                    var нештатноеСообщение = Данные.СписокНештатныхСообщений[j];
                                    if (нештатноеСообщение.Активность == true)
                                    {
                                        DateTime времяСобытия = нештатноеСообщение.ПривязкаКВремени == 0 ? Данные.ВремяПрибытия : Данные.ВремяОтправления;
                                        времяСобытия = времяСобытия.AddMinutes(нештатноеСообщение.ВремяСмещения);

                                        if (DateTime.Now < времяСобытия)
                                        {
                                            if (нештатноеСообщение.СостояниеВоспроизведения != SoundRecordStatus.ОжиданиеВоспроизведения)
                                            {
                                                нештатноеСообщение.СостояниеВоспроизведения = SoundRecordStatus.ОжиданиеВоспроизведения;
                                                Данные.СписокНештатныхСообщений[j] = нештатноеСообщение;
                                                внесеныИзменения = true;
                                            }
                                        }
                                        else if (DateTime.Now >= времяСобытия.AddSeconds(1))
                                        {
                                            if (QueueSound.FindItem(Данные.ID, нештатноеСообщение.Id) == null) //Если нету элемента в очереди сообщений, то запись уже воспроизведенна.
                                            {
                                                if (нештатноеСообщение.СостояниеВоспроизведения != SoundRecordStatus.Воспроизведена)
                                                {
                                                    нештатноеСообщение.СостояниеВоспроизведения = SoundRecordStatus.Воспроизведена;
                                                    Данные.СписокНештатныхСообщений[j] = нештатноеСообщение;
                                                    внесеныИзменения = true;
                                                }
                                            }
                                        }
                                        else if (нештатноеСообщение.СостояниеВоспроизведения == SoundRecordStatus.ОжиданиеВоспроизведения)
                                        {
                                            // СРАБОТКА------------------------------------------------------------
                                            if ((ТекущееВремя.Hour == времяСобытия.Hour) && (ТекущееВремя.Minute == времяСобытия.Minute) && (ТекущееВремя.Second == времяСобытия.Second))
                                            {
                                                нештатноеСообщение.СостояниеВоспроизведения = SoundRecordStatus.ДобавленВОчередьАвтомат;
                                                Данные.СписокНештатныхСообщений[j] = нештатноеСообщение;
                                                внесеныИзменения = true;

                                                if (РазрешениеРаботы && (нештатноеСообщение.Шаблон != ""))
                                                {
                                                    СостояниеФормируемогоСообщенияИШаблон шаблонФормируемогоСообщения = new СостояниеФормируемогоСообщенияИШаблон
                                                    {
                                                        Id = нештатноеСообщение.Id,
                                                        SoundRecordId = Данные.ID,
                                                        Приоритет = Priority.Midlle,
                                                        Шаблон = нештатноеСообщение.Шаблон,
                                                        ЯзыкиОповещения =
                                                            new List<NotificationLanguage>
                                                            {
                                                                NotificationLanguage.Ru,
                                                                NotificationLanguage.Eng
                                                            },
                                                        //TODO: вычислять языки оповещения 
                                                        НазваниеШаблона = нештатноеСообщение.НазваниеШаблона,
                                                    };
                                                    MainWindowForm.ВоспроизвестиШаблонОповещения("Автоматическое воспроизведение сообщения о внештатной ситуации", Данные, шаблонФормируемогоСообщения, ТипСообщения.ДинамическоеАварийное);
                                                }
                                            }
                                        }

                                        if (DateTime.Now > времяСобытия.AddMinutes(-30) && !(нештатноеСообщение.СостояниеВоспроизведения == SoundRecordStatus.Воспроизведена && DateTime.Now > времяСобытия.AddSeconds(ВремяЗадержкиВоспроизведенныхСобытий)))//убрать через 5 мин. после воспроизведения
                                        {
                                            byte состояниеСтроки = 0;
                                            switch (нештатноеСообщение.СостояниеВоспроизведения)
                                            {
                                                case SoundRecordStatus.Воспроизведена:
                                                case SoundRecordStatus.Выключена:
                                                    состояниеСтроки = 0;
                                                    break;

                                                case SoundRecordStatus.ДобавленВОчередьАвтомат:
                                                case SoundRecordStatus.ОжиданиеВоспроизведения:
                                                    состояниеСтроки = 3;
                                                    break;

                                                case SoundRecordStatus.ВоспроизведениеАвтомат:
                                                    состояниеСтроки = 4;
                                                    break;
                                            }

                                            TaskSound taskSound = new TaskSound
                                            {
                                                НомерСписка = 0,
                                                СостояниеСтроки = состояниеСтроки,
                                                Описание = Данные.НомерПоезда + " " + Данные.НазваниеПоезда + ": " + Данные.ОписаниеСостоянияКарточки,
                                                Время = времяСобытия,
                                                Ключ = SoundRecords.ElementAt(i).Key,
                                                ParentId = нештатноеСообщение.Id,
                                                ШаблонИлиСообщение = нештатноеСообщение.Шаблон
                                            };

                                            TaskManager.AddItem(taskSound);
                                        }
                                    }
                                }
                            }
                            break;
                        }



                        // Проверка на наличие шаблонов оповещения
                        if (Данные.СписокФормируемыхСообщений.Count == 0)
                        {
                            if (Данные.СостояниеКарточки != 1)
                            {
                                Данные.СостояниеКарточки = 1;
                                Данные.ОписаниеСостоянияКарточки = "Нет шаблонов оповещения";
                                внесеныИзменения = true;
                            }

                            break;
                        }


                        ОбработкаРучногоВоспроизведенияШаблона(ref Данные, key);


                        // Проверка на приближения времени оповещения (за 30 минут)
                        DateTime СамоеРаннееВремя = DateTime.Now, СамоеПозднееВремя = DateTime.Now;
                        for (int j = 0; j < Данные.СписокФормируемыхСообщений.Count; j++)
                        {
                            var формируемоеСообщение = Данные.СписокФормируемыхСообщений[j];
                            if (!Данные.Автомат)
                            {
                                if (формируемоеСообщение.НазваниеШаблона.StartsWith("@") &&
                                   (Данные.ФиксированноеВремяПрибытия == null))
                                {
                                    continue;
                                }
                            }

                            var ручноШаблон = формируемоеСообщение.НазваниеШаблона.StartsWith("@");
                            var времяПриб = (Данные.ФиксированноеВремяПрибытия == null || !ручноШаблон) ? Данные.ВремяПрибытия : Данные.ФиксированноеВремяПрибытия.Value;
                            var времяОтпр = (Данные.ФиксированноеВремяПрибытия == null || !ручноШаблон) ? Данные.ВремяОтправления : Данные.ФиксированноеВремяОтправления.Value;
                            DateTime времяСобытия = формируемоеСообщение.ПривязкаКВремени == 0 ? времяПриб : времяОтпр;
                            времяСобытия = времяСобытия.AddMinutes(формируемоеСообщение.ВремяСмещения);
                            if (j == 0)
                            {
                                СамоеРаннееВремя = СамоеПозднееВремя = времяСобытия;
                            }
                            else
                            {
                                if (времяСобытия < СамоеРаннееВремя)
                                    СамоеРаннееВремя = времяСобытия;

                                if (времяСобытия > СамоеПозднееВремя)
                                    СамоеПозднееВремя = времяСобытия;
                            }
                        }


                        if (DateTime.Now < СамоеРаннееВремя.AddMinutes(Program.Настройки.ОповещениеСамогоРаннегоВремениШаблона))
                        {
                            if (!Данные.Автомат)
                            {
                                if (Данные.СостояниеКарточки != 7)
                                {
                                    Данные.СостояниеКарточки = 7;
                                    Данные.ОписаниеСостоянияКарточки = "Рано в ручном";
                                    внесеныИзменения = true;
                                }
                            }
                            else
                            if (Данные.СостояниеКарточки != 2)
                            {
                                Данные.СостояниеКарточки = 2;
                                Данные.ОписаниеСостоянияКарточки = "Рано";
                                внесеныИзменения = true;
                            }

                            break;
                        }

                        if (DateTime.Now > СамоеПозднееВремя.AddMinutes(3))
                        {
                            if (Данные.СостояниеКарточки != 0)
                            {
                                Данные.СостояниеКарточки = 0;
                                Данные.ОписаниеСостоянияКарточки = "Поздно";
                                внесеныИзменения = true;
                            }

                            break;
                        }


                        // Проверка на установку пути
                        if (Данные.НомерПути == "")
                        {
                            if (!Данные.Автомат) //в РУЧНОМ режиме отсутсвие пути не отображаем
                            {
                                if (Данные.СостояниеКарточки != 7)
                                {
                                    Данные.СостояниеКарточки = 7;
                                    Данные.ОписаниеСостоянияКарточки = "";
                                    внесеныИзменения = true;
                                }
                            }
                            else
                            if (Данные.СостояниеКарточки != 3)
                            {
                                Данные.СостояниеКарточки = 3;
                                Данные.ОписаниеСостоянияКарточки = "Нет пути";
                                внесеныИзменения = true;
                            }
                            break;
                        }


                        // ОБЛАСТЬ СРАБОТКИ ШАБЛОНОВ
                        int КоличествоВключенныхГалочек = 0;
                        for (int j = 0; j < Данные.СписокФормируемыхСообщений.Count; j++)
                        {
                            var формируемоеСообщение = Данные.СписокФормируемыхСообщений[j];
                            if (!Данные.Автомат)
                            {
                                if (формируемоеСообщение.НазваниеШаблона.StartsWith("@") &&
                                   (Данные.ФиксированноеВремяПрибытия == null))
                                {
                                    continue;
                                }
                            }

                            var ручноШаблон = формируемоеСообщение.НазваниеШаблона.StartsWith("@");
                            var времяПриб = (Данные.ФиксированноеВремяПрибытия == null || !ручноШаблон) ? Данные.ВремяПрибытия : Данные.ФиксированноеВремяПрибытия.Value;
                            var времяОтпр = (Данные.ФиксированноеВремяПрибытия == null || !ручноШаблон) ? Данные.ВремяОтправления : Данные.ФиксированноеВремяОтправления.Value;
                            DateTime времяСобытия = формируемоеСообщение.ПривязкаКВремени == 0 ? времяПриб : времяОтпр;
                            времяСобытия = времяСобытия.AddMinutes(формируемоеСообщение.ВремяСмещения);

                            if (формируемоеСообщение.Активность == true)
                            {
                                КоличествоВключенныхГалочек++;
                                if (формируемоеСообщение.Воспроизведен == false)
                                {
                                    if (DateTime.Now < времяСобытия)
                                    {
                                        if (формируемоеСообщение.СостояниеВоспроизведения != SoundRecordStatus.ОжиданиеВоспроизведения)
                                        {
                                            формируемоеСообщение.СостояниеВоспроизведения = SoundRecordStatus.ОжиданиеВоспроизведения;
                                            Данные.СписокФормируемыхСообщений[j] = формируемоеСообщение;
                                            внесеныИзменения = true;
                                        }
                                    }
                                    else if (DateTime.Now >= времяСобытия.AddSeconds(1))
                                    {
                                        if (QueueSound.FindItem(Данные.ID, формируемоеСообщение.Id) == null) //Если нету элемента в очереди сообщений, то запись уже воспроизведенна.
                                        {
                                            if (формируемоеСообщение.СостояниеВоспроизведения != SoundRecordStatus.Воспроизведена)
                                            {
                                                формируемоеСообщение.СостояниеВоспроизведения = SoundRecordStatus.Воспроизведена;
                                                Данные.СписокФормируемыхСообщений[j] = формируемоеСообщение;
                                                внесеныИзменения = true;
                                            }
                                        }
                                    }
                                    else if (формируемоеСообщение.СостояниеВоспроизведения == SoundRecordStatus.ОжиданиеВоспроизведения)
                                    {
                                        //СРАБОТКА-------------------------------
                                        if ((ТекущееВремя.Hour == времяСобытия.Hour) && (ТекущееВремя.Minute == времяСобытия.Minute) && (ТекущееВремя.Second >= времяСобытия.Second))
                                        {
                                            формируемоеСообщение.СостояниеВоспроизведения = SoundRecordStatus.ДобавленВОчередьАвтомат;
                                            Данные.СписокФормируемыхСообщений[j] = формируемоеСообщение;
                                            внесеныИзменения = true;

                                            if (РазрешениеРаботы == true)
                                                MainWindowForm.ВоспроизвестиШаблонОповещения("Автоматическое воспроизведение расписания", Данные, формируемоеСообщение, ТипСообщения.Динамическое);
                                        }
                                    }


                                    //Динамическое сообщение попадет в список если ФормируемоеСообщение еще не воспроезведенно  и не прошло 1мин с момента попадания в список.
                                    //==================================================================================
                                    if (DateTime.Now > времяСобытия.AddMinutes(-30) && !(формируемоеСообщение.СостояниеВоспроизведения == SoundRecordStatus.Воспроизведена && DateTime.Now > времяСобытия.AddSeconds(ВремяЗадержкиВоспроизведенныхСобытий)))
                                    {
                                        byte состояниеСтроки = 0;
                                        switch (формируемоеСообщение.СостояниеВоспроизведения)
                                        {
                                            case SoundRecordStatus.Воспроизведена:
                                            case SoundRecordStatus.Выключена:
                                                состояниеСтроки = 0;
                                                break;

                                            case SoundRecordStatus.ДобавленВОчередьАвтомат:
                                            case SoundRecordStatus.ОжиданиеВоспроизведения:
                                                состояниеСтроки = 1;
                                                break;

                                            case SoundRecordStatus.ВоспроизведениеАвтомат:
                                                состояниеСтроки = 4;
                                                break;
                                        }

                                        TaskSound taskSound = new TaskSound
                                        {
                                            НомерСписка = 0,
                                            СостояниеСтроки = состояниеСтроки,
                                            Описание = Данные.НомерПоезда + " " + Данные.НазваниеПоезда + ": " + формируемоеСообщение.НазваниеШаблона,
                                            Время = времяСобытия,
                                            Ключ = SoundRecords.ElementAt(i).Key,
                                            ParentId = формируемоеСообщение.Id,
                                            ШаблонИлиСообщение = формируемоеСообщение.Шаблон
                                        };

                                        TaskManager.AddItem(taskSound);
                                    }
                                }
                            }
                        }


                        var количествоЭлементов = Данные.Автомат
                            ? Данные.СписокФормируемыхСообщений.Count
                            : Данные.СписокФормируемыхСообщений.Count(s => !s.НазваниеШаблона.StartsWith("@"));

                        if (КоличествоВключенныхГалочек < количествоЭлементов)
                        {
                            if (Данные.СостояниеКарточки != 4)
                            {
                                Данные.СостояниеКарточки = 4;
                                Данные.ОписаниеСостоянияКарточки = "Не все шаблоны разрешены";
                                внесеныИзменения = true;
                            }
                        }
                        else
                        {
                            if (Данные.СостояниеКарточки != 5)
                            {
                                Данные.СостояниеКарточки = 5;
                                Данные.ОписаниеСостоянияКарточки = "Все шаблоны разрешены";
                                внесеныИзменения = true;
                            }
                        }

                        if (!Данные.Автомат)
                        {
                            if (Данные.СостояниеКарточки != 8)
                            {
                                Данные.СостояниеКарточки = 8;
                                Данные.ОписаниеСостоянияКарточки = "Ручной режим с выставленным путем";
                                внесеныИзменения = true;
                            }
                        }
                    }
                    else
                    {
                        if (Данные.СостояниеКарточки != 0)
                        {
                            Данные.СостояниеКарточки = 0;
                            Данные.ОписаниеСостоянияКарточки = "Отключена";
                            внесеныИзменения = true;
                        }
                    }

                    break;
                }


                if (внесеныИзменения == true)
                {
                    string Key = SoundRecords.ElementAt(i).Key;
                    SoundRecords.Remove(Key);
                    SoundRecords.Add(Key, Данные);
                }
            }
            #endregion



            #region Определить композицию для запуска технического сообщения

            for (int i = 0; i < TechnicalMessageForm.SoundRecords.Count; i++)
            {
                var record = TechnicalMessageForm.SoundRecords[i];
                if (record.СписокФормируемыхСообщений.Any())
                {
                    var формируемоеСообщение = record.СписокФормируемыхСообщений[0];
                    if (формируемоеСообщение.СостояниеВоспроизведения == SoundRecordStatus.ДобавленВОчередьРучное ||
                        формируемоеСообщение.СостояниеВоспроизведения == SoundRecordStatus.ВоспроизведениеРучное)
                    {
                        byte состояниеСтроки = 0;
                        switch (формируемоеСообщение.СостояниеВоспроизведения)
                        {
                            case SoundRecordStatus.ДобавленВОчередьРучное:
                                состояниеСтроки = 1;
                                break;

                            case SoundRecordStatus.ВоспроизведениеРучное:
                                состояниеСтроки = 4;
                                break;
                        }

                        TaskSound taskSound = new TaskSound
                        {
                            НомерСписка = 0,
                            СостояниеСтроки = состояниеСтроки,
                            Описание = формируемоеСообщение.НазваниеШаблона,
                            Время = record.Время,
                            Ключ = SoundRecords.ElementAt(i).Key,
                            ParentId = формируемоеСообщение.Id,
                            ШаблонИлиСообщение = формируемоеСообщение.Шаблон
                        };

                        TaskManager.AddItem(taskSound);
                    }
                    else
                    {
                        TechnicalMessageForm.SoundRecords.RemoveAt(i);
                    }
                }
            }

            #endregion



            lVСобытия_ОбновитьСостояниеТаблицы();

            ОтобразитьСубтитры();
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
                    var binding2OperativeShedule = Binding2GeneralScheduleBehaviors.Where(b => b.SourceLoad == SourceLoad.SheduleOperative).ToList();


                    //Отправить расписание из окна РАСПИСАНИЕ
                    if (binding2Shedule.Any())
                    {
                        if (TrainTable.TrainTableRecords != null && TrainTable.TrainTableRecords.Any())
                        {
                            var table = TrainTable.TrainTableRecords.Select(Mapper.MapTrainTableRecord2UniversalInputType).ToList();
                            table.ForEach(t => t.Message = $"ПОЕЗД:{t.NumberOfTrain}, ПУТЬ:{t.PathNumber}, СОБЫТИЕ:{t.Event}, СТАНЦИИ:{t.Stations}, ВРЕМЯ:{t.Time.ToShortTimeString()}");

                            var inData = new UniversalInputType { TableData = table };
                            foreach (var beh in binding2Shedule)
                            {
                                beh.InitializePagingBuffer(inData, beh.CheckContrains, beh.GetCountDataTake());
                            }
                        }
                    }


                    //Отправить расписание из окна ОПЕРАТИВНОГО РАСПИСАНИЕ
                    if (binding2OperativeShedule.Any())
                    {
                        if (TrainTableOperative.TrainTableRecords != null && TrainTableOperative.TrainTableRecords.Any())
                        {
                            var table = TrainTableOperative.TrainTableRecords.Select(Mapper.MapTrainTableRecord2UniversalInputType).ToList();
                            table.ForEach(t => t.Message = $"ПОЕЗД:{t.NumberOfTrain}, ПУТЬ:{t.PathNumber}, СОБЫТИЕ:{t.Event}, СТАНЦИИ:{t.Stations}, ВРЕМЯ:{t.Time.ToShortTimeString()}");

                            var inData = new UniversalInputType { TableData = table };
                            foreach (var beh in binding2OperativeShedule)
                            {
                                beh.InitializePagingBuffer(inData, beh.CheckContrains, beh.GetCountDataTake());
                            }
                        }
                    }


                    //Отправить расписание из ГЛАВНОГО окна  
                    if (binding2MainWindow.Any())
                    {
                        if (SoundRecords != null && SoundRecords.Any())
                        {
                            foreach (var beh in binding2MainWindow)
                            {
                                var table = SoundRecords.Select(t => Mapper.MapSoundRecord2UniveralInputType(t.Value, beh.GetDeviceSetting.PathPermission, false)).ToList();
                                table.ForEach(t => t.Message = $"ПОЕЗД:{t.NumberOfTrain}, ПУТЬ:{t.PathNumber}, СОБЫТИЕ:{t.Event}, СТАНЦИИ:{t.Stations}, ВРЕМЯ:{t.Time.ToShortTimeString()}");
                                var inData = new UniversalInputType { TableData = table };
                                beh.InitializePagingBuffer(inData, beh.CheckContrains, beh.GetCountDataTake());
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

                    if (!данные.Автомат)
                        continue;

                    var _checked = данные.Состояние != SoundRecordStatus.Выключена;
                    if (_checked && (данные.ТипСообщения == SoundRecordType.ДвижениеПоезда))
                    {
                        //ВЫВОД НА ПУТЕВЫЕ ТАБЛО
                        var номераПутей = Program.PathWaysRepository.List().ToList();
                        var index = номераПутей.Select(p => p.Name).ToList().IndexOf(данные.НомерПути) + 1;
                        var indexOld = номераПутей.Select(p => p.Name).ToList().IndexOf(данныеOld.НомерПути) + 1;
                        var номерПути = (index > 0) ? index : 0;
                        var номерПутиOld = (indexOld > 0) ? indexOld : 0;

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
                                        if ((данные.БитыНештатныхСитуаций & 0x0F) == 0x00)
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
                                    if (((данные.БитыНештатныхСитуаций & 0x0F) == 0x00)
                                        && ((данныеOld.БитыНештатныхСитуаций & 0x0F) != 0x00)
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
                                        if ((данные.БитыНештатныхСитуаций & 0x0F) == 0x00)
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
                                    if (((данные.БитыНештатныхСитуаций & 0x0F) == 0x00)
                                        && ((данныеOld.БитыНештатныхСитуаций & 0x0F) != 0x00)
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
        private void ОбработкаЗвуковогоПотка()
        {
            int СекундаТекущегоВремени = DateTime.Now.Second;
            if (СекундаТекущегоВремени != ТекущаяСекунда)
            {
                ТекущаяСекунда = СекундаТекущегоВремени;
                ОпределитьКомпозициюДляЗапуска();
                CheckAutoApdate();
            }

            ОбновитьСостояниеЗаписейТаблицы();
            QueueSound.Invoke();

            SoundFileStatus status = Player.GetFileStatus();
            switch (status)
            {
                case SoundFileStatus.Stop:
                case SoundFileStatus.Paused:
                    MainForm.Пауза.Text = "Старт";
                    MainForm.Пауза.Enabled = true;
                    MainForm.Пауза.BackColor = Color.White;
                    MainForm.Остановить.Enabled = false;
                    MainForm.Остановить.BackColor = Color.White;
                    break;

                case SoundFileStatus.Error:
                    MainForm.Пауза.Text = "...";
                    MainForm.Пауза.Enabled = false;
                    MainForm.Пауза.BackColor = Color.White;
                    MainForm.Остановить.Enabled = false;
                    MainForm.Остановить.BackColor = Color.White;
                    break;

                case SoundFileStatus.Playing:
                    MainForm.Пауза.Text = "Пауза";
                    MainForm.Пауза.Enabled = true;
                    MainForm.Пауза.BackColor = Color.DarkOrange;
                    MainForm.Остановить.Enabled = true;
                    MainForm.Остановить.BackColor = Color.Brown;
                    break;
            }
        }

        private void CheckAutoApdate()
        {
            if (!Program.Настройки.РазрешениеАвтообновленияРасписания)
                return;

            var hourAutoApdate = Program.Настройки.ВремяАвтообновленияРасписания.Hour;
            var minuteAutoApdate = Program.Настройки.ВремяАвтообновленияРасписания.Minute;
            var secondAutoApdate = Program.Настройки.ВремяАвтообновленияРасписания.Second;

            if ((DateTime.Now.Hour == hourAutoApdate) && (DateTime.Now.Minute == minuteAutoApdate) && (DateTime.Now.Second == secondAutoApdate))
            {
                btnОбновитьСписок_Click(null, null);
            }
        }



        private void СобытиеНачалоПроигрыванияОчередиЗвуковыхСообщений()
        {
            //Debug.WriteLine("НАЧАЛО ПРОИГРЫВАНИЯ");//DEBUG
            Log.log.Fatal("НАЧАЛО ПРОИГРЫВАНИЯ ОЧЕРЕДИ");

            if (SoundChanelManagment != null)
            {
                var soundChUit = new UniversalInputType { SoundChanels = Program.Настройки.КаналыДальнегоСлед.ToList(), ViewBag = new Dictionary<string, dynamic>() };
                soundChUit.ViewBag["SoundChanelManagmentEventPlaying"] = "StartPlaying";

                SoundChanelManagment.AddOneTimeSendData(soundChUit); //период отсыла регулируется TimeRespone.
            }
        }


        private void СобытиеКонецПроигрыванияОчередиЗвуковыхСообщений()
        {
            //Debug.WriteLine("КОНЕЦ ПРОИГРЫВАНИЯ");//DEBUG
            Log.log.Fatal("КОНЕЦ ПРОИГРЫВАНИЯ ОЧЕРЕДИ");

            if (SoundChanelManagment != null)
            {
                var soundChUit = new UniversalInputType { SoundChanels = Program.Настройки.КаналыДальнегоСлед.ToList(), ViewBag = new Dictionary<string, dynamic>() };
                soundChUit.ViewBag["SoundChanelManagmentEventPlaying"] = "StopPlaying";

                SoundChanelManagment.AddOneTimeSendData(soundChUit); //период отсыла регулируется TimeRespone.
            }
        }



        // ВоспроизведениеАвтомат выбраной в таблице записи
        private void btnПауза_Click(object sender, EventArgs e)
        {
            SoundFileStatus status = Player.GetFileStatus();
            switch (status)
            {
                case SoundFileStatus.Paused:
                case SoundFileStatus.Stop:
                    QueueSound.PlayPlayer();
                    break;

                case SoundFileStatus.Playing:
                    QueueSound.PausePlayer();
                    break;
            }
        }



        // ВоспроизведениеАвтомат выбраной в таблице записи
        private void btnОстановить_Click(object sender, EventArgs e)
        {
            SoundFileStatus status = Player.GetFileStatus();
            switch (status)
            {
                case SoundFileStatus.Playing:
                    QueueSound.Erase();
                    break;
            }
        }



        // Обработка закрытия основной формы
        private void MainWindowForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (myMainForm == this)
                myMainForm = null;
        }



        //Отправка сообшений на табло
        private void SendOnPathTable(SoundRecord data)
        {
            if (data.СостояниеОтображения == TableRecordStatus.Выключена || data.СостояниеОтображения == TableRecordStatus.ОжиданиеОтображения)
                return;

            if (data.НазванияТабло == null || !data.НазванияТабло.Any())
                return;


            var devicesId = data.НазванияТабло.Select(s => new string(s.TakeWhile(c => c != ':').ToArray())).Select(int.Parse).ToList();
            foreach (var devId in devicesId)
            {
                var beh = Binding2PathBehaviors.FirstOrDefault(b => b.GetDeviceId == devId);
                if (beh != null)
                {
                    var inData = Mapper.MapSoundRecord2UniveralInputType(data, beh.GetDeviceSetting.PathPermission, true);
                    inData.Message = $"ПОЕЗД:{inData.NumberOfTrain}, ПУТЬ:{inData.PathNumber}, СОБЫТИЕ:{inData.Event}, СТАНЦИИ:{inData.Stations}, ВРЕМЯ:{inData.Time.ToShortTimeString()}";

                    beh.SendMessage4Path(inData, data.НомерПоезда, beh.CheckContrains);
                    //Debug.WriteLine($" ТАБЛО= {beh.GetDeviceName}: {beh.GetDeviceId} для ПУТИ {data.НомерПути}.  Сообшение= {inData.Message}  ");
                }
            }
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

                                string Key2 = Данные.Время.ToString("yy.MM.dd  HH:mm:ss");
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
                                        Key2 = Данные.Время.ToString("yy.MM.dd  HH:mm:ss");
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
                        string key = listView.Items[item].SubItems[0].Text;
                        string keyOld = key;

                        if (SoundRecords.Keys.Contains(key) == true)
                        {
                            SoundRecord данные = SoundRecords[key];

                            КарточкаДвиженияПоезда карточка = new КарточкаДвиженияПоезда(данные, key);
                            if (карточка.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                SoundRecord старыеДанные = данные;
                                данные = карточка.ПолучитьИзмененнуюКарточку();
                                данные = ИзменениеДанныхВКарточке(старыеДанные, данные, key);

                                if (DateTime.ParseExact(key, "yy.MM.dd  HH:mm:ss", new DateTimeFormatInfo()) != данные.Время)
                                {
                                    key = данные.Время.ToString("yy.MM.dd  HH:mm:ss");
                                    listView.Items[item].SubItems[0].Text = key;
                                }


                                //Изменение названия поезда
                                switch (listView.Name)
                                {
                                    case "listView1":
                                        if (listView.Items[item].SubItems[3].Text != данные.НазваниеПоезда)
                                            listView.Items[item].SubItems[3].Text = данные.НазваниеПоезда;
                                        break;

                                    case "lVПрибытие":
                                    case "lVОтправление":
                                        if (listView.Items[item].SubItems[4].Text != данные.НазваниеПоезда)
                                            listView.Items[item].SubItems[4].Text = данные.НазваниеПоезда;
                                        break;

                                    case "lVТранзит":
                                        if (listView.Items[item].SubItems[5].Text != данные.НазваниеПоезда)
                                            listView.Items[item].SubItems[5].Text = данные.НазваниеПоезда;
                                        break;
                                }

                                //Изменение номера поезда
                                switch (listView.Name) 
                                {
                                    case "listView1":
                                        if (listView.Items[item].SubItems[1].Text != данные.НомерПоезда)
                                            listView.Items[item].SubItems[1].Text = данные.НомерПоезда;
                                        break;

                                    case "lVПрибытие":
                                    case "lVОтправление":
                                        if (listView.Items[item].SubItems[1].Text != данные.НомерПоезда)
                                            listView.Items[item].SubItems[1].Text = данные.НомерПоезда;
                                        break;

                                    case "lVТранзит":
                                        if (listView.Items[item].SubItems[1].Text != данные.НомерПоезда)
                                            listView.Items[item].SubItems[1].Text = данные.НомерПоезда;
                                        break;
                                }



                                if (данные.БитыНештатныхСитуаций != старыеДанные.БитыНештатныхСитуаций)
                                {
                                    данные = ЗаполнениеСпискаНештатныхСитуаций(данные, key);
                                }



                                //Изменение ДОПОЛНЕНИЯ
                                switch (listView.Name)
                                {
                                    case "listView1":
                                        if (listView.Items[item].SubItems[7].Text != данные.Дополнение)
                                            listView.Items[item].SubItems[7].Text = данные.ИспользоватьДополнение["звук"] ? данные.Дополнение : String.Empty;
                                        break;

                                    case "lVПрибытие":
                                    case "lVОтправление":
                                        if (listView.Items[item].SubItems[5].Text != данные.НазваниеПоезда)
                                            listView.Items[item].SubItems[5].Text = данные.ИспользоватьДополнение["звук"] ? данные.Дополнение : String.Empty;
                                        break;

                                    case "lVТранзит":
                                        if (listView.Items[item].SubItems[6].Text != данные.НазваниеПоезда)
                                            listView.Items[item].SubItems[6].Text = данные.ИспользоватьДополнение["звук"] ? данные.Дополнение : String.Empty;
                                        break;
                                }


                                //Обновить Время ПРИБ
                                var actStr = "";
                                if ((данные.БитыАктивностиПолей & 0x04) != 0x00)
                                {
                                    данные = ЗаполнениеСпискаНештатныхСитуаций(данные, key);

                                    actStr = данные.ВремяПрибытия.ToString("HH:mm");
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
                                if ((данные.БитыАктивностиПолей & 0x10) != 0x00)
                                {
                                    данные = ЗаполнениеСпискаНештатныхСитуаций(данные, key);

                                    actStr = данные.ВремяОтправления.ToString("HH:mm");
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

                                //Смена Режима Работы.
                                if (старыеДанные.Автомат != данные.Автомат)
                                {
                                    MainForm.РежимРаботы.BackColor = Color.LightGray;
                                    MainForm.РежимРаботы.Text = @"Пользовательский";
                                }


                                if (SoundRecords.ContainsKey(keyOld) == false)  // поменяли время приб. или отпр. т.е. изменили ключ записи. Т.е. удалили запись под старым ключем.
                                {
                                    ОбновитьСписокЗвуковыхСообщенийВТаблице(); //Перерисуем список на UI.
                                }

                                if (!StructCompare.SoundRecordComparer(ref данные, ref старыеДанные))
                                {
                                    СохранениеИзмененийДанныхВКарточке(старыеДанные, данные);
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


        private SoundRecord ЗаполнениеСпискаНештатныхСитуаций(SoundRecord данные, string key)
        {
            if ((данные.БитыНештатныхСитуаций & 0x0F) == 0x00)
                return данные;

            DateTime ВременноеВремяСобытия = (данные.БитыАктивностиПолей & 0x04) != 0x00 ? данные.ВремяПрибытия : данные.ВремяОтправления;
            string ФормируемоеСообщение = "";

            //Сформируем список нештатных сообщений--------------------------------------
            var startDate = ВременноеВремяСобытия.AddMinutes(-30);
            var endDate = ВременноеВремяСобытия.AddHours(27 - DateTime.Now.Hour); //часы до конца суток  +3 часа
            List<СостояниеФормируемогоСообщенияИШаблон> текущийСписокНештатныхСообщений = new List<СостояниеФормируемогоСообщенияИШаблон>();


            int ТипПоезда = (int)данные.ТипПоезда;
            int indexШаблона = 1000;              //нештатные сообшения индексируются от 1000
            for (var date = startDate; date < endDate; date += new TimeSpan(0, 0, (int)(Program.Настройки.ИнтервалМеждуОповещениемОбОтменеПоезда * 60.0)))
            {
                СостояниеФормируемогоСообщенияИШаблон новыйШаблон;
                новыйШаблон.Id = indexШаблона++;
                новыйШаблон.SoundRecordId = данные.ID;
                новыйШаблон.Активность = данные.Активность;
                новыйШаблон.Приоритет = Priority.Midlle;
                новыйШаблон.Воспроизведен = false;
                новыйШаблон.СостояниеВоспроизведения = SoundRecordStatus.ОжиданиеВоспроизведения;
                новыйШаблон.ВремяСмещения = (((ВременноеВремяСобытия - date).Hours * 60) + (ВременноеВремяСобытия - date).Minutes) * -1;
                новыйШаблон.НазваниеШаблона = String.Empty;
                новыйШаблон.Шаблон = String.Empty;
                новыйШаблон.ПривязкаКВремени = ((данные.БитыАктивностиПолей & 0x04) != 0x00) ? 0 : 1;
                новыйШаблон.ЯзыкиОповещения = new List<NotificationLanguage> { NotificationLanguage.Ru, NotificationLanguage.Eng };

                if ((данные.БитыНештатныхСитуаций & 0x01) != 0x00)
                {
                    новыйШаблон.НазваниеШаблона = "Авария:Отмена";
                    ФормируемоеСообщение = Program.ШаблонОповещенияОбОтменеПоезда[ТипПоезда];
                }
                else if ((данные.БитыНештатныхСитуаций & 0x02) != 0x00)
                {
                    новыйШаблон.НазваниеШаблона = "Авария:ЗадержкаПрибытия";
                    ФормируемоеСообщение = Program.ШаблонОповещенияОЗадержкеПрибытияПоезда[ТипПоезда];
                }
                else if ((данные.БитыНештатныхСитуаций & 0x04) != 0x00)
                {
                    новыйШаблон.НазваниеШаблона = "Авария:ЗадержкаОтправления";
                    ФормируемоеСообщение = Program.ШаблонОповещенияОЗадержкеОтправленияПоезда[ТипПоезда];
                }
                else if ((данные.БитыНештатныхСитуаций & 0x08) != 0x00)
                {
                    новыйШаблон.НазваниеШаблона = "Авария:ОтправлениеПоГотов.";
                    ФормируемоеСообщение = Program.ШаблонОповещенияООтправлениеПоГотовностиПоезда[ТипПоезда];
                }

                if (ФормируемоеСообщение != "")
                {
                    foreach (var Item in DynamicSoundForm.DynamicSoundRecords)
                        if (Item.Name == ФормируемоеСообщение)
                        {
                            новыйШаблон.Шаблон = Item.Message;
                            break;
                        }
                }

                текущийСписокНештатныхСообщений.Add(новыйШаблон);
            }

            данные.СписокНештатныхСообщений = текущийСписокНештатныхСообщений;
            SoundRecords[key] = данные;

            return данные;
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

                                        var paths = Program.PathWaysRepository.List().Select(p => p.Name).ToList();
                                        for (int i = 0; i < СписокПолейПути.Length - 1; i++)
                                        {
                                            if (i < paths.Count)
                                            {
                                                СписокПолейПути[i + 1].Text = paths[i];
                                                СписокПолейПути[i + 1].Visible = true;
                                            }
                                            else
                                            {
                                                СписокПолейПути[i + 1].Visible = false;
                                            }
                                        }

                                        foreach (ToolStripMenuItem t in СписокПолейПути)
                                            t.Checked = false;

                                        int номерПути = paths.IndexOf(Данные.НомерПути) + 1;
                                        if (номерПути >= 1 && номерПути < СписокПолейПути.Length)
                                            СписокПолейПути[номерПути].Checked = true;
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




        public static void ВоспроизвестиШаблонОповещения(string названиеСообщения, SoundRecord Record, СостояниеФормируемогоСообщенияИШаблон формируемоеСообщение, ТипСообщения типСообщения)
        {
            string Text;
            string logMessage = "";

            string[] ФайлыМинут = new string[] { "00 минут", "01 минута", "02 минуты", "03 минуты", "04 минуты", "05 минут", "06 минут", "07 минут", "08 минут",
                        "09 минут", "10 минут", "11 минут", "12 минут", "13 минут", "14 минут", "15 минут", "16 минут", "17 минут",
                        "18 минут", "19 минут", "20 минут", "21 минута", "22 минуты", "23 минуты", "24 минуты", "25 минут", "26 минут",
                        "27 минут", "28 минут", "29 минут", "30 минут", "31 минута", "32 минуты", "33 минуты", "34 минуты", "35 минут",
                        "36 минут", "37 минут", "38 минут", "39 минут", "40 минут", "41 минута", "42 минуты", "43 минуты", "44 минуты",
                        "45 минут", "46 минут", "47 минут", "48 минут", "49 минут", "50 минут", "51 минута", "52 минуты", "53 минуты",
                        "54 минуты", "55 минут", "56 минут", "57 минут", "58 минут", "59 минут" };


            string[] файлыЧасовПрефиксВ = new string[] { "В 00 часов", "В 01 час", "В 02 часа", "В 03 часа", "В 04 часа", "В 05 часов", "В 06 часов", "В 07 часов",
                                                                                        "В 08 часов", "В 09 часов", "В 10 часов", "В 11 часов", "В 12 часов", "В 13 часов", "В 14 часов", "В 15 часов",
                                                                                        "В 16 часов", "В 17 часов", "В 18 часов", "В 19 часов", "В 20 часов", "В 21 час", "В 22 часа", "В 23 часа" };

            string[] файлыЧасов = new string[] { "00 часов", "01 час", "02 часа", "03 часа", "04 часа", "05 часов", "06 часов", "07 часов",
                                                                                        "08 часов", "09 часов", "10 часов", "11 часов", "12 часов", "13 часов", "14 часов", "15 часов",
                                                                                        "16 часов", "17 часов", "18 часов", "19 часов", "20 часов", "21 час", "22 часа", "23 часа" };

            string[] НазваниеФайловНумерацииПутей = new string[] { "", "Нумерация с головы", "Нумерация с хвоста" };


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

            var воспроизводимыеСообщения = new List<ВоспроизводимоеСообщение>();

            var номераПутей = Program.PathWaysRepository.List().ToList();
            var путь = номераПутей.FirstOrDefault(p => p.Name == Record.НомерПути);

            string[] элементыШаблона = формируемоеСообщение.Шаблон.Split('|');
            foreach (var язык in формируемоеСообщение.ЯзыкиОповещения)
            {
                foreach (string шаблон in элементыШаблона)
                {
                    string текстПодстановки = String.Empty;

                    switch (шаблон)
                    {
                        case "НА НОМЕР ПУТЬ":
                        case "НА НОМЕРом ПУТИ":
                        case "С НОМЕРого ПУТИ":
                            if (путь == null)
                                break;
                            if (шаблон == "НА НОМЕР ПУТЬ") текстПодстановки = путь.НаНомерПуть;
                            if (шаблон == "НА НОМЕРом ПУТИ") текстПодстановки = путь.НаНомерОмПути;
                            if (шаблон == "С НОМЕРого ПУТИ") текстПодстановки = путь.СНомерОгоПути;

                            Text = текстПодстановки;
                            logMessage += Text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = Text,
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                Приоритет = формируемоеСообщение.Приоритет
                            });
                            break;

                        case "ПУТЬ ДОПОЛНЕНИЕ":
                            if (путь?.Addition == null)
                                break;

                            Text = путь.Addition;
                            logMessage += Text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = Text,
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                Приоритет = формируемоеСообщение.Приоритет
                            });
                            break;

                        case "СТ.ОТПРАВЛЕНИЯ":
                            Text = Record.СтанцияОтправления;
                            logMessage += Text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = Text,
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                Приоритет = формируемоеСообщение.Приоритет
                            });
                            break;


                        case "НОМЕР ПОЕЗДА":
                            Text = Record.НомерПоезда;
                            logMessage += Text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = Text,
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                Приоритет = формируемоеСообщение.Приоритет
                            });
                            break;


                        case "НОМЕР ПОЕЗДА ТРАНЗИТ ОТПР":
                            if (!string.IsNullOrEmpty(Record.НомерПоезда2))
                            {
                                Text = Record.НомерПоезда2;
                                logMessage += Text + " ";
                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                {
                                    ИмяВоспроизводимогоФайла = Text,
                                    ТипСообщения = типСообщения,
                                    Язык = язык,
                                    ParentId = формируемоеСообщение.Id,
                                    RootId = формируемоеСообщение.SoundRecordId,
                                    Приоритет = формируемоеСообщение.Приоритет
                                });
                            }
                            break;


                        case "ДОПОЛНЕНИЕ":
                            if (Record.ИспользоватьДополнение["звук"])
                            {
                                Text = Record.Дополнение;
                                logMessage += Text + " ";
                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                {
                                    ИмяВоспроизводимогоФайла = Text,
                                    ТипСообщения = типСообщения,
                                    Язык = язык,
                                    ParentId = формируемоеСообщение.Id,
                                    RootId = формируемоеСообщение.SoundRecordId,
                                    Приоритет = формируемоеСообщение.Приоритет
                                });
                            }
                            break;


                        case "СТ.ПРИБЫТИЯ":
                            Text = Record.СтанцияНазначения;
                            logMessage += Text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = Text,
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                Приоритет = формируемоеСообщение.Приоритет
                            });
                            break;


                        case "ВРЕМЯ ПРИБЫТИЯ":
                            logMessage += "Время прибытия: ";
                            Text = Record.ВремяПрибытия.ToString("HH:mm");
                            logMessage += Text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыЧасовПрефиксВ[Record.ВремяПрибытия.Hour],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                Приоритет = формируемоеСообщение.Приоритет
                            });
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = ФайлыМинут[Record.ВремяПрибытия.Minute],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                Приоритет = формируемоеСообщение.Приоритет
                            });
                            continue;


                        case "ВРЕМЯ СТОЯНКИ":
                            if (Record.ВремяСтоянки.HasValue)
                            {
                                logMessage += "Стоянка: ";
                                Text = Record.ВремяСтоянки.Value.Hours.ToString("D2") + ":" + Record.ВремяСтоянки.Value.Minutes.ToString("D2") + " минут";
                                logMessage += Text + " ";

                                if (Record.ВремяСтоянки.Value.Hours > 0)
                                {
                                    воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                    {
                                        ИмяВоспроизводимогоФайла = файлыЧасов[Record.ВремяСтоянки.Value.Hours],
                                        ТипСообщения = типСообщения,
                                        Язык = язык,
                                        ParentId = формируемоеСообщение.Id,
                                        RootId = формируемоеСообщение.SoundRecordId,
                                        Приоритет = формируемоеСообщение.Приоритет
                                    });
                                }
                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                {
                                    ИмяВоспроизводимогоФайла = ФайлыМинут[Record.ВремяСтоянки.Value.Minutes],
                                    ТипСообщения = типСообщения,
                                    Язык = язык,
                                    ParentId = формируемоеСообщение.Id,
                                    RootId = формируемоеСообщение.SoundRecordId,
                                    Приоритет = формируемоеСообщение.Приоритет
                                });
                            }
                            continue;


                        case "ВРЕМЯ ОТПРАВЛЕНИЯ":
                            logMessage += "Время отправления: ";
                            Text = Record.ВремяОтправления.ToString("HH:mm");
                            logMessage += Text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыЧасовПрефиксВ[Record.ВремяОтправления.Hour],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                Приоритет = формируемоеСообщение.Приоритет
                            });
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = ФайлыМинут[Record.ВремяОтправления.Minute],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                Приоритет = формируемоеСообщение.Приоритет
                            });
                            continue;


                        case "ВРЕМЯ ЗАДЕРЖКИ":
                            if (Record.ВремяЗадержки != null)
                            {
                                logMessage += "Время задержки: ";
                                Text = Record.ВремяЗадержки.Value.ToString("HH:mm");
                                logMessage += Text + " ";

                                if (Record.ВремяЗадержки.Value.Hour > 0)
                                {
                                    воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                    {
                                        ИмяВоспроизводимогоФайла = файлыЧасов[Record.ВремяЗадержки.Value.Hour],
                                        ТипСообщения = типСообщения,
                                        Язык = язык,
                                        ParentId = формируемоеСообщение.Id,
                                        RootId = формируемоеСообщение.SoundRecordId,
                                        Приоритет = формируемоеСообщение.Приоритет
                                    });
                                }
                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                {
                                    ИмяВоспроизводимогоФайла = ФайлыМинут[Record.ВремяЗадержки.Value.Minute],
                                    ТипСообщения = типСообщения,
                                    Язык = язык,
                                    ParentId = формируемоеСообщение.Id,
                                    RootId = формируемоеСообщение.SoundRecordId,
                                    Приоритет = формируемоеСообщение.Приоритет
                                });
                            }
                            continue;


                        case "ОЖИДАЕМОЕ ВРЕМЯ":
                            logMessage += "Ожидаемое время: ";
                            Text = Record.ОжидаемоеВремя.ToString("HH:mm");
                            logMessage += Text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыЧасовПрефиксВ[Record.ОжидаемоеВремя.Hour],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                Приоритет = формируемоеСообщение.Приоритет
                            });
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = ФайлыМинут[Record.ОжидаемоеВремя.Minute],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                Приоритет = формируемоеСообщение.Приоритет
                            });
                            continue;


                        case "ВРЕМЯ СЛЕДОВАНИЯ":
                            if (!Record.ВремяСледования.HasValue)
                                continue;

                            logMessage += "Время следования: ";
                            Text = Record.ВремяСледования.Value.ToString("HH:mm");
                            logMessage += Text + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = файлыЧасовПрефиксВ[Record.ВремяСледования.Value.Hour],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                Приоритет = формируемоеСообщение.Приоритет
                            });
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = ФайлыМинут[Record.ВремяСледования.Value.Minute],
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                Приоритет = формируемоеСообщение.Приоритет
                            });
                            continue;


                        case "НУМЕРАЦИЯ СОСТАВА":
                            if ((Record.НумерацияПоезда > 0) && (Record.НумерацияПоезда <= 2))
                            {
                                Text = НазваниеФайловНумерацииПутей[Record.НумерацияПоезда];
                                logMessage += Text + " ";
                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                {
                                    ИмяВоспроизводимогоФайла = Text,
                                    ТипСообщения = типСообщения,
                                    Язык = язык,
                                    ParentId = формируемоеСообщение.Id,
                                    RootId = формируемоеСообщение.SoundRecordId,
                                    Приоритет = формируемоеСообщение.Приоритет
                                });
                            }
                            break;


                        case "СТАНЦИИ":
                            if ((Record.ТипПоезда == ТипПоезда.Пригородный) || (Record.ТипПоезда == ТипПоезда.Ласточка) ||
                                (Record.ТипПоезда == ТипПоезда.РЭКС))
                            {
                                var списокСтанцийНаправления = Program.ПолучитьСтанцииНаправления(Record.Направление)?.Select(st => st.NameRu).ToList();
                                var списокСтанцийParse = Record.Примечание.Substring(Record.Примечание.IndexOf(":", StringComparison.Ordinal) + 1).Split(',').Select(st => st.Trim()).ToList();

                                if (списокСтанцийНаправления == null || !списокСтанцийНаправления.Any())
                                    break;

                                if (!списокСтанцийParse.Any())
                                    break;

                                if (Record.Примечание.Contains("Со всеми остановками"))
                                {
                                    logMessage += "Электропоезд движется со всеми остановками ";
                                    if (Program.FilesFolder.Contains("СоВсемиОстановками"))
                                    {
                                        воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                        {
                                            ИмяВоспроизводимогоФайла = "СоВсемиОстановками",
                                            ТипСообщения = типСообщения,
                                            Язык = язык,
                                            ParentId = формируемоеСообщение.Id,
                                            RootId = формируемоеСообщение.SoundRecordId,
                                            Приоритет = формируемоеСообщение.Приоритет
                                        });
                                    }
                                }
                                else if (Record.Примечание.Contains("С остановк"))
                                {
                                    logMessage += "Электропоезд движется с остановками на станциях: ";
                                    foreach (var станция in списокСтанцийНаправления)
                                        if (списокСтанцийParse.Contains(станция))
                                            logMessage += станция + " ";

                                    if (Program.FilesFolder.Contains("СОстановками"))
                                    {
                                        воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                        {
                                            ИмяВоспроизводимогоФайла = "СОстановками",
                                            ТипСообщения = типСообщения,
                                            Язык = язык,
                                            ParentId = формируемоеСообщение.Id,
                                            RootId = формируемоеСообщение.SoundRecordId,
                                            Приоритет = формируемоеСообщение.Приоритет
                                        });
                                    }

                                    foreach (var станция in списокСтанцийНаправления)
                                        if (списокСтанцийParse.Contains(станция))
                                            if (Program.FilesFolder.Contains(станция))
                                            {
                                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                                {
                                                    ИмяВоспроизводимогоФайла = станция,
                                                    ТипСообщения = типСообщения,
                                                    Язык = язык,
                                                    ParentId = формируемоеСообщение.Id,
                                                    RootId = формируемоеСообщение.SoundRecordId,
                                                    Приоритет = формируемоеСообщение.Приоритет
                                                });
                                            }
                                }
                                else if (Record.Примечание.Contains("Кроме"))
                                {
                                    logMessage += "Электропоезд движется с остановками кроме станций: ";
                                    foreach (var станция in списокСтанцийНаправления)
                                        if (списокСтанцийParse.Contains(станция))
                                            logMessage += станция + " ";

                                    if (Program.FilesFolder.Contains("СОстановкамиКроме"))
                                    {
                                        воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                        {
                                            ИмяВоспроизводимогоФайла = "СОстановкамиКроме",
                                            ТипСообщения = типСообщения,
                                            Язык = язык,
                                            ParentId = формируемоеСообщение.Id,
                                            RootId = формируемоеСообщение.SoundRecordId,
                                            Приоритет = формируемоеСообщение.Приоритет
                                        });
                                    }

                                    foreach (var станция in списокСтанцийНаправления)
                                        if (списокСтанцийParse.Contains(станция))
                                            if (Program.FilesFolder.Contains(станция))
                                            {
                                                воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                                                {
                                                    ИмяВоспроизводимогоФайла = станция,
                                                    ТипСообщения = типСообщения,
                                                    Язык = язык,
                                                    ParentId = формируемоеСообщение.Id,
                                                    RootId = формируемоеСообщение.SoundRecordId,
                                                    Приоритет = формируемоеСообщение.Приоритет
                                                });
                                            }
                                }
                            }
                            break;


                        default:
                            logMessage += шаблон + " ";
                            воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                            {
                                ИмяВоспроизводимогоФайла = шаблон,
                                ТипСообщения = типСообщения,
                                Язык = язык,
                                ParentId = формируемоеСообщение.Id,
                                RootId = формируемоеСообщение.SoundRecordId,
                                Приоритет = формируемоеСообщение.Приоритет
                            });
                            break;
                    }
                }

                //Пауза между языками
                if ((формируемоеСообщение.ЯзыкиОповещения.Count > 1) && язык == NotificationLanguage.Ru)
                {
                    воспроизводимыеСообщения.Add(new ВоспроизводимоеСообщение
                    {
                        ИмяВоспроизводимогоФайла = "СТОП ",
                        ТипСообщения = типСообщения,
                        Язык = язык,
                        ParentId = формируемоеСообщение.Id,
                        RootId = формируемоеСообщение.SoundRecordId,
                        Приоритет = формируемоеСообщение.Приоритет,
                        ВремяПаузы = (int)(Program.Настройки.ЗадержкаМеждуЗвуковымиСообщениями * 10.0)
                    });
                }
            }

            var сообщениеШаблона = new ВоспроизводимоеСообщение
            {
                ИмяВоспроизводимогоФайла = $"Шаблон: \"{формируемоеСообщение.НазваниеШаблона}\"",
                ТипСообщения = типСообщения,
                ParentId = (int?)((формируемоеСообщение.Id >= 0) ? (ValueType)формируемоеСообщение.Id : null),
                RootId = формируемоеСообщение.SoundRecordId,
                Приоритет = формируемоеСообщение.Приоритет,
                ОчередьШаблона = new Queue<ВоспроизводимоеСообщение>(воспроизводимыеСообщения)
            };

            for (int i = 0; i < Record.КоличествоПовторений; i++)
            {
                QueueSound.AddItem(сообщениеШаблона);
            }

            var логНомерПоезда = string.IsNullOrEmpty(Record.НомерПоезда2) ? Record.НомерПоезда : Record.НомерПоезда + "/" + Record.НомерПоезда2;
            var логНазваниеПоезда = Record.НазваниеПоезда;
            Program.ЗаписьЛога(названиеСообщения, $"Формирование звукового сообщения для поезда \"№{логНомерПоезда}  {логНазваниеПоезда}\": " + logMessage + ". Повтор " + Record.КоличествоПовторений + " раз.");
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
                                    Program.ЗаписьЛога("Действие оператора", "ВоспроизведениеАвтомат статического звукового сообщения: " + Sound.Name);
                                    var воспроизводимоеСообщение = new ВоспроизводимоеСообщение
                                    {
                                        ParentId = null,
                                        RootId = Данные.ID,
                                        ИмяВоспроизводимогоФайла = Sound.Name,
                                        Приоритет = Priority.Low,
                                        Язык = NotificationLanguage.Ru,
                                        ОчередьШаблона = null
                                    };
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
                    SoundRecord данные = SoundRecords[КлючВыбранныйМеню];
                    var paths = Program.PathWaysRepository.List().Select(p => p.Name).ToList();

                    for (int i = 0; i < СписокПолейПути.Length; i++)
                        if (СписокПолейПути[i].Name == tsmi.Name)
                        {
                            string старыйНомерПути = данные.НомерПути;
                            данные.НомерПути = i == 0 ? "" : paths[i - 1];
                            if (старыйНомерПути != данные.НомерПути) Program.ЗаписьЛога("Действие оператора", "Изменение настроек поезда: " + данные.НомерПоезда + " " + данные.НазваниеПоезда + ": " + "Путь: " + старыйНомерПути + " -> " + данные.НомерПути + "; ");

                            данные.ТипСообщения = SoundRecordType.ДвижениеПоезда;
                            данные.НазванияТабло = данные.НомерПути != "0" ? Binding2PathBehaviors.Select(beh => beh.GetDevicesName4Path(данные.НомерПути)).Where(str => str != null).ToArray() : null;

                            данные.НомерПутиБезАвтосброса = данные.НомерПути;
                            SoundRecords[КлючВыбранныйМеню] = данные;

                            var старыеДанные = данные;
                            старыеДанные.НомерПути = старыйНомерПути;
                            if (!StructCompare.SoundRecordComparer(ref данные, ref старыеДанные))
                            {
                                СохранениеИзмененийДанныхВКарточке(старыеДанные, данные);
                            }
                            return;
                        }


                    ToolStripMenuItem[] СписокНумерацииВагонов = new ToolStripMenuItem[] { отсутсвуетToolStripMenuItem, сГоловыСоставаToolStripMenuItem, сХвостаСоставаToolStripMenuItem };
                    string[] СтроковыйСписокНумерацииВагонов = new string[] { "отсутсвуетToolStripMenuItem", "сГоловыСоставаToolStripMenuItem", "сХвостаСоставаToolStripMenuItem" };
                    if (СтроковыйСписокНумерацииВагонов.Contains(tsmi.Name))
                        for (int i = 0; i < СтроковыйСписокНумерацииВагонов.Length; i++)
                            if (СтроковыйСписокНумерацииВагонов[i] == tsmi.Name)
                            {
                                byte СтараяНумерацияПоезда = данные.НумерацияПоезда;
                                данные.НумерацияПоезда = (byte)i;
                                if (СтараяНумерацияПоезда != данные.НумерацияПоезда) Program.ЗаписьЛога("Действие оператора", "Изменение настроек поезда: " + данные.НомерПоезда + " " + данные.НазваниеПоезда + ": " + "Нум.пути: " + СтараяНумерацияПоезда.ToString() + " -> " + данные.НумерацияПоезда.ToString() + "; ");
                                SoundRecords[КлючВыбранныйМеню] = данные;

                                var старыеДанные = данные;
                                старыеДанные.НумерацияПоезда = СтараяНумерацияПоезда;
                                if (!StructCompare.SoundRecordComparer(ref данные, ref старыеДанные))
                                {
                                    СохранениеИзмененийДанныхВКарточке(старыеДанные, данные);
                                }
                                return;
                            }


                    ToolStripMenuItem[] СписокКоличестваПовторов = new ToolStripMenuItem[] { повтор1ToolStripMenuItem, повтор2ToolStripMenuItem, повтор3ToolStripMenuItem };
                    string[] СтроковыйСписокКоличестваПовторов = new string[] { "повтор1ToolStripMenuItem", "повтор2ToolStripMenuItem", "повтор3ToolStripMenuItem" };
                    if (СтроковыйСписокКоличестваПовторов.Contains(tsmi.Name))
                        for (int i = 0; i < СтроковыйСписокКоличестваПовторов.Length; i++)
                            if (СтроковыйСписокКоличестваПовторов[i] == tsmi.Name)
                            {
                                byte СтароеКоличествоПовторений = данные.КоличествоПовторений;
                                данные.КоличествоПовторений = (byte)(i + 1);
                                if (СтароеКоличествоПовторений != данные.КоличествоПовторений) Program.ЗаписьЛога("Действие оператора", "Изменение настроек поезда: " + данные.НомерПоезда + " " + данные.НазваниеПоезда + ": " + "Кол.повт.: " + СтароеКоличествоПовторений.ToString() + " -> " + данные.КоличествоПовторений.ToString() + "; ");
                                SoundRecords[КлючВыбранныйМеню] = данные;

                                var старыеДанные = данные;
                                старыеДанные.КоличествоПовторений = СтароеКоличествоПовторений;
                                if (!StructCompare.SoundRecordComparer(ref данные, ref старыеДанные))
                                {
                                    СохранениеИзмененийДанныхВКарточке(старыеДанные, данные);
                                }
                                return;
                            }


                    if (шаблоныОповещенияToolStripMenuItem1.DropDownItems.Contains(tsmi))
                    {
                        int ИндексШаблона = шаблоныОповещенияToolStripMenuItem1.DropDownItems.IndexOf(tsmi);
                        if (ИндексШаблона >= 0 && ИндексШаблона < 10 && ИндексШаблона < данные.СписокФормируемыхСообщений.Count)
                        {
                            var ФормируемоеСообщение = данные.СписокФормируемыхСообщений[ИндексШаблона];
                            ФормируемоеСообщение.Активность = !tsmi.Checked;
                            данные.СписокФормируемыхСообщений[ИндексШаблона] = ФормируемоеСообщение;
                            SoundRecords[КлючВыбранныйМеню] = данные;
                            return;
                        }
                    }


                    if (Табло_отображениеПутиToolStripMenuItem.DropDownItems.Contains(tsmi))
                    {
                        int индексВарианта = Табло_отображениеПутиToolStripMenuItem.DropDownItems.IndexOf(tsmi);
                        if (индексВарианта >= 0)
                        {
                            данные.РазрешениеНаОтображениеПути = (PathPermissionType)индексВарианта;
                            SoundRecords[КлючВыбранныйМеню] = данные;
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





        private void lVСобытия_ОбновитьСостояниеТаблицы()
        {
            int НомерСтроки = 0;
            foreach (var taskSound in TaskManager.Tasks)
            {
                if (НомерСтроки >= lVСобытия.Items.Count)
                {
                    ListViewItem lvi1 = new ListViewItem(new string[] { taskSound.Key, taskSound.Value.Описание });
                    switch (taskSound.Value.СостояниеСтроки)
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
                    if (lVСобытия.Items[НомерСтроки].SubItems[0].Text != taskSound.Key)
                        lVСобытия.Items[НомерСтроки].SubItems[0].Text = taskSound.Key;

                    if (lVСобытия.Items[НомерСтроки].SubItems[1].Text != taskSound.Value.Описание)
                        lVСобытия.Items[НомерСтроки].SubItems[1].Text = taskSound.Value.Описание;

                    switch (taskSound.Value.СостояниеСтроки)
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


        private string currentPlayingTemplate = string.Empty;
        private void ОтобразитьСубтитры()
        {
            var subtaitles = TaskManager.GetElements.FirstOrDefault(ev => ev.СостояниеСтроки == 4);
            if (subtaitles != null && subtaitles.СостояниеСтроки == 4)
            {
                if (subtaitles.НомерСписка == 1) //статические звуковые сообщения
                {
                    if (СтатическиеЗвуковыеСообщения.Keys.Contains(subtaitles.Ключ))
                    {
                        currentPlayingTemplate = subtaitles.ШаблонИлиСообщение;
                        rtb_subtaitles.Text = currentPlayingTemplate;
                    }
                }
                else
                if (subtaitles.НомерСписка == 0) //динамические звуковые сообщения
                {
                    if (subtaitles.ШаблонИлиСообщение != currentPlayingTemplate)
                    {
                        currentPlayingTemplate = subtaitles.ШаблонИлиСообщение;
                        var card = new КарточкаДвиженияПоезда(SoundRecords[subtaitles.Ключ], subtaitles.Ключ);
                        card.ОтобразитьШаблонОповещенияНаRichTb(currentPlayingTemplate, rtb_subtaitles);
                    }
                }
            }
            else
            {
                rtb_subtaitles.Text = string.Empty;
                currentPlayingTemplate = string.Empty;
            }
        }



        private void lVСобытия_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                string Key = lVСобытия.SelectedItems[0].SubItems[0].Text;
                if (TaskManager.Tasks.ContainsKey(Key))
                {
                    var данныеСтроки = TaskManager.Tasks[Key];
                    if (данныеСтроки.НомерСписка == 1)
                    {
                        Key = данныеСтроки.Ключ;
                        if (СтатическиеЗвуковыеСообщения.Keys.Contains(Key))
                        {
                            СтатическоеСообщение Данные = СтатическиеЗвуковыеСообщения[Key];
                            КарточкаСтатическогоЗвуковогоСообщения Карточка = new КарточкаСтатическогоЗвуковогоСообщения(Данные);
                            if (Карточка.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                Данные = Карточка.ПолучитьИзмененнуюКарточку();

                                string Key2 = Данные.Время.ToString("yy.MM.dd  HH:mm:ss");
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
                                        Key2 = Данные.Время.ToString("yy.MM.dd  HH:mm:ss");
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
                        Key = данныеСтроки.Ключ;
                        if (SoundRecords.Keys.Contains(Key) == true)
                        {
                            SoundRecord Данные = SoundRecords[Key];
                            КарточкаДвиженияПоезда Карточка = new КарточкаДвиженияПоезда(Данные, Key);
                            if (Карточка.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                SoundRecord СтарыеДанные = Данные;
                                Данные = Карточка.ПолучитьИзмененнуюКарточку();
                                ИзменениеДанныхВКарточке(СтарыеДанные, Данные, Key);
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



        private SoundRecord ИзменениеДанныхВКарточке(SoundRecord старыеДанные, SoundRecord данные, string key)
        {
            данные.ТипСообщения = SoundRecordType.ДвижениеПоезда;

            //если Поменяли время--------------------------------------------------------
            if ((старыеДанные.ВремяПрибытия != данные.ВремяПрибытия) ||
                (старыеДанные.ВремяОтправления != данные.ВремяОтправления))
            {
                данные.Время = (данные.БитыАктивностиПолей & 0x04) != 0x00 ? данные.ВремяПрибытия : данные.ВремяОтправления;

                var pipelineService = new SchedulingPipelineService();
                var newkey = pipelineService.GetUniqueKey(SoundRecords.Keys, данные.Время);
                if (!string.IsNullOrEmpty(newkey))
                {
                    данные.Время = DateTime.ParseExact(newkey, "yy.MM.dd  HH:mm:ss", new DateTimeFormatInfo());
                    SoundRecords.Remove(key);           //удалим старую запись
                    SoundRecordsOld.Remove(key);
                    SoundRecords.Add(newkey, данные);   //Добавим запись под новым ключем
                    SoundRecordsOld.Add(newkey, старыеДанные);
                }
            }
            else
            {
                SoundRecords[key] = данные;
            }

            string сообщениеОбИзменениях = "";
            if (старыеДанные.НазваниеПоезда != данные.НазваниеПоезда) сообщениеОбИзменениях += "Поезд: " + старыеДанные.НазваниеПоезда + " -> " + данные.НазваниеПоезда + "; ";
            if (старыеДанные.НомерПоезда != данные.НомерПоезда) сообщениеОбИзменениях += "№Поезда: " + старыеДанные.НомерПоезда + " -> " + данные.НомерПоезда + "; ";
            if (старыеДанные.НомерПути != данные.НомерПути) сообщениеОбИзменениях += "Путь: " + старыеДанные.НомерПути + " -> " + данные.НомерПути + "; ";
            if (старыеДанные.НумерацияПоезда != данные.НумерацияПоезда) сообщениеОбИзменениях += "Нум.пути: " + старыеДанные.НумерацияПоезда.ToString() + " -> " + данные.НумерацияПоезда.ToString() + "; ";
            if (старыеДанные.СтанцияОтправления != данные.СтанцияОтправления) сообщениеОбИзменениях += "Ст.Отпр.: " + старыеДанные.СтанцияОтправления + " -> " + данные.СтанцияОтправления + "; ";
            if (старыеДанные.СтанцияНазначения != данные.СтанцияНазначения) сообщениеОбИзменениях += "Ст.Назн.: " + старыеДанные.СтанцияНазначения + " -> " + данные.СтанцияНазначения + "; ";
            if ((старыеДанные.БитыАктивностиПолей & 0x04) != 0x00) if (старыеДанные.ВремяПрибытия != данные.ВремяПрибытия) сообщениеОбИзменениях += "Прибытие: " + старыеДанные.ВремяПрибытия.ToString("HH:mm") + " -> " + данные.ВремяПрибытия.ToString("HH:mm") + "; ";
            if ((старыеДанные.БитыАктивностиПолей & 0x10) != 0x00) if (старыеДанные.ВремяОтправления != данные.ВремяОтправления) сообщениеОбИзменениях += "Отправление: " + старыеДанные.ВремяОтправления.ToString("HH:mm") + " -> " + данные.ВремяОтправления.ToString("HH:mm") + "; ";
            if (старыеДанные.Автомат != данные.Автомат) сообщениеОбИзменениях += "Режим работы измененн: " +
                    (старыеДанные.Автомат ? "Автомат" : "Ручное") + " -> " +
                    (данные.Автомат ? "Автомат" : "Ручное") + "; ";
            if (старыеДанные.ФиксированноеВремяПрибытия != данные.ФиксированноеВремяПрибытия) сообщениеОбИзменениях += "Фиксированное время ПРИБЫТИЯ измененно: " +
                    ((старыеДанные.ФиксированноеВремяПрибытия == null) ? "--:--" : старыеДанные.ФиксированноеВремяПрибытия.Value.ToString("HH:mm")) + " -> " +
                    ((данные.ФиксированноеВремяПрибытия == null) ? "--:--" : данные.ФиксированноеВремяПрибытия.Value.ToString("HH:mm")) + "; ";
            if (старыеДанные.ФиксированноеВремяОтправления != данные.ФиксированноеВремяОтправления) сообщениеОбИзменениях += "Фиксированное время ОТПРАВЛЕНИЯ измененно: " +
                    ((старыеДанные.ФиксированноеВремяОтправления == null) ? "--:--" : старыеДанные.ФиксированноеВремяОтправления.Value.ToString("HH:mm")) + " -> " +
                    ((данные.ФиксированноеВремяОтправления == null) ? "--:--" : данные.ФиксированноеВремяОтправления.Value.ToString("HH:mm")) + "; ";

            if (сообщениеОбИзменениях != "")
                Program.ЗаписьЛога("Действие оператора", "Изменение настроек поезда: " + старыеДанные.НомерПоезда + " " + старыеДанные.НазваниеПоезда + ": " + сообщениеОбИзменениях);

            return данные;
        }



        private void СохранениеИзмененийДанныхВКарточке(SoundRecord старыеДанные, SoundRecord данные)
        {
            var recChange = new SoundRecordChanges
            {
                TimeStamp = DateTime.Now,
                Rec = старыеДанные,
                NewRec = данные
            };
            SoundRecordChanges.Add(recChange);
           // var hh = Mapper.SoundRecordChanges2SoundRecordChangesDb(recChange);//DEBUG
            Program.SoundRecordChangesDbRepository.Add(Mapper.SoundRecordChanges2SoundRecordChangesDb(recChange));
        }



        private void ОбработкаРучногоВоспроизведенияШаблона(ref SoundRecord Данные, string key)
        {
            foreach (var формируемоеСообщение in Данные.СписокФормируемыхСообщений)
            {
                DateTime времяСобытия = формируемоеСообщение.ПривязкаКВремени == 0 ? Данные.ВремяПрибытия : Данные.ВремяОтправления;
                времяСобытия = времяСобытия.AddMinutes(формируемоеСообщение.ВремяСмещения);

                if (формируемоеСообщение.СостояниеВоспроизведения == SoundRecordStatus.ДобавленВОчередьРучное || формируемоеСообщение.СостояниеВоспроизведения == SoundRecordStatus.ВоспроизведениеРучное)
                {
                    if (QueueSound.FindItem(Данные.ID, формируемоеСообщение.Id) == null)
                        continue;

                    byte состояниеСтроки = 0;
                    switch (формируемоеСообщение.СостояниеВоспроизведения)
                    {
                        case SoundRecordStatus.ДобавленВОчередьРучное:
                            состояниеСтроки = 1;
                            break;

                        case SoundRecordStatus.ВоспроизведениеРучное:
                            состояниеСтроки = 4;
                            break;
                    }

                    TaskSound taskSound = new TaskSound
                    {
                        НомерСписка = 0,
                        СостояниеСтроки = состояниеСтроки,
                        Описание = Данные.НомерПоезда + " " + Данные.НазваниеПоезда + ": " + формируемоеСообщение.НазваниеШаблона,
                        Время = времяСобытия,
                        Ключ = key,
                        ParentId = формируемоеСообщение.Id,
                        ШаблонИлиСообщение = формируемоеСообщение.Шаблон
                    };

                    TaskManager.AddItem(taskSound);
                }
            }
        }




        protected override void OnClosed(EventArgs e)
        {
            DispouseCisClientIsConnectRx.Dispose();
            DispouseQueueChangeRx.Dispose();
            DispouseStaticChangeRx.Dispose();

            base.OnClosed(e);
        }
    }
}
