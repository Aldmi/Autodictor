using System;


namespace Communication.SibWayApi
{
    public class ItemSibWay
    {
        public string NumberOfTrain { get; set; }
        public string TypeTrain { get; set; }
        public string Route { get; set; }
        public DateTime TimeArrival { get; set; }
        public DateTime TimeDeparture { get; set; }
        public string Path { get; set; }
    }
}