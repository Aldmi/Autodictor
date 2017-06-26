﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MainExample.Entites;

namespace MainExample
{
    public partial class TechnicalMessageForm : Form
    {
        #region prop

        private List<DynamicSoundRecord> DynamicTechnicalSoundRecords { get; }= new List<DynamicSoundRecord>();
        public static List<SoundRecord> SoundRecords { get; } = new List<SoundRecord>();  //Добавленные для воспроизведения сообщения

        #endregion





        #region ctor

        public TechnicalMessageForm()
        {
            InitializeComponent();
        }

        #endregion





        protected override void OnLoad(EventArgs e)
        {
            LoadDynamicTechniclaTemplate();

            //cBШаблонОповещения.Items.Add("Блокировка");
            foreach (var item in DynamicTechnicalSoundRecords)
                cBШаблонОповещения.Items.Add(item.Name);


            cBПутьПоУмолчанию.Items.Add("Не определен");
            foreach (var путь in Program.НомераПутей)
                cBПутьПоУмолчанию.Items.Add(путь);
            cBПутьПоУмолчанию.SelectedIndex = 0;

            base.OnLoad(e);
        }





        private void LoadDynamicTechniclaTemplate()
        {
            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader("DynamicSoundTechnical.ini"))
                {
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

                            DynamicTechnicalSoundRecords.Add(Данные);
                        }
                    }

                    file.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки файла DynamicSoundTechnical: {ex.Message}");
            }
        }



        private СостояниеФормируемогоСообщенияИШаблон СоздатьСостояниеФормируемогоСообщенияИШаблон(int soundRecordId, DynamicSoundRecord template)
        {
            СостояниеФормируемогоСообщенияИШаблон новыйШаблон;
            новыйШаблон.Id = 1;
            новыйШаблон.SoundRecordId = soundRecordId;
            новыйШаблон.Активность = true;
            новыйШаблон.Приоритет = Priority.Hight;
            новыйШаблон.Воспроизведен = false;
            новыйШаблон.СостояниеВоспроизведения = SoundRecordStatus.ДобавленВОчередьРучное;
            новыйШаблон.ВремяСмещения = 0;
            новыйШаблон.НазваниеШаблона = template.Name;
            новыйШаблон.Шаблон = template.Message;
            новыйШаблон.ПривязкаКВремени = 0;
            новыйШаблон.ЯзыкиОповещения = new List<NotificationLanguage> { NotificationLanguage.Ru, NotificationLanguage.Eng };

            return новыйШаблон;
        }



        private SoundRecord СоздатьSoundRecord(int soundRecordId, string pathNumber, СостояниеФормируемогоСообщенияИШаблон template)
        {
            SoundRecord record = new SoundRecord
            {
                ID = soundRecordId,
                НомерПоезда = "xxx",
                НомерПути = pathNumber,
                Время = DateTime.Now,
                СписокФормируемыхСообщений = new List<СостояниеФормируемогоСообщенияИШаблон> {template},
                КоличествоПовторений = 1
            };

            return record;
        }







        #region EventHandler

        private void btn_Play_Click(object sender, EventArgs e)
        {              
            if (cBШаблонОповещения.SelectedIndex < 0)
            {
                MessageBox.Show(@"Шаблон не выбранн !!!");
                return;
            }

            if (cBПутьПоУмолчанию.SelectedIndex < 1)
            {
                MessageBox.Show(@"Путь не выбранн !!!");
                return;
            }

            var template = DynamicTechnicalSoundRecords[cBШаблонОповещения.SelectedIndex];
            var pathNumber = cBПутьПоУмолчанию.Text;

            if(template.Name.Contains("---"))
            {
                MessageBox.Show(@"Выбран разделитель вместо шаблона !!!");
                return;
            }

            var newId = SoundRecords.Any() ? SoundRecords.Max(rec => rec.ID) + 1 : 1;
            var формируемоеСообщение = СоздатьСостояниеФормируемогоСообщенияИШаблон(newId, template);
            var record = СоздатьSoundRecord(newId, pathNumber, формируемоеСообщение);

            SoundRecords.Add(record);
            MainWindowForm.ВоспроизвестиШаблонОповещения("Техническое сообщение", record, формируемоеСообщение, ТипСообщения.ДинамическоеТехническое);
        }

        #endregion
    }
}
