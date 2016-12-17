using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.ServiceModel;
using CommunicationDevices.Model;


namespace MainExample
{
    public partial class MainForm : Form
    {
        public ExchangeModel ExchangeModel { get; set; }

        

        public MainForm()
        {
            InitializeComponent();

            StaticSoundForm.ЗагрузитьСписок();
            DynamicSoundForm.ЗагрузитьСписок();
            SoundConfiguration.ЗагрузитьСписок();
            TrainTable.ЗагрузитьСписок();                   //TODO: грузится из файла по умолчанию
        
           // Player.PlayFile("");                          //TODO: ???? включить

            ExchangeModel = new ExchangeModel();
        }




        private void MainForm_Load(object sender, EventArgs e)
        {
           ExchangeModel.CreateCisClient(new EndpointAddress("http://localhost:50000/Service/Cis"));
           ExchangeModel.StartCisClient();
           ExchangeModel.LoadSetting();
        }


        private void fileExit_Click(object sender, EventArgs e)
        {
            CancelEventArgs cancel = new CancelEventArgs(false);
            Application.Exit(cancel);
        }

        private void buttonExample_Click(object sender, EventArgs e)
        {
            if (MainWindowForm.myMainForm != null)
            {
                MainWindowForm.myMainForm.Show();
                MainWindowForm.myMainForm.WindowState = FormWindowState.Normal;
            }
            else
            {
                MainWindowForm mainform = new MainWindowForm(ExchangeModel.CisClient, ExchangeModel.BindingBehaviors)
                {
                    MdiParent = this
                };
                mainform.Show();
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
            DynamicSoundForm form = new DynamicSoundForm();
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
            else                                                                                         //Открытие окна
            {
                BoardForm boardForm = new BoardForm(ExchangeModel.Devices);
                boardForm.MdiParent = this;
                boardForm.Show();
            }
        }



        protected override void OnClosed(EventArgs e)
        {
            ExchangeModel.Dispose();
            base.OnClosed(e);
        }


    }
}
