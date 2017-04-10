namespace MainExample.Entites
{
    public class ВоспроизводимоеСообщение
    {
        public string ИмяВоспроизводимогоФайла;
        public NotificationLanguage Язык;

        public int? ВремяПаузы;            //Если указанно, значит сообщение это пауза
    }
}