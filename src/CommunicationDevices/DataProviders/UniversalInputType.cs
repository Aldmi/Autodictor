﻿using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;



namespace CommunicationDevices.DataProviders
{
    public enum TypeTrain {None, Suburb, LongDistance }

    public enum Command { None, View, Update, Delete, Clear, Restart }

    public class UniversalInputType
    {
        public int Id { get; set; }

        public string AddressDevice { get; set; }                    //Адресс устройсва

        public TypeTrain TypeTrain { get; set; }                     //Пригород или дальнего следования
        public string NumberOfTrain { get; set; }                    //Номер поезда
        public string PathNumber { get; set; }                       //Номер пути
        public string Event { get; set; }                            //Событие (отправление/прибытие)
        public string Stations { get; set; }                         //Станции Отправления-Назначения.
        public string Note { get; set; }                             //Примечание.
        public string DaysFollowing { get; set; }                    //Дни следования
        public DateTime Time { get; set; }                           //Время
        public string Message { get; set; }                          //Сообщение

        public Command Command { get; set; }                         //Команда (если указанна команда, то приоритет отдается выполнению команды.)

        public List<UniversalInputType> TableData { get; set; }     //Данные для табличного представления

        public List<bool> SoundChanels { get; set; }                //Настройка звуковых каналов (по каким каналам передавать данное сообщение)


        public void Initialize(UniversalInputType initializeData)
        {
            AddressDevice = initializeData.AddressDevice;
            TypeTrain = initializeData.TypeTrain;
            NumberOfTrain = initializeData.NumberOfTrain;
            PathNumber = initializeData.PathNumber;
            Event = initializeData.Event;
            Stations = initializeData.Stations;
            Note= initializeData.Note;
            Time = initializeData.Time;
            Message = initializeData.Message;
  
            if (initializeData.TableData != null && initializeData.TableData.Any())
            {
                TableData = new List<UniversalInputType>(initializeData.TableData);
            }

            if (initializeData.SoundChanels != null && initializeData.SoundChanels.Any())
            {
                SoundChanels = new List<bool>(initializeData.SoundChanels);
            }
        }


        public static List<UniversalInputType> GetFilteringByDateTimeTable(int outElement, IEnumerable<UniversalInputType> table)
        {
            if (outElement <= 0)
                return null;

            if (table.Count() < outElement)
                return null;


            var filtredCollection = new List<UniversalInputType>();
            var copyTableData = new List<UniversalInputType>(table);
            var today = DateTime.Now;
            for (int i = 0; i < outElement; i++)
            {
                var nearVal = copyTableData.MinBy(d => (d.Time - today).Duration());
                filtredCollection.Add(nearVal);
                copyTableData.RemoveAt(copyTableData.IndexOf(nearVal));
            }

            return filtredCollection;
        }
    }
}