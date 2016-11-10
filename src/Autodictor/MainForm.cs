using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.ServiceModel;
using CommunicationDevices.Model;
using MainExample.ClientWCF;

namespace MainExample
{
    public partial class MainForm : Form
    {
        public CisClient CisClient { get; set; }
        public ExchangeModel ExchangeModel { get; set; }



        public MainForm()
        {
            InitializeComponent();

            StaticSoundForm.ЗагрузитьСписок();
            DynamicSoundForm.ЗагрузитьСписок();
            SoundConfiguration.ЗагрузитьСписок();
            TrainTable.ЗагрузитьСписок();

            //Player.PlayFile("");  //TODO: включить

            ExchangeModel = new ExchangeModel();
        }




        private void MainForm_Load(object sender, EventArgs e)
        {
            CisClient = new CisClient(new EndpointAddress("http://localhost:50000/Service/Cis"));
            CisClient.Start();
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
                MainWindowForm mainform = new MainWindowForm(CisClient);
                mainform.MdiParent = this;
                mainform.Show();
            }
        }

        //Расписание движения поездов
        private void listExample_Click(object sender, EventArgs e)
        {
            TrainTable listForm = new TrainTable(CisClient);
            listForm.MdiParent = this;
            listForm.Show();
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
                OperativeSheduleForm operativeSheduleForm = new OperativeSheduleForm(CisClient);
                operativeSheduleForm.MdiParent = this;
                operativeSheduleForm.Show();
            }
        }
    }

 

}
