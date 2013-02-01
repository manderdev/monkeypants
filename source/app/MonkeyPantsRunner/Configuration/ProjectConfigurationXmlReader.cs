using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using MonkeyPants.Extensions;

namespace MonkeyPants.Configuration
{
    /// <summary>
    /// todo: load for versions
    /// </summary>
    internal class ProjectConfigurationXmlReader
    {
        public ProjectConfiguration Load(string file)
        {
            XmlDocument doc = OpenFile(file);
            XmlElement rootElement = GetRoot(file, doc);

            XmlElement fixturesElement = GetRequiredSingleElement(rootElement, "fixtures");
            List<string> fixtureAssemblyPaths = GetAssemblyPaths(file, fixturesElement);

            List<InputConfiguration> input = ReadInputConfiguration(rootElement);
            List<OutputConfiguration> output = ReadOutputConfiguration(rootElement);

            return new ProjectConfiguration(fixtureAssemblyPaths, input, output);
        }

        private XmlDocument OpenFile(string file)
        {
            if (!File.Exists(file))
            {
                throw new MonkeyPantsApplicationException(String.Format(                                                              
                                                              "Configuration file {0} not found", Path.GetFullPath(file)));
            }

            var doc = new XmlDocument();
            doc.Load(file);
            return doc;
        }

        private XmlElement GetRoot(string file, XmlDocument doc)
        {
            var root = doc.DocumentElement;
            if (root == null || !root.Name.Equals("monkey_pants_config"))
            {
                throw new MonkeyPantsApplicationException(String.Format(                                                              
                                                              "Root config element {0} not found in file {1}", "monkey_pants_config", file));
            }
            return root;
        }

        /// <summary>
        /// <input>
        ///   <match file="*.xml">
        ///     <reader class="MonkeyPants.Reading.Excel2003XmlReader" assembly="MonkeyPants.exe"/>
        ///   </match>
        /// </input>
        /// </summary>
        private List<InputConfiguration> ReadInputConfiguration(XmlElement rootElement)
        {
            XmlElement inputElement = GetRequiredSingleElement(rootElement, "input");

            List<InputConfiguration> inputConfiguration = new List<InputConfiguration>();
            foreach(XmlElement matchElement in inputElement.GetElementsByTagName("match"))
            {
                inputConfiguration.Add(ReadInputReaderConfiguration(matchElement));
            }
            return inputConfiguration;
        }

        private InputConfiguration ReadInputReaderConfiguration(XmlElement matchElement)
        {
            XmlElement readerElement = GetRequiredSingleElement(matchElement, "reader");
            // todo: this supports builtins only. Identify builtins and support ext loading for custom classes
            string readerAssembly = Assembly.GetExecutingAssembly().Location;
            return new InputConfiguration
                       {
                           Match = matchElement.GetAttribute("file"),
                           Reader = readerElement.GetAttribute("class"),
                           ReaderAssembly = readerAssembly
                       };
        }

        /// <summary>
        /// <output>
        ///    <writer class="MonkeyPants.Output.SimpleTextResultsWriter" assembly="MonkeyPants.exe">
        ///       <channel type="file" file="MonkeyPantsResults.txt"/>
        ///       <channel type="console"/>
        ///    </writer>
        /// </output>
        /// </summary>
        private List<OutputConfiguration> ReadOutputConfiguration(XmlElement rootElement)
        {
            XmlElement outputElement = GetRequiredSingleElement(rootElement, "output");

            List<OutputConfiguration> outputConfiguration = new List<OutputConfiguration>();                
            foreach (XmlElement writerElement in outputElement.GetElementsByTagName("writer"))
            {
                outputConfiguration.Add(ReadOutputWriterConfiguration(writerElement));
            }
            return outputConfiguration;
        }

        private OutputConfiguration ReadOutputWriterConfiguration(XmlElement writerElement)
        {
            XmlElement channelElement = GetRequiredSingleElement(writerElement, "channel");
            ChannelConfiguration channel = ReadOutputWriterChannelsConfiguration(channelElement);

            // todo: this supports builtins only. Identify builtins and support ext loading for custom classes
            string writerAssembly = Assembly.GetExecutingAssembly().Location;

            return new OutputConfiguration
                       {
                           OutputWriter = writerElement.GetAttribute("class"),
                           OutputWriterAssembly = writerAssembly,
                           Channel = channel
                       };
        }

        private ChannelConfiguration ReadOutputWriterChannelsConfiguration(XmlElement channelElement)
        {
            return new ChannelConfiguration
                       {
                           Type = channelElement.GetAttribute("type"),
                           File = channelElement.GetAttribute("file")
                       };
        }

        // todo: needs test for all the absolute/relative pathing
        private List<string> GetAssemblyPaths(string configFile, XmlElement fixturesElement)
        {                
            XmlElement assemblies = GetRequiredSingleElement(fixturesElement, "assemblies");
            List<string> assemblyPaths = new List<string>();
            XmlNodeList assemblyItems = assemblies.GetElementsByTagName("assembly");
            foreach (XmlElement assembly in assemblyItems)
            {
                assemblyPaths.Add(assembly.InnerText);
            }

            string configDir = Path.GetDirectoryName(configFile);
            string fixtureAssembliesRoot = ResolveFixtureAssembliesPathRoot(assemblies, configDir);
            return assemblyPaths.ConvertAll(s => PathExtensions.EnsureIsRooted(s, fixtureAssembliesRoot));
        }

        private string ResolveFixtureAssembliesPathRoot(XmlElement assemblies, string configDir)
        {
            // root attribute is optional
            string assembliesRoot = assemblies.GetAttribute("root") ?? configDir;
            return PathExtensions.EnsureIsRooted(assembliesRoot, configDir);
        }

        private static XmlElement GetRequiredSingleElement(XmlElement parent, string name)
        {
            XmlNodeList list = parent.GetElementsByTagName(name);
            if (list.Count == 0)
            {
                throw new MonkeyPantsApplicationException(String.Format("Missing required element {0}", name));
            }
            if (list.Count > 1)
            {
                throw new MonkeyPantsApplicationException(String.Format("Multiple {0} elements, expected only one", name));
            }
            return (XmlElement)list[0];
        }
    }
}