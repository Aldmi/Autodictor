using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;
using ConsoleApplication_test.Entitys;
using Domain.Abstract;
using Domain.Concrete;
using Library.Logs;
using Library.Xml;


namespace MainExample
{
    public enum NotificationLanguage {Ru, Eng };

    static class Program
    {
        static Mutex m_mutex;

        public static List<string> FilesFolder = null;
        public static List<string> TrainNumbersFolder = null;
        public static List<string> СписокСтатическихСообщений = null;
        public static List<string> СписокДинамическихСообщений = null;
        public static List<string> НомераПутей = new List<string>();
        public static List<string> НомераПоездов = new List<string>();

        public static string ИнфСтрокаНаТабло = "";
        public static Dictionary<string, string> Станции = new Dictionary<string, string>();  //Key - название на RU, Value - название на ENG

        private static IRepository<Direction> DirectionRepository; //хранилище XML

        public static byte ПолучитьНомерПути(string НомерПути)
        {
            if (НомераПутей.Contains(НомерПути))
                return (byte)(НомераПутей.IndexOf(НомерПути) + 1);

            return 0;
        }

        public static _Настройки Настройки;

        public static string[] ТипыОповещения = new string[] { "Не определено", "На Х-ый путь", "На Х-ом пути", "С Х-ого пути" };
        public static string[] ТипыВремени = new string[] { "Прибытие", "Отправление" };

        public static List<string> ШаблоныОповещения = new List<string>();

        public static string[] ШаблонОповещенияОбОтменеПоезда = new string[] { "", "Отмена пассажирского поезда", "Отмена пригородного электропоезда", "Отмена фирменного поезда", "Отмена скорого поезда", "Отмена скоростного поезда", "Отмена ласточки", "Отмена РЭКСа" };
        public static string[] ШаблонОповещенияОЗадержкеПрибытияПоезда = new string[] { "", "Задержка прибытия пассажирского поезда", "Задержка прибытия пригородного электропоезда", "Задержка прибытия фирменного поезда", "Задержка прибытия скорого поезда", "Задержка прибытия скоростного поезда", "Задержка прибытия ласточки", "Задержка прибытия РЭКСа" };
        public static string[] ШаблонОповещенияОЗадержкеОтправленияПоезда = new string[] { "", "Задержка отправления пассажирского поезда", "Задержка отправления пригородного электропоезда", "Задержка отправления фирменного поезда", "Задержка отправления скорого поезда", "Задержка отправления скоростного поезда", "Задержка отправления ласточки", "Задержка отправления РЭКСа" };
        public static string[] ШаблонОповещенияООтправлениеПоГотовностиПоезда = new string[] { "", "Отправление по готовности пассажирского поезда", "Отправление по готовности пригородного электропоезда", "Отправление по готовности фирменного поезда", "Отправление по готовности скорого поезда", "Отправление по готовности скоростного поезда", "Отправление по готовности ласточки", "Отправление по готовности РЭКСа" };




        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (InstanceExists())
                return;

            ЗагрузкаНазванийПутей();
            ЗагрузкаНазванийПоездов();
            ОкноНастроек.ЗагрузитьНастройки();


            try
            {
                var dir = new DirectoryInfo(Application.StartupPath + @"\Wav\Sounds\");
                FilesFolder = new List<string>();
                foreach (FileInfo file in dir.GetFiles("*.wav"))
                    FilesFolder.Add(Path.GetFileNameWithoutExtension(file.FullName));

                dir = new DirectoryInfo(Application.StartupPath + @"\Wav\Number of trains\");
                TrainNumbersFolder = new List<string>();
                foreach (FileInfo file in dir.GetFiles("*.wav"))
                    TrainNumbersFolder.Add(Path.GetFileNameWithoutExtension(file.FullName));

                dir = new DirectoryInfo(Application.StartupPath + @"\Wav\Static message\");
                СписокСтатическихСообщений = new List<string>();
                foreach (FileInfo file in dir.GetFiles("*.wav"))
                    СписокСтатическихСообщений.Add(Path.GetFileNameWithoutExtension(file.FullName));

                dir = new DirectoryInfo(Application.StartupPath + @"\Wav\Dynamic message\");
                СписокДинамическихСообщений = new List<string>();
                foreach (FileInfo file in dir.GetFiles("*.wav"))
                    СписокДинамическихСообщений.Add(Path.GetFileNameWithoutExtension(file.FullName));
            }
            catch (Exception ex) { };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //ОБРАБОТКА НЕ ПЕРЕХВАЧЕННЫХ ИСКЛЮЧЕНИЙ
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.Run(new MainForm());
        }


        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Log.log.Fatal($"Исключение из не UI потока {e.Exception.Message}");
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.log.Fatal($"Исключение основного UI потока {(e.ExceptionObject as Exception)?.Message}");
        }


        static bool InstanceExists()
        {
            bool createdNew;
            m_mutex = new Mutex(false, "AutodictorOneInstanceApplication", out createdNew);
            return (!createdNew);
        }

        public static string ByteArrayToHexString(byte[] data, int begin, int count)
        {
            int i;
            StringBuilder sb = new StringBuilder(count * 3);
            for (i = 0; i < count; i++)
                sb.Append(Convert.ToString(data[begin + i], 16).PadLeft(2, '0').PadRight(3, ' '));
            return sb.ToString().ToUpper();
        }

       
        public static string GetFileName(string track, NotificationLanguage lang = NotificationLanguage.Ru)
        {
            string langPostfix = String.Empty;
            switch (lang)
            {
                case NotificationLanguage.Eng:
                    langPostfix = "_" + NotificationLanguage.Eng;
                    break;
            }
            track += langPostfix;
            string Path = Application.StartupPath + @"\";
         

            if (FilesFolder != null && FilesFolder.Contains(track))
                return Path + @"Wav\Sounds\" + track + ".wav";

            if (TrainNumbersFolder != null && TrainNumbersFolder.Contains(track))
                return Path + @"Wav\Number of trains\" + track + ".wav";

            if (СписокСтатическихСообщений != null && СписокСтатическихСообщений.Contains(track))
                return Path + @"Wav\Static message\" + track + ".wav";

            if (СписокДинамическихСообщений != null && СписокДинамическихСообщений.Contains(track))
                return Path + @"Wav\Dynamic message\" + track + ".wav";

            foreach (var sound in StaticSoundForm.StaticSoundRecords)
                if (sound.Name == track)
                    return sound.Path;

            return "";
        }



        public static void ЗагрузкаНазванийПоездов()
        {
            var xmlFile = XmlWorker.LoadXmlFile(string.Empty, "Stations.xml"); //все настройки в одном файле
            if (xmlFile == null)
                return;

            DirectionRepository = new RepositoryXmlDirection(xmlFile);
            var directions = DirectionRepository.List();  

            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader("Stations.ini"))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        var str = line.Split(':');
                        if (str.Length == 2)
                        {
                            Станции[str[0].Trim()]= str[1].Trim();           //RU,ENG название станции
                        }
                        else
                        {
                            Станции[str[0].Trim()] = String.Empty;
                        }                      
                    }

                    if (file != null)
                        file.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }



        public static void ЗагрузкаНазванийПутей()
        {
            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader("PathNames.ini"))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                        НомераПутей.Add(line);

                    file.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void ЗаписьЛога(string ТипСообщения, string Сообщение)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(File.Open("Logs\\" + DateTime.Now.ToString("yy.MM.dd") + ".log", FileMode.Append)))
                {
                    sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + ": \t" + ТипСообщения + "\t" + Сообщение);
                    sw.Close();
                }
            }
            catch (Exception ex) { };
        }
    }
}
