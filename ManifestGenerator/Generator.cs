using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ManifestTypes;
using ManifestTypes.AssemblyAttributes;

namespace ManifestGenerator
{
    internal class Generator
    {
        private readonly List<string> clientAssemblies = new List<string>();
        private readonly List<string> serverAssemblies = new List<string>();
        private readonly List<string> sharedAssemblies = new List<string>();
        
        /// <summary>
        /// Adds an assembly to be used as a resource script.
        /// </summary>
        public Generator LoadAssembly(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            var assembly = Assembly.LoadFrom(filePath);

            var assemblyType = assembly.GetCustomAttribute<AssemblyTypeAttribute>();

            if (assemblyType == null)
                throw new InvalidResourceAssemblyException("The assembly is missing the AssemblyType attribute.");

            switch (assemblyType.Type)
            {
                case AssemblyType.Client:
                    clientAssemblies.Add(fileName);
                    break;
                case AssemblyType.Server:
                    serverAssemblies.Add(fileName);
                    break;
                case AssemblyType.Shared:
                    sharedAssemblies.Add(fileName);
                    break;
                default: throw new InvalidResourceAssemblyException("The assembly's AssemblyType attribute has an unknown value.");
            }

            return this;
        }
        
        public string GenerateString(string template)
        {
            if (clientAssemblies.Count == 0 && serverAssemblies.Count == 0 && sharedAssemblies.Count == 0)
            {
                throw new InvalidOperationException("There are no assemblies added.");
            }
            
            string result = template;

            result = result.Replace("{SCRIPTS}", GenerateScriptsString());

            return result.Trim();
        }

        private string GenerateScriptsString()
        {
            StringBuilder builder = new StringBuilder();

            if (clientAssemblies.Count > 0)
            {
                builder.AppendLine($"client_scripts {{\n\t{string.Join(",\n\t", sharedAssemblies.Concat(clientAssemblies))}\n}}\n");
            }
            
            if (serverAssemblies.Count > 0)
            {
                builder.AppendLine($"server_scripts {{\n\t{string.Join(",\n\t", sharedAssemblies.Concat(serverAssemblies))}\n}}");
            }

            return builder.ToString().Trim();
        }
    }
}