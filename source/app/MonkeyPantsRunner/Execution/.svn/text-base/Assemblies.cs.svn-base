using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MonkeyPants.Output;
using MonkeyPants.Output.Channels;

namespace MonkeyPants.Execution
{
    public class Assemblies
    {
        private readonly List<Assembly> assemblies;
		
        public Assemblies(List<Assembly> assemblies)
        {
            this.assemblies = assemblies;
        }

        public Type ResolveType(params string[] typeNameAndAliases)
        {
            string currentName = string.Empty;
            List<Type> matches = new List<Type>();
            foreach (string name in typeNameAndAliases)
            {
                currentName = CleanName(name);
                matches = FindTypes(currentName);
                if (matches.Count > 0) break;
            }

            if (matches.Count > 1)
            {
                throw new MalformedTestException(string.Format("Cannot create test. Multiple types matching name '{0}' found", currentName));
            }

            if (matches.Count == 0)
            {
                throw new MalformedTestException(string.Format("Cannot create test. None of types '{0}' found", string.Join(" / ", typeNameAndAliases)));
            }

            return matches[0];
        }

        private string CleanName(string typeName)
        {
            typeName = typeName.Replace(" ", "");
            if (typeName.Contains("."))
            {
                typeName = typeName.Substring(typeName.LastIndexOf(".") + 1);
            }
            return typeName;
        }

        private List<Type> FindTypes(string typeName)
        {
            List<Type> matches = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                Type[] availableTypes = assembly.GetExportedTypes();
                matches.AddRange(Array.FindAll(availableTypes, type => type.Name.Equals(typeName)));
            }
            return matches;
        }

        public static object Instantiate(Type type, params object[] parameters)
        {
            try
            {
                return Activator.CreateInstance(type, parameters);
            }
            catch (Exception x)
            {
                throw new MalformedTestException(
                    string.Format("Cannot create object - cannot instantiate '{0}'", type.Name), x);
            }
        }

        public static T Instantiate<T>(string assemblyPath, string typeName, params object[] args)
        {
            Assembly assembly = LoadAssembly(assemblyPath);
            Type writerType = assembly.GetType(typeName);
            return (T)Activator.CreateInstance(writerType, args);
        }

        public static Assemblies Load(List<string> list)
        {
            return new Assemblies(list.ConvertAll(input => LoadAssembly(input)));
        }

        private static Assembly LoadAssembly(string input)
        {
            string fullPath = Path.GetFullPath(input);
            if (!File.Exists(fullPath))
            {
                throw new MonkeyPantsApplicationException(String.Format("Cannot find assembly '{0}'", fullPath));
            }
            return Assembly.LoadFile(fullPath);
        }
    }
}