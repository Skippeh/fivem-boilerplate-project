using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using ManifestTypes;
using ManifestTypes.AssemblyAttributes;

namespace ManifestGenerator
{
    internal class Generator
    {
        private readonly List<string> clientAssemblies = new List<string>();
        private readonly List<string> serverAssemblies = new List<string>();
        private readonly List<string> sharedAssemblies = new List<string>();
        private readonly List<string> files = new List<string>();
        private bool serverOnly = false;
        
        /// <summary>
        /// Adds an assembly to be used as a resource script.
        /// </summary>
        public Generator LoadAssembly(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            var assembly = Assembly.LoadFrom(filePath);

            var assemblyType = assembly.GetCustomAttribute<AssemblyTypeAttribute>();

            if (assemblyType == null)
                throw new InvalidResourceAssemblyException("The assembly is missing the AssemblyType attribute.");

            switch (assemblyType.Type)
            {
                case AssemblyType.Client:
                    clientAssemblies.Add(fileName);
                    serverOnly = false;
                    break;
                case AssemblyType.Server:
                    serverAssemblies.Add(fileName);
                    break;
                case AssemblyType.Shared:
                    sharedAssemblies.Add(fileName);
                    files.Add(Path.GetFileName(fileName));
                    serverOnly = false;
                    break;
                default: throw new InvalidResourceAssemblyException("The assembly's AssemblyType attribute has an unknown value.");
            }

            // Add dependant assemblies for shared/client assemblies
            if (assemblyType.Type != AssemblyType.Server)
            {
                AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();

                foreach (AssemblyName refAssembly in referencedAssemblies)
                {
                    if (refAssembly.Name == "CitizenFX.Core")
                        continue;
                    
                    string refFilePath = Path.Combine(directory, $"{refAssembly.Name}.dll");

                    if (File.Exists(refFilePath))
                    {
                        string refFileName = Path.GetFileName(refFilePath);

                        if (!files.Any(file => string.Equals(file, refFileName, StringComparison.InvariantCultureIgnoreCase)))
                            files.Add(refFileName);
                    }
                }
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
            result = result.Replace("{FILES}", GenerateFilesString());
            result = result.Replace("{SERVER_ONLY}", GenerateServerOnlyString());

            result = Regex.Replace(result, @"(?<=(?:\r?\n){2}|\A)(?:\r?\n)+", ""); // Replace multiple empty lines in a row with a single empty line.

            return result.Trim();
        }

        private string GenerateScriptsString()
        {
            StringBuilder builder = new StringBuilder();

            if (clientAssemblies.Count > 0)
            {
                builder.AppendLine($"client_scripts {{\n\t{string.Join(",\n\t", sharedAssemblies.Concat(clientAssemblies).Select(name => $"\"{name}\""))}\n}}\n");
            }
            
            if (serverAssemblies.Count > 0)
            {
                builder.AppendLine($"server_scripts {{\n\t{string.Join(",\n\t", sharedAssemblies.Concat(serverAssemblies).Select(name => $"\"{name}\""))}\n}}");
            }

            return builder.ToString().Trim();
        }

        private string GenerateFilesString()
        {
            return string.Join(",\n\t", files.Select(file => $"\"{file}\""));
        }

        private string GenerateServerOnlyString()
        {
            if (serverOnly)
                return "server_only \"yes\"";

            return string.Empty;
        }
    }
}