using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace ManifestGenerator
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            int returnValue = 0;

            Parser.Default.ParseArguments<LaunchArguments>(args)
                .WithParsed(opts => returnValue = Run(opts))
                .WithNotParsed(errors => returnValue = OnArgsParseError(errors));

            return returnValue;
        }

        private static int Run(LaunchArguments options)
        {
            var generator = new Generator();

            foreach (var assemblyPath in options.Assemblies)
            {
                if (!File.Exists(assemblyPath))
                {
                    Console.WriteLine($"Warning: Could not find assembly file at: \"{assemblyPath}\". This may not be a problem if this is a clean build.");
                    continue;
                }

                try
                {
                    generator.LoadAssembly(assemblyPath);
                }
                catch (InvalidResourceAssemblyException ex)
                {
                    Console.Error.WriteLine("Error: " + ex.Message);
                }
            }

            string resourceFileContents = generator.GenerateString(File.ReadAllText(options.ResourceTemplateFileName));

            if (options.OutputPath == null)
            {
                Console.WriteLine(resourceFileContents);
            }
            else
            {
                File.WriteAllText(options.OutputPath, resourceFileContents);
            }

            return 0;
        }

        private static int OnArgsParseError(IEnumerable<Error> errors)
        {
            return 1;
        }
    }
}