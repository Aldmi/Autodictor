﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Domain.Entitys;
using MainExample.Entites;
using MainExample.Services;


namespace MainExample
{
    public partial class КарточкаДвиженияПоезда : Form
    {
        private SoundRecord Record;
        private readonly string _key;

        public bool ПрименитьКоВсемСообщениям = true;
        private bool СделаныИзменения = false;
        private bool РазрешениеИзменений = false;
        private List<string> СтанцииВыбранногоНаправления { get; set; }
        public List<Pathways> НомераПутей { get; set; }



        public КарточкаДвиженияПоезда(SoundRecord Record, string key)
        {
            this.Record = Record;
            _key = key;
            СтанцииВыбранногоНаправления = Program.ПолучитьСтанцииНаправления(Record.Направление)?.Select(st => st.NameRu).ToList() ?? new List<string>();
            НомераПутей = Program.PathWaysRepository.List().ToList();

            InitializeComponent();


            cBОтменен.Checked = !Record.Активность;

            cBПрибытие.Checked = ((this.Record.БитыАктивностиПолей & 0x04) != 0x00) ? true : false;
            cBОтправление.Checked = ((this.Record.БитыАктивностиПолей & 0x10) != 0x00) ? true : false;

            dTP_Прибытие.Enabled = cBПрибытие.Checked;
            btn_ИзменитьВремяПрибытия.Enabled = cBПрибытие.Checked;

            dTP_ВремяОтправления.Enabled = cBОтправление.Checked;
            btn_ИзменитьВремяОтправления.Enabled = cBОтправление.Checked;

            groupBox1.Enabled = (this.Record.ТипПоезда == ТипПоезда.Пригородный) || (this.Record.ТипПоезда == ТипПоезда.Ласточка) || (this.Record.ТипПоезда == ТипПоезда.РЭКС);   //разблокируем только для пригорода

            //cB_НомерПути.Items.Add("Не определен");
            //foreach (var путь in Program.НомераПутей)
            //    cB_НомерПути.Items.Add(путь);

            //cB_НомерПути.SelectedIndex = Program.НомераПутей.IndexOf(this.Record.НомерПути) + 1;



            cB_НомерПути.Items.Add("Не определен");
            var paths = НомераПутей.Select(p=> p.Name).ToList();
            foreach (var путь in paths)
                cB_НомерПути.Items.Add(путь);

            cB_НомерПути.SelectedIndex = paths.IndexOf(this.Record.НомерПути) + 1;



            dTP_Прибытие.Value = this.Record.ВремяПрибытия;
            dTP_ВремяОтправления.Value = this.Record.ВремяОтправления;
            dTP_Задержка.Value = (this.Record.ВремяЗадержки == null) ? DateTime.Parse("00:00") : this.Record.ВремяЗадержки.Value;

            dTP_ОжидаемоеВремя.Value = this.Record.Время;

            dTP_ВремяВПути.Value = (this.Record.ВремяСледования.HasValue) ? this.Record.ВремяСледования.Value : DateTime.Parse("00:00");



            switch (this.Record.НумерацияПоезда)
            {
                case 0: rB_Нумерация_Отсутствует.Checked = true; break;
                case 1: rB_Нумерация_СГоловы.Checked = true; break;
                case 2: rB_Нумерация_СХвоста.Checked = true; break;
            }


            tb_Дополнение.Text = Record.Дополнение;
            cb_Дополнение_Звук.Checked = Record.ИспользоватьДополнение["звук"];
            cb_Дополнение_Табло.Checked = Record.ИспользоватьДополнение["табло"];

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

            foreach (var номерПоезда in Program.НомераПоездов)
            {
                cBНомерПоезда.Items.Add(номерПоезда);
                cBНомерПоезда2.Items.Add(номерПоезда);
            }
            cBНомерПоезда.Text = Record.НомерПоезда;
            cBНомерПоезда2.Text = Record.НомерПоезда2;


            var directions = Program.DirectionRepository.List().ToList();
            if (directions.Any())
            {
                var stationsNames = directions.FirstOrDefault(d => d.Name == Record.Направление)?.Stations?.Select(st => st.NameRu).ToArray();
                if (stationsNames != null && stationsNames.Any())
                {
                    cBОткуда.Items.AddRange(stationsNames);
                    cBКуда.Items.AddRange(stationsNames);
                }
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

                string языки = String.Empty;
                ФормируемоеСообщение.ЯзыкиОповещения.ForEach(lang => языки += lang.ToString() + ", ");
                языки = языки.Remove(языки.Length - 2, 2);

                ListViewItem lvi = new ListViewItem(new string[] { ВремяАктивации.ToString("HH:mm"), ФормируемоеСообщение.НазваниеШаблона, языки });
                lvi.Checked = ФормируемоеСообщение.Активность;
                lvi.Tag = i;
                this.lVШаблоны.Items.Add(lvi);

                gBНастройкиПоезда.Enabled = Record.Активность;
            }

            cBПоездОтменен.Checked = false;
            cBПрибытиеЗадерживается.Checked = false;
            cBОтправлениеЗадерживается.Checked = false;
            cBОтправлениеПоГотовности.Checked = false;
            if ((this.Record.БитыНештатныхСитуаций & 0x01) != 0x00) cBПоездОтменен.Checked = true;
            else if ((this.Record.БитыНештатныхСитуаций & 0x02) != 0x00) cBПрибытиеЗадерживается.Checked = true;
            else if ((this.Record.БитыНештатныхСитуаций & 0x04) != 0x00) cBОтправлениеЗадерживается.Checked = true;
            else if ((this.Record.БитыНештатныхСитуаций & 0x08) != 0x00) cBОтправлениеПоГотовности.Checked = true;

            if (this.Record.Автомат)
            {
                btn_Автомат.Text = "АВТОМАТ";
                btn_Автомат.BackColor = Color.Aquamarine;
                btn_Фиксировать.Enabled = false;
            }
            else
            {
                btn_Автомат.Text = "РУЧНОЙ";
                btn_Автомат.BackColor = Color.DarkSlateBlue;
                btn_Фиксировать.Enabled = true;
            }

            lb_фиксВрПриб.Text = Record.ФиксированноеВремяПрибытия == null ? "--:--" : Record.ФиксированноеВремяПрибытия.Value.ToString("t");
            lb_фиксВрОтпр.Text = Record.ФиксированноеВремяОтправления == null ? "--:--" : Record.ФиксированноеВремяОтправления.Value.ToString("t");
            lb_фиксВрПриб.BackColor = Record.ФиксированноеВремяПрибытия == null ? Color.Empty: Color.Aqua;
            lb_фиксВрОтпр.BackColor = Record.ФиксированноеВремяОтправления == null ? Color.Empty : Color.Aqua;
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
                    var списокСтанцийParse = Примечание.Substring(Примечание.IndexOf(":", StringComparison.Ordinal) + 1).Split(',').Select(st => st.Trim()).ToList();

                    if (Примечание.Contains("С остановк"))
                    {
                        rB_ПоСтанциям.Checked = true;
                        foreach (var станция in СтанцииВыбранногоНаправления)
                        {
                            if (списокСтанцийParse.Contains(станция))
                                lB_ПоСтанциям.Items.Add(станция);
                        }

                        lB_ПоСтанциям.Enabled = true;
                        btnРедактировать.Enabled = true;
                    }
                    else if (Примечание.Contains("Со всеми остановками"))
                    {
                        rB_СоВсемиОстановками.Checked = true;
                        foreach (var станция in СтанцииВыбранногоНаправления)
                            lB_ПоСтанциям.Items.Add(станция);

                        lB_ПоСтанциям.Enabled = true;
                        btnРедактировать.Enabled = true;
                    }
                    else if (Примечание.Contains("Кроме"))
                    {
                        rB_КромеСтанций.Checked = true;
                        foreach (var станция in СтанцииВыбранногоНаправления)
                        {
                            if (списокСтанцийParse.Contains(станция))
                                lB_ПоСтанциям.Items.Add(станция);
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


            if (cBОтправлениеЗадерживается.Checked || cBПрибытиеЗадерживается.Checked)
            {
                dTP_Задержка.Enabled = true;
                btn_ИзменитьВремяЗадержки.Enabled = true;
            }
            else
            {
                dTP_Задержка.Enabled = false;
                btn_ИзменитьВремяЗадержки.Enabled = false;
            }


            var время = (cBПрибытие.Checked) ? Record.ВремяПрибытия : Record.ВремяОтправления;
            if (Record.ВремяЗадержки != null)
                Record.ОжидаемоеВремя = время.AddHours(Record.ВремяЗадержки.Value.Hour).AddMinutes(Record.ВремяЗадержки.Value.Minute);
            dTP_ОжидаемоеВремя.Value = Record.ОжидаемоеВремя;


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


            Record.НомерПоезда = cBНомерПоезда.Text;
            Record.НомерПоезда2 = cBНомерПоезда2.Text;


            if (rB_СоВсемиОстановками.Checked == true)
                Примечание = "Со всеми остановками";
            else if (rB_ПоСтанциям.Checked == true)
            {
                Примечание = "С остановками: ";
                foreach (var станция in СтанцииВыбранногоНаправления)
                    if (lB_ПоСтанциям.Items.Contains(станция))
                    {
                        if (ПерваяСтанция == true)
                            ПерваяСтанция = false;
                        else
                            Примечание += ", ";

                        Примечание += станция;
                    }
            }
            else if (rB_КромеСтанций.Checked == true)
            {
                Примечание = "Кроме: ";
                foreach (var станция in СтанцииВыбранногоНаправления)
                    if (lB_ПоСтанциям.Items.Contains(станция))
                    {
                        if (ПерваяСтанция == true)
                            ПерваяСтанция = false;
                        else
                            Примечание += ", ";
                        Примечание += станция;
                    }
            }
            this.Record.Примечание = Примечание;

            Record.СтанцияОтправления = cBОткуда.Text;
            Record.СтанцияНазначения = cBКуда.Text;

            Record.Дополнение = tb_Дополнение.Text;
            Record.ИспользоватьДополнение["звук"] = cb_Дополнение_Звук.Checked;
            Record.ИспользоватьДополнение["табло"] = cb_Дополнение_Табло.Checked;

            Record.НазваниеПоезда = Record.СтанцияОтправления == "" ? Record.СтанцияНазначения : Record.СтанцияОтправления + " - " + Record.СтанцияНазначения;


            //корректировка суток времени отправления ТРАНЗИТА
            if (Record.БитыАктивностиПолей == 31)
            {
                var deltaDay = (Record.ВремяОтправления.Day - Record.ВремяПрибытия.Day);
                if (deltaDay > 0)
                {
                    //Отправление уже выставленно на след. сутки. Если время отправления перенесли обратно на тиек сутки, то пересчет суток
                    if (Record.ВремяОтправления.AddDays(-1) > Record.ВремяПрибытия)
                    {
                        Record.ВремяОтправления = Record.ВремяОтправления.AddDays(-1);
                    }
                }
                else
                {
                    //Отправление выставили на след сутки
                    if (Record.ВремяОтправления < Record.ВремяПрибытия)
                    {
                        Record.ВремяОтправления = Record.ВремяОтправления.AddDays(1);
                    }
                }

                Record.ВремяСтоянки = Record.ВремяОтправления - Record.ВремяПрибытия;
            }

            DialogResult = DialogResult.OK;
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


        private void btn_ИзменитьВремяЗадержки_Click(object sender, EventArgs e)
        {
            //не стоят обе галочки приб. и отпр.
            if (!(cBПрибытие.Checked || cBОтправление.Checked))
                return;

            Record.ВремяЗадержки = dTP_Задержка.Value;
            ОбновитьТекстВОкне();
            ОбновитьСостояниеТаблицыШаблонов();
            if (РазрешениеИзменений == true) СделаныИзменения = true;
        }


        private void btn_ИзменитьВремяВПути_Click(object sender, EventArgs e)
        {
            Record.ВремяСледования = dTP_ВремяВПути.Value;
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

            var direction = Program.DirectionRepository.List().FirstOrDefault(d => d.Name == Record.Направление);
            var станцииНаправления = direction?.Stations.Select(st => st.NameRu).ToArray();

            СписокСтанций списокСтанций = new СписокСтанций(СписокВыбранныхСтанций, станцииНаправления);

            if (списокСтанций.ShowDialog() == DialogResult.OK)
            {
                List<string> РезультирующиеСтанции = списокСтанций.ПолучитьСписокВыбранныхСтанций();
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
                    foreach (var станция in СтанцииВыбранногоНаправления)
                        if (lB_ПоСтанциям.Items.Contains(станция))
                        {
                            if (ПерваяСтанция == true)
                                ПерваяСтанция = false;
                            else
                                Примечание += ", ";

                            Примечание += станция;
                        }
                }
                else if (rB_КромеСтанций.Checked == true)
                {
                    Примечание = "Кроме: ";
                    foreach (var станция in СтанцииВыбранногоНаправления)
                        if (lB_ПоСтанциям.Items.Contains(станция))
                        {
                            if (ПерваяСтанция == true)
                                ПерваяСтанция = false;
                            else
                                Примечание += ", ";
                            Примечание += станция;
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

            string[] НазваниеФайловНумерацииПутей = new string[] { "", "Нумерация поезда с головы состава", "Нумерация поезда с хвоста состава" };

            List<int> УказательВыделенныхФрагментов = new List<int>();

            string[] ЭлементыШаблона = шаблонОповещения.Split('|');
            foreach (string шаблон in ЭлементыШаблона)
            {
                string текстПодстановки = String.Empty;
                Pathways путь;
                switch (шаблон)
                {
                    case "НА НОМЕР ПУТЬ":
                    case "НА НОМЕРом ПУТИ":
                    case "С НОМЕРого ПУТИ":
                        путь = НомераПутей.FirstOrDefault(p => p.Name == Record.НомерПути);
                        if(путь == null)
                            break;
                        if (шаблон == "НА НОМЕР ПУТЬ") текстПодстановки =  путь.НаНомерПуть;
                        if (шаблон == "НА НОМЕРом ПУТИ") текстПодстановки = путь.НаНомерОмПути;
                        if (шаблон == "С НОМЕРого ПУТИ") текстПодстановки = путь.СНомерОгоПути;
                        
                        УказательВыделенныхФрагментов.Add(rTb.Text.Length);
                        Text = текстПодстановки;
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTb.AppendText(Text + " ");
                        break;

                    case "ПУТЬ ДОПОЛНЕНИЕ":
                        путь = НомераПутей.FirstOrDefault(p => p.Name == Record.НомерПути);
                        текстПодстановки = путь?.Addition ?? string.Empty;
                        УказательВыделенныхФрагментов.Add(rTb.Text.Length);
                        Text = текстПодстановки;
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTb.AppendText(Text + " ");
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

                    case "НОМЕР ПОЕЗДА ТРАНЗИТ ОТПР":
                        УказательВыделенныхФрагментов.Add(rTb.Text.Length);
                        Text = Record.НомерПоезда2;
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
                        Text = Record.ВремяСтоянки.HasValue ?
                            (Record.ВремяСтоянки.Value.Hours.ToString("D2") + ":" + Record.ВремяСтоянки.Value.Minutes.ToString("D2"))
                            : String.Empty;
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

                    case "ВРЕМЯ ЗАДЕРЖКИ":
                        rTb.Text += "Время задержки: ";
                        УказательВыделенныхФрагментов.Add(rTb.Text.Length);
                        Text = (Record.ВремяЗадержки == null) ? "00:00" : this.Record.ВремяЗадержки.Value.ToString("HH:mm");
                        УказательВыделенныхФрагментов.Add(Text.Length);
                        rTb.AppendText(Text + " ");
                        break;

                    case "ОЖИДАЕМОЕ ВРЕМЯ":
                        rTb.Text += "Ожидаемое время: ";
                        УказательВыделенныхФрагментов.Add(rTb.Text.Length);
                        Text = Record.ОжидаемоеВремя.ToString("HH:mm");
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
                                foreach (var станция in СтанцииВыбранногоНаправления)
                                    if (lB_ПоСтанциям.Items.Contains(станция))
                                    {
                                        rTb.AppendText(станция + " ");
                                    }
                            }
                            else if (rB_КромеСтанций.Checked == true)
                            {
                                rTb.AppendText("Электропоезд движется с остановками кроме станций: ");
                                foreach (var станция in СтанцииВыбранногоНаправления)
                                    if (lB_ПоСтанциям.Items.Contains(станция))
                                    {
                                        rTb.AppendText(станция + " ");
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
                    ФормируемоеСообщение.СостояниеВоспроизведения = SoundRecordStatus.ДобавленВОчередьРучное;
                    ФормируемоеСообщение.Приоритет = Priority.Hight;
                    Record.СписокФормируемыхСообщений[item] = ФормируемоеСообщение;

                    MainWindowForm.ВоспроизвестиШаблонОповещения("Действие оператора", Record, ФормируемоеСообщение, ТипСообщения.Динамическое);
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

                    var ручноШаблон= ФормируемоеСообщение.НазваниеШаблона.StartsWith("@");
                    var времяПриб = (Record.ФиксированноеВремяПрибытия == null || !ручноШаблон) ? Record.ВремяПрибытия : Record.ФиксированноеВремяПрибытия.Value;
                    var времяОтпр = (Record.ФиксированноеВремяОтправления == null || !ручноШаблон) ? Record.ВремяОтправления : Record.ФиксированноеВремяОтправления.Value;
                    var времяАктивации = ФормируемоеСообщение.ПривязкаКВремени == 0 ? времяПриб.AddMinutes(ФормируемоеСообщение.ВремяСмещения) : времяОтпр.AddMinutes(ФормируемоеСообщение.ВремяСмещения);
                    string текстовоеПредставлениеВремениАктивации = времяАктивации.ToString("HH:mm");

                    if (this.lVШаблоны.Items[item].Text != текстовоеПредставлениеВремениАктивации)
                        this.lVШаблоны.Items[item].Text = текстовоеПредставлениеВремениАктивации;

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

                case "btnОтправлениеПоГотовности":
                    ФормируемоеСообщение = Program.ШаблонОповещенияООтправлениеПоГотовностиПоезда[ТипПоезда];
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
                    Id = 2000,
                    Приоритет = Priority.Hight,
                    SoundRecordId = Record.ID,
                    Шаблон = ФормируемоеСообщение,
                    ЯзыкиОповещения = new List<NotificationLanguage> { NotificationLanguage.Ru, NotificationLanguage.Eng }, //TODO: вычислять языки оповещения 
                    НазваниеШаблона = "Авария"
                };

                MainWindowForm.ВоспроизвестиШаблонОповещения("Действие оператора нештатная ситуация", Record, шаблонФормируемогоСообщения, ТипСообщения.ДинамическоеАварийное);
            }
        }



        private void cBПоездОтменен_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Record.БитыНештатныхСитуаций &= (byte)0x00;
                if ((sender as CheckBox).Checked == true)
                {
                    switch ((sender as CheckBox).Name)
                    {
                        case "cBПоездОтменен":
                            Record.БитыНештатныхСитуаций |= 0x01;
                            if (cBПрибытиеЗадерживается.Checked)
                                cBПрибытиеЗадерживается.Checked = false;
                            if (cBОтправлениеЗадерживается.Checked)
                                cBОтправлениеЗадерживается.Checked = false;
                            if (cBОтправлениеПоГотовности.Checked)
                                cBОтправлениеПоГотовности.Checked = false;
                            break;

                        case "cBПрибытиеЗадерживается":
                            Record.БитыНештатныхСитуаций |= 0x02;
                            if (cBПоездОтменен.Checked)
                                cBПоездОтменен.Checked = false;
                            if (cBОтправлениеЗадерживается.Checked)
                                cBОтправлениеЗадерживается.Checked = false;
                            if (cBОтправлениеПоГотовности.Checked)
                                cBОтправлениеПоГотовности.Checked = false;
                            break;

                        case "cBОтправлениеЗадерживается":
                            Record.БитыНештатныхСитуаций |= 0x04;
                            cBПоездОтменен.Checked = false;
                            cBПрибытиеЗадерживается.Checked = false;
                            cBОтправлениеПоГотовности.Checked = false;
                            break;

                        case "cBОтправлениеПоГотовности":
                            Record.БитыНештатныхСитуаций |= 0x08;
                            cBПоездОтменен.Checked = false;
                            cBПрибытиеЗадерживается.Checked = false;
                            cBОтправлениеЗадерживается.Checked = false;
                            break;
                    }
                    ОбновитьТекстВОкне();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }


        }



        private void btn_Автомат_Click(object sender, EventArgs e)
        {
            if (this.Record.Автомат)
            {
                this.Record.Автомат = false;
                btn_Автомат.Text = "РУЧНОЙ";
                btn_Автомат.BackColor = Color.DarkSlateBlue;
                btn_Фиксировать.Enabled = true;
            }
            else
            {
                this.Record.Автомат = true;
                btn_Автомат.Text = "АВТОМАТ";
                btn_Автомат.BackColor = Color.Aquamarine;
                btn_Фиксировать.Enabled = false;

                СброситьФиксированноеВремяВШаблонах();
                ОбновитьСостояниеТаблицыШаблонов();
            }
        }


        private void СброситьФиксированноеВремяВШаблонах()
        {
            Record.ФиксированноеВремяПрибытия = null;
            Record.ФиксированноеВремяОтправления = null;
            lb_фиксВрПриб.Text = @"--:--";
            lb_фиксВрОтпр.Text = @"--:--";
            lb_фиксВрПриб.BackColor = Color.Empty;
            lb_фиксВрОтпр.BackColor = Color.Empty;
        }


        private void btn_Фиксировать_Click(object sender, EventArgs e)
        {
            DateTime текВремя = DateTime.Now;
            текВремя = текВремя.AddSeconds(-текВремя.Second);

            Record.ФиксированноеВремяПрибытия = текВремя;
            Record.ФиксированноеВремяОтправления = (Record.ВремяСтоянки != null) ? (текВремя + Record.ВремяСтоянки.Value) : текВремя;

            lb_фиксВрПриб.Text = Record.ФиксированноеВремяПрибытия.Value.ToString("t");
            lb_фиксВрОтпр.Text = Record.ФиксированноеВремяОтправления.Value.ToString("t");
            lb_фиксВрПриб.BackColor = Color.Aqua;
            lb_фиксВрОтпр.BackColor = Color.Aqua;

            int? привязкаКоВремени = null;
            if (Record.ФиксированноеВремяПрибытия == Record.ФиксированноеВремяОтправления)
            {
                привязкаКоВремени = null;     //шаблоны привязанные к ПРИБ и ОТПР с 0 смещением добавятся в очередь
            }
            else
            if (Record.ФиксированноеВремяПрибытия == текВремя)
            {
                привязкаКоВремени = 0;     //шаблоны привязанные к ПРИБ с 0 смещением добавятся в очередь
            }
            else
            if (Record.ФиксированноеВремяОтправления == текВремя)
            {
                привязкаКоВремени = 1;   //шаблоны привязанные к ОТПР с 0 смещением добавятся в очередь
            }

            ДобавитьШаблонВОчередьЗвуковыхСообщений(привязкаКоВремени);
            ОбновитьСостояниеТаблицыШаблонов();
        }


        private void ДобавитьШаблонВОчередьЗвуковыхСообщений(int? привязкаКоВремени)
        {
            for (int i= 0; i < Record.СписокФормируемыхСообщений.Count; i++)
            {
                var формируемоеСообщение = Record.СписокФормируемыхСообщений[i];
                if (привязкаКоВремени != null &&
                    формируемоеСообщение.ПривязкаКВремени != привязкаКоВремени.Value)
                {
                    continue;
                }

                if (формируемоеСообщение.НазваниеШаблона.StartsWith("@") && формируемоеСообщение.ВремяСмещения == 0)
                {
                    формируемоеСообщение.Воспроизведен = true;
                    формируемоеСообщение.СостояниеВоспроизведения = SoundRecordStatus.ДобавленВОчередьРучное;
                    формируемоеСообщение.Приоритет = Priority.Hight;
                    Record.СписокФормируемыхСообщений[i] = формируемоеСообщение;

                    MainWindowForm.ВоспроизвестиШаблонОповещения("Воспроизведение шаблона в ручном режиме при фиксации времени", Record, формируемоеСообщение, ТипСообщения.Динамическое);
                }
            }
        }
    }
}
