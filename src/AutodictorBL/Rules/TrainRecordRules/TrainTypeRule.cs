using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutodictorBL.Settings;
using AutodictorBL.Settings.XmlSettings;
using Library.Logs;
using Library.Xml;


namespace AutodictorBL.Rules.TrainRecordRules
{
    public enum TypeTrain { None, Suburb, LongDist }
    public enum ActionType { None, Arrival, Departure }
    public enum Emergency { None, DelayedArrival, DelayedDeparture, Cancel, DispatchOnReadiness }



    public class TrainTypeRule 
    {
        #region prop

        public int Id { get; set; }              //Id типа.
        public TypeTrain TypeTrain { get; set; }         //Принадлежность к типу (дальний/пригород)
        public string NameRu { get; set; }       //Имя и его Alias
        public string AliasRu { get; set; }
        public string NameEng { get; set; }
        public string AliasEng { get; set; }
        public string NameCh { get; set; }
        public string AliasCh { get; set; }

        public string ShowPathTimer { get; set; }//???
        public int WarningTimer { get; set; }  //окрашивать в главном окне в жёлтый за X минут до первого события.

        public List<ActionTrain> ActionTrains { get; set; }

        #endregion




        #region ctor

        public TrainTypeRule(string id, string typeTrain, string nameRu, string aliasRu, string nameEng, string aliasEng, string nameCh, string aliasCh, string showPathTimer, string warningTimer, List<ActionTrain> actionTrains)
        {         
            Id = int.Parse(id);
            switch (typeTrain)
            {
                case "Дальний":
                    TypeTrain = TypeTrain.LongDist;
                    break;

                case "Пригород":
                    TypeTrain = TypeTrain.Suburb;
                    break;

                default:
                    TypeTrain = TypeTrain.None;
                    break;
            }
            NameRu = nameRu;
            AliasRu = aliasRu;
            NameEng = nameEng;
            AliasEng = aliasEng;
            NameCh = nameCh;
            AliasCh = aliasCh;
            ShowPathTimer = showPathTimer;
            WarningTimer = int.Parse(warningTimer);
            ActionTrains = actionTrains;
        }

        #endregion
    }



    /// <summary>
    /// действие (шаблон)
    /// </summary>
    public class ActionTrain
    {
        #region prop

        public int Id { get; }                    //Id действия
        public string Name { get; set; }
        public ActionType ActionType { get; set; }
        public int Priority { get; set; }
        public int Repeat { get; set; }
        public bool Transit { get; set; }
        public Emergency Emergency { get; set; }
        public ActionTime Time { get; set; }
        public List<Lang> Langs { get; set; }      //Шаблоны на разных языках

        #endregion




        #region ctor

        public ActionTrain(string id, string name, string actionType, string priority, string repeat, string transit, string emergency, string times, List<Lang> langs)
        {
            Id = int.Parse(id);
            Name = name;

            switch (actionType)
            {
                case "ПРИБ":
                    ActionType = ActionType.Arrival;
                    break;

                case "ОТПР":
                    ActionType = ActionType.Departure;
                    break;

                default:
                    ActionType = ActionType.None;
                    break;
            }

            Priority = int.Parse(priority);
            Repeat = int.Parse(repeat);
            Transit = bool.Parse(transit);

            switch (emergency)
            {
                case "Отмена":
                    Emergency = Emergency.Cancel;
                    break;

                case "ЗадПриб":
                    Emergency = Emergency.DelayedArrival;
                    break;

                case "ЗадОтпр":
                    Emergency = Emergency.DelayedDeparture;
                    break;

                case "ОтпрПоГотов":
                    Emergency = Emergency.DispatchOnReadiness;
                    break;

                default:
                    Emergency = Emergency.None;
                    break;
            }

            Time= new ActionTime(times);
            Langs = langs;
        }

        #endregion
    }


    /// <summary>
    /// Время воспроизведенния для шаблона.
    /// </summary>
    public class ActionTime
    {
        #region

        public int? CycleTime { get; }     // Если стоит CycleTime, то Times игнорируется
        public int? Time { get; }          // Если стоит Time, то CycleTime игнорируется

        #endregion




        #region ctor

        public ActionTime(string time)
        {
            if(string.IsNullOrEmpty(time))
                return;

            if (time.StartsWith("~"))
            {
                Time = null;
                CycleTime = int.Parse(time.Remove(0, 1));
            }
            else
            {
                CycleTime = null;
                Time = int.Parse(time);
            }
        }

        #endregion
    }



    /// <summary>
    /// Язык и  шаблон для него
    /// </summary>
    public class Lang
    {
        #region prop

        public int Id { get; }              //Id действия
        public string Name { get; set; }
        public List<string> TemplateSoundStart { get; }
        public List<string> TemplateSoundBody { get; }
        public List<string> TemplateSoundEnd { get; }

        #endregion




        #region ctor

        public Lang(string id, string name, string templateSoundStart, string templateSoundBody, string templateSoundEnd)
        {
            Id = int.Parse(id);
            Name = name;
            TemplateSoundStart = string.IsNullOrEmpty(templateSoundStart) ? null : templateSoundStart.Split('|').ToList();
            TemplateSoundBody = string.IsNullOrEmpty(templateSoundBody) ? null : templateSoundBody.Split('|').ToList();
            TemplateSoundEnd = string.IsNullOrEmpty(templateSoundEnd) ? null : templateSoundEnd.Split('|').ToList();
        }

        #endregion
    }
}