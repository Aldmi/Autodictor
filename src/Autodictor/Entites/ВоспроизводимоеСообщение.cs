namespace MainExample.Entites
{
    public enum SoundType { Статическое, Динамическое }

    public class ВоспроизводимоеСообщение
    {
        public SoundType ТипСообщения;
        public bool Воспроизведен { get; set; }
        public Priority Приоритет { get; set; }

        public int RootId { get; set; }                         //Id корня, стастика- СтатическоеСообщение.Id, динамика- SoundRecord.Id
        public int? ParentId { get; set; }                      //Id родителя, стастика- null, динамика- СостояниеФормируемогоСообщенияИШаблон.Id

        public string ИмяВоспроизводимогоФайла { get; set; }
        public NotificationLanguage Язык { get; set; }
        public int? ВремяПаузы { get; set; }                    //Если указанно, значит сообщение это пауза
    }
}