using System;

namespace CommunicationDevices.Infrastructure
{
    public interface IUniversalInputType
    {
        string AddressDevice { get; set; }                    //Адресс устройсва

        string NumberOfTrain { get; set; }                      //Номер поезда
        string PathNumber { get; set; }                         //Номер пути
        string Event { get; set; }                              //Событие (отправление/прибытие)
        string Stations { get; set; }                           //Станции остановочные.
        DateTime Time { get; set; }                             //Время

        string Message { get; set; }                          //Сообщение
    }


    public class UniversalInputType : IUniversalInputType
    {
        public string AddressDevice { get; set; }                    //Адресс устройсва

        public string NumberOfTrain { get; set; }                    //Номер поезда
        public string PathNumber { get; set; }                       //Номер пути
        public string Event { get; set; }                            //Событие (отправление/прибытие)
        public string Stations { get; set; }                         //Станции остановочные.
        public DateTime Time { get; set; }                           //Время
        public string Message { get; set; }                          //Сообщение
    }
}