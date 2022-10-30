using System;

namespace DataModel.Attributes
{
    public sealed class ResourceExtensionAttribute : Attribute
    {
        public string[] extensions;
        public ResourceExtensionAttribute(params string[] extensions)
        {
            this.extensions = extensions;
        }
    }
}
