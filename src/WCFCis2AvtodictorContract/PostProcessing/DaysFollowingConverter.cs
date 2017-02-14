using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace WCFCis2AvtodictorContract.PostProcessing
{
    /// <summary>
    /// указанные дни в строке спецификации
    /// в формате пар ключ-значние, где
    /// ключ- номер месяца
    /// значение- список дней
    /// </summary>
    public struct DaysSpecification
    {
        public Dictionary<byte, List<byte>> IncludingDays;        // включая дни
        public Dictionary<byte, List<byte>> ExcludingDays;        // исключая дни
    }





   public class DaysFollowingConverter
    {
        public IEnumerable<string> DaysFollowingCis { get; }                           //входная коллекция
        private List<string> DaysFollowingAutodictor { get; }= new List<string>();     //выходная коллекция

       public Dictionary<byte, string> MathingNameMonth { get; }= new Dictionary<byte, string>
       {
           {1, "Январь"},
           {2, "Февраль"},
           {3, "Март"},
           {4, "Апрель"},
           {5, "Май"},
           {6, "Июнь"},
           {7, "Июль"},
           {8, "Август"},
           {9, "Сентябрь"},
           {10, "Октябрь"},
           {11, "Ноябрь"},
           {12, "Декабрь"},
       }; 



        public DaysFollowingConverter(IEnumerable<string> daysFollowingCis )
        {
            DaysFollowingCis = daysFollowingCis;
        }




       public async Task<IList<string>> Convert()               //TODO: DaysFollowingCis передавать напрямую
        {
            //foreach (var days in DaysFollowingCis)
            //{
            //    DaysFollowingAutodictor.Add(ConvertDays(days));
            //}

            //DRBUG------------------------------------------------
            var inStr =
                  "Тип: \"ОН\"   Дни: \"Включая: 26.12, 22.12, 03.12, 29.12, 02.01, 04.01, 05.01; Кроме: 07.01, 09.03\"";

             // "Тип: \"ОН\"   Дни: \"Кроме: 23.12, 25.12, 26.12, 29.12, 30.12, 01.01, 06.01, 08.01, 13.01, 15.01, 20.01, 22.01, 27.01, 29.01, 03.02, 05.02, 10.02, 12.02, 17.02\"";


             DaysFollowingAutodictor.Add(await ConvertDays(inStr));
            //DRBUG------------------------------------------------

            return DaysFollowingAutodictor;
        }



        private async Task<string> ConvertDays(string days)
        {
           return await Task<string>.Factory.StartNew(() =>
           {
               //парсим информацию между кавычками
               Regex regex = new Regex("\"[^\"]*\"", RegexOptions.IgnoreCase);
               MatchCollection matches = regex.Matches(days);

               string type = null;
               string specification = null;
               if (matches.Count == 2)
               {
                   type = matches[0].Value.Trim(new[] { '"' });
                   specification = matches[1].Value.Trim(new[] { '"' });
               }
               else
               {
                   return "НЕ КОРРЕКТНОЕ ПРЕОБРАЗОВАНИЕ";
               }

               var daysSpecification = new DaysSpecification();

               if (!string.IsNullOrEmpty(specification))
               {
                   var indexInclude = specification.IndexOf("Включая:", StringComparison.Ordinal);
                   var indexExclude = specification.IndexOf("Кроме:", StringComparison.Ordinal);

                   string strInclude = null;
                   string strExclude = null;
                   if (indexInclude >= 0 && indexExclude >= 0)
                   {
                       //вначале строки секция Кроме:.
                       if (indexExclude < indexInclude)
                       {
                           var indexSemicolon = specification.IndexOf(";", StringComparison.Ordinal);
                           if (indexSemicolon >= 0)
                           {
                               strExclude = specification.Substring((indexExclude + "Кроме:".Length), (indexSemicolon - "Кроме:".Length));
                               strInclude = specification.Substring((indexInclude + "Включая:".Length));
                           }
                       }

                       //вначале строки секция Включая:.
                       if (indexInclude < indexExclude)
                       {
                           var indexSemicolon = specification.IndexOf(";", StringComparison.Ordinal);
                           if (indexSemicolon >= 0)
                           {
                               strInclude = specification.Substring((indexInclude + "Включая:".Length), (indexSemicolon - "Включая:".Length));
                               strExclude = specification.Substring((indexExclude + "Кроме:".Length));
                           }
                       }
                   }
                   else
                   if (indexInclude >= 0)
                   {
                       strInclude = specification.Substring(indexInclude + "Включая:".Length);
                   }
                   else
                   if (indexExclude >= 0)
                   {
                       strExclude = specification.Substring(indexExclude + "Кроме:".Length);
                   }


                   if (!string.IsNullOrEmpty(strInclude))
                   {
                       daysSpecification.IncludingDays=new Dictionary<byte, List<byte>>();
                       var dateCollection = strInclude.Split(',');
                       foreach (var date in dateCollection)
                       {
                           var monthDay = date.Split('.');
                           if (monthDay.Length == 2)
                           {
                               byte key= Byte.Parse(monthDay[1]);
                               byte value= Byte.Parse(monthDay[0]);

                               if (!daysSpecification.IncludingDays.ContainsKey(key))
                                   daysSpecification.IncludingDays[key] = new List<byte>();
                             
                               daysSpecification.IncludingDays[key].Add(value);                
                           }
                       }
                   }

                   if (!string.IsNullOrEmpty(strExclude))
                   {
                       daysSpecification.ExcludingDays = new Dictionary<byte, List<byte>>();
                       var dateCollection = strExclude.Split(',');
                       foreach (var date in dateCollection)
                       {
                           var monthDay = date.Split('.');
                           if (monthDay.Length == 2)
                           {
                               byte key = Byte.Parse(monthDay[1]);
                               byte value = Byte.Parse(monthDay[0]);

                               if (!daysSpecification.ExcludingDays.ContainsKey(key))
                                   daysSpecification.ExcludingDays[key] = new List<byte>();

                               daysSpecification.ExcludingDays[key].Add(value);
                           }
                       }
                   }
               }


               string result = null;
               switch (type)
               {
                   case "ОН":
                       result= ONHandler(daysSpecification);
                       break;

                   case "ЧЕТН":
                       result = EvenHandler(daysSpecification);
                       break;

                   case "НЕЧЕТН":
                       //result = OddHandler(daysSpecification);
                       break;

                   case "Еж.":
                       //result = EverydayHandler(daysSpecification);
                       break;

                   case "В":
                       result = EvenHandler(daysSpecification);
                       break;

                   case "Р":
                       result = EvenHandler(daysSpecification);
                       break;

                   case "ПТ":
                       result = EvenHandler(daysSpecification);
                       break;

                   case "ПТ ВСК":
                       result = EvenHandler(daysSpecification);
                       break;

                   case "ПТ СБ":
                       result = EvenHandler(daysSpecification);
                       break;

                   case "ПТ СБ ВСК":
                       result = EvenHandler(daysSpecification);
                       break;

                   case "ЧТ":
                       result = EvenHandler(daysSpecification);
                       break;

                   case "ЧТ ВСК":
                       result = EvenHandler(daysSpecification);
                       break;

                   default:
                       result = "НЕ КОРРЕКТНОЕ ПРЕОБРАЗОВАНИЕ";
                       break;
               }


               return result;
           });



        }



        /// <summary>
        /// Обработчик дней особого назначения.
        /// В секции "включая" находятся выбранные дни в году.
        /// </summary>
        private string ONHandler(DaysSpecification daysSpecification)
        {
            //"Тип: \"ОН\"   Дни: \"Включая: 26.12, 22.12, 03.12, 29.12, 02.01, 04.01, 05.01; Кроме: 07.01, 09.03\"";

            if ((daysSpecification.IncludingDays == null) || (!daysSpecification.IncludingDays.Any()))
            {
                return "НЕ КОРРЕКТНОЕ ПРЕОБРАЗОВАНИЕ";
            }

            var year = DateTime.Now.Year;

            string sumStr= "Режим работы: Выборочные дни: ";                                 //TODO: заменить string на StringBuilder


            foreach (var key in daysSpecification.IncludingDays.Keys)
            {
                sumStr += MathingNameMonth[key] + ":";                //Название месяца.
                foreach (var day in daysSpecification.IncludingDays[key])
                {
                    sumStr += day + ",";
                }
            }



            //foreach (var kvp in daysSpecification.IncludingDays)
            //{
            //    var month = int.Parse(kvp.Key);

            //    //foreach (var VARIABLE in sumStr)
            //    //{

            //    //}

            //    var day = int.Parse(kvp.Value);
            //    var currentDay = new DateTime(year, month, day);

            //    sumStr += currentDay.DayOfWeek;
            //}




            //var year = DateTime.Now.Year;
            //var month = 2;
            //var startDay = new DateTime(year, month, 1);
            //var endDay = startDay.AddMonths(1);
            //for (var date = startDay; date != endDay; date = date.AddDays(1))
            //{

            //    Console.WriteLine($"Number: {date.Day}, day of week: {date.DayOfWeek}   ");
            //}



            return sumStr;
        }




        /// <summary>
        /// Обработчик четных дней
        /// </summary>
        private string EvenHandler(DaysSpecification daysSpecification)
        {
            //все четные дни года
            if (!daysSpecification.ExcludingDays.Any())
            {
                return "Режим работы: По четным дням";
            }


            //Пробегаем по всем месяцам календаря выбираем четные дни, если номера дня нету в списке ExcludingDays
            //для этого месяца и формируем строку для каждого месяца "{Название_месяца}: " + Номер дня + ","
            

            // Режим работы: Выборочные дни: Январь: 2,4,6,8,10,12,14,16,18,20,22,24,26,28,30,Февраль: 1,3,5,7,9,11,13,15,17,19,21,23,25,27,29,31,Август: 1,3,5,7,9,11,13,15,17,19,21,23,25,27,29,31,Сентябрь: 2,4,6,8,10,12,14,16,18,20,22,24,26,28,30,Октябрь: 2,4,6,8,10,12,14,16,18,20,22,24,26,28,30,Ноябрь: 2,4,6,8,10,13,14,16,18,20,22,24,26,28,30,Декабрь: 4,11,18,25


            return "  ";
        }
    }
}