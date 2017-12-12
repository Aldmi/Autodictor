using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Xml.Linq;
using AutodictorBL.Rules.TrainRecordRules;
using AutodictorBL.Settings.XmlSettings;


namespace AutodictorBL.Settings
{
    public static class XmlSettingFactory
    {
        #region Methode

        /// <summary>
        /// Создание списка настроек для правил поезда зависмых от ТИПА поезда
        /// </summary>
        public static List<TrainTypeRule> CreateXmlTrainTypeRules(XElement xml)
        {
            var trainTypes = xml?.Elements("TrainType").ToList();
            if (trainTypes == null || !trainTypes.Any())
                return null;

            var rules= new List<TrainTypeRule>();
            foreach (var el in trainTypes)
            {
                var actions = el.Elements("Action").ToList();
                var listActs= new List<ActionTrain>();
                foreach (var act in actions)
                {
                    var langs = act.Elements("Lang").ToList();
                    var listLangs= new List<Lang>();
                    foreach (var lang in langs)
                    {
                        listLangs.Add(new Lang(
                            (string)lang.Attribute("Id"),
                            (string)lang.Attribute("Name"),
                            (string)lang.Attribute("SoundStart"),
                            (string)lang.Attribute("SoundBody"),
                            (string)lang.Attribute("SoundEnd")));
                    }

                    //для каждого "Time" указанного через зяпятую, создается копия шаблона (т.е. у кахждого шаблона одно время, циклическое или обычное)
                    var times = (string) act.Attribute("Time");
                    if (!string.IsNullOrEmpty(times))
                    {
                      var deltaTimes = times.Split(',').ToList();
                      foreach (var deltaTime in deltaTimes)
                      {
                          listActs.Add(new ActionTrain(
                              (string)act.Attribute("Id"),
                              (string)act.Attribute("Name"),
                              (string)act.Attribute("Type"),
                              (string)act.Attribute("Priority"),
                              (string)act.Attribute("Repeat"),
                              (string)act.Attribute("Transit"),
                              (string)act.Attribute("Emergency"),
                              deltaTime,
                              listLangs));
                        }      
                    }
                }

                rules.Add(new TrainTypeRule(
                    (string)el.Attribute("Id"),
                    (string)el.Attribute("Type"),
                    (string)el.Attribute("NameRu"),
                    (string)el.Attribute("AliasRu"),
                    (string)el.Attribute("NameEng"),
                    (string)el.Attribute("AliasEng"),
                    (string)el.Attribute("NameCh"),
                    (string)el.Attribute("AliasCh"),
                    (string)el.Attribute("ShowPathTimer"),
                    (string)el.Attribute("WarningTimer"),
                    listActs));
            }

            return rules;
        }

        #endregion

    }
}