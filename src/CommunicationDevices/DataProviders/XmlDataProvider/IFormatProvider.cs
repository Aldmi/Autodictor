using System.Collections.Generic;


namespace CommunicationDevices.DataProviders.XmlDataProvider
{
    public enum DateTimeFormat
    {
        None,
        Sortable,
        LinuxTimeStamp
    }


    public interface IFormatProvider
    {
        string CreateDoc(IEnumerable<UniversalInputType> tables);
    }
}