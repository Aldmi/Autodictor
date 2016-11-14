using System;
using System.Windows.Forms;





namespace MainExample
{
    public partial class Оповещение : Form
    {
        public Оповещение(TrainTableRecord РасписаниеПоезда)
        {
            InitializeComponent();

            this.Text = "Шаблон оповещения для поезда: " + РасписаниеПоезда.Num + " - " + РасписаниеПоезда.Name;

            ComboBox[] ComboBoxes = new ComboBox[5] { cB_ПередПребытием, cB_ПослеПрибытия, cB_ВоВремяСтоянки, cB_ПередОтправлением, cB_ПослеОтправления };
            TextBox[] TextBoxes = new TextBox[5] { tB_ПередПребытием, tB_ПослеПрибытия, tB_ВоВремяСтоянки, tB_ПередОтправлением, tB_ПослеОтправления };
            ComboBox[] ComboBoxes2 = new ComboBox[5] { cB_ПередПребытием_Путь, cB_ПослеПребытия_Путь, cB_ВоВремяСтоянки_Путь, cB_ПередОтправлением_Путь, cB_ПослеОтправления_Путь };

            for (int i = 0; i < 5; i++)
                ComboBoxes2[i].SelectedIndex = 0;

            string[] ШаблонОповещения = РасписаниеПоезда.SoundTemplates.Split(':');
            if (ШаблонОповещения.Length == 15)
            {
                for (int i = 0; i < 5; i++)
                {
                    ComboBoxes[i].Items.Add("Блокировка");
                    foreach (var Item in DynamicSoundForm.DynamicSoundRecords)
                        ComboBoxes[i].Items.Add(Item.Name);

                    if (ComboBoxes[i].Items.Contains(ШаблонОповещения[3 * i]))
                        ComboBoxes[i].SelectedIndex = ComboBoxes[i].Items.IndexOf(ШаблонОповещения[3 * i]);

                    int ТипОповещенияПути = 0;
                    int.TryParse(ШаблонОповещения[3 * i + 2], out ТипОповещенияПути);
                    if ((ТипОповещенияПути < 0) || (ТипОповещенияПути > 3))
                        ТипОповещенияПути = 0;

                    ComboBoxes2[i].SelectedIndex = ТипОповещенияПути;
                    
                    TextBoxes[i].Text = ШаблонОповещения[3 * i + 1];
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        public string ПолучитьШаблоныОповещения()
        {
            return
                cB_ПередПребытием.Text + ":" + tB_ПередПребытием.Text + ":" + cB_ПередПребытием_Путь.SelectedIndex.ToString() + ":" +
                cB_ПослеПрибытия.Text + ":" + tB_ПослеПрибытия.Text + ":" + cB_ПослеПребытия_Путь.SelectedIndex.ToString() + ":" +
                cB_ВоВремяСтоянки.Text + ":" + tB_ВоВремяСтоянки.Text + ":" + cB_ВоВремяСтоянки_Путь.SelectedIndex.ToString() + ":" +
                cB_ПередОтправлением.Text + ":" + tB_ПередОтправлением.Text + ":" + cB_ПередОтправлением_Путь.SelectedIndex.ToString() + ":" +
                cB_ПослеОтправления.Text + ":" + tB_ПослеОтправления.Text + ":" + cB_ПослеОтправления_Путь.SelectedIndex.ToString();
        }
    }
}
