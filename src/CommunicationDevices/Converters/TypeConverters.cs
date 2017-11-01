using CommunicationDevices.DataProviders;

namespace CommunicationDevices.Converters
{
    internal static class TypeConverters
    {
        public static string TypeTrainEnum2RusString(TypeTrain typeTrain)
        {
            switch (typeTrain)
            {
                case TypeTrain.None:
                    return "Не определен";

                case TypeTrain.Passenger:
                    return "Пассажирский";

                case TypeTrain.Suburban:
                    return "Пригородный";

                case TypeTrain.Corporate:
                    return "Фирменный";

                case TypeTrain.Express:
                    return "Скорый";

                case TypeTrain.HighSpeed:
                    return "Скоростной";

                case TypeTrain.Swallow:
                    return "Ласточка";

                case TypeTrain.Rex:
                    return "РЭКС";
            }

            return string.Empty;
        }
    }
}