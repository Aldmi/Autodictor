using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Library.Logs;


namespace MainExample
{
    static class Program
    {
        static Mutex m_mutex;

        public static List<string> FilesFolder = null;
        public static List<string> TrainNumbersFolder = null;

        public static string ИнфСтрокаНаТабло = "";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (InstanceExists())
                return;
       
                var dir = new DirectoryInfo(Application.StartupPath + @"\Wav\Sounds\");
                FilesFolder = new List<string>();
                foreach (FileInfo file in dir.GetFiles("*.wav"))
                    FilesFolder.Add(Path.GetFileNameWithoutExtension(file.FullName));

                dir = new DirectoryInfo(Application.StartupPath + @"\Wav\Number of trains\");
                TrainNumbersFolder = new List<string>();
                foreach (FileInfo file in dir.GetFiles("*.wav"))
                    TrainNumbersFolder.Add(Path.GetFileNameWithoutExtension(file.FullName));


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

       
        public static string GetFileName(string track)
        {
            string Path = Application.StartupPath + @"\";

            if (track == "00 минут") return Path + @"Wav\Dynamic message\00 минут.wav";
            if (track == "01 минута") return Path + @"Wav\Dynamic message\01 минута.wav";
            if (track == "02 минуты") return Path + @"Wav\Dynamic message\02 минуты.wav";
            if (track == "03 минуты") return Path + @"Wav\Dynamic message\03 минуты.wav";
            if (track == "04 минуты") return Path + @"Wav\Dynamic message\04 минуты.wav";
            if (track == "05 минут") return Path + @"Wav\Dynamic message\05 минут.wav";
            if (track == "06 минут") return Path + @"Wav\Dynamic message\06 минут.wav";
            if (track == "07 минут") return Path + @"Wav\Dynamic message\07 минут.wav";
            if (track == "08 минут") return Path + @"Wav\Dynamic message\08 минут.wav";
            if (track == "09 минут") return Path + @"Wav\Dynamic message\09 минут.wav";
            if (track == "10 минут") return Path + @"Wav\Dynamic message\10 минут.wav";
            if (track == "11 минут") return Path + @"Wav\Dynamic message\11 минут.wav";
            if (track == "12 минут") return Path + @"Wav\Dynamic message\12 минут.wav";
            if (track == "13 минут") return Path + @"Wav\Dynamic message\13 минут.wav";
            if (track == "14 минут") return Path + @"Wav\Dynamic message\14 минут.wav";
            if (track == "15 минут") return Path + @"Wav\Dynamic message\15 минут.wav";
            if (track == "16 минут") return Path + @"Wav\Dynamic message\16 минут.wav";
            if (track == "17 минут") return Path + @"Wav\Dynamic message\17 минут.wav";
            if (track == "18 минут") return Path + @"Wav\Dynamic message\18 минут.wav";
            if (track == "19 минут") return Path + @"Wav\Dynamic message\19 минут.wav";
            if (track == "20 минут") return Path + @"Wav\Dynamic message\20 минут.wav";
            if (track == "21 минута") return Path + @"Wav\Dynamic message\21 минута.wav";
            if (track == "22 минуты") return Path + @"Wav\Dynamic message\22 минуты.wav";
            if (track == "23 минуты") return Path + @"Wav\Dynamic message\23 минуты.wav";
            if (track == "24 минуты") return Path + @"Wav\Dynamic message\24 минуты.wav";
            if (track == "25 минут") return Path + @"Wav\Dynamic message\25 минут.wav";
            if (track == "26 минут") return Path + @"Wav\Dynamic message\26 минут.wav";
            if (track == "27 минут") return Path + @"Wav\Dynamic message\27 минут.wav";
            if (track == "28 минут") return Path + @"Wav\Dynamic message\28 минут.wav";
            if (track == "29 минут") return Path + @"Wav\Dynamic message\29 минут.wav";
            if (track == "30 минут") return Path + @"Wav\Dynamic message\30 минут.wav";
            if (track == "31 минута") return Path + @"Wav\Dynamic message\31 минута.wav";
            if (track == "32 минуты") return Path + @"Wav\Dynamic message\32 минуты.wav";
            if (track == "33 минуты") return Path + @"Wav\Dynamic message\33 минуты.wav";
            if (track == "34 минуты") return Path + @"Wav\Dynamic message\34 минуты.wav";
            if (track == "35 минут") return Path + @"Wav\Dynamic message\35 минут.wav";
            if (track == "36 минут") return Path + @"Wav\Dynamic message\36 минут.wav";
            if (track == "37 минут") return Path + @"Wav\Dynamic message\37 минут.wav";
            if (track == "38 минут") return Path + @"Wav\Dynamic message\38 минут.wav";
            if (track == "39 минут") return Path + @"Wav\Dynamic message\39 минут.wav";
            if (track == "40 минут") return Path + @"Wav\Dynamic message\40 минут.wav";
            if (track == "41 минута") return Path + @"Wav\Dynamic message\41 минута.wav";
            if (track == "42 минуты") return Path + @"Wav\Dynamic message\42 минуты.wav";
            if (track == "43 минуты") return Path + @"Wav\Dynamic message\43 минуты.wav";
            if (track == "44 минуты") return Path + @"Wav\Dynamic message\44 минуты.wav";
            if (track == "45 минут") return Path + @"Wav\Dynamic message\45 минут.wav";
            if (track == "46 минут") return Path + @"Wav\Dynamic message\46 минут.wav";
            if (track == "47 минут") return Path + @"Wav\Dynamic message\47 минут.wav";
            if (track == "48 минут") return Path + @"Wav\Dynamic message\48 минут.wav";
            if (track == "49 минут") return Path + @"Wav\Dynamic message\49 минут.wav";
            if (track == "50 минут") return Path + @"Wav\Dynamic message\50 минут.wav";
            if (track == "51 минута") return Path + @"Wav\Dynamic message\51 минута.wav";
            if (track == "52 минуты") return Path + @"Wav\Dynamic message\52 минуты.wav";
            if (track == "53 минуты") return Path + @"Wav\Dynamic message\53 минуты.wav";
            if (track == "54 минуты") return Path + @"Wav\Dynamic message\54 минуты.wav";
            if (track == "55 минут") return Path + @"Wav\Dynamic message\55 минут.wav";
            if (track == "56 минут") return Path + @"Wav\Dynamic message\56 минут.wav";
            if (track == "57 минут") return Path + @"Wav\Dynamic message\57 минут.wav";
            if (track == "58 минут") return Path + @"Wav\Dynamic message\58 минут.wav";
            if (track == "59 минут") return Path + @"Wav\Dynamic message\59 минут.wav";

            if (track == "В 00 часов") return Path + @"Wav\Dynamic message\В 00 часов.wav";
            if (track == "В 01 час") return Path + @"Wav\Dynamic message\В 01 час.wav";
            if (track == "В 02 часа") return Path + @"Wav\Dynamic message\В 02 часа.wav";
            if (track == "В 03 часа") return Path + @"Wav\Dynamic message\В 03 часа.wav";
            if (track == "В 04 часа") return Path + @"Wav\Dynamic message\В 04 часа.wav";
            if (track == "В 05 часов") return Path + @"Wav\Dynamic message\В 05 часов.wav";
            if (track == "В 06 часов") return Path + @"Wav\Dynamic message\В 06 часов.wav";
            if (track == "В 07 часов") return Path + @"Wav\Dynamic message\В 07 часов.wav";
            if (track == "В 08 часов") return Path + @"Wav\Dynamic message\В 08 часов.wav";
            if (track == "В 09 часов") return Path + @"Wav\Dynamic message\В 09 часов.wav";
            if (track == "В 10 часов") return Path + @"Wav\Dynamic message\В 10 часов.wav";
            if (track == "В 11 часов") return Path + @"Wav\Dynamic message\В 11 часов.wav";
            if (track == "В 12 часов") return Path + @"Wav\Dynamic message\В 12 часов.wav";
            if (track == "В 13 часов") return Path + @"Wav\Dynamic message\В 13 часов.wav";
            if (track == "В 14 часов") return Path + @"Wav\Dynamic message\В 14 часов.wav";
            if (track == "В 15 часов") return Path + @"Wav\Dynamic message\В 15 часов.wav";
            if (track == "В 16 часов") return Path + @"Wav\Dynamic message\В 16 часов.wav";
            if (track == "В 17 часов") return Path + @"Wav\Dynamic message\В 17 часов.wav";
            if (track == "В 18 часов") return Path + @"Wav\Dynamic message\В 18 часов.wav";
            if (track == "В 19 часов") return Path + @"Wav\Dynamic message\В 19 часов.wav";
            if (track == "В 20 часов") return Path + @"Wav\Dynamic message\В 20 часов.wav";
            if (track == "В 21 час") return Path + @"Wav\Dynamic message\В 21 час.wav";
            if (track == "В 22 часа") return Path + @"Wav\Dynamic message\В 22 часа.wav";
            if (track == "В 23 часа") return Path + @"Wav\Dynamic message\В 23 часа.wav";

            if (track == "На 1ый путь") return Path + @"Wav\Dynamic message\На 1ый путь.wav";
            if (track == "На 2ой путь") return Path + @"Wav\Dynamic message\На 2ой путь.wav";
            if (track == "На 3ий путь") return Path + @"Wav\Dynamic message\На 3ий путь.wav";
            if (track == "На 4ый путь") return Path + @"Wav\Dynamic message\На 4ый путь.wav";
            if (track == "На 5ый путь") return Path + @"Wav\Dynamic message\На 5ый путь.wav";
            if (track == "На 6ой путь") return Path + @"Wav\Dynamic message\На 6ой путь.wav";
            if (track == "На 7ой путь") return Path + @"Wav\Dynamic message\На 7ой путь.wav";
            if (track == "На 8ой путь") return Path + @"Wav\Dynamic message\На 8ой путь.wav";
            if (track == "На 9ый путь") return Path + @"Wav\Dynamic message\На 9ый путь.wav";
            if (track == "На 10ый путь") return Path + @"Wav\Dynamic message\На 10ый путь.wav";
            if (track == "На 11ый путь") return Path + @"Wav\Dynamic message\На 11ый путь.wav";
            if (track == "На 12ый путь") return Path + @"Wav\Dynamic message\На 12ый путь.wav";
            if (track == "На 13ый путь") return Path + @"Wav\Dynamic message\На 13ый путь.wav";
            if (track == "На 14ый путь") return Path + @"Wav\Dynamic message\На 14ый путь.wav";

            if (track == "На 1ом пути") return Path + @"Wav\Dynamic message\На 1ом пути.wav";
            if (track == "На 2ом пути") return Path + @"Wav\Dynamic message\На 2ом пути.wav";
            if (track == "На 3ем пути") return Path + @"Wav\Dynamic message\На 3ем пути.wav";
            if (track == "На 4ом пути") return Path + @"Wav\Dynamic message\На 4ом пути.wav";
            if (track == "На 5ом пути") return Path + @"Wav\Dynamic message\На 5ом пути.wav";
            if (track == "На 6ом пути") return Path + @"Wav\Dynamic message\На 6ом пути.wav";
            if (track == "На 7ом пути") return Path + @"Wav\Dynamic message\На 7ом пути.wav";
            if (track == "На 8ом пути") return Path + @"Wav\Dynamic message\На 8ом пути.wav";
            if (track == "На 9ом пути") return Path + @"Wav\Dynamic message\На 9ом пути.wav";
            if (track == "На 10ом пути") return Path + @"Wav\Dynamic message\На 10ом пути.wav";
            if (track == "На 11ом пути") return Path + @"Wav\Dynamic message\На 11ом пути.wav";
            if (track == "На 12ом пути") return Path + @"Wav\Dynamic message\На 12ом пути.wav";
            if (track == "На 13ом пути") return Path + @"Wav\Dynamic message\На 13ом пути.wav";
            if (track == "На 14ом пути") return Path + @"Wav\Dynamic message\На 14ом пути.wav";

            if (track == "С 1ого пути") return Path + @"Wav\Dynamic message\С 1ого пути.wav";
            if (track == "С 2ого пути") return Path + @"Wav\Dynamic message\С 2ого пути.wav";
            if (track == "С 3его пути") return Path + @"Wav\Dynamic message\С 3его пути.wav";
            if (track == "С 4ого пути") return Path + @"Wav\Dynamic message\С 4ого пути.wav";
            if (track == "С 5ого пути") return Path + @"Wav\Dynamic message\С 5ого пути.wav";
            if (track == "С 6ого пути") return Path + @"Wav\Dynamic message\С 6ого пути.wav";
            if (track == "С 7ого пути") return Path + @"Wav\Dynamic message\С 7ого пути.wav";
            if (track == "С 8ого пути") return Path + @"Wav\Dynamic message\С 8ого пути.wav";
            if (track == "С 9ого пути") return Path + @"Wav\Dynamic message\С 9ого пути.wav";
            if (track == "С 10ого пути") return Path + @"Wav\Dynamic message\С 10ого пути.wav";
            if (track == "С 11ого пути") return Path + @"Wav\Dynamic message\С 11ого пути.wav";
            if (track == "С 12ого пути") return Path + @"Wav\Dynamic message\С 12ого пути.wav";
            if (track == "С 13ого пути") return Path + @"Wav\Dynamic message\С 13ого пути.wav";
            if (track == "С 14ого пути") return Path + @"Wav\Dynamic message\С 14ого пути.wav";

            if (track == "Нумерация с головы") return Path + @"Wav\Dynamic message\Нумерация с головы.wav";
            if (track == "Нумерация с хвоста") return Path + @"Wav\Dynamic message\Нумерация с хвоста.wav";
            
            //==========================================

            if (FilesFolder.Contains(track))
                return Path + @"Wav\Sounds\" + track + ".wav";

            if (TrainNumbersFolder.Contains(track))
                return Path + @"Wav\Number of trains\" + track + ".wav";

            return "";
        }
    }
}
