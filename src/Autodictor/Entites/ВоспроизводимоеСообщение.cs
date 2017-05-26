using System.Collections.Generic;

namespace MainExample.Entites
{


    public class ВоспроизводимоеСообщение
    {
        public Priority Приоритет { get; set; }

        public int RootId { get; set; }                                         //Id корня, стастика- СтатическоеСообщение.Id, динамика- SoundRecord.Id
        public int? ParentId { get; set; }                                       //Id родителя, стастика- null, динамика- СостояниеФормируемогоСообщенияИШаблон.Id

        public string ИмяВоспроизводимогоФайла { get; set; }
        public NotificationLanguage Язык { get; set; }
        public int? ВремяПаузы { get; set; }                                       //Если указанно, значит сообщение это пауза

        public Queue<ВоспроизводимоеСообщение> ОчередьШаблона { get; set; }        //все файлы шаблона хранятся в этой коллекции (для статики null)
    }
}