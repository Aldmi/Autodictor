using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestGrid4WinForms
{
    public partial class DataGridColumnViewStatus : Form
    {
        private readonly List<CheckBox> _checkBoxes;
        private readonly List<TextBox> _textBoxes;



        public DataGridViewColumnCollection Columns { get; set; }





        public DataGridColumnViewStatus(DataGridViewColumnCollection columns)
        {
            Columns = columns;
            InitializeComponent();


            //TODO: динамичесмки размещать элементы на форме. Name и Tag равны Columns[i].Name.
            _checkBoxes = new List<CheckBox> {chb_Id, chb_Номер, chb_ВремяПрибытия, chb_ВремяОтпр, chb_Маршрут, chb_ДниСледования};
            _textBoxes= new List<TextBox> { tb_ViewIndexId, tb_ViewIndexНомер, tb_ViewIndexВремяПриб, tb_ViewIndexВремяОтпр, tb_ViewIndexМаршрут, tb_ViewIndexДниСлед};

            Model2Controls();
        }





        private void Model2Controls()
        {
            for (var i = 0; i < Columns.Count; i++)
            {
                var chBox = _checkBoxes.FirstOrDefault(ch => (string)ch.Tag == Columns[i].Name);
                if (chBox != null)
                {
                    chBox.Checked = Columns[i].Visible;
                }

                var txtBox = _textBoxes.FirstOrDefault(ch => (string)ch.Tag == Columns[i].Name);
                if (txtBox != null)
                {
                    txtBox.Text = Columns[i].DisplayIndex.ToString();
                }
            }
        }



        private void chb_CheckedChanged(object sender, EventArgs e)
        {
            var chb = sender as CheckBox;
            if (chb != null)
            {
                for (var i = 0; i < Columns.Count; i++)
                {
                    if (Columns[i].Name == (string)chb.Tag)
                    {
                        Columns[i].Visible= chb.Checked;
               
                        var txtBox = _textBoxes.FirstOrDefault(t => ((string) t.Tag == Columns[i].Name));
                        if (txtBox != null)
                        {
                            if (Columns[i].Visible == false)
                            {
                                txtBox.Text = (Columns.Count - 1).ToString();
                            }

                            txtBox.Enabled = chb.Checked;
                        }
                        
                        return;
                    }



                }
            }
        }



        private void tb_ViewIndex_TextChanged(object sender, EventArgs e)
        {
            var txtBox = sender as TextBox;
            if (txtBox != null)
            {
                for (var i = 0; i < Columns.Count; i++)
                {
                    if (Columns[i].Name == (string)txtBox.Tag)
                    {
                        int index;
                        if (int.TryParse(txtBox.Text, out index))
                        {
                            if (index < Columns.Count)
                            {
                                Columns[i].DisplayIndex = index;
                                return;
                            }                    
                        }
                        txtBox.Text = @"0";
                        MessageBox.Show(@"Недопустимое значние индекса колонки");
                    }
                }
            }
        }
    }
}
