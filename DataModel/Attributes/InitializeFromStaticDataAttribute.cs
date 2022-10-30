using System;

namespace DataModel.Attributes
{
    public sealed class InitializeFromStaticDataAttribute : Attribute
    {
        public string property;

        public InitializeFromStaticDataAttribute(string property)
        {
            this.property = property;
        }
    }
}
