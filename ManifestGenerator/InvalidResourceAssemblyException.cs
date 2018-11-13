using System;

namespace ManifestGenerator
{
    internal class InvalidResourceAssemblyException : Exception
    {
        public InvalidResourceAssemblyException(string message) : base(message)
        {
        }
    }
}