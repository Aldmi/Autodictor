using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WCFAvtodictor2PcTableContract.DataContract
{
    [DataContract]
    public class UniversalDisplayType
    {
        [DataMember]
        public string NumberOfTrain { get; set; }                      //Номер поезда

        [DataMember]
        public string PathNumber { get; set; }                         //Номер пути

        [DataMember]
        public string Event { get; set; }                              //Событие (отправление/прибытие)

        [DataMember]
        public string Stations { get; set; }                           //Станции

        [DataMember]
        public DateTime Time { get; set; }                             //Время

        [DataMember]
        public string Message { get; set; }                            //Сообщение

        [DataMember]
        public List<UniversalDisplayType> TableData { get; set; }     //Данные для табличного представления
    }
}