using System.Xml.Linq;

namespace XMLReader.Readers;

internal class XmlFileReader
{
    private readonly string _filePath;

    public XmlFileReader(string filePath)
    {
        _filePath = filePath;
    }

    public XDocument XDocument => GetXmlDocument();

    public IEnumerable<XElement> GetAllElements(string elementName = "")
    {
        try
        {
            if (XDocument is null) return [];
            if (string.IsNullOrWhiteSpace(elementName))
            {
                return XDocument.Descendants().ToList();
            }

            return XDocument.Descendants(elementName).ToList();
        }
        catch(Exception ex)
        {
            throw ex;
        } 
    }

    private XDocument GetXmlDocument()
    {
        try
        {
            var fileInfo = new FileInfo(_filePath);
            if(fileInfo is null)
            {
                throw new Exception($"Не создан тип {nameof(FileInfo)}");
            }
            
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException();
            }
                   
            if (string.IsNullOrWhiteSpace(fileInfo.Extension) || !fileInfo.Extension.ToLower().Equals(".xml"))
            {
                throw new Exception($"Файл имеет не корректное расширение - [{fileInfo.Extension}]");
            }  
  
            using var fileStream = File.OpenRead(_filePath);
            fileStream.Position = 0;
            return XDocument.Load(fileStream, LoadOptions.None);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
