namespace CommunicationDevices.Behavior.ExchangeRules
{
    public class BaseExchangeRule
    {
        #region prop

        public string Format { get; set; }

        public RequestRule RequestRule { get; set; }
        public ResponseRule ResponseRule { get; set; }
        public RepeatRule RepeatRule { get; set; }

        #endregion





        #region ctor

        public BaseExchangeRule(RequestRule requestRule, ResponseRule responseRule, RepeatRule repeatRule, string format)
        {
            RequestRule = requestRule;
            ResponseRule = responseRule;
            RepeatRule = repeatRule;
            Format = format;
        }

        #endregion
    }





    public class RequestRule
    {
        public int? MaxLenght { get; set; }
        public string Body { get; set; }
    }


    public class ResponseRule
    {
        public int? MaxLenght { get; set; }
        public string Body { get; set; }
        public int Time { get; set; }
    }


    public class RepeatRule
    {
        public int Count { get; set; }
        public int? DeltaX { get; set; }
        public int? DeltaY { get; set; }
    }

}