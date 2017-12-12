using System;
using System.Collections.Generic;
using System.IO;
using AutodictorBL.Settings;
using AutodictorBL.Settings.XmlSettings;
using Library.Xml;

namespace AutodictorBL.Rules.TrainRecordRules
{
    /// <summary>
    /// Все правила для создания поезда
    /// </summary>
    public class TrainRules : ITrainTypeRule
    {
        #region prop

        public IEnumerable<TrainTypeRule> TrainTypeRules { get; set; }

        #endregion





        #region Methode

        public string LoadSetting()
        {
            //ЗАГРУЗКА НАСТРОЕК------------------------------------------------------------------------------------------------------------------
            try
            {
                var xmlFile = XmlWorker.LoadXmlFile("Config", "DynamicSound.xml"); //все настройки в одном файле
                if (xmlFile == null)
                    return "DynamicSound.xml не загружен";

                TrainTypeRules= XmlSettingFactory.CreateXmlTrainTypeRules(xmlFile);
                return null;
            }
            catch (FileNotFoundException ex)
            {
                //Log.log.Error(ErrorString);
                return "DynamicSound.xml не найденн";
            }
            catch (Exception ex)
            {
                //Log.log.Error(ErrorString);
                return $"DynamicSound.xml ОШИБКА в узлах дерева XML файла настроек: {ex}";
            }
        }

        #endregion
    }
}