using System;
using System.Windows.Forms;





namespace MainExample
{
    public partial class Оповещение : Form
    {

        public TrainTableRecord РасписаниеПоезда;

        public Оповещение(TrainTableRecord РасписаниеПоезда)
        {
            this.РасписаниеПоезда = РасписаниеПоезда;

            InitializeComponent();

            cBПутьПоУмолчанию.Items.Add("Не определен");
            foreach (var Путь in Program.НомераПутей)
                cBПутьПоУмолчанию.Items.Add(Путь);
            cBПутьПоУмолчанию.SelectedIndex = 0;
            cBПутьПоУмолчанию.Text = this.РасписаниеПоезда.TrainPathNumber;
            cBОтсчетВагонов.SelectedIndex = this.РасписаниеПоезда.TrainPathDirection;


            foreach (var Станция in Program.Станции)
            {
                cBОткуда.Items.Add(Станция);
                cBКуда.Items.Add(Станция);
            }

            rBВремяДействияС.Checked = false;
            rBВремяДействияПо.Checked = false;
            rBВремяДействияСПо.Checked = false;
            rBВремяДействияПостоянно.Checked = false;
            if ((РасписаниеПоезда.ВремяНачалаДействияРасписания <= new DateTime(1901, 1, 1)) && (РасписаниеПоезда.ВремяОкончанияДействияРасписания >= new DateTime(2099, 1, 1)))
                rBВремяДействияПостоянно.Checked = true;
            else if ((РасписаниеПоезда.ВремяНачалаДействияРасписания > new DateTime(1901, 1, 1)) && (РасписаниеПоезда.ВремяОкончанияДействияРасписания < new DateTime(2099, 1, 1)))
            {
                dTPВремяДействияС2.Value = РасписаниеПоезда.ВремяНачалаДействияРасписания;
                dTPВремяДействияПо2.Value = РасписаниеПоезда.ВремяОкончанияДействияРасписания;
                rBВремяДействияСПо.Checked = true;
            }
            else if ((РасписаниеПоезда.ВремяНачалаДействияРасписания > new DateTime(1901, 1, 1)) && (РасписаниеПоезда.ВремяОкончанияДействияРасписания >= new DateTime(2099, 1, 1)))
            {
                dTPВремяДействияС.Value = РасписаниеПоезда.ВремяНачалаДействияРасписания;
                rBВремяДействияС.Checked = true;
            }
            else if ((РасписаниеПоезда.ВремяНачалаДействияРасписания <= new DateTime(1901, 1, 1)) && (РасписаниеПоезда.ВремяОкончанияДействияРасписания < new DateTime(2099, 1, 1)))
            {
                dTPВремяДействияПо.Value = РасписаниеПоезда.ВремяОкончанияДействияРасписания;
                rBВремяДействияПо.Checked = true;
            }

            ПланРасписанияПоезда ТекущийПланРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(this.РасписаниеПоезда.Days);
            Расписание расписание = new Расписание(ТекущийПланРасписанияПоезда);
            tBОписаниеДнейСледования.Text = расписание.ПолучитьПланРасписанияПоезда().ПолучитьСтрокуОписанияРасписания();


            this.Text = "Расписание движения для поезда: " + РасписаниеПоезда.Num + " - " + РасписаниеПоезда.Name;
            tBНомерПоезда.Text = РасписаниеПоезда.Num;
            tb_Дополнение.Text = РасписаниеПоезда.Addition;

            string[] Станции = РасписаниеПоезда.Name.Split('-');
            if (Станции.Length == 2)
            {
                cBОткуда.Text = Станции[0].Trim(new char[] { ' ' });
                cBКуда.Text = Станции[1].Trim(new char[] { ' ' });
            }
            else if (Станции.Length == 1 && РасписаниеПоезда.Name != "")
            {
                cBКуда.Text = РасписаниеПоезда.Name.Trim(new char[] { ' ' }); ;
            }


            cBШаблонОповещения.Items.Add("Блокировка");

            foreach (var Item in DynamicSoundForm.DynamicSoundRecords)
                cBШаблонОповещения.Items.Add(Item.Name);

            string[] ШаблонОповещения = РасписаниеПоезда.SoundTemplates.Split(':');
            int ТипОповещенияПути = 0;
            if ((ШаблонОповещения.Length % 3) == 0)
            {
                for (int i = 0; i < ШаблонОповещения.Length / 3; i++)
                {
                    if (cBШаблонОповещения.Items.Contains(ШаблонОповещения[3 * i + 0]))
                    {
                        int.TryParse(ШаблонОповещения[3 * i + 2], out ТипОповещенияПути);
                        ТипОповещенияПути %= 2;
                        ListViewItem lvi = new ListViewItem(new string[] { ШаблонОповещения[3 * i + 0], ШаблонОповещения[3 * i + 1], Program.ТипыВремени[ТипОповещенияПути] });
                        this.lVШаблоныОповещения.Items.Add(lvi);                        
                    }
                }
            }

            cBВремяОповещения.SelectedIndex = 0;


            string ВремяПрибытия = this.РасписаниеПоезда.ArrivalTime;
            string ВремяОтправления = this.РасписаниеПоезда.DepartureTime;

            rBПрибытие.Checked = false;
            rBОтправление.Checked = false;
            rBТранзит.Checked = false;

            int Часы = 0, Минуты = 0;
            if (ВремяПрибытия == "")
            {
                rBОтправление.Checked = true;
                string[] SubStrings = ВремяОтправления.Split(':');
                if (int.TryParse(SubStrings[0], out Часы) && int.TryParse(SubStrings[1], out Минуты))
                    dTPОтправление.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Часы, Минуты, 0);
            }
            else if (ВремяОтправления == "")
            {
                rBПрибытие.Checked = true;
                string[] SubStrings = ВремяПрибытия.Split(':');
                if (int.TryParse(SubStrings[0], out Часы) && int.TryParse(SubStrings[1], out Минуты))
                    dTPОтправление.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Часы, Минуты, 0);
            }
            else
            {
                rBТранзит.Checked = true;
                string[] SubStrings;

                SubStrings = ВремяПрибытия.Split(':');
                if (int.TryParse(SubStrings[0], out Часы) && int.TryParse(SubStrings[1], out Минуты))
                    dTPОтправление.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Часы, Минуты, 0);

                SubStrings = ВремяОтправления.Split(':');
                if (int.TryParse(SubStrings[0], out Часы) && int.TryParse(SubStrings[1], out Минуты))
                    dTPПрибытие.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Часы, Минуты, 0);
            }

            cBБлокировка.Checked = !РасписаниеПоезда.Active;
            cBКатегория.SelectedIndex = (int)РасписаниеПоезда.ТипПоезда;




            rBНеОповещать.Checked = false;
            rBСоВсемиОстановками.Checked = false;
            rBБезОстановок.Checked = false;
            rBСОстановкамиНа.Checked = false;
            rBСОстановкамиКроме.Checked = false;

            if (РасписаниеПоезда.Примечание.Contains("Со всеми остановками"))
            {
                rBСоВсемиОстановками.Checked = true;
            }
            else if (РасписаниеПоезда.Примечание.Contains("Без остановок"))
            {
                rBБезОстановок.Checked = true;
            }
            else if (РасписаниеПоезда.Примечание.Contains("С остановками: "))
            {
                rBСОстановкамиНа.Checked = true;
                string Примечание = РасписаниеПоезда.Примечание.Replace("С остановками: ", "");
                string[] СписокСтанций = Примечание.Split(',');
                foreach (var Станция in СписокСтанций)
                    if (Program.Станции.Contains(Станция))
                        lVСписокСтанций.Items.Add(Станция);
            }
            else if (РасписаниеПоезда.Примечание.Contains("Кроме: "))
            {
                rBСОстановкамиКроме.Checked = true;
                string Примечание = РасписаниеПоезда.Примечание.Replace("Кроме: ", "");
                string[] СписокСтанций = Примечание.Split(',');
                foreach (var Станция in СписокСтанций)
                    if (Program.Станции.Contains(Станция))
                        lVСписокСтанций.Items.Add(Станция);
            }
            else
            {
                rBНеОповещать.Checked = true;
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            РасписаниеПоезда.Num = tBНомерПоезда.Text;
            РасписаниеПоезда.Addition = tb_Дополнение.Text;

            if (cBОткуда.Text != "")
                РасписаниеПоезда.Name = cBОткуда.Text + " - " + cBКуда.Text;
            else
                РасписаниеПоезда.Name = cBКуда.Text;

            if (rBВремяДействияС.Checked == true)
            {
                РасписаниеПоезда.ВремяНачалаДействияРасписания = dTPВремяДействияС.Value;
                РасписаниеПоезда.ВремяОкончанияДействияРасписания = new DateTime(2100, 1, 1);
            }
            else if (rBВремяДействияПо.Checked == true)
            {
                РасписаниеПоезда.ВремяНачалаДействияРасписания = new DateTime(1900, 1, 1);
                РасписаниеПоезда.ВремяОкончанияДействияРасписания = dTPВремяДействияПо.Value;
            }
            else if (rBВремяДействияСПо.Checked == true)
            {
                РасписаниеПоезда.ВремяНачалаДействияРасписания = dTPВремяДействияС2.Value;
                РасписаниеПоезда.ВремяОкончанияДействияРасписания = dTPВремяДействияПо2.Value;
            }
            else if (rBВремяДействияПостоянно.Checked == true)
            {
                РасписаниеПоезда.ВремяНачалаДействияРасписания = new DateTime(1900, 1, 1);
                РасписаниеПоезда.ВремяОкончанияДействияРасписания = new DateTime(2100, 1, 1);
            }

            РасписаниеПоезда.Active = !cBБлокировка.Checked;
            //РасписаниеПоезда.Days = ...; Уже установлены при выборе дней, либо не изменены изначально
            РасписаниеПоезда.SoundTemplates = ПолучитьШаблоныОповещения();
            РасписаниеПоезда.TrainPathNumber = cBПутьПоУмолчанию.Text;
            РасписаниеПоезда.TrainPathDirection = (byte)cBОтсчетВагонов.SelectedIndex;
            РасписаниеПоезда.ТипПоезда = (ТипПоезда)cBКатегория.SelectedIndex;


            if (rBПрибытие.Checked == true)
            {
                РасписаниеПоезда.ArrivalTime = dTPОтправление.Value.ToString("HH:mm");
                РасписаниеПоезда.StopTime = "";
                РасписаниеПоезда.DepartureTime = "";
            }
            else if (rBОтправление.Checked == true)
            {
                РасписаниеПоезда.ArrivalTime = "";
                РасписаниеПоезда.StopTime = "";
                РасписаниеПоезда.DepartureTime = dTPОтправление.Value.ToString("HH:mm");
            }
            else
            {
                РасписаниеПоезда.ArrivalTime = dTPОтправление.Value.ToString("HH:mm");
                РасписаниеПоезда.StopTime = (dTPПрибытие.Value - dTPОтправление.Value).TotalMinutes.ToString("0");
                РасписаниеПоезда.DepartureTime = dTPПрибытие.Value.ToString("HH:mm");
            }


            if (rBНеОповещать.Checked)
            {
                РасписаниеПоезда.Примечание = "";
            }
            else if (rBСоВсемиОстановками.Checked)
            {
                РасписаниеПоезда.Примечание = "Со всеми остановками";
            }
            else if (rBБезОстановок.Checked)
            {
                РасписаниеПоезда.Примечание = "Без остановок";
            }
            else if (rBСОстановкамиНа.Checked)
            {
                РасписаниеПоезда.Примечание = "С остановками: ";
                for (int i = 0; i < lVСписокСтанций.Items.Count; i++)
                    РасписаниеПоезда.Примечание += lVСписокСтанций.Items[i].SubItems[0].Text + ",";

                if (РасписаниеПоезда.Примечание.Length > 10)
                    if (РасписаниеПоезда.Примечание[РасписаниеПоезда.Примечание.Length - 1] == ',')
                        РасписаниеПоезда.Примечание = РасписаниеПоезда.Примечание.Remove(РасписаниеПоезда.Примечание.Length - 1);
            }
            else if (rBСОстановкамиКроме.Checked)
            {
                РасписаниеПоезда.Примечание = "Кроме: ";
                for (int i = 0; i < lVСписокСтанций.Items.Count; i++)
                    РасписаниеПоезда.Примечание += lVСписокСтанций.Items[i].SubItems[0].Text + ",";

                if (РасписаниеПоезда.Примечание.Length > 10)
                    if (РасписаниеПоезда.Примечание[РасписаниеПоезда.Примечание.Length - 1] == ',')
                        РасписаниеПоезда.Примечание = РасписаниеПоезда.Примечание.Remove(РасписаниеПоезда.Примечание.Length - 1);
            }


            DialogResult = System.Windows.Forms.DialogResult.OK;
        }



        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }



        public string ПолучитьШаблоныОповещения()
        {
            string РезультирующийШаблонОповещения = "";

            for (int item = 0; item < this.lVШаблоныОповещения.Items.Count; item++)
            {
                РезультирующийШаблонОповещения += this.lVШаблоныОповещения.Items[item].SubItems[0].Text + ":";
                РезультирующийШаблонОповещения += this.lVШаблоныОповещения.Items[item].SubItems[1].Text + ":";
                РезультирующийШаблонОповещения += (this.lVШаблоныОповещения.Items[item].SubItems[2].Text == "Отправление") ? "1:" : "0:";
            }

            if (РезультирующийШаблонОповещения.Length > 0)
                if (РезультирующийШаблонОповещения[РезультирующийШаблонОповещения.Length - 1] == ':')
                    РезультирующийШаблонОповещения = РезультирующийШаблонОповещения.Remove(РезультирующийШаблонОповещения.Length - 1);

            return РезультирующийШаблонОповещения;
        }

        private void rBОтправление_CheckedChanged(object sender, EventArgs e)
        {
            if (rBПрибытие.Checked)
            {
                dTPПрибытие.Visible = false;
            }
            else if (rBОтправление.Checked)
            {
                dTPПрибытие.Visible = false;
            }
            else
            {
                dTPПрибытие.Visible = true;
            }
        }

        private void btnРедактировать_Click(object sender, EventArgs e)
        {
            string СписокВыбранныхСтанций = "";
            for (int i = 0; i < lVСписокСтанций.Items.Count; i++)
                СписокВыбранныхСтанций += lVСписокСтанций.Items[i].Text + ",";

            СписокСтанций списокСтанций = new СписокСтанций(СписокВыбранныхСтанций);
            
            if (списокСтанций.ShowDialog() == DialogResult.OK)
            {
                System.Collections.Generic.List <string> РезультирующиеСтанции = списокСтанций.ПолучитьСписокВыбранныхСтанций();
                lVСписокСтанций.Items.Clear();
                foreach (var res in РезультирующиеСтанции)
                    lVСписокСтанций.Items.Add(res);
            }
        }

        private void cBБлокировка_CheckedChanged(object sender, EventArgs e)
        {
            if (cBБлокировка.Checked)
            {
                tBНомерПоезда.Enabled = false;
                cBКатегория.Enabled = false;
                gBНаправление.Enabled = false;
                gBОстановки.Enabled = false;
                gBДниСледования.Enabled = false;
                cBПутьПоУмолчанию.Enabled = false;
                cBОтсчетВагонов.Enabled = false;
                gBШаблонОповещения.Enabled = false;
            }
            else
            {
                tBНомерПоезда.Enabled = true;
                cBКатегория.Enabled = true;
                gBНаправление.Enabled = true;
                gBОстановки.Enabled = true;
                gBДниСледования.Enabled = true;
                cBПутьПоУмолчанию.Enabled = true;
                cBОтсчетВагонов.Enabled = true;
                gBШаблонОповещения.Enabled = true;
            }
        }

        private void btnДниСледования_Click(object sender, EventArgs e)
        {
            ПланРасписанияПоезда ТекущийПланРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(this.РасписаниеПоезда.Days);
            ТекущийПланРасписанияПоезда.УстановитьНомерПоезда(this.РасписаниеПоезда.Num);
            ТекущийПланРасписанияПоезда.УстановитьНазваниеПоезда(this.РасписаниеПоезда.Name);

            Расписание расписание = new Расписание(ТекущийПланРасписанияПоезда);


            string ВремяДействия = "";
            if (rBВремяДействияС.Checked)
                ВремяДействия = "c " + dTPВремяДействияС.Value.ToString("dd.MM.yyyy");
            else if (rBВремяДействияПо.Checked)
                ВремяДействия = "по " + dTPВремяДействияПо.Value.ToString("dd.MM.yyyy");
            else if (rBВремяДействияСПо.Checked)
                ВремяДействия = "c " + dTPВремяДействияС2.Value.ToString("dd.MM.yyyy") + " по " + dTPВремяДействияПо2.Value.ToString("dd.MM.yyyy");
            else
                ВремяДействия = "постоянно";

            расписание.УстановитьВремяДействия(ВремяДействия);
            расписание.ShowDialog();
            if (расписание.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                this.РасписаниеПоезда.Days = расписание.ПолучитьПланРасписанияПоезда().ПолучитьСтрокуРасписания();
                tBОписаниеДнейСледования.Text = расписание.ПолучитьПланРасписанияПоезда().ПолучитьСтрокуОписанияРасписания();
            }
        }

        private void btnДобавитьШаблон_Click(object sender, EventArgs e)
        {
            if (cBШаблонОповещения.SelectedIndex >= 0)
            {
                string ВремяОповещения = tBВремяОповещения.Text.Replace(" ", "");
                string[] Времена = ВремяОповещения.Split(',');

                int TempInt = 0;
                bool Result = true;

                foreach (var ВременнойИнтервал in Времена)
                    Result &= int.TryParse(ВременнойИнтервал, out TempInt);

                if (Result == true) 
                {
                    ListViewItem lvi = new ListViewItem(new string[] { cBШаблонОповещения.Text, tBВремяОповещения.Text, cBВремяОповещения.Text });
                    this.lVШаблоныОповещения.Items.Add(lvi);
                }
                else
                {
                    MessageBox.Show(this, "Строка должна содержать время смещения шаблона оповещения, разделенного запятыми", "Внимание !!!");
                }
            }
        }

        private void btnУдалитьШаблон_Click(object sender, EventArgs e)
        {
            while (lVШаблоныОповещения.SelectedItems.Count > 0)
            {
                lVШаблоныОповещения.Items.Remove(lVШаблоныОповещения.SelectedItems[0]);
            }
        }
    }
}
