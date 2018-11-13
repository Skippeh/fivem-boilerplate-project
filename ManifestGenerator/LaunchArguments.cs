using System.Collections.Generic;
using CommandLine;

namespace ManifestGenerator
{
    internal class LaunchArguments
    {
        [Option("resourceTemplate", Required = true, HelpText = "Specify the file to use as a template for generating the resource.lua file.")]
        public string ResourceTemplateFileName { get; set; }
        
        [Option("assemblies", Required = true, HelpText = "Specify filepats to assemblies that should be included.")]
        public IEnumerable<string> Assemblies { get; set; }
        
        [Option("outputPath", Required = false, HelpText = "If specified, the generated resource.lua file will be saved at this path. If not specified, the generated file contents will be written to stdout.")]
        public string OutputPath { get; set; }
    }
}