using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MainExample.Extension;





namespace MainExample
{
    public partial class СписокВоспроизведения : Form
    {
        public СписокВоспроизведения()
        {
            InitializeComponent();
        }

        private void btnОбновить_Click(object sender, EventArgs e)
        {
            lVСписокФайлов.InvokeIfNeeded(() =>
            {
                lVСписокФайлов.Items.Clear();
                try
                {
                    for (int i = 0; i < MainWindowForm.ОчередьВоспроизводимыхЗвуковыхСообщений.Count(); i++)
                    {
                        var Данные = MainWindowForm.ОчередьВоспроизводимыхЗвуковыхСообщений.ElementAt(i);

                        ListViewItem lvi1 = new ListViewItem(new string[] { Данные });
                        this.lVСписокФайлов.Items.Add(lvi1);
                    }
                }
                catch (Exception ex) { };
            });
        }

        private void btnОчистить_Click(object sender, EventArgs e)
        {
            try
            {
                lVСписокФайлов.Items.Clear();
                MainWindowForm.ОчередьВоспроизводимыхЗвуковыхСообщений.Clear();
            }
            catch (Exception ex) { };
        }

        private void btnУдалитьВыделенные_Click(object sender, EventArgs e)
        {
            try
            {
                while (lVСписокФайлов.SelectedItems.Count > 0)
                {
                    string НазваниеФайла = lVСписокФайлов.SelectedItems[0].Text;
                    if (MainWindowForm.ОчередьВоспроизводимыхЗвуковыхСообщений.Contains(НазваниеФайла))
                        MainWindowForm.ОчередьВоспроизводимыхЗвуковыхСообщений.Remove(НазваниеФайла);

                    lVСписокФайлов.Items.Remove(lVСписокФайлов.SelectedItems[0]);
                }
            }
            catch (Exception ex) { };
        }
    }
}
