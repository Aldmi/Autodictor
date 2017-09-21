﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Settings;


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

        public int? TableSize { get; set; }                   // Размер таблицы. выставляется если Type == Table
        public int? FirstTableElement { get; set; }          //Номер стартового элемента. выставляется если Type == Table
    }



    public class BaseExchangeRule
    {
        #region prop

        public string Format { get; set; }
        public Conditions Resolution { get; set; }

        public RequestRule RequestRule { get; set; }
        public ResponseRule ResponseRule { get; set; }
        public RepeatRule RepeatRule { get; set; }

        #endregion




        #region ctor

        public BaseExchangeRule(RequestRule requestRule, ResponseRule responseRule, RepeatRule repeatRule, string format, Conditions resolution)
        {
            RequestRule = requestRule;
            ResponseRule = responseRule;
            RepeatRule = repeatRule;
            Format = format;
            Resolution = resolution;
        }

        #endregion





        #region Methode

        /// <summary>
        /// Проверка условий разрешения выполнения правила.
        /// </summary>
        public bool CheckResolution(UniversalInputType inData)
        {
            if (Resolution == null)
                return true;

            return Resolution.CheckResolutions(inData);  //инверсия ограничения 
        }

        #endregion

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


                    if (replaseStr.Contains(nameof(uit.Addition)))
                    {
                        var formatStr = string.Format(replaseStr.Replace(nameof(uit.Addition), "0"), uit.Addition);
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


                    if (replaseStr.Contains(nameof(uit.DelayTime)))
                    {
                        if (uit.DelayTime == null)
                            continue;

                        if (replaseStr.Contains(":")) //если указзанн формат времени
                        {
                            var dateFormat = s.Split(':')[1]; //без закр. скобки
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.DelayTime), "0"), (uit.DelayTime == DateTime.MinValue) ? " " : uit.DelayTime.Value.ToString(dateFormat));
                            resStr.Append(formatStr);
                        }
                        else                         //вывод в минутах
                        {
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.DelayTime), "0"), (uit.DelayTime == DateTime.MinValue) ? " " : ((uit.DelayTime.Value.Hour * 60) + (uit.DelayTime.Value.Minute)).ToString());
                            resStr.Append(formatStr);
                        }
                        continue;
                    }


                    if (replaseStr.Contains(nameof(uit.ExpectedTime)))
                    {
                        if (replaseStr.Contains(":")) //если указзанн формат времени
                        {
                            var dateFormat = s.Split(':')[1]; //без закр. скобки
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.ExpectedTime), "0"), (uit.ExpectedTime == DateTime.MinValue) ? " " : uit.ExpectedTime.ToString(dateFormat));
                            resStr.Append(formatStr);
                        }
                        else
                        {
                            var formatStr = string.Format(replaseStr.Replace(nameof(uit.ExpectedTime), "0"), (uit.ExpectedTime == DateTime.MinValue) ? " " : uit.ExpectedTime.ToString(CultureInfo.InvariantCulture));
                            resStr.Append(formatStr);
                        }
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


                    if (replaseStr.Contains("TDepart"))
                    {
                        var timeDepart = (uit.TransitTime != null && uit.TransitTime.ContainsKey("приб")) ? uit.TransitTime["приб"] : DateTime.MinValue;
                        if (replaseStr.Contains(":")) //если указзанн формат времени
                        {
                            var dateFormat = s.Split(':')[1]; //без закр. скобки
                            var formatStr = string.Format(replaseStr.Replace("TDepart", "0"), (timeDepart == DateTime.MinValue) ? " " : timeDepart.ToString(dateFormat));
                            resStr.Append(formatStr);
                        }
                        else
                        {
                            var formatStr = string.Format(replaseStr.Replace("TDepart", "0"), (timeDepart == DateTime.MinValue) ? " " : timeDepart.ToString(CultureInfo.InvariantCulture));
                            resStr.Append(formatStr);
                        }
                        continue;
                    }


                    if (replaseStr.Contains("TArrival"))
                    {
                        var timeDepart = (uit.TransitTime != null && uit.TransitTime.ContainsKey("отпр")) ? uit.TransitTime["отпр"] : DateTime.MinValue;
                        if (replaseStr.Contains(":")) //если указзанн формат времени
                        {
                            var dateFormat = s.Split(':')[1]; //без закр. скобки
                            var formatStr = string.Format(replaseStr.Replace("TArrival", "0"), (timeDepart == DateTime.MinValue) ? " " : timeDepart.ToString(dateFormat));
                            resStr.Append(formatStr);
                        }
                        else
                        {
                            var formatStr = string.Format(replaseStr.Replace("TArrival", "0"), (timeDepart == DateTime.MinValue) ? " " : timeDepart.ToString(CultureInfo.InvariantCulture));
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