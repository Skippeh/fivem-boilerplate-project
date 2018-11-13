using System;

namespace ManifestTypes.AssemblyAttributes
{
    /// <summary>
    /// Specifies what kind of resource script type this assembly is.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AssemblyTypeAttribute : Attribute
    {
        public AssemblyTypeAttribute(AssemblyType type)
        {
            Type = type;
        }
        
        public AssemblyType Type { get; set; }
    }
}