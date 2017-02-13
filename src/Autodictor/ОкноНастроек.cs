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
    public struct _Настройки
    {
        public float ЗадержкаМеждуЗвуковымиСообщениями;
        public float ИнтервалМеждуОповещениемОбОтменеПоезда;
        public float ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда;
        public float ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда;
        public bool АвтФормСообщНаПассажирскийПоезд;
        public bool АвтФормСообщНаСкорыйПоезд;
        public bool АвтФормСообщНаСкоростнойПоезд;
        public bool АвтФормСообщНаПригородныйЭлектропоезд;
        public bool АвтФормСообщНаФирменный;
        public bool АвтФормСообщНаЛасточку;
        public bool АвтФормСообщНаРЭКС;
        public bool РазрешениеДобавленияЗаблокированныхПоездовВСписок;
        public Color[] НастройкиЦветов;
    };

    public partial class ОкноНастроек : Form
    {
        private Panel[] Панели;

        public ОкноНастроек()
        {
            InitializeComponent();

            Панели = new Panel[] { pCol1, pCol2, pCol3, pCol4, pCol5, pCol6, pCol7, pCol8, pCol9, pCol10, pCol11, pCol12, pCol13, pCol14 };

            ОтобразитьНастройкиВОкне();

            tBРегуляторГромкости.Value = Player.GetVolume();
        }

        private void btnЗагрузить_Click(object sender, EventArgs e)
        {
            ЗагрузитьНастройки();
            ОтобразитьНастройкиВОкне();
        }

        private void btnСохранить_Click(object sender, EventArgs e)
        {
            СчитатьНастройкиИзОкнаВФайл();
            СохранитьНастройки();
        }

        private void ОтобразитьНастройкиВОкне()
        {
            tBВремяМеждуСообщениями.Text = Program.Настройки.ЗадержкаМеждуЗвуковымиСообщениями.ToString("0.0");
            tBИнтОповещОбОтменеПоезда.Text = Program.Настройки.ИнтервалМеждуОповещениемОбОтменеПоезда.ToString("0.0");
            tBИнтОповещОЗадержкеПрибытия.Text = Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда.ToString("0.0");
            tBИнтОповещОЗадержкеОтправления.Text = Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда.ToString("0.0");
            cBПассажирскийПоезд.Checked = Program.Настройки.АвтФормСообщНаПассажирскийПоезд;
            cBСкорыйПоезд.Checked = Program.Настройки.АвтФормСообщНаСкорыйПоезд;
            cBСкоростнойПоезд.Checked = Program.Настройки.АвтФормСообщНаСкоростнойПоезд;
            cBПригЭлектропоезд.Checked = Program.Настройки.АвтФормСообщНаПригородныйЭлектропоезд;
            cBФирменный.Checked = Program.Настройки.АвтФормСообщНаФирменный;
            cBЛасточка.Checked = Program.Настройки.АвтФормСообщНаЛасточку;
            cBРЭКС.Checked = Program.Настройки.АвтФормСообщНаРЭКС;
            cBРазрешениеДобавленияЗаблокированныхПоездовВСписок.Checked = Program.Настройки.РазрешениеДобавленияЗаблокированныхПоездовВСписок;

            for (int i = 0; i < 14; i++)
                Панели[i].BackColor = Program.Настройки.НастройкиЦветов[i];
        }

        private void СчитатьНастройкиИзОкнаВФайл()
        {
            float НастройкаВремени;

            if (float.TryParse(tBВремяМеждуСообщениями.Text.Replace('.', ','), out НастройкаВремени))
                Program.Настройки.ЗадержкаМеждуЗвуковымиСообщениями = НастройкаВремени;
            if (float.TryParse(tBИнтОповещОбОтменеПоезда.Text.Replace('.', ','), out НастройкаВремени))
                Program.Настройки.ИнтервалМеждуОповещениемОбОтменеПоезда = НастройкаВремени;
            if (float.TryParse(tBИнтОповещОЗадержкеПрибытия.Text.Replace('.', ','), out НастройкаВремени))
                Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда = НастройкаВремени;
            if (float.TryParse(tBИнтОповещОЗадержкеОтправления.Text.Replace('.', ','), out НастройкаВремени))
                Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда = НастройкаВремени;

            Program.Настройки.АвтФормСообщНаПассажирскийПоезд = cBПассажирскийПоезд.Checked;
            Program.Настройки.АвтФормСообщНаСкорыйПоезд = cBСкорыйПоезд.Checked;
            Program.Настройки.АвтФормСообщНаСкоростнойПоезд = cBСкоростнойПоезд.Checked;
            Program.Настройки.АвтФормСообщНаПригородныйЭлектропоезд = cBПригЭлектропоезд.Checked;
            Program.Настройки.АвтФормСообщНаФирменный = cBФирменный.Checked;
            Program.Настройки.АвтФормСообщНаЛасточку = cBЛасточка.Checked;
            Program.Настройки.АвтФормСообщНаРЭКС = cBРЭКС.Checked;
            Program.Настройки.РазрешениеДобавленияЗаблокированныхПоездовВСписок = cBРазрешениеДобавленияЗаблокированныхПоездовВСписок.Checked;
        }

        public static void ЗагрузитьНастройки()
        {
            Program.Настройки.НастройкиЦветов = new Color[] { Color.Black, Color.LightGray, Color.Black, Color.LightBlue, Color.Black, Color.White, Color.Black, Color.Yellow, Color.Black, Color.LightGreen, Color.Black, Color.YellowGreen, Color.Black, Color.Orange };

            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader("Settings.ini"))
                {
                    string line;
                    float ПеременнаяFloat;
                    bool ПеременнаяBool;

                    while ((line = file.ReadLine()) != null)
                    {
                        string[] Settings = line.Split(':');
                        if (Settings.Length == 2)
                        {
                            switch (Settings[0])
                            {
                                case "ЗадержкаМеждуЗвуковымиСообщениями":
                                    if (float.TryParse(Settings[1], out ПеременнаяFloat))
                                        Program.Настройки.ЗадержкаМеждуЗвуковымиСообщениями = ПеременнаяFloat;
                                    break;

                                case "ИнтервалМеждуОповещениемОбОтменеПоезда":
                                    if (float.TryParse(Settings[1], out ПеременнаяFloat))
                                        Program.Настройки.ИнтервалМеждуОповещениемОбОтменеПоезда = ПеременнаяFloat;
                                    break;

                                case "ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда":
                                    if (float.TryParse(Settings[1], out ПеременнаяFloat))
                                        Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда = ПеременнаяFloat;
                                    break;

                                case "ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда":
                                    if (float.TryParse(Settings[1], out ПеременнаяFloat))
                                        Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда = ПеременнаяFloat;
                                    break;

                                case "АвтФормСообщНаПассажирскийПоезд":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.АвтФормСообщНаПассажирскийПоезд = ПеременнаяBool;
                                    break;

                                case "АвтФормСообщНаСкорыйПоезд":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.АвтФормСообщНаСкорыйПоезд = ПеременнаяBool;
                                    break;

                                case "АвтФормСообщНаСкоростнойПоезд":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.АвтФормСообщНаСкоростнойПоезд = ПеременнаяBool;
                                    break;

                                case "АвтФормСообщНаПригородныйЭлектропоезд":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.АвтФормСообщНаПригородныйЭлектропоезд = ПеременнаяBool;
                                    break;

                                case "АвтФормСообщНаФирменный":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.АвтФормСообщНаФирменный = ПеременнаяBool;
                                    break;

                                case "АвтФормСообщНаЛасточку":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.АвтФормСообщНаЛасточку = ПеременнаяBool;
                                    break;

                                case "АвтФормСообщНаРЭКС":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.АвтФормСообщНаРЭКС = ПеременнаяBool;
                                    break;

                                case "РазрешениеДобавленияЗаблокированныхПоездовВСписок":
                                    if (bool.TryParse(Settings[1], out ПеременнаяBool))
                                        Program.Настройки.РазрешениеДобавленияЗаблокированныхПоездовВСписок = ПеременнаяBool;
                                    break;

                                case "ЦветовыеНастройки":
                                    string[] ЦветовыеНастройки = Settings[1].Split(',');
                                    if (ЦветовыеНастройки.Length == 14)
                                        for (int i = 0; i < 14; i++)
                                            Program.Настройки.НастройкиЦветов[i] = ColorTranslator.FromHtml(ЦветовыеНастройки[i]);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Program.ЗаписьЛога("Системное сообщение", "Ошибка загрузки настроек: " + ex.Message);
            }

            if (Program.Настройки.ЗадержкаМеждуЗвуковымиСообщениями < 1)
                Program.Настройки.ЗадержкаМеждуЗвуковымиСообщениями = 1;
            if (Program.Настройки.ИнтервалМеждуОповещениемОбОтменеПоезда < 1)
                Program.Настройки.ИнтервалМеждуОповещениемОбОтменеПоезда = 1;
            if (Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда < 1)
                Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда = 1;
            if (Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда < 1)
                Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда = 1;
        }

        public static void СохранитьНастройки()
        {
            try
            {
                using (System.IO.StreamWriter DumpFile = new System.IO.StreamWriter("Settings.ini"))
                {
                    DumpFile.WriteLine("ЗадержкаМеждуЗвуковымиСообщениями:" + Program.Настройки.ЗадержкаМеждуЗвуковымиСообщениями.ToString("0.0"));
                    DumpFile.WriteLine("ИнтервалМеждуОповещениемОбОтменеПоезда:" + Program.Настройки.ИнтервалМеждуОповещениемОбОтменеПоезда.ToString("0.0"));
                    DumpFile.WriteLine("ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда:" + Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеПрибытияПоезда.ToString("0.0"));
                    DumpFile.WriteLine("ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда:" + Program.Настройки.ИнтервалМеждуОповещениемОЗадержкеОтправленияПоезда.ToString("0.0"));
                    DumpFile.WriteLine("АвтФормСообщНаПассажирскийПоезд:" + Program.Настройки.АвтФормСообщНаПассажирскийПоезд.ToString());
                    DumpFile.WriteLine("АвтФормСообщНаСкорыйПоезд:" + Program.Настройки.АвтФормСообщНаСкорыйПоезд.ToString());
                    DumpFile.WriteLine("АвтФормСообщНаСкоростнойПоезд:" + Program.Настройки.АвтФормСообщНаСкоростнойПоезд.ToString());
                    DumpFile.WriteLine("АвтФормСообщНаПригородныйЭлектропоезд:" + Program.Настройки.АвтФормСообщНаПригородныйЭлектропоезд.ToString());
                    DumpFile.WriteLine("АвтФормСообщНаФирменный:" + Program.Настройки.АвтФормСообщНаФирменный.ToString());
                    DumpFile.WriteLine("АвтФормСообщНаЛасточку:" + Program.Настройки.АвтФормСообщНаЛасточку.ToString());
                    DumpFile.WriteLine("АвтФормСообщНаРЭКС:" + Program.Настройки.АвтФормСообщНаРЭКС.ToString());
                    DumpFile.WriteLine("РазрешениеДобавленияЗаблокированныхПоездовВСписок:" + Program.Настройки.РазрешениеДобавленияЗаблокированныхПоездовВСписок.ToString());

                    string ЦветовыеНастройки = "";
                    for (int i = 0; i < 14; i++)
                        ЦветовыеНастройки += ColorTranslator.ToHtml(Program.Настройки.НастройкиЦветов[i]) + ",";
                    if (ЦветовыеНастройки[ЦветовыеНастройки.Length - 1] == ',')
                        ЦветовыеНастройки = ЦветовыеНастройки.Remove(ЦветовыеНастройки.Length - 1, 1);
                    DumpFile.WriteLine("ЦветовыеНастройки:" + ЦветовыеНастройки);

                    DumpFile.Close();
                }
            }
            catch (Exception ex)
            {
                Program.ЗаписьЛога("Системное сообщение", "Ошибка сохранения настроек: " + ex.Message);
            }
        }

        private void pCol1_DoubleClick(object sender, EventArgs e)
        {
            int НомерПанели = int.Parse((string)(sender as Panel).Tag);

            colorDialog1.Color = Program.Настройки.НастройкиЦветов[НомерПанели];
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Program.Настройки.НастройкиЦветов[НомерПанели] = colorDialog1.Color;
                (sender as Panel).BackColor = Program.Настройки.НастройкиЦветов[НомерПанели];
            }
        }

        private void tBРегуляторГромкости_Scroll(object sender, EventArgs e)
        {
            Player.SetVolume(tBРегуляторГромкости.Value);
        }
    }
}
