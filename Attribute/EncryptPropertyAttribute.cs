using System;

namespace EncryptProperty.Attribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class EncryptPropertyAttribute : System.Attribute
    {
    }
}
