using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace MainExample
{
    public partial class ОкноРасписания2 : Form
    {
        public int СчетчикСтроки = 0;

        public Font drawFont = new Font("Arial", 8, FontStyle.Bold);
        public SolidBrush drawBrushWhite = new SolidBrush(Color.White);
        public SolidBrush drawBrushRed = new SolidBrush(Color.DarkRed);
        public SolidBrush drawBrushBlue = new SolidBrush(Color.Black);
        public bool ОбновитьЧасы = false;
        public bool ОбноыитьРасписание = false;
        public bool ОбновитьСтрокуПрокрутки = false;
        private DateTime ВремяПоследнегоОбновления = DateTime.Now;
        private DateTime ВремяПоследнегоОбновленияРасписания = DateTime.Now;
        private int НомерСтраницыПоездовДальнегоСледования = 0;
        private int НомерСтраницыЭлектропоездов = 0;

        const int КоличествоСтрокДляПоездовДальнегоСледования = 6;
        const int КоличествоСтрокДляЭлектропоездов = 4;

        public struct СтрокаВРасписаниии
        {
            public string НомерПоезда;
            public string НазваниеПоезда;
            public string ВремяПрибытия;
            public string ВремяСтоянки;
            public string ВремяОтправления;
            public string Примечание;
        }

        static public SortedDictionary<string, СтрокаВРасписаниии> РасписаниеПоездов = new SortedDictionary<string, СтрокаВРасписаниии>();
        static public SortedDictionary<string, СтрокаВРасписаниии> РасписаниеЭлектричек = new SortedDictionary<string, СтрокаВРасписаниии>();



        public ОкноРасписания2()
        {
            InitializeComponent();
            ОбновитьЧасы = true;
            ОбноыитьРасписание = true;
            ОбновитьСтрокуПрокрутки = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Minute != ВремяПоследнегоОбновления.Minute)
            {
                ВремяПоследнегоОбновления = DateTime.Now;
                ОбновитьЧасы = true;
            }

            if ((DateTime.Now - ВремяПоследнегоОбновленияРасписания).Seconds > 10)
            {
                ВремяПоследнегоОбновленияРасписания = DateTime.Now;
                ОбноыитьРасписание = true;
                ОбновитьЧасы = true;
                НомерСтраницыПоездовДальнегоСледования++;
                НомерСтраницыЭлектропоездов++;
            }

            
            Graphics g = Graphics.FromHwnd(panel1.Handle);

            if (ОбноыитьРасписание)
            {
                СформироватьСписокСообщений();

                g.FillRectangle(drawBrushBlue, new Rectangle(0, 40, 390, 250));
                g.FillRectangle(drawBrushRed, new Rectangle(0, 0, 390, 40));
                g.FillRectangle(drawBrushRed, new Rectangle(0, 150, 390, 40));

                g.DrawString("Расписание движения поездов                    Московское время:", drawFont, drawBrushWhite, new Point(6, 6));
                g.DrawString("№ Поезда| Маршрут следования          |Приб. |Ст. |Отпр. |Дни", drawFont, drawBrushWhite, new Point(6, 25));

                g.DrawString("Расписание движения эл. поездов                   Местное время:", drawFont, drawBrushWhite, new Point(6, 156));
                g.DrawString("№ Поезда| Маршрут следования          |Приб. |Ст. |Отпр. |Дни", drawFont, drawBrushWhite, new Point(6, 175));



                int КоличествоСтраницПоездовДальнегоСледования = (РасписаниеПоездов.Count / КоличествоСтрокДляПоездовДальнегоСледования) + ((РасписаниеПоездов.Count % КоличествоСтрокДляПоездовДальнегоСледования) == 0 ? 0 : 1);
                if (НомерСтраницыПоездовДальнегоСледования >= КоличествоСтраницПоездовДальнегоСледования)
                    НомерСтраницыПоездовДальнегоСледования = 0;

                int КоличествоСтраницЭлектропоездов = (РасписаниеЭлектричек.Count / КоличествоСтрокДляЭлектропоездов) + ((РасписаниеЭлектричек.Count % КоличествоСтрокДляЭлектропоездов) == 0 ? 0 : 1);
                if (НомерСтраницыЭлектропоездов >= КоличествоСтраницЭлектропоездов)
                    НомерСтраницыЭлектропоездов = 0;


                for (int i = 0, j = 0; i < РасписаниеПоездов.Count; i++)
                {
                    if (i < (НомерСтраницыПоездовДальнегоСледования * КоличествоСтрокДляПоездовДальнегоСледования)) continue;

                    if (j < КоличествоСтрокДляПоездовДальнегоСледования)
                    {
                        g.DrawString(РасписаниеПоездов.ElementAt(i).Value.НомерПоезда, drawFont, drawBrushWhite, new Point(2, 45 + j * 18));
                        g.DrawString(РасписаниеПоездов.ElementAt(i).Value.НазваниеПоезда, drawFont, drawBrushWhite, new Point(30, 45 + j * 18));
                        g.DrawString(РасписаниеПоездов.ElementAt(i).Value.ВремяПрибытия, drawFont, drawBrushWhite, new Point(220, 45 + j * 18));
                        g.DrawString(РасписаниеПоездов.ElementAt(i).Value.ВремяСтоянки.ToString(), drawFont, drawBrushWhite, new Point(254, 45 + j * 18));
                        g.DrawString(РасписаниеПоездов.ElementAt(i).Value.ВремяОтправления, drawFont, drawBrushWhite, new Point(272, 45 + j * 18));
                        g.DrawString(РасписаниеПоездов.ElementAt(i).Value.Примечание, drawFont, drawBrushWhite, new Point(310, 45 + j * 18));
                        j++;
                    }
                }

                for (int i = 0, j = 0; i < РасписаниеЭлектричек.Count; i++)
                {
                    if (i < (НомерСтраницыЭлектропоездов * КоличествоСтрокДляЭлектропоездов)) continue;

                    if (j < КоличествоСтрокДляЭлектропоездов)
                    {
                        g.DrawString(РасписаниеЭлектричек.ElementAt(i).Value.НомерПоезда, drawFont, drawBrushWhite, new Point(2, 190 + j * 18));
                        g.DrawString(РасписаниеЭлектричек.ElementAt(i).Value.НазваниеПоезда, drawFont, drawBrushWhite, new Point(30, 190 + j * 18));
                        g.DrawString(РасписаниеЭлектричек.ElementAt(i).Value.ВремяПрибытия, drawFont, drawBrushWhite, new Point(220, 190 + j * 18));
                        g.DrawString(РасписаниеЭлектричек.ElementAt(i).Value.ВремяСтоянки.ToString(), drawFont, drawBrushWhite, new Point(254, 190 + j * 18));
                        g.DrawString(РасписаниеЭлектричек.ElementAt(i).Value.ВремяОтправления, drawFont, drawBrushWhite, new Point(272, 190 + j * 18));
                        g.DrawString(РасписаниеЭлектричек.ElementAt(i).Value.Примечание, drawFont, drawBrushWhite, new Point(310, 190 + j * 18));
                        j++;
                    }
                }


                ОбноыитьРасписание = false;
            }

            if (ОбновитьЧасы)
            {
                g.FillRectangle(drawBrushRed, new Rectangle(353, 5, 34, 18));
                g.DrawString(DateTime.Now.ToString("HH:mm"), drawFont, drawBrushWhite, new Point(353, 6));

                g.FillRectangle(drawBrushRed, new Rectangle(353, 155, 34, 18));
                g.DrawString(DateTime.Now.AddHours(4).ToString("HH:mm"), drawFont, drawBrushWhite, new Point(353, 156));

                ОбновитьЧасы = false;
            }
        }

        private void ОкноРасписания_Load(object sender, EventArgs e)
        {
            ОбновитьЧасы = true;
            ОбноыитьРасписание = true;
            ОбновитьСтрокуПрокрутки = true;
            panel1.Invalidate();
        }

        private void ОкноРасписания_Activated(object sender, EventArgs e)
        {
            ОбновитьЧасы = true;
            ОбноыитьРасписание = true;
            ОбновитьСтрокуПрокрутки = true;
            panel1.Invalidate();
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            ОбновитьЧасы = true;
            ОбноыитьРасписание = true;
            ОбновитьСтрокуПрокрутки = true;
            panel1.Invalidate();
        }



        private void СформироватьСписокСообщений()
        {
            РасписаниеПоездов.Clear();
            РасписаниеЭлектричек.Clear();

            foreach (TrainTableRecord Config in TrainTable.TrainTableRecords)
            {
                if (Config.Active == true)
                {
                    DateTime ТекущееВремя = DateTime.Now;
                    ПланРасписанияПоезда планРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(Config.Days);

                    if (планРасписанияПоезда.ПроверитьАктивностьРасписания() == true)
                    {
                        СтрокаВРасписаниии Строка;
                        Строка.НомерПоезда = Config.Num;
                        Строка.НазваниеПоезда = Config.Name;
                        Строка.ВремяПрибытия = Config.ArrivalTime;
                        Строка.ВремяОтправления = Config.DepartureTime;
                        Строка.ВремяСтоянки = Config.StopTime;
                        Строка.Примечание = Config.Примечание;

                        string Ключ = Строка.ВремяПрибытия == "" ? Строка.ВремяОтправления : Строка.ВремяПрибытия;
                        if (Config.ShowInPanels == 0x01)
                            РасписаниеПоездов.Add(Ключ, Строка);
                        else if (Config.ShowInPanels == 0x02)
                        {
                            if (Строка.ВремяПрибытия != "")
                            {
                                int Часы = (Строка.ВремяПрибытия[0] - '0') * 10 + (Строка.ВремяПрибытия[1] - '0');
                                int Минуты = (Строка.ВремяПрибытия[3] - '0') * 10 + (Строка.ВремяПрибытия[4] - '0');
                                if ((Часы >= 0) && (Часы <= 23))
                                {
                                    Часы = (Часы + 4) % 24;
                                    Строка.ВремяПрибытия = Часы.ToString("00") + ":" + Минуты.ToString("00");
                                }
                            }
                            if (Строка.ВремяОтправления != "")
                            {
                                int Часы = (Строка.ВремяОтправления[0] - '0') * 10 + (Строка.ВремяОтправления[1] - '0');
                                int Минуты = (Строка.ВремяОтправления[3] - '0') * 10 + (Строка.ВремяОтправления[4] - '0');
                                if ((Часы >= 0) && (Часы <= 23))
                                {
                                    Часы = (Часы + 4) % 24;
                                    Строка.ВремяОтправления = Часы.ToString("00") + ":" + Минуты.ToString("00");
                                }
                            }
                            РасписаниеЭлектричек.Add(Ключ, Строка);
                        }
                    }
                }
            }
        }    
    }
}
