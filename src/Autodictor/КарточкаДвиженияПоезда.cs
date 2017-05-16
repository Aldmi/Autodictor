using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MainExample
{
    public partial class КарточкаДвиженияПоезда : Form
    {
        private SoundRecord Record;
        public bool ПрименитьКоВсемСообщениям = true;
        private bool СделаныИзменения = false;
        private bool РазрешениеИзменений = false;


        public КарточкаДвиженияПоезда(SoundRecord Record)
        {
            this.Record = Record;
            InitializeComponent();
            cBОтменен.Checked = !Record.Активность;

            cBПрибытие.Checked = ((this.Record.БитыАктивностиПолей & 0x04) != 0x00) ? true : false;
            cBОтправление.Checked = ((this.Record.БитыАктивностиПолей & 0x10) != 0x00) ? true : false;

            dTP_Прибытие.Enabled = cBПрибытие.Checked;
            btn_ИзменитьВремяПрибытия.Enabled = cBПрибытие.Checked;

            dTP_ВремяОтправления.Enabled = cBОтправление.Checked;
            btn_ИзменитьВремяОтправления.Enabled = cBОтправление.Checked;

            groupBox1.Enabled = (this.Record.ТипПоезда == ТипПоезда.Пригородный) || (this.Record.ТипПоезда == ТипПоезда.Ласточка) || (this.Record.ТипПоезда == ТипПоезда.РЭКС);   //разблокируем только для пригорода

            cB_НомерПути.Items.Add("Не определен");
            foreach (var Путь in Program.НомераПутей)
                cB_НомерПути.Items.Add(Путь);

            cB_НомерПути.SelectedIndex = Program.НомераПутей.IndexOf(this.Record.НомерПути) + 1;
            dTP_Прибытие.Value = this.Record.ВремяПрибытия;
            dTP_ВремяОтправления.Value = this.Record.ВремяОтправления;

            switch (this.Record.НумерацияПоезда)
            {
                case 0: rB_Нумерация_Отсутствует.Checked = true; break;
                case 1: rB_Нумерация_СГоловы.Checked = true; break;
                case 2: rB_Нумерация_СХвоста.Checked = true; break;
            }


            tb_Дополнение.Text = Record.Дополнение;
            cb_Дополнение.Checked = Record.ИспользоватьДополнение;

            ОбновитьТекстВОкне();

            string Text = "Карточка ";
            switch (Record.ТипПоезда)
            {
                case ТипПоезда.Пассажирский: Text += "пассажирского поезда: "; break;
                case ТипПоезда.Пригородный: Text += "пригородного электропоезда: "; break;
                case ТипПоезда.Скоростной: Text += "скоростного поезда: "; break;
                case ТипПоезда.Скорый: Text += "скорого поезда: "; break;
                case ТипПоезда.Ласточка: Text += "скоростного поезда Ласточка: "; break;
                case ТипПоезда.РЭКС: Text += "скоростного поезда РЭКС: "; break;
                case ТипПоезда.Фирменный: Text += "фирменного поезда: "; break;
            }
            Text += Record.НомерПоезда + ": " + Record.СтанцияОтправления + " - " + Record.СтанцияНазначения;
            this.Text = Text;

            foreach (var НомерПоезда in Program.НомераПоездов)
                cBНомерПоезда.Items.Add(НомерПоезда);
            cBНомерПоезда.Text = Record.НомерПоезда;

            foreach (var Станция in Program.Станции)
            {
                cBОткуда.Items.Add(Станция);
                cBКуда.Items.Add(Станция);
            }

            cBОткуда.Text = Record.СтанцияОтправления;
            cBКуда.Text = Record.СтанцияНазначения;


            switch (Record.КоличествоПовторений)
            {
                default:
                case 1:
                    btnПовторения.Text = "1 ПОВТОР";
                    break;

                case 2:
                    btnПовторения.Text = "2 ПОВТОРА";
                    break;

                case 3:
                    btnПовторения.Text = "3 ПОВТОРА";
                    break;
            };

            for (int i = 0; i < Record.СписокФормируемыхСообщений.Count(); i++)
            {
                var ФормируемоеСообщение = Record.СписокФормируемыхСообщений[i];

                DateTime ВремяАктивации = DateTime.Now;
                if (ФормируемоеСообщение.ПривязкаКВремени == 0)
                    ВремяАктивации = this.Record.ВремяПрибытия.AddMinutes(ФормируемоеСообщение.ВремяСмещения);
                else
                    ВремяАктивации = this.Record.ВремяОтправления.AddMinutes(ФормируемоеСообщение.ВремяСмещения);

                string языки= String.Empty;
                ФормируемоеСообщение.ЯзыкиОповещения.ForEach(lang => языки += lang.ToString() + ", ");
                языки= языки.Remove(языки.Length - 2, 2);

                ListViewItem lvi = new ListViewItem(new string[] { ВремяАктивации.ToString("HH:mm"), ФормируемоеСообщение.НазваниеШаблона, языки });
                lvi.Checked = ФормируемоеСообщение.Активность;
                lvi.Tag = i;
                this.lVШаблоны.Items.Add(lvi);

                gBНастройкиПоезда.Enabled = Record.Активность;
            }

            cBПоездОтменен.Checked = false;
            cBПрибытиеЗадерживается.Checked = false;
            cBОтправлениеЗадерживается.Checked = false;
            if ((this.Record.БитыНештатныхСитуаций & 0x01) != 0x00) cBПоездОтменен.Checked = true;
            else if ((this.Record.БитыНештатныхСитуаций & 0x02) != 0x00) cBПрибытиеЗадерживается.Checked = true;
            else if ((this.Record.БитыНештатныхСитуаций & 0x04) != 0x00) cBОтправлениеЗадерживается.Checked = true;
        }


        private void ОбновитьТекстВОкне()
        {
            if (Record.ТипСообщения == SoundRecordType.Обычное)
            {
                string ПутьКФайлу = Path.GetFileNameWithoutExtension(Record.ИменаФайлов[0]);
                rTB_Сообщение.Text = "Звуковой трек: " + ПутьКФайлу;
                rTB_Сообщение.SelectionStart = 15;
                rTB_Сообщение.SelectionLength = ПутьКФайлу.Length;
                rTB_Сообщение.SelectionColor = Color.DarkGreen;
                rTB_Сообщение.SelectionLength = 0;
            }
            else if ((Record.ТипСообщения == SoundRecordType.ДвижениеПоезда) || (Record.ТипСообщения == SoundRecordType.ДвижениеПоездаНеПодтвержденное))
            {
                #region Движение по станциям
                lB_ПоСтанциям.Items.Clear();
                rB_ПоРасписанию.Checked = false;
                rB_ПоСтанциям.Checked = false;
                rB_КромеСтанций.Checked = false;
                rB_СоВсемиОстановками.Checked = false;

                if ((this.Record.ТипПоезда == ТипПоезда.Пригородный) || (this.Record.ТипПоезда == ТипПоезда.Ласточка) || (this.Record.ТипПоезда == ТипПоезда.РЭКС))
                {
                    string Примечание = this.Record.Примечание;
                    if (Примечание.Contains("С остановк"))
                    {
                        rB_ПоСтанциям.Checked = true;
                        foreach (var Станция in Program.Станции)
                        {
                            if (Примечание.Contains(Станция))
                                lB_ПоСтанциям.Items.Add(Станция);
                        }

                        lB_ПоСтанциям.Enabled = true;
                        btnРедактировать.Enabled = true;
                    }
                    else if (Примечание.Contains("Со всеми остановками"))
                    {
                        rB_СоВсемиОстановками.Checked = true;
                        foreach (var Станция in Program.Станции)
                            lB_ПоСтанциям.Items.Add(Станция);

                        lB_ПоСтанциям.Enabled = true;
                        btnРедактировать.Enabled = true;
                    }
                    else if (Примечание.Contains("Кроме"))
                    {
                        rB_КромеСтанций.Checked = true;
                        foreach (var Станция in Program.Станции)
                        {
                            if (Примечание.Contains(Станция))
                                lB_ПоСтанциям.Items.Add(Станция);
                        }

                        lB_ПоСтанциям.Enabled = true;
                        btnРедактировать.Enabled = true;
                    }
                    else
                    {
                        rB_ПоРасписанию.Checked = true;
                        lB_ПоСтанциям.Enabled = false;
                        btnРедактировать.Enabled = false;
                    }
                }
                #endregion
            }


            //Обновить список табло
            comboBox_displayTable.Items.Clear();
            comboBox_displayTable.SelectedIndex = -1;
            if (Record.НазванияТабло != null && Record.НазванияТабло.Any())
            {
                foreach (var table in Record.НазванияТабло)
                {
                    comboBox_displayTable.Items.Add(table);
                }

                comboBox_displayTable.BackColor = Color.White;
            }
            else
            {
                comboBox_displayTable.BackColor = Color.DarkRed;
            }
        }


        private void cB_НомерПути_SelectedIndexChanged(object sender, EventArgs e)
        {
            int НомерПути = cB_НомерПути.SelectedIndex;
            Record.НомерПути = cB_НомерПути.SelectedIndex == 0 ? "" : cB_НомерПути.Text;
            Record.НазванияТабло = НомерПути != 0 ? MainWindowForm.Binding2PathBehaviors.Select(beh => beh.GetDevicesName4Path((byte)НомерПути)).Where(str => str != null).ToArray() : null;
            ОбновитьТекстВОкне();
            if (РазрешениеИзменений == true) СделаныИзменения = true;
        }


        private void rB_Нумерация_CheckedChanged(object sender, EventArgs e)
        {
            if (rB_Нумерация_Отсутствует.Checked)
                Record.НумерацияПоезда = 0;
            else if (rB_Нумерация_СГоловы.Checked)
                Record.НумерацияПоезда = 1;
            else if (rB_Нумерация_СХвоста.Checked)
                Record.НумерацияПоезда = 2;

            ОбновитьТекстВОкне();
            if (РазрешениеИзменений == true) СделаныИзменения = true;
        }


        private void btn_Подтвердить_Click(object sender, EventArgs e)
        {
            bool ПерваяСтанция = true;
            string Примечание = "";

            if (rB_СоВсемиОстановками.Checked == true)
                Примечание = "Со всеми остановками";
            else if (rB_ПоСтанциям.Checked == true)
            {
                Примечание = "С остановками: ";
                foreach (var Станция in Program.Станции)
                    if (lB_ПоСтанциям.Items.Contains(Станция))
                    {
                        if (ПерваяСтанция == true)
                            ПерваяСтанция = false;
                        else
                            Примечание += ", ";

                        Примечание += Станция;
                    }
            }
            else if (rB_КромеСтанций.Checked == true)
            {
                Примечание = "Кроме: ";
                foreach (var Станция in Program.Станции)
                    if (lB_ПоСтанциям.Items.Contains(Станция))
                    {
                        if (ПерваяСтанция == true)
                            ПерваяСтанция = false;
                        else
                            Примечание += ", ";
                        Примечание += Станция;
                    }
            }
            this.Record.Примечание = Примечание;

            Record.СтанцияОтправления = cBОткуда.Text;
            Record.СтанцияНазначения = cBКуда.Text;

            Record.Дополнение = tb_Дополнение.Text;
            Record.ИспользоватьДополнение = cb_Дополнение.Checked;

            Record.НазваниеПоезда = Record.СтанцияОтправления == "" ? Record.СтанцияНазначения : Record.СтанцияОтправления + " - " + Record.СтанцияНазначения;

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btn_ЗадатьВремя_Click(object sender, EventArgs e)
        {
            //Record.Время = dTP_Время.Value;
        }

        private void btn_ИзменитьВремяПрибытия_Click(object sender, EventArgs e)
        {
            Record.ВремяПрибытия = dTP_Прибытие.Value;
            ОбновитьТекстВОкне();
            ОбновитьСостояниеТаблицыШаблонов();
            if (РазрешениеИзменений == true) СделаныИзменения = true;
        }

        private void btn_ИзменитьВремяОтправления_Click(object sender, EventArgs e)
        {
            Record.ВремяОтправления = dTP_ВремяОтправления.Value;
            ОбновитьТекстВОкне();
            ОбновитьСостояниеТаблицыШаблонов();
            if (РазрешениеИзменений == true) СделаныИзменения = true;
        }

        public SoundRecord ПолучитьИзмененнуюКарточку()
        {
            return Record;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string СписокВыбранныхСтанций = "";
            for (int i = 0; i < lB_ПоСтанциям.Items.Count; i++)
                СписокВыбранныхСтанций += lB_ПоСтанциям.Items[i] + ",";

            СписокСтанций списокСтанций = new СписокСтанций(СписокВыбранныхСтанций);

            if (списокСтанций.ShowDialog() == DialogResult.OK)
            {
                System.Collections.Generic.List<string> РезультирующиеСтанции = списокСтанций.ПолучитьСписокВыбранныхСтанций();
                lB_ПоСтанциям.Items.Clear();
                foreach (var res in РезультирующиеСтанции)
                    lB_ПоСтанциям.Items.Add(res);

                bool ПерваяСтанция = true;

                string Примечание = "";
                if (rB_СоВсемиОстановками.Checked == true)
                {
                    Примечание = "Со всеми остановками";
                }
                else if (rB_ПоСтанциям.Checked == true)
                {
                    Примечание = "С остановками: ";
                    foreach (var Станция in Program.Станции)
                        if (lB_ПоСтанциям.Items.Contains(Станция))
                        {
                            if (ПерваяСтанция == true)
                                ПерваяСтанция = false;
                            else
                                Примечание += ", ";

                            Примечание += Станция;
                        }
                }
                else if (rB_КромеСтанций.Checked == true)
                {
                    Примечание = "Кроме: ";
                    foreach (var Станция in Program.Станции)
                        if (lB_ПоСтанциям.Items.Contains(Станция))
                        {
                            if (ПерваяСтанция == true)
                                ПерваяСтанция = false;
                            else
                                Примечание += ", ";
                            Примечание += Станция;
                        }
                }

                this.Record.Примечание = Примечание;

                ОбновитьТекстВОкне();
                if (РазрешениеИзменений == true) СделаныИзменения = true;
            }
        }


        private void rB_ПоСтанциям_CheckedChanged(object sender, EventArgs e)
        {
            if ((rB_ПоСтанциям.Checked == true) || (rB_КромеСтанций.Checked == true) || (rB_СоВсемиОстановками.Checked == true))
            {
                lB_ПоСтанциям.Enabled = true;
                btnРедактировать.Enabled = true;
            }
            else
            {
                lB_ПоСтанциям.Enabled = false;
                btnРедактировать.Enabled = false;
            }
            if (РазрешениеИзменений == true) СделаныИзменения = true;
        }


        private void btnПовторения_Click(object sender, EventArgs e)
        {
            if (btnПовторения.Text == "1 ПОВТОР")
            {
                btnПовторения.Text = "2 ПОВТОРА";
                Record.КоличествоПовторений = 2;
            }
            else if (btnПовторения.Text == "2 ПОВТОРА")
            {
                btnПовторения.Text = "3 ПОВТОРА";
                Record.КоличествоПовторений = 3;
            }
            else
            {
                btnПовторения.Text = "1 ПОВТОР";
                Record.КоличествоПовторений = 1;
            }
            if (РазрешениеИзменений == true) СделаныИзменения = true;
        }


        private void cBПрибытие_CheckedChanged(object sender, EventArgs e)
        {
            dTP_Прибытие.Enabled = cBПрибытие.Checked;
            btn_ИзменитьВремяПрибытия.Enabled = cBПрибытие.Checked;
            if (РазрешениеИзменений == true) СделаныИзменения = true;
        }


        private void cBОтправление_CheckedChanged(object sender, EventArgs e)
        {
            dTP_ВремяОтправления.Enabled = cBОтправление.Checked;
            btn_ИзменитьВремяОтправления.Enabled = cBОтправление.Checked;
            if (РазрешениеИзменений == true) СделаныИзменения = true;
        }


        private void lVШаблоны_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.lVШаблоны.SelectedIndices;

            foreach (int item in sic)
            {
                string Key = this.lVШаблоны.Items[item].SubItems[1].Text;
                string Шаблон = "";

                bool НаличиеШаблона = false;
                foreach (var Item in DynamicSoundForm.DynamicSoundRecords)
                    if (Item.Name == Key)
                    {
                        НаличиеШаблона = true;
                        Шаблон = Item.Message;
                        int ТекущийШаблон = (int)this.lVШаблоны.Items[item].Tag;
                        if (ТекущийШаблон < Record.СписокФормируемыхСообщений.Count())
                        {
                            var ФормируемоеСообщение = Record.СписокФормируемыхСообщений[ТекущийШаблон];
                        }
                        break;
                    }

                if (НаличиеШаблона == true)
                    ОтобразитьШаблонОповещенияНаRichTb(Шаблон, rTB_Сообщение);
            }
        }



        public void ОтобразитьШаблонОповещенияНаRichTb(string шаблонОповещения, RichTextBox rTb)
        {
            rTb.Text = "";
            string Text;

            string[] НазваниеФайловПутей = new string[] { "",   "На 1ый путь", "На 2ой путь", "На 3ий путь", "На 4ый путь", "На 5ый путь", "На 6ой путь", "На 7ой путь", "На 8ой путь", "На 9ый путь", "На 10ый путь", "На 11ый путь", "На 12ый путь", "На 13ый путь", "На 14ый путь", "На 15ый путь", "На 16ый путь", "На 17ый путь", "На 18ый путь", "На 19ый путь", "На 20ый путь", "На 21ый путь", "На 22ой путь", "На 23ий путь", "На 24ый путь", "На 25ый путь",
                                                                "На 1ом пути", "На 2ом пути", "На 3ем пути", "На 4ом пути", "На 5ом пути", "На 6ом пути", "На 7ом пути", "На 8ом пути", "На 9ом пути", "На 10ом пути", "На 11ом пути", "На 12ом пути", "На 13ом пути", "На 14ом пути", "На 15ом пути", "На 16ом пути", "На 17ом пути", "На 18ом пути", "На 19ом пути", "На 20ом пути", "На 21ом пути", "На 22ом пути", "На 23им пути", "На 24ом пути", "На 25ом пути",
                                                                "С 1ого пути", "С 2ого пути", "С 3его пути", "С 4ого пути", "С 5ого пути", "С 6ого пути", "С 7ого пути", "С 8ого пути", "С 9ого пути", "С 10ого пути", "С 11ого пути", "С 12ого пути", "С 13ого пути", "С 14ого пути", "С 15ого пути", "С 16ого пути", "С 17ого пути", "С 18ого пути", "С 19ого пути", "С 20ого пути", "С 21ого пути", "С 22ого пути", "С 23его пути", "С 24ого пути", "С 25ого пути" };

            string[] НазваниеФайловНумерацииПутей = new string[] { "", "Нумерация поезда с головы состава", "Нумерация поезда с хвоста состава" };

            List<int> УказательВыделенныхФрагментов = new List<int>();

            string[] ЭлементыШаблона = шаблонОповещения.Split('|');
            foreach (string шаблон in ЭлементыШаблона)
            {
                int ВидНомерацииПути = 0;
                switch (шаблон)
                {
                    case "НА НОМЕР ПУТЬ":
                    case "НА НОМЕРом ПУТИ":
                    case "С НОМЕРого ПУТИ":
                        if (шаблон == "НА НОМЕРом ПУТИ") ВидНомерацииПути = 1;
                        if (шаблон == "С НОМЕРого ПУТИ") ВидНомерацииПути = 2;
                        if (Program.НомераПутей.Contains(Record.НомерПути))
                        {
                            УказательВыделенныхФрагментов.Add(rTb.Text.Length);
                            Text = НазваниеФайловПутей[Program.НомераПутей.IndexOf(this.Record.НомерПути) + 1 + ВидНомерацииПути * 25];

                            УказательВыделенныхФрагментов.Add(Text.Length);
                            rTb.AppendText(Text + " ");
                        }
                        break;

                    case "СТ.ОТПРАВЛЕНИЯ":
                        УказательВыделенныхФрагментов.Add(rTb.Text.Length);
                        Text = Record.СтанцияОтправления;
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTb.AppendText(Text + " ");
                        break;

                    case "НОМЕР ПОЕЗДА":
                        УказательВыделенныхФрагментов.Add(rTb.Text.Length);
                        Text = Record.НомерПоезда;
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTb.AppendText(Text + " ");
                        break;

                    case "ДОПОЛНЕНИЕ":
                        УказательВыделенныхФрагментов.Add(rTb.Text.Length);
                        Text = Record.Дополнение;
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTb.AppendText(Text + " ");
                        break;

                    case "СТ.ПРИБЫТИЯ":
                        УказательВыделенныхФрагментов.Add(rTb.Text.Length);
                        Text = Record.СтанцияНазначения;
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTb.AppendText(Text + " ");
                        break;

                    case "ВРЕМЯ ПРИБЫТИЯ":
                        rTb.Text += "Время прибытия: ";
                        УказательВыделенныхФрагментов.Add(rTb.Text.Length);
                        Text = Record.ВремяПрибытия.ToString("HH:mm");
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTb.AppendText(Text + " ");
                        break;

                    case "ВРЕМЯ СТОЯНКИ":
                        rTb.Text += "Стоянка: ";
                        УказательВыделенныхФрагментов.Add(rTb.Text.Length);
                        Text = Record.ВремяСтоянки.ToString() + " минут";
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTb.AppendText(Text + " ");
                        break;

                    case "ВРЕМЯ ОТПРАВЛЕНИЯ":
                        rTb.Text += "Время отправления: ";
                        УказательВыделенныхФрагментов.Add(rTb.Text.Length);
                        Text = Record.ВремяОтправления.ToString("HH:mm");
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTb.AppendText(Text + " ");
                        break;


                    case "НУМЕРАЦИЯ СОСТАВА":
                        if ((Record.НумерацияПоезда > 0) && (Record.НумерацияПоезда <= 2))
                        {
                            УказательВыделенныхФрагментов.Add(rTb.Text.Length);
                            Text = НазваниеФайловНумерацииПутей[Record.НумерацияПоезда];
                            УказательВыделенныхФрагментов.Add(Text.Length);
                            rTb.AppendText(Text + " ");
                        }
                        break;


                    case "СТАНЦИИ":
                        if ((Record.ТипПоезда == ТипПоезда.Пригородный) || (Record.ТипПоезда == ТипПоезда.Ласточка) || (Record.ТипПоезда == ТипПоезда.РЭКС))
                        {
                            if (rB_СоВсемиОстановками.Checked == true)
                            {
                                rTb.AppendText("Электропоезд движется со всеми остановками");
                            }
                            else if (rB_ПоСтанциям.Checked == true)
                            {
                                rTb.AppendText("Электропоезд движется с остановками на станциях: ");
                                foreach (var Станция in Program.Станции)
                                    if (lB_ПоСтанциям.Items.Contains(Станция))
                                    {
                                        rTb.AppendText(Станция + " ");
                                    }
                            }
                            else if (rB_КромеСтанций.Checked == true)
                            {
                                rTb.AppendText("Электропоезд движется с остановками кроме станций: ");
                                foreach (var Станция in Program.Станции)
                                    if (lB_ПоСтанциям.Items.Contains(Станция))
                                    {
                                        rTb.AppendText(Станция + " ");
                                    }
                            }
                        }
                        break;


                    default:
                        rTb.AppendText(шаблон + " ");
                        break;
                }
            }

            for (int i = 0; i < УказательВыделенныхФрагментов.Count / 2; i++)
            {
                rTb.SelectionStart = УказательВыделенныхФрагментов[2 * i];
                rTb.SelectionLength = УказательВыделенныхФрагментов[2 * i + 1];
                rTb.SelectionColor = Color.Red;
            }

            rTb.SelectionLength = 0;
        }



        private void lVШаблоны_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            for (int item = 0; item < this.lVШаблоны.Items.Count; item++)
            {
                if (item <= Record.СписокФормируемыхСообщений.Count)
                {
                    var ФормируемоеСообщение = Record.СписокФормируемыхСообщений[item];
                    ФормируемоеСообщение.Активность = this.lVШаблоны.Items[item].Checked;
                    Record.СписокФормируемыхСообщений[item] = ФормируемоеСообщение;
                }
            }

            ОбновитьСостояниеТаблицыШаблонов();
            if (РазрешениеИзменений == true) СделаныИзменения = true;
        }

        private void btnВоспроизвестиВыбранныйШаблон_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.lVШаблоны.SelectedIndices;

            foreach (int item in sic)
            {
                int НомерШаблона = (int)this.lVШаблоны.Items[item].Tag;

                if (НомерШаблона < Record.СписокФормируемыхСообщений.Count())
                {
                    var ФормируемоеСообщение = Record.СписокФормируемыхСообщений[НомерШаблона];
                    ФормируемоеСообщение.Воспроизведен = true;
                    Record.СписокФормируемыхСообщений[item] = ФормируемоеСообщение;
                    MainWindowForm.ВоспроизвестиШаблонОповещения("Действие оператора", Record, ФормируемоеСообщение);
                    break;
                }
            }

            ОбновитьСостояниеТаблицыШаблонов();
        }

        private void ОбновитьСостояниеТаблицыШаблонов()
        {
            for (int item = 0; item < this.lVШаблоны.Items.Count; item++)
            {
                if (item <= Record.СписокФормируемыхСообщений.Count)
                {
                    var ФормируемоеСообщение = Record.СписокФормируемыхСообщений[item];

                    DateTime ВремяАктивации = DateTime.Now;
                    if (ФормируемоеСообщение.ПривязкаКВремени == 0)
                        ВремяАктивации = this.Record.ВремяПрибытия.AddMinutes(ФормируемоеСообщение.ВремяСмещения);
                    else
                        ВремяАктивации = this.Record.ВремяОтправления.AddMinutes(ФормируемоеСообщение.ВремяСмещения);
                    string ТекстовоеПредставлениеВремениАктивации = ВремяАктивации.ToString("HH:mm");

                    if (this.lVШаблоны.Items[item].Text != ТекстовоеПредставлениеВремениАктивации)
                        this.lVШаблоны.Items[item].Text = ТекстовоеПредставлениеВремениАктивации;

                    if (ФормируемоеСообщение.Воспроизведен == true)
                        this.lVШаблоны.Items[item].BackColor = Color.LightGray;
                    else
                    {
                        if (ФормируемоеСообщение.Активность == true)
                            this.lVШаблоны.Items[item].BackColor = Color.LightGreen;
                        else
                            this.lVШаблоны.Items[item].BackColor = Color.White;
                    }
                }
            }
        }



        private void КарточкаДвиженияПоезда_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (СделаныИзменения == false)
                {
                    Close();
                }
                else
                {
                    DialogResult Результат = MessageBox.Show("Вы желаете сохранить изменения?", "Внимание !!!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (Результат == DialogResult.Yes)
                    {
                        btn_Подтвердить_Click(null, null);
                    }
                    else if (Результат == DialogResult.No)
                    {
                        Close();
                    }
                }
            }
        }

        private void КарточкаДвиженияПоезда_Load(object sender, EventArgs e)
        {
            РазрешениеИзменений = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Record.Активность = !cBОтменен.Checked;
            СделаныИзменения = true;
            gBНастройкиПоезда.Enabled = Record.Активность;
        }

        private void btnОтменаПоезда_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите воспроизвести данное сообщение в эфир?", "Внимание !!!", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            string ФормируемоеСообщение = "";
            int ТипПоезда = (int)Record.ТипПоезда;

            switch ((sender as Button).Name)
            {
                case "btnОтменаПоезда":
                    ФормируемоеСообщение = Program.ШаблонОповещенияОбОтменеПоезда[ТипПоезда];
                    break;

                case "btnЗадержкаПрибытия":
                    ФормируемоеСообщение = Program.ШаблонОповещенияОЗадержкеПрибытияПоезда[ТипПоезда];
                    break;

                case "btnЗадержкаОтправления":
                    ФормируемоеСообщение = Program.ШаблонОповещенияОЗадержкеОтправленияПоезда[ТипПоезда];
                    break;
            }

            bool НаличиеШаблона = false;
            foreach (var Item in DynamicSoundForm.DynamicSoundRecords)
                if (Item.Name == ФормируемоеСообщение)
                {
                    НаличиеШаблона = true;
                    ФормируемоеСообщение = Item.Message;
                    break;
                }


            if (НаличиеШаблона == true)
            {
                СостояниеФормируемогоСообщенияИШаблон шаблонФормируемогоСообщения = new СостояниеФормируемогоСообщенияИШаблон
                {
                    Шаблон = ФормируемоеСообщение,
                    ЯзыкиОповещения = new List<NotificationLanguage> { NotificationLanguage.Ru, NotificationLanguage.Eng }, //TODO: вычислять языки оповещения 
                    НазваниеШаблона = "Авария"
                };

                MainWindowForm.ВоспроизвестиШаблонОповещения("Действие оператора", Record, шаблонФормируемогоСообщения);
            }
        }

        private void cBПоездОтменен_CheckedChanged(object sender, EventArgs e)
        {
            Record.БитыНештатныхСитуаций &= (byte)0xF8;

            if ((sender as CheckBox).Checked == true)
            switch ((sender as CheckBox).Name)
            {
                case "cBПоездОтменен":
                    Record.БитыНештатныхСитуаций |= 0x01;

                    if (cBПрибытиеЗадерживается.Checked == true)
                        cBПрибытиеЗадерживается.Checked = false;
                    if (cBОтправлениеЗадерживается.Checked == true)
                        cBОтправлениеЗадерживается.Checked = false;
                    break;

                case "cBПрибытиеЗадерживается":
                     Record.БитыНештатныхСитуаций |= 0x02;

                    if (cBПоездОтменен.Checked == true)
                        cBПоездОтменен.Checked = false;
                    if (cBОтправлениеЗадерживается.Checked == true)
                        cBОтправлениеЗадерживается.Checked = false;
                    break;

                case "cBОтправлениеЗадерживается":
                    Record.БитыНештатныхСитуаций |= 0x04;

                    if (cBПоездОтменен.Checked == true)
                        cBПоездОтменен.Checked = false;
                    if (cBПрибытиеЗадерживается.Checked == true)
                        cBПрибытиеЗадерживается.Checked = false;
                    break;
            }
        }
    }
}
