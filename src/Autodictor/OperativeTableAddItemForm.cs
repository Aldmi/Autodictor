using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainExample
{
    /// <summary>
    /// выбирается поезд из Главного распсисания, 
    /// </summary>
    public partial class OperativeTableAddItemForm : Form
    {
        public TrainTableRecord TableRec { get;  set; }
        private string[] СтанцииВыбранногоНаправления { get; set; } = new string[0];


        #region ctor

        public OperativeTableAddItemForm()
        {
            InitializeComponent();

            InitializeFormDate();
        }

        #endregion




        #region Methode

        private void InitializeFormDate()
        {
            foreach (var данные in TrainTable.TrainTableRecords)
            {
                string поезд = данные.ID + ":   " + данные.Num + " " + данные.Name + (данные.ArrivalTime != "" ? "   Приб: " + данные.ArrivalTime : "") + (данные.DepartureTime != "" ? "   Отпр: " + данные.DepartureTime : "");
                cBПоездИзРасписания.Items.Add(поезд);
            }


            foreach (var номерПоезда in Program.НомераПоездов)
                cBНомерПоезда.Items.Add(номерПоезда);


            foreach (var item in DynamicSoundForm.DynamicSoundRecords)
                cBШаблонОповещения.Items.Add(item.Name);
        }




        private void InitializeFormDate(TrainTableRecord tableRec)
        {
            cBНомерПоезда.Text = tableRec.Num;

            СтанцииВыбранногоНаправления = Program.ПолучитьСтанцииНаправления(tableRec.Direction)?.Select(st => st.NameRu).ToArray();
            if (СтанцииВыбранногоНаправления != null)
            {
                cBОткуда.Items.Clear();
                cBКуда.Items.Clear();
                cBОткуда.Items.AddRange(СтанцииВыбранногоНаправления);
                cBКуда.Items.AddRange(СтанцииВыбранногоНаправления);
            }
            cBОткуда.Text = tableRec.StationDepart;
            cBКуда.Text = tableRec.StationArrival;


            int Часы = 0;
            int Минуты = 0;
            DateTime времяСобытия = new DateTime(2000, 1, 1, 0, 0, 0);
            DateTime ВремяПрибытия = new DateTime(2000, 1, 1, 0, 0, 0);
            DateTime ВремяОтправления = new DateTime(2000, 1, 1, 0, 0, 0);
            byte НомерСписка = 0x00;
            // бит 0 - задан номер пути
            // бит 1 - задана нумерация поезда
            // бит 2 - прибытие
            // бит 3 - стоянка
            // бит 4 - отправления

            if (tableRec.ArrivalTime != "")
            {
                string[] subStrings = tableRec.ArrivalTime.Split(':');
                if (int.TryParse(subStrings[0], out Часы) && int.TryParse(subStrings[1], out Минуты))
                {
                    ВремяПрибытия = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Часы, Минуты, 0);
                    dTPВремя1.Value = ВремяПрибытия;
                    НомерСписка |= 0x04;
                }
            }

            if (tableRec.DepartureTime != "")
            {
                string[] subStrings = tableRec.DepartureTime.Split(':');
                if (int.TryParse(subStrings[0], out Часы) && int.TryParse(subStrings[1], out Минуты))
                {
                    ВремяОтправления = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Часы, Минуты, 0);
                    dTPВремя2.Value = ВремяОтправления;
                    НомерСписка |= 0x10;
                }
            }

            if ((НомерСписка & 0x14) == 0x14)
                rBТранзит.Invoke((MethodInvoker)(() => rBТранзит.Checked = true));
            else if ((НомерСписка & 0x10) == 0x10)
                rBОтправление.Invoke((MethodInvoker)(() => rBОтправление.Checked = true));
            else
                rBПрибытие.Invoke((MethodInvoker)(() => rBПрибытие.Checked = true));


            if (НомерСписка == 0x14)
            {
                var времяПрибытия = ВремяПрибытия;
                if (ВремяОтправления > времяПрибытия)
                {
                    времяПрибытия = времяПрибытия.AddDays(1);
                }
                var stopTime = (времяПрибытия - ВремяОтправления);
                tableRec.StopTime = stopTime.ToString("t");
            }

            //var номер= tableRec.TrainPathNumber[WeekDays.Постоянно];
            //byte номерПути = (byte)(Program.НомераПутей.IndexOf(номер) + 1);


            // Шаблоны оповещения
            lVШаблоныОповещения.Items.Clear();
            string[] шаблонОповещения = tableRec.SoundTemplates.Split(':');
            if ((шаблонОповещения.Length % 3) == 0)
            {
                for (int i = 0; i < шаблонОповещения.Length / 3; i++)
                {
                    if (Program.ШаблоныОповещения.Contains(шаблонОповещения[3 * i + 0]))
                    {
                        var типОповещенияПути = 0;
                        int.TryParse(шаблонОповещения[3 * i + 2], out типОповещенияПути);
                        if (типОповещенияПути > 1) типОповещенияПути = 0;
                        ListViewItem lvi = new ListViewItem(new string[] { шаблонОповещения[3 * i + 0], шаблонОповещения[3 * i + 1], Program.ТипыВремени[типОповещенияПути] });
                        this.lVШаблоныОповещения.Items.Add(lvi);
                    }
                }
            }

            // станции следования
            if (tableRec.Примечание.Contains("Со всеми остановками"))
            {
                rBСоВсемиОстановками.Checked = true;
            }
            else if (tableRec.Примечание.Contains("Без остановок"))
            {
                rBБезОстановок.Checked = true;
            }
            else if (tableRec.Примечание.Contains("С остановками: "))
            {
                string примечание = tableRec.Примечание.Replace("С остановками: ", "");
                string[] списокСтанций = примечание.Split(',');
                foreach (var станция in списокСтанций)
                    if (СтанцииВыбранногоНаправления.Contains(станция))
                        lB_ПоСтанциям.Items.Add(станция);

                rBСОстановкамиНа.Checked = true;
            }
            else if (tableRec.Примечание.Contains("Кроме: "))
            {
                string примечание = tableRec.Примечание.Replace("Кроме: ", "");
                string[] списокСтанций = примечание.Split(',');
                foreach (var станция in списокСтанций)
                    if (СтанцииВыбранногоНаправления.Contains(станция))
                        lB_ПоСтанциям.Items.Add(станция);

                rBСОстановкамиКроме.Checked = true;
            }
            else
            {
                rBНеОповещать.Checked = true;
            }
        }



        #endregion




        #region EventHandler

        private void cBПоездИзРасписания_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] parts = cBПоездИзРасписания.Text.Split(':');
            if (parts.Length > 0)
            {
                int ID;
                if (int.TryParse(parts[0], out ID) == true)
                {
                    foreach (var config in TrainTable.TrainTableRecords)
                    {
                        if (config.ID == ID)
                        {
                            TableRec = config;
                            InitializeFormDate(TableRec);
                        }
                    }
                }
            }
        }


        private void lVШаблоныОповещения_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lVШаблоныОповещения.SelectedItems.Count > 0)
            {
                string шаблон = lVШаблоныОповещения.SelectedItems[0].SubItems[0].Text;
                foreach (var item in DynamicSoundForm.DynamicSoundRecords)
                {
                    if (item.Name == шаблон)
                    {
                        var soundRec = new SoundRecord();  //TODO: получать через мапппинг
                        var key = soundRec.Время.ToString();

                        КарточкаДвиженияПоезда карточка= new КарточкаДвиженияПоезда(soundRec, key);
                        карточка.ОтобразитьШаблонОповещенияНаRichTb(item.Message, rTB_Сообщение);
                        break;
                    }
                }
            }
        }


        #endregion


    }
}
