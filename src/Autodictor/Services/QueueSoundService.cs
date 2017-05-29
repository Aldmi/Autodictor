using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using MainExample.Entites;
using WCFCis2AvtodictorContract.DataContract;


namespace MainExample.Services
{
    public enum StatusPlaying { Start, Playing, Stop }


    public class QueueSoundService : IDisposable
    {
        #region Field

        private int _pauseTime;

        #endregion





        #region prop

        private Queue<ВоспроизводимоеСообщение> Queue { get; set; } = new Queue<ВоспроизводимоеСообщение>();
        public IEnumerable<ВоспроизводимоеСообщение> GetElements => Queue;
        public int Count => Queue.Count;

        public bool IsStaticSoundPlaying =>(MainWindowForm.QueueSound.CurrentTemplatePlaying == null) &&
                                           (MainWindowForm.QueueSound.CurrentSoundMessagePlaying != null);


        public ВоспроизводимоеСообщение CurrentSoundMessagePlaying { get; private set; }
        public СостояниеФормируемогоСообщенияИШаблон? CurrentTemplatePlaying { get; private set; }


        private List<ВоспроизводимоеСообщение> ElementsOnTemplatePlaying { get; set; }
        public IEnumerable<ВоспроизводимоеСообщение> GetElementsOnTemplatePlaying => ElementsOnTemplatePlaying;


        public IEnumerable<ВоспроизводимоеСообщение> GetElementsWithFirstElem
        {
            get
            {
                var result= new List<ВоспроизводимоеСообщение>();
                if (IsStaticSoundPlaying)
                {
                    result.Add(CurrentSoundMessagePlaying);
                    result.AddRange(GetElements);
                }
                else
                {
                    result.AddRange(GetElements);
                }

                return result;
            }
        }


        public bool IsWorking { get; private set; }

        #endregion




        #region Rx

        public Subject<StatusPlaying> QueueChangeRx { get; } = new Subject<StatusPlaying>();             //Событие определния начала/конца проигрывания ОЧЕРЕДИ
        public Subject<StatusPlaying> SoundMessageChangeRx { get; } = new Subject<StatusPlaying>();      //Событие определния начала/конца проигрывания ФАЙЛА
        public Subject<StatusPlaying> TemplateChangeRx { get; } = new Subject<StatusPlaying>();          //Событие определния начала/конца проигрывания динамического ШАБЛОНА
        public Subject<StatusPlaying> StaticChangeRx { get; } = new Subject<StatusPlaying>();            //Событие определния начала/конца проигрывания  статического ФАЙЛА

        #endregion




        #region Methode

        public void StartQueue()
        {
            IsWorking = true;
        }


        public void StopQueue()
        {
            IsWorking = false;
        }




        /// <summary>
        /// Добавить элемент в очередь
        /// </summary>
        public void AddItem(ВоспроизводимоеСообщение item)
        {
            if (item == null)
                return;

            //делать сортировку по приоритету.
            if (item.Приоритет == Priority.Low)
            {
                Queue.Enqueue(item);
            }
            else
            {
                Queue.Enqueue(item);
                Queue = new Queue<ВоспроизводимоеСообщение>(Queue.OrderByDescending(elem=>elem.Приоритет));
            }
        }



        /// <summary>
        /// Очистить очередь
        /// </summary>
        public void Clear()
        {
            Queue.Clear();
            ElementsOnTemplatePlaying.Clear();
            CurrentTemplatePlaying = null;
            CurrentSoundMessagePlaying = null;
        }




        /// <summary>
        /// Разматывание очереди, внешним кодом
        /// </summary>
        private bool _isAnyOldQueue;
        private bool _isEmptyRaiseQueue;
        public void Invoke()
        {
            if(!IsWorking)
                return;

            try
            {
                SoundFileStatus status = Player.GetFileStatus();

                //Определение событий Начала проигрывания очереди и конца проигрывания очереди.-----------------
                if (Queue.Any() && !_isAnyOldQueue && CurrentSoundMessagePlaying == null)
                {
                    EventStartPlayingQueue();
                }

                if (!Queue.Any() && _isAnyOldQueue)
                {
                    _isEmptyRaiseQueue = true;
                }

                if ((CurrentSoundMessagePlaying != null) && (status != SoundFileStatus.Playing))
                {
                    EventEndPlayingSoundMessage(CurrentSoundMessagePlaying);
                }

                if (!Queue.Any() && _isEmptyRaiseQueue && (status != SoundFileStatus.Playing)) // ожидание проигрывания последнего файла.
                {
                    _isEmptyRaiseQueue = false;
                    CurrentSoundMessagePlaying = null;
                    EventEndPlayingQueue();
                }

                _isAnyOldQueue = Queue.Any();



                //Разматывание очереди. Определение проигрываемого файла-----------------------------------------------------------------------------
                if (status != SoundFileStatus.Playing)
                {
                    if (_pauseTime > 0)
                    {
                        _pauseTime--;
                        return;
                    }

                    if (Queue.Any())
                    {
                        var peekItem = Queue.Peek();
                        if (peekItem.ОчередьШаблона == null)               //Статическое сообщение
                        {
                            CurrentSoundMessagePlaying = Queue.Dequeue();
                            ElementsOnTemplatePlaying = null;
                        }
                        else                                              //Динамическое сообщение
                        {
                            if (peekItem.ОчередьШаблона.Any())
                            {
                                ElementsOnTemplatePlaying = peekItem.ОчередьШаблона.ToList();//DEBUG
                                var item = peekItem.ОчередьШаблона.Dequeue();

                                if ((CurrentSoundMessagePlaying == null) ||
                                    (CurrentSoundMessagePlaying.ParentId != item.ParentId &&
                                     CurrentSoundMessagePlaying.RootId != item.RootId))
                                {
                                    EventStartPlayingTemplate(item);
                                }
                                CurrentSoundMessagePlaying = item;
                            }
                            else
                            {
                                Queue.Dequeue();
                                EventEndPlayingTemplate(CurrentSoundMessagePlaying);
                                CurrentSoundMessagePlaying = null;
                                ElementsOnTemplatePlaying = null;
                            }
                        }


                        //Проигрывание фалйла-----------------------------------------------------------------------------------------------------------
                        if (CurrentSoundMessagePlaying == null)
                            return;

                        var названиеФайла = CurrentSoundMessagePlaying.ИмяВоспроизводимогоФайла;
                        var язык = CurrentSoundMessagePlaying.Язык;

                        if (CurrentSoundMessagePlaying.ВремяПаузы.HasValue)                         //воспроизводимое сообщение - это ПАУЗА
                        {
                            _pauseTime = CurrentSoundMessagePlaying.ВремяПаузы.Value;
                            //CurrentSoundMessagePlaying = null;//???
                            return;
                        }

                        if (названиеФайла.Contains(".wav") == false)
                            названиеФайла = Program.GetFileName(названиеФайла, язык);

                        if (Player.PlayFile(названиеФайла) == true)
                            EventStartPlayingSoundMessage(CurrentSoundMessagePlaying);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Invoke = {ex.ToString()}");//DEBUG
            }


        }


        /// <summary>
        /// Событие НАЧАЛА проигрывания очереди.
        /// До этого момента очередь была пуста.
        /// </summary>
        private void EventStartPlayingQueue()
        {
            //Debug.WriteLine("НАЧАЛО ПРОИГРЫВАНИЯ ОЧЕРЕДИ *********************");//DEBUG
            QueueChangeRx.OnNext(StatusPlaying.Start);
        }



        /// <summary>
        /// Событие КОНЦА проигрывания очереди.
        /// До этого момента из очереди проигрывался послдений файл.
        /// </summary>
        private void EventEndPlayingQueue()
        {
            //Debug.WriteLine("КОНЕЦ ПРОИГРЫВАНИЯ ОЧЕРЕДИ *********************");//DEBUG
            QueueChangeRx.OnNext(StatusPlaying.Stop);
        }



        /// <summary>
        /// Событие НАЧАЛА проигрывания элемента из очереди.
        /// </summary>
        private void EventStartPlayingSoundMessage(ВоспроизводимоеСообщение soundMessage)
        {
            SoundMessageChangeRx.OnNext(StatusPlaying.Start);

            if(IsStaticSoundPlaying)
                EventStartPlayingStatic(soundMessage);


           // Debug.WriteLine($"начало проигрывания файла: {soundMessage.ИмяВоспроизводимогоФайла}");//DEBUG
        }



        /// <summary>
        /// Событие КОНЦА проигрывания элемента из очереди.
        /// </summary>
        private void EventEndPlayingSoundMessage(ВоспроизводимоеСообщение soundMessage)
        {
            SoundMessageChangeRx.OnNext(StatusPlaying.Stop);

            if (IsStaticSoundPlaying)
                EventEndPlayingStatic(soundMessage);

            //Debug.WriteLine($"конец проигрывания файла: {soundMessage.ИмяВоспроизводимогоФайла}");//DEBUG
        }



        /// <summary>
        /// Событие НАЧАЛА проигрывания шаблона.
        /// </summary>
        private void EventStartPlayingTemplate(ВоспроизводимоеСообщение soundMessage)
        {
            var soundRecord = MainWindowForm.SoundRecords.FirstOrDefault(rec => rec.Value.ID == soundMessage.RootId).Value;
            if (soundRecord.СписокФормируемыхСообщений != null && soundRecord.СписокФормируемыхСообщений.Any())
            {
                var template = soundRecord.СписокФормируемыхСообщений.FirstOrDefault(sm => sm.Id == soundMessage.ParentId.Value);
                if (!string.IsNullOrEmpty(template.НазваниеШаблона))
                {
                    CurrentTemplatePlaying = template;
                    TemplateChangeRx.OnNext(StatusPlaying.Start);
                    Debug.WriteLine($"-------------НАЧАЛО проигрывания ШАБЛОНА: НазваниеШаблона= {template.НазваниеШаблона}-----------------");//DEBUG
                }
            }
        }



        /// <summary>
        /// Событие КОНЦА проигрывания шаблона.
        /// </summary>
        private void EventEndPlayingTemplate(ВоспроизводимоеСообщение soundMessage)
        {
            var soundRecord = MainWindowForm.SoundRecords.FirstOrDefault(rec => rec.Value.ID == soundMessage.RootId).Value;
            if (soundMessage.ParentId.HasValue)
            {
                var template = soundRecord.СписокФормируемыхСообщений.FirstOrDefault(sm => sm.Id == soundMessage.ParentId.Value);
                if (!string.IsNullOrEmpty(template.НазваниеШаблона))
                {
                    CurrentTemplatePlaying = null;
                    TemplateChangeRx.OnNext(StatusPlaying.Start);
                    Debug.WriteLine($"--------------КОНЕЦ проигрывания ШАБЛОНА: НазваниеШаблона= {template.НазваниеШаблона}-----------------");//DEBUG
                }
            }

            CurrentTemplatePlaying = null;
        }


        /// <summary>
        /// Событие НАЧАЛА проигрывания статического файла.
        /// </summary>
        private void EventStartPlayingStatic(ВоспроизводимоеСообщение soundMessage)
        {
            StaticChangeRx.OnNext(StatusPlaying.Start);
             Debug.WriteLine($"^^^^^^^^^^^СТАТИКА НАЧАЛО {soundMessage.ИмяВоспроизводимогоФайла}");//DEBUG
        }



        /// <summary>
        /// Событие КОНЦА проигрывания статического файла.
        /// </summary>
        private void EventEndPlayingStatic(ВоспроизводимоеСообщение soundMessage)
        {
            StaticChangeRx.OnNext(StatusPlaying.Stop);
            Debug.WriteLine($"^^^^^^^^^^^СТАТИКА КОНЕЦ: {soundMessage.ИмяВоспроизводимогоФайла}");//DEBUG
        }

        #endregion




        #region IDisposable

        public void Dispose()
        {
            if (!QueueChangeRx.IsDisposed)
                QueueChangeRx.Dispose();

            if (!SoundMessageChangeRx.IsDisposed)
                SoundMessageChangeRx.Dispose();

            if (!TemplateChangeRx.IsDisposed)
                TemplateChangeRx.Dispose();
        }

        #endregion
    }
}