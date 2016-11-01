using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;



namespace MainExample
{
    public struct DynamicSoundRecord
    {
        public int ID;
        public string Name;
        public string Message;
    };



    public partial class DynamicSoundForm : Form
    {
        public static List<DynamicSoundRecord> DynamicSoundRecords = new List<DynamicSoundRecord>();
        private static int ID = 0;

        private string[] PlayList;
        private int CurrentPlayList = 100;
        private int ТекущаяПозицияЗвучания = 0;
        private float ОбщаяДлительностьЗвучания = 0;
        private int ИндексВыделенойПодстроки = -1;

        public DynamicSoundForm()
        {
            InitializeComponent();

            this.comboBox_Messages.Items.Add("НОМЕР ПОЕЗДА");
            this.comboBox_Messages.Items.Add("НОМЕР ПУТИ");
            this.comboBox_Messages.Items.Add("ВРЕМЯ ПРИБЫТИЯ");
            this.comboBox_Messages.Items.Add("ВРЕМЯ СТОЯНКИ");
            this.comboBox_Messages.Items.Add("ВРЕМЯ ОТПРАВЛЕНИЯ");
            this.comboBox_Messages.Items.Add("НУМЕРАЦИЯ СОСТАВА");

            foreach (var Данные in Program.FilesFolder)
                this.comboBox_Messages.Items.Add(Данные);
                        

            ЗагрузитьСписок();
            ОбновитьДанныеВСписке();
        }

        private void ОбновитьДанныеВСписке()
        {
            int НомерЭлемента = 0;

            listView1.Items.Clear();

            foreach (var Данные in DynamicSoundRecords)
            {
                ListViewItem lvi = new ListViewItem(new string[] { Данные.ID.ToString(), Данные.Name, Данные.Message });
                lvi.Tag = Данные.ID;
                lvi.BackColor = (НомерЭлемента++ % 2) == 0 ? Color.Aqua : Color.LightGreen;
                this.listView1.Items.Add(lvi);
            }
        }

        // Обновить список сообщений
        private void button1_Click(object sender, EventArgs e)
        {
            ОбновитьДанныеВСписке();
        }

        // Добавить сообщение
        private void button2_Click(object sender, EventArgs e)
        {
            DynamicSoundRecord Данные;

            Данные.ID = ++ID;
            Данные.Name = this.textBox_Name.Text;
            Данные.Message = this.textBox_Message.Text;

            DynamicSoundRecords.Add(Данные);
            ОбновитьДанныеВСписке();
        }

        // Изменить сообщение
        private void button3_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

            foreach (int item in sic)
            {
                this.listView1.Items[item].SubItems[1].Text = this.textBox_Name.Text;
                this.listView1.Items[item].SubItems[2].Text = this.textBox_Message.Text;

                DynamicSoundRecord Данные = DynamicSoundRecords[item];
                Данные.Name = this.textBox_Name.Text;
                Данные.Message = this.textBox_Message.Text;
                DynamicSoundRecords[item] = Данные;
            }
        }

        // Удалить сообщение
        private void button4_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

            foreach (int item in sic)
                DynamicSoundRecords.RemoveAt(item);

            ОбновитьДанныеВСписке();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

                foreach (int item in sic)
                {
                    this.textBox_Name.Text = this.listView1.Items[item].SubItems[1].Text;
                    this.textBox_Message.Text = this.listView1.Items[item].SubItems[2].Text;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Вставить текст
        private void button5_Click(object sender, EventArgs e)
        {
            if (this.textBox_Message.Text != "")
                this.textBox_Message.Text += "|";

            this.textBox_Message.Text += this.comboBox_Messages.Text;
            
        }

        // Передвинуть курсор влево
        private void button_left_Click(object sender, EventArgs e)
        {
            int СтартоваяПозиция = 0;
            int ИндексВыбранойСтроки = 0;

            if (!String.IsNullOrEmpty(this.textBox_Message.Text))
            {
                string[] Подстроки = this.textBox_Message.Text.Split('|');

                if (ИндексВыделенойПодстроки == -1)
                    ИндексВыделенойПодстроки = Подстроки.Length - 1;
                else if (ИндексВыделенойПодстроки > 0)
                    ИндексВыделенойПодстроки--;

                if (ИндексВыделенойПодстроки >= Подстроки.Length)
                    ИндексВыделенойПодстроки = Подстроки.Length - 1;

                for (ИндексВыбранойСтроки = 0; (ИндексВыбранойСтроки < Подстроки.Length) && (ИндексВыбранойСтроки < ИндексВыделенойПодстроки); ИндексВыбранойСтроки++)
                    СтартоваяПозиция += Подстроки[ИндексВыбранойСтроки].Length + 1;

                if (ИндексВыбранойСтроки < Подстроки.Length)
                {
                    this.textBox_Message.Focus();
                    this.textBox_Message.Select();
                    this.textBox_Message.ScrollToCaret();
                    this.textBox_Message.SelectionStart = СтартоваяПозиция;
                    this.textBox_Message.SelectionLength = Подстроки[ИндексВыбранойСтроки].Length;
                }
            }
            else
                ИндексВыделенойПодстроки = -1;
        }

        // Передвинуть курсор вправо
        private void button_right_Click(object sender, EventArgs e)
        {
            int СтартоваяПозиция = 0;
            int ИндексВыбранойСтроки = 0;

            if (!String.IsNullOrEmpty(this.textBox_Message.Text))
            {
                string[] Подстроки = this.textBox_Message.Text.Split('|');

                if (ИндексВыделенойПодстроки == -1)
                    ИндексВыделенойПодстроки = 0;
                else
                    ИндексВыделенойПодстроки++;

                if (ИндексВыделенойПодстроки >= Подстроки.Length)
                    ИндексВыделенойПодстроки = Подстроки.Length - 1;

                for (ИндексВыбранойСтроки = 0; (ИндексВыбранойСтроки < Подстроки.Length) && (ИндексВыбранойСтроки < ИндексВыделенойПодстроки); ИндексВыбранойСтроки++)
                    СтартоваяПозиция += Подстроки[ИндексВыбранойСтроки].Length + 1;

                if (ИндексВыбранойСтроки < Подстроки.Length)
                {
                    this.textBox_Message.Focus();
                    this.textBox_Message.Select();
                    this.textBox_Message.ScrollToCaret();
                    this.textBox_Message.SelectionStart = СтартоваяПозиция;
                    this.textBox_Message.SelectionLength = Подстроки[ИндексВыбранойСтроки].Length;
                }
            }
            else
                ИндексВыделенойПодстроки = -1;
        }

        // Удалить выделенное сообщение
        private void button_delete_Click(object sender, EventArgs e)
        {
            int ИндексВыбранойСтроки = 0;

            if (!String.IsNullOrEmpty(this.textBox_Message.Text))
            {
                string[] Подстроки = this.textBox_Message.Text.Split('|');

                if ((ИндексВыделенойПодстроки >= 0) && (ИндексВыделенойПодстроки < Подстроки.Length))
                {
                    bool ПерваяСтрока = true;
                    this.textBox_Message.Text = "";

                    for (ИндексВыбранойСтроки = 0; ИндексВыбранойСтроки < Подстроки.Length; ИндексВыбранойСтроки++)
                        if (ИндексВыбранойСтроки != ИндексВыделенойПодстроки)
                        {
                            this.textBox_Message.Text += (ПерваяСтрока == true ? "" : "|") + Подстроки[ИндексВыбранойСтроки];
                            ПерваяСтрока = false;
                        }
                }
            }
        }
        private void btnСохранить_Click(object sender, EventArgs e)
        {
            СохранитьСписок();
        }


        public static void ЗагрузитьСписок()
        {
            DynamicSoundRecords.Clear();
            ID = 0;

            System.IO.StreamReader file = null;

            try
            {
                file = new System.IO.StreamReader("DynamicSound.ini");

                string line;

                while ((line = file.ReadLine()) != null)
                {
                    string[] Settings = line.Split(';');
                    if (Settings.Length == 3)
                    {
                        DynamicSoundRecord Данные;

                        Данные.ID = int.Parse(Settings[0]);
                        Данные.Name = Settings[1];
                        Данные.Message = Settings[2];

                        DynamicSoundRecords.Add(Данные);

                        if (Данные.ID > ID)
                            ID = Данные.ID;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (file != null)
                    file.Close();
            }
        }

        private static void СохранитьСписок()
        {
            try
            {
                System.IO.StreamWriter DumpFile = new System.IO.StreamWriter("DynamicSound.ini");

                foreach (var Данные in DynamicSoundRecords)
                    DumpFile.WriteLine(Данные.ID.ToString() + ";" + Данные.Name + ";" + Данные.Message);

                DumpFile.Flush();
                DumpFile.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void button6_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "Стоп")
            {
                CurrentPlayList = 100;
                button6.Text = "Пуск";
                Player.PlayFile("");
                return;
            }

            НачатьВоспроизведениеФайла();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ТекущаяПозицияЗвучания++;

            SoundFileStatus status = Player.GetFileStatus();

            if (status == SoundFileStatus.Playing)
            {
                int CurrentPosition = Player.GetCurrentPosition();
                float Duration = Player.GetDuration();

                button6.Text = "Стоп";
                Player_Label.Text = (ТекущаяПозицияЗвучания / 60).ToString() + ":" + (ТекущаяПозицияЗвучания % 60).ToString("00") + " / " + (ОбщаяДлительностьЗвучания / 60).ToString("0") + ":" + (ОбщаяДлительностьЗвучания % 60).ToString("00");
            }
            else
            {
                if ((status == SoundFileStatus.Paused) || (status == SoundFileStatus.Stop))
                {
                    int CurrentPosition = Player.GetCurrentPosition();
                    float Duration = Player.GetDuration();
                    Player_Label.Text = (ТекущаяПозицияЗвучания / 600).ToString() + ":" + ((ТекущаяПозицияЗвучания / 10) % 60).ToString("00") + " / " + (ОбщаяДлительностьЗвучания / 60).ToString("0") + ":" + (ОбщаяДлительностьЗвучания % 60).ToString("00");
                }

                if ((PlayList != null) && (CurrentPlayList < PlayList.Length))
                {
                    string nextfile = Program.GetFileName(PlayList[CurrentPlayList]);

                    if (nextfile != "")
                    {
                        CurrentPlayList++;
                        Player.PlayFile(nextfile);
                        return;
                    }
                }
                else
                {
                    Player.PlayFile("");
                }

                button6.Text = "Пуск";
            }
        }

        private bool НачатьВоспроизведениеФайла()
        {
            PlayList = textBox_Message.Text.Split('|');

            ТекущаяПозицияЗвучания = 0;
            ОбщаяДлительностьЗвучания = 0;

            foreach (string str in PlayList)
            {
                string filename = Program.GetFileName(str);

                if (filename != "")
                {
                    Player.PlayFile(filename);
                    ОбщаяДлительностьЗвучания += Player.GetDuration();
                    Player.PlayFile("");
                }
            }


            if (PlayList.Length > 0)
            {
                CurrentPlayList = 0;
                string filename = Program.GetFileName(PlayList[CurrentPlayList]);
                if (filename != "")
                {
                    Player.PlayFile(filename);
                    CurrentPlayList++;
                    return true;
                }
            }

            return false;
        }
    }
}
