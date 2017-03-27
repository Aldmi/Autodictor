using System.Linq;
using CommunicationDevices.DataProviders;



namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings
{

    public class XmlContrainsSetting
    {
        #region prop

        public Contrains Contrains { get; }

        #endregion




        #region ctor

        public XmlContrainsSetting(string contrains)
        {
            var contr = contrains.Split(';');
            if (contr.Any())
            {
                Contrains = new Contrains();
                foreach (var s in contr)
                {
                    switch (s)
                    {
                        case "ПРИБ.":
                            Contrains.Event = s;
                            break;

                        case "ОТПР.":
                            Contrains.Event = s;
                            break;

                        case "ПРИГ.":
                            Contrains.TypeTrain = TypeTrain.Suburb;
                            break;

                        case "ДАЛЬН.":
                            Contrains.TypeTrain = TypeTrain.LongDistance;
                            break;

                        case "ПРИБ.+ПРИГ.":
                            Contrains.ArrivalAndSuburb = true;
                            break;

                        case "ПРИБ.+ДАЛЬН.":
                            Contrains.ArrivalAndLongDistance = true;
                            break;

                        case "ОТПР.+ПРИГ.":
                            Contrains.DepartureAndSuburb = true;
                            break;

                        case "ОТПР.+ДАЛЬН.":
                            Contrains.DepartureAndLongDistance = true;
                            break;

                        case "МеньшеТекВремени":
                            Contrains.LowCurrentTime = true;
                            break;

                        case "БольшеТекВремени":
                            Contrains.HightCurrentTime = true;
                            break;

                        default:
                            Contrains = null;
                            return;
                    }
                }
            }
        }

        #endregion
    }
}