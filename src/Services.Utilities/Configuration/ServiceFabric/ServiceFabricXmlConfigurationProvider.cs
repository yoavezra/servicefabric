// <copyright file="ServiceFabricXmlConfigurationProvider.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Services.Utilities.Configuration.ServiceFabric
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Injects Service Fabric configurations into ASP.net IConfiguration object
    /// </summary>
    public class ServiceFabricXmlConfigurationProvider : FileConfigurationProvider
    {
        public ServiceFabricXmlConfigurationProvider(ServiceFabricXmlConfigurationSource source)
            : base(source)
        {
        }

        public override void Load(Stream stream)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
            var xmlReaderSettings = new XmlReaderSettings()
            {
                CloseInput = false, // caller will close the stream
                DtdProcessing = DtdProcessing.Prohibit,
                IgnoreComments = true,
                IgnoreWhitespace = true,
            };

            Settings settings;

            using (XmlReader reader = XmlReader.Create(stream, xmlReaderSettings))
            {
                settings = (Settings)xmlSerializer.Deserialize(reader);
            }

            if (settings?.Sections == null)
            {
                return;
            }

            var dataKeyValuePairs = settings?.Sections?
                                    .Where(section => section.Parameters != null)
                                    .SelectMany(section =>
                                    {
                                        return section.Parameters.Select(parameter =>
                                                new KeyValuePair<string, string>($"{section.Name}:{parameter.Name}", parameter.Value));
                                    });

            dataKeyValuePairs.Aggregate(
                this.Data,
                (data, dataKeyValuePair) =>
                {
                    data[dataKeyValuePair.Key] = dataKeyValuePair.Value;
                    return data;
                });
        }
#pragma warning disable CA1034 // Nested types should not be visible - Serialization

#pragma warning disable CA1812 // Avoid uninstantiated internal classes - Serialization

#pragma warning disable CA2227 // Collection properties should be read only - Serialization
        [XmlRoot(ElementName = "Parameter", Namespace = "http://schemas.microsoft.com/2011/01/fabric")]

        public class Parameter
        {
            [XmlAttribute(AttributeName = "Name")]
            public string Name { get; set; }

            [XmlAttribute(AttributeName = "Value")]
            public string Value { get; set; }
        }

        [XmlRoot(ElementName = "Section", Namespace = "http://schemas.microsoft.com/2011/01/fabric")]
        public class Section
        {
            [XmlElement(ElementName = "Parameter", Namespace = "http://schemas.microsoft.com/2011/01/fabric")]
            public List<Parameter> Parameters { get; set; }

            [XmlAttribute(AttributeName = "Name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "Settings", Namespace = "http://schemas.microsoft.com/2011/01/fabric")]
        public class Settings
        {
            [XmlElement(ElementName = "Section", Namespace = "http://schemas.microsoft.com/2011/01/fabric")]
            public List<Section> Sections { get; set; }
        }
#pragma warning restore CA2227
#pragma warning restore CA1812
#pragma warning restore CA1034
    }
}