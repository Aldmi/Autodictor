﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;





namespace MainExample
{
    public enum РежимРасписанияДвиженияПоезда
    {
        Отсутствует = 0,
        Ежедневно = 1,
        ПоЧетным = 2,
        ПоНечетным = 3,
        Выборочно = 4,
        ЕжедневноКромеВыходных = 5,
    };

    public class ПланРасписанияПоезда
    {
        private uint[] БитыРасписания;
        private РежимРасписанияДвиженияПоезда[] РежимРасписания;
        private string НомерПоезда;
        private string НазваниеПоезда;

        public static string[] НазваниеМесяцев = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };

        public ПланРасписанияПоезда()
        {
            БитыРасписания = new uint[12];
            РежимРасписания = new РежимРасписанияДвиженияПоезда[12];
            НомерПоезда = "";
            НазваниеПоезда = "";
        }

        public string ПолучитьНомерПоезда()
        {
            return this.НомерПоезда;
        }

        public void УстановитьНомерПоезда(string НомерПоезда)
        {
            this.НомерПоезда = НомерПоезда;
        }

        public string ПолучитьНазваниеПоезда()
        {
            return this.НазваниеПоезда;
        }

        public void УстановитьНазваниеПоезда(string НазваниеПоезда)
        {
            this.НазваниеПоезда = НазваниеПоезда;
        }

        public bool ПолучитьАктивностьДняДвижения(byte НомерМесяца, byte НомерДня)
        {
            if ((НомерМесяца < 12) && (НомерДня < 31))
            {
                switch (РежимРасписания[НомерМесяца])
                {
                    case РежимРасписанияДвиженияПоезда.Отсутствует:
                        return false;

                    case РежимРасписанияДвиженияПоезда.Ежедневно:
                        return true;

                    case РежимРасписанияДвиженияПоезда.ПоЧетным:
                        return (НомерДня % 2) == 1 ? true : false;

                    case РежимРасписанияДвиженияПоезда.ПоНечетным:
                        return (НомерДня % 2) == 0 ? true : false;

                    case РежимРасписанияДвиженияПоезда.Выборочно:
                    case РежимРасписанияДвиженияПоезда.ЕжедневноКромеВыходных:
                        return (БитыРасписания[НомерМесяца] & (1 << НомерДня)) != 0 ? true : false;
                }
            }

            return false;
        }

        public void ЗадатьАктивностьДняДвижения(byte НомерМесяца, byte НомерДня, bool Активность)
        {
            if ((НомерМесяца < 12) && (НомерДня < 31))
                if (Активность == true)
                    БитыРасписания[НомерМесяца] |= (uint)(1 << НомерДня);
                else
                    БитыРасписания[НомерМесяца] &= (uint)((1 << НомерДня) ^ 0xFFFFFFFF);
        }

        public РежимРасписанияДвиженияПоезда ПолучитьРежимРасписания(byte НомерМесяца)
        {
            if (НомерМесяца < 12)
                return РежимРасписания[НомерМесяца];

            return РежимРасписанияДвиженияПоезда.Отсутствует;
        }

        public void ЗадатьРежимРасписания(byte НомерМесяца, РежимРасписанияДвиженияПоезда РежимРасписанияПоезда)
        {
            if ((НомерМесяца < 12) && (РежимРасписанияПоезда <= РежимРасписанияДвиженияПоезда.ЕжедневноКромеВыходных))
                РежимРасписания[НомерМесяца] = РежимРасписанияПоезда;
        }

        public string ПолучитьСтрокуРасписания()
        {
            string СтрокаРасписания = "";

            for (byte i = 0; i < 12; i++)
            {
                СтрокаРасписания += НазваниеМесяцев[i] + ",";

                switch (ПолучитьРежимРасписания(i))
                {
                    case РежимРасписанияДвиженияПоезда.Отсутствует:
                        СтрокаРасписания += "Отсутствует,";
                        break;

                    case РежимРасписанияДвиженияПоезда.Ежедневно:
                        СтрокаРасписания += "Ежедневно,";
                        break;

                    case РежимРасписанияДвиженияПоезда.ПоЧетным:
                        СтрокаРасписания += "ПоЧетным,";
                        break;

                    case РежимРасписанияДвиженияПоезда.ПоНечетным:
                        СтрокаРасписания += "ПоНечетным,";
                        break;

                    case РежимРасписанияДвиженияПоезда.Выборочно:
                    case РежимРасписанияДвиженияПоезда.ЕжедневноКромеВыходных:
                        for (byte j = 0; j < 31; j++)
                            if ((БитыРасписания[i] & (1 << j)) != 0)
                                СтрокаРасписания += (j+1).ToString() + ",";
                        break;
                }

                if (СтрокаРасписания[СтрокаРасписания.Length - 1] == ',')
                    СтрокаРасписания = СтрокаРасписания.Remove(СтрокаРасписания.Length - 1);

                СтрокаРасписания += ":";
            }

            if (СтрокаРасписания[СтрокаРасписания.Length - 1] == ':')
                СтрокаРасписания = СтрокаРасписания.Remove(СтрокаРасписания.Length - 1);

            return СтрокаРасписания;
        }

        public static ПланРасписанияПоезда ПолучитьИзСтрокиПланРасписанияПоезда(string РасписаниеПоезда)
        {
            ПланРасписанияПоезда ПланРасписания = new ПланРасписанияПоезда();

            string[] ПланПоМесяцам = РасписаниеПоезда.Split(':');
            if (ПланПоМесяцам.Length == 12)
            {
                for (byte i = 0; i < 12; i++)
                {
                    string[] ПоляМесячногоПлана = ПланПоМесяцам[i].Split(',');
                    if (ПоляМесячногоПлана.Contains("Отсутствует"))
                        ПланРасписания.ЗадатьРежимРасписания(i, РежимРасписанияДвиженияПоезда.Отсутствует);
                    else if (ПоляМесячногоПлана.Contains("Ежедневно"))
                        ПланРасписания.ЗадатьРежимРасписания(i, РежимРасписанияДвиженияПоезда.Ежедневно);
                    else if (ПоляМесячногоПлана.Contains("ПоЧетным"))
                        ПланРасписания.ЗадатьРежимРасписания(i, РежимРасписанияДвиженияПоезда.ПоЧетным);
                    else if (ПоляМесячногоПлана.Contains("ПоНечетным"))
                        ПланРасписания.ЗадатьРежимРасписания(i, РежимРасписанияДвиженияПоезда.ПоНечетным);
                    else
                    {
                        ПланРасписания.ЗадатьРежимРасписания(i, РежимРасписанияДвиженияПоезда.Выборочно);

                        foreach (string item in ПоляМесячногоПлана)
                        {
                            int День = 0;
                            if (int.TryParse(item, out День))
                                if ((День > 0) && (День < 32))
                                    ПланРасписания.ЗадатьАктивностьДняДвижения(i, (byte)(День - 1), true);
                        }
                    }

                }
            }

            return ПланРасписания;
        }


        public bool ПроверитьАктивностьРасписания()
        {
            DateTime ПервыйАктивныйДень = new DateTime(2000, 1, 1);
            DateTime ПоследнийАктивныйДень = new DateTime(2000, 1, 1);
            bool ПервыйДеньНайден = false;


            byte[] КоличествоДнейВМесяце = new byte[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
            for (byte НомерМесяца = 0; НомерМесяца < 12; НомерМесяца++)
            {
                byte ПоследнийДеньМесяца = КоличествоДнейВМесяце[НомерМесяца];
                if (((DateTime.Now.Year % 4) == 0) && (НомерМесяца == 1)) ПоследнийДеньМесяца = 29;
                for (byte НомерДня = 0; НомерДня < 31; НомерДня++)
                {
                    bool Результат = ПолучитьАктивностьДняДвижения(НомерМесяца, НомерДня);

                    if (Результат == true)
                    {
                        if (ПервыйДеньНайден == false)
                        {
                            ПервыйДеньНайден = true;
                            ПервыйАктивныйДень = new DateTime(DateTime.Now.Year, НомерМесяца + 1, НомерДня + 1, 0, 0, 0);
                        }

                        try
                        {
                            ПоследнийАктивныйДень = new DateTime(DateTime.Now.Year, НомерМесяца + 1, НомерДня + 1, 23, 59, 59);
                        }
                        catch (Exception ex)
                        {
                            //ex.Message;
                        }
                    }
                }
            }

            if ((DateTime.Now >= ПервыйАктивныйДень) && (DateTime.Now <= ПоследнийАктивныйДень))
                return true;

            return false;
        }



    }
}
