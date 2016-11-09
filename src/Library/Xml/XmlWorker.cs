using System.IO;
using System.Xml.Linq;

namespace Library.Xml
{
    static public class XmlWorker
    {
        public static XElement LoadXmlFile(string folderName, string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), $"{folderName}\\{fileName}");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"XML файл не НАЙДЕНН!!!   \"{path} \"");
            }

            return XElement.Load(path);
        }
    }
}