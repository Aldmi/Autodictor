using System;
using System.Drawing;
using System.Windows.Forms;





namespace MainExample
{
    public partial class Расписание : Form
    {
        private byte ИндексТекущегоМесяца = 0;
        private byte ПредыдущийИндексТекущегоМесяца = 0;
        private ПланРасписанияПоезда РасписаниеПоезда;
        private DateTime ВремяПоследнегоНажатияКлавиатуры = DateTime.Now;
        private bool ПризнакИзмененияСтроки = false;
        private РежимРасписанияДвиженияПоезда СтарыйРежимРасписания = РежимРасписанияДвиженияПоезда.Отсутствует;
        private bool ИнициализацияЗавершена = false;

        string[] Месяцы = new string[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь", "Январь", "Февраль" };
        byte[] КоличествоДнейВМесяце = new byte[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31, 31, 28 };



        public Расписание(ПланРасписанияПоезда РасписаниеПоезда)
        {
            InitializeComponent();

            this.РасписаниеПоезда = РасписаниеПоезда;

            for (int i = 0; i < 14; i++)
            {
                string[] ШаблонСтроки = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
                    "", "", "", "", "", "", "", "", "", "", "", "" };

                ШаблонСтроки[0] = Месяцы[i];
                byte КоличествоДнейВТекущемМесяце = ((i == 1) && ((DateTime.Now.Year % 4) == 0)) || ((i == 13) && (((DateTime.Now.Year + 1) % 4) == 0)) ? (byte)29 : КоличествоДнейВМесяце[i];

                DateTime НачалоМесяца = new DateTime(DateTime.Now.Year + (i / 12), (i % 12) + 1, 1);
                for (byte j = 1; j <= КоличествоДнейВТекущемМесяце; j++)
                    ШаблонСтроки[j + ((byte)НачалоМесяца.DayOfWeek + 6) % 7] = j.ToString();

                ListViewItem item = new ListViewItem(ШаблонСтроки, 0);
                item.UseItemStyleForSubItems = false;

                for (byte j = 1; j <= КоличествоДнейВТекущемМесяце; j++)
                    item.SubItems[j + ((byte)НачалоМесяца.DayOfWeek + 6) % 7].Tag = j + ((byte)НачалоМесяца.DayOfWeek + 6) % 7;

                
                listView1.Items.Add(item);
            }

            this.Text = "Расписание движения поезда: " + РасписаниеПоезда.ПолучитьНомерПоезда() + " - " + РасписаниеПоезда.ПолучитьНазваниеПоезда();

            byte АктивностьДняДвижения = РасписаниеПоезда.ПолучитьАктивностьПоездаВКрайниеДни();
            if ((АктивностьДняДвижения & 0x80) != 0x00) cBНаГрМес.Checked = true;
            if ((АктивностьДняДвижения & 0x01) != 0x00) cBГр31.Checked = true;
            if ((АктивностьДняДвижения & 0x02) != 0x00) cBГр1.Checked = true;
            if ((АктивностьДняДвижения & 0x04) != 0x00) cBГр2.Checked = true;

            UInt16 ДниНедели = РасписаниеПоезда.ПолучитьАктивностьПоДнямНедели();
            if ((ДниНедели & 0x0001) != 0x0000) cBПн.Checked = true;
            if ((ДниНедели & 0x0002) != 0x0000) cBВт.Checked = true;
            if ((ДниНедели & 0x0004) != 0x0000) cBСр.Checked = true;
            if ((ДниНедели & 0x0008) != 0x0000) cBЧт.Checked = true;
            if ((ДниНедели & 0x0010) != 0x0000) cBПт.Checked = true;
            if ((ДниНедели & 0x0020) != 0x0000) cBСб.Checked = true;
            if ((ДниНедели & 0x0040) != 0x0000) cBВск.Checked = true;
            if ((ДниНедели & 0x0100) != 0x0000) cBКрПн.Checked = true;
            if ((ДниНедели & 0x0200) != 0x0000) cBКрВт.Checked = true;
            if ((ДниНедели & 0x0400) != 0x0000) cBКрСр.Checked = true;
            if ((ДниНедели & 0x0800) != 0x0000) cBКрЧт.Checked = true;
            if ((ДниНедели & 0x1000) != 0x0000) cBКрПт.Checked = true;
            if ((ДниНедели & 0x2000) != 0x0000) cBКрСб.Checked = true;
            if ((ДниНедели & 0x4000) != 0x0000) cBКрВск.Checked = true;

            ЗадатьКодПереключателейРежима(РасписаниеПоезда.ПолучитьРежимРасписания());

            ИнициализацияЗавершена = true;

            ОбновитьЦветовуюМаркировкуРасписания();
        }

        public void УстановитьВремяДействия(string ВремяДействия)
        {
            lblВремяДействия.Text = ВремяДействия;
        }

        public ПланРасписанияПоезда ПолучитьПланРасписанияПоезда()
        {
            return РасписаниеПоезда;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void РежимРасписания_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked == false)
                return;

            bool[] МассивАктивныхМесяцев = new bool[14];
            bool РаботаПоВыбраннымМесяцам = false;

            CheckBox[] МассивЭлементовАктивныхМесяцев = new CheckBox[14] { cBЯнв, cBФев, cBМар, cBАпр, cBМай, cBИюн, cBИюл, cBАвг, cBСен, cBОкт, cBНоя, cBДек, cBЯнв, cBФев };


            for (int i = 0; i < 14; i++)
                if (cBРаспространитьНаВсе.Checked || МассивЭлементовАктивныхМесяцев[i].Checked)
                    МассивАктивныхМесяцев[i] = РаботаПоВыбраннымМесяцам = true;

            if (РаботаПоВыбраннымМесяцам == false)
                МассивАктивныхМесяцев[ИндексТекущегоМесяца] = true;


            for (byte ИндексМесяца = 0; ИндексМесяца < 14; ИндексМесяца++)
            {
                if (МассивАктивныхМесяцев[ИндексМесяца] == false)
                    continue;

                РежимРасписанияДвиженияПоезда новыйРежимРасписания = ПолучитьКодПереключателейРежима();

               // if (СтарыйРежимРасписания != новыйРежимРасписания)
                {
                    switch (новыйРежимРасписания)
                    {
                        case РежимРасписанияДвиженияПоезда.Отсутствует:
                            for (byte i = 0; i < 32; i++)
                                РасписаниеПоезда.ЗадатьАктивностьДняДвижения(ИндексМесяца, i, false);
                            break;

                        case РежимРасписанияДвиженияПоезда.Ежедневно:
                            for (byte i = 0; i < 32; i++)
                                РасписаниеПоезда.ЗадатьАктивностьДняДвижения(ИндексМесяца, i, true);
                            break;

                        case РежимРасписанияДвиженияПоезда.ПоЧетным:
                            for (byte i = 0; i < 32; i++)
                                РасписаниеПоезда.ЗадатьАктивностьДняДвижения(ИндексМесяца, i, ((i % 2) == 1) ? true : false);
                            break;

                        case РежимРасписанияДвиженияПоезда.ПоНечетным:
                            for (byte i = 0; i < 32; i++)
                                РасписаниеПоезда.ЗадатьАктивностьДняДвижения(ИндексМесяца, i, ((i % 2) == 0) ? true : false);
                            break;

                        case РежимРасписанияДвиженияПоезда.Выборочно:
                            break;

                        case РежимРасписанияДвиженияПоезда.ПоДням:
                            ЗадатьАктивностьРасписанияПоДням();
                            break;

                    }

                    if (cBНаГрМес.Checked == true)
                    {
                        РасписаниеПоезда.ЗадатьАктивностьДняДвижения(ИндексМесяца, 0, cBГр1.Checked);
                        РасписаниеПоезда.ЗадатьАктивностьДняДвижения(ИндексМесяца, 1, cBГр2.Checked);
                        РасписаниеПоезда.ЗадатьАктивностьДняДвижения(ИндексМесяца, 30, cBГр31.Checked);
                    }

                    РасписаниеПоезда.ЗадатьРежимРасписания(новыйРежимРасписания);
                    СтарыйРежимРасписания = новыйРежимРасписания;
                }
            }

            ОбновитьЦветовуюМаркировкуРасписания();
        }

        private РежимРасписанияДвиженияПоезда ПолучитьКодПереключателейРежима()
        {
            if (radioButton1.Checked == true)
                return РежимРасписанияДвиженияПоезда.Ежедневно;
            else if (radioButton2.Checked == true)
                return РежимРасписанияДвиженияПоезда.ПоЧетным;
            else if (radioButton3.Checked == true)
                return РежимРасписанияДвиженияПоезда.ПоНечетным;
            else if (radioButton4.Checked == true)
                return РежимРасписанияДвиженияПоезда.Выборочно;
            else if (radioButton6.Checked == true)
                return РежимРасписанияДвиженияПоезда.ПоДням;

            return РежимРасписанияДвиженияПоезда.Отсутствует;
        }

        private void ЗадатьКодПереключателейРежима(РежимРасписанияДвиженияПоезда КодПереключателейРежима)
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;
            radioButton6.Checked = false;

            if (КодПереключателейРежима == РежимРасписанияДвиженияПоезда.Отсутствует)
                radioButton5.Checked = true;
            else if (КодПереключателейРежима == РежимРасписанияДвиженияПоезда.Ежедневно)
                radioButton1.Checked = true;
            else if (КодПереключателейРежима == РежимРасписанияДвиженияПоезда.ПоЧетным)
                radioButton2.Checked = true;
            else if (КодПереключателейРежима == РежимРасписанияДвиженияПоезда.ПоНечетным)
                radioButton3.Checked = true;
            else if (КодПереключателейРежима == РежимРасписанияДвиженияПоезда.Выборочно)
                radioButton4.Checked = true;
            else if (КодПереключателейРежима == РежимРасписанияДвиженияПоезда.ПоДням)
                radioButton6.Checked = true;
        }

        private void ОбновитьЦветовуюМаркировкуРасписания()
        {
            for (byte i = 0; i < 14; i++)
            {
                string[] ШаблонСтроки = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
                    "", "", "", "", "", "", "", "", "", "", "", "" };

                byte КоличествоДнейВТекущемМесяце = ((i == 1) && ((DateTime.Now.Year % 4) == 0)) || ((i == 13) && (((DateTime.Now.Year + 1) % 4) == 0)) ? (byte)29 : КоличествоДнейВМесяце[i];

                DateTime НачалоМесяца = new DateTime(DateTime.Now.Year + (i / 12), (i % 12) + 1, 1);
                for (byte j = 1; j <= КоличествоДнейВТекущемМесяце; j++)
                {
                    byte НомерСтолбца = (byte)(j + ((byte)НачалоМесяца.DayOfWeek + 6) % 7);
                    bool АктивностьВТекущийДень = РасписаниеПоезда.ПолучитьАктивностьДняДвижения((byte)i, (byte)(j - 1));

                    ListViewItem.ListViewSubItem SubItem = listView1.Items[i].SubItems[НомерСтолбца];
                    SubItem.BackColor = АктивностьВТекущийДень ? Color.LightGreen : Color.White;
                    SubItem.ForeColor = ((НомерСтолбца % 7) == 6) || ((НомерСтолбца % 7) == 0) ? Color.Red : Color.Black;
                    listView1.Items[i].SubItems[НомерСтолбца] = SubItem;
                }
            }
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Y >= 24)
            {
                int Строка = (e.Y - 24) / 24;

                if (Строка < 14)
                {
                    lblВыбранМесяц.Text = Месяцы[Строка];
                    ИндексТекущегоМесяца = (byte)Строка;
                    РежимРасписанияДвиженияПоезда РежимРасписанияПоезда = РасписаниеПоезда.ПолучитьРежимРасписания();
                    ЗадатьКодПереключателейРежима(РежимРасписанияПоезда);

                    if (ПредыдущийИндексТекущегоМесяца != ИндексТекущегоМесяца)
                    {
                        ПредыдущийИндексТекущегоМесяца = ИндексТекущегоМесяца;
                        ПризнакИзмененияСтроки = true;
                    }
                    else
                        ПризнакИзмененияСтроки = false;

                    if ((ПризнакИзмененияСтроки == false) && ((DateTime.Now - ВремяПоследнегоНажатияКлавиатуры).Seconds < 1) && (РежимРасписанияПоезда == РежимРасписанияДвиженияПоезда.Выборочно))
                    {
                        ListViewHitTestInfo lvhti;
                        lvhti = listView1.HitTest(new Point(e.X, e.Y));
                        ListViewItem.ListViewSubItem my4 = lvhti.SubItem;

                        int Число;
                        if (int.TryParse(my4.Text, out Число) == true)
                        {
                            int Tag = (int)my4.Tag;

                            if ((Tag >= 1) && (Tag <= 43) && (Число > 0) && (Число < 32))
                            {
                                bool ТекущаяАктивность = РасписаниеПоезда.ПолучитьАктивностьДняДвижения(ИндексТекущегоМесяца, (byte)(Число - 1));
                                РасписаниеПоезда.ЗадатьАктивностьДняДвижения(ИндексТекущегоМесяца, (byte)(Число - 1), !ТекущаяАктивность);
                                ОбновитьЦветовуюМаркировкуРасписания();
                            }
                        }
                    }

                    ВремяПоследнегоНажатияКлавиатуры = DateTime.Now;
                }
            }            
        }

        private void ИзмененоПоДням(object sender, EventArgs e)
        {
            if (ИнициализацияЗавершена == false)
                return; 

            CheckBox cb = sender as CheckBox;

            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;
            radioButton6.Checked = true;

            if (cb.Checked == true)
                switch (cb.Name)
                {
                    case "cBПн": 
                    case "cBВт":
                    case "cBСр":
                    case "cBЧт":
                    case "cBПт":
                    case "cBСб":
                    case "cBВск":
                        cBКрПн.Checked = false;
                        cBКрВт.Checked = false;
                        cBКрСр.Checked = false;
                        cBКрЧт.Checked = false;
                        cBКрПт.Checked = false;
                        cBКрСб.Checked = false;
                        cBКрВск.Checked = false;
                        cBКрВ.Checked = false;
                        break;

                    case "cBКрПн":
                    case "cBКрВт":
                    case "cBКрСр":
                    case "cBКрЧт":
                    case "cBКрПт":
                    case "cBКрСб":
                    case "cBКрВск":
                        cBПн.Checked = false;
                        cBВт.Checked = false;
                        cBСр.Checked = false;
                        cBЧт.Checked = false;
                        cBПт.Checked = false;
                        cBСб.Checked = false;
                        cBВск.Checked = false;
                        cBВ.Checked = false;
                        break;
                }

            if ((cBСб.Checked == true) && (cBВск.Checked == true))
                cBВ.Checked = true;

            if ((cBКрСб.Checked == true) && (cBКрВск.Checked == true))
                cBКрВ.Checked = true;

            ЗадатьАктивностьРасписанияПоДням();
        }

        private void ЗадатьАктивностьРасписанияПоДням()
        {
            if (ИнициализацияЗавершена == false)
                return;

            bool[] МассивАктивныхДней = new bool[32];
            bool[] МассивАктивныхМесяцев = new bool[14];
            bool РаботаПоВыбраннымМесяцам = false;

            CheckBox[] МассивЭлементовАктивныхМесяцев = new CheckBox[14] { cBЯнв, cBФев, cBМар, cBАпр, cBМай, cBИюн, cBИюл, cBАвг, cBСен, cBОкт, cBНоя, cBДек, cBЯнв, cBФев };


            for (int i = 0; i < 14; i++)
                if (cBРаспространитьНаВсе.Checked || МассивЭлементовАктивныхМесяцев[i].Checked)
                    МассивАктивныхМесяцев[i] = РаботаПоВыбраннымМесяцам = true;

            if (РаботаПоВыбраннымМесяцам == false)
                МассивАктивныхМесяцев[ИндексТекущегоМесяца] = true;


            byte ПоДням = 0x00;
            if (cBПн.Checked) ПоДням |= 0x01;
            if (cBВт.Checked) ПоДням |= 0x02;
            if (cBСр.Checked) ПоДням |= 0x04;
            if (cBЧт.Checked) ПоДням |= 0x08;
            if (cBПт.Checked) ПоДням |= 0x10;
            if (cBСб.Checked) ПоДням |= 0x20;
            if (cBВск.Checked) ПоДням |= 0x40;
            if (cBВ.Checked) ПоДням |= 0x60;

            byte КромеДней = 0x00;
            if (cBКрПн.Checked) КромеДней |= 0x01;
            if (cBКрВт.Checked) КромеДней |= 0x02;
            if (cBКрСр.Checked) КромеДней |= 0x04;
            if (cBКрЧт.Checked) КромеДней |= 0x08;
            if (cBКрПт.Checked) КромеДней |= 0x10;
            if (cBКрСб.Checked) КромеДней |= 0x20;
            if (cBКрВск.Checked) КромеДней |= 0x40;
            if (cBКрВ.Checked) КромеДней |= 0x60;

            РасписаниеПоезда.ЗадатьАктивностьПоДнямНедели((ushort)((КромеДней << 8) | ПоДням));
            РасписаниеПоезда.ЗадатьРежимРасписания(РежимРасписанияДвиженияПоезда.ПоДням);

            for (int i = 0; i < 14; i++)
            {
                if (МассивАктивныхМесяцев[i])
                {
                    byte НомерПервогоДня = 0;
                    byte НомерПоследнегоДня = 30;

                    for (int j = 0; j < 32; j++)
                        МассивАктивныхДней[j] = false;

                    DateTime date = new DateTime(DateTime.Now.Year + (i / 12), (i % 12) + 1, 1);
                    byte DayOfWeek = (byte)((byte)date.DayOfWeek + 6);

                    if (cBНаГрМес.Checked == true)
                    {
                        МассивАктивныхДней[0] = cBГр1.Checked;
                        МассивАктивныхДней[1] = cBГр2.Checked;
                        МассивАктивныхДней[30] = cBГр31.Checked;
                        НомерПервогоДня = 2;
                        НомерПоследнегоДня = 29;
                    }

                    if (ПоДням != 0x00)
                    {
                        for (int j = НомерПервогоДня; j <= НомерПоследнегоДня; j++)
                            if ((ПоДням & (0x01 << ((j + DayOfWeek) % 7))) != 0x00)
                                МассивАктивныхДней[j] = true;
                    }
                    else if (КромеДней != 0x00)
                    {
                        for (int j = НомерПервогоДня; j <= НомерПоследнегоДня; j++)
                            if ((КромеДней & (0x01 << ((j + DayOfWeek) % 7))) == 0x00)
                                МассивАктивныхДней[j] = true;
                    }


                    for (byte j = 0; j < 31; j++)
                        РасписаниеПоезда.ЗадатьАктивностьДняДвижения((byte)i, j, МассивАктивныхДней[j]);
                }
            }

            ОбновитьЦветовуюМаркировкуРасписания();
        }

        private void ИзмененоНаГраницеМесяца(object sender, EventArgs e)
        {
            byte АктивностьРасписанияВКрайниеДни = 0x00;
            if (cBНаГрМес.Checked) АктивностьРасписанияВКрайниеДни |= 0x80;
            if (cBГр31.Checked) АктивностьРасписанияВКрайниеДни |= 0x01;
            if (cBГр1.Checked) АктивностьРасписанияВКрайниеДни |= 0x02;
            if (cBГр2.Checked) АктивностьРасписанияВКрайниеДни |= 0x04;
            РасписаниеПоезда.УстановитьАктивностьПоездаВКрайниеДни(АктивностьРасписанияВКрайниеДни);

            if (radioButton6.Checked)
                ЗадатьАктивностьРасписанияПоДням();
        }
    }
}
