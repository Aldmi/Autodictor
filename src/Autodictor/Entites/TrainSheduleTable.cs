using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entitys;

namespace MainExample.Entites
{
    public enum SourceData { Local, RemoteCis }



    public class TrainSheduleTable
    {
        #region Field

        private static object _lockObj = new object();
        private const string FileNameLocalTableRec = @"TableRecords.ini";
        private const string FileNameRemoteCisTableRec = @"TableRecordsRemoteCis.ini";

        public static SourceData _sourceLoad = SourceData.Local;
        public static List<TrainTableRecord> TrainTableRecords = new List<TrainTableRecord>(); // Содержит актуальное рабочее расписание

        #endregion




        #region Methode 

        /// <summary>
        /// Выбор источника загрузки и загрузка
        /// </summary>
        public static void SourceLoadMainList()
        {
            ЗагрузитьСписок(_sourceLoad == SourceData.Local ? FileNameLocalTableRec : FileNameRemoteCisTableRec);
        }


        /// <summary>
        /// Выбор источника сохранения и сохранение
        /// </summary>
        public static void SourceSaveMainList()
        {
            СохранитьСписок(TrainTableRecords, _sourceLoad == SourceData.Local ? FileNameLocalTableRec : FileNameRemoteCisTableRec);
        }


        /// <summary>
        /// Сохранить список от ЦИС
        /// </summary>
        public static void СохранитьИПрименитьСписокРегулярноеРасписаниеЦис(IList<TrainTableRecord> trainTableRecords)
        {
            СохранитьСписок(trainTableRecords, FileNameRemoteCisTableRec);
            switch (_sourceLoad)
            {
                case SourceData.RemoteCis:
                    TrainTableRecords = trainTableRecords as List<TrainTableRecord>;
                    break;
            }
        }


        /// <summary>
        /// Сохранить список в файл
        /// </summary>
        private static void СохранитьСписок(IList<TrainTableRecord> trainTableRecords, string fileName)
        {
            try
            {
                lock (_lockObj)
                {
                    using (StreamWriter dumpFile = new StreamWriter(fileName))
                    {
                        for (int i = 0; i < trainTableRecords.Count; i++)
                        {
                            string line = trainTableRecords[i].ID + ";" +
                                          trainTableRecords[i].Num + ";" +
                                          trainTableRecords[i].Name + ";" +
                                          trainTableRecords[i].ArrivalTime + ";" +
                                          trainTableRecords[i].StopTime + ";" +
                                          trainTableRecords[i].DepartureTime + ";" +
                                          trainTableRecords[i].Days + ";" +
                                          (trainTableRecords[i].Active ? "1" : "0") + ";" +
                                          trainTableRecords[i].SoundTemplates + ";" +
                                          trainTableRecords[i].TrainPathDirection.ToString() + ";" +
                                          SavePath2File(trainTableRecords[i].TrainPathNumber,
                                              trainTableRecords[i].PathWeekDayes) + ";" +
                                          trainTableRecords[i].ТипПоезда.ToString() + ";" +
                                          trainTableRecords[i].Примечание + ";" +
                                          trainTableRecords[i].ВремяНачалаДействияРасписания.ToString("dd.MM.yyyy HH:mm:ss") + ";" +
                                          trainTableRecords[i].ВремяОкончанияДействияРасписания.ToString("dd.MM.yyyy HH:mm:ss") + ";" +
                                          trainTableRecords[i].Addition + ";" +
                                          (trainTableRecords[i].ИспользоватьДополнение["табло"] ? "1" : "0") + ";" +
                                          (trainTableRecords[i].ИспользоватьДополнение["звук"] ? "1" : "0") + ";" +
                                          (trainTableRecords[i].Автомат ? "1" : "0") + ";" +

                                          trainTableRecords[i].Num2 + ";" +
                                          trainTableRecords[i].FollowingTime + ";" +
                                          trainTableRecords[i].DaysAlias + ";" +

                                          trainTableRecords[i].StationDepart + ";" +
                                          trainTableRecords[i].StationArrival + ";" +
                                          trainTableRecords[i].Direction + ";" +
                                          trainTableRecords[i].ChangeTrainPathDirection + ";" +
                                          trainTableRecords[i].ОграничениеОтправки;

                            dumpFile.WriteLine(line);
                        }

                        dumpFile.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }



        /// <summary>
        /// загрузить список из файл
        /// </summary>
        private static void ЗагрузитьСписок(string fileName)
        {
            lock (_lockObj)
            {
                TrainTableRecords.Clear();

                try
                {
                    using (StreamReader file = new StreamReader(fileName))
                    {
                        string line;
                        while ((line = file.ReadLine()) != null)
                        {
                            string[] settings = line.Split(';');
                            if ((settings.Length == 13) || (settings.Length == 15) || (settings.Length >= 16))
                            {
                                TrainTableRecord данные;

                                данные.ID = int.Parse(settings[0]);
                                данные.Num = settings[1];
                                данные.Name = settings[2];
                                данные.ArrivalTime = settings[3];
                                данные.StopTime = settings[4];
                                данные.DepartureTime = settings[5];
                                данные.Days = settings[6];
                                данные.Active = settings[7] == "1" ? true : false;
                                данные.SoundTemplates = settings[8];
                                данные.TrainPathDirection = byte.Parse(settings[9]);
                                данные.TrainPathNumber = LoadPathFromFile(settings[10], out данные.PathWeekDayes);
                                данные.ИспользоватьДополнение = new Dictionary<string, bool>()
                                {
                                    ["звук"] = false,
                                    ["табло"] = false
                                };


                                ТипПоезда типПоезда = ТипПоезда.НеОпределен;
                                try
                                {
                                    типПоезда = (ТипПоезда)Enum.Parse(typeof(ТипПоезда), settings[11]);
                                }
                                catch (ArgumentException) { }
                                данные.ТипПоезда = типПоезда;

                                данные.Примечание = settings[12];

                                if (данные.TrainPathDirection > 2)
                                    данные.TrainPathDirection = 0;

                                var path = Program.PathWaysRepository.List().FirstOrDefault(p => p.Name == данные.TrainPathNumber[WeekDays.Постоянно]);
                                if (path == null)
                                    данные.TrainPathNumber[WeekDays.Постоянно] = "";

                                DateTime началоДействия = new DateTime(1900, 1, 1);
                                DateTime конецДействия = new DateTime(2100, 1, 1);
                                if (settings.Length >= 15)
                                {
                                    DateTime.TryParse(settings[13], out началоДействия);
                                    DateTime.TryParse(settings[14], out конецДействия);
                                }
                                данные.ВремяНачалаДействияРасписания = началоДействия;
                                данные.ВремяОкончанияДействияРасписания = конецДействия;


                                var addition = "";
                                if (settings.Length >= 16)
                                {
                                    addition = settings[15];
                                }
                                данные.Addition = addition;


                                if (settings.Length >= 18)
                                {
                                    данные.ИспользоватьДополнение["табло"] = settings[16] == "1";
                                    данные.ИспользоватьДополнение["звук"] = settings[17] == "1";
                                }

                                данные.Автомат = true;
                                if (settings.Length >= 19)
                                {
                                    данные.Автомат = (string.IsNullOrEmpty(settings[18]) || settings[18] == "1"); // по умолчанию true
                                }


                                данные.Num2 = String.Empty;
                                данные.FollowingTime = String.Empty;
                                данные.DaysAlias = String.Empty;
                                if (settings.Length >= 22)
                                {
                                    данные.Num2 = settings[19];
                                    данные.FollowingTime = settings[20];
                                    данные.DaysAlias = settings[21];
                                }


                                данные.StationDepart = String.Empty;
                                данные.StationArrival = String.Empty;
                                if (settings.Length >= 23)
                                {
                                    данные.StationDepart = settings[22];
                                    данные.StationArrival = settings[23];
                                }

                                данные.Direction = String.Empty;
                                if (settings.Length >= 25)
                                {
                                    данные.Direction = settings[24];
                                }

                                данные.ChangeTrainPathDirection = false;
                                if (settings.Length >= 26)
                                {
                                    bool changeDirection;
                                    bool.TryParse(settings[25], out changeDirection);
                                    данные.ChangeTrainPathDirection = changeDirection;
                                }

                                данные.ОграничениеОтправки = false;
                                if (settings.Length >= 27)
                                {
                                    bool ограничениеОтправки;
                                    bool.TryParse(settings[26], out ограничениеОтправки);
                                    данные.ОграничениеОтправки = ограничениеОтправки;
                                }


                                TrainTableRecords.Add(данные);
                                Program.НомераПоездов.Add(данные.Num);
                                if (!string.IsNullOrEmpty(данные.Num2))
                                    Program.НомераПоездов.Add(данные.Num2);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }



        private static string SavePath2File(Dictionary<WeekDays, string> pathDictionary, bool pathWeekDayes)
        {
            StringBuilder strBuild = new StringBuilder();
            foreach (var keyVal in pathDictionary)
            {
                var value = (keyVal.Value == "Не определен") ? string.Empty : keyVal.Value;
                strBuild.Append(keyVal.Key).Append(":").Append(value).Append("|");
            }
            strBuild.Append("ПутиПоДням").Append(":").Append(pathWeekDayes ? "1" : "0");

            return strBuild.ToString();
        }



        private static Dictionary<WeekDays, string> LoadPathFromFile(string str, out bool pathWeekDayes)
        {
            Dictionary<WeekDays, string> pathDictionary = new Dictionary<WeekDays, string>
            {
                [WeekDays.Постоянно] = String.Empty,
                [WeekDays.Пн] = String.Empty,
                [WeekDays.Вт] = String.Empty,
                [WeekDays.Ср] = String.Empty,
                [WeekDays.Ср] = String.Empty,
                [WeekDays.Чт] = String.Empty,
                [WeekDays.Пт] = String.Empty,
                [WeekDays.Сб] = String.Empty,
                [WeekDays.Вс] = String.Empty
            };
            pathWeekDayes = false;

            if (!string.IsNullOrEmpty(str) && str.Contains("|") && str.Contains(":"))
            {
                var pairs = str.Split('|');
                if (pairs.Length == 9)
                {
                    foreach (var pair in pairs)
                    {
                        var keyVal = pair.Split(':');

                        var value = (keyVal[1] == "Не определен") ? string.Empty : keyVal[1];
                        switch (keyVal[0])
                        {
                            case "Постоянно":
                                pathDictionary[WeekDays.Постоянно] = value;
                                break;

                            case "Пн":
                                pathDictionary[WeekDays.Пн] = value;
                                break;

                            case "Вт":
                                pathDictionary[WeekDays.Вт] = value;
                                break;

                            case "Ср":
                                pathDictionary[WeekDays.Ср] = value;
                                break;

                            case "Чт":
                                pathDictionary[WeekDays.Чт] = value;
                                break;

                            case "Пт":
                                pathDictionary[WeekDays.Пт] = value;
                                break;

                            case "Сб":
                                pathDictionary[WeekDays.Сб] = value;
                                break;

                            case "Вс":
                                pathDictionary[WeekDays.Вс] = value;
                                break;

                            case "ПутиПоДням":
                                pathWeekDayes = (keyVal[1] == "1");
                                break;
                        }
                    }
                }
            }

            return pathDictionary;
        }

        #endregion
    }
}
