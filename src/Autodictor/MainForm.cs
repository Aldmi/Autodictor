using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using CommunicationDevices.Model;
using System.Drawing;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using CommunicationDevices.Behavior.BindingBehavior;
using CommunicationDevices.Behavior.BindingBehavior.ToPath;
using CommunicationDevices.ClientWCF;
using MainExample.Entites;
using MainExample.Extension;



namespace MainExample
{
    public partial class MainForm : Form
    {
        public ExchangeModel ExchangeModel { get; set; }
        public IDisposable DispouseCisClientIsConnectRx { get; set; }

        static public int VisibleStyle = 0;

        static public MainForm mainForm = null;
        static public ToolStripButton СвязьСЦис = null;
        static public ToolStripButton Пауза = null;
        static public ToolStripButton Остановить = null;
        static public ToolStripButton Включить = null;
        static public ToolStripButton ОбновитьСписок = null;




        public MainForm()
        {
            InitializeComponent();

            StaticSoundForm.ЗагрузитьСписок();
            DynamicSoundForm.ЗагрузитьСписок();
            SoundConfiguration.ЗагрузитьСписок();
            TrainTable.ЗагрузитьСписок();                 
        
           // Player.PlayFile("");                          //TODO: ???? включить

            ExchangeModel = new ExchangeModel();

            if (mainForm == null)
                mainForm = this;

            СвязьСЦис = tSLСостояниеСвязиСЦИС;
            СвязьСЦис.BackColor = Color.Orange;

            Пауза = tSBПауза;
            Остановить = tSBОстановить;
            Включить = tSBВключить;
            ОбновитьСписок = tSBОбновитьСписок;

            Включить.BackColor = Color.Orange;
        }




        private void MainForm_Load(object sender, EventArgs e)
        {
            ExchangeModel.LoadSetting();
            ExchangeModel.StartCisClient();

            ExchangeModel.InitializeDeviceSoundChannelManagement();

            DispouseCisClientIsConnectRx = ExchangeModel.CisClient.IsConnectChange.Subscribe(isConnect =>
            {
                //TODO: вызывать через Invoke
                //if (isConnect)
                //{
                //    СвязьСЦис = tSLСостояниеСвязиСЦИС;
                //    СвязьСЦис.BackColor = Color.LightGreen;
                //    СвязьСЦис.Text = "ЦИС на связи";
                //}
                //else
                //{
                //    СвязьСЦис = tSLСостояниеСвязиСЦИС;
                //    СвязьСЦис .BackColor = Color.Orange;
                //    СвязьСЦис.Text = "ЦИС НЕ на связи";
                //}
            });


            btnMainWindowShow_Click(null, EventArgs.Empty);
        }


        private void btnMainWindowShow_Click(object sender, EventArgs e)
        {
            if (MainWindowForm.myMainForm != null)
            {
                MainWindowForm.myMainForm.Show();
                MainWindowForm.myMainForm.WindowState = FormWindowState.Maximized;
            }
            else
            {
                MainWindowForm mainform = new MainWindowForm(ExchangeModel.CisClient, ExchangeModel.Binding2PathBehaviors, ExchangeModel.Binding2GeneralSchedules, ExchangeModel.DeviceSoundChannelManagement)
                {
                    MdiParent = this,
                    WindowState = FormWindowState.Maximized
                };
                mainform.Show();
                mainform.btnОбновитьСписок_Click(null, EventArgs.Empty);
            }
        }

        //Расписание движения поездов
        private void listExample_Click(object sender, EventArgs e)
        {
            if (TrainTable.myMainForm != null)
            {
                TrainTable.myMainForm.Show();
                TrainTable.myMainForm.WindowState = FormWindowState.Normal;
            }
            else
            {
                TrainTable listForm = new TrainTable(ExchangeModel.CisClient) {MdiParent = this};
                listForm.Show();
            }
        }

        private void validationExample_Click(object sender, EventArgs e)
        {
            SoundConfiguration soundConfiguration = new SoundConfiguration();
            soundConfiguration.MdiParent = this;
            soundConfiguration.Show();
        }

        private void textBoxExample_Click(object sender, EventArgs e)
        {
        }

        private void dataSetExample_Click(object sender, EventArgs e)
        {
            StaticSoundForm form = new StaticSoundForm();
            form.MdiParent = this;
            form.Show();
        }

        private void arrayDataSourceExample_Click(object sender, EventArgs e)
        {
            DynamicSoundForm form = new DynamicSoundForm(ExchangeModel.DeviceSoundChannelManagement);
            form.MdiParent = this;
            form.Show();
        }


        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm();
            form.Show();
        }

        private void просмотрСправкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("about.cmd");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void OperativeShedules_Click(object sender, EventArgs e)
        {
            if (OperativeSheduleForm.MyOperativeSheduleForm != null)                                     //Открытие окна повторно, при открытом первом экземпляре.
            {
                OperativeSheduleForm.MyOperativeSheduleForm.Show();
                OperativeSheduleForm.MyOperativeSheduleForm.WindowState = FormWindowState.Normal;
            }
            else                                                                                         //Открытие окна
            {
                OperativeSheduleForm operativeSheduleForm = new OperativeSheduleForm(ExchangeModel.CisClient);
                operativeSheduleForm.MdiParent = this;
                operativeSheduleForm.Show();
            }
        }


        private void RegulatoryShedules_Click(object sender, EventArgs e)
        {
            if (RegulatorySheduleForm.MyRegulatorySheduleForm != null)                                     
            {
                RegulatorySheduleForm.MyRegulatorySheduleForm.Show();
                RegulatorySheduleForm.MyRegulatorySheduleForm.WindowState = FormWindowState.Normal;
            }
            else                                                                                         
            {
                RegulatorySheduleForm regulatorySheduleForm = new RegulatorySheduleForm(ExchangeModel.CisClient);
                regulatorySheduleForm.MdiParent = this;
                regulatorySheduleForm.Show();
            }
        }


        private void Boards_Click(object sender, EventArgs e)
        {
            if (BoardForm.MyBoardForm != null)                                     //Открытие окна повторно, при открытом первом экземпляре.
            {
                BoardForm.MyBoardForm.Show();
                BoardForm.MyBoardForm.WindowState = FormWindowState.Normal;
            }
            else                                                                   //Открытие окна
            {
                BoardForm boardForm = new BoardForm(ExchangeModel.DeviceTables);
                boardForm.MdiParent = this;
                boardForm.Show();
            }
        }


        private  void коммуникацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CommunicationForm.MyCommunicationForm != null)                                     //Открытие окна повторно, при открытом первом экземпляре.
            {
                CommunicationForm.MyCommunicationForm.Show();
                CommunicationForm.MyCommunicationForm.WindowState = FormWindowState.Normal;
            }
            else                                                                   //Открытие окна
            {
                CommunicationForm boardForm = new CommunicationForm(ExchangeModel.MasterSerialPorts, ExchangeModel.ReOpenMasterSerialPorts);
                boardForm.MdiParent = this;
                boardForm.Show();
            }
        }



        protected override void OnClosed(EventArgs e)
        {
            DispouseCisClientIsConnectRx.Dispose();

            ExchangeModel.Dispose();
            base.OnClosed(e);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            toolStripMenuItem1.Checked = true;
            toolStripMenuItem2.Checked = false;
            VisibleStyle = 0;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            toolStripMenuItem1.Checked = false;
            toolStripMenuItem2.Checked = true;
            VisibleStyle = 1;
        }

        private void добавитьСтатическоеСообщениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            СтатическоеСообщение Сообщение;
            Сообщение.ID = 0;
            Сообщение.Активность = true;
            Сообщение.Время = DateTime.Now;
            Сообщение.НазваниеКомпозиции = "";
            Сообщение.ОписаниеКомпозиции = "";
            Сообщение.СостояниеВоспроизведения = SoundRecordStatus.ОжиданиеВоспроизведения;
            КарточкаСтатическогоЗвуковогоСообщения ОкноСообщения = new КарточкаСтатическогоЗвуковогоСообщения(Сообщение);
            if (ОкноСообщения.ShowDialog() == DialogResult.OK)
            {
                Сообщение = ОкноСообщения.ПолучитьИзмененнуюКарточку();

                int ПопыткиВставитьСообщение = 5;
                while (ПопыткиВставитьСообщение-- > 0)
                {
                    string Key = Сообщение.Время.ToString("HH:mm:ss");
                    string[] SubKeys = Key.Split(':');
                    if (SubKeys[0].Length == 1)
                        Key = "0" + Key;

                    if (MainWindowForm.СтатическиеЗвуковыеСообщения.ContainsKey(Key))
                    {
                        Сообщение.Время = Сообщение.Время.AddSeconds(1);
                        continue;
                    }

                    MainWindowForm.СтатическиеЗвуковыеСообщения.Add(Key, Сообщение);
                    MainWindowForm.СтатическиеЗвуковыеСообщения.OrderBy(key => key.Value);
                    MainWindowForm.ФлагОбновитьСписокЗвуковыхСообщений = true;
                    break;
                }
            }
        }

        private void TSMIПоКалендарю_Click(object sender, EventArgs e)
        {
            TSMIПоПонедельнику.Checked = false;
            TSMIПоВторнику.Checked = false;
            TSMIПоСреде.Checked = false;
            TSMIПоЧетвергу.Checked = false;
            TSMIПоПятнице.Checked = false;
            TSMIПоСубботе.Checked = false;
            TSMIПоВоскресенью.Checked = false;
            TSMIПоКалендарю.Checked = false;

            (sender as ToolStripMenuItem).Checked = true;

            tSDDBРаботаПоДням.BackColor = TSMIПоКалендарю.Checked == true ? Color.LightGray : Color.Yellow;
            switch ((sender as ToolStripMenuItem).Name)
            {
                case "TSMIПоПонедельнику":
                    tSDDBРаботаПоДням.Text = "РАБОТА ПО ПОНЕДЕЛЬНИКУ";
                    MainWindowForm.РаботаПоНомеруДняНедели = 0;
                    break;

                case "TSMIПоВторнику":
                    tSDDBРаботаПоДням.Text = "РАБОТА ПО ВТОРНИКУ";
                    MainWindowForm.РаботаПоНомеруДняНедели = 1;
                    break;

                case "TSMIПоСреде":
                    tSDDBРаботаПоДням.Text = "РАБОТА ПО СРЕДЕ";
                    MainWindowForm.РаботаПоНомеруДняНедели = 2;
                    break;

                case "TSMIПоЧетвергу":
                    tSDDBРаботаПоДням.Text = "РАБОТА ПО ЧЕТВЕРГУ";
                    MainWindowForm.РаботаПоНомеруДняНедели = 3;
                    break;

                case "TSMIПоПятнице":
                    tSDDBРаботаПоДням.Text = "РАБОТА ПО ПЯТНИЦЕ";
                    MainWindowForm.РаботаПоНомеруДняНедели = 4;
                    break;

                case "TSMIПоСубботе":
                    tSDDBРаботаПоДням.Text = "РАБОТА ПО СУББОТЕ";
                    MainWindowForm.РаботаПоНомеруДняНедели = 5;
                    break;

                case "TSMIПоВоскресенью":
                    tSDDBРаботаПоДням.Text = "РАБОТА ПО ВОСКРЕСЕНЬЮ";
                    MainWindowForm.РаботаПоНомеруДняНедели = 6;
                    break;

                case "TSMIПоКалендарю":
                    tSDDBРаботаПоДням.Text = "РАБОТА ПО КАЛЕНДАРЮ";
                    MainWindowForm.РаботаПоНомеруДняНедели = 7;
                    break;
            }

            MainWindowForm.ФлагОбновитьСписокЖелезнодорожныхСообщенийПоДнюНедели = true;
        }



        private void настройкиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ОкноНастроек окно = new ОкноНастроек();
            окно.ShowDialog();
        }


        private void добавитьВнештатныйПоездToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ОкноДобавленияПоезда окно = new ОкноДобавленияПоезда();
            if (окно.ShowDialog() == DialogResult.OK)
            {
                SoundRecord Record = окно.Record;
                int TryCounter = 50;
                while (--TryCounter > 0)
                {
                    string Key = Record.Время.ToString("HH:mm:ss");
                    string[] SubKeys = Key.Split(':');
                    if (SubKeys[0].Length == 1)
                        Key = "0" + Key;

                    if (MainWindowForm.SoundRecords.ContainsKey(Key) == false)
                    {
                        MainWindowForm.SoundRecords.Add(Key, Record);
                        break;
                    }

                    Record.Время = Record.Время.AddSeconds(1);
                }

                MainWindowForm.SoundRecords.OrderBy(key => key.Value);
                MainWindowForm.ФлагОбновитьСписокЖелезнодорожныхСообщенийВТаблице = true;
            }
        }



        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            СписокВоспроизведения список = new СписокВоспроизведения();
            список.Show();
        }



        private void timer_Clock_Tick(object sender, EventArgs e)
        {
            toolClockLabel.Text = DateTime.Now.ToString("dd.MM  HH:mm:ss");
        }



        private void статическаяИнформацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StaticDisplayForm.MyStaticDisplayForm != null)                                     //Открытие окна повторно, при открытом первом экземпляре.
            {
                StaticDisplayForm.MyStaticDisplayForm.Show();
                StaticDisplayForm.MyStaticDisplayForm.WindowState = FormWindowState.Normal;
            }
            else                                                                   //Открытие окна
            {
                StaticDisplayForm staticDisplayForm = new StaticDisplayForm(ExchangeModel.Binding2StaticFormBehaviors);
                //staticDisplayForm.MdiParent = this;
                staticDisplayForm.Show();
            }
        }


        //DEBUG- ДОБАВЛЕНИЕ САТИЧЕСКОГО СООБШЕНИЯ В ОЧЕРЕДЬ----------------------------------------------------------------------------
        private int index = 0;
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            var Sound = StaticSoundForm.StaticSoundRecords[index];
            if (index++ > StaticSoundForm.StaticSoundRecords.Count)
                index = 0;

            var воспроизводимоеСообщение = new ВоспроизводимоеСообщение
                {
                    ParentId = null,
                    RootId = Sound.ID,
                    ИмяВоспроизводимогоФайла = Sound.Name,
                    Язык = NotificationLanguage.Ru,
                    ОчередьШаблона = null
                };
             MainWindowForm.QueueSound.AddItem(воспроизводимоеСообщение);
        }


        private int indexTemplate = 0;
        //DEBUG- ДОБАВЛЕНИЕ ДИНАМИЧЕСКОГО СООБШЕНИЯ В ОЧЕРЕДЬ
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            var template = MainWindowForm.SoundRecords.Values.ToList()[indexTemplate];
            if (indexTemplate++ > MainWindowForm.SoundRecords.Values.Count)
                indexTemplate = 0;

            if (template.СписокФормируемыхСообщений.Any())
            {
                var ФормируемоеСообщение = template.СписокФормируемыхСообщений[0];
                ФормируемоеСообщение.Воспроизведен = true;
                MainWindowForm.ВоспроизвестиШаблонОповещения("Действие оператора", template, ФормируемоеСообщение);
            }
        }
    }
}
