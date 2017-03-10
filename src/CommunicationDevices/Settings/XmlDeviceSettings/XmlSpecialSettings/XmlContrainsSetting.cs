using System.Linq;
using CommunicationDevices.DataProviders;

namespace CommunicationDevices.Settings.XmlDeviceSettings.XmlSpecialSettings
{

    public class XmlContrainsSetting
    {
        #region prop

        public UniversalInputType Contrains { get; }

        #endregion




        #region ctor

        public XmlContrainsSetting(string contrains)
        {
            var contr = contrains.Split(';');
            if (contr.Any())
            {
                Contrains = new UniversalInputType();
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