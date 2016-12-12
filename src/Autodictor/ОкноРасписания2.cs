using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        const int КоличествоСтрокДляПоездовДальнегоСледования = 5;
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


        public int[] МассивСмещенийСтрок = new int[10];
        public int СмещениеИнформационнойСтроки = 0;
        public string ИнформационнаяСтрока = "";
        public DateTime ВремяДействия = new DateTime(2016, 12, 12);


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

                g.FillRectangle(drawBrushBlue, new Rectangle(0, 40, 390, 245));
                g.FillRectangle(drawBrushRed, new Rectangle(0, 0, 390, 40));
                g.FillRectangle(drawBrushRed, new Rectangle(0, 132, 390, 40));

                if (DateTime.Now < ВремяДействия)
                    g.DrawString("Расписание движ. поездов    ДО 11.12.16 !!!  Московск.время:", drawFont, drawBrushWhite, new Point(6, 6));
                else
                    g.DrawString("Расписание движ. поездов                          Московское время:", drawFont, drawBrushWhite, new Point(6, 6));
                g.DrawString("№ Поезда| Маршрут следования          |Приб. |Ст. |Отпр. |Дни", drawFont, drawBrushWhite, new Point(6, 25));

                g.DrawString("Расписание движения эл. поездов                   Местное время:", drawFont, drawBrushWhite, new Point(6, 138));
                g.DrawString("№ Поезда| Маршрут следования          |Приб. |Ст. |Отпр. |Дни", drawFont, drawBrushWhite, new Point(6, 157));



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
                        g.DrawString(РасписаниеЭлектричек.ElementAt(i).Value.НомерПоезда, drawFont, drawBrushWhite, new Point(2, 178 + j * 18));
                        g.DrawString(РасписаниеЭлектричек.ElementAt(i).Value.НазваниеПоезда, drawFont, drawBrushWhite, new Point(30, 178 + j * 18));
                        g.DrawString(РасписаниеЭлектричек.ElementAt(i).Value.ВремяПрибытия, drawFont, drawBrushWhite, new Point(220, 178 + j * 18));
                        g.DrawString(РасписаниеЭлектричек.ElementAt(i).Value.ВремяСтоянки.ToString(), drawFont, drawBrushWhite, new Point(254, 178 + j * 18));
                        g.DrawString(РасписаниеЭлектричек.ElementAt(i).Value.ВремяОтправления, drawFont, drawBrushWhite, new Point(272, 178 + j * 18));
                        g.DrawString(РасписаниеЭлектричек.ElementAt(i).Value.Примечание, drawFont, drawBrushWhite, new Point(310, 178 + j * 18));
                        j++;
                    }
                }


                // Информационная строка
                if (ИнформационнаяСтрока != Program.ИнфСтрокаНаТабло)
                {
                    СмещениеИнформационнойСтроки = 0;
                    ИнформационнаяСтрока = Program.ИнфСтрокаНаТабло;
                    g.DrawString(ИнформационнаяСтрока, drawFont, drawBrushWhite, new Point(5, 250));
                }
                else
                {
                    Size len2 = TextRenderer.MeasureText(ИнформационнаяСтрока, drawFont);
                    if (len2.Width >= 380)
                    {
                        if (СмещениеИнформационнойСтроки >= 10)
                        {
                            string SubString = ИнформационнаяСтрока.Substring(СмещениеИнформационнойСтроки - 10);
                            g.DrawString(SubString, drawFont, drawBrushWhite, new Point(5, 250));
                            if (SubString == "")
                            {
                                СмещениеИнформационнойСтроки = 0;
                                g.DrawString(ИнформационнаяСтрока, drawFont, drawBrushWhite, new Point(5, 250));
                            }
                        }
                    }
                    else
                    {
                        g.DrawString(ИнформационнаяСтрока, drawFont, drawBrushWhite, new Point(5, 250));
                    }
                }




                ОбноыитьРасписание = false;

                for (int i = 0; i < 10; i++)
                    МассивСмещенийСтрок[i] = 0;
            }


            // Обновить бегущую строку
            {
                for (int i = 0, j = 0; i < РасписаниеПоездов.Count; i++)
                {
                    if (i < (НомерСтраницыПоездовДальнегоСледования * КоличествоСтрокДляПоездовДальнегоСледования)) continue;

                    if (j < КоличествоСтрокДляПоездовДальнегоСледования)
                    {
                        Size len = TextRenderer.MeasureText(РасписаниеПоездов.ElementAt(i).Value.Примечание, drawFont);
                        if (len.Width >= 90)
                        {
                            if (++МассивСмещенийСтрок[j] < 10);
                            else
                            {
                                g.FillRectangle(drawBrushBlue, new Rectangle(310, 45 + j * 18, 90, 18));
                                string SubString = РасписаниеПоездов.ElementAt(i).Value.Примечание.Substring(МассивСмещенийСтрок[j] - 10);
                                g.DrawString(SubString, drawFont, drawBrushWhite, new Point(310, 45 + j * 18));
                                if (SubString == "")
                                {
                                    МассивСмещенийСтрок[j] = 0;
                                    g.DrawString(РасписаниеПоездов.ElementAt(i).Value.Примечание, drawFont, drawBrushWhite, new Point(310, 45 + j * 18));
                                }
                            }
                        }
                        
                        j++;
                    }
                }

                for (int i = 0, j = 0; i < РасписаниеЭлектричек.Count; i++)
                {
                    if (i < (НомерСтраницыЭлектропоездов * КоличествоСтрокДляЭлектропоездов)) continue;

                    if (j < КоличествоСтрокДляЭлектропоездов)
                    {
                        Size len = TextRenderer.MeasureText(РасписаниеЭлектричек.ElementAt(i).Value.Примечание, drawFont);
                        if (len.Width >= 90)
                        {
                            if (++МассивСмещенийСтрок[j + КоличествоСтрокДляПоездовДальнегоСледования] < 10);
                            else
                            {
                                g.FillRectangle(drawBrushBlue, new Rectangle(310, 178 + j * 18, 90, 18));
                                string SubString = РасписаниеЭлектричек.ElementAt(i).Value.Примечание.Substring(МассивСмещенийСтрок[j + КоличествоСтрокДляПоездовДальнегоСледования] - 10);
                                g.DrawString(SubString, drawFont, drawBrushWhite, new Point(310, 178 + j * 18));
                                if (SubString == "")
                                {
                                    МассивСмещенийСтрок[j + КоличествоСтрокДляПоездовДальнегоСледования] = 0;
                                    g.DrawString(РасписаниеЭлектричек.ElementAt(i).Value.Примечание, drawFont, drawBrushWhite, new Point(310, 178 + j * 18));
                                }
                            }
                        }
                        
                        j++;
                    }
                }


                // Бегущая строка
                if (ИнформационнаяСтрока != Program.ИнфСтрокаНаТабло)
                {
                    СмещениеИнформационнойСтроки = 0;
                    ИнформационнаяСтрока = Program.ИнфСтрокаНаТабло;
                    g.FillRectangle(drawBrushBlue, new Rectangle(0, 250, 400, 25));
                    g.DrawString(ИнформационнаяСтрока, drawFont, drawBrushWhite, new Point(5, 250));
                }

                Size len2 = TextRenderer.MeasureText(ИнформационнаяСтрока, drawFont);
                if (len2.Width >= 380)
                {
                    if (++СмещениеИнформационнойСтроки >= 10)
                    {
                        g.FillRectangle(drawBrushBlue, new Rectangle(0, 250, 400, 25));
                        string SubString = ИнформационнаяСтрока.Substring(СмещениеИнформационнойСтроки - 10);
                        g.DrawString(SubString, drawFont, drawBrushWhite, new Point(5, 250));
                        if (SubString == "")
                        {
                            СмещениеИнформационнойСтроки = 0;
                            g.DrawString(ИнформационнаяСтрока, drawFont, drawBrushWhite, new Point(5, 250));
                        }
                    }
                }
            }


            // Обновить часы
            if (ОбновитьЧасы)
            {
                g.FillRectangle(drawBrushRed, new Rectangle(353, 5, 34, 18));
                g.DrawString(DateTime.Now.ToString("HH:mm"), drawFont, drawBrushWhite, new Point(353, 6));

                g.FillRectangle(drawBrushRed, new Rectangle(353, 137, 34, 18));
                g.DrawString(DateTime.Now.AddHours(4).ToString("HH:mm"), drawFont, drawBrushWhite, new Point(353, 138));

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
                        {
                            if (РасписаниеПоездов.ContainsKey(Ключ) == false)
                                РасписаниеПоездов.Add(Ключ, Строка);
                            else
                            {
                                if (РасписаниеПоездов.ContainsKey(Ключ + " ") == false)
                                    РасписаниеПоездов.Add(Ключ + " ", Строка);
                            }
                        }
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
                            if (РасписаниеЭлектричек.ContainsKey(Ключ) == false)
                                РасписаниеЭлектричек.Add(Ключ, Строка);
                            else
                            {
                                if (РасписаниеЭлектричек.ContainsKey(Ключ + " ") == false)
                                    РасписаниеЭлектричек.Add(Ключ + " ", Строка);
                            }
                        }
                    }
                }
            }
        }    
    }
}
