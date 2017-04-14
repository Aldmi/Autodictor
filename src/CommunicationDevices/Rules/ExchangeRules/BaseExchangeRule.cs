using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using CommunicationDevices.DataProviders;


namespace CommunicationDevices.Rules.ExchangeRules
{
    public class MainRule
    {
        public List<BaseExchangeRule> ExchangeRules { get; set; }
        public ViewType ViewType { get; set; }
    }



    public class ViewType
    {
        public string Type { get; set; }

        public int? TableSize { get; set; }          //выставляется если Type == Table
    }



    public class BaseExchangeRule
    {
        #region prop

        public string Format { get; set; }
        public string Condition { get; set; }

        public RequestRule RequestRule { get; set; }
        public ResponseRule ResponseRule { get; set; }
        public RepeatRule RepeatRule { get; set; }

        #endregion




        #region ctor

        public BaseExchangeRule(RequestRule requestRule, ResponseRule responseRule, RepeatRule repeatRule, string format, string condition)
        {
            RequestRule = requestRule;
            ResponseRule = responseRule;
            RepeatRule = repeatRule;
            Format = format;
            Condition = condition;
        }

        #endregion




        public bool IsEnableTableRule(int rowNumber, int tableLenght)
        {
            if (string.IsNullOrEmpty(Condition))
                return true;


            switch (Condition)
            {
                case "rowNumber LowOrEqual Table.Lenght": return rowNumber <= tableLenght;

                case "rowNumber Low Table.Lenght": return rowNumber < tableLenght;

                case "rowNumber Hight Table.Lenght": return rowNumber > tableLenght;

                case "rowNumber HightOrEqual Table.Lenght": return rowNumber >= tableLenght;

                default:
                    return false;
            }
        }
    }




    public class RequestRule
    {
        public int? MaxLenght { get; set; }
        public string Body { get; set; }


        #region Method

        public virtual string GetFillBody(UniversalInputType uit, byte? currentRow)
        {
            if (Body.Contains("}"))                                                           //если указанны переменные подстановки
            {
                var subStr = Body.Split('}');
                StringBuilder resStr = new StringBuilder();
                int parseVal;
                foreach (var s in subStr)
                {
                    var replaseStr = (s.Contains("{")) ? (s + "}") : s;
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
                            else
                            {
                                var formatStr = string.Format(replaseStr.Replace(nameof(uit.NumberOfTrain), "0"), " ");
                                resStr.Append(formatStr);
                            }
                        }
                        else
                        {
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.NumberOfTrain), "0"), string.IsNullOrEmpty(uit.NumberOfTrain) ? " " : uit.NumberOfTrain);
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
                            else
                            {
                                var formatStr = string.Format(replaseStr.Replace(nameof(uit.NumberOfTrain), "0"), " ");
                                resStr.Append(formatStr);
                            }
                        }
                        else
                        {
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.PathNumber), "0"), string.IsNullOrEmpty(uit.PathNumber) ? " " : uit.PathNumber);
                            resStr.Append(formatStr);
                        }
                        continue;
                    }


                    if (replaseStr.Contains(nameof(uit.Event)))
                    {
                        var formatStr = string.Format(replaseStr.Replace(nameof(uit.Event), "0"), string.IsNullOrEmpty(uit.Event) ? " " : uit.Event);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (replaseStr.Contains(nameof(uit.Stations)))
                    {
                        var formatStr = string.Format(replaseStr.Replace(nameof(uit.Stations), "0"), string.IsNullOrEmpty(uit.Stations) ? " " : uit.Stations);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (replaseStr.Contains(nameof(uit.Note)))
                    {
                        var formatStr = string.Format(replaseStr.Replace(nameof(uit.Note), "0"), string.IsNullOrEmpty(uit.Note) ? " " : uit.Note);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (replaseStr.Contains(nameof(uit.DaysFollowing)))
                    {
                        var formatStr = string.Format(replaseStr.Replace(nameof(uit.DaysFollowing), "0"), string.IsNullOrEmpty(uit.DaysFollowing) ? " " : uit.DaysFollowing);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (replaseStr.Contains(nameof(uit.Time)))
                    {
                        if (replaseStr.Contains(":")) //если указзанн формат времени
                        {
                            var dateFormat = s.Split(':')[1]; //без закр. скобки
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.Time), "0"), (uit.Time == DateTime.MinValue) ? " " : uit.Time.ToString(dateFormat));
                            resStr.Append(formatStr);
                        }
                        else
                        {
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.Time), "0"), (uit.Time == DateTime.MinValue) ? " " : uit.Time.ToString(CultureInfo.InvariantCulture));
                            resStr.Append(formatStr);
                        }
                        continue;
                    }


                    if (replaseStr.Contains("Hour"))
                    {
                        var formatStr = string.Format(replaseStr.Replace("Hour", "0"), DateTime.Now.Hour);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (replaseStr.Contains("Minute"))
                    {
                        var formatStr = string.Format(replaseStr.Replace("Minute", "0"), DateTime.Now.Minute);
                        resStr.Append(formatStr);
                        continue;
                    }


                    if (replaseStr.Contains("Second"))
                    {
                        var formatStr = string.Format(replaseStr.Replace("Second", "0"), DateTime.Now.Second);
                        resStr.Append(formatStr);
                        continue;
                    }

                    if (replaseStr.Contains("rowNumber"))
                    {
                        if (currentRow.HasValue)
                        {
                            var formatStr = CalculateMathematicFormat(replaseStr, currentRow.Value);
                            resStr.Append(formatStr);
                            continue;
                        }
                    }


                    //Добавим в неизменном виде спецификаторы байтовой информации.
                    resStr.Append(replaseStr);
                }

                var testStr = resStr.ToString();                 //DEBUG

                return resStr.ToString();
            }

            return Body;
        }


        public static string CalculateMathematicFormat(string str, int row)
        {
            var matchString = Regex.Match(str, "\\{\\((.*)\\)\\:(.*)\\}").Groups[1].Value;

            var calc = new Sprache.Calc.XtensibleCalculator();
            var expr = calc.ParseExpression(matchString, rowNumber => row);
            var func = expr.Compile();
            var arithmeticResult = (int)func();

            var reultStr = str.Replace("(" + matchString + ")", "0");
            reultStr = String.Format(reultStr, arithmeticResult);

            return reultStr;
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