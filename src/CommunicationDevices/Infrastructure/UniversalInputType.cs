namespace CommunicationDevices.Infrastructure
{
    public interface IUniversalInputType
    {
        string Message { get; set; }
        byte[] Buffer { get; set; }
    }


    public class UniversalInputType : IUniversalInputType
    {
        public string Message { get; set; }
        public byte[] Buffer { get; set; }
    }
}