using System.Collections.Generic;
using System.Security.AccessControl;
using LedScreenLibNetWrapper;


namespace Communication.SibWayApi
{
    /// <summary>
    /// Класс настроек окна для отображения инофрмации
    /// </summary>
    public class WindowSett
    {
        public byte Number { get; set; } //Номер окна
        public string Name { get; set; } //Название поля для вывода, т.е. имя переменной отображаемой в этом окне
        public int Width { get; set; }   //Ширина окна
        public int Height { get; set; }  //Высота окна

        public DisplayEffect Effect { get; set; } //Эффект отображения
        public DisplayTextHAlign TextHAlign { get; set; } //Выравнивание по горизонтали
        public DisplayTextVAlign TextVAlign { get; set; } //Выравнивание по вертикали
        public DisplayTextHeight TextHeight { get; set; } //Размер каждого шрифта 8/12/16/24/32 пикселя в высоту
        public ushort DisplayTime { get; set; } //???
        public int DelayBetweenSending { get; set; } // задержка времени в мсек, на отправку инфы между экранами.

        public byte[] ColorBytes { get; set; } = new byte[4]; //Цвет { color.B, color.G, color.R, 0x00 }

        public List<string> SendingStrings { get; set; } = new List<string>();
    }




    public class SettingSibWay
    {
        private readonly string _pathFont8Px; //= Application.StartupPath + @"\LEDFont8px.xml";

        public byte FontSize { get; set; }  //Размер шрифта 8/12/16/24/32 пикселя в высоту. DisplayTextHeight для каждого окна берется соответсвующий.
        public List<WindowSett> WindowSett { get; set; }


        public SettingSibWay()
        {
            FontSize = 16;
            WindowSett = new List<WindowSett>
            {
                new WindowSett {Number = 1, Name ="NumberOfTrain", Height = 160, Width = 24},
                new WindowSett {Number = 2, Name ="TypeTrain", Height = 160, Width = 24},
                new WindowSett {Number = 3, Name ="Route", Height = 160, Width = 136},
                new WindowSett {Number = 4, Name ="TimeArrival", Height = 160, Width = 24},
                new WindowSett {Number = 5, Name ="TimeDeparture", Height = 160, Width = 24},
                new WindowSett {Number = 6, Name ="Path", Height = 160, Width = 24}
            };
        }
    }
}