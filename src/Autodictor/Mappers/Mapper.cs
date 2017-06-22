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


    }
}