namespace CommunicationDevices.Infrastructure
{
    public interface IUniversalInputType
    {
        string Address { get; set; }                          //адресс устройсва
        string Message { get; set; }                          //сообщение
        byte[] Buffer { get; set; }                           //добавочная информация
        string NumberOfTrain { get; set; }
        string PathNumber { get; set; }
    }


    public class UniversalInputType : IUniversalInputType
    {
        public string Address { get; set; }
        public string Message { get; set; }
        public byte[] Buffer { get; set; }
        public string NumberOfTrain { get; set; }
        public string PathNumber { get; set; }
    }
}