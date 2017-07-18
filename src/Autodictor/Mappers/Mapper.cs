using System;
using System.Collections.Generic;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Model;


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




        public static SoundRecord MapTrainTableRecord2SoundRecord(TrainTableRecord config, DateTime day, int id)
        {
            var record = new SoundRecord();
            record.ID = id;
            record.НомерПоезда = config.Num;
            record.НомерПоезда2 = config.Num2;
            record.НазваниеПоезда = config.Name;
            record.Дополнение = config.Addition;
            record.ИспользоватьДополнение = new Dictionary<string, bool>
            {
                ["звук"] = config.ИспользоватьДополнение["звук"],
                ["табло"] = config.ИспользоватьДополнение["табло"]
            };
            record.Направление = config.Direction;
            record.СтанцияОтправления = "";
            record.СтанцияНазначения = "";
            record.ДниСледования = config.Days;
            record.ДниСледованияAlias = config.DaysAlias;
            record.Активность = config.Active;
            record.Автомат = config.Автомат;
            record.ШаблонВоспроизведенияСообщений = config.SoundTemplates;
            record.НомерПути = ПолучитьНомерПутиПоДнямНедели(config);
            record.НомерПутиБезАвтосброса = record.НомерПути;
            record.НумерацияПоезда = config.TrainPathDirection;
            record.Примечание = config.Примечание;
            record.ТипПоезда = config.ТипПоезда;
            record.Состояние = SoundRecordStatus.ОжиданиеВоспроизведения;
            record.ТипСообщения = SoundRecordType.ДвижениеПоездаНеПодтвержденное;
            record.Описание = "";
            record.КоличествоПовторений = 1;
            record.СостояниеКарточки = 0;
            record.ОписаниеСостоянияКарточки = "";
            record.БитыНештатныхСитуаций = 0x00;
            record.ТаймерПовторения = 0;
            record.РазрешениеНаОтображениеПути = PathPermissionType.ИзФайлаНастроек;

            record.ИменаФайлов = new string[0];
            record.ФиксированноеВремяПрибытия = null;
            record.ФиксированноеВремяОтправления = null;


            //string[] названияСтанций = config.Name.Split('-');
            //if (названияСтанций.Length == 2)
            //{
            //    record.СтанцияОтправления = названияСтанций[0].Trim();
            //    record.СтанцияНазначения = названияСтанций[1].Trim();
            //}
            //else if (названияСтанций.Length == 1)
            //    record.СтанцияНазначения = названияСтанций[0].Trim();


            record.СтанцияОтправления = config.StationDepart;
            record.СтанцияНазначения = config.StationArrival;


            int часы = 0;
            int минуты = 0;
            DateTime времяПрибытия = new DateTime(2000, 1, 1, 0, 0, 0);
            DateTime времяОтправления = new DateTime(2000, 1, 1, 0, 0, 0);
            record.ВремяПрибытия = DateTime.Now;
            record.ВремяОтправления = DateTime.Now;
            record.ОжидаемоеВремя = DateTime.Now;
            record.ВремяСледования = null;
            record.ВремяЗадержки = null;
            byte номерСписка = 0x00;


            if (config.ArrivalTime != "")
            {
                string[] subStrings = config.ArrivalTime.Split(':');
                if (int.TryParse(subStrings[0], out часы) && int.TryParse(subStrings[1], out минуты))
                {
                    времяПрибытия = new DateTime(day.Year, day.Month, day.Day, часы, минуты, 0);
                    record.ВремяПрибытия = времяПрибытия;
                    record.ОжидаемоеВремя = времяПрибытия;
                    номерСписка |= 0x04;
                }
            }

            if (config.DepartureTime != "")
            {
                string[] subStrings = config.DepartureTime.Split(':');
                if (int.TryParse(subStrings[0], out часы) && int.TryParse(subStrings[1], out минуты))
                {
                    времяОтправления = new DateTime(day.Year, day.Month, day.Day, часы, минуты, 0);
                    record.ВремяОтправления = времяОтправления;
                    record.ОжидаемоеВремя = времяОтправления;
                    номерСписка |= 0x10;
                }
            }

            if (!string.IsNullOrEmpty(config.FollowingTime))
            {
                string[] subStrings = config.FollowingTime.Split(':');
                if (int.TryParse(subStrings[0], out часы) && int.TryParse(subStrings[1], out минуты))
                {
                    record.ВремяСледования = new DateTime(day.Year, day.Month, day.Day, часы, минуты, 0);
                }
            }

            if ((номерСписка & 0x04) != 0x00)
            {
                record.Время = record.ВремяПрибытия;
                record.ОжидаемоеВремя = record.ВремяПрибытия;
            }
            else
            {
                record.Время = record.ВремяОтправления;
                record.ОжидаемоеВремя = record.ВремяОтправления;
            }

            //ТРАНЗИТ
            record.ВремяСтоянки = null;
            if (номерСписка == 0x14)
            {
                //вермя отправления указанно для след. суток
                if (времяОтправления < времяПрибытия)
                {
                    record.ВремяОтправления = времяОтправления.AddDays(1);
                }

                TimeSpan времяСтоянки;
                if (TimeSpan.TryParse(config.StopTime, out времяСтоянки))
                {
                    record.ВремяСтоянки = времяСтоянки;
                }
                номерСписка |= 0x08;                                              //TODO: ???
            }

            record.БитыАктивностиПолей = номерСписка;
            record.БитыАктивностиПолей |= 0x03;                                   //TODO: ???



            // Шаблоны оповещения
            record.СписокФормируемыхСообщений = new List<СостояниеФормируемогоСообщенияИШаблон>();
            string[] шаблонОповещения = record.ШаблонВоспроизведенияСообщений.Split(':');
            if ((шаблонОповещения.Length % 3) == 0)
            {
                bool активностьШаблоновДанногоПоезда = record.ТипПоезда == ТипПоезда.Пассажирский && Program.Настройки.АвтФормСообщНаПассажирскийПоезд;
                if (record.ТипПоезда == ТипПоезда.Пригородный && Program.Настройки.АвтФормСообщНаПригородныйЭлектропоезд) активностьШаблоновДанногоПоезда = true;
                if (record.ТипПоезда == ТипПоезда.Скоростной && Program.Настройки.АвтФормСообщНаСкоростнойПоезд) активностьШаблоновДанногоПоезда = true;
                if (record.ТипПоезда == ТипПоезда.Скорый && Program.Настройки.АвтФормСообщНаСкорыйПоезд) активностьШаблоновДанногоПоезда = true;
                if (record.ТипПоезда == ТипПоезда.Ласточка && Program.Настройки.АвтФормСообщНаЛасточку) активностьШаблоновДанногоПоезда = true;
                if (record.ТипПоезда == ТипПоезда.Фирменный && Program.Настройки.АвтФормСообщНаФирменный) активностьШаблоновДанногоПоезда = true;
                if (record.ТипПоезда == ТипПоезда.РЭКС && Program.Настройки.АвтФормСообщНаРЭКС) активностьШаблоновДанногоПоезда = true;

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
                                новыйШаблон.SoundRecordId = record.ID;
                                новыйШаблон.Активность = активностьШаблоновДанногоПоезда;
                                новыйШаблон.Приоритет = Priority.Midlle;
                                новыйШаблон.Воспроизведен = false;
                                новыйШаблон.СостояниеВоспроизведения = SoundRecordStatus.ОжиданиеВоспроизведения;
                                новыйШаблон.ВремяСмещения = времяСмещения;
                                новыйШаблон.НазваниеШаблона = шаблонОповещения[3 * i + 0];
                                новыйШаблон.Шаблон = шаблон;
                                новыйШаблон.ПривязкаКВремени = привязкаВремени;
                                новыйШаблон.ЯзыкиОповещения = new List<NotificationLanguage> { NotificationLanguage.Ru, NotificationLanguage.Eng };  //TODO:Брать из ШаблонОповещения полученого из TrainTable.

                                record.СписокФормируемыхСообщений.Add(новыйШаблон);
                            }
                        }
                    }
                }
            }


            record.СписокНештатныхСообщений = new List<СостояниеФормируемогоСообщенияИШаблон>();


            return record;
        }




        public static UniversalInputType MapTrainTableRecord2UniversalInputType(TrainTableRecord t)
        {
            Func<string, string, DateTime> timePars = (arrival, depart) =>
            {
                DateTime outData;
                if (DateTime.TryParse(arrival, out outData))
                    return outData;

                if (DateTime.TryParse(depart, out outData))
                    return outData;

                return DateTime.MinValue;
            };

            Func<string, string, string> eventPars = (arrivalTime, departTime) =>
            {
                if ((!string.IsNullOrEmpty(arrivalTime)) && (!string.IsNullOrEmpty(departTime)))
                {
                    return "СТОЯНКА";
                }

                if (!string.IsNullOrEmpty(arrivalTime))
                {
                    return "ПРИБ.";
                }

                if (!string.IsNullOrEmpty(departTime))
                {
                    return "ОТПР.";
                }

                return String.Empty;
            };


            Func<string, string, Dictionary<string, DateTime>> transitTimePars = (arrivalTime, departTime) =>
            {
                var transitTime = new Dictionary<string, DateTime>();
                if ((!string.IsNullOrEmpty(arrivalTime)) && (!string.IsNullOrEmpty(departTime)))
                {
                    transitTime["приб"] = timePars(arrivalTime, String.Empty);
                    transitTime["отпр"] = timePars(departTime, String.Empty);
                }

                return transitTime;
            };


            Func<string, string, KeyValuePair<string, string>> stationsPars = (station, direction) =>
            {
                if (string.IsNullOrEmpty(direction) || string.IsNullOrEmpty(station))
                {
                    return new KeyValuePair<string, string>();
                }

                var stationDir = Program.ПолучитьСтанциюНаправления(direction, station);
                if (stationDir == null)
                    return new KeyValuePair<string, string>();

                return new KeyValuePair<string, string>(stationDir.NameRu, stationDir.NameEng);
            };



            TimeSpan stopTime;
            UniversalInputType uit = new UniversalInputType
            {
                IsActive = t.Active,
                Event = eventPars(t.ArrivalTime, t.DepartureTime),
                TypeTrain = (t.ТипПоезда == ТипПоезда.Пассажирский) ? TypeTrain.Passenger :
                                            (t.ТипПоезда == ТипПоезда.Пригородный) ? TypeTrain.Suburban :
                                            (t.ТипПоезда == ТипПоезда.Фирменный) ? TypeTrain.Corporate :
                                            (t.ТипПоезда == ТипПоезда.Скорый) ? TypeTrain.Express :
                                            (t.ТипПоезда == ТипПоезда.Скоростной) ? TypeTrain.HighSpeed :
                                            (t.ТипПоезда == ТипПоезда.Ласточка) ? TypeTrain.Swallow :
                                            (t.ТипПоезда == ТипПоезда.РЭКС) ? TypeTrain.Rex : TypeTrain.None,
                Note = t.Примечание, //C остановками: ...
                PathNumber = ПолучитьНомерПутиПоДнямНедели(t),
                VagonDirection = (VagonDirection)t.TrainPathDirection,
                NumberOfTrain = t.Num,
                Stations = t.Name,
                DirectionStation = t.Direction,
                StationDeparture = stationsPars(t.StationDepart, t.Direction),
                StationArrival = stationsPars(t.StationArrival, t.Direction),
                Time = timePars(t.ArrivalTime, t.DepartureTime),
                TransitTime = transitTimePars(t.ArrivalTime, t.DepartureTime),
                DelayTime = null,
                StopTime = (TimeSpan?)(TimeSpan.TryParse(t.StopTime, out stopTime) ? (ValueType)stopTime : null),
                ExpectedTime = timePars(t.ArrivalTime, t.DepartureTime),
                DaysFollowing = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(t.Days).ПолучитьСтрокуОписанияРасписания(),
                DaysFollowingAlias = t.DaysAlias,
                Addition = t.Addition,
                Command = Command.None,
                EmergencySituation = 0x00
            };

            return uit;
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




        public static UniversalInputType MapSoundRecord2UniveralInputType(SoundRecord data, bool pathPermission, bool isShow)
        {
            DateTime time = DateTime.MinValue;
            Dictionary<string, DateTime> transitTimes = new Dictionary<string, DateTime>();

            string actStr = "   ";

            var номерПоезда = data.НомерПоезда;
            if ((data.БитыАктивностиПолей & 0x14) == 0x14)
            {
                actStr = "СТОЯНКА";
                time = data.ВремяПрибытия; //TODO: выполняется фильтрация по этому полю, нужно понять по какому времени фильтровать
                transitTimes["приб"] = data.ВремяПрибытия;
                transitTimes["отпр"] = data.ВремяОтправления;

                номерПоезда = (string.IsNullOrEmpty(data.НомерПоезда2) || string.IsNullOrWhiteSpace(data.НомерПоезда2)) ? data.НомерПоезда : (data.НомерПоезда + "/" + data.НомерПоезда2);
            }
            else if ((data.БитыАктивностиПолей & 0x04) == 0x04)
            {
                actStr = "ПРИБ.";
                time = data.ВремяПрибытия;
                номерПоезда = data.НомерПоезда;
            }
            else if ((data.БитыАктивностиПолей & 0x10) == 0x10)
            {
                actStr = "ОТПР.";
                time = data.ВремяОтправления;
                номерПоезда = data.НомерПоезда;
            }

            TypeTrain typeTrain;
            switch (data.ТипПоезда)
            {
                case ТипПоезда.Пассажирский:
                    typeTrain = TypeTrain.Passenger;
                    break;

                case ТипПоезда.Пригородный:
                    typeTrain = TypeTrain.Suburban;
                    break;

                case ТипПоезда.Фирменный:
                    typeTrain = TypeTrain.Corporate;
                    break;

                case ТипПоезда.Скорый:
                    typeTrain = TypeTrain.Express;
                    break;

                case ТипПоезда.Скоростной:
                    typeTrain = TypeTrain.HighSpeed;
                    break;

                case ТипПоезда.Ласточка:
                    typeTrain = TypeTrain.Swallow;
                    break;

                case ТипПоезда.РЭКС:
                    typeTrain = TypeTrain.Rex;
                    break;

                default:
                    typeTrain = TypeTrain.None;
                    break;
            }

            var command = Command.None;
            switch (data.СостояниеОтображения)
            {
                case TableRecordStatus.Отображение:
                    command = Command.View;
                    break;

                case TableRecordStatus.Очистка:
                    command = Command.Delete;
                    break;

                case TableRecordStatus.Обновление:
                    command = Command.Update;
                    break;
            }

            var номерПути = string.Empty;
            switch (data.РазрешениеНаОтображениеПути)
            {
                case PathPermissionType.ИзФайлаНастроек:
                    номерПути = pathPermission ? data.НомерПути : "   ";
                    break;

                case PathPermissionType.Отображать:
                    номерПути = data.НомерПути;
                    break;

                case PathPermissionType.НеОтображать:
                    номерПути = "   ";
                    break;
            }

            var defaultStation = ExchangeModel.NameRailwayStation;

            var cтанцияОтправления = Program.ПолучитьСтанциюНаправления(data.Направление, data.СтанцияОтправления);
            var cтанцияНазначения = Program.ПолучитьСтанциюНаправления(data.Направление, data.СтанцияНазначения);

            var stationDepartMyltiLang = new KeyValuePair<string, string>(
                                         cтанцияОтправления == null ? defaultStation.Key : cтанцияОтправления.NameRu,
                                         cтанцияОтправления == null ? defaultStation.Value : cтанцияОтправления.NameEng);

            var stationArrivalMyltiLang = new KeyValuePair<string, string>(
                                         cтанцияНазначения == null ? defaultStation.Key : cтанцияНазначения.NameRu,
                                         cтанцияНазначения == null ? defaultStation.Value : cтанцияНазначения.NameEng);

            UniversalInputType mapData;
            if (isShow)
            {
                mapData = new UniversalInputType
                {
                    Id = data.ID,
                    IsActive = data.Активность,
                    NumberOfTrain = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? номерПоезда : "   ",
                    VagonDirection = (VagonDirection)data.НумерацияПоезда,
                    PathNumber = номерПути,
                    Event = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? actStr : "   ",
                    Time = time,
                    TransitTime = transitTimes,
                    DelayTime = data.ВремяЗадержки,
                    ExpectedTime = data.ОжидаемоеВремя,
                    StopTime = data.ВремяСтоянки,
                    Stations = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? data.НазваниеПоезда : "   ",
                    DirectionStation = data.Направление,
                    StationDeparture = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? stationDepartMyltiLang : new KeyValuePair<string, string>(),
                    StationArrival = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? stationArrivalMyltiLang : new KeyValuePair<string, string>(),
                    Note = (data.СостояниеОтображения != TableRecordStatus.Очистка) ? data.Примечание : "   ",
                    TypeTrain = typeTrain,
                    Addition = (data.ИспользоватьДополнение["табло"]) ? data.Дополнение : string.Empty,
                    Command = command,
                    EmergencySituation = data.БитыНештатныхСитуаций
                };
            }
            else
            {
                mapData = new UniversalInputType
                {
                    Id = data.ID,
                    IsActive = data.Активность,
                    NumberOfTrain = номерПоезда,
                    VagonDirection = (VagonDirection)data.НумерацияПоезда,
                    PathNumber = номерПути,
                    Event = actStr,
                    Time = time,
                    TransitTime = transitTimes,
                    DelayTime = data.ВремяЗадержки,
                    ExpectedTime = data.ОжидаемоеВремя,
                    StopTime = data.ВремяСтоянки,
                    Stations = data.НазваниеПоезда,
                    DirectionStation = data.Направление,
                    StationDeparture = stationDepartMyltiLang,
                    StationArrival = stationArrivalMyltiLang,
                    Note = data.Примечание,
                    TypeTrain = typeTrain,
                    Addition = (data.ИспользоватьДополнение["табло"]) ? data.Дополнение : string.Empty,
                    Command = command,
                    EmergencySituation = data.БитыНештатныхСитуаций
                };
            }

            return mapData;
        }
    }
}