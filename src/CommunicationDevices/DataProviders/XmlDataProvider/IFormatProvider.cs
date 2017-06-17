using System.Collections.Generic;
using System.Xml.Linq;



namespace CommunicationDevices.DataProviders.XmlDataProvider
{
    public interface IFormatProvider
    {
        string CreateDoc(IEnumerable<UniversalInputType> tables);
    }
}