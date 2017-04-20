using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CommunicationDevices.DataProviders;



namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings
{

    public class XmlConditionsSetting
    {
        #region prop

        public Conditions Conditions { get; }

        #endregion




        #region ctor

        public XmlConditionsSetting(string contrains)
        {
            var contr = contrains.Split(';');
            if (contr.Any())
            {
                Conditions = new Conditions();
                foreach (var s in contr)
                {
                    var matchString = Regex.Match(s, "ПРИГ.+ПУТЬ:(.*)").Groups[1].Value;
                    if (!string.IsNullOrEmpty(matchString))
                    {
                        Conditions.SuburbPaths= new List<string>(matchString.Split(','));
                        continue;
                    }

                    matchString = Regex.Match(s, "ДАЛЬН.+ПУТЬ:(.*)").Groups[1].Value;
                    if (!string.IsNullOrEmpty(matchString))
                    {
                        Conditions.LongDistancePaths = new List<string>(matchString.Split(','));
                        continue;
                    }

                    matchString = Regex.Match(s, "ПРИБ.+ПУТЬ:(.*)").Groups[1].Value;
                    if (!string.IsNullOrEmpty(matchString))
                    {
                        Conditions.ArrivalPaths = new List<string>(matchString.Split(','));
                        continue;
                    }

                    matchString = Regex.Match(s, "ОТПР.+ПУТЬ:(.*)").Groups[1].Value;
                    if (!string.IsNullOrEmpty(matchString))
                    {
                        Conditions.DeparturePaths = new List<string>(matchString.Split(','));
                        continue;
                    }

                    switch (s)
                    {
                        case "ПРИБ.":
                            Conditions.Event = s;
                            break;

                        case "ОТПР.":
                            Conditions.Event = s;
                            break;

                        case "ПРИГ.":
                            Conditions.TypeTrain = TypeTrain.Suburb;
                            break;

                        case "ДАЛЬН.":
                            Conditions.TypeTrain = TypeTrain.LongDistance;
                            break;

                        case "ПРИБ.+ПРИГ.":
                            Conditions.ArrivalAndSuburb = true;
                            break;

                        case "ПРИБ.+ДАЛЬН.":
                            Conditions.ArrivalAndLongDistance = true;
                            break;

                        case "ОТПР.+ПРИГ.":
                            Conditions.DepartureAndSuburb = true;
                            break;

                        case "ОТПР.+ДАЛЬН.":
                            Conditions.DepartureAndLongDistance = true;
                            break;

                        case "МеньшеТекВремени":
                            Conditions.LowCurrentTime = true;
                            break;

                        case "БольшеТекВремени":
                            Conditions.HightCurrentTime = true;
                            break;

                        default:
                            Conditions = null;
                            return;
                    }
                }
            }
        }

        #endregion
    }
}