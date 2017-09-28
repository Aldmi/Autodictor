using System.Collections.Generic;
using System.Xml.Linq;
using CommunicationDevices.DataProviders;

namespace CommunicationDevices.Behavior.GetDataBehavior.ConvertGetedData
{
    public interface IInputDataConverter
    {
        IEnumerable<UniversalInputType> ParseXml2ApkDkschedule(XDocument xDoc);
    }
}