using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;



namespace CommunicationDevices.DataProviders
{
    public enum TypeTrain
    {
        None,                    //НеОпределен
        Passenger,               //Пассажирский
        Suburban,                //Пригородный
        Corporate,               //Фирменный
        Express,                 //Скорый
        HighSpeed,               //Скоростной
        Swallow,                 //Ласточка
        Rex,                     //РЭКС
    }

    public enum VagonDirection { None, FromTheHead, FromTheTail }
    public enum Command { None, View, Update, Delete, Clear, Restart }


    public class UniversalInputType
    {
        public int Id { get; set; }
        public string AddressDevice { get; set; }                    //Адресс устройсва
        public bool IsActive { get; set; }                           //Флаг активности записи ("Отменен без объявления")

        public TypeTrain TypeTrain { get; set; }                     //Пригород или дальнего следования
        public string NumberOfTrain { get; set; }                    //Номер поезда
        public string PathNumber { get; set; }                       //Номер пути
        public string Event { get; set; }                            //Событие (отправление/прибытие/Транзит)
        public string Addition { get; set; }                         //Дополнение (свободная строка)
        public string Stations { get; set; }                         //Станции Отправления-Назначения.
        public string Note { get; set; }                             //Примечание.
        public string DaysFollowing { get; set; }                    //Дни следования
        public DateTime Time { get; set; }                           //Время
        public Dictionary<string, DateTime> TransitTime { get; set; } //Транзитное время ["приб"]/["отпр"]
        public DateTime? DelayTime { get; set; }                     //Время задержки (прибытия или отправления поезда)
        public DateTime ExpectedTime { get; set; }                   //Ожидаемое время (Время + Время задержки)
        public VagonDirection VagonDirection { get; set; }           //Нумерация вагона (с головы, с хвоста)
        public string Message { get; set; }                          //Сообщение

        public byte EmergencySituation { get; set; }                 //Нешатная ситуация (бит 0 - Отмена, бит 1 - задержка прибытия, бит 2 - задержка отправления, бит 3 - отправление по готовности)

        public Command Command { get; set; }                         //Команда (если указанна команда, то приоритет отдается выполнению команды.)

        public List<UniversalInputType> TableData { get; set; }      //Данные для табличного представления

        public List<bool> SoundChanels { get; set; }                 //Настройка звуковых каналов (по каким каналам передавать данное сообщение)

        public Dictionary<string, dynamic> ViewBag { get; set; }     //Не типизированный контейнер для передачи любых данных






        #region Methode

        public void Initialize(UniversalInputType initializeData)
        {
            AddressDevice = initializeData.AddressDevice;
            IsActive = initializeData.IsActive;
            TypeTrain = initializeData.TypeTrain;
            NumberOfTrain = initializeData.NumberOfTrain;
            PathNumber = initializeData.PathNumber;
            Event = initializeData.Event;
            Addition = initializeData.Addition;
            Stations = initializeData.Stations;
            Note= initializeData.Note;
            Time = initializeData.Time;
            TransitTime = initializeData.TransitTime;
            DelayTime = initializeData.DelayTime;
            ExpectedTime = initializeData.ExpectedTime;
            Message = initializeData.Message;
            EmergencySituation = initializeData.EmergencySituation;
            Command = initializeData.Command;
            VagonDirection = initializeData.VagonDirection;


            if (initializeData.TableData != null && initializeData.TableData.Any())
            {
                TableData = new List<UniversalInputType>(initializeData.TableData);
            }

            if (initializeData.SoundChanels != null && initializeData.SoundChanels.Any())
            {
                SoundChanels = new List<bool>(initializeData.SoundChanels);
            }

            if (initializeData.ViewBag != null && initializeData.ViewBag.Any())
            {
                ViewBag= new Dictionary<string, dynamic>(initializeData.ViewBag);
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

        #endregion
    }
}