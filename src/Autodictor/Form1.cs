using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FolderTest
{
    public partial class Form1 : Form
    {
        public string PathSource { get; set; }
        public string PathDest { get; set; }




        public Form1()
        {
            InitializeComponent();

            PathSource = @"D:\Wav\Dynamic message";//\Dynamic message
            PathDest = @"D:\Dest";
        }





        private void btn_Source_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                PathSource = fbd.SelectedPath;
                tb_PathSource.Text= PathSource;
            }    
        }

        private void btn_Dest_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                PathDest = fbd.SelectedPath;
                tb_PathDest.Text = PathDest;
            }
        }



        private async void btn_Convert_Click(object sender, EventArgs e)
        {
            var converter = new Bosh8CharacterFileNameConverter(); //конверетер поставляет плеер
            var copyService= new CopyWavFileService(converter);  // сервис - отдельный самостоятельный

            if (string.IsNullOrWhiteSpace(PathSource) ||
                string.IsNullOrWhiteSpace(PathDest))
            {
                MessageBox.Show(@"Выберите папки Источника и Результата");
                return;
            }

            try
            {
               prBar.Value= 0;
               tb_Statys.Text = String.Empty;
               await copyService.CopyFile(PathSource, PathDest, prog =>{ prBar.Value = prog;}); 
               tb_Statys.ForeColor = Color.Green;
               tb_Statys.Text = @"Копирование файлов прошло успешно";
            }
            catch (Exception ex)
            {
                tb_Statys.ForeColor = Color.Red;
                tb_Statys.Text = ex.Message;
            }
        }
    }
}
