using System;
using System.IO;
using System.Xml;

namespace MonkeyPants.Reading
{
    /// <summary>
    /// Adds some convenience methods to XmlReader. Not exposing any XmlReader methods we're not using, just out of laziness.
    /// </summary>
    public class XmlReaderAdapter
    {
        public static XmlReaderAdapter Create(StreamReader reader)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;
            settings.IgnoreWhitespace = true;

            return new XmlReaderAdapter(XmlReader.Create(new StringReader(reader.ReadToEnd()), settings));
        }

        private readonly XmlReader reader;

        public XmlReaderAdapter(XmlReader reader)
        {
            this.reader = reader;
        }

        public bool IsEmptyElement
        {
            get { return reader.IsEmptyElement; }
        }

        public void MoveToNextElement()
        {
            do
            {
                reader.Read();
            } while (reader.NodeType != XmlNodeType.Element && reader.NodeType != XmlNodeType.EndElement);
        }

        public void MoveToNextElement(string elementName)
        {
            do
            {
                MoveToNextElement();
            } while (reader.Name != elementName);
        }

        public bool MoveToElement()
        {
            return reader.MoveToElement();
        }

        public void AssertIsStartElement(string elementName)
        {
            if (!reader.IsStartElement(elementName)) throw new ApplicationException("Invalid reader state");
        }

        public bool IsAttribute(string name)
        {
            return reader.NodeType == XmlNodeType.Attribute && reader.Name == name;
        }

        public bool IsEndElement(string elementName)
        {
            return reader.NodeType == XmlNodeType.EndElement && reader.Name == elementName;
        }

        public bool IsStartElement(string name)
        {
            return reader.IsStartElement(name);
        }

        public bool MoveToNextAttribute()
        {
            return reader.MoveToNextAttribute();
        }

        public bool MoveToAttribute(string name)
        {
            return reader.MoveToAttribute(name);
        }

        public int ReadContentAsInt()
        {
            return reader.ReadContentAsInt();
        }

        public string ReadContentAsString()
        {
            return reader.ReadContentAsString();
        }

        public string ReadElementContentAsString()
        {        
            return reader.ReadElementContentAsString();
        }

        public bool ReadElementContentAsBoolean()
        {
            return reader.ReadElementContentAsBoolean();
        }

        public int ReadElementContentAsInt()
        {
            return reader.ReadElementContentAsInt();
        }

        public DateTime ReadElementContentAsDateTime()
        {
            return reader.ReadElementContentAsDateTime();
        }
    }
}