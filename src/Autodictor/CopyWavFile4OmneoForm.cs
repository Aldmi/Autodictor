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
using AutodictorBL.Sound.Converters;
using AutodictorBL.Sound.Services;



namespace MainExample
{
    public partial class CopyWavFile4OmneoForm : Form
    {
        #region Field

        private readonly IFileNameConverter _converter;

        #endregion




        #region ctor

        public string PathSource { get; set; }
        public string PathDest { get; set; }

        #endregion





        #region ctor

        public CopyWavFile4OmneoForm(IFileNameConverter converter)
        {
            _converter = converter;
            InitializeComponent();
        }

        #endregion





        #region EventHandler

        protected override void OnLoad(EventArgs e)
        {
            PathSource = Application.StartupPath + @"\Wav"; // инициализация исходного каталого по умолчанию
            tb_PathSource.Text = PathSource;
            base.OnLoad(e);
        }


        private void btn_Source_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                PathSource = fbd.SelectedPath;
                tb_PathSource.Text = PathSource;
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
            var copyService = new CopyWavFileService(_converter);

            if (string.IsNullOrWhiteSpace(PathSource) ||
                string.IsNullOrWhiteSpace(PathDest))
            {
                MessageBox.Show(@"Выберите папки Источника и Результата");
                return;
            }

            try
            {
                prBar.Value = 0;
                tb_Statys.Text = String.Empty;
                await copyService.CopyFile(PathSource, PathDest, prog => { prBar.Value = prog; });
                tb_Statys.ForeColor = Color.Green;
                tb_Statys.Text = @"Копирование файлов прошло успешно";
            }
            catch (Exception ex)
            {
                tb_Statys.ForeColor = Color.Red;
                tb_Statys.Text = ex.Message;
            }
        }

        #endregion


    }
}
