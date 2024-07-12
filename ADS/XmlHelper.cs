using System.Xml;

namespace ADS
{
    public class XmlHelper
    {
        public string GetValueXml(string xPath, string document)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(document);
            return xmlDocument.SelectSingleNode(xPath) == null ? string.Empty : xmlDocument.SelectSingleNode(xPath).InnerText;
        }
    }
}
