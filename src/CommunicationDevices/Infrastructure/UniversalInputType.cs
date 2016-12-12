using System;
using System.Collections.Generic;

namespace CommunicationDevices.Infrastructure
{
    //public interface IUniversalInputType
    //{
    //    string AddressDevice { get; set; }                      //Адресс устройсва

    //    string NumberOfTrain { get; set; }                      //Номер поезда
    //    string PathNumber { get; set; }                         //Номер пути
    //    string Event { get; set; }                              //Событие (отправление/прибытие)
    //    string Stations { get; set; }                           //Станции Отправления-Назначения.
    //    string Note { get; set; }                               //Примечание.
    //    DateTime Time { get; set; }                             //Время

    //    string Message { get; set; }                            //Сообщение
    //}


    public enum TypeTrain {None, Suburb, LongDistance }

    public class UniversalInputType //: IUniversalInputType
    {
        public string AddressDevice { get; set; }                    //Адресс устройсва

        public TypeTrain TypeTrain { get; set; }                     //Приигород или дальнего следования
        public string NumberOfTrain { get; set; }                    //Номер поезда
        public string PathNumber { get; set; }                       //Номер пути
        public string Event { get; set; }                            //Событие (отправление/прибытие)
        public string Stations { get; set; }                         //Станции Отправления-Назначения.
        public string Note { get; set; }                             //Примечание.
        public DateTime Time { get; set; }                           //Время
        public string Message { get; set; }                          //Сообщение

        public List<UniversalInputType> TableData { get; set; }     //Данные для табличного представления


        public void Initialize(UniversalInputType initializeData)
        {
            AddressDevice = initializeData.AddressDevice;
            NumberOfTrain = initializeData.NumberOfTrain;
            PathNumber = initializeData.PathNumber;
            Event = initializeData.Event;
            Stations = initializeData.Stations;
            Time = initializeData.Time;
            Message = initializeData.Message;
        }
    }
}