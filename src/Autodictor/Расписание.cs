using System;
using System.Drawing;
using System.Windows.Forms;





namespace MainExample
{
    public partial class Расписание : Form
    {
        private byte ИндексТекущегоМесяца = 15;
        private byte ПредыдущийИндексТекущегоМесяца = 16;
        private ПланРасписанияПоезда РасписаниеПоезда;
        private DateTime ВремяПоследнегоНажатияКлавиатуры = DateTime.Now;
        private bool ПризнакИзмененияСтроки = false;
        private РежимРасписанияДвиженияПоезда СтарыйРежимРасписания = РежимРасписанияДвиженияПоезда.Отсутствует;



        public Расписание(ПланРасписанияПоезда РасписаниеПоезда)
        {
            InitializeComponent();

            this.РасписаниеПоезда = РасписаниеПоезда;

            string[] Месяцы = new string[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
            byte[] КоличествоДнейВМесяце = new byte[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            for (int i = 0; i < 12; i++)
            {
                string[] ШаблонСтроки = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
                    "", "", "", "", "", "", "", "", "", "", "", "" };

                ШаблонСтроки[0] = Месяцы[i];
                byte КоличествоДнейВТекущемМесяце = (i == 1) && ((DateTime.Now.Year % 4) == 0) ? (byte)29 : КоличествоДнейВМесяце[i];

                DateTime НачалоМесяца = new DateTime(DateTime.Now.Year, i + 1, 1);
                for (byte j = 1; j <= КоличествоДнейВТекущемМесяце; j++)
                    ШаблонСтроки[j + ((byte)НачалоМесяца.DayOfWeek + 6) % 7] = j.ToString();

                ListViewItem item = new ListViewItem(ШаблонСтроки, 0);
                item.UseItemStyleForSubItems = false;

                for (byte j = 1; j <= КоличествоДнейВТекущемМесяце; j++)
                    item.SubItems[j + ((byte)НачалоМесяца.DayOfWeek + 6) % 7].Tag = j + ((byte)НачалоМесяца.DayOfWeek + 6) % 7;

                
                listView1.Items.Add(item);
            }

            this.Text = "Расписание движения поезда: " + РасписаниеПоезда.ПолучитьНомерПоезда() + " - " + РасписаниеПоезда.ПолучитьНазваниеПоезда();
            ОбновитьЦветовуюМаркировкуРасписания();
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


            РежимРасписанияДвиженияПоезда новыйРежимРасписания = ПолучитьКодПереключателейРежима();

            if ((ИндексТекущегоМесяца < 12) && (СтарыйРежимРасписания != новыйРежимРасписания))
            {
                switch (новыйРежимРасписания)
                {
                    case РежимРасписанияДвиженияПоезда.Отсутствует:
                        for (byte i = 0; i < 32; i++)
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения(ИндексТекущегоМесяца, i, false);
                        break;

                    case РежимРасписанияДвиженияПоезда.Ежедневно:
                        for (byte i = 0; i < 32; i++)
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения(ИндексТекущегоМесяца, i, true);
                        break;

                    case РежимРасписанияДвиженияПоезда.ПоЧетным:
                        for (byte i = 0; i < 32; i++)
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения(ИндексТекущегоМесяца, i, ((i % 2) == 0) ? true : false);
                        break;

                    case РежимРасписанияДвиженияПоезда.ПоНечетным:
                        for (byte i = 0; i < 32; i++)
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения(ИндексТекущегоМесяца, i, ((i % 2) == 1) ? true : false);
                        break;

                    case РежимРасписанияДвиженияПоезда.Выборочно:
                        break;

                    case РежимРасписанияДвиженияПоезда.ЕжедневноКромеВыходных:
                        DateTime ТестовоеВремя = new DateTime(DateTime.Now.Year, ИндексТекущегоМесяца + 1, 1);
                        int ИндексДняНедели = (int)ТестовоеВремя.DayOfWeek;
                        
                        for (byte i = 0; i < 32; i++)
                            РасписаниеПоезда.ЗадатьАктивностьДняДвижения(ИндексТекущегоМесяца, i, ((((ИндексДняНедели + i) % 7) == 0) || ((((ИндексДняНедели + i) % 7)) == 6)) ? false : true);
                        break;

                }

                РасписаниеПоезда.ЗадатьРежимРасписания(ИндексТекущегоМесяца, новыйРежимРасписания);
                СтарыйРежимРасписания = новыйРежимРасписания;
                ОбновитьЦветовуюМаркировкуРасписания();
            }
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
                return РежимРасписанияДвиженияПоезда.ЕжедневноКромеВыходных;

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
            else if (КодПереключателейРежима == РежимРасписанияДвиженияПоезда.ЕжедневноКромеВыходных)
                radioButton6.Checked = true;
        }

        private void ОбновитьЦветовуюМаркировкуРасписания()
        {
            byte[] КоличествоДнейВМесяце = new byte[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            for (byte i = 0; i < 12; i++)
            {
                string[] ШаблонСтроки = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
                    "", "", "", "", "", "", "", "", "", "", "", "" };

                byte КоличествоДнейВТекущемМесяце = (i == 1) && ((DateTime.Now.Year % 4) == 0) ? (byte)29 : КоличествоДнейВМесяце[i];

                DateTime НачалоМесяца = new DateTime(DateTime.Now.Year, i + 1, 1);
                for (byte j = 1; j <= КоличествоДнейВТекущемМесяце; j++)
                {
                    byte НомерСтолбца = (byte)(j + ((byte)НачалоМесяца.DayOfWeek + 6) % 7);
                    bool АктивностьВТекущийДень = РасписаниеПоезда.ПолучитьАктивностьДняДвижения(i, (byte)(j - 1));

                    ListViewItem.ListViewSubItem SubItem = listView1.Items[i].SubItems[НомерСтолбца];
                    SubItem.BackColor = АктивностьВТекущийДень ? Color.LightGreen : Color.White;
                    SubItem.ForeColor = ((НомерСтолбца % 7) == 6) || ((НомерСтолбца % 7) == 0) ? Color.Red : Color.Black;
                    listView1.Items[i].SubItems[НомерСтолбца] = SubItem;
                }
            }

        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Y >= 0x0018)
            {
                int Строка = (e.Y - 0x0018) / 0x0011;
                if (Строка < 12)
                {
                    ИндексТекущегоМесяца = (byte)Строка;
                    РежимРасписанияДвиженияПоезда РежимРасписанияПоезда = РасписаниеПоезда.ПолучитьРежимРасписания(ИндексТекущегоМесяца);
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
    }
}
