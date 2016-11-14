using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;



namespace MainExample
{
    public struct StaticSoundRecord
    {
        public int ID;
        public string Name;
        public string Message;
        public string Path;
    };


    
    public partial class StaticSoundForm : Form
    {
        public static List<StaticSoundRecord> StaticSoundRecords = new List<StaticSoundRecord>();
        private static int ID = 0;

        public StaticSoundForm()
        {
            InitializeComponent();
            ЗагрузитьСписок();
            ОбновитьДанныеВСписке();
        }

        private void ОбновитьДанныеВСписке()
        {
            int НомерЭлемента = 0;

            listView1.Items.Clear();

            foreach (var Данные in StaticSoundRecords)
            {
                ListViewItem lvi = new ListViewItem(new string[] { Данные.ID.ToString(), Данные.Name, Данные.Message, Данные.Path });
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
            StaticSoundRecord Данные;

            Данные.ID = ++ID;
            Данные.Name = this.textBox_Name.Text;
            Данные.Message = this.textBox_Message.Text;
            Данные.Path = this.textBox_Path.Text;

            StaticSoundRecords.Add(Данные);
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
                this.listView1.Items[item].SubItems[3].Text = this.textBox_Path.Text;

                StaticSoundRecord Данные = StaticSoundRecords[item];
                Данные.Name = this.textBox_Name.Text;
                Данные.Message = this.textBox_Message.Text;
                Данные.Path = this.textBox_Path.Text;
                StaticSoundRecords[item] = Данные;
            }
        }

        // Удалить сообщение
        private void button4_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

            foreach (int item in sic)
                StaticSoundRecords.RemoveAt(item);

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
                    this.textBox_Path.Text = this.listView1.Items[item].SubItems[3].Text;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                this.textBox_Path.Text = dialog.FileName;
        }

        private void staticSoundForm_SizeChanged(object sender, EventArgs e)
        {
        }

        private void btnСохранить_Click(object sender, EventArgs e)
        {
            СохранитьСписок();
        }

        public static void ЗагрузитьСписок()
        {
            StaticSoundRecords.Clear();
            ID = 0;

            System.IO.StreamReader file = null;

            try
            {
                file = new System.IO.StreamReader("StaticSound.ini");

                string line;

                while ((line = file.ReadLine()) != null)
                {
                    string[] Settings = line.Split(';');
                    if (Settings.Length == 4)
                    {
                        StaticSoundRecord Данные;

                        Данные.ID = int.Parse(Settings[0]);
                        Данные.Name = Settings[1];
                        Данные.Message = Settings[2];
                        Данные.Path = Settings[3];

                        StaticSoundRecords.Add(Данные);

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
                System.IO.StreamWriter DumpFile = new System.IO.StreamWriter("StaticSound.ini");

                foreach (var Данные in StaticSoundRecords)
                    DumpFile.WriteLine(Данные.ID.ToString() + ";" + Данные.Name + ";" + Данные.Message + ";" + Данные.Path);

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
                button6.Text = "Пуск";
                Player.PlayFile("");
                return;
            }

            ListView.SelectedIndexCollection sic = this.listView1.SelectedIndices;

            foreach (int item in sic)
            {
                Player.PlayFile(this.textBox_Path.Text);
                return;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SoundFileStatus status = Player.GetFileStatus();

            if (status == SoundFileStatus.Playing)
            {
                int CurrentPosition = Player.GetCurrentPosition();
                float Duration = Player.GetDuration();

                button6.Text = "Стоп";
                Player_Label.Text = (CurrentPosition / 60).ToString() + ":" + (CurrentPosition % 60).ToString("00") + " / " + (Duration / 60).ToString("0") + ":" + (Duration % 60).ToString("00");
            }
            else
            {
                button6.Text = "Пуск";

                if ((status == SoundFileStatus.Paused) || (status == SoundFileStatus.Stop))
                {
                    int CurrentPosition = Player.GetCurrentPosition();
                    float Duration = Player.GetDuration();
                    Player_Label.Text = (CurrentPosition / 60).ToString() + ":" + (CurrentPosition % 60).ToString("00") + " / " + (Duration / 60).ToString("0") + ":" + (Duration % 60).ToString("00");
                }
            }
        }

        public static string GetFilePath(string Name)
        {
            foreach (var Item in StaticSoundRecords)
                if (Item.Name == Name)
                    return Item.Path;

            return "";
        }
    }
}
