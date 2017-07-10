using System;
using System.Collections.Generic;
using System.Linq;

namespace MainExample.Services
{
    public class SchedulingPipelineService
    {
        public bool CheckTrainActuality(ref TrainTableRecord config, DateTime dateCheck, Func<int, bool> limitationTime, byte workWithNumberOfDays)
        {
            var планРасписанияПоезда = ПланРасписанияПоезда.ПолучитьИзСтрокиПланРасписанияПоезда(config.Days, config.ВремяНачалаДействияРасписания, config.ВремяОкончанияДействияРасписания);
            if ((workWithNumberOfDays == 7) || (планРасписанияПоезда.ПолучитьРежимРасписания() != РежимРасписанияДвиженияПоезда.ПоДням) || (config.ТипПоезда == ТипПоезда.Пассажирский) || (config.ТипПоезда == ТипПоезда.Скоростной) || (config.ТипПоезда == ТипПоезда.Скорый))
            {
                var активностьНаДень = планРасписанияПоезда.ПолучитьАктивностьДняДвижения((byte)(dateCheck.Month - 1), (byte)(dateCheck.Day - 1), dateCheck);
                if (активностьНаДень == false)
                    return false;

                if (limitationTime != null)
                {
                    DateTime времяПрибытия;
                    if (DateTime.TryParse(config.ArrivalTime, out времяПрибытия))
                    {
                        if (!limitationTime(времяПрибытия.Hour))
                            return false;
                    }

                    DateTime времяОтправления;
                    if (DateTime.TryParse(config.DepartureTime, out времяОтправления))
                    {
                        if (!limitationTime(времяОтправления.Hour))
                            return false;
                    }
                }
            }
            else
            {
                if (планРасписанияПоезда.ПолучитьАктивностьДняДвижения((byte)4, (byte)workWithNumberOfDays, dateCheck) == false)
                    return false;
            }

            return true;
        }


        public string GetUniqueKey(IEnumerable<string> currentKeys,  DateTime addingKey)
        {
            int tryCounter = 50;
            while (--tryCounter > 0)
            {
                string key = addingKey.ToString("yy.MM.dd  HH:mm:ss");
                if (!currentKeys.Contains(key))
                {
                    return key;
                }
                addingKey = addingKey.AddSeconds(1);
            }

            return null;

           // throw new Exception($"Невозможно добавить запись под ключем: {addingKey:yy.MM.dd  HH:mm:ss}");
        }

    }
}