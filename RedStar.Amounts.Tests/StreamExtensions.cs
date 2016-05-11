using System.IO;
using System.Text;
using System.Xml;

namespace RedStar.Amounts.Tests
{
    internal static class StreamExtensions
    {

        public static string ToXmlString(this Stream stream)
        {
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
                Indent = true,
            };

            StreamReader reader = new StreamReader(stream);
            NameTable nt = new NameTable();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(reader.ReadToEnd());
            StringBuilder sb = new StringBuilder();
            using (XmlWriter xwri = XmlWriter.Create(sb, settings))
            {
                doc.WriteTo(xwri);
            }
            return sb.ToString();
        }
    }
}