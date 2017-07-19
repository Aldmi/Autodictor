using System.Collections.Generic;


namespace CommunicationDevices.DataProviders.XmlDataProvider
{
    public enum DateTimeFormat
    {
        None,
        Sortable,      //2015-07-17T17:04:43
        LinuxTimeStamp
    }


    public interface IFormatProvider
    {
        string CreateDoc(IEnumerable<UniversalInputType> tables);
    }
}