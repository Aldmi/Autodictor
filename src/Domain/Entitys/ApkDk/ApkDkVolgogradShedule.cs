using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Domain.Entitys.ApkDk
{
    public class ApkDkVolgogradShedule
    {
        public string Id { get; set; }            //алиас остановочного пункта и код запроса клиента 
        public string Ln { get; set; }            //номер линии расписания
        public TimeSpan ServerTime { get; set; }  //Текущее время сервера
        public string Ntrain { get; set; }        //Номер поезда
        public string Kp { get; set; }            //Категория поезда
        public string StFinish { get; set; }      //Станция назначения
        public string StDeparture { get; set; }   //Станция отправления
        public TimeSpan TmOtpr { get; set; }      //время отправления с запрошенной станции
        public TimeSpan TmPrib { get; set; }      //время прибытия с запрошенной станции
        public DateTime DtOtpr { get; set; }      //дата отправления с запрошенной станции
        public DateTime DtPrib { get; set; }      //дата прибытия с запрошенной станции
        public string Stops { get; set; }         //Тип остановок (Везде, Кроме, На...)
        public string OstFull { get; set; }       //Остановочные станции 	(Остановки: ...)
        public string Put { get; set; }           //Путь
        public string Platf { get; set; }         //Платформа
        public int TmLate { get; set; }           //Время опоздания в мин.





        public static IEnumerable<ApkDkVolgogradShedule> ParseXml2ApkDkschedule(XDocument xDoc)
        {
            var shedules = new List<ApkDkVolgogradShedule>();

            var tablo1 = xDoc.Element("tablo_sys")?.Element("tablo_1");
            if (tablo1 != null)
            {
                string idServer = null;
                var id = tablo1.Element("id")?.Value;
                if (!string.IsNullOrEmpty(id))
                {
                    idServer = Regex.Replace(id, "[\r\n\t]+", "");
                }

                TimeSpan serverTime = TimeSpan.Zero;
                var text = tablo1.Element("text");
                if (text != null)
                {
                    var timeServerStr = new string(Regex.Replace(text.Value, "[\r\n\t]+", "").Take(8).ToArray());
                    TimeSpan.TryParse(timeServerStr, out serverTime);
                }

                var lines = text?.Element("rasplines")?.Element("tab_side")?.Element("lines");
                if (lines != null)
                {
                    foreach (var line in lines.Elements())
                    {
                        var apkDk = new ApkDkVolgogradShedule { Id = idServer, ServerTime = serverTime };

                        var elem = line?.Element("ln")?.Value ?? string.Empty;
                        apkDk.Ln = Regex.Replace(elem, "[\r\n\t]+", "");

                        elem = line?.Element("ntrain")?.Value.Replace("\\", "/") ?? string.Empty;
                        apkDk.Ntrain = Regex.Replace(elem, "[\r\n\t]+", "");

                        elem = line?.Element("kp")?.Value ?? string.Empty;
                        apkDk.Kp = Regex.Replace(elem, "[\r\n\t]+", "");

                        elem = line?.Element("st_finish")?.Value ?? string.Empty;
                        apkDk.StFinish = Regex.Replace(elem, "[\r\n\t]+", "");

                        elem = line?.Element("station_departure")?.Value ?? string.Empty;
                        apkDk.StDeparture = Regex.Replace(elem, "[\r\n\t]+", "");

                        elem = line?.Element("tm_otpr")?.Value ?? string.Empty;
                        elem = Regex.Replace(elem, "[\r\n\t]+", "");
                        TimeSpan tmOtpr;
                        TimeSpan.TryParse(elem, out tmOtpr);
                        apkDk.TmOtpr = tmOtpr;

                        elem = line?.Element("tm_prib")?.Value ?? string.Empty;
                        elem = Regex.Replace(elem, "[\r\n\t]+", "");
                        TimeSpan tmPrib;
                        TimeSpan.TryParse(elem, out tmPrib);
                        apkDk.TmPrib = tmPrib;

                        elem = line?.Element("dt_otpr")?.Value ?? string.Empty;
                        elem = Regex.Replace(elem, "[\r\n\t]+", "");
                        DateTime dtOtpr;
                        DateTime.TryParse(elem, out dtOtpr);
                        apkDk.DtOtpr = dtOtpr;

                        elem = line?.Element("dt_prib")?.Value ?? string.Empty;
                        elem = Regex.Replace(elem, "[\r\n\t]+", "");
                        DateTime dtPrib;
                        DateTime.TryParse(elem, out dtPrib);
                        apkDk.DtPrib = dtPrib;


                        elem = line?.Element("stops")?.Value ?? string.Empty;
                        apkDk.Stops = Regex.Replace(elem, "[\r\n\t]+", "");

                        elem = line?.Element("ost_full")?.Value ?? string.Empty;
                        apkDk.OstFull = Regex.Replace(elem, "[\r\n\t]+", "");

                        elem = line?.Element("put")?.Value ?? string.Empty;
                        apkDk.Put = Regex.Replace(elem, "[\r\n\t]+", "");

                        elem = line?.Element("platf")?.Value ?? string.Empty;
                        apkDk.Platf = Regex.Replace(elem, "[\r\n\t]+", "");

                        elem = line?.Element("tm_late")?.Value ?? string.Empty;
                        elem = Regex.Replace(elem, "[\r\n\t]+", "");
                        int minute;
                        int.TryParse(elem, out minute);
                        apkDk.TmLate = minute;

                        shedules.Add(apkDk);
                    }
                }
            }

            return shedules;
        }

    }
}