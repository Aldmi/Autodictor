using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainExample
{
    public partial class СписокСтанций : Form
    {
        public СписокСтанций(string СписокСтанций)
        {
            InitializeComponent();

            string[] ВыбранныеСтанции = СписокСтанций.Split(',');

            foreach (var Станция in Program.Станции)
                if (ВыбранныеСтанции.Contains(Станция.Key))
                    lVВыбранныеСтанции.Items.Add(Станция.Key);
                else
                    lVОбщийСписок.Items.Add(Станция.Key);
        }

        private void btnВыбратьВсе_Click(object sender, EventArgs e)
        {
            lVОбщийСписок.Items.Clear();

            lVВыбранныеСтанции.Items.Clear();
            foreach (var Станция in Program.Станции)
                lVВыбранныеСтанции.Items.Add(Станция.Key);
        }

        private void btnУдалитьВсе_Click(object sender, EventArgs e)
        {
            lVВыбранныеСтанции.Items.Clear();

            lVОбщийСписок.Items.Clear();
            foreach (var Станция in Program.Станции)
                lVОбщийСписок.Items.Add(Станция.Key);
        }

        private void btnВыбратьВыделенные_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.lVОбщийСписок.SelectedIndices;

            int DeletedCounter = 0;
            foreach (int item in sic)
            {
                lVВыбранныеСтанции.Items.Add(this.lVОбщийСписок.Items[item - DeletedCounter].SubItems[0].Text);
                this.lVОбщийСписок.Items[item - DeletedCounter].Remove();
                DeletedCounter++;
            }
        }

        private void btnУдалитьВыбранные_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.lVВыбранныеСтанции.SelectedIndices;

            int DeletedCounter = 0;
            foreach (int item in sic)
            {
                lVОбщийСписок.Items.Add(this.lVВыбранныеСтанции.Items[item - DeletedCounter].SubItems[0].Text);
                this.lVВыбранныеСтанции.Items[item - DeletedCounter].Remove();
                DeletedCounter++;
            }
        }

        public List<string> ПолучитьСписокВыбранныхСтанций()
        {
            List<string> TempList = new List<string>();
            List<string> Result = new List<string>();

            for (int i = 0; i < lVВыбранныеСтанции.Items.Count; i++)
                TempList.Add(lVВыбранныеСтанции.Items[i].SubItems[0].Text);

            foreach (var Станция in Program.Станции)
                if (TempList.Contains(Станция.Key))
                    Result.Add(Станция.Key);

            return Result;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
