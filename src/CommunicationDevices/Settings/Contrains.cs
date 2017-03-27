using System;
using CommunicationDevices.DataProviders;


namespace CommunicationDevices.Settings
{
    public class Contrains
    {
        public TypeTrain TypeTrain { get; set; }                     //Пригород или дальнего следования

        public string Event { get; set; }                            //Событие (отправление/прибытие)

        public bool LowCurrentTime { get; set; }                     // Больше Тек. времени
        public bool HightCurrentTime { get; set; }                   // Меньше Тек. времени


        public bool ArrivalAndSuburb { get; set; }                   //Пригород прибывающий
        public bool DepartureAndSuburb { get; set; }                 //Пригород отбывающий

        public bool ArrivalAndLongDistance { get; set; }             //Дальнего след. прибывающий
        public bool DepartureAndLongDistance { get; set; }           //Дальнего след.  отбывающий




        /// <summary>
        /// Проверка ограничения привязки.
        /// </summary>
        public bool CheckContrains(UniversalInputType inData)
        {
            var timeFilter = true;
            if (LowCurrentTime)    //"МеньшеТекВремени"
            {
                timeFilter = inData.Time < DateTime.Now;
            }
            if (HightCurrentTime)  //"БольшеТекВремени"
            {
                timeFilter = inData.Time > DateTime.Now;
            }

            var arrivalAndSuburbFilter = true;
            if (ArrivalAndSuburb)
            {
                arrivalAndSuburbFilter = !((inData.Event == "ПРИБ.") && (inData.TypeTrain == TypeTrain.Suburb));
            }

            var arrivalAndLongDistanceFilter = true;
            if (ArrivalAndLongDistance)
            {
                arrivalAndLongDistanceFilter = !((inData.Event == "ПРИБ.") && (inData.TypeTrain == TypeTrain.LongDistance));
            }

            var departureAndSuburbFilter = true;
            if (DepartureAndSuburb)
            {
                departureAndSuburbFilter = !((inData.Event == "ОТПР.") && (inData.TypeTrain == TypeTrain.Suburb));
            }

            var departureAndLongDistanceFilter = true;
            if (DepartureAndLongDistance)
            {
                departureAndLongDistanceFilter = !((inData.Event == "ОТПР.") && (inData.TypeTrain == TypeTrain.LongDistance));
            }


            return inData.TypeTrain != TypeTrain &&
                   inData.Event != Event &&
                   timeFilter &&
                   arrivalAndSuburbFilter &&
                   arrivalAndLongDistanceFilter &&
                   departureAndSuburbFilter &&
                   departureAndLongDistanceFilter;
        }

    }

}