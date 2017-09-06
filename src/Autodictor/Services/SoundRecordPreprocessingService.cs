using System;
using System.Collections.Generic;
using Domain.Entitys;

namespace MainExample.Services
{
    public interface ISoundRecordPreprocessing
    {
        void StartPreprocessing(ref SoundRecord rec);
    }




    public class SoundRecordPreprocessingTimezone : ISoundRecordPreprocessing
    {
        private int DeltaTimezoneMinute { get; set; }



        public void StartPreprocessing(ref SoundRecord rec)
        {
            switch (rec.ТипПоезда)
            {
                case ТипПоезда.Пассажирский:
                    DeltaTimezoneMinute = Program.Настройки.TimeZoneНаПассажирскийПоезд;
                    break;

                case ТипПоезда.Пригородный:
                    DeltaTimezoneMinute = Program.Настройки.TimeZoneНаПригородныйЭлектропоезд;
                    break;

                case ТипПоезда.Фирменный:
                    DeltaTimezoneMinute = Program.Настройки.TimeZoneНаФирменный;
                    break;

                case ТипПоезда.Скорый:
                    DeltaTimezoneMinute = Program.Настройки.TimeZoneНаСкорыйПоезд;
                    break;

                case ТипПоезда.Скоростной:
                    DeltaTimezoneMinute = Program.Настройки.TimeZoneНаСкоростнойПоезд;
                    break;

                case ТипПоезда.Ласточка:
                    DeltaTimezoneMinute = Program.Настройки.TimeZoneНаЛасточку;
                    break;

                case ТипПоезда.РЭКС:
                    DeltaTimezoneMinute = Program.Настройки.TimeZoneНаРЭКС;
                    break;
            }

            rec.ВремяОтправления = rec.ВремяОтправления.AddMinutes(DeltaTimezoneMinute);
            rec.ВремяПрибытия = rec.ВремяПрибытия.AddMinutes(DeltaTimezoneMinute);
            rec.Время = rec.Время.AddMinutes(DeltaTimezoneMinute);
        }
    }




    public class SoundRecordPreprocessingService
    {
        public IEnumerable<ISoundRecordPreprocessing> PreprocessingList { get; set; }



        public SoundRecordPreprocessingService(IEnumerable<ISoundRecordPreprocessing> preprocessingList)
        {
            PreprocessingList = preprocessingList;
        }



        public void StartPreprocessing(ref SoundRecord rec)
        {
            foreach (var item in PreprocessingList)
            {
                item.StartPreprocessing(ref rec);
            }
        }
    }
}