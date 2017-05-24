using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using MainExample.Entites;

namespace MainExample.Services
{
    public class QueueSoundService : IDisposable
    {
        private int _pauseTime;




        #region prop

        private Queue<ВоспроизводимоеСообщение> Queue { get; set; } = new Queue<ВоспроизводимоеСообщение>();

        public ВоспроизводимоеСообщение CurrentSoundMessagePlaying { get; private set; }

        public СостояниеФормируемогоСообщенияИШаблон? CurrentTemplatePlaying { get; private set; }

        #endregion







        #region Methode

        /// <summary>
        /// Добавить элемент в очередь
        /// </summary>
        public void AddItem(ВоспроизводимоеСообщение item)
        {
            if (item == null)
                return;

            Queue.Enqueue(item);
        }


        public void Clear()
        {
            Queue.Clear();
        }



        /// <summary>
        /// Добавить шаблон в очередь.
        /// </summary>
        //public void AddTemplate( ref СостояниеФормируемогоСообщенияИШаблон template)
        //{





        //}



        /// <summary>
        /// Разматывание очереди, внешним кодом
        /// </summary>
        private bool _isAnyOldQueue;
        private bool _isEmptyRaiseQueue;
        public void Invoke()
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



            //Разматывание очереди-----------------------------------------------------------------------------
            if (status != SoundFileStatus.Playing)
            {
                if (_pauseTime > 0)
                {
                    _pauseTime--;
                    return;
                }

                if (Queue.Any())
                {
                    CurrentSoundMessagePlaying = Queue.Dequeue();
                    var названиеФайла = CurrentSoundMessagePlaying.ИмяВоспроизводимогоФайла;
                    var язык = CurrentSoundMessagePlaying.Язык;

                    if (CurrentSoundMessagePlaying.ВремяПаузы.HasValue)                         //воспроизводимое сообщение - это ПАУЗА
                    {
                        _pauseTime = CurrentSoundMessagePlaying.ВремяПаузы.Value;
                        CurrentSoundMessagePlaying = null;
                        return;
                    }

                    if (названиеФайла.Contains(".wav") == false)
                        названиеФайла = Program.GetFileName(названиеФайла, язык);

                    if (Player.PlayFile(названиеФайла) == true)
                        EventStartPlayingSoundMessage(CurrentSoundMessagePlaying);
                }
            }

        }


        /// <summary>
        /// Событие НАЧАЛА проигрывания очереди.
        /// До этого момента очередь была пуста.
        /// </summary>
        private void EventStartPlayingQueue()
        {
            Debug.WriteLine("НАЧАЛО ПРОИГРЫВАНИЯ ОЧЕРЕДИ");//DEBUG
        }



        /// <summary>
        /// Событие КОНЦА проигрывания очереди.
        /// До этого момента из очереди проигрывался послдений файл.
        /// </summary>
        private void EventEndPlayingQueue()
        {
            Debug.WriteLine("КОНЕЦ ПРОИГРЫВАНИЯ ОЧЕРЕДИ");//DEBUG
        }



        /// <summary>
        /// Событие НАЧАЛА проигрывания элемента из очереди.
        /// </summary>
        private void EventStartPlayingSoundMessage(ВоспроизводимоеСообщение soundMessage)
        {
            Debug.WriteLine($"начало проигрывания файла: {soundMessage.ИмяВоспроизводимогоФайла}");//DEBUG

            if (soundMessage.ТипСообщения == SoundType.Статическое)
            {
                CurrentTemplatePlaying = null;
                return;
            }

            //Определение НАЧАЛА проигрывания ШАБЛОНА----------------------------------------------------
            if (CurrentTemplatePlaying != null)
                return;

            var soundRecord = MainWindowForm.SoundRecords.FirstOrDefault(rec => rec.Value.ID == soundMessage.RootId).Value;
            if (soundMessage.ParentId.HasValue)
            {
                var template = soundRecord.СписокФормируемыхСообщений.FirstOrDefault(sm => sm.Id == soundMessage.ParentId.Value);
                if (!string.IsNullOrEmpty(template.НазваниеШаблона))
                {
                    CurrentTemplatePlaying = template;
                    EventStartPlayingTemplate(CurrentTemplatePlaying.Value);
                }
            }         
        }



        /// <summary>
        /// Событие КОНЦА проигрывания элемента из очереди.
        /// </summary>
        private void EventEndPlayingSoundMessage(ВоспроизводимоеСообщение soundMessage)
        {
            Debug.WriteLine($"конец проигрывания файла: {soundMessage.ИмяВоспроизводимогоФайла}");//DEBUG

            //Определение КОНЦА проигрывания ШАБЛОНА----------------------------------------------------
            if (Queue.Any() && CurrentTemplatePlaying.HasValue)
            {
                var peekItem = Queue.Peek();

                if ((peekItem.ParentId == CurrentTemplatePlaying.Value.Id) &&
                    (peekItem.RootId == CurrentTemplatePlaying.Value.SoundRecordId))
                {
                    var g = 5 + 5;
                }
                else
                {
                    EventEndPlayingTemplate(CurrentTemplatePlaying.Value);
                    var g = 5 + 5;
                }
            }
            else
            {
                EventEndPlayingTemplate(CurrentTemplatePlaying.Value);
            }


        }



        /// <summary>
        /// Событие НАЧАЛА проигрывания шаблона.
        /// </summary>
        private void EventStartPlayingTemplate(СостояниеФормируемогоСообщенияИШаблон template)
        {
            Debug.WriteLine($"начало проигрывания ШАБЛОНА: {template.НазваниеШаблона}   Id: {template.Id}");//DEBUG
        }


        /// <summary>
        /// Событие КОНЦА проигрывания шаблона.
        /// </summary>
        private void EventEndPlayingTemplate(СостояниеФормируемогоСообщенияИШаблон template)
        {
            Debug.WriteLine($"конец проигрывания ШАБЛОНА: {template.НазваниеШаблона}   Id: {template.Id}");//DEBUG
            CurrentTemplatePlaying = null;
        }

        #endregion






        #region IDisposable

        public void Dispose()
        {

        }

        #endregion
    }
}