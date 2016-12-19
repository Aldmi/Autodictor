using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MainExample
{
    public partial class КарточкаДвиженияПоезда : Form
    {
        private SoundRecord Record;
        public bool ПрименитьКоВсемСообщениям = true;

        public КарточкаДвиженияПоезда(SoundRecord Record)
        {
            this.Record = Record;
            InitializeComponent();

            cB_НомерПути.Enabled = ((this.Record.БитыАктивностиПолей & 0x01) != 0x00) ? true : false;
            gB_НумерацияПоезда.Enabled = ((this.Record.БитыАктивностиПолей & 0x02) != 0x00) ? true : false;
            gB_Прибытие.Enabled = ((this.Record.БитыАктивностиПолей & 0x04) != 0x00) ? true : false;
            gB_Стоянка.Enabled = ((this.Record.БитыАктивностиПолей & 0x08) != 0x00) ? true : false;
            gB_Отправление.Enabled = ((this.Record.БитыАктивностиПолей & 0x10) != 0x00) ? true : false;

            groupBox1.Enabled = (this.Record.ОтображениеВТаблицах == 0x02);   //разблокируем только для пригорода

            cB_НомерПути.SelectedIndex = this.Record.НомерПути;
            dTP_Время.Value = this.Record.Время;
            dTP_Прибытие.Value = this.Record.ВремяПрибытия;
            tB_ВремяСтоянки.Text = this.Record.ВремяСтоянки.ToString();
            dTP_ВремяОтправления.Value = this.Record.ВремяОтправления;

            switch (this.Record.НумерацияПоезда)
            {
                case 0: rB_Нумерация_Отсутствует.Checked = true; break;
                case 1: rB_Нумерация_СГоловы.Checked = true; break;
                case 2: rB_Нумерация_СХвоста.Checked = true; break;
            }

            ОбновитьТекстВОкне();
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
                rTB_Сообщение.Text = "";
                string Text;

                string[] НазваниеФайловПутей = new string[] { "",   "На 1ый путь", "На 2ой путь", "На 3ий путь", "На 4ый путь", "На 5ый путь", "На 6ой путь", "На 7ой путь", "На 8ой путь", "На 9ый путь", "На 10ый путь", "На 11ый путь", "На 12ый путь", "На 13ый путь", "На 14ый путь",
                                                                    "На 1ом пути", "На 2ом пути", "На 3ем пути", "На 4ом пути", "На 5ом пути", "На 6ом пути", "На 7ом пути", "На 8ом пути", "На 9ом пути", "На 10ом пути", "На 11ом пути", "На 12ом пути", "На 13ом пути", "На 14ом пути",
                                                                    "С 1ого пути", "С 2ого пути", "С 3его пути", "С 4ого пути", "С 5ого пути", "С 6ого пути", "С 7ого пути", "С 8ого пути", "С 9ого пути", "С 10ого пути", "С 11ого пути", "С 12ого пути", "С 13ого пути", "С 14ого пути" };

                string[] НазваниеФайловНумерацииПутей = new string[] { "", "Нумерация поезда с головы состава", "Нумерация поезда с хвоста состава" };

                List<int> УказательВыделенныхФрагментов = new List<int>();


                #region Движение по станциям
                lB_ПоСтанциям.Items.Clear();
                lB_СписокСтанций.Items.Clear();
                rB_ПоРасписанию.Checked = false;
                rB_ПоСтанциям.Checked = false;
                rB_КромеСтанций.Checked = false;
                rB_СоВсемиОстановками.Checked = false;

                if (this.Record.ОтображениеВТаблицах == 0x02)
                {
                    string Примечание = this.Record.Примечание;
                    if (Примечание.Contains("С остановк"))
                    {
                        rB_ПоСтанциям.Checked = true;
                        foreach (var Станция in MainWindowForm.Станции)
                        {
                            if (Примечание.Contains(Станция))
                                lB_ПоСтанциям.Items.Add(Станция);
                            else
                                lB_СписокСтанций.Items.Add(Станция);
                        }

                        lB_СписокСтанций.Enabled = true;
                        lB_ПоСтанциям.Enabled = true;
                        button2.Enabled = true;
                        button3.Enabled = true;
                        button4.Enabled = true;
                        button5.Enabled = true;
                        button6.Enabled = true;
                    }
                    else if (Примечание.Contains("Со всеми остановками"))
                    {
                        rB_СоВсемиОстановками.Checked = true;
                        foreach (var Станция in MainWindowForm.Станции)
                            lB_ПоСтанциям.Items.Add(Станция);

                        lB_СписокСтанций.Enabled = true;
                        lB_ПоСтанциям.Enabled = true;
                        button2.Enabled = true;
                        button3.Enabled = true;
                        button4.Enabled = true;
                        button5.Enabled = true;
                        button6.Enabled = true;
                    }
                    else if (Примечание.Contains("Кроме"))
                    {
                        rB_КромеСтанций.Checked = true;
                        foreach (var Станция in MainWindowForm.Станции)
                        {
                            if (Примечание.Contains(Станция))
                                lB_ПоСтанциям.Items.Add(Станция);
                            else
                                lB_СписокСтанций.Items.Add(Станция);
                        }

                        lB_СписокСтанций.Enabled = true;
                        lB_ПоСтанциям.Enabled = true;
                        button2.Enabled = true;
                        button3.Enabled = true;
                        button4.Enabled = true;
                        button5.Enabled = true;
                        button6.Enabled = true;
                    }
                    else
                    {
                        rB_ПоРасписанию.Checked = true;
                        foreach (var Станция in MainWindowForm.Станции)
                            lB_СписокСтанций.Items.Add(Станция);

                        lB_СписокСтанций.Enabled = false;
                        lB_ПоСтанциям.Enabled = false;
                        button2.Enabled = false;
                        button3.Enabled = false;
                        button4.Enabled = false;
                        button5.Enabled = false;
                        button6.Enabled = false;
                    }
                }
                #endregion


                foreach (string шаблон in Record.ИменаФайлов)
                {
                    switch (шаблон)
                    {
                        case "НОМЕР ПУТИ":
                            if ((Record.НомерПути >= 1) && (Record.НомерПути <= 14))
                            {
                                УказательВыделенныхФрагментов.Add(rTB_Сообщение.Text.Length);

                                Text = "";
                                if ((Record.ШаблонВоспроизведенияПути == 0) || (Record.ШаблонВоспроизведенияПути == 1))
                                    Text = НазваниеФайловПутей[Record.НомерПути];
                                else if ((Record.ШаблонВоспроизведенияПути == 2) || (Record.ШаблонВоспроизведенияПути == 3))
                                    Text = НазваниеФайловПутей[Record.НомерПути + (Record.ШаблонВоспроизведенияПути - 1) * 14];

                                УказательВыделенныхФрагментов.Add(Text.Length);
                                rTB_Сообщение.AppendText(Text + " ");
                            }
                            break;

                        case "ВРЕМЯ ПРИБЫТИЯ":
                            rTB_Сообщение.Text += "Время прибытия: ";
                            УказательВыделенныхФрагментов.Add(rTB_Сообщение.Text.Length);
                            Text = Record.ВремяПрибытия.ToString("HH:mm");
                            УказательВыделенныхФрагментов.Add(Text.Length);
                            rTB_Сообщение.AppendText(Text + " ");
                            break;

                        case "ВРЕМЯ СТОЯНКИ":
                            rTB_Сообщение.Text += "Стоянка: ";
                            УказательВыделенныхФрагментов.Add(rTB_Сообщение.Text.Length);
                            Text = Record.ВремяСтоянки.ToString() + " минут";
                            УказательВыделенныхФрагментов.Add(Text.Length);
                            rTB_Сообщение.AppendText(Text + " ");
                            break;

                        case "ВРЕМЯ ОТПРАВЛЕНИЯ":
                            rTB_Сообщение.Text += "Время отправления: ";
                            УказательВыделенныхФрагментов.Add(rTB_Сообщение.Text.Length);
                            Text = Record.ВремяОтправления.ToString("HH:mm");
                            УказательВыделенныхФрагментов.Add(Text.Length);
                            rTB_Сообщение.AppendText(Text + " ");
                            break;


                        case "НУМЕРАЦИЯ СОСТАВА":
                            if ((Record.НумерацияПоезда > 0) && (Record.НумерацияПоезда <= 2))
                            {
                                УказательВыделенныхФрагментов.Add(rTB_Сообщение.Text.Length);
                                Text = НазваниеФайловНумерацииПутей[Record.НумерацияПоезда];
                                УказательВыделенныхФрагментов.Add(Text.Length);
                                rTB_Сообщение.AppendText(Text + " ");
                            }
                            break;


                        case "СТАНЦИИ":
                            if (this.Record.ОтображениеВТаблицах == 0x02)
                            {
                                if (rB_СоВсемиОстановками.Checked == true)
                                {
                                    rTB_Сообщение.AppendText("Электропоезд движется со всеми остановками");
                                }
                                else if (rB_ПоСтанциям.Checked == true)
                                {
                                    rTB_Сообщение.AppendText("Электропоезд движется с остановками на станциях: ");
                                    foreach (var Станция in MainWindowForm.Станции)
                                        if (lB_ПоСтанциям.Items.Contains(Станция))
                                        {
                                            rTB_Сообщение.AppendText(Станция + " ");
                                        }
                                }
                                else if (rB_КромеСтанций.Checked == true)
                                {
                                    rTB_Сообщение.AppendText("Электропоезд движется с остановками кроме станций: ");
                                    foreach (var Станция in MainWindowForm.Станции)
                                        if (lB_ПоСтанциям.Items.Contains(Станция))
                                        {
                                            rTB_Сообщение.AppendText(Станция + " ");
                                        }
                                }
                            }
                            break;


                        default:
                            rTB_Сообщение.AppendText(шаблон + " ");
                            break;
                    }
                }

                for (int i = 0; i < УказательВыделенныхФрагментов.Count / 2; i++)
                {
                    rTB_Сообщение.SelectionStart = УказательВыделенныхФрагментов[2 * i];
                    rTB_Сообщение.SelectionLength = УказательВыделенныхФрагментов[2 * i + 1];
                    rTB_Сообщение.SelectionColor = Color.Red;
                }

                rTB_Сообщение.SelectionLength = 0;
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
            Record.НомерПути = (byte)cB_НомерПути.SelectedIndex;
            Record.НазванияТабло = Record.НомерПути != 0 ? MainWindowForm.BindingBehaviors.Select(beh => beh.GetDevicesName4Path(Record.НомерПути)).Where(str => str != null).ToArray() : null;
            ОбновитьТекстВОкне();
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
        }

        private void btn_Подтвердить_Click(object sender, EventArgs e)
        {
            ПрименитьКоВсемСообщениям = cB_ПрименитьКоВсем.Checked;
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btn_ЗадатьВремя_Click(object sender, EventArgs e)
        {
            Record.Время = dTP_Время.Value;
        }

        private void btn_ИзменитьВремяПрибытия_Click(object sender, EventArgs e)
        {
            Record.ВремяПрибытия = dTP_Прибытие.Value;
            ОбновитьТекстВОкне();
        }

        private void btn_ИзменитьВремяСтоянки_Click(object sender, EventArgs e)
        {
            uint.TryParse(tB_ВремяСтоянки.Text, out Record.ВремяСтоянки);
            ОбновитьТекстВОкне();
        }

        private void btn_ИзменитьВремяОтправления_Click(object sender, EventArgs e)
        {
            Record.ВремяОтправления = dTP_ВремяОтправления.Value;
            ОбновитьТекстВОкне();
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (lB_СписокСтанций.SelectedIndex != -1)
            {
                string Станция = (string)lB_СписокСтанций.SelectedItem;
                lB_СписокСтанций.Items.Remove(Станция);
                lB_ПоСтанциям.Items.Add(Станция);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lB_СписокСтанций.Items.Clear();
            foreach (var Станция in MainWindowForm.Станции)
                lB_ПоСтанциям.Items.Add(Станция);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (lB_ПоСтанциям.SelectedIndex != -1)
            {
                string Станция = (string)lB_ПоСтанциям.SelectedItem;
                lB_ПоСтанциям.Items.Remove(Станция);
                lB_СписокСтанций.Items.Add(Станция);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            lB_ПоСтанциям.Items.Clear();
            foreach (var Станция in MainWindowForm.Станции)
                lB_СписокСтанций.Items.Add(Станция);
        }

        private void rB_ПоСтанциям_CheckedChanged(object sender, EventArgs e)
        {
            if ((rB_ПоСтанциям.Checked == true) || (rB_КромеСтанций.Checked == true) || (rB_СоВсемиОстановками.Checked == true))
            {
                lB_СписокСтанций.Enabled = true;
                lB_ПоСтанциям.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
            }
            else
            {
                lB_СписокСтанций.Enabled = false;
                lB_ПоСтанциям.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            bool ПерваяСтанция = true;

            if (this.Record.ОтображениеВТаблицах == 0x02)
            {
                string Примечание = "";
                if (rB_СоВсемиОстановками.Checked == true)
                {
                    Примечание = "Со всеми остановками";
                }
                else if (rB_ПоСтанциям.Checked == true)
                {
                    Примечание = "С остановками: ";
                    foreach (var Станция in MainWindowForm.Станции)
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
                    foreach (var Станция in MainWindowForm.Станции)
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
            }

            ОбновитьТекстВОкне();
        }
    }
}



    

