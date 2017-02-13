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
    /// ключ- номер дня
    /// значение- номер месяца
    /// </summary>
    public struct DaysSpecification
    {
        public List<KeyValuePair<string, string>> IncludingDays;        // включая дни
        public List<KeyValuePair<string, string>> ExcludingDays;        // исключая дни
    }



   public class DaysFollowingConverter
    {
        public IEnumerable<string> DaysFollowingCis { get; }                           //входная коллекция
        private List<string> DaysFollowingAutodictor { get; }= new List<string>();     //выходная коллекция





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
                "Тип: \"ОН\"   Дни: \"Включая: 26.12, 29.12, 02.01, 04.01, 05.01; Кроме: 07.01, 09.03\"";

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
                       daysSpecification.IncludingDays = new List<KeyValuePair<string, string>>();
                       var dateCollection = strInclude.Split(',');
                       foreach (var date in dateCollection)
                       {
                           var monthDay = date.Split('.');
                           if (monthDay.Length == 2)
                           {
                               daysSpecification.IncludingDays.Add(new KeyValuePair<string, string>(monthDay[0], monthDay[1]));
                           }
                       }
                   }

                   if (!string.IsNullOrEmpty(strExclude))
                   {
                       daysSpecification.ExcludingDays = new List<KeyValuePair<string, string>>();
                       var dateCollection = strExclude.Split(',');
                       foreach (var date in dateCollection)
                       {
                           var monthDay = date.Split('.');
                           if (monthDay.Length == 2)
                           {
                               daysSpecification.ExcludingDays.Add(new KeyValuePair<string, string>(monthDay[0], monthDay[1]));
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

            return "  ";
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