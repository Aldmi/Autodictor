using System;
using System.Collections.Generic;

namespace MainExample.Mappers
{
    public static class Mapper
    {
        public static List<СтатическоеСообщение> MapSoundConfigurationRecord2СтатическоеСообщение(SoundConfigurationRecord scr, ref int newId)
        {
            СтатическоеСообщение statRecord;
            statRecord.СостояниеВоспроизведения = SoundRecordStatus.ОжиданиеВоспроизведения;
            List<СтатическоеСообщение> resultList = new List<СтатическоеСообщение>();

            if (scr.Enable == true)
            {
                if (scr.EnablePeriodic == true)
                {
                    statRecord.ОписаниеКомпозиции = scr.Name;
                    statRecord.НазваниеКомпозиции = scr.Name;

                    if (statRecord.НазваниеКомпозиции == string.Empty)
                        return null;

                    string[] Times = scr.MessagePeriodic.Split(',');
                    if (Times.Length != 3)
                        return null;

                    DateTime НачалоИнтервала2 = DateTime.Parse(Times[0]), КонецИнтервала2 = DateTime.Parse(Times[1]);
                    int Интервал = int.Parse(Times[2]);

                    while (НачалоИнтервала2 < КонецИнтервала2)
                    {
                        statRecord.ID = newId++;
                        statRecord.Время = НачалоИнтервала2;
                        statRecord.Активность = true;

                        resultList.Add(statRecord);
                        НачалоИнтервала2 = НачалоИнтервала2.AddMinutes(Интервал);
                    }
                }

                if (scr.EnableSingle == true)
                {
                    statRecord.ОписаниеКомпозиции = scr.Name;
                    statRecord.НазваниеКомпозиции = scr.Name;

                    if (statRecord.НазваниеКомпозиции == string.Empty)
                        return null;

                    string[] Times = scr.MessageSingle.Split(',');

                    foreach (string time in Times)
                    {
                        statRecord.ID = newId++;
                        statRecord.Время = DateTime.Parse(time);
                        statRecord.Активность = true;

                        resultList.Add(statRecord);
                    }
                }
            }

            return resultList;
        }



        public static SoundRecord MapTrainTableRecord2SoundRecord(TrainTableRecord Config, DateTime day)
        {
            var Record = new SoundRecord();
            Record.ID = 0;
            Record.НомерПоезда = Config.Num;
            Record.НомерПоезда2 = Config.Num2;
            Record.НазваниеПоезда = Config.Name;
            Record.Дополнение = Config.Addition;
            Record.ИспользоватьДополнение = new Dictionary<string, bool>
            {
                ["звук"] = Config.ИспользоватьДополнение["звук"],
                ["табло"] = Config.ИспользоватьДополнение["табло"]
            };
            Record.Направление = Config.Direction;
            Record.СтанцияОтправления = "";
            Record.СтанцияНазначения = "";
            Record.ДниСледования = Config.Days;
            Record.ДниСледованияAlias = Config.DaysAlias;
            Record.Активность = Config.Active;
            Record.Автомат = Config.Автомат;
            Record.ШаблонВоспроизведенияСообщений = Config.SoundTemplates;
            Record.НомерПути = ПолучитьНомерПутиПоДнямНедели(Config);
            Record.НумерацияПоезда = Config.TrainPathDirection;
            Record.Примечание = Config.Примечание;
            Record.ТипПоезда = Config.ТипПоезда;
            Record.Состояние = SoundRecordStatus.ОжиданиеВоспроизведения;
            Record.ТипСообщения = SoundRecordType.ДвижениеПоездаНеПодтвержденное;
            Record.Описание = "";
            Record.КоличествоПовторений = 1;
            Record.СостояниеКарточки = 0;
            Record.ОписаниеСостоянияКарточки = "";
            Record.БитыНештатныхСитуаций = 0x00;
            Record.ТаймерПовторения = 0;
            Record.РазрешениеНаОтображениеПути = PathPermissionType.ИзФайлаНастроек;

            Record.ИменаФайлов = new string[0];
            Record.ФиксированноеВремяПрибытия = null;
            Record.ФиксированноеВремяОтправления = null;


            //string[] названияСтанций = Config.Name.Split('-');
            //if (названияСтанций.Length == 2)
            //{
            //    Record.СтанцияОтправления = названияСтанций[0].Trim();
            //    Record.СтанцияНазначения = названияСтанций[1].Trim();
            //}
            //else if (названияСтанций.Length == 1)
            //    Record.СтанцияНазначения = названияСтанций[0].Trim();


            Record.СтанцияОтправления = Config.StationDepart;
            Record.СтанцияНазначения = Config.StationArrival;


            int часы = 0;
            int минуты = 0;
            DateTime времяПрибытия = new DateTime(2000, 1, 1, 0, 0, 0);
            DateTime времяОтправления = new DateTime(2000, 1, 1, 0, 0, 0);
            Record.ВремяПрибытия = DateTime.Now;
            Record.ВремяОтправления = DateTime.Now;
            Record.ОжидаемоеВремя = DateTime.Now;
            Record.ВремяСледования = null;
            Record.ВремяЗадержки = null;
            byte номерСписка = 0x00;


            if (Config.ArrivalTime != "")
            {
                string[] subStrings = Config.ArrivalTime.Split(':');
                if (int.TryParse(subStrings[0], out часы) && int.TryParse(subStrings[1], out минуты))
                {
                    времяПрибытия = new DateTime(day.Year, day.Month, day.Day, часы, минуты, 0);
                    Record.ВремяПрибытия = времяПрибытия;
                    Record.ОжидаемоеВремя = времяПрибытия;
                    номерСписка |= 0x04;
                }
            }

            if (Config.DepartureTime != "")
            {
                string[] subStrings = Config.DepartureTime.Split(':');
                if (int.TryParse(subStrings[0], out часы) && int.TryParse(subStrings[1], out минуты))
                {
                    времяОтправления = new DateTime(day.Year, day.Month, day.Day, часы, минуты, 0);
                    Record.ВремяОтправления = времяОтправления;
                    Record.ОжидаемоеВремя = времяОтправления;
                    номерСписка |= 0x10;
                }
            }

            if (!string.IsNullOrEmpty(Config.FollowingTime))
            {
                string[] subStrings = Config.FollowingTime.Split(':');
                if (int.TryParse(subStrings[0], out часы) && int.TryParse(subStrings[1], out минуты))
                {
                    Record.ВремяСледования = new DateTime(day.Year, day.Month, day.Day, часы, минуты, 0);
                }
            }

            if ((номерСписка & 0x04) != 0x00)
            {
                Record.Время = Record.ВремяПрибытия;
                Record.ОжидаемоеВремя = Record.ВремяПрибытия;
            }
            else
            {
                Record.Время = Record.ВремяОтправления;
                Record.ОжидаемоеВремя = Record.ВремяОтправления;
            }

            //ТРАНЗИТ
            Record.ВремяСтоянки = null;
            if (номерСписка == 0x14)
            {
                //вермя отправления указанно для след. суток
                if (времяОтправления < времяПрибытия)
                {
                    Record.ВремяОтправления = времяОтправления.AddDays(1);
                }

                TimeSpan времяСтоянки;
                if (TimeSpan.TryParse(Config.StopTime, out времяСтоянки))
                {
                    Record.ВремяСтоянки = времяСтоянки;
                }
                номерСписка |= 0x08;                                              //TODO: ???
            }

            Record.БитыАктивностиПолей = номерСписка;
            Record.БитыАктивностиПолей |= 0x03;                                   //TODO: ???



            // Шаблоны оповещения
            Record.СписокФормируемыхСообщений = new List<СостояниеФормируемогоСообщенияИШаблон>();
            string[] шаблонОповещения = Record.ШаблонВоспроизведенияСообщений.Split(':');
            if ((шаблонОповещения.Length % 3) == 0)
            {
                bool активностьШаблоновДанногоПоезда = Record.ТипПоезда == ТипПоезда.Пассажирский && Program.Настройки.АвтФормСообщНаПассажирскийПоезд;
                if (Record.ТипПоезда == ТипПоезда.Пригородный && Program.Настройки.АвтФормСообщНаПригородныйЭлектропоезд) активностьШаблоновДанногоПоезда = true;
                if (Record.ТипПоезда == ТипПоезда.Скоростной && Program.Настройки.АвтФормСообщНаСкоростнойПоезд) активностьШаблоновДанногоПоезда = true;
                if (Record.ТипПоезда == ТипПоезда.Скорый && Program.Настройки.АвтФормСообщНаСкорыйПоезд) активностьШаблоновДанногоПоезда = true;
                if (Record.ТипПоезда == ТипПоезда.Ласточка && Program.Настройки.АвтФормСообщНаЛасточку) активностьШаблоновДанногоПоезда = true;
                if (Record.ТипПоезда == ТипПоезда.Фирменный && Program.Настройки.АвтФормСообщНаФирменный) активностьШаблоновДанногоПоезда = true;
                if (Record.ТипПоезда == ТипПоезда.РЭКС && Program.Настройки.АвтФормСообщНаРЭКС) активностьШаблоновДанногоПоезда = true;

                int indexШаблона = 0;
                for (int i = 0; i < шаблонОповещения.Length / 3; i++)
                {
                    bool наличиеШаблона = false;
                    string шаблон = "";
                    foreach (var item in DynamicSoundForm.DynamicSoundRecords)
                        if (item.Name == шаблонОповещения[3 * i + 0])
                        {
                            наличиеШаблона = true;
                            шаблон = item.Message;
                            break;
                        }

                    if (наличиеШаблона == true)
                    {
                        var привязкаВремени = 0;
                        int.TryParse(шаблонОповещения[3 * i + 2], out привязкаВремени);

                        string[] времяАктивацииШаблона = шаблонОповещения[3 * i + 1].Replace(" ", "").Split(',');
                        foreach (var время in времяАктивацииШаблона)
                        {
                            int времяСмещения = 0;
                            if ((int.TryParse(время, out времяСмещения)) == true)
                            {
                                СостояниеФормируемогоСообщенияИШаблон новыйШаблон;
                                новыйШаблон.Id = indexШаблона++;
                                новыйШаблон.SoundRecordId = Record.ID;
                                новыйШаблон.Активность = активностьШаблоновДанногоПоезда;
                                новыйШаблон.Приоритет = Priority.Midlle;
                                новыйШаблон.Воспроизведен = false;
                                новыйШаблон.СостояниеВоспроизведения = SoundRecordStatus.ОжиданиеВоспроизведения;
                                новыйШаблон.ВремяСмещения = времяСмещения;
                                новыйШаблон.НазваниеШаблона = шаблонОповещения[3 * i + 0];
                                новыйШаблон.Шаблон = шаблон;
                                новыйШаблон.ПривязкаКВремени = 0;
                                новыйШаблон.ЯзыкиОповещения = new List<NotificationLanguage> { NotificationLanguage.Ru, NotificationLanguage.Eng };  //TODO:Брать из ШаблонОповещения полученого из TrainTable.

                                Record.СписокФормируемыхСообщений.Add(новыйШаблон);
                            }
                        }
                    }
                }
            }


            Record.СписокНештатныхСообщений = new List<СостояниеФормируемогоСообщенияИШаблон>();



            return Record;
        }




        public static string ПолучитьНомерПутиПоДнямНедели(TrainTableRecord record)
        {
            if (!record.PathWeekDayes)
            {
                return record.TrainPathNumber[WeekDays.Постоянно] == "Не определен" ? string.Empty : record.TrainPathNumber[WeekDays.Постоянно];
            }

            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return record.TrainPathNumber[WeekDays.Пн];

                case DayOfWeek.Tuesday:
                    return record.TrainPathNumber[WeekDays.Вт];

                case DayOfWeek.Wednesday:
                    return record.TrainPathNumber[WeekDays.Ср];

                case DayOfWeek.Thursday:
                    return record.TrainPathNumber[WeekDays.Чт];

                case DayOfWeek.Friday:
                    return record.TrainPathNumber[WeekDays.Пн];

                case DayOfWeek.Saturday:
                    return record.TrainPathNumber[WeekDays.Сб];

                case DayOfWeek.Sunday:
                    return record.TrainPathNumber[WeekDays.Вс];
            }

            return String.Empty;
        }

    }
}