using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MainExample.Extension;
using MainExample.Services;


namespace MainExample
{
    public partial class СписокВоспроизведения : Form
    {
        #region prop

        private Timer Timer { get; set; }

        #endregion





        #region ctor

        public СписокВоспроизведения()
        {
            Timer= new Timer {Interval = 100};
            Timer.Tick += Timer_Tick;
            InitializeComponent();
        }

        #endregion





        protected override void OnLoad(EventArgs e)
        {
            Timer.Start();
            base.OnLoad(e);
        }


        protected override void OnClosed(EventArgs e)
        {
            if (Timer != null)
            {
                Timer.Tick -= Timer_Tick;
                Timer.Dispose();
            }

            base.OnClosed(e);
        }


        private int _oldCountElemQueue;
        private int _oldCountElemFiles;
        private bool _oldIsStaticSoundPlaying;
        private void Timer_Tick(object sender, EventArgs e)
        {
            var currentCount = MainWindowForm.QueueSound.IsStaticSoundPlaying ? (MainWindowForm.QueueSound.Count + 1) : MainWindowForm.QueueSound.Count;
            if (currentCount != _oldCountElemQueue)
            {
                _oldCountElemQueue = currentCount;
                //Сработка при изменении кол-ва элементов в очереди
                ОбновитьСодержимоеСпискаЭлементов();
                ВизуализироватьСписокЭлементов(lVСписокЭлементов);
                ОбновитьСодержимоеСпискаФайлов();
                return;
            }


            if(MainWindowForm.QueueSound.GetElementsOnTemplatePlaying != null && MainWindowForm.QueueSound.GetElementsOnTemplatePlaying.Any())
            {
                var currentCountElem = MainWindowForm.QueueSound.GetElementsOnTemplatePlaying.Count();
                if (currentCountElem != _oldCountElemFiles)
                {
                    _oldCountElemFiles = currentCountElem;
                    //Сработка при изменении кол-ва элементов в очереди проигрывания шаблона
                    ОбновитьСодержимоеСпискаФайлов();
                }   
            }


            if (MainWindowForm.QueueSound.IsStaticSoundPlaying != _oldIsStaticSoundPlaying)
            {
                _oldIsStaticSoundPlaying = MainWindowForm.QueueSound.IsStaticSoundPlaying;
                ОбновитьСодержимоеСпискаФайлов();
            }
        }




        #region Methode

        public void ОбновитьСодержимоеСпискаЭлементов()
        {
            lVСписокЭлементов.InvokeIfNeeded(() =>
            {
                lVСписокЭлементов.Items.Clear();
                try
                {
                    //проигрывается статика. Добавим первый элемент.
                    if (MainWindowForm.QueueSound.IsStaticSoundPlaying)
                    {
                        ListViewItem lvi1 =new ListViewItem(new string[]{MainWindowForm.QueueSound.CurrentSoundMessagePlaying.ИмяВоспроизводимогоФайла});
                        this.lVСписокЭлементов.Items.Add(lvi1);
                    }

                    foreach (var elem in MainWindowForm.QueueSound.GetElements)
                    {
                        ListViewItem lvi1 = new ListViewItem(new string[] {elem.ИмяВоспроизводимогоФайла});
                        this.lVСписокЭлементов.Items.Add(lvi1);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ОбновитьСодержимоеСпискаЭлементов = {ex.ToString()}");//DEBUG
                }
            });
        }



        public void ОбновитьСодержимоеСпискаФайлов()
        {
            lVСписокФайлов.InvokeIfNeeded(() =>
            {
                lVСписокФайлов.Items.Clear();
                try
                {
                    if (MainWindowForm.QueueSound.IsStaticSoundPlaying)
                    {
                        ListViewItem lvi1 = new ListViewItem(new string[] { MainWindowForm.QueueSound.CurrentSoundMessagePlaying.ИмяВоспроизводимогоФайла });
                        this.lVСписокФайлов.Items.Add(lvi1);
                    }
                    else
                    {
                        if(MainWindowForm.QueueSound.GetElementsOnTemplatePlaying == null)
                            return;

                        foreach (var elem in MainWindowForm.QueueSound.GetElementsOnTemplatePlaying)
                        {
                            ListViewItem lvi1 = new ListViewItem(new string[] { elem.ИмяВоспроизводимогоФайла });
                            this.lVСписокФайлов.Items.Add(lvi1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ОбновитьСодержимоеСпискаФайлов = {ex.ToString()}");//DEBUG
                }
            });
        }


        public void ВизуализироватьСписокЭлементов(ListView lv)
        {
            var listElem = MainWindowForm.QueueSound.GetElements.ToList();
            lVСписокЭлементов.InvokeIfNeeded(() =>
            {
                try
                {
                    for (int item = 0; item < lv.Items.Count; item++)
                    {
                        if (item == 0)
                        {
                            lv.Items[item].Font = new Font(FontFamily.GenericSansSerif, 18);
                            continue;
                        }

                        if (MainWindowForm.QueueSound.Count < lv.Items.Count) // первое сообшение статика (уже удаленно из очереди)
                        {
                            lv.Items[item].ForeColor = listElem[item - 1].ОчередьШаблона == null
                                ? Color.Brown
                                : Color.DodgerBlue;
                        }
                        else
                        {
                            lv.Items[item].ForeColor = listElem[item].ОчередьШаблона == null
                                ? Color.Brown
                                : Color.DodgerBlue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ВизуализироватьСписокЭлементов = {ex.ToString()}");//DEBUG
                };
            });

        }

        #endregion
    }
}
