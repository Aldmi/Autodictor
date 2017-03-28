using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CommunicationDevices.DataProviders;


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


        #region Method

        public virtual string GetFillBody(UniversalInputType uit)
        {
            if (Body.Contains("}"))                                                           //если указанны переменные подстановки
            {
                var subStr = Body.Split('}');
                StringBuilder resStr = new StringBuilder();
                int parseVal;
                foreach (var s in subStr)
                {
                    var replaseStr = (s.Contains("{")) ?  (s + "}") : s;
                    if (replaseStr.Contains(nameof(uit.AddressDevice)))
                    {
                        if (replaseStr.Contains(":")) //если указзанн формат числа
                        {
                            if (int.TryParse(uit.AddressDevice, out parseVal))
                            {
                                var formatStr = string.Format(replaseStr.Replace(nameof(uit.AddressDevice), "0"), parseVal);
                                resStr.Append(formatStr);
                            }
                        }
                        else
                        {
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.AddressDevice), "0"), uit.AddressDevice);
                            resStr.Append(formatStr);
                        }
                        continue;
                    }


                    if (replaseStr.Contains(nameof(uit.TypeTrain)))
                    {
                        var formatStr = string.Format(replaseStr.Replace(nameof(uit.TypeTrain), "0"), uit.TypeTrain);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (replaseStr.Contains(nameof(uit.NumberOfTrain)))
                    {
                        if (replaseStr.Contains(":")) //если указзанн формат числа
                        {
                            if (int.TryParse(uit.NumberOfTrain, out parseVal))
                            {
                                var formatStr = string.Format(replaseStr.Replace(nameof(uit.NumberOfTrain), "0"), parseVal);
                                resStr.Append(formatStr);
                            }
                        }
                        else
                        {
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.NumberOfTrain), "0"), uit.NumberOfTrain);
                            resStr.Append(formatStr);
                        }
                        continue;
                    }


                    if (replaseStr.Contains(nameof(uit.PathNumber)))
                    {
                        if (replaseStr.Contains(":")) //если указзанн формат числа
                        {
                            if (int.TryParse(uit.PathNumber, out parseVal))
                            {
                                var formatStr = string.Format(replaseStr.Replace(nameof(uit.PathNumber), "0"), parseVal);
                                resStr.Append(formatStr);
                            }
                        }
                        else
                        {
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.PathNumber), "0"), uit.PathNumber);
                            resStr.Append(formatStr);
                        }
                        continue;
                    }


                    if (replaseStr.Contains(nameof(uit.Event)))
                    {
                        var formatStr = string.Format(replaseStr.Replace(nameof(uit.Event), "0"), uit.Event);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (replaseStr.Contains(nameof(uit.Stations)))
                    {
                        var formatStr = string.Format(replaseStr.Replace(nameof(uit.Stations), "0"), uit.Stations);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (replaseStr.Contains(nameof(uit.Note)))
                    {
                        var formatStr = string.Format(replaseStr.Replace(nameof(uit.Note), "0"), uit.Note);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (replaseStr.Contains(nameof(uit.DaysFollowing)))
                    {
                        var formatStr = string.Format(replaseStr.Replace(nameof(uit.DaysFollowing), "0"), uit.DaysFollowing);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (replaseStr.Contains(nameof(uit.Time)))
                    {
                        if (replaseStr.Contains(":")) //если указзанн формат времени
                        {
                            var dateFormat = s.Split(':')[1]; //без закр. скобки
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.Time), "0"), uit.Time.ToString(dateFormat));
                            resStr.Append(formatStr);
                        }
                        else
                        {
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.Time), "0"), uit.Time);
                            resStr.Append(formatStr);
                        }
                        continue;
                    }


                    //Добавим в неизменном виде спецификаторы байтовой информации.
                    resStr.Append(replaseStr);
                }

                var testStr = resStr.ToString();                 //DEBUG

                return resStr.ToString();
            }

            return Body;
        }

        #endregion
    }


    public class ResponseRule : RequestRule
    {
        public int Time { get; set; }
    }


    public class RepeatRule
    {
        public int Count { get; set; }
        public int? DeltaX { get; set; }
        public int? DeltaY { get; set; }
    }

}