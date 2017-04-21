using System;
using System.Collections.Generic;
using System.Linq;
using CommunicationDevices.DataProviders;


namespace CommunicationDevices.Settings
{
    public class Conditions
    {
        #region prop

        public TypeTrain TypeTrain { get; set; }                     //Пригород или дальнего следования

        public string Event { get; set; }                            //Событие (отправление/прибытие)

        public bool LowCurrentTime { get; set; }                     // Больше Тек. времени
        public bool HightCurrentTime { get; set; }                   // Меньше Тек. времени


        public bool ArrivalAndSuburb { get; set; }                   //Пригород прибывающий
        public bool DepartureAndSuburb { get; set; }                 //Пригород отбывающий

        public bool ArrivalAndLongDistance { get; set; }             //Дальнего след. прибывающий
        public bool DepartureAndLongDistance { get; set; }           //Дальнего след.  отбывающий


        public IEnumerable<string> SuburbPaths { get; set; }         //Пути для пригородного поыезда
        public IEnumerable<string> LongDistancePaths { get; set; }   //Пути для поезда дальнего след.
        public IEnumerable<string> ArrivalPaths { get; set; }        //Пути для прибывающего поезда
        public IEnumerable<string> DeparturePaths { get; set; }      //Пути для отправляющегося поезда

        #endregion







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

            var suburbPathsFilter = true;
            if (SuburbPaths != null && SuburbPaths.Any())
            {
               suburbPathsFilter = !((inData.TypeTrain == TypeTrain.Suburb) && SuburbPaths.Contains(inData.PathNumber));
            }

            var longDistancePathsFilter = true;
            if (LongDistancePaths != null && LongDistancePaths.Any())
            {
                longDistancePathsFilter = !((inData.TypeTrain == TypeTrain.LongDistance) && LongDistancePaths.Contains(inData.PathNumber));
            }

            var arrivalPathsFilter = true;
            if (ArrivalPaths != null && ArrivalPaths.Any())
            {
                arrivalPathsFilter = !((inData.Event == "ПРИБ.") && ArrivalPaths.Contains(inData.PathNumber));
            }

            var departurePathsFilter = true;
            if (DeparturePaths != null && DeparturePaths.Any())
            {
                departurePathsFilter = !((inData.Event == "ОТПР.") && DeparturePaths.Contains(inData.PathNumber));
            }


            return inData.TypeTrain != TypeTrain &&
                   inData.Event != Event &&
                   timeFilter &&
                   arrivalAndSuburbFilter && 
                   arrivalAndLongDistanceFilter &&
                   departureAndSuburbFilter &&
                   departureAndLongDistanceFilter &&
                   suburbPathsFilter &&
                   longDistancePathsFilter &&
                   arrivalPathsFilter &&
                   departurePathsFilter;
        }




        /// <summary>
        /// Проверка разрешения.
        /// </summary>
        public bool CheckResolutions(UniversalInputType inData)
        {
            var typeTrainFilter = true;
            if (TypeTrain != TypeTrain.None)
            {
                typeTrainFilter = (inData.TypeTrain == TypeTrain);
            }

            var eventFilter = true;
            if (!string.IsNullOrEmpty(Event))
            {
                eventFilter = (inData.Event == Event);
            }

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
                arrivalAndSuburbFilter = (inData.Event == "ПРИБ.") && (inData.TypeTrain == TypeTrain.Suburb);
            }

            var arrivalAndLongDistanceFilter = true;
            if (ArrivalAndLongDistance)
            {
                arrivalAndLongDistanceFilter = (inData.Event == "ПРИБ.") && (inData.TypeTrain == TypeTrain.LongDistance);
            }

            var departureAndSuburbFilter = true;
            if (DepartureAndSuburb)
            {
                departureAndSuburbFilter = (inData.Event == "ОТПР.") && (inData.TypeTrain == TypeTrain.Suburb);
            }

            var departureAndLongDistanceFilter = true;
            if (DepartureAndLongDistance)
            {
                departureAndLongDistanceFilter = (inData.Event == "ОТПР.") && (inData.TypeTrain == TypeTrain.LongDistance);
            }

            var suburbPathsFilter = true;
            if (SuburbPaths != null && SuburbPaths.Any())
            {
                suburbPathsFilter = (inData.TypeTrain == TypeTrain.Suburb) && SuburbPaths.Contains(inData.PathNumber);
            }

            var longDistancePathsFilter = true;
            if (LongDistancePaths != null && LongDistancePaths.Any())
            {
                longDistancePathsFilter = (inData.TypeTrain == TypeTrain.LongDistance) && LongDistancePaths.Contains(inData.PathNumber);
            }

            var arrivalPathsFilter = true;
            if (ArrivalPaths != null && ArrivalPaths.Any())
            {
                arrivalPathsFilter = (inData.Event == "ПРИБ.") && ArrivalPaths.Contains(inData.PathNumber);
            }

            var departurePathsFilter = true;
            if (DeparturePaths != null && DeparturePaths.Any())
            {
                departurePathsFilter = (inData.Event == "ОТПР.") && DeparturePaths.Contains(inData.PathNumber);
            }


            return typeTrainFilter &&
                   eventFilter &&
                   timeFilter &&
                   arrivalAndSuburbFilter &&
                   arrivalAndLongDistanceFilter &&
                   departureAndSuburbFilter &&
                   departureAndLongDistanceFilter &&
                   suburbPathsFilter &&
                   longDistancePathsFilter &&
                   arrivalPathsFilter &&
                   departurePathsFilter;
        }

    }

}