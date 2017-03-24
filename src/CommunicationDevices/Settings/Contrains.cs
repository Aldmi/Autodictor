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


        public bool SuburbAndArrival { get; set; }                   //Пригород прибывающий
        public bool SuburbAndDepart { get; set; }                    //Пригород отбывающий

        public bool LongDistanceAndArrival { get; set; }             //Дальнего след. прибывающий
        public bool LongDistanceAndDepart { get; set; }              //Дальнего след.  отбывающий
    }
}