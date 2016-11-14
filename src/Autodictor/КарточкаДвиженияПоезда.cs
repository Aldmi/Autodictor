using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

                string[] НазваниеФайловПутей = new string[] { "",   "На 1ый путь", "На 2ой путь", "На 3ий путь", "На 4ый путь", "На 5ый путь", "На 6ой путь", "На 7ой путь", "На 8ой путь", "На 9ый путь", "На 10ый путь",
                                                                    "На 1ом пути", "На 2ом пути", "На 3ем пути", "На 4ом пути", "На 5ом пути", "На 6ом пути", "На 7ом пути", "На 8ом пути", "На 9ом пути", "На 10ом пути", 
                                                                    "С 1ого пути", "С 2ого пути", "С 3его пути", "С 4ого пути", "С 5ого пути", "С 6ого пути", "С 7ого пути", "С 8ого пути", "С 9ого пути", "С 10ого пути" };

                string[] НазваниеФайловНумерацииПутей = new string[] { "", "Нумерация поезда с головы состава", "Нумерация поезда с хвоста состава" };

                List<int> УказательВыделенныхФрагментов = new List<int>();

                foreach (string шаблон in Record.ИменаФайлов)
                {
                    switch (шаблон)
                    {
                        case "НОМЕР ПУТИ":
                            if ((Record.НомерПути >= 1) && (Record.НомерПути <= 10))
                            {
                                УказательВыделенныхФрагментов.Add(rTB_Сообщение.Text.Length);

                                Text = "";
                                if ((Record.ШаблонВоспроизведенияПути == 0) || (Record.ШаблонВоспроизведенияПути == 1))
                                    Text = НазваниеФайловПутей[Record.НомерПути];
                                else if ((Record.ШаблонВоспроизведенияПути == 2) || (Record.ШаблонВоспроизведенияПути == 3))
                                    Text = НазваниеФайловПутей[Record.НомерПути + (Record.ШаблонВоспроизведенияПути - 1) * 10];

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
                            rTB_Сообщение.ForeColor = Color.Black;
                            rTB_Сообщение.Text += "Время отправления: ";
                            УказательВыделенныхФрагментов.Add(rTB_Сообщение.Text.Length);
                            rTB_Сообщение.ForeColor = Color.Red;
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
        }

        private void cB_НомерПути_SelectedIndexChanged(object sender, EventArgs e)
        {
            Record.НомерПути = (byte)cB_НомерПути.SelectedIndex;

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
    }
}
