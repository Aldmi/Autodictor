﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainExample
{
    public struct Настройки
    {
        public float ЗадержкаМеждуЗвуковымиСообщениями;
        public float ИнтервалМеждуОповещениемОбОтменеПоезда;
        public float ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда;
        public float ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда;
        public float ИнтервалМеждуОповещениемООтправлениеПоГотовности;

        public float ОповещениеСамогоРаннегоВремениШаблона;

        public bool АвтФормСообщНаПассажирскийПоезд;
        public bool АвтФормСообщНаСкорыйПоезд;
        public bool АвтФормСообщНаСкоростнойПоезд;
        public bool АвтФормСообщНаПригородныйЭлектропоезд;
        public bool АвтФормСообщНаФирменный;
        public bool АвтФормСообщНаЛасточку;
        public bool АвтФормСообщНаРЭКС;

        public bool EngСообщНаПассажирскийПоезд;
        public bool EngСообщНаСкорыйПоезд;
        public bool EngСообщНаСкоростнойПоезд;
        public bool EngСообщНаПригородныйЭлектропоезд;
        public bool EngСообщНаФирменный;
        public bool EngСообщНаЛасточку;
        public bool EngСообщНаРЭКС;

        public bool РазрешениеДобавленияЗаблокированныхПоездовВСписок;
        public bool РазрешениеАвтообновленияРасписания;
        public DateTime ВремяАвтообновленияРасписания;

        public Color[] НастройкиЦветов;

        public bool[] КаналыДальнегоСлед;
        public bool[] КаналыПригорода;
        public bool[] КаналыПлатформ;
        public bool[] КаналыКасс;

        public float КаналыПериодОтправкиПакетов;

        public int УровеньГромкостиДень;
        public int УровеньГромкостиНочь;

        public DateTime ВремяНочнойПериодНачало;
        public DateTime ВремяНочнойПериодКонец;

        public Font FontПригород;
        public Font FontДальние;


        #region Methode

        public int ВыборУровняГромкости()
        {
            if (DateTime.Now >= ВремяНочнойПериодНачало &&
                DateTime.Now <= ВремяНочнойПериодКонец)
            {
                return УровеньГромкостиНочь;
            }

            return УровеньГромкостиДень;
        }

        #endregion
    };



    public partial class ОкноНастроек : Form
    {
        private Panel[] _панели;

        private CheckBox[] _каналыДальнегоСлед;
        private CheckBox[] _каналыПригорода;
        private CheckBox[] _каналыПлатформ;
        private CheckBox[] _каналыКасс;


        public ОкноНастроек()
        {
            InitializeComponent();

            _панели = new Panel[] { pCol1, pCol2, pCol3, pCol4, pCol5, pCol6, pCol7, pCol8, pCol9, pCol10, pCol11, pCol12, pCol13, pCol14, pCol15, pCol16, pCol17, pCol18 };

            _каналыДальнегоСлед= new CheckBox[] {chBox1_LongDist, chBox2_LongDist, chBox3_LongDist, chBox4_LongDist, chBox5_LongDist, chBox6_LongDist, chBox7_LongDist, chBox8_LongDist, chBox9_LongDist, chBox10_LongDist, chBox11_LongDist, chBox12_LongDist, chBox13_LongDist, chBox14_LongDist, chBox15_LongDist, chBox16_LongDist, chBox17_LongDist, chBox18_LongDist, chBox19_LongDist, chBox20_LongDist};
            _каналыПригорода = new CheckBox[] { chBox1_Suburb, chBox2_Suburb, chBox3_Suburb, chBox4_Suburb, chBox5_Suburb, chBox6_Suburb, chBox7_Suburb, chBox8_Suburb, chBox9_Suburb, chBox10_Suburb, chBox11_Suburb, chBox12_Suburb, chBox13_Suburb, chBox14_Suburb, chBox15_Suburb, chBox16_Suburb, chBox17_Suburb, chBox18_Suburb, chBox19_Suburb, chBox20_Suburb };
            _каналыПлатформ = new CheckBox[] { chBox1_Platform, chBox2_Platform, chBox3_Platform, chBox4_Platform, chBox5_Platform, chBox6_Platform, chBox7_Platform, chBox8_Platform, chBox9_Platform, chBox10_Platform, chBox11_Platform, chBox12_Platform, chBox13_Platform, chBox14_Platform, chBox15_Platform, chBox16_Platform, chBox17_Platform, chBox18_Platform, chBox19_Platform, chBox20_Platform };
            _каналыКасс = new CheckBox[] { chBox1_Cashbox, chBox2_Cashbox, chBox3_Cashbox, chBox4_Cashbox, chBox5_Cashbox, chBox6_Cashbox, chBox7_Cashbox, chBox8_Cashbox, chBox9_Cashbox, chBox10_Cashbox, chBox11_Cashbox, chBox12_Cashbox, chBox13_Cashbox, chBox14_Cashbox, chBox15_Cashbox, chBox16_Cashbox, chBox17_Cashbox, chBox18_Cashbox, chBox19_Cashbox, chBox20_Cashbox };

            ОтобразитьНастройкиВОкне();
        }



        private void btnЗагрузить_Click(object sender, EventArgs e)
        {
            ЗагрузитьНастройки();
            ОтобразитьНастройкиВОкне();
        }



        private void btnСохранить_Click(object sender, EventArgs e)
        {
            СчитатьНастройкиИзОкнаВФайл();
            СохранитьНастройки();
        }



        private void ОтобразитьНастройкиВОкне()
        {
            tBВремяМеждуСообщениями.Text = Program.Настройки.ЗадержкаМеждуЗвуковымиСообщениями.ToString("0.0");
            tBИнтОповещОбОтменеПоезда.Text = Program.Настройки.ИнтервалМеждуОповещениемОбОтменеПоезда.ToString("0.0");
            tBИнтОповещОЗадержкеПрибытия.Text = Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда.ToString("0.0");
            tBИнтОповещОЗадержкеОтправления.Text = Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда.ToString("0.0");
            tBИнтОповещООтпрПоГотов.Text = Program.Настройки.ИнтервалМеждуОповещениемООтправлениеПоГотовности.ToString("0.0");
            tBСамоеРанееВремяШаблона.Text = Program.Настройки.ОповещениеСамогоРаннегоВремениШаблона.ToString("0.0");

            cBПассажирскийПоезд.Checked = Program.Настройки.АвтФормСообщНаПассажирскийПоезд;
            cBСкорыйПоезд.Checked = Program.Настройки.АвтФормСообщНаСкорыйПоезд;
            cBСкоростнойПоезд.Checked = Program.Настройки.АвтФормСообщНаСкоростнойПоезд;
            cBПригЭлектропоезд.Checked = Program.Настройки.АвтФормСообщНаПригородныйЭлектропоезд;
            cBФирменный.Checked = Program.Настройки.АвтФормСообщНаФирменный;
            cBЛасточка.Checked = Program.Настройки.АвтФормСообщНаЛасточку;
            cBРЭКС.Checked = Program.Настройки.АвтФормСообщНаРЭКС;

            cBПассажирскийПоездEng.Checked = Program.Настройки.EngСообщНаПассажирскийПоезд;
            cBСкорыйПоездEng.Checked = Program.Настройки.EngСообщНаСкорыйПоезд;
            cBСкоростнойПоездEng.Checked = Program.Настройки.EngСообщНаСкоростнойПоезд;
            cBПригЭлектропоездEng.Checked = Program.Настройки.EngСообщНаПригородныйЭлектропоезд;
            cBФирменныйEng.Checked = Program.Настройки.EngСообщНаФирменный;
            cBЛасточкаEng.Checked = Program.Настройки.EngСообщНаЛасточку;
            cBРЭКСEng.Checked = Program.Настройки.EngСообщНаРЭКС;

            cBРазрешениеДобавленияЗаблокированныхПоездовВСписок.Checked = Program.Настройки.РазрешениеДобавленияЗаблокированныхПоездовВСписок;
            cbРазрешитьАвтоОбновлениеРасписания.Checked = Program.Настройки.РазрешениеАвтообновленияРасписания;
            dTP_Автообновление.Value = Program.Настройки.ВремяАвтообновленияРасписания;

            for (int i = 0; i < 18; i++)
                _панели[i].BackColor = Program.Настройки.НастройкиЦветов[i];

            for (int i = 0; i < Program.Настройки.КаналыДальнегоСлед.Length; i++)
                _каналыДальнегоСлед[i].Checked = Program.Настройки.КаналыДальнегоСлед[i];

            for (int i = 0; i < Program.Настройки.КаналыПригорода.Length; i++)
                _каналыПригорода[i].Checked = Program.Настройки.КаналыПригорода[i];

            for (int i = 0; i < Program.Настройки.КаналыПлатформ.Length; i++)
                _каналыПлатформ[i].Checked = Program.Настройки.КаналыПлатформ[i];

            for (int i = 0; i < Program.Настройки.КаналыКасс.Length; i++)
                _каналыКасс[i].Checked = Program.Настройки.КаналыКасс[i];

            tBКаналыПериодОтправкиПакетов.Text = Program.Настройки.КаналыПериодОтправкиПакетов.ToString("0.0");

            tBРегуляторГромкостиДень.Value = Program.Настройки.УровеньГромкостиДень;
            lbl_громкостьДень.Text = ПреобразрватьУровеньГромкостиВПроценты(Program.Настройки.УровеньГромкостиДень).ToString("F") + @"%";

            tBРегуляторГромкостиНочь.Value = Program.Настройки.УровеньГромкостиНочь;
            lbl_громкостьНочь.Text = ПреобразрватьУровеньГромкостиВПроценты(Program.Настройки.УровеньГромкостиНочь).ToString("F") + @"%";

            dTP_НочнойПериодНачало.Value=Program.Настройки.ВремяНочнойПериодНачало;
            dTP_НочнойПериодКонец.Value=Program.Настройки.ВремяНочнойПериодКонец;

        }



        private void СчитатьНастройкиИзОкнаВФайл()
        {
            float НастройкаВремени;

            if (float.TryParse(tBВремяМеждуСообщениями.Text.Replace('.', ','), out НастройкаВремени))
                Program.Настройки.ЗадержкаМеждуЗвуковымиСообщениями = НастройкаВремени;
            if (float.TryParse(tBИнтОповещОбОтменеПоезда.Text.Replace('.', ','), out НастройкаВремени))
                Program.Настройки.ИнтервалМеждуОповещениемОбОтменеПоезда = НастройкаВремени;
            if (float.TryParse(tBИнтОповещОЗадержкеПрибытия.Text.Replace('.', ','), out НастройкаВремени))
                Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда = НастройкаВремени;
            if (float.TryParse(tBИнтОповещОЗадержкеОтправления.Text.Replace('.', ','), out НастройкаВремени))
                Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда = НастройкаВремени;
            if (float.TryParse(tBИнтОповещООтпрПоГотов.Text.Replace('.', ','), out НастройкаВремени))
                Program.Настройки.ИнтервалМеждуОповещениемООтправлениеПоГотовности = НастройкаВремени;

            if (float.TryParse(tBСамоеРанееВремяШаблона.Text.Replace('.', ','), out НастройкаВремени))
                Program.Настройки.ОповещениеСамогоРаннегоВремениШаблона = НастройкаВремени;


            Program.Настройки.АвтФормСообщНаПассажирскийПоезд = cBПассажирскийПоезд.Checked;
            Program.Настройки.АвтФормСообщНаСкорыйПоезд = cBСкорыйПоезд.Checked;
            Program.Настройки.АвтФормСообщНаСкоростнойПоезд = cBСкоростнойПоезд.Checked;
            Program.Настройки.АвтФормСообщНаПригородныйЭлектропоезд = cBПригЭлектропоезд.Checked;
            Program.Настройки.АвтФормСообщНаФирменный = cBФирменный.Checked;
            Program.Настройки.АвтФормСообщНаЛасточку = cBЛасточка.Checked;
            Program.Настройки.АвтФормСообщНаРЭКС = cBРЭКС.Checked;

            Program.Настройки.EngСообщНаПассажирскийПоезд = cBПассажирскийПоездEng.Checked;
            Program.Настройки.EngСообщНаСкорыйПоезд = cBСкорыйПоездEng.Checked;
            Program.Настройки.EngСообщНаСкоростнойПоезд = cBСкоростнойПоездEng.Checked;
            Program.Настройки.EngСообщНаПригородныйЭлектропоезд = cBПригЭлектропоездEng.Checked;
            Program.Настройки.EngСообщНаФирменный = cBФирменныйEng.Checked;
            Program.Настройки.EngСообщНаЛасточку = cBЛасточкаEng.Checked;
            Program.Настройки.EngСообщНаРЭКС = cBРЭКСEng.Checked;

            Program.Настройки.РазрешениеДобавленияЗаблокированныхПоездовВСписок = cBРазрешениеДобавленияЗаблокированныхПоездовВСписок.Checked;
            Program.Настройки.РазрешениеАвтообновленияРасписания = cbРазрешитьАвтоОбновлениеРасписания.Checked;
            Program.Настройки.ВремяАвтообновленияРасписания = dTP_Автообновление.Value;

            for (int i = 0; i < Program.Настройки.КаналыДальнегоСлед.Length; i++)
                Program.Настройки.КаналыДальнегоСлед[i]= _каналыДальнегоСлед[i].Checked;

            for (int i = 0; i < Program.Настройки.КаналыПригорода.Length; i++)
                Program.Настройки.КаналыПригорода[i] = _каналыПригорода[i].Checked;

            for (int i = 0; i < Program.Настройки.КаналыПлатформ.Length; i++)
                Program.Настройки.КаналыПлатформ[i] = _каналыПлатформ[i].Checked;

            for (int i = 0; i < Program.Настройки.КаналыКасс.Length; i++)
                Program.Настройки.КаналыКасс[i] = _каналыКасс[i].Checked;


            if (float.TryParse(tBКаналыПериодОтправкиПакетов.Text.Replace('.', ','), out НастройкаВремени))
                Program.Настройки.КаналыПериодОтправкиПакетов = НастройкаВремени;


            Program.Настройки.УровеньГромкостиДень = tBРегуляторГромкостиДень.Value;
            Program.Настройки.УровеньГромкостиНочь = tBРегуляторГромкостиНочь.Value;

            Program.Настройки.ВремяНочнойПериодНачало = dTP_НочнойПериодНачало.Value;
            Program.Настройки.ВремяНочнойПериодКонец = dTP_НочнойПериодКонец.Value;
        }



        public static void ЗагрузитьНастройки()
        {
            Program.Настройки.НастройкиЦветов = new Color[] { Color.Black, Color.LightGray, Color.Black, Color.LightBlue, Color.Black, Color.White, Color.Black, Color.Yellow, Color.Black, Color.LightGreen, Color.Black, Color.YellowGreen, Color.Black, Color.Orange, Color.Black, Color.DarkSalmon, Color.Black, Color.Black };

            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader("Settings.ini"))
                {
                    string line;
                    int  ПеременнаяInt;
                    float ПеременнаяFloat;
                    bool ПеременнаяBool;
                    DateTime переменнаяDateTime;

                    Program.Настройки.ВремяАвтообновленияРасписания= DateTime.Parse("00:00:00");

                    while ((line = file.ReadLine()) != null)
                    {
                        string[] Settings = line.Split('=');
                        if (Settings.Length == 2)
                        {
                            switch (Settings[0])
                            {
                                case "ЗадержкаМеждуЗвуковымиСообщениями":
                                    if (float.TryParse(Settings[1], out ПеременнаяFloat))
                                        Program.Настройки.ЗадержкаМеждуЗвуковымиСообщениями = ПеременнаяFloat;
                                    break;

                                case "ИнтервалМеждуОповещениемОбОтменеПоезда":
                                    if (float.TryParse(Settings[1], out ПеременнаяFloat))
                                        Program.Настройки.ИнтервалМеждуОповещениемОбОтменеПоезда = ПеременнаяFloat;
                                    break;

                                case "ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда":
                                    if (float.TryParse(Settings[1], out ПеременнаяFloat))
                                        Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда = ПеременнаяFloat;
                                    break;

                                case "ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда":
                                    if (float.TryParse(Settings[1], out ПеременнаяFloat))
                                        Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда = ПеременнаяFloat;
                                    break;

                                case "ИнтервалМеждуОповещениемОГотовностиОтправленияПоезда":
                                    if (float.TryParse(Settings[1], out ПеременнаяFloat))
                                        Program.Настройки.ИнтервалМеждуОповещениемООтправлениеПоГотовности = ПеременнаяFloat;
                                    break;

                                case "ОповещениеСамогоРаннегоВремениШаблона":
                                    if (float.TryParse(Settings[1], out ПеременнаяFloat))
                                        Program.Настройки.ОповещениеСамогоРаннегоВремениШаблона = ПеременнаяFloat;
                                    break;

                                case "АвтФормСообщНаПассажирскийПоезд":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.АвтФормСообщНаПассажирскийПоезд = ПеременнаяBool;
                                    break;

                                case "АвтФормСообщНаСкорыйПоезд":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.АвтФормСообщНаСкорыйПоезд = ПеременнаяBool;
                                    break;

                                case "АвтФормСообщНаСкоростнойПоезд":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.АвтФормСообщНаСкоростнойПоезд = ПеременнаяBool;
                                    break;

                                case "АвтФормСообщНаПригородныйЭлектропоезд":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.АвтФормСообщНаПригородныйЭлектропоезд = ПеременнаяBool;
                                    break;

                                case "АвтФормСообщНаФирменный":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.АвтФормСообщНаФирменный = ПеременнаяBool;
                                    break;

                                case "АвтФормСообщНаЛасточку":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.АвтФормСообщНаЛасточку = ПеременнаяBool;
                                    break;

                                case "АвтФормСообщНаРЭКС":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.АвтФормСообщНаРЭКС = ПеременнаяBool;
                                    break;


                                case "EngСообщНаПассажирскийПоезд":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.EngСообщНаПассажирскийПоезд = ПеременнаяBool;
                                    break;

                                case "EngСообщНаСкорыйПоезд":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.EngСообщНаСкорыйПоезд = ПеременнаяBool;
                                    break;

                                case "EngСообщНаСкоростнойПоезд":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.EngСообщНаСкоростнойПоезд = ПеременнаяBool;
                                    break;

                                case "EngСообщНаПригородныйЭлектропоезд":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.EngСообщНаПригородныйЭлектропоезд = ПеременнаяBool;
                                    break;

                                case "EngСообщНаФирменный":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.EngСообщНаФирменный = ПеременнаяBool;
                                    break;

                                case "EngСообщНаЛасточку":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.EngСообщНаЛасточку = ПеременнаяBool;
                                    break;

                                case "EngСообщНаРЭКС":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.EngСообщНаРЭКС = ПеременнаяBool;
                                    break;



                                case "РазрешениеДобавленияЗаблокированныхПоездовВСписок":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.РазрешениеДобавленияЗаблокированныхПоездовВСписок = ПеременнаяBool;
                                    break;

                                case "ЦветовыеНастройки":
                                    string[] ЦветовыеНастройки = Settings[1].Split(',');
                                    if (ЦветовыеНастройки.Length == 18)
                                        for (int i = 0; i < 18; i++)
                                            Program.Настройки.НастройкиЦветов[i] = ColorTranslator.FromHtml(ЦветовыеНастройки[i]);
                                    break;

                                case "РазрешениеАвтообновленияРасписания":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.РазрешениеАвтообновленияРасписания = ПеременнаяBool;
                                    break;

                                case "ВремяАвтообновленияРасписания":
                                    if (DateTime.TryParse(Settings[1],out переменнаяDateTime))
                                        Program.Настройки.ВремяАвтообновленияРасписания = переменнаяDateTime;
                                    break;

                                case "НастройкиКаналовДальнегоСлед":
                                    string[] настройкиКаналов = Settings[1].Split(',');
                                    Program.Настройки.КаналыДальнегоСлед= new bool[настройкиКаналов.Length];
                                    for (int i = 0; i < Program.Настройки.КаналыДальнегоСлед.Length; i++)
                                    {
                                        Program.Настройки.КаналыДальнегоСлед[i] = bool.Parse(настройкиКаналов[i]);
                                    }
                                    break;

                                case "НастройкиКаналовПригород":
                                    настройкиКаналов = Settings[1].Split(',');
                                    Program.Настройки.КаналыПригорода = new bool[настройкиКаналов.Length];
                                    for (int i = 0; i < Program.Настройки.КаналыПригорода.Length; i++)
                                    {
                                        Program.Настройки.КаналыПригорода[i] = bool.Parse(настройкиКаналов[i]);
                                    }
                                    break;

                                case "НастройкиКаналовПлатформ":
                                    настройкиКаналов = Settings[1].Split(',');
                                    Program.Настройки.КаналыПлатформ = new bool[настройкиКаналов.Length];
                                    for (int i = 0; i < Program.Настройки.КаналыПлатформ.Length; i++)
                                    {
                                        Program.Настройки.КаналыПлатформ[i] = bool.Parse(настройкиКаналов[i]);
                                    }
                                    break;

                                case "НастройкиКаналовКасс":
                                    настройкиКаналов = Settings[1].Split(',');
                                    Program.Настройки.КаналыКасс = new bool[настройкиКаналов.Length];
                                    for (int i = 0; i < Program.Настройки.КаналыКасс.Length; i++)
                                    {
                                        Program.Настройки.КаналыКасс[i] = bool.Parse(настройкиКаналов[i]);
                                    }
                                    break;

                                case "КаналыПериодОтправкиПакетов":
                                    if (float.TryParse(Settings[1], out ПеременнаяFloat))
                                        Program.Настройки.КаналыПериодОтправкиПакетов = ПеременнаяFloat;
                                    break;

                                case "УровеньГромкостиДень":
                                    if (int.TryParse(Settings[1], out ПеременнаяInt))
                                        Program.Настройки.УровеньГромкостиДень = ПеременнаяInt;
                                    break;

                                case "УровеньГромкостиНочь":
                                    if (int.TryParse(Settings[1], out ПеременнаяInt))
                                        Program.Настройки.УровеньГромкостиНочь = ПеременнаяInt;
                                    break;

                                case "ВремяНочнойПериодНачало":
                                    if (DateTime.TryParse(Settings[1], out переменнаяDateTime))
                                        Program.Настройки.ВремяНочнойПериодНачало = переменнаяDateTime;
                                    break;

                                case "ВремяНочнойПериодКонец":
                                    if (DateTime.TryParse(Settings[1], out переменнаяDateTime))
                                        Program.Настройки.ВремяНочнойПериодКонец = переменнаяDateTime;
                                    break;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.ЗаписьЛога("Системное сообщение", "Ошибка загрузки настроек: " + ex.Message);
            }

            if (Program.Настройки.ЗадержкаМеждуЗвуковымиСообщениями < 1)
                Program.Настройки.ЗадержкаМеждуЗвуковымиСообщениями = 1;
            if (Program.Настройки.ИнтервалМеждуОповещениемОбОтменеПоезда < 1)
                Program.Настройки.ИнтервалМеждуОповещениемОбОтменеПоезда = 1;
            if (Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда < 1)
                Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда = 1;
            if (Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда < 1)
                Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда = 1;
        }



        public static void СохранитьНастройки()
        {
            try
            {
                using (System.IO.StreamWriter DumpFile = new System.IO.StreamWriter("Settings.ini"))
                {
                    DumpFile.WriteLine("ЗадержкаМеждуЗвуковымиСообщениями=" + Program.Настройки.ЗадержкаМеждуЗвуковымиСообщениями.ToString("0.0"));
                    DumpFile.WriteLine("ИнтервалМеждуОповещениемОбОтменеПоезда=" + Program.Настройки.ИнтервалМеждуОповещениемОбОтменеПоезда.ToString("0.0"));
                    DumpFile.WriteLine("ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда=" + Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда.ToString("0.0"));
                    DumpFile.WriteLine("ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда=" + Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда.ToString("0.0"));
                    DumpFile.WriteLine("ИнтервалМеждуОповещениемОГотовностиОтправленияПоезда=" + Program.Настройки.ИнтервалМеждуОповещениемООтправлениеПоГотовности.ToString("0.0"));

                    DumpFile.WriteLine("АвтФормСообщНаПассажирскийПоезд=" + Program.Настройки.АвтФормСообщНаПассажирскийПоезд.ToString());
                    DumpFile.WriteLine("АвтФормСообщНаСкорыйПоезд=" + Program.Настройки.АвтФормСообщНаСкорыйПоезд.ToString());
                    DumpFile.WriteLine("АвтФормСообщНаСкоростнойПоезд=" + Program.Настройки.АвтФормСообщНаСкоростнойПоезд.ToString());
                    DumpFile.WriteLine("АвтФормСообщНаПригородныйЭлектропоезд=" + Program.Настройки.АвтФормСообщНаПригородныйЭлектропоезд.ToString());
                    DumpFile.WriteLine("АвтФормСообщНаФирменный=" + Program.Настройки.АвтФормСообщНаФирменный.ToString());
                    DumpFile.WriteLine("АвтФормСообщНаЛасточку=" + Program.Настройки.АвтФормСообщНаЛасточку.ToString());
                    DumpFile.WriteLine("АвтФормСообщНаРЭКС=" + Program.Настройки.АвтФормСообщНаРЭКС.ToString());

                    DumpFile.WriteLine("EngСообщНаПассажирскийПоезд=" + Program.Настройки.EngСообщНаПассажирскийПоезд.ToString());
                    DumpFile.WriteLine("EngСообщНаСкорыйПоезд=" + Program.Настройки.EngСообщНаСкорыйПоезд.ToString());
                    DumpFile.WriteLine("EngСообщНаСкоростнойПоезд=" + Program.Настройки.EngСообщНаСкоростнойПоезд.ToString());
                    DumpFile.WriteLine("EngСообщНаПригородныйЭлектропоезд=" + Program.Настройки.EngСообщНаПригородныйЭлектропоезд.ToString());
                    DumpFile.WriteLine("EngСообщНаФирменный=" + Program.Настройки.EngСообщНаФирменный.ToString());
                    DumpFile.WriteLine("EngСообщНаЛасточку=" + Program.Настройки.EngСообщНаЛасточку.ToString());
                    DumpFile.WriteLine("EngСообщНаРЭКС=" + Program.Настройки.EngСообщНаРЭКС.ToString());

                    DumpFile.WriteLine("РазрешениеДобавленияЗаблокированныхПоездовВСписок=" + Program.Настройки.РазрешениеДобавленияЗаблокированныхПоездовВСписок.ToString());
                    DumpFile.WriteLine("РазрешениеАвтообновленияРасписания=" + Program.Настройки.РазрешениеАвтообновленияРасписания.ToString());
                    DumpFile.WriteLine("ВремяАвтообновленияРасписания=" + Program.Настройки.ВремяАвтообновленияРасписания.ToString("t"));

                    DumpFile.WriteLine("ОповещениеСамогоРаннегоВремениШаблона=" + Program.Настройки.ОповещениеСамогоРаннегоВремениШаблона.ToString("0.0"));

                    string ЦветовыеНастройки = "";
                    for (int i = 0; i < 18; i++)
                        ЦветовыеНастройки += ColorTranslator.ToHtml(Program.Настройки.НастройкиЦветов[i]) + ",";
                    if (ЦветовыеНастройки[ЦветовыеНастройки.Length - 1] == ',')
                        ЦветовыеНастройки = ЦветовыеНастройки.Remove(ЦветовыеНастройки.Length - 1, 1);
                    DumpFile.WriteLine("ЦветовыеНастройки=" + ЦветовыеНастройки);


                    string настройкиКаналов = Program.Настройки.КаналыДальнегоСлед.Aggregate("", (current, t) => current + (t + ","));
                    if (настройкиКаналов[настройкиКаналов.Length - 1] == ',')
                        настройкиКаналов = настройкиКаналов.Remove(настройкиКаналов.Length - 1, 1);

                    DumpFile.WriteLine("НастройкиКаналовДальнегоСлед=" + настройкиКаналов);


                    настройкиКаналов = Program.Настройки.КаналыПригорода.Aggregate("", (current, t) => current + (t + ","));
                    if (настройкиКаналов[настройкиКаналов.Length - 1] == ',')
                        настройкиКаналов = настройкиКаналов.Remove(настройкиКаналов.Length - 1, 1);

                    DumpFile.WriteLine("НастройкиКаналовПригород=" + настройкиКаналов);


                    настройкиКаналов = Program.Настройки.КаналыПлатформ.Aggregate("", (current, t) => current + (t + ","));
                    if (настройкиКаналов[настройкиКаналов.Length - 1] == ',')
                        настройкиКаналов = настройкиКаналов.Remove(настройкиКаналов.Length - 1, 1);

                    DumpFile.WriteLine("НастройкиКаналовПлатформ=" + настройкиКаналов);


                    настройкиКаналов = Program.Настройки.КаналыКасс.Aggregate("", (current, t) => current + (t + ","));
                    if (настройкиКаналов[настройкиКаналов.Length - 1] == ',')
                        настройкиКаналов = настройкиКаналов.Remove(настройкиКаналов.Length - 1, 1);

                    DumpFile.WriteLine("НастройкиКаналовКасс=" + настройкиКаналов);


                    DumpFile.WriteLine("КаналыПериодОтправкиПакетов=" + Program.Настройки.КаналыПериодОтправкиПакетов.ToString("0.0"));

                    DumpFile.WriteLine("УровеньГромкостиДень=" + Program.Настройки.УровеньГромкостиДень);
                    DumpFile.WriteLine("УровеньГромкостиНочь=" + Program.Настройки.УровеньГромкостиНочь);
                    DumpFile.WriteLine("ВремяНочнойПериодНачало=" + Program.Настройки.ВремяНочнойПериодНачало.ToString("t"));
                    DumpFile.WriteLine("ВремяНочнойПериодКонец=" + Program.Настройки.ВремяНочнойПериодКонец.ToString("t"));

                    DumpFile.Close();
                }
            }
            catch (Exception ex)
            {
                Program.ЗаписьЛога("Системное сообщение", "Ошибка сохранения настроек: " + ex.Message);
            }
        }



        private void pCol1_DoubleClick(object sender, EventArgs e)
        {
            int НомерПанели = int.Parse((string)(sender as Panel).Tag);

            colorDialog1.Color = Program.Настройки.НастройкиЦветов[НомерПанели];
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Program.Настройки.НастройкиЦветов[НомерПанели] = colorDialog1.Color;
                (sender as Panel).BackColor = Program.Настройки.НастройкиЦветов[НомерПанели];
            }
        }



        private void tBРегуляторГромкостиДень_Scroll(object sender, EventArgs e)
        {
            //Громкость сохраняется сразу.
            Program.Настройки.УровеньГромкостиДень = tBРегуляторГромкостиДень.Value; 
            lbl_громкостьДень.Text = ПреобразрватьУровеньГромкостиВПроценты(Program.Настройки.УровеньГромкостиДень).ToString("F") + @"%";

            var уровеньГромкости= Program.Настройки.ВыборУровняГромкости();
            Player.SetVolume(уровеньГромкости);
        }



        private void tBРегуляторГромкостиНочь_Scroll(object sender, EventArgs e)
        {
            //Громкость сохраняется сразу.
            Program.Настройки.УровеньГромкостиНочь = tBРегуляторГромкостиНочь.Value;
            lbl_громкостьНочь.Text = ПреобразрватьУровеньГромкостиВПроценты(Program.Настройки.УровеньГромкостиНочь).ToString("F") + @"%";

            var уровеньГромкости = Program.Настройки.ВыборУровняГромкости();
            Player.SetVolume(уровеньГромкости);
        }


        private double ПреобразрватьУровеньГромкостиВПроценты(int volume)
        {
            double maxScale = 7000.0;
            double scale = maxScale + volume;

            var percent= (scale * 100.0) / maxScale;
            return percent;
        }


        private void txtb_Пригород_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var fontDialog = new FontDialog { Font = Program.Настройки.FontПригород };
            if (fontDialog.ShowDialog() != DialogResult.Cancel)
            {
                Program.Настройки.FontПригород = fontDialog.Font;
                txtb_Пригород.Text = $@"{Program.Настройки.FontПригород.Name} {Program.Настройки.FontПригород.Size}";
            }
        }


        private void txtb_Дальние_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var fontDialog = new FontDialog { Font = Program.Настройки.FontДальние };
            if (fontDialog.ShowDialog() != DialogResult.Cancel)
            {
                Program.Настройки.FontДальние = fontDialog.Font;
                txtb_Дальние.Text = $@"{Program.Настройки.FontДальние.Name} {Program.Настройки.FontДальние.Size}";
            }
        }
    }
}
